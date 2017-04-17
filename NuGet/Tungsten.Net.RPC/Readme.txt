﻿Tungsten.Net.RPC is a client/server solution for invoking methods on a server.  Communications are made over the port you specify.  RPC methods on the server are created by attributing classes and methods with the [RPCClass] and [RPCMethod] attributes.  Clients make calls by passing in the method name ("MyNameSpace.MyClass.Method1") and any parameters to one of the Call or CallAsync methods.

4.17.2017 - v1.2.4
Added more overrides to W.Net.RPC.Client.Call
Added W.Net.RPC.Client.CallAsync asynchronous methods
Default timeout for RPC calls is now 60 seconds, but can be modified through the new W.Net.RPC.Client.SetMaxTimeout method
Fixed several bugs in Tungsten.Net.RPC.Waiter.cs

4.11.2017 - v1.2.3
Initial release under new namespace (replaces Tungsten.Net.Core)

Thanks for using Tungsten.Net.RPC,

Jordan Duerksen
