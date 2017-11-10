EventTemplate&lt;TEventArg1, TEventArg2>.Raise Method
=====================================================
   Raises the template event

  **Namespace:**  [W][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public void Raise(
	Object sender,
	TEventArg1 arg1,
	TEventArg2 arg2,
	string callerMemberName = ""
)
```

#### Parameters

##### *sender*
Type: [System.Object][2]  
The object which raised this event

##### *arg1*
Type: [TEventArg1][3]  
The first detailed event argument

##### *arg2*
Type: [TEventArg2][3]  
The second detailed event argument

##### *callerMemberName* (Optional)
Type: [System.String][4]  
The name of the method which raised the event


See Also
--------

#### Reference
[EventTemplate&lt;TEventArg1, TEventArg2> Class][3]  
[W Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[3]: README.md
[4]: http://msdn.microsoft.com/en-us/library/s1wwdcbf