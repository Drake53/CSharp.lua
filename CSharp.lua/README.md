# War3Net.CSharpLua

## About

War3Net.CSharpLua is a C# to Lua compiler built on [Microsoft Roslyn](https://github.com/dotnet/roslyn), with edits to make it compatible with Warcraft III Lua maps. It is a fork of [CSharp.lua](https://github.com/yanghuan/CSharp.lua) and part of the [War3Net](https://github.com/Drake53/War3Net) modding toolset.

## Key features

* Compile C# source code to Lua, with support for C# 10
* Highly readable code generation via C# AST to Lua AST to Lua code
* Compile from source files, directories, or .csproj projects
* Support for NuGet package and assembly references
* Export metadata, enums, and attributes to Lua
* Inline simple single-line properties
* Support for multi-module compilation

## How to Use

### Compile a C# project to Lua

```csharp
using CSharpLua;

var compiler = new Compiler(
    input: "path/to/src",
    output: "path/to/output",
    lib: "path/to/libs",
    meta: "path/to/meta",
    csc: null,
    isClassic: false,
    attributes: null,
    enums: null,
    isExportEnumAll: false);

compiler.IsExportMetadata = true;
compiler.IsInlineSimpleProperty = true;

compiler.CompileSingleFile("out.lua", "path/to/coresystem");
```

## Main Types

The main types provided by this library are:

* `CSharpLua.Compiler` - Compiles C# source code or projects to Lua
* `CSharpLua.LuaSyntaxGenerator` - Generates Lua syntax trees from C# compilation units

## Related Packages

* [War3Net.CSharpLua.CoreSystem](https://www.nuget.org/packages/War3Net.CSharpLua.CoreSystem) - Lua runtime library required by the compiled output
* [War3Net.Build](https://www.nuget.org/packages/War3Net.Build) - Generate map scripts and compile Warcraft III maps

## Feedback and contributing

War3Net.CSharpLua is part of a fork of [CSharp.lua](https://github.com/yanghuan/CSharp.lua), released as open source under the [Apache 2.0 license](https://github.com/yanghuan/CSharp.lua/blob/master/LICENSE). General bug reports and contributions should be made to the upstream repository:

* [File an issue (upstream)](https://github.com/yanghuan/CSharp.lua/issues)
* [Submit a pull request (upstream)](https://github.com/yanghuan/CSharp.lua/pulls)

For issues specific to the War3Net fork, use the [fork repository](https://github.com/Drake53/CSharp.lua).

## Disclaimer

This README was generated with the assistance of AI and may contain inaccuracies. Please verify the information and consult the source code for authoritative details.
