# README #

This is a utility component for the evaluation of the performance of the .Net implementation of the runtime.
It measure mean lexing and parsing times.
This component is not included in any binary distribution.



### Requirements ###

* To run:
	* .Net Framework 2.0 or upper or Mono
* To build:
	* MSBuild or xbuild for Mono



### How to use ###

Use the wrapper script on Windows:

```
$ utilities\benchmark.bat
```

Use the wrapper script on Linux:

```
$ ./utilities/benchmark.sh
```