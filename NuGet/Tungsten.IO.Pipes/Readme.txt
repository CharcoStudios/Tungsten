﻿Named pipe wrappers to easily add asynchronous named pipes to your application.  Server and client are provided.

? v?
Added W.IO.Pipes.PipeTransceiver.WaitForSendComplete() so you can now wait for all messages to be sent
Removed static Write methods in W.IO.Pipes.FormattedPipeClient because they're wasteful and bad practice

5.11.2017 v1.2.7
Added PipeClientLogger

5.5.2017 -v1.2.6.1
Corrected the .Net45 dependencies in the NuGet package

5.5.2017 - v1.2.6
Rewrote PipeTransceiver.cs to be simpler and use less resources
Fixed some bugs in the process
Added Tungsten.IO.Pipes (for .Net45)
Added W.IO.Pipes.PipeClient.AddNamedPipeLogger which adds a named pipe logger to the W.Logging.Log methods

5.3.2017 - v1.2.5
Added static Write(string pipeName, byte[] message) method to W.IO.Pipes.FormattedClient and W.IO.Pipes.PipeClient
Fixed a bug where the pipe server would stop listening for messages

5.2.2017 - v1.2.4
Now using Newtonsoft for serialization instead of basic xml (this will be more robust out of the box)
Updated Tungsten reference to v1.2.4
Renamed output dll to just Tungsten.IO.Pipes

3.15.2017 - v1.2.0
Initial Release (replaces Tungsten.IO.Pipes.Standard)

Thanks for using Tungsten.IP.Pipes,

Jordan Duerksen
