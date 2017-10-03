# README #

The Hime parser generator is a parser generator for the .Net and Java platforms. It primarily supports the LR family of parsing methods, including GLR (Generalized-LR). Key distinguishing features are:

* Fast LR and GLR parsing.
* Modern implementation of GLR with the [RNGLR algorithm](http://portal.acm.org/citation.cfm?id=1146809.1146810&coll=DL&dl=GUIDE&CFID=9339017&CFTOKEN=49072692).
* [Simple and clear API](https://cenotelie.fr/hime/api-net/v3.1.0/namespaceHime_1_1Redist.html) to manipulate parse trees.
* [Extensive SDK API](https://cenotelie.fr/hime/api-net/v3.1.0/namespaceHime_1_1SDK.html) to programmatically manipulate grammars, generate lexers and parsers and use them.
* Strong emphasis on the separation of data and code. Hime forbids the inclusion of inline code in its grammar definitions in order to have very readable grammars that can be easily understood, debugged, improved. It is still possible to have custom code invoked during parsing with semantic actions.

The generated parsers can run on:

* Any implementation of [.Net Standard](https://docs.microsoft.com/en-us/dotnet/standard/net-standard) 1.0, including the [.Net Framework](https://www.microsoft.com/net/download/framework) 4.5 and up (installed by default on Windows 8 and up), [.Net Core](https://www.microsoft.com/net/core) 1.0 and up, [Mono](http://www.mono-project.com/) 4.6 and up, etc.
* [.Net Framework](https://www.microsoft.com/net/download/framework) 2.0 and up (installed by default on Windows Vista and up); or compatible implementations such as [Mono](http://www.mono-project.com/).

* Java 7 and up

The parser generator (himecc) can run on:

* Any implementation of [.Net Standard](https://docs.microsoft.com/en-us/dotnet/standard/net-standard) 2.0, including the [.Net Framework](https://www.microsoft.com/net/download/framework) 4.6.1 and up (installed by default on Windows 10), [.Net Core](https://www.microsoft.com/net/core) 2.0 and up, [Mono](http://www.mono-project.com/) 5.2 and up, etc.
* [.Net Framework](https://www.microsoft.com/net/download/framework) 2.0 and up (installed by default on Windows Vista and up); or compatible implementations such as [Mono](http://www.mono-project.com/).
* For the production of compiled parser assemblies in Java, Maven is required.


## Products ##

* [.Net Runtime on NuGet](https://www.nuget.org/packages/Hime.Redist/)
* [.Net SDK on NuGet](https://www.nuget.org/packages/Hime.Redist/)
* [Java Runtime on Maven Central](http://search.maven.org/#search%7Cga%7C1%7Cg%3A%22fr.cenotelie.hime%22)
* [Downloadable distribution](https://bitbucket.org/cenotelie/hime/downloads/), including all runtimes and the generator (himecc).

## How do I use this software? ##

* [Kickstart in 5 mins](http://cenotelie.fr/hime/kickstart.html)
* [Complete user documentation](http://cenotelie.fr/hime/index.html)
* [Grammar library](https://bitbucket.org/cenotelie/hime-grams)


## Repository structure ##

* Software components
	* `runtime-net`: Contains the C# sources for the .Net implementation of the runtime library for the generated parsers.
	* `runtime-java`: Contains the Java sources for the Java implementation of the runtime library.
	* `sdk`: Contains the C# sources of the SDK API, i.e. the parser generator.
	`himecc`: Contains the C# sources of the .Net CLI interface of the parser generator (himecc).
	* `tests-driver`: Sources of the tests driver for all runtime tests.
	* `tests-executor-net`: Sources of the test executor for the .Net runtime implementation.
	* `tests-executor-java`: Sources of the test executor for the Java runtime implementation.
* Others
	* `.assets`: Contains some extra products, e.g. standard grammars.
	* `.releng`: Contains the release engineering artifacts.


## How to build ##

To build the code and execute all tests, run

```
$ sh .releng/build.sh
```

To build all the release artifacts (requires a GPG key), run

```
$ sh .releng/release.sh
```

This requires:

* A local installation of [Mono](http://www.mono-project.com/) 4.4, or up (preferably 5.2)
* A local installation of [.Net Core SDK](https://www.microsoft.com/net/download/core) 2.0, or up
* Java 7 and Maven 3.3

During the build, a common set of tests are executed on all runtime implementations.
The results are output in the JUnit format in `TestResults.xml` at the repository's root.

> **Warning**: The build process on Windows is not supported at this time.

## How can I contribute? ##

The simplest way to contribute is to:

* Fork this repository on [Bitbucket](https://bitbucket.org/cenotelie/hime).
* Fix [some issue](https://bitbucket.org/cenotelie/hime/issues?status=new&status=open) or implement a new feature.
* Create a merge request on Bitbucket.

Patches can also be submitted by email, or through the [issue management system](https://bitbucket.org/cenotelie/hime/issues).

The [isse tracker](https://bitbucket.org/cenotelie/hime/issues) contains tickets that are accessible to newcomers. Look for tickets with `[beginner]` in the title. These tickets are good ways to become more familiar with the project and the codebase.
