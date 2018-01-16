ArrayMethods.Peek&lt;T> Method
==============================
   Retrieves the specified range of elements from the array

  **Namespace:**  [W][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public static T[] Peek<T>(
	T[] source,
	int startIndex,
	int length
)

```

#### Parameters

##### *source*
Type: **T**[]  
The source array

##### *startIndex*
Type: [System.Int32][2]  
The index from which to start retrieving elements

##### *length*
Type: [System.Int32][2]  
The number of elements to retrieve

#### Type Parameters

##### *T*
The data type

#### Return Value
Type: **T**[]  
A new array containing only the specified subset of elements

See Also
--------

#### Reference
[ArrayMethods Class][3]  
[W Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/td2s409d
[3]: README.md