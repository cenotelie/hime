# README #

The Hime parser generator is a parser generator for the .Net platform (including Mono on Linux and MacOS) and Java. It primarily supports the LR family of parsing methods, including GLR (Generalized-LR). Key distinguishing features are:

* Fast LR and GLR parsing for the .Net platform and Java.
* Modern implementation of GLR with the RNGLR algorithm.
* Simple and clear API to manipulate parse trees.
* Extensive SDK API to programmatically manipulate grammars, generate lexers and parsers and use them.
* Strong emphasis on separating data and code. Hime forbids the inclusion of inline code in its grammar definitions in order to have very readable grammars that can be easily understood, debugged, improved. It is still possible to have custom code invoked during parsing with semantic actions.

### How do I use this software? ###

All user documentation is available in the [wiki](https://bitbucket.org/laurentw/hime/wiki/Home).

### How can I contribute? ###

#### Summary ####

The simplest way to contribute is to:

* Fork this repository on [Bitbucket](https://bitbucket.org/laurentw/hime).
* Fix [some issue](https://bitbucket.org/laurentw/hime/issues?status=new&status=open) or implement a new feature.
* Create a merge request on bitbucket.

Patches can also be submitted by email, or through the [issue management system](https://bitbucket.org/laurentw/hime/issues). Email contacts:

* [laurentw on Bitbucket](https://bitbucket.org/laurentw): lwouters at xowl dot org.

#### Repository content ####

The Hime parser generator generates parsers for the .Net platform and the Java runtime. The generator itselfs runs on the .Net platform. The repository is then structured as follow:

* Software components
	* `runtimes/net`: Contains the C# sources for the .Net implementation of the runtime library for the generated parsers.
	* `runtimes/java`: Contains the Java sources for the Java implementation of the runtime library.
	* `core`: Contains the C# sources of the SDK API, i.e. the parser generator.
	* `cli/net`: Contains the C# sources of the CLI interface of the parser generator.
	* `utilities/net/demo`: Contains the C# sources of demonstration usage of the SDK API.
	* `utilities/net/benchmark`: Contains the C# sources of the benchmark for the .Net platform.
* Others
	* `tests/net`: Contains the C# sources of the test suites for the .Net platform.
	* `packages`: Contains the NuGet dependencies for the .Net software components.
	* `extras`: Contains some extra products, e.g. standard grammars.
	* `releng`: Contains the release engineering artifacts.

#### Dependencies ####

* External Dependencies:
	* All .Net software artifacts depends on the .Net platform 2.0 and are compatible with higher versions, as well as Mono.
	* All Java software artifacts depends on Java 1.6 API.
	* Tests suites for the .Net platform (tests/net) depends on NUnit 2.6.3.
* Internal Dependencies:
	* `core` depends on `runtimes/net`.
	* `cli/net` depends on `core` and indirectly on `runtimes/net`.
	* `utilities/net/demo` depends on `core` and indirectly on `runtimes/net`.
	* `utilities/net/benchmark` depends on `core` and indirectly on `runtimes/net`.
	* `tests/net` depends on directly`core` and `cli/net` and indirectly on `runtimes/net`.

#### How to build? ####

* All .Net artifacts are organized in Visual Studio C# projects.
* The global solution for all the .Net project is HimeSystems.sln at the root of the repository.
* All Java artifacts are built using Maven.

Building .Net artifacts:

* Use the Visual Studio solution HimeSystems.sln at the root of the repository, it also works with MonoDevelop.

Building Java artifacts:

* Execute Maven `mvn package` on each Java artifact.