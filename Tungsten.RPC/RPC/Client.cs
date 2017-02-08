using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using W.Logging;

namespace W.RPC
{
    /// <summary>
    /// Provides simple access to a Tungsten RPC Server
    /// </summary>
    public class Client : IDisposable
    {
        private EncryptedClient<Message> _client;
        private IPAddress _remoteAddress;
        private readonly Lockable<bool> _isConnected = new Lockable<bool>(false);

        /// <summary>
        /// The default timeout for Connect
        /// </summary>
        public const int DefaultConnectTimeout = 10000;
        /// <summary>
        /// The default timeout for a call to MakeRPCCall
        /// </summary>
        public const int DefaultMakeRPCCallTimeout = 30000;

        /// <summary>
        /// Delegate to notify the programmer when the Client has connected to the Server
        /// </summary>
        /// <param name="client">A reference to the Client which has connected</param>
        /// <param name="remoteAddress">The IP Address of the Server</param>
        public delegate void ConnectedDelegate(Client client, IPAddress remoteAddress);
        /// <summary>
        /// Raised when the Client has connected to the Server
        /// </summary>
        public event ConnectedDelegate Connected;
        /// <summary>
        /// Delegate to notify the programmer when the Client has disconnected from the Server
        /// </summary>
        /// <param name="client">A reference to the Client which has disconnected</param>
        /// <param name="exception">Specifies the exception which caused the disconnection.  If no exception ocurred, this value is null.</param>
        public delegate void DisconnectedDelegate(Client client, Exception exception);
        /// <summary>
        /// Raised when the Client has disconnected from the Server
        /// </summary>
        public event DisconnectedDelegate Disconnected;
        /// <summary>
        /// True if the Client is currently connected to a Tungsten RPC Server, otherwise False
        /// </summary>
        public bool IsConnected => _isConnected.Value;

        /// <summary>
        /// Calls a method on the Tungsten RPC Server
        /// </summary>
        /// <param name="methodName">The name of the method to call, including full namespace and class name</param>
        /// <param name="onResponse">A callback where </param>
        /// <typeparam name="T">The result from the call</typeparam>
        /// <returns>A ManualResetEvent which can be joined (with or without a timeout) to block the calling thread until a respoonse is received.</returns>
        public ManualResetEvent MakeRPCCall<T>(string methodName, Action<T> onResponse)
        {
            return MakeRPCCall(methodName, onResponse, null);
        }
        /// <summary>
        /// Calls a method on the Tungsten RPC Server
        /// </summary>
        /// <param name="methodName">The name of the method to call, including full namespace and class name</param>
        /// <param name="onResponse">A callback where </param>
        /// <param name="args">Optional parameters to be passed into the method</param>
        /// <typeparam name="T">The result from the call</typeparam>
        /// <returns>A ManualResetEvent which can be joined (with or without a timeout) to block the calling thread until a respoonse is received.</returns>
        public ManualResetEvent MakeRPCCall<T>(string methodName, Action<T> onResponse, params object[] args)
        {
            var msg = new Message();
            var mre = new ManualResetEvent(false);
            msg.Method = methodName;
            if (args != null)
                msg.Parameters.AddRange(args);

            _client.Post(msg, (client, response, isExpired) =>
            {
                var result = default(T);
                if (response?.Response != null)
                {
                    var token = response.Response as JToken;
                    if (token != null)
                        result = token.ToObject<T>();
                    else if (response.Response is T)
                        result = (T)response.Response;
                }
                Task.Factory.FromAsync((asyncCallback, @object) =>
                {
                    return onResponse.BeginInvoke(result, asyncCallback, @object);
                }, (ar) =>
                {
                    onResponse.EndInvoke(ar);
                    mre.Set();
                }
                , null);
                //onResponse?.Invoke(result);
                //mre.Set(); //this needs to happen after the call to onResponse
            }, DefaultMakeRPCCallTimeout);
            return mre;
        }
        /// <summary>
        /// Not sure I should keep this method.  Shouldn't all RPC calls have a result?  Otherwise, the client wouldn't know if it succeeded.
        /// </summary>
        /// <param name="onResponse"></param>
        /// <param name="methodName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public ManualResetEvent MakeRPCCall(string methodName, Action onResponse, params object[] args)
        {
            var msg = new Message();
            var mre = new ManualResetEvent(false);
            msg.Method = methodName;
            if (args != null)
                msg.Parameters.AddRange(args);

            _client.Post(msg, (client, response, isExpired) =>
            {
                Task.Factory.FromAsync((asyncCallback, @object) =>
                {
                    return onResponse.BeginInvoke(asyncCallback, @object);
                }, (ar) =>
                {
                    onResponse.EndInvoke(ar);
                    mre.Set();
                }
                , null);
                //onResponse?.Invoke();
                //mre.Set(); //this needs to happen after the call to onResponse
            }, DefaultMakeRPCCallTimeout);
            return mre;
        }

        internal void Disconnect(Exception e)
        {
            _client.Disconnect(e);
        }
        internal CallResult Connect(System.Net.Sockets.TcpClient client) //called by Server
        {
            if (_isConnected.Value)
                return new CallResult(true);

            var result = _client.Connect(client);
            if (result.Success)
            {
                if (!_client.IsSecure.WaitForChanged(DefaultConnectTimeout)) //default timeout
                {
                    Log.w("Connection timed out while securing");
                    Disconnect(new Exception("Server failed to secure the connection"));
                }
                else
                    Log.i("Client Connected");
            }
            else
                Disconnect(new Exception("Server failed to respond"));
            return result;
        }
        /// <summary>
        /// Attempts to connect to a local or remote Tungsten RPC Server
        /// </summary>
        /// <param name="remoteAddress">The IP address of the Tungsten RPC Server</param>
        /// <param name="remotePort">The port on which the Tungsten RPC Server is listening</param>
        /// <param name="msTimeout">The call will fail if the client can't connect within the specified elapsed time (in milliseconds)</param>
        /// <returns>A CallResult specifying success/failure and an Exception if one ocurred</returns>
        public CallResult Connect(string remoteAddress, int remotePort, int msTimeout = DefaultConnectTimeout, Action<Client, IPAddress> onConnection = null)
        {
            if (_isConnected.Value)
                return new CallResult(true);

            var result = _client.Connect(remoteAddress, remotePort, msTimeout);
            if (result.Success)
            {
                if (!_client.IsSecure.WaitForChanged(msTimeout))
                {
                    Log.w("Connection timed out while securing");
                    Disconnect(new Exception("Server failed to secure the connection"));
                }
                else
                    Log.i("Client Connected");
                if (onConnection != null)
                    Task.Factory.FromAsync((asyncCallback, @object) => onConnection.BeginInvoke(this, IPAddress.Parse(remoteAddress), asyncCallback, @object), onConnection.EndInvoke, null);
            }
            else
                Disconnect(new Exception("Server failed to respond"));
            return result;
        }
        /// <summary>
        /// Attempts to connect to a local or remote Tungsten RPC Server
        /// </summary>
        /// <param name="remoteAddress">The IP address of the Tungsten RPC Server</param>
        /// <param name="remotePort">The port on which the Tungsten RPC Server is listening</param>
        /// <param name="msTimeout">The call will fail if the client can't connect within the specified elapsed time (in milliseconds)</param>
        /// <returns>A CallResult specifying success/failure and an Exception if one ocurred</returns>
        public CallResult Connect(IPAddress remoteAddress, int remotePort, int msTimeout = DefaultConnectTimeout, Action<Client, IPAddress> onConnection = null)
        {
            if (_isConnected.Value)
                return new CallResult(true);
            return Connect(remoteAddress.ToString(), remotePort, msTimeout, onConnection);
        }
        /// <summary>
        /// Attempts to connect to a local or remote Tungsten RPC Server asynchronously
        /// </summary>
        /// <param name="remoteAddress">The IP address of the Tungsten RPC Server</param>
        /// <param name="remotePort">The port on which the Tungsten RPC Server is listening</param>
        /// <param name="msTimeout">The call will fail if the client can't connect within the specified elapsed time (in milliseconds)</param>
        /// <returns>A Task which can be awaited</returns>
        public async Task ConnectAsync(string remoteAddress, int remotePort, int msTimeout = DefaultConnectTimeout)
        {
            if (_isConnected.Value)
                return;
            await _client.ConnectAsync(remoteAddress, remotePort, msTimeout).ContinueWith(f => _client.IsSecure.WaitForChanged(msTimeout));
        }
        /// <summary>
        /// Attempts to connect to a local or remote Tungsten RPC Server asynchronously
        /// </summary>
        /// <param name="remoteAddress">The IP address of the Tungsten RPC Server</param>
        /// <param name="remotePort">The port on which the Tungsten RPC Server is listening</param>
        /// <param name="msTimeout">The call will fail if the client can't connect within the specified elapsed time (in milliseconds)</param>
        /// <returns>A Task which can be awaited</returns>
        public async Task ConnectAsync(IPAddress remoteAddress, int remotePort, int msTimeout = DefaultConnectTimeout)
        {
            if (_isConnected.Value)
                return;
            await _client.ConnectAsync(remoteAddress, remotePort, msTimeout).ContinueWith(f => _client.IsSecure.WaitForChanged(msTimeout));
        }
        /// <summary>
        /// Disconnects from the Server
        /// </summary>
        public void Disconnect()
        {
            Disconnect(null);
        }

        /// <summary>
        /// Constructs a new Tungsten RPC Client
        /// </summary>
        public Client()
        {
            _client = new EncryptedClient<Message>();
            _client.Connected += (client, address) =>
            {
                //don't care when we connect, we care when we're secured
                _remoteAddress = address;
            };
            _client.Secured += client =>
            {
                _isConnected.Value = true;
                Task.Run(() =>
                {
                    var evt = this.Connected;
                    //evt?.Invoke(this, _remoteAddress);
                    if (evt != null)
                        Task.Factory.FromAsync((asyncCallback, @object) => evt.BeginInvoke(this, _remoteAddress, asyncCallback, @object), evt.EndInvoke, null);
                    //Task.Factory.FromAsync taken from:
                    // http://stackoverflow.com/questions/1916095/how-do-i-make-an-eventhandler-run-asynchronously
                });
            };
            _client.Disconnected += (client, address, exception) =>
            {
                _isConnected.Value = false;

                Log.i("Client Disconnected: {0}", client.Name);
                if (exception != null)
                    Log.i("Disconnect Reason: {0}", exception.Message);
                var evt = this.Disconnected;
                //evt?.Invoke(this, exception);
                if (evt != null)
                    Task.Factory.FromAsync((asyncCallback, @object) => evt.BeginInvoke(this, exception, asyncCallback, @object), evt.EndInvoke, null);
                //Task.Factory.FromAsync taken from:
                // http://stackoverflow.com/questions/1916095/how-do-i-make-an-eventhandler-run-asynchronously
            };
        }
        /// <summary>
        /// Constructs a new Tungsten RPC Client and automatically connects to the specified remote server
        /// </summary>
        /// <param name="remoteAddress">The IP address of the Tungsten RPC Server</param>
        /// <param name="remotePort">The port on which the Tungsten RPC Server is listening</param>
        /// <param name="msTimeout">The call will fail if the client can't connect within the specified elapsed time (in milliseconds)</param>
        public Client(string remoteAddress, int remotePort, int msTimeout = DefaultConnectTimeout) : this()
        {
            var mre = new ManualResetEvent(false);
            W.Threading.Thread.Create(cts =>
            //Task.Run(() =>
            {
                Connect(remoteAddress, remotePort, msTimeout, (client, address) =>
                {
                    mre.Set();
                });
                //Connected += (client, address) => { mre.Set(); };
            });
            if (!mre.WaitOne(DefaultConnectTimeout))
                throw new TimeoutException("Unable to connect to the remote Tungsten RPC Server: " + remoteAddress);
        }
        /// <summary>
        /// Constructs a new Tungsten RPC Client and automatically connects to the specified remote server
        /// </summary>
        /// <param name="remoteAddress">The IP address of the Tungsten RPC Server</param>
        /// <param name="remotePort">The port on which the Tungsten RPC Server is listening</param>
        /// <param name="msTimeout">The call will fail if the client can't connect within the specified elapsed time (in milliseconds)</param>
        public Client(IPAddress remoteAddress, int remotePort, int msTimeout = DefaultConnectTimeout) : this()
        {
            var mre = new ManualResetEvent(false);
            W.Threading.Thread.Create(cts =>
            //Task.Run(() =>
            {
                Connect(remoteAddress, remotePort, msTimeout, (client, address) =>
                {
                    mre.Set();
                });
                //Connected += (client, address) => { mre.Set(); };
            });
            if (!mre.WaitOne(DefaultConnectTimeout))
                throw new TimeoutException("Unable to connect to the remote Tungsten RPC Server: " + remoteAddress);
        }

        /// <summary>
        /// Destructs the Tungsten RPC Client.  Calls Dispose.
        /// </summary>
        ~Client()
        {
            Dispose();
        }
        /// <summary>Disconnects from a Tungsten RPC Server if connected.  Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            Disconnect();
        }
    }
}