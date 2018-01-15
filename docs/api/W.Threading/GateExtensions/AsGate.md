GateExtensions.AsGate Method (Action&lt;CancellationToken>)
===========================================================
   Creates a Gate with the supplied action

  **Namespace:**  [W.Threading][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public static Gate AsGate(
	this Action<CancellationToken> action
)
```

#### Parameters

##### *action*
Type: [System.Action][2]&lt;[CancellationToken][3]>  
The Action to call when the gate is relased (when Run is called)

#### Return Value
Type: [Gate][4]  
A reference to a new Gate
#### Usage Note
In Visual Basic and C#, you can call this method as an instance method on any object of type [Action][2]&lt;[CancellationToken][3]>. When you use instance method syntax to call this method, omit the first parameter. For more information, see [Extension Methods (Visual Basic)][5] or [Extension Methods (C# Programming Guide)][6].

See Also
--------

#### Reference
[GateExtensions Class][7]  
[W.Threading Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/018hxwa8
[3]: http://msdn.microsoft.com/en-us/library/dd384802
[4]: ../Gate/README.md
[5]: http://msdn.microsoft.com/en-us/library/bb384936.aspx
[6]: http://msdn.microsoft.com/en-us/library/bb383977.aspx
[7]: README.md