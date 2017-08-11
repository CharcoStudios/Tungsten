W Namespace
===========
The root namespace for Tungsten. W is used because it is the symbol for Tungsten on the periodic table of elements.


Classes
-------

                | Class                                 | Description                                                                                                                                                                                                                                             
--------------- | ------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public class] | [ActionQueue&lt;T>][1]                | 
Allows the programmer to enqueue items for processing on a separate thread. The ActionQueue will process items sequentially whenever an item is added.
                                                                                              
![Public class] | [AsExtensions][2]                     | Extensions which convert objects of one type to another                                                                                                                                                                                                 
![Public class] | [CallResult][3]                       | A non-generic return value for a function. CallResult encapsulates a success/failure and an exception.                                                                                                                                                  
![Public class] | [CallResult&lt;TResult>][4]           | 
Generic class to be used as a return value. CallResult encapsulates a success/failure, an exception and a return value.
                                                                                                                             
![Public class] | [FromExtensions][5]                   | Extensions which convert objects of one type to another                                                                                                                                                                                                 
![Public class] | [InvokeExtensions][6]                 | Extension methods to provide code shortcuts to evaluate InvokeRequired and run code appropriately                                                                                                                                                       
![Public class] | [Lockable&lt;TValue>][7]              | 
Provides thread safety via locking
                                                                                                                                                                                                                  
![Public class] | [Property&lt;TValue>][8]              | A generic Property with no owner (self-owned)                                                                                                                                                                                                           
![Public class] | [Property&lt;TOwner, TValue>][9]      | A generic Property with an owner                                                                                                                                                                                                                        
![Public class] | [PropertyBase&lt;TOwner, TValue>][10] | Provides the functionality for the Property classes                                                                                                                                                                                                     
![Public class] | [PropertyChangedNotifier][11]         | 
This is a base class for supporting INotifyPropertyChanged
                                                                                                                                                                                          
![Public class] | [PropertyHost][12]                    | 
Provides a base class to automate the IsDirty, MarkAsClean and InitializeProperties functionality Note that this class does not support INotifyPropertyChanged and is not intented to host owned properties (though nothing prevents you from doing so)
 
![Public class] | [PropertyHostMethods][13]             | Exposes static PropertyHost extension methods                                                                                                                                                                                                           
![Public class] | [PropertyHostNotifier][14]            | 
Provides a base class to automate the IsDirty, MarkAsClean and InitializeProperties functionality Note that this class inherits PropertyChangedNotifier for INotifyPropertyChanged support
                                                          
![Public class] | [Singleton&lt;TSingletonType>][15]    | Thread-safe Singleton implementation                                                                                                                                                                                                                    
![Public class] | [StringExtensions][16]                | Some useful string extensions                                                                                                                                                                                                                           


Interfaces
----------

                    | Interface                  | Description                                                                                                                                         
------------------- | -------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------- 
![Public interface] | [IOwnedProperty][17]       | 
Used by PropertyHostMethods.InitializeProperties to find properties on which to set the owner. This interface is not used by self-owned properties.
 
![Public interface] | [IProperty][18]            | The base interface which Property must support                                                                                                      
![Public interface] | [IProperty&lt;TValue>][19] | The base interface which Property must support                                                                                                      


Delegates
---------

                   | Delegate                                                            | Description                                                                                     
------------------ | ------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------- 
![Public delegate] | [PropertyBase&lt;TOwner, TValue>.OnValueChangedDelegate][20]        | Used by the constructor to handle the property change via a callback rather than the events     
![Public delegate] | [PropertyBase&lt;TOwner, TValue>.PropertyValueChangedDelegate][21]  | Raised when the value of the property changes                                                   
![Public delegate] | [PropertyBase&lt;TOwner, TValue>.PropertyValueChangingDelegate][22] | Raised prior to the value of the property changing. Allows the programmer to cancel the change. 

[1]: ActionQueue_1/README.md
[2]: AsExtensions/README.md
[3]: CallResult/README.md
[4]: CallResult_1/README.md
[5]: FromExtensions/README.md
[6]: InvokeExtensions/README.md
[7]: Lockable_1/README.md
[8]: Property_1/README.md
[9]: Property_2/README.md
[10]: PropertyBase_2/README.md
[11]: PropertyChangedNotifier/README.md
[12]: PropertyHost/README.md
[13]: PropertyHostMethods/README.md
[14]: PropertyHostNotifier/README.md
[15]: Singleton_1/README.md
[16]: StringExtensions/README.md
[17]: IOwnedProperty/README.md
[18]: IProperty/README.md
[19]: IProperty_1/README.md
[20]: PropertyBase_2_OnValueChangedDelegate/README.md
[21]: PropertyBase_2_PropertyValueChangedDelegate/README.md
[22]: PropertyBase_2_PropertyValueChangingDelegate/README.md
[Public class]: ../_icons/pubclass.gif "Public class"
[Public interface]: ../_icons/pubinterface.gif "Public interface"
[Public delegate]: ../_icons/pubdelegate.gif "Public delegate"