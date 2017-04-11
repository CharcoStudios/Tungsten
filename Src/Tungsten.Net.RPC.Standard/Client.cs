﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json.Linq;
using W.Logging;

namespace W.Net.RPC
{
    /// <summary>
    /// Supports calling methods exposed by a Tungsten.Net.RPC Server
    /// </summary>
    public partial class Client : IDisposable
    {
        private GenericClient<Message> _client;
        private delegate void MessageArrivedCallback(Client clientClient, Message response, bool expired);
        private readonly List<Waiter> _waiters = new List<Waiter>();
        private Lockable<bool> _isConnected { get; } = new Lockable<bool>();

        //public delegate void HandleResponseDelegate<TResponseType>(TResponseType response, bool expired);
        /// <summary>
        /// Multi-cast delegate called when the client connects to the server
        /// </summary>
        public Action<Client> Connected { get; set; }
        /// <summary>
        /// Multi-cast delegate called when the client disconnects from the server
        /// </summary>
        public Action<Client, Exception> Disconnected { get; set; }

        /// <summary>
        /// True if the client is connected to the server, otherwise False
        /// </summary>
        public bool IsConnected => _isConnected.Value;
        
        /// <summary>
        /// Calls Dispose and deconstructs the Client
        /// </summary>
        ~Client()
        {
            Dispose();
        }
        /// <summary>
        /// Calls a method on the Tungsten RPC Server
        /// </summary>
        /// <param name="methodName">The name of the method to call, including full namespace and class name</param>
        public void Call(string methodName)
        {
            Call<object>(methodName, null, null);
        }
        /// <summary>
        /// Calls a method on the Tungsten RPC Server
        /// </summary>
        /// <param name="methodName">The name of the method to call, including full namespace and class name</param>
        /// <param name="args">Arguments, if any, to be passed into the remote method</param>
        public void Call(string methodName, params object[] args)
        {
            Call<object>(methodName, null, args);
        }
        /// <summary>
        /// Calls a method on the Tungsten RPC Server
        /// </summary>
        /// <param name="methodName">The name of the method to call, including full namespace and class name</param>
        /// <param name="onResponse">Called when a response to has been received.</param>
        /// <typeparam name="TResponseType">The result from the call</typeparam>
        /// <returns>A ManualResetEvent which can be joined (with or without a timeout) to block the calling thread until a respoonse is received.</returns>
        /// <remarks>The return value will be null if the onResponse parameter is null</remarks>
        public ManualResetEvent Call<TResponseType>(string methodName, Action<TResponseType, bool> onResponse)
        {
            return Call<TResponseType>(methodName, onResponse, null);
        }
        /// <summary>
        /// Calls a method on the Tungsten RPC Server
        /// </summary>
        /// <param name="methodName">The name of the method to call, including full namespace, class name, and method name</param>
        /// <param name="onResponse">Called when a response to has been received.</param>
        /// <param name="args">Optional parameters to be passed into the method</param>
        /// <typeparam name="TResponseType">The result from the call</typeparam>
        /// <returns>A ManualResetEvent which can be joined (with or without a timeout) to block the calling thread until a respoonse is received.</returns>
        /// <remarks>The return value will be null if the onResponse parameter is null</remarks>
        public ManualResetEvent Call<TResponseType>(string methodName, Action<TResponseType, bool> onResponse, params object[] args)
        {
            if (_client == null)
                throw new IOException("Socket is not connected");
            var msg = new Message();
            ManualResetEvent mre = null;
            msg.Method = methodName;
            if (args != null)
                msg.Parameters.AddRange(args);

            if (msg.Id == Guid.Empty)
                msg.Id = Guid.NewGuid();
            msg.ExpireDateTime = DateTime.Now.AddMilliseconds(10000);

            if (onResponse != null)
            {
                mre = new ManualResetEvent(false);
                var waiter = new Waiter(this, _client, 10000);
                waiter.Message = msg;
                waiter.Callback = (client, response, isExpired) =>
                {
                    var result = default(TResponseType);
                    if (response?.Response != null)
                    {
                        var token = response.Response as JToken;
                        //handle object deserialization
                        if (token != null)
                            result = token.ToObject<TResponseType>();
                        //handle exact type match deserialization
                        else if (response.Response is TResponseType)
                            result = (TResponseType)response.Response;
                        //Newtonsoft always converts Int32 to Int64 when serializing (so we have to handle this case)
                        else if (response.Response is Int64 && typeof(TResponseType) == typeof(Int32))
                            result = (TResponseType)Convert.ChangeType(response.Response, typeof(TResponseType));
                    }
                    //if (onResponse != null)
                    //{
                    //Task.Run(() => { onResponse?.Invoke(result, isExpired); });

                    //Task.Factory.FromAsync((asyncCallback, @object) =>
                    //{
                    //    return onResponse.BeginInvoke(result, isExpired, asyncCallback, @object);
                    //}, (ar) =>
                    //{
                    //    onResponse.EndInvoke(ar);
                    //    mre.Set();
                    //}
                    //, null);
                    //}
                    //else
                    onResponse?.Invoke(result, isExpired);
                    mre.Set();
                };
                waiter.Completed += (w, success) =>
                {
                    w.Dispose();
                    _waiters.Remove(w);
                };
                _waiters.Add(waiter);
            }
            _client.Send(msg);
            return mre;
        }

        /// <summary>
        /// Attempts to connect to a local or remote Tungsten RPC Server
        /// </summary>
        /// <param name="remoteAddress">The IP address of the Tungsten RPC Server</param>
        /// <param name="remotePort">The port on which the Tungsten RPC Server is listening</param>
        /// <param name="msTimeout">The call will fail if the client can't connect within the specified elapsed time (in milliseconds)</param>
        /// <param name="onConnection">Called when a connection has been established</param>
        /// <returns>A CallResult specifying success/failure and an Exception if one ocurred</returns>
        public bool Connect(string remoteAddress, int remotePort, int msTimeout = 10000, Action<Client, IPAddress> onConnection = null)//, Action<Exception> onException = null)
        {
            if (_isConnected.Value)
                return true;
            var mre = new ManualResetEvent(false);
            _client = new GenericClient<Message>();
            _client.ConnectionSecured += o =>
            {
                //if (onConnection != null)
                //    Task.Factory.FromAsync((asyncCallback, @object) => onConnection.BeginInvoke(this, IPAddress.Parse(remoteAddress), asyncCallback, @object), onConnection.EndInvoke, null);
                try
                { 
                    onConnection?.Invoke(this, IPAddress.Parse(remoteAddress));
                }
                catch (Exception e)
                {
                    Log.e(e);
                }
                try
                {
                    Connected?.Invoke(this);
                }
                catch (Exception e)
                {
                    Log.e(e);
                }
                mre?.Set();
            };
            //3.28.2017 - now each Waiter taps the GenericMessageReceived to listen for their own message (identified by Message.Id)

            var isConnected = _client.Socket.Connect(remoteAddress, remotePort).Wait(msTimeout);
            if (!isConnected || (!mre?.WaitOne(msTimeout) ?? false)) //wait for secured
                Disconnect(new Exception("Server failed to respond"));
            mre.Dispose();
            mre = null;
            return isConnected;
        }
        /// <summary>
        /// Attempts to connect to a local or remote Tungsten RPC Server
        /// </summary>
        /// <param name="remoteAddress">The IP address of the Tungsten RPC Server</param>
        /// <param name="remotePort">The port on which the Tungsten RPC Server is listening</param>
        /// <param name="msTimeout">The call will fail if the client can't connect within the specified elapsed time (in milliseconds)</param>
        /// <param name="onConnection">Called when a connection has been established</param>
        /// <returns>A CallResult specifying success/failure and an Exception if one ocurred</returns>
        public bool Connect(IPAddress remoteAddress, int remotePort, int msTimeout = 10000, Action<Client, IPAddress> onConnection = null)//, Action<Exception> onException = null)
        {
            return Connect(remoteAddress.ToString(), remotePort, msTimeout, onConnection);//, onException);
        }
        internal void Disconnect(Exception e)
        {
            //attempt to wait for completion or timeouts
            Task.Run(() =>
            {
                while (_waiters?.Count > 0)
                {
                    _waiters[0].Cancel();
                    System.Threading.Thread.Sleep(5);
                }
            }).Wait(10000);
            _waiters?.Clear();
            _client?.Socket.Disconnect(e);
            _client = null;
            try
            { 
                Disconnected?.Invoke(this, e);
            }
            catch (Exception ex)
            {
                Log.e(ex);
            }
        }
        /// <summary>
        /// Disconnects the socket from the server
        /// </summary>
        public void Disconnect()
        {
            Disconnect(null);
        }
        /// <summary>
        /// Disposes the Client and releases resources.  Disconnects if necessary.
        /// </summary>
        public void Dispose()
        {
            Disconnect();
            GC.SuppressFinalize(this);
        }
    }
}
