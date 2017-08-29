# Vdf.NET

[![NuGet](https://img.shields.io/nuget/v/Gameloop.Vdf.svg?style=flat-square)](https://www.nuget.org/packages/Gameloop.Vdf)
[![AppVeyor](https://img.shields.io/appveyor/ci/Shravan2x/gameloop-vdf.svg?maxAge=2592000&style=flat-square)](https://ci.appveyor.com/project/Shravan2x/gameloop-vdf)

A fast, easy-to-use Valve Data Format parser for .NET

## Getting Binaries

Vdf.NET is available as a [NuGet package](https://www.nuget.org/packages/Gameloop.Vdf). Binaries can also be found on the [releases page](https://github.com/Shravan2x/Gameloop.Vdf/releases).

## Performance

Vdf.NET was originally written as an experiment in deserialization performance. It is significantly faster than alternatives like SteamKit's KeyValue and even Json.NET (though I admit Json.NET is far more feature rich).

The test source file is [VdfNetBenchmark.cs](https://github.com/shravan2x/Gameloop.Vdf/blob/master/Tests/VdfNetBenchmark.cs). I used version [Vdf.NET 0.4.1](https://github.com/shravan2x/Gameloop.Vdf/releases/tag/Vdf.NET_0.4.1) and the TF2 schema, which is available both in [JSON](http://api.steampowered.com/IEconItems_440/GetSchema/v0001/?key=xxxxxx&format=json) and [VDF](http://api.steampowered.com/IEconItems_440/GetSchema/v0001/?key=xxxxxx&format=vdf) formats (you'll need an API key to obtain them).

The following are the times taken for 10 iterations of deserializing the schema on an Intel i7-4790k processor.
```
Vdf.NET (VDF)	    : 129ms, 501871ticks average
Json.NET (JSON)	    : 270ms, 1022480ticks average
SK2 KeyValue (VDF)  : 340ms, 1255055ticks average
```

## Documentation

To deserialize a file _importantInfo.vdf_,
```
"Steam"
{
	"SSAVersion"		"3"
	"PrivacyPolicyVersion"		"2"
	"SteamDefaultDialog"		"#app_store"
	"DesktopShortcutCheck"		"0"
	"StartMenuShortcutCheck"		"0"
}
```
do
```c#
dynamic volvo = VdfConvert.Deserialize(File.ReadAllText("importantInfo.vdf"));
// 'volvo' is a VProperty, analogous to Json.NET's JProperty

// Now do whatever with this
Console.WriteLine(volvo.Value.SSAVersion); // Prints 3
```

Note the need to use `.Value` and skip the enclosing property name `Steam`. This is because root types in VDF are _properties_, as opposed to _objects_ in traditional JSON.

## Extensions

[Gameloop.Vdf.JsonConverter](https://github.com/shravan2x/Gameloop.Vdf.JsonConverter): VDF-JSON converters for Vdf.NET.

## License

Vdf.NET is released under the [MIT license](https://opensource.org/licenses/MIT).
