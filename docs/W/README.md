W Namespace
===========

[Missing &lt;summary> documentation for "N:W"]



Classes
-------

                | Class                                | Description                                                                                                                                                                                                                                             
--------------- | ------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public class] | [ActionQueue&lt;T>][1]               | 
Allows the programmer to enqueue items for processing on a separate thread. The ActionQueue will process items sequentially whenever an item is added.
                                                                                              
![Public class] | [CallResult][2]                      | A non-generic return value for a function. CallResult encapsulates a success/failure and an exception.                                                                                                                                                  
![Public class] | [CallResult&lt;TResult>][3]          | 
Generic class to be used as a return value. CallResult encapsulates a success/failure, an exception and a return value.
                                                                                                                             
![Public class] | [InvokeExtensions][4]                | Extension methods to provide code shortcuts to evaluate InvokeRequired and run code appropriately                                                                                                                                                       
![Public class] | [Lockable&lt;TValue>][5]             | 
Provides thread safety via locking
                                                                                                                                                                                                                  
![Public class] | [Property&lt;TValue>][6]             | A generic Property with no owner (self-owned)                                                                                                                                                                                                           
![Public class] | [Property&lt;TOwner, TValue>][7]     | A generic Property with an owner                                                                                                                                                                                                                        
![Public class] | [PropertyBase&lt;TOwner, TValue>][8] | Provides the functionality for the Property classes                                                                                                                                                                                                     
![Public class] | [PropertyChangedNotifier][9]         | 
This is a base class for supporting INotifyPropertyChanged
                                                                                                                                                                                          
![Public class] | [PropertyHost][10]                   | 
Provides a base class to automate the IsDirty, MarkAsClean and InitializeProperties functionality Note that this class does not support INotifyPropertyChanged and is not intented to host owned properties (though nothing prevents you from doing so)
 
![Public class] | [PropertyHostMethods][11]            | Exposes static PropertyHost extension methods                                                                                                                                                                                                           
![Public class] | [PropertyHostNotifier][12]           | 
Provides a base class to automate the IsDirty, MarkAsClean and InitializeProperties functionality Note that this class inherits PropertyChangedNotifier for INotifyPropertyChanged support
                                                          


Interfaces
----------

                    | Interface                  | Description                                                                                                                                         
------------------- | -------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public interface] | [IOwnedProperty][13]       | 
Used by PropertyHostMethods.InitializeProperties to find properties on which to set the owner. This interface is not used by self-owned properties.
 
![Public interface] | [IProperty][14]            | The base interface which Property must support                                                                                                      
![Public interface] | [IProperty&lt;TValue>][15] | The base interface which Property must support                                                                                                      


Delegates
---------

                   | Delegate                                                            | Description                                                                                     
------------------ | ------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------- 
![Public delegate] | [PropertyBase&lt;TOwner, TValue>.OnValueChangedDelegate][16]        | Used by the constructor to handle the property change via a callback rather than the events     
![Public delegate] | [PropertyBase&lt;TOwner, TValue>.PropertyValueChangedDelegate][17]  | Raised when the value of the property changes                                                   
![Public delegate] | [PropertyBase&lt;TOwner, TValue>.PropertyValueChangingDelegate][18] | Raised prior to the value of the property changing. Allows the programmer to cancel the change. 

[1]: ActionQueue_1/README.md
[2]: CallResult/README.md
[3]: CallResult_1/README.md
[4]: InvokeExtensions/README.md
[5]: Lockable_1/README.md
[6]: Property_1/README.md
[7]: Property_2/README.md
[8]: PropertyBase_2/README.md
[9]: PropertyChangedNotifier/README.md
[10]: PropertyHost/README.md
[11]: PropertyHostMethods/README.md
[12]: PropertyHostNotifier/README.md
[13]: IOwnedProperty/README.md
[14]: IProperty/README.md
[15]: IProperty_1/README.md
[16]: PropertyBase_2_OnValueChangedDelegate/README.md
[17]: PropertyBase_2_PropertyValueChangedDelegate/README.md
[18]: PropertyBase_2_PropertyValueChangingDelegate/README.md
[19]: ../_icons/Help.png
[Public class]: ../_icons/pubclass.gif "Public class"
[Public interface]: ../_icons/pubinterface.gif "Public interface"
[Public delegate]: ../_icons/pubdelegate.gif "Public delegate"