RSAMethods.Decrypt Method (String, RSAParameters, Int32)
========================================================
  Decrypts a string previously encrypted with RSA encryption

  **Namespace:**  [W.Encryption][1]  
  **Assembly:**  Tungsten (in Tungsten.dll)

Syntax
------

```csharp
public static string Decrypt(
	string text,
	RSAParameters key,
	int keySize = 2048
)
```

#### Parameters

##### *text*
Type: [System.String][2]  
The RSA encrypted string

##### *key*
Type: [System.Security.Cryptography.RSAParameters][3]  
The private key to use for decrypting

##### *keySize* (Optional)
Type: [System.Int32][4]  
The keysize, in bits, of the private key

#### Return Value
Type: [String][2]  
The decrypted string

See Also
--------

#### Reference
[RSAMethods Class][5]  
[W.Encryption Namespace][1]  

[1]: ../README.md
[2]: http://msdn.microsoft.com/en-us/library/s1wwdcbf
[3]: http://msdn.microsoft.com/en-us/library/ke2te33h
[4]: http://msdn.microsoft.com/en-us/library/td2s409d
[5]: README.md