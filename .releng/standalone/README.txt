Hime Parser Generator
https://github.com/cenotelie/hime/

The Hime parser generator is a parser generator for the .Net platform (including Mono on Linux and MacOS) and Java.
It primarily supports the LR family of parsing methods, including GLR (Generalized-LR).
Parsers generated with this tool need the appropriate runtime contained in this package.

This package is a standalone distribution of the binaries for the .Net and Java platforms. It contains:

* nuget: contains the NuGet packages for
    * Hime.Redist, the redistributable runtime for generated parsers on .Net. This package supports the .Net Standard 1.0, 2.0 and .Net Framework 2.0 and up.
    * Hime.SDK, the parser generator SDK. This package supports the .Net Standard 2.0 and .Net Framework 2.0 and up.
* net20: contains the .Net artifacts specific to the .Net Framework 2.0 implementation (and up).
    * Hime.Redist.dll, the redistributable runtime for generated parsers on .Net.
    * Hime.SDK.dll, the parser generator SDK.
    * himecc.exe, the parser generator command line.
    * parseit.exe, the command-line parser of input text.
* net461: contains the .Net artifacts specific to the .Net Framework 4.6.1 implementation (and up).
    * Hime.Redist.dll, the redistributable runtime for generated parsers on .Net.
    * Hime.SDK.dll, the parser generator SDK.
    * himecc.exe, the parser generator command line.
    * parseit.exe, the command-line parser of input text.
* netcore20: contains the .Net artifacts specific to the .Net Core 2.0 implementation.
    * Hime.Redist.dll, the redistributable runtime for generated parsers on .Net.
    * Hime.SDK.dll, the parser generator SDK.
    * himecc.dll, the parser generator command line.
    * parseit.dll, the command-line parser of input text.
* java: contains the Java artifacts
    * hime-redist.jar, the redistributable runtime for generated parsers on Java.
* rust: contains the artifacts for Rust
    * hime_redist.crate, the redistributable runtime for the generated parsers in Rust.

* himecc: Linux frontend for the himecc parser generator command line. This programs switches between a Mono runtime, or a .Net Core runtime.
* himecc.bat: Windows frontend for the himecc parser generator command line.
* parseit: Linux frontend for the parseit command line. This programs switches between a Mono runtime, or a .Net Core runtime.
* parseit.bat: Windows frontend for the parseit command line.