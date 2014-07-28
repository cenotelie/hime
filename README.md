# README #

The Hime parser generator is a parser generator for various platforms (see below). It primarily supports the LR family of parsing methods, including GLR (Generalized-LR). Key distinguishing features are:

* Fast LR and GLR parsing.
* Modern implementation of GLR with the [RNGLR algorithm](http://portal.acm.org/citation.cfm?id=1146809.1146810&coll=DL&dl=GUIDE&CFID=9339017&CFTOKEN=49072692).
* [Simple and clear API](http://himedoc.bitbucket.org/v1.1.0/namespaceHime_1_1Redist.html) to manipulate parse trees.
* [Extensive SDK API](http://himedoc.bitbucket.org/v1.1.0/namespaceHime_1_1CentralDogma.html) to programmatically manipulate grammars, generate lexers and parsers and use them.
* Strong emphasis on the separation of data and code. Hime forbids the inclusion of inline code in its grammar definitions in order to have very readable grammars that can be easily understood, debugged, improved. It is still possible to have custom code invoked during parsing with semantic actions.

The parser generator requires the .Net runtime (or Mono) for the generation of parsers. The generated parsers can be used with the corresponding runtime on the following platforms:

* .Net framework 2.0+, or Mono on Linux and MacOS
* Java 7+



### Status ###

[ ![Build Status](https://www.codeship.io/projects/187d0060-f89b-0131-618b-4abf95291133/status?branch=default)](https://www.codeship.io/projects/28719)



### How do I use this software? ###

All user documentation is available in the [wiki](https://bitbucket.org/laurentw/hime/wiki/Home).



### Repository structure ###

* Software components
	* `runtimes`: Contains the sources for the different runtime implementation for the generated parsers:
		* `runtimes/net`: Contains the C# sources for the .Net implementation of the runtime library for the generated parsers.
		* `runtimes/java`: Contains the Java sources for the Java implementation of the runtime library.
	* `core`: Contains the C# sources of the SDK API, i.e. the parser generator.
	* `cli`: Contains the sources for the command-line interfaces of the parser generator, organized by platforms:
		* `cli/net`: Contains the C# sources of the .Net CLI interface of the parser generator.
	* `utilities`: Contains the sources for other helper software components, organized by platform:
		* `utilities/net/demo`: Contains the C# sources of demonstration usage of the SDK API.
		* `utilities/net/benchmark`: Contains the C# sources of the benchmark for the .Net platform.
	* `tests`: Contains the all the test-related software components:
		* `tests/multi`: Contains the common tests for the different runtime implementations.
			* `tests/multi/driver`: Sources of the tests driver for all runtime tests.
			* `tests/multi/net`: Sources of the test executor for the .Net runtime implementation.
			* `tests/multi/java`: Sources of the test executor for the Java runtime implementation.
		* `tests/net`: Contains the tests for other .Net software components.
* Others
	* `packages`: Contains the NuGet dependencies for the .Net software components.
	* `extras`: Contains some extra products, e.g. standard grammars.
	* `releng`: Contains the release engineering artifacts.



### How to build ###

All .Net software components are written in C# and integrated in a single Visual Studio solution file: `HimeSystems.sln` at the repository's root. All software components can be built from the component as follow:

#### Build the runtimes ####

Build the .Net runtime (on the .Net Framework with MSBuild):

```
$ msbuild /p:Configuration=Release runtimes/net/Hime.Redist.csproj
```

Build the .Net runtime (on Mono with xbuild):

```
$ xbuild /p:Configuration=Release runtimes/net/Hime.Redist.csproj
```

Build the Java runtime with Maven:

```
$ mvn -f runtimes/java/pom.xml clean package
```

#### Build the parser generator ####

Building the .Net command line interface for the parser generator will automatically build its required dependencies.

On the .Net Framework with MSBuild:

```
$ msbuild /p:Configuration=Release cli/net/HimeCC.csproj
```

On Mono with xbuild:

```
$ xbuild /p:Configuration=Release cli/net/HimeCC.csproj
```

The results are put in `cli/net/bin/Release`.



### Run the tests ###

#### Run the runtimes tests ####

A common set of tests are executed on all runtime implementation.
The results are output in the JUnit format at `tests/multi/TestResults.xml`.
Running the tests requires a local installation of Mono, xbuild and Maven.

```
$ sh tests/multi/execute.sh
```

On Windows, the script can be run with Cygwin.



### How can I contribute? ###

#### Summary ####

The simplest way to contribute is to:

* Fork this repository on [Bitbucket](https://bitbucket.org/laurentw/hime).
* Fix [some issue](https://bitbucket.org/laurentw/hime/issues?status=new&status=open) or implement a new feature.
* Create a merge request on Bitbucket.

Patches can also be submitted by email, or through the [issue management system](https://bitbucket.org/laurentw/hime/issues). Email contacts:

* [laurentw on Bitbucket](https://bitbucket.org/laurentw): lwouters at xowl dot org.

#### For newcomers ####

The [isse tracker](https://bitbucket.org/laurentw/hime/issues) contains tickets that are accessible to newcomers. Look for tickets with `[beginner]` in the title. These tickets are good ways to become more familiar with the project and the codebase.
