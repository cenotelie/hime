/*******************************************************************************
 * Copyright (c) 2017 Association Cénotélie (cenotelie.fr)
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as
 * published by the Free Software Foundation, either version 3
 * of the License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General
 * Public License along with this program.
 * If not, see <http://www.gnu.org/licenses/>.
 ******************************************************************************/

//! This crate provides the redistributable runtime for Hime-generated parsers
//! It contains the runtime APIs for lexers and parsers generated using [Hime](https://cenotelie.fr/projects/hime).
//! For more information about how to generate parsers using Hime, head to [Hime](https://cenotelie.fr/projects/hime).
//! The code for this library is available on [Bitbucket](https://bitbucket.org/cenotelie/hime).
//! This software is developed by the [Assocation Cénotélie](https://cenotelie.fr/), France.
//!
//! # Usage
//! This crate is [on crates.io](https://crates.io/crates/hime_redist) and can be
//! used by adding `hime_redist` to your dependencies in your project's `Cargo.toml`.
//!
//! ```toml
//! [dependencies]
//! hime_redist = "3.4.0"
//! ```
//!
//! and this to your crate root:
//!
//! ```rust
//! extern crate hime_redist;
//! ```

pub mod utils;
pub mod text;
pub mod errors;
pub mod symbols;
pub mod tokens;
pub mod lexers;
pub mod ast;
pub mod result;
pub mod parsers;
