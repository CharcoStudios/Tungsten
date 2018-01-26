﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using W.AsExtensions;
using W;
using W.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace W.Tests
{
    [TestClass]
    public class TcpTests
    {
        //private ITestOutputHelper output;
        //public NetTests(ITestOutputHelper output)
        //{
        //    this.output = output;
        //}

        //[TestMethod]
        //public void ClientServer_WithCompression()
        //{
        //    var mreContinue = new System.Threading.ManualResetEventSlim(false);
        //    var hostEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5150);
        //    var latin = System.IO.File.ReadAllText(@"C:\Source\Repos\Tungsten\Test\Test Files\4500_Latin_Words.txt");
        //    var latinBytes = latin.AsBytes();
        //    using (var host = new Tcp.TcpHost())
        //    {
        //        //host.ServerConnected += (h, s) =>
        //        //{
        //        //    Console.WriteLine("Server: Client Connected = {0}", s.Socket.RemoteEndPoint.ToString());
        //        //};
        //        //host.ServerDisconnected += (h, s) =>
        //        //{
        //        //    Console.WriteLine("Server: Client Disconnected = {0}", s.Socket.RemoteEndPoint.ToString());
        //        //};
        //        host.BytesReceived.OnRaised += (h, s, bytes) =>
        //        {
        //            Console.WriteLine("Server Echoing {0} bytes", bytes.Length);
        //            //echo
        //            s.Write(bytes);
        //        };
        //        //host.UseCompression = true;
        //        host.Listen(hostEndPoint, 20);

        //        using (var client = new Tcp.TcpClient())
        //        {
        //            client.BytesReceived.OnRaised += (c, bytes) =>
        //            {
        //                Console.WriteLine("Client Received {0} bytes", bytes.Length);
        //                Assert.IsTrue(bytes.SequenceEqual(latinBytes));
        //                mreContinue.Set();
        //            };
        //            //client.UseCompression = true;
        //            client.Connect(hostEndPoint);
        //            client.Write(latinBytes);
        //            Assert.IsTrue(mreContinue.Wait(5000));
        //        }
        //    }
        //}
        //[TestMethod]
        //public void SecureStringClientLogger()
        //{
        //    var received = 0;
        //    var numberOfMessagesToSend = 10;
        //    var mreQuit = new System.Threading.ManualResetEventSlim(false);


        //    IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2213);
        //    using (var server = new W.Net.Server<W.Net.SecureClient>())
        //    {
        //        server.ClientConnected += client =>
        //        {
        //            client.DataReceived += (c, m) =>
        //            {
        //                received += 1;
        //                Console.WriteLine("Received Message " + received.ToString() + ": " + m.AsString());
        //                if (received == numberOfMessagesToSend)
        //                    mreQuit.Set();
        //            };
        //        };
        //        server.Start(ipEndPoint.Address, ipEndPoint.Port);
        //        Console.WriteLine("Server Started");

        //        //To verify this method, an external server must be listening
        //        using (var logger = new W.Net.SecureStringClientLogger(ipEndPoint))
        //        {
        //            var r = new Random();
        //            for(int t=1; t<=numberOfMessagesToSend; t++)
        //            {
        //                var msg = "Test Log Message: " + t.ToString();
        //                switch (r.Next(0, 4))
        //                {
        //                    case 0:
        //                        W.Logging.Log.e(msg);
        //                        break;
        //                    case 1:
        //                        W.Logging.Log.w(msg);
        //                        break;
        //                    case 2:
        //                        W.Logging.Log.i(msg);
        //                        break;
        //                    case 3:
        //                        W.Logging.Log.v(msg);
        //                        break;
        //                }
        //            }
        //            Assert.IsTrue(mreQuit.Wait(20000), "Failed To Receive All Messages");
        //            Console.WriteLine("Completed Logging");
        //        }
        //        Console.WriteLine("Complete");
        //        Assert.IsTrue(true);
        //    }
        //}

        [TestMethod]
        public void TcpLogger()
        {
            var received = 0;
            var numberOfMessagesToSend = 10;
            var mreQuit = new System.Threading.ManualResetEventSlim(false);

            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2213);
            using (var server = new Tcp.TcpHost())
            {
                server.BytesReceived.OnRaised += (h, s, b) =>
                {
                    received += 1;
                    Console.WriteLine("Received Message " + received.ToString() + ": " + b.AsString());
                    if (received == numberOfMessagesToSend)
                        mreQuit.Set();
                };
                server.Listen(ipEndPoint, 20);
                Console.WriteLine("Server Started");

                //To verify this method, an external server must be listening
                using (var logger = new Tcp.TcpLogger(ipEndPoint, true))
                {
                    var r = new Random();
                    for (int t = 1; t <= numberOfMessagesToSend; t++)
                    {
                        var msg = "Test Log Message: " + t.ToString();
                        switch (r.Next(0, 4))
                        {
                            case 0:
                                W.Logging.Log.e(msg);
                                break;
                            case 1:
                                W.Logging.Log.w(msg);
                                break;
                            case 2:
                                W.Logging.Log.i(msg);
                                break;
                            case 3:
                                W.Logging.Log.v(msg);
                                break;
                        }
                    }
                    Assert.IsTrue(mreQuit.Wait(20000), "Failed To Receive All Messages");
                    Console.WriteLine("Completed Logging");
                }
                Console.WriteLine("Complete");
            }
        }
        [TestMethod]
        public void CreateClient()
        {
            var client = new Tcp.TcpClient();
            client.Connected.OnRaised += c => { };
            client.Disconnected.OnRaised += c => { };
            client.BytesReceived.OnRaised += (c, b) => { };
            client.Dispose();
        }
        [TestMethod]
        public void CreateServer()
        {
            var server = new Tcp.TcpHost();
            server.BytesReceived.OnRaised += (h, s, b) => { };
            server.Dispose();
        }
        [TestMethod]
        public void EchoOnce()
        {
            var mreContinue = new System.Threading.ManualResetEventSlim(false);
            var hostEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5150);
            var latin = System.IO.File.ReadAllText(@"C:\Source\Repos\Tungsten\Test\Test Files\4500_Latin_Words.txt");
            var latinBytes = latin.AsBytes();
            using (var host = new Tcp.TcpHost())
            {
                host.BytesReceived.OnRaised += (h, s, bytes) =>
                {
                    Console.WriteLine("Server Echoing {0} bytes", bytes.Length);
                    //echo
                    s.Write(bytes);
                };

                host.Listen(hostEndPoint, 20);

                using (var client = new Tcp.TcpClient())
                {
                    client.BytesReceived.OnRaised += (c, bytes) =>
                    {
                        Console.WriteLine("Client Received {0} bytes", bytes.Length);
                        Assert.IsTrue(bytes.SequenceEqual(latinBytes));
                        mreContinue.Set();
                    };
                    client.Connect(hostEndPoint);
                    client.Write(latinBytes);
                    Assert.IsTrue(mreContinue.Wait(1000));
                }
            }
        }
        [TestMethod]
        public void EchoLatin()
        {
            var mreContinue = new System.Threading.ManualResetEventSlim(false);
            var hostEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5150);
            var latin = System.IO.File.ReadAllText(@"C:\Source\Repos\Tungsten\Test\Test Files\4500_Latin_Words.txt");
            var latinBytes = latin.AsBytes();
            using (var host = new Tcp.TcpHost())
            {
                //host.ServerConnected += (h, s) =>
                //{
                //    Console.WriteLine("Server: Client Connected = {0}", s.Socket.RemoteEndPoint.ToString());
                //};
                //host.ServerDisconnected += (h, s) =>
                //{
                //    Console.WriteLine("Server: Client Disconnected = {0}", s.Socket.RemoteEndPoint.ToString());
                //};
                host.BytesReceived.OnRaised += (h, s, bytes) =>
                {
                    Console.WriteLine("Server Echoing {0} bytes", bytes.Length);
                    //echo
                    s.Write(bytes);
                };

                host.Listen(hostEndPoint, 20);

                using (var client = new Tcp.TcpClient())
                {
                    client.BytesReceived.OnRaised += (c, bytes) =>
                    {
                        Console.WriteLine("Client Received {0} bytes", bytes.Length);
                        Assert.IsTrue(bytes.SequenceEqual(latinBytes));
                        mreContinue.Set();
                    };
                    client.Connect(hostEndPoint);
                    client.Write(latinBytes);
                    Assert.IsTrue(mreContinue.Wait(1000));
                }
            }
        }
    }
}
