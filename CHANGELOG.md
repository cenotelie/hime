# Updates

## 3.2.1

To be released

* Features
    * [Issue #37](https://bitbucket.org/cenotelie/hime/issues/37): New API in .Net and Java runtimes to distinguish between types of symbols attached to AST nodes.

## 3.2.0

Released on October 4th, 2017.

* Features
    * [Issue #36](https://bitbucket.org/cenotelie/hime/issues/36): .Net Runtime: Changed build targets to .Net Standard 1.0, 2.0 and .Net Framework 2.0
    * [Issue #36](https://bitbucket.org/cenotelie/hime/issues/36): .Net SDK: Changed build targets to .Net Standard 2.0 and .Net Framework 2.0
    * [Issue #36](https://bitbucket.org/cenotelie/hime/issues/36): Himecc CLI: Changed build target to .Net Core 2.0, .Net Framework 2.0 and .Net Framework 4.6.1
    * Added parseit command-line tool to parse pieces of input text

## 3.1.0

Released on September 26th, 2017.

* Features
    * Support for the specification of grammar compilation options directly in the grammar in the options section.

## 3.0.1

Released on August 3rd, 2017.

* Features
    * [Issue #35](https://bitbucket.org/cenotelie/hime/issues/35): Support for Unicode 10 character classes and blocks

## 3.0.0

Released on May 4th, 2017.

* Fixes
    * Updated manifest for the Java runtime with new organization (moved to Cénotélie).
    * Updated the copyright of all artifacts to Association Cénotélie.
    * Updated Maven group id to fr.cenotelie (breaking API compatibility).

## 2.0.6

Released on February 7th, 2017. This is a partial release of the Java runtime alone.

* Fixes
    * Updated manifest for the Java runtime.

## 2.0.5

Released on September 10th, 2016.

* Features
    * [Issue #33](https://bitbucket.org/cenotelie/hime/issues/33): Support for Unicode 9 character classes and blocks
* Fixes 
    * [Issue #34](https://bitbucket.org/cenotelie/hime/issues/34): Misconstructed AST on RNGLR with pathological ambiguities and tree actions
    * .Net Runtime: Use .Net 4.5 as build target (instead of 2.0)
    * Java Runtime: Use Java 7 as build target (instead of 6)

## 2.0.4

Released on March 29th, 2016.

* Fixes
    * [Issue #32](https://bitbucket.org/cenotelie/hime/issues/32): Fail to execute parser actions for LR(k) parsers

## 2.0.3

Released on March 1st, 2016. This is a partial release of the Java runtime alone.

* Fixes
    * Bug in Java runtime with non-termination on error at the input's end

## 2.0.2

Released on January 20th, 2016. This is a partial release of the Java runtime alone.

* Fixes
    * Support the deployment of the Hime java runtime as an OSGi bundle

## 2.0.1

Released on October 25th, 2015.

* Features
    * [Issue #3](https://bitbucket.org/cenotelie/hime/issues/3): Support of context-sensitive lexing
    * [Issue #12](https://bitbucket.org/cenotelie/hime/issues/12): Renamed the SDK API namespace
    * [Issue #17](https://bitbucket.org/cenotelie/hime/issues/17): Cleaning-up of the Redist API to mask implementation details
    * [Issue #18](https://bitbucket.org/cenotelie/hime/issues/18): Detection of incorrect UTF-16 encoding sequences
    * [Issue #25](https://bitbucket.org/cenotelie/hime/issues/25): Better handling of lexical errors with a fuzzy matching
    * [Issue #30](https://bitbucket.org/cenotelie/hime/issues/30): Support of Unicode 8
    * Support of terminal fragments (non-matching lexical rules)
* Fixes
    * [Issue #29](https://bitbucket.org/cenotelie/hime/issues/29): Clashes of symbol names on code generation
    * Efficiency issues on the Java runtime while traversing a result AST
    * Efficiency issues on the Java runtime with list of integers
    * Bug in getting the substring of a streaming text in Java

## 1.3.2

Released on January 22nd, 2015.

* Fixes
    * [Issue #21](https://bitbucket.org/cenotelie/hime/issues/21): Duplication of nullable variables in AST produced by RNGLR parsers in some corner cases
    * [Issue #22](https://bitbucket.org/cenotelie/hime/issues/22): Wrong AST generation by RNGLR parsers due to faulty reuse of sub-AST
    * [Issue #23](https://bitbucket.org/cenotelie/hime/issues/23): Buffer overflow in the SPPF builder of RNGLR parsers
    * [Issue #24](https://bitbucket.org/cenotelie/hime/issues/24): Buffer overflow on sub-SPPF on large reductions (Java only)
    * [Issue #26](https://bitbucket.org/cenotelie/hime/issues/26): Removed dependency on Mono for the executions of tests on Windows
    * [Issue #27](https://bitbucket.org/cenotelie/hime/issues/27): Parsers only reporting expecting terminals triggering shift actions, missing some actually possible terminals
    * [Issue #28](https://bitbucket.org/cenotelie/hime/issues/28): The use of reserved Java keyword as a symbol in a grammar leads to compilation errors in the generated Java code
    * Error in the generated Java code referencing the binary resource of a parser
    * Some typos in the API documentation

## 1.3.1

Released on October 23rd, 2014.

* Fixes
    * Bug with overflowing handle in LR(k) AST builder
    * [Issue #10](https://bitbucket.org/cenotelie/hime/issues/10): Bug with overflowing history buffers in RNGLR parser
    * [Issue #11](https://bitbucket.org/cenotelie/hime/issues/11): Bug with failing special entry in .Net tests
    * [Issue #13](https://bitbucket.org/cenotelie/hime/issues/13): Bug with non-explicit exception when a required embedded resource is missing
    * [Issue #19](https://bitbucket.org/cenotelie/hime/issues/19): Bug with missing diagnostic on terminals that are used but cannot be produced by the lexer
    * [Issue #20](https://bitbucket.org/cenotelie/hime/issues/20): Bug with overflowing stack in LR(k) parsers
    Minor typos in the API

## 1.3.0

Released on September 16th, 2014.

* Features
    * [Issue #8](https://bitbucket.org/cenotelie/hime/issues/8): Strong name signing of the .Net artifacts
    * Refactored the Hime.Redist.Text.GetContext() API to return a Context object
    * Improved debug printing of GSS stacks (RNGLR parsers)
    * Improved the string representation in error messages of the end of input markers
* Fixes
    * [Issue #7](https://bitbucket.org/cenotelie/hime/issues/7): Bug with lexers not handling well error reporting on the last line when it is empty
    * [Issue #9](https://bitbucket.org/cenotelie/hime/issues/9): Bug in RNGLR parsers producing incorrect parse trees

## 1.2.0

Released on August 18th, 2014.

* Features:
    * Support of Unicode 7 character blocks and character classes
    * Support of Unicode escape sequence in literal strings in grammars
    * Support of multiple parsers in the same generated assembly
    * Support of pre-parsed grammars as inputs to the SDK
    * Support of custom reporters in CompilationTask
* Fixes:
    * Bug in grammar inheritance
    * Bug in Java code generation
    * Bug in error positioning and context generation

## 1.1.0

Released on May 29th, 2014.

## 1.0.0

May 12th, 2014, initial release of Hime.