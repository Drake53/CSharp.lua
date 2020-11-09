# CSharp.lua
The C# to Lua compiler.

## Introduction
CSharp.lua is a C# to Lua compiler. Write C# then run on lua VM.
* Build on [Microsoft Roslyn](https://github.com/dotnet/roslyn). Support for C# 8.0.

* Highly readable code generation. C# AST ---> Lua AST ---> Lua Code.

* Allowing almost all of the C# language features.

* Provides [CoreSystem.lua](https://github.com/yanghuan/CSharp.lua/tree/master/CSharp.lua/CoreSystem.Lua/CoreSystem) library, can run away of CLR.

* Self-Compiling, run "./test/self-compiling/self.bat".

* Used by .NET Core, Ability to across platforms.

## Sample
C# code
```csharp
using System;

namespace HelloLua {
  public static class Program {
    public static void Main() {
      Console.WriteLine("hello lua!");
    }
  }
}
```
To Lua
```lua
-- Generated by CSharp.lua Compiler
local System = System
System.namespace("HelloLua", function (namespace) 
  namespace.class("Program", function (namespace) 
    local Main
    Main = function () 
      System.Console.WriteLine("hello lua!")
    end
    return {
      Main = Main
    }
  end)
end)
```

## Try Live
https://yanghuan.github.io/external/bridgelua-editor/index.html

## How to Use 
### Command Line Parameters
```cmd
D:\>dotnet CSharp.Lua.Launcher.dll -h
Usage: CSharp.lua [-s srcfolder] [-d dstfolder]
Arguments
-s              : can be a directory where all cs files will be compiled, or a list of files, using ';' or ',' to separate
-d              : destination directory, will put the out lua files

Options
-h              : show the help message and exit
-l              : libraries referenced, use ';' to separate
                  if the librarie is a module, whitch is compield by CSharp.lua with -module arguemnt, the last character needs to be '!' in order to mark  

-m              : meta files, like System.xml, use ';' to separate
-csc            : csc.exe command argumnets, use ' ' or '\t' to separate

-c              : support classic lua version(5.1), default support 5.3
-a              : attributes need to export, use ';' to separate, if ""-a"" only, all attributes whill be exported
-e              : enums need to export, use ';' to separate, if ""-e"" only, all enums will be exported
-p              : do not use debug.setmetatable, in some Addon/Plugin environment debug object cannot be used
-metadata       : export all metadata, use @CSharpLua.Metadata annotations for precise control
-module         : the currently compiled assembly needs to be referenced, it's useful for multiple module compiled
-inline-property: inline some single-line properties
-include        : the root directory of the CoreSystem library, adds all the dependencies to a single file named out.lua
```
Make sure that .NET 5.0 is installed.
https://dotnet.microsoft.com/download/dotnet/5.0

### Download
https://github.com/yanghuan/CSharp.lua/releases

## CoreSystem.lua
[CoreSystem.lua library](https://github.com/yanghuan/CSharp.lua/tree/master/CSharp.lua/CoreSystem.Lua/CoreSystem) that implements most of the [.NET Framework core classes](http://referencesource.microsoft.com/), including support for basic type, delegate, generic collection classes & linq. The Converted lua code, need to reference it  

## Example
- [fibonacci](https://github.com/yanghuan/CSharp.lua/tree/master/test/fibonacci), a console program code, print Fibonacci number. 

## Documentation
https://github.com/yanghuan/CSharp.lua/wiki

## *License*
[Apache 2.0 license](https://raw.githubusercontent.com/yanghuan/CSharp.lua/master/LICENSE).

## *Acknowledgements*
- [Bridge.NET](http://bridge.net/)
- [WootzJs](https://github.com/kswoll/WootzJs)
- [.NET referencesource](http://referencesource.microsoft.com/)

## Communication
- [Issues](https://github.com/yanghuan/CSharp.lua/issues)
- Mail：sy.yanghuan@gmail.com
- QQ Group: 715350749 (Chinese Only)

