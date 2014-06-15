# README #

This directory contains the components for testing the multiple runtime implementations against a common set of tests.
This testing framework is designed as follow:

* `driver` is a component that defines the common tests for all runtimes and manages the execution of their execution. It relies on runtime-specific executors.
* `net` is the test executor for the .Net runtime implementation. It executes tests provided by the driver on the .Net runtime implementation.
* `java` is the test executor for the Java runtime implementation. It executes tests provided by the driver on the Java runtime implementation.

The common tests are specified in suites located in `driver/Resources/Suites`. The suites are expressed in a specialized DSL, which grammar is specified in `driver/Resources/ParsingFixture.gram`.
A single test is usually expressed as follow:

```
test Test_Minimal_Single_MatchOne_LR:
	// The grammar to test, note that the grammar's name is the same as the test's name
	grammar Test_Minimal_Single_MatchOne_LR { options {Axiom="e";} terminals {A->'a';} rules { e->A; } }
	// Specifies the parsing method to test, here LALR(1), for RNGLR, the value RNGLALR1 is used
	parser LALR1
	// Specifies the input to test in a quoted string
	on "a"
	// Specifies the AST that must be produced, here a root node named 'e', with a child node named 'A' with the value 'a'.
	yields e(A='a')
```

The results are output in the JUnit format at `tests/multi/TestResults.xml`.



### Requirements ###

* .Net Framework 2.0 or upper or Mono
* Java JDK 7 or upper
* MSBuild or xbuild for Mono
* Maven 3 or upper


### How to run ###

Run the following command from the repository's root:

```
$ sh tests/multi/execute.sh
```

On Windows, the script can be run with Cygwin.
