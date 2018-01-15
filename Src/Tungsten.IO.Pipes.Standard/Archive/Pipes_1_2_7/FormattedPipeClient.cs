﻿using System;
using System.IO.Pipes;
using W;

namespace W.IO.Pipes
{
    /// <summary>
    /// Base class for PipeClient.  When inherited, it supports customized formatting before sending and after receiving data.
    /// </summary>
    /// <remarks>Override the FormatReceivedMessage and FormatMessageToSend functions to customize the formatting</remarks>
    public class FormattedPipeClient<TDataType> : PipeTransceiver<TDataType>, IPipeClient where TDataType : class /* Initialize and Dispose are implemented by PipeTransmitter */
    {
        private Exception _connectionException = null;
        internal Lockable<bool> IsConnected { get; } = new Lockable<bool>(false);

        /// <summary>
        /// Called when the client connects to the server
        /// </summary>
        public Action<object> Connected { get; set; }
        /// <summary>
        /// Called when the client disconnects from the server
        /// </summary>
        public Action<object, Exception> Disconnected { get; set; }

        private bool TryConnect(string serverName, string pipeName, PipeDirection direction)//, Action<FormattedPipeClient<TDataType>, Exception> onException = null)
        {
            var result = false;
            try
            {
                var client = new NamedPipeClientStream(serverName, pipeName, direction, PipeOptions.Asynchronous);
                client.Connect(1000);
                client.ReadMode = System.IO.Pipes.PipeTransmissionMode.Byte; //it appears this must be done immediately after connecting
                Stream.Value = client;
                IsConnected.Value = true;
                result = true;
            }
            catch (TimeoutException e)
            {
                //Could not connect to the server within the specified timeout period
                //onException?.Invoke(this, e);
                _connectionException = e;
            }
            catch (ArgumentOutOfRangeException e)
            {
                //Timeout is less than 0 and not set to Infinit
                //onException?.Invoke(this, e);
                _connectionException = e;
            }
            catch (System.IO.IOException e)
            {
                //The server is connected to another client and the timeout period has expired
                //onException?.Invoke(this, e);
                _connectionException = e;
            }
            //catch (Exception e)
            //{
            //    //plan for the unexpected
            //    onException?.Invoke(this, e);
            //}
            return result;
        }

        /// <summary>
        /// Attempts to connect to the local named pipe server
        /// </summary>
        /// <returns>True if a connection was established, otherwise false</returns>
        public bool Connect(string pipeName, PipeDirection pipeDirection = PipeDirection.InOut)
        {
            return Connect(".", pipeName, pipeDirection);
        }
        /// <summary>
        /// Attempts to connect to a local or remote named pipe server
        /// </summary>
        /// <returns>True if a connection was established, otherwise false</returns>
        public bool Connect(string serverName, string pipeName, PipeDirection pipeDirection = PipeDirection.InOut)
        {
            if (!TryConnect(serverName, pipeName, pipeDirection))//, (o, e) =>
            {
                Disconnected?.Invoke(this, _connectionException);
                _connectionException = null;
                return false;
            }//);
            if (Stream.Value?.IsConnected ?? false)
            {
                Initialize(Stream.Value, false);
                Connected?.Invoke(this);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        protected override void OnDispose()
        {
            System.Diagnostics.Debug.WriteLine("Disposing IsServerSide={0}", IsServerSide);
            Stream.Value?.Flush();
            var s = Stream.Value as NamedPipeServerStream;
            if (s != null && s.IsConnected)
                s.Disconnect(); //I think this needs to execute before the base disposes

            base.OnDispose();
#if !NETSTANDARD1_4
            s?.Close();
#endif
            s?.Dispose();
            if (Stream.Value is NamedPipeClientStream c)
                c.Dispose();
            Stream.Value = null;
            if (IsConnected.Value)
                OnDisconnected();
        }
        /// <summary>
        /// Override to handle a disconnect
        /// </summary>
        /// <param name="e">The exception, if one occurred</param>
        protected override void OnDisconnected(Exception e = null)
        {
            IsConnected.Value = false;
            Disconnected?.Invoke(this, e);
        }

        /// <summary>
        /// Constructs a new GenericPipe
        /// </summary>
        /// <remarks>Used when creating a client and calling Connect</remarks>
        public FormattedPipeClient() { }
        /// <summary>
        /// Disposes the Pipe
        /// </summary>
        ~FormattedPipeClient()
        {
            Dispose();
        }
    }
}
