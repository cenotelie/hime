# Hime Rust SDK #

The Hime Rust SDK for the generation of LR and GLR parsers.
For more information about how to generate parsers using Hime, head to [Hime](https://cenotelie.fr/projects/hime).
The code for this library is available on [GitHub](https://github.com/cenotelie/hime).
The API documentation is available on [docs.rs](https://docs.rs/hime_sdk/latest/hime_sdk/).
This software is developed by the [Assocation Cénotélie](https://cenotelie.fr/), France.

## Usage ##

This crate is [on crates.io](https://crates.io/crates/hime_sdk) and can be
used by adding `hime_sdk` to your dependencies in your project's `Cargo.toml`.

```toml
[dependencies]
hime_sdk = "4.0.0"
```

Generated lexer and parser codes will required the [associated runtime](https://crates.io/crates/hime_redist),
and provide a simple API to parse input text.

## How can I contribute? ##

The simplest way to contribute is to:

* Fork this repository on [GitHub](https://github.com/cenotelie/hime).
* Fix [some issue](https://github.com/cenotelie/hime/issues?status=new&status=open) or implement a new feature.
* Create a merge request on GitHub.

Patches can also be submitted by email, or through the [issue management system](https://github.com/cenotelie/hime/issues).

The [isse tracker](https://github.com/cenotelie/hime/issues) contains tickets that are accessible to newcomers. Look for tickets with `[beginner]` in the title. These tickets are good ways to become more familiar with the project and the codebase.

## License ##

This software is available under the terms of the [Apache License 2.0](https://www.apache.org/licenses/LICENSE-2.0).
