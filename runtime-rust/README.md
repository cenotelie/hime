# Hime Rust Runtime #

The Rust implementation of the runtime for lexers and parsers generated with [Hime](https://github.com/cenotelie/hime).
For more information about how to generate parsers using Hime, head to [Hime](https://cenotelie.fr/projects/hime).
The code for this library is available on [Github](https://github.com/cenotelie/hime).
The API documentation is available on [docs.rs](https://docs.rs/hime_redist/latest/hime_redist/).
This software is developed by the [Assocation Cénotélie](https://cenotelie.fr/), France.

## Usage ##

This crate is [on crates.io](https://crates.io/crates/hime_redist) and can be
used by adding `hime_redist` to your dependencies in your project's `Cargo.toml`.

```toml
[dependencies]
hime_redist = "4.3"
```

Generated lexer and parser codes will import this crate and provide a simple API to parse input text.

## Support for `no_std`

As of version `4.3.0`, this crate supports `no_std` contexts.
This crate has an `std` feature which is activated by default for retro-compatibility but can deactivated as follow:

```toml
[dependencies]
hime_redist = { version = "4.3", default-features = false }
```

The only dependency of this crate (`serde`) also does not require `std` support, and will only use its `std` feature when the `std` feature of this crate is activated (which it is by default).
De-activating `std` for this crate also de-activates `std` for serde.

## How can I contribute? ##

The simplest way to contribute is to:

* Fork this repository on [GitHub](https://github.com/cenotelie/hime).
* Fix [some issue](https://github.com/cenotelie/hime/issues?status=new&status=open) or implement a new feature.
* Create a merge request on GitHub.

Patches can also be submitted by email, or through the [issue management system](https://github.com/cenotelie/hime/issues).

The [isse tracker](https://github.com/cenotelie/hime/issues) contains tickets that are accessible to newcomers. Look for tickets with `[beginner]` in the title. These tickets are good ways to become more familiar with the project and the codebase.

## License ##

This software is available under the terms of the [Apache License 2.0](https://www.apache.org/licenses/LICENSE-2.0).
