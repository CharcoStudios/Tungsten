MonitorLocker Class
===================
   Uses Monitor to provide resource locking


Inheritance Hierarchy
---------------------
[System.Object][1]  
  **W.Threading.Lockers.MonitorLocker**  

  **Namespace:**  [W.Threading.Lockers][2]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public class MonitorLocker : ILocker<Object>, 
	ILocker
```

The **MonitorLocker** type exposes the following members.


Constructors
------------

                 | Name               | Description                                               
---------------- | ------------------ | --------------------------------------------------------- 
![Public method] | [MonitorLocker][3] | Initializes a new instance of the **MonitorLocker** class 


Properties
----------

                   | Name        | Description                      
------------------ | ----------- | -------------------------------- 
![Public property] | [Locker][4] | The object used to perform locks 


Methods
-------

                    | Name                                          | Description                               
------------------- | --------------------------------------------- | ----------------------------------------- 
![Public method]    | [Equals][5]                                   | (Inherited from [Object][1].)             
![Protected method] | [Finalize][6]                                 | (Inherited from [Object][1].)             
![Public method]    | [GetHashCode][7]                              | (Inherited from [Object][1].)             
![Public method]    | [GetType][8]                                  | (Inherited from [Object][1].)             
![Public method]    | [InLock(Action)][9]                           | Executes an action from within a Monitor  
![Public method]    | [InLock&lt;TValue>(Func&lt;TValue>)][10]      | Executes a function from within a Monitor 
![Public method]    | [InLockAsync(Action)][11]                     | Executes an action from within a Monitor  
![Public method]    | [InLockAsync&lt;TValue>(Func&lt;TValue>)][12] | Executes a function from within a Monitor 
![Protected method] | [MemberwiseClone][13]                         | (Inherited from [Object][1].)             
![Public method]    | [ToString][14]                                | (Inherited from [Object][1].)             


Remarks
-------
Can be overridden to provide additional functionality

See Also
--------

#### Reference
[W.Threading.Lockers Namespace][2]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../README.md
[3]: _ctor.md
[4]: Locker.md
[5]: http://msdn.microsoft.com/en-us/library/bsc2ak47
[6]: http://msdn.microsoft.com/en-us/library/4k87zsw7
[7]: http://msdn.microsoft.com/en-us/library/zdee4b3y
[8]: http://msdn.microsoft.com/en-us/library/dfwy45w9
[9]: InLock.md
[10]: InLock__1.md
[11]: InLockAsync.md
[12]: InLockAsync__1.md
[13]: http://msdn.microsoft.com/en-us/library/57ctke0a
[14]: http://msdn.microsoft.com/en-us/library/7bxwbwt2
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Protected method]: ../../_icons/protmethod.gif "Protected method"