# README #

[![Build Status](https://dev.azure.com/cenotelie/cenotelie/_apis/build/status%2Fcenotelie.hime?branchName=master)](https://dev.azure.com/cenotelie/cenotelie/_build/latest?definitionId=6&branchName=master)
[![Crates.io](https://img.shields.io/crates/v/hime_redist.svg)](https://crates.io/crates/hime_redist)

Hime is a parser generator that targets .Net, Java and Rust.
Hime relies on the [LR](https://en.wikipedia.org/wiki/LR_parser)
family of parsing techniques, including the state of the art
[RNGLR algorithm]((http://portal.acm.org/citation.cfm?id=1146809.1146810&coll=DL&dl=GUIDE&CFID=9339017&CFTOKEN=49072692)) for generalized LR parsing used for
ambiguous grammars. Hime provides a powerful, expressive,
and feature-rich grammar language with support for
[template syntactic rules](http://cenotelie.fr/projects/hime/referenceLangTemplates),
[context-sensitive lexing](http://cenotelie.fr/projects/hime/referenceLangContextSensitive)
(useful for contextal keywords),
[tree actions](http://cenotelie.fr//projects/hime/referenceLangTreeActions)
(useful for clean syntax trees), and more!
Hime strongly emphasizes the separation of data and code. Hime forbids the inclusion of inline code in its grammar definitions in order to have very readable grammars that can be easily understood, debugged, improved. It is still possible to have custom code invoked during parsing with semantic actions.

The generated parsers require a runtime, available for the following platforms:

* [.Net framework](https://www.nuget.org/packages/Hime.Redist/)
* [JVM](https://central.sonatype.com/artifact/fr.cenotelie.hime/hime-redist)
* [Rust](https://crates.io/crates/hime_redist)

## Parser generator

The parser generator (himecc) has native builds for Windows, MacOS and Linux.
* [Windows](https://cenotelie.s3.fr-par.scw.cloud/hime/stable/windows/himecc.exe)
* [Linux](https://cenotelie.s3.fr-par.scw.cloud/hime/stable/linux-musl/himecc)
* [MacOS](https://cenotelie.s3.fr-par.scw.cloud/hime/stable/macos/himecc)

Alternatively, himecc can be built and installed from sources with cargo :

```bash
cargo install hime_compiler
```

See all download options in [the download page of the doc](https://cenotelie.fr/projects/hime/download).

## How do I use this software? ##

* [Complete user documentation](https://cenotelie.fr/projects/hime)
* [Grammar library](https://github.com/cenotelie/hime-grams)


## License

All software is available under the terms of the [Apache License 2.0](https://www.apache.org/licenses/LICENSE-2.0).

## Repository structure ##

* Runtime implementations
	* `runtime-net`: .Net implementation of the runtime.
	* `runtime-java`: Java implementation of the runtime.
	* `runtime-rust`: Rust implementation of the runtime.
* SDK
	* `sdk-rust`: Rust implementation of the SDK for parser generation.
	* `sdk-debugger`: CLI tool to inspect and debug generated binary automata.
	* `sdk-unicode-gen`: CLI tool to generate the code to be put in `sdk-rust` related to unicode blocks and categories.
* Final tools
	* `himecc`: Rust implementation of the parser generator CLI.
	* `parseit`: Rust implementation of the tool for parsing bits of texts using a previously generated parser assembly.
	* `langserv`: Language server for the Hime grammar language.
* Tests
	* `tests-driver`: Sources of the tests driver for all runtime tests.
	* `tests-executor-net`: Sources of the test executor for the .Net runtime implementation.
	* `tests-executor-java`: Sources of the test executor for the Java runtime implementation.
	* `tests-executor-rust`: Sources of the test executor for the Rust runtime implementation.
* Others
	* `.assets`: Contains some extra products, e.g. standard grammars.
	* `.releng`: Contains the release engineering artifacts.


## How to build ##

To build the code and execute all tests, run

```
$ sh build.sh
```

Note that the development environment is fully dockerized and executing this command requires docker and will pull the required docker image if not locally available.


## How can I contribute? ##

The simplest way to contribute is to:

* Fork this repository on [GitHub](https://github.com/cenotelie/hime).
* Fix [some issue](https://github.com/cenotelie/hime/issues?status=new&status=open) or implement a new feature.
* Create a merge request on GitHub.

Patches can also be submitted by email, or through the [issue management system](https://github.com/cenotelie/hime/issues).

The [isse tracker](https://github.com/cenotelie/hime/issues) contains tickets that are accessible to newcomers. Look for tickets with `[beginner]` in the title. These tickets are good ways to become more familiar with the project and the codebase.
