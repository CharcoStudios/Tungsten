﻿Tungsten is a C# library to make application development easier.  See the wiki @ https://github.com/mode51/Tungsten/wiki for details, examples and use.

8.27.2017 v1.2.8
Added W.Threading.Thread.Sleep(CPUProfileEnum level)
Added overloads to W.Logging.Log.x methods which add callerName and callerLineNumber to the message
Added W.ExtensionMethods.WaitForValue extensions

8.10.2017 v1.2.7.1
Corrected target framework order in NuGet package

7.31.2017 v1.2.7
Added W.Logging.Log.MessageHistory
Added support for NetStandard1.0 with most functionality (no encryption or compression)

6.22.2017 v1.2.6
All Tungsten projects now share most of their code from Tungsten.Standard (Threading.Thread and Encryption.RSA are about the only items not able to be shared)
Added NuGet targetframework netcoreapp1.0
Added Singleton<TSingletonType>
Added Property(TValue defaultValue, OnValueChangedDelegate onValueChanged = null) constructor

5.11.2017 v1.2.5
Added W.StringExtensions (string.IsValidBase64())
Added W.Logging.CustomLogger to make adding custom loggers easier

5.2.2017 v1.2.4
All target dlls are now named Tungsten.dll - this should help with cross-framework compatibility

4.12.2017 v1.2.3.2
Changed netstandard minimum from 1.4 to 1.3

4.11.2017 v1.2.3.1
All target frameworks should work now

4.6.2017 v1.2.3
Attempting to fix NuGet inclusion of Tungsten.Universal

3.30.2017 v1.2.2
Added ExecuteInLock/ExecutInLockAsync to Lockable class
Added additional functionality to W.Threading.Thread and W.Threading.ThreadBase
Updated some XML documentation

3.16.2017 v1.2.1
Added RSA encryption to Tungsten (net45).  It already exists in netstandard1.4 (it is not in PCL or Universal - and may never)

3.15.2017 v1.2.0
Merged Tungsten.Standard into Tungsten NuGet Package
  - Tungsten package now contains net45, portable, universal and netstandard targets

2.28.2017 v1.1.4
Had to remove Tungsten.Core from this package because NuProj doesn't support it yet

2.28.2017 v1.1.3
Merged the Tungsten, Tungsten.Portable, Tungsten.Universal and Tungsten.Core NuGet packages

2.16.2017 v1.1.2
Fixed AsJson extension method

2.16.2017 v1.1.1
Fixed As<TType> and AsJson<TType>

2.16.2017
Added As.cs which contains a number of extension methods for converting objects from one type to another

2.9.2017
Removed Tungsten.Threading.GateExtensions - the methods seem rather redundant or at least unnecessary

2.1.2017
Removed unused references

1.29.2017
PropertyHostMethods now actually exposes extension methods (forgot to make them extensions)

1.27.2017
Added PropertyHostNotifier which aggregates the functionality of PropertyChangedNotifier and PropertyHost
Fixed Property so it actually updates the UI automatically (Owner functionality wasn't working)

1.21.2017
Added PropertyBase.WaitForChanged

1.16.2017
Added W.ActionQueue<T>
Added W.Threading.Gate/Gate<T>
Added W.Threading.Thread/Thread<T>
Updated ThreadExtensions

1.13.2017
Added ThreadExtensions

1.12.2017

Added Invoke (InvokeExtensions)
Added CallResult and CallResult<TResult>
Added CallResult tests


Initial Release 1.11.2017

Lockable<TValue>
Property<TValue>
Property<TOwner, TValue>
PropertyHost

Thanks for using Tungsten,

Jordan Duerksen
