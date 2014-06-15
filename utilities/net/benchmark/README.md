# README #

This is a utility component for the evaluation of the performance of the .Net implementation of the runtime.
It measure mean lexing and parsing times.
This component is not included in any binary distribution.



### Requirements ###

* To run:
	* .Net Framework 2.0 or upper or Mono
* To build:
	* MSBuild or xbuild for Mono



### How to build ###

On the .Net Framework with MSBuild:

```
$ msbuild /p:Configuration=Release Benchmark.csproj
```

On Mono with xbuild:

```
$ xbuild /p:Configuration=Release Benchmark.csproj
```



### How to run ###

On the .Net Framework:

```
$ bin/Release/Hime.Benchmark.exe
```

On Mono:

```
$ mono bin/Release/Hime.Benchmark.exe
```

The benchmark uses the `extras/Grammars/CSharp4.gram` grammar as a base and generate a file with 600 times the original content in it to use as an input sample.
Then, it measures the mean lexing and parsing times for this input.
