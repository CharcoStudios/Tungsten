@echo Cleaning
@del *.dll /s
@del *.xml /s

@echo Copying Files
@mkdir Tungsten\lib\net45
@mkdir Tungsten\lib\netstandard1.4
@mkdir Tungsten\lib\uap10.0
@mkdir Tungsten\lib\portable-net45+win+wpa81+MonoAndroid10+MonoTouch10+xamarinios10+xamarinmac20
@copy ..\Src\Tungsten\bin\Release\Tungsten.dll Tungsten\lib\net45 /y
@copy ..\Src\Tungsten\bin\Release\Tungsten.xml Tungsten\lib\net45 /y

@copy ..\Src\Tungsten.Standard\bin\Release\netstandard1.4\Tungsten.Standard.dll Tungsten\lib\netstandard1.4 /y
@copy ..\Src\Tungsten.Standard\bin\Release\netstandard1.4\Tungsten.Standard.xml Tungsten\lib\netstandard1.4 /y

@copy ..\Src\Tungsten.Universal\bin\Release\Tungsten.Universal.dll Tungsten\lib\uap10.0 /y
@copy ..\Src\Tungsten.Universal\bin\Release\Tungsten.Universal.xml Tungsten\lib\uap10.0 /y

@copy ..\Src\Tungsten.Portable\bin\Release\Tungsten.Portable.dll "Tungsten\lib\portable-net45+win+wpa81+MonoAndroid10+MonoTouch10+xamarinios10+xamarinmac20\" /y
@copy ..\Src\Tungsten.Portable\bin\Release\Tungsten.Portable.xml "Tungsten\lib\portable-net45+win+wpa81+MonoAndroid10+MonoTouch10+xamarinios10+xamarinmac20\" /y

nuget pack Tungsten

@mkdir Tungsten.Net\lib\netstandard1.4
@copy ..\Src\Tungsten.Net.Standard\bin\Release\netstandard1.4\Tungsten.Net.Standard.dll Tungsten.Net\lib\netstandard1.4 /y
@copy ..\Src\Tungsten.Net.Standard\bin\Release\netstandard1.4\Tungsten.Net.Standard.xml Tungsten.Net\lib\netstandard1.4 /y
nuget pack Tungsten.Net
 
@mkdir Tungsten.IO.Pipes\lib\netstandard1.4
@copy ..\Src\Tungsten.IO.Pipes.Standard\bin\Release\netstandard1.4\Tungsten.IO.Pipes.Standard.dll Tungsten.IO.Pipes\lib\netstandard1.4 /y
@copy ..\Src\Tungsten.IO.Pipes.Standard\bin\Release\netstandard1.4\Tungsten.IO.Pipes.Standard.xml Tungsten.IO.Pipes\lib\netstandard1.4 /y
 
nuget pack Tungsten.IO.Pipes

pause