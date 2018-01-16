SemaphoreSlimExtensions.InLock&lt;TType> Method (SemaphoreSlim, Func&lt;TType>)
===============================================================================
   Performs the action in a lock

  **Namespace:**  [W.LockExtensions][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public static TType InLock<TType>(
	this SemaphoreSlim this,
	Func<TType> func
)

```

#### Parameters

##### *this*
Type: [System.Threading.SemaphoreSlim][2]  
The SemaphoreSlim to provide resource locking

##### *func*
Type: [System.Func][3]&lt;**TType**>  
The function to perform

#### Type Parameters

##### *TType*

[Missing &lt;typeparam name="TType"/> documentation for "M:W.LockExtensions.SemaphoreSlimExtensions.InLock``1(System.Threading.SemaphoreSlim,System.Func{``0})"]


#### Return Value
Type: **TType**  

[Missing &lt;returns> documentation for "M:W.LockExtensions.SemaphoreSlimExtensions.InLock``1(System.Threading.SemaphoreSlim,System.Func{``0})"]

#### Usage Note
In Visual Basic and C#, you can call this method as an instance method on any object of type [SemaphoreSlim][2]. When you use instance method syntax to call this method, omit the first parameter. For more information, see [Extension Methods (Visual Basic)][4] or [Extension Methods (C# Programming Guide)][5].

See Also
--------

#### Reference
[SemaphoreSlimExtensions Class][6]  
[W.LockExtensions Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/dd271035
[3]: http://msdn.microsoft.com/en-us/library/bb534960
[4]: http://msdn.microsoft.com/en-us/library/bb384936.aspx
[5]: http://msdn.microsoft.com/en-us/library/bb383977.aspx
[6]: README.md