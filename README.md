# Vdf.NET

[![NuGet](https://img.shields.io/nuget/v/Gameloop.Vdf.svg?style=flat-square)](https://www.nuget.org/packages/Gameloop.Vdf)
[![AppVeyor](https://img.shields.io/appveyor/ci/Shravan2x/gameloop-vdf.svg?maxAge=2592000&style=flat-square)](https://ci.appveyor.com/project/Shravan2x/gameloop-vdf)

A fast, easy-to-use Valve Data Format parser for .NET

## Getting Binaries

Vdf.NET is available as a [NuGet package](https://www.nuget.org/packages/Gameloop.Vdf). Binaries can also be found on the [releases page](https://github.com/Shravan2x/Gameloop.Vdf/releases).

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

## License

Vdf.NET is released under the [MIT license](https://opensource.org/licenses/MIT).
