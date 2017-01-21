﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using W.Logging;
using Newtonsoft.Json.Linq;

namespace W.RPC
{
    [RPCClass] //use the RPCClass attribute to declare that this class has methods which can be called via client connections
    internal class Sample_Use
    {
        [RPCMethod] // use the RPCMethod attribute to expose functionality via RPC
        public static void LogInformation(string format, params object[] args)
        {
            Log.i(format, args);
        }
    }

    /// <summary>
    /// Hosts an RPC instance
    /// </summary>
    public class Server
    {
        private EncryptedServer<EncryptedClient<Message>> _host;
        private Dictionary<string, MethodInfo> _methods = new Dictionary<string, MethodInfo>();

        private void FindAllRPCMethods()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var srcPath = System.IO.Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);

            _methods.Clear();
            foreach (var asm in assemblies)
            {
                if (asm.IsDynamic)
                    continue;
                var path = System.IO.Path.GetDirectoryName(asm.Location);
                if (!path.StartsWith(srcPath))
                    continue;
                foreach (var t in asm.GetExportedTypes())
                {
                    foreach (var a in t.GetCustomAttributes())
                    {
                        if (a is RPCClassAttribute)
                        {
                            foreach (var mi in t.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy))
                            {
                                foreach (var a2 in mi.GetCustomAttributes())
                                {
                                    if (a2 is RPCMethodAttribute)
                                    {
                                        var key = t.Namespace + "." + t.Name + "." + mi.Name;
                                        if (!_methods.ContainsKey(key))
                                            _methods.Add(key, mi);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            Console.WriteLine($"Found {_methods.Count} RPC Methods");
        }
        private CallResult<object> FindAndCallMethod(string method, params object[] args)
        {
            var result = new CallResult<object>(true, null);
            MethodInfo mi = null;
            if (_methods.ContainsKey(method))
                mi = _methods[method];

            try
            {
                //result.Result = mi?.Invoke(null, BindingFlags.Static | BindingFlags.Public, null, args, CultureInfo.CurrentCulture);
                if (args != null)
                {
                    var parameters = mi?.GetParameters();
                    if (parameters != null)
                    {
                        if (parameters.Length != args.Length)
                            throw new Exception("Wrong number of arguments");
                        for (int t = 0; t < args.Length; t++)
                        {
                            if (args[t] is JToken)
                                args[t] = ((JToken)args[t]).ToObject(parameters[t].ParameterType);
                            //args[t] = Newtonsoft.Json.JsonConvert.DeserializeObject((string)args[t], parameters[t].ParameterType);
                        }
                    }
                }

                //for (int t = 0; t < args.Length; t++)
                //{
                //    if (args[t] is JArray)
                //        args[t] = ((JArray)args[t]).ToArray();
                //}

                if (mi != null)
                {
                    if (mi.ReturnType.FullName == "System.Void")
                        mi?.Invoke(null, args);
                    else
                        //TODO: 12/4/2016
                        //invoking this method isn't working because the actual User/Address/whatever class isn't being deserialized from JSON
                        //instead it's leaving it as JSON (as JArray or JObject or the like)
                        //figure out how to deserialize the objects as objects rather than json strings
                        result.Result = mi?.Invoke(null, args);
                }
            }
            catch (Exception e)
            {
                result.Success = false;
                result.Exception = e;
                Log.e(e);
            }
            return result;
        }

        private bool OnMessageArrived(ref Message message)
        {
            var result = FindAndCallMethod(message.Method, message.Parameters.ToArray());
            //var result = FindAndCallMethod(message.Method, message.Arguments);
            message.Exception = result.Exception;
            message.Response = result.Result;
            return result.Success;
        }

        /// <summary>
        /// Starts listening for client connections
        /// </summary>
        /// <remarks><para>This method will use reflection to inspect all loaded dlls for classes supporting the RPCClass and RPCMethod attributes</para></remarks>
        /// <param name="ipAddress"></param>
        /// <param name="port"></param>
        public void Start(IPAddress ipAddress, int port)
        {
            Stop();
            FindAllRPCMethods();
            _host = new EncryptedServer<EncryptedClient<Message>>();
            _host.Start(ipAddress, port);
            _host.ClientConnected += (sender, client) =>
            {
                Log.i("Client Connected: {0}", client.Name);
                client.Disconnected += (c2, address, exception) =>
                {
                    Log.i("Client Disconnected: {0}", address?.ToString());
                };
                client.MessageArrived += (c3, message) =>
                {
                    try
                    {
                        if (!c3.IsSecure.Value) //ignore any messages that are sent before being secured
                                {
                            Log.w("Message arrived at server before client was secure");
                            return;
                        }
                        OnMessageArrived(ref message);
                        try
                        {
                            Task.Run(() =>
                            {
                                c3.Post(message);
                            });
                        }
                        catch (Exception e)
                        {
                            Log.e(e);
                        }
                    }
                    catch (Exception e)
                    {
                        Log.e(e);
                    }
                };
            };
        }
        /// <summary>
        /// Stops listening for client connections
        /// </summary>
        public void Stop()
        {
            _host?.Stop();
        }
    }

}
