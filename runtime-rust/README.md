# Hime Rust Runtime #

The Rust implementation of the runtime for lexers and parsers generated with [Hime](https://bitbucket.org/cenotelie/hime).
For more information about how to generate parsers using Hime, head to [Hime](https://cenotelie.fr/projects/hime).
The code for this library is available on [Bitbucket](https://bitbucket.org/cenotelie/hime).
The API documentation is available on [docs.rs](https://docs.rs/hime_redist/3.4.2/hime_redist/).
This software is developed by the [Assocation Cénotélie](https://cenotelie.fr/), France.

## Usage ##

This crate is [on crates.io](https://crates.io/crates/hime_redist) and can be
used by adding `hime_redist` to your dependencies in your project's `Cargo.toml`.

```toml
[dependencies]
hime_redist = "3.4.2"
```

Generated lexer and parser codes will import this crate and provide a simple API to parse input text.

## How can I contribute? ##

The simplest way to contribute is to:

* Fork this repository on [Bitbucket](https://bitbucket.org/cenotelie/hime).
* Fix [some issue](https://bitbucket.org/cenotelie/hime/issues?status=new&status=open) or implement a new feature.
* Create a merge request on Bitbucket.

Patches can also be submitted by email, or through the [issue management system](https://bitbucket.org/cenotelie/hime/issues).

The [isse tracker](https://bitbucket.org/cenotelie/hime/issues) contains tickets that are accessible to newcomers. Look for tickets with `[beginner]` in the title. These tickets are good ways to become more familiar with the project and the codebase.

## License ##

This software is licenced under the Lesser General Public License (LGPL) v3.
Refers to the `LICENSE.txt` file at the root of the repository for the full text, or to [the online version](http://www.gnu.org/licenses/lgpl-3.0.html).