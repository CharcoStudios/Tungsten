PropertyBase&lt;TOwner, TValue> Class
=====================================
  
[Missing &lt;summary> documentation for "T:W.PropertyBase`2"]



Inheritance Hierarchy
---------------------
[System.Object][1]  
  [W.PropertyChangedNotifier][2]  
    **W.PropertyBase<TOwner, TValue>**  
      [W.Property&lt;TValue>][3]  
      [W.Property&lt;TOwner, TValue>][4]  

  **Namespace:**  [W][5]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public abstract class PropertyBase<TOwner, TValue> : PropertyChangedNotifier, 
	IProperty<TValue>, IProperty
where TOwner : class

```

#### Type Parameters

##### *TOwner*

[Missing &lt;typeparam name="TOwner"/> documentation for "T:W.PropertyBase`2"]


##### *TValue*

[Missing &lt;typeparam name="TValue"/> documentation for "T:W.PropertyBase`2"]


The **PropertyBase<TOwner, TValue>** type exposes the following members.


Constructors
------------

                    | Name                                 | Description                                                              
------------------- | ------------------------------------ | ------------------------------------------------------------------------ 
![Protected method] | [PropertyBase&lt;TOwner, TValue>][6] | Initializes a new instance of the **PropertyBase<TOwner, TValue>** class 


Properties
----------

                   | Name              | Description 
------------------ | ----------------- | ----------- 
![Public property] | [DefaultValue][7] |             
![Public property] | [IsDirty][8]      |             
![Public property] | [Owner][9]        |             
![Public property] | [Value][10]       |             


Methods
-------

                    | Name                             | Description                                                               
------------------- | -------------------------------- | ------------------------------------------------------------------------- 
![Protected method] | [ExecuteOnValueChanged][11]      |                                                                           
![Protected method] | [GetValue][12]                   | (Overrides [PropertyChangedNotifier.GetValue()][13].)                     
![Public method]    | [LoadValue][14]                  | Loads Value without raising events or calling the OnValueChanged callback 
![Protected method] | [OnPropertyChanged][15]          | (Overrides [PropertyChangedNotifier.OnPropertyChanged(String)][16].)      
![Protected method] | [RaisePropertyValueChanged][17]  |                                                                           
![Protected method] | [RaisePropertyValueChanging][18] |                                                                           
![Public method]    | [ResetToDefaultValue][19]        |                                                                           
![Protected method] | [SetValue][20]                   | (Overrides [PropertyChangedNotifier.SetValue(Object, String)][21].)       
![Public method]    | [WaitForChanged][22]             | Allows the caller to suspend it's thread until Value changes              


Events
------

                | Name                | Description                                                                         
--------------- | ------------------- | ----------------------------------------------------------------------------------- 
![Public event] | [ValueChanged][23]  | Raised after Value has changed                                                      
![Public event] | [ValueChanging][24] | Raised before Value has changed. To prevent Value from changing set cancel to true. 


Fields
------

                   | Name                 | Description 
------------------ | -------------------- | ----------- 
![Protected field] | [OnValueChanged][25] |             


Extension Methods
-----------------

                           | Name                     | Description                                              
-------------------------- | ------------------------ | -------------------------------------------------------- 
![Public Extension Method] | [CreateThread&lt;T>][26] | Starts a new thread (Defined by [ThreadExtensions][27].) 


See Also
--------

#### Reference
[W Namespace][5]  

[1]: http://msdn.microsoft.com/en-us/library/e5kfa45b
[2]: ../PropertyChangedNotifier/README.md
[3]: ../Property_1/README.md
[4]: ../Property_2/README.md
[5]: ../README.md
[6]: _ctor.md
[7]: DefaultValue.md
[8]: IsDirty.md
[9]: Owner.md
[10]: Value.md
[11]: ExecuteOnValueChanged.md
[12]: GetValue.md
[13]: ../PropertyChangedNotifier/GetValue.md
[14]: LoadValue.md
[15]: OnPropertyChanged.md
[16]: ../PropertyChangedNotifier/OnPropertyChanged.md
[17]: RaisePropertyValueChanged.md
[18]: RaisePropertyValueChanging.md
[19]: ResetToDefaultValue.md
[20]: SetValue.md
[21]: ../PropertyChangedNotifier/SetValue.md
[22]: WaitForChanged.md
[23]: ValueChanged.md
[24]: ValueChanging.md
[25]: OnValueChanged.md
[26]: ../../W.Threading/ThreadExtensions/CreateThread__1.md
[27]: ../../W.Threading/ThreadExtensions/README.md
[28]: ../../_icons/Help.png
[Protected method]: ../../_icons/protmethod.gif "Protected method"
[Public property]: ../../_icons/pubproperty.gif "Public property"
[Public method]: ../../_icons/pubmethod.gif "Public method"
[Public event]: ../../_icons/pubevent.gif "Public event"
[Protected field]: ../../_icons/protfield.gif "Protected field"
[Public Extension Method]: ../../_icons/pubextension.gif "Public Extension Method"