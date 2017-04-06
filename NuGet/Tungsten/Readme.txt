﻿Tungsten is a C# library to make application development easier.  See the wiki @ https://github.com/mode51/Tungsten/wiki for details, examples and use.

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
