# README #

This is the core component of the Hime parser generator. It is implemented in C# as a library. It can be used as a SDK to programmatically generated parsers and more.
This software component is available:
* [As a NuGet package](https://www.nuget.org/packages/Hime.SDK/)
* [In the standalone distribution](https://bitbucket.org/laurentw/hime/downloads/)



### Requirements ###

* To run:
	* .Net Framework 2.0 or upper or Mono
* To build:
	* MSBuild or xbuild for Mono



### How to build ###

On the .Net Framework with MSBuild:

```
$ msbuild /p:Configuration=Release Hime.SDK.csproj
```

On Mono with xbuild:

```
$ xbuild /p:Configuration=Release Hime.SDK.csproj
```



### How to package ###

The target product for this component is a NuGet package:

```
$ nuget pack Hime.SDK.csproj -Build -Symbols -Properties Configuration=Release
```

To publish the package:

```
$ nuget push Hime.SDK.nupkg
```
