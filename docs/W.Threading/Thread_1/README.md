Thread&lt;T> Class
==================
  A thread wrapper which makes multi-threading easier


Inheritance Hierarchy
---------------------
[System.Object][1]  
  [W.Threading.ThreadBase][2]  
    [W.Threading.Thread][3]  
      **W.Threading.Thread<T>**  
        [W.Threading.Gate&lt;T>][4]  

  **Namespace:**  [W.Threading][5]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public class Thread<T> : Thread

```

#### Type Parameters

##### *T*

[Missing &lt;typeparam name="T"/> documentation for "T:W.Threading.Thread`1"]


The **Thread<T>** type exposes the following members.


Constructors
------------

                 | Name              | Description         
---------------- | ----------------- | ------------------- 
![Public method] | [Thread&lt;T>][6] | Starts a new thread 


Properties
----------

                      | Name            | Description                              
--------------------- | --------------- | ---------------------------------------- 
![Protected property] | [Action][7]     | The parameterized thread procedure       
![Public property]    | [CustomData][8] | Custom data to be passed into the thread 


Methods
-------

                                 | Name               | Description                                                                                               
-------------------------------- | ------------------ | --------------------------------------------------------------------------------------------------------- 
![Public method]![Static member] | [Create][9]        | Starts a new thread                                                                                       
![Protected method]              | [InvokeAction][10] | Overridden implementation which calls Action with CustomData (Overrides [ThreadBase.InvokeAction()][11].) 


Extension Methods
-----------------

                                          | Name                       | Description                                                                                                                                                                                                                      
----------------------------------------- | -------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public Extension Method]![Code example] | [As&lt;TType>][12]         | Use Generic syntax for the as operator. (Defined by [AsExtensions][13].)                                                                                                                                                         
![Public Extension Method]                | [AsJson&lt;TType>][14]     | Serializes an object to a Json string (Defined by [AsExtensions][13].)                                                                                                                                                           
![Public Extension Method]                | [AsXml&lt;TType>][15]      | Serializes an object to an xml string (Defined by [AsExtensions][13].)                                                                                                                                                           
![Public Extension Method]                | [CreateThread&lt;T>][16]   | Starts a new thread (Defined by [ThreadExtensions][17].)                                                                                                                                                                         
![Public Extension Method]                | [InitializeProperties][18] | 
Scans the fields and properties of "owner" and sets the member's Owner property to "owner" This method should be called in the constructor of any class which has IOwnedProperty members
 (Defined by [PropertyHostMethods][19].) 
![Public Extension Method]                | [IsDirty][20]              | 
Scans the IsDirty value of each field and property of type IProperty
 (Defined by [PropertyHostMethods][19].)                                                                                                                 
![Public Extension Method]                | [MarkAsClean][21]          | 
Scans each field and property of type IProperty and sets it's IsDirty flag to false
 (Defined by [PropertyHostMethods][19].)                                                                                                  


See Also
--------

#### Reference
[W.Threading Namespace][5]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../ThreadBase/README.md
[3]: ../Thread/README.md
[4]: ../Gate_1/README.md
[5]: ../README.md
[6]: _ctor.md
[7]: Action.md
[8]: CustomData.md
[9]: Create.md
[10]: InvokeAction.md
[11]: ../ThreadBase/InvokeAction.md
[12]: ../../W/AsExtensions/As__1.md
[13]: ../../W/AsExtensions/README.md
[14]: ../../W/AsExtensions/AsJson__1.md
[15]: ../../W/AsExtensions/AsXml__1.md
[16]: ../ThreadExtensions/CreateThread__1.md
[17]: ../ThreadExtensions/README.md
[18]: ../../W/PropertyHostMethods/InitializeProperties.md
[19]: ../../W/PropertyHostMethods/README.md
[20]: ../../W/PropertyHostMethods/IsDirty.md
[21]: ../../W/PropertyHostMethods/MarkAsClean.md
[22]: ../../_icons/Help.png
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Protected property]: ../../_icons/protproperty.gif "Protected property"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Static member]: ../../_icons/static.gif "Static member"
[Protected method]: ../../_icons/protmethod.gif "Protected method"
[Public Extension Method]: ../../_icons/pubextension.gif "Public Extension Method"
[Code example]: ../../_icons/CodeExample.png "Code example"