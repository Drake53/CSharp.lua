# War3Net.CSharpLua.CoreSystem

## About

War3Net.CSharpLua.CoreSystem is a NuGet package that provides the Lua runtime library files required by code compiled with [CSharp.lua](https://github.com/Drake53/CSharp.lua). It implements core .NET types and collections in Lua, enabling C#-compiled Lua code to run on a Lua VM. It is part of the [War3Net](https://github.com/Drake53/War3Net) modding toolset for Warcraft III.

## Key features

* Lua implementations of core .NET types (strings, numbers, booleans, dates, enums, etc.)
* Generic collections (List, Dictionary, HashSet, Queue, Stack, SortedSet, LinkedList, PriorityQueue, etc.)
* LINQ support
* StringBuilder, Math, Random, and Convert utilities
* Numerics types (Vector2, Vector3, Vector4, Matrix3x2, Matrix4x4, Quaternion, Complex, etc.)
* Threading primitives (Task, Thread, Timer)
* Reflection and type system support
* Span and ReadOnlySpan support

## How to Use

### Get the list of CoreSystem Lua files in dependency order

```csharp
using CSharpLua.CoreSystem;

// Returns Lua file paths in the correct load order
foreach (var file in CoreSystemProvider.GetCoreSystemFiles())
{
    // Load each Lua file into the Lua VM
    Console.WriteLine(file);
}
```

## Main Types

The main type provided by this library is:

* `CSharpLua.CoreSystem.CoreSystemProvider` - Provides the ordered list of CoreSystem Lua files needed at runtime

## Related Packages

* [War3Net.CSharpLua](https://www.nuget.org/packages/War3Net.CSharpLua) - The C# to Lua compiler
* [War3Net.Build](https://www.nuget.org/packages/War3Net.Build) - Generate map scripts and compile Warcraft III maps

## Feedback and contributing

War3Net.CSharpLua.CoreSystem is part of a fork of [CSharp.lua](https://github.com/yanghuan/CSharp.lua), released as open source under the [Apache 2.0 license](https://github.com/yanghuan/CSharp.lua/blob/master/LICENSE). General bug reports and contributions should be made to the upstream repository:

* [File an issue (upstream)](https://github.com/yanghuan/CSharp.lua/issues)
* [Submit a pull request (upstream)](https://github.com/yanghuan/CSharp.lua/pulls)

For issues specific to the War3Net fork, use the [fork repository](https://github.com/Drake53/CSharp.lua).

## Disclaimer

This README was generated with the assistance of AI and may contain inaccuracies. Please verify the information and consult the source code for authoritative details.
