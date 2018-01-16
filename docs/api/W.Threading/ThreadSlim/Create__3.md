ThreadSlim.Create&lt;TArg1, TArg2, TArg3> Method (Action&lt;CancellationToken, TArg1, TArg2, TArg3>)
====================================================================================================
   Constructs a new ThreadSlim using an Action&lt;CancellationToken> as the thread method

  **Namespace:**  [W.Threading][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public static ThreadSlim Create<TArg1, TArg2, TArg3>(
	Action<CancellationToken, TArg1, TArg2, TArg3> action
)

```

#### Parameters

##### *action*
Type: [System.Action][2]&lt;[CancellationToken][3], **TArg1**, **TArg2**, **TArg3**>  
The action to run on a separate thread

#### Type Parameters

##### *TArg1*
The Type of the first argument to be passed into the thread method

##### *TArg2*
The Type of the second argument to be passed into the thread method

##### *TArg3*
The Type of the third argument to be passed into the thread method

#### Return Value
Type: [ThreadSlim][4]  

[Missing &lt;returns> documentation for "M:W.Threading.ThreadSlim.Create``3(System.Action{System.Threading.CancellationToken,``0,``1,``2})"]


See Also
--------

#### Reference
[ThreadSlim Class][4]  
[W.Threading Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/bb548654
[3]: http://msdn.microsoft.com/en-us/library/dd384802
[4]: README.md