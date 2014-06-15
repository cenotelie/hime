# README #

This is the command line interface for the Hime parser generator. It allows the generation of new parsers from the command line.
This software component is available:

* [In the standalone distribution](https://bitbucket.org/laurentw/hime/downloads/)



### Requirements ###

* To run:
	* .Net Framework 2.0 or upper or Mono
* To build:
	* MSBuild or xbuild for Mono



### How to build ###

On the .Net Framework with MSBuild:

```
$ msbuild /p:Configuration=Release HimeCC.csproj
```

On Mono with xbuild:

```
$ xbuild /p:Configuration=Release HimeCC.csproj
```
