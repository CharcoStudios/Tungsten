﻿Tungsten.Net.RPC is a client/server solution for invoking methods on a server.  Communications are made over the port you specify.  RPC methods on the server are created by attributing classes and methods with the [RPCClass] and [RPCMethod] attributes.  Clients make calls by passing in the method name ("MyNameSpace.MyClass.Method1") and any parameters to one of the Call or CallAsync methods.

4.28.2017 - v1.2.5.0
Updated reference to Tungsten.Net and made the appropriate code changes
Added W.Net.RPC.Client.RemoteEndPoint read-only property
W.Net.RPC.Client.Connected now also passes the remote IPEndPoint
W.Net.RPC.Client.Disconnected now also passes the remote IPEndPoint

4.17.2017 - v1.2.4.1
Rebuilt NuGet package with latest Release build

4.17.2017 - v1.2.4
Added more overrides to W.Net.RPC.Client.Call
Added W.Net.RPC.Client.CallAsync asynchronous methods
Default timeout for RPC calls is now 60 seconds, but can be modified through the new W.Net.RPC.Client.SetMaxTimeout method
Fixed several bugs in Tungsten.Net.RPC.Waiter.cs

4.11.2017 - v1.2.3
Initial release under new namespace (replaces Tungsten.Net.Core)

Thanks for using Tungsten.Net.RPC,

Jordan Duerksen
