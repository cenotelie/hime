/*******************************************************************************
 * Copyright (c) 2018 Association Cénotélie (cenotelie.fr)
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

//! Module for the definition of Hime grammars

pub mod symbols;
pub mod rules;

/// The identifier of a grammar symbol
pub type SymbolId = usize;

/// The prefix for the generated terminal names
pub const PREFIX_GENERATED_TERMINAL: &'static str = "__T";

/// The prefix for the generated variable names
pub const PREFIX_GENERATED_VARIABLE: &'static str = "__V";

/// The name of the generated axiom variable
pub const GENERATED_AXIOM: &'static str = "__VAxiom";

/// Name of the grammar option specifying the grammar's axiom variable
pub const OPTION_AXIOM: &'static str = "Axiom";

/// Name of the grammar option specifying the grammar's separator terminal
pub const OPTION_SEPARATOR: &'static str = "Separator";

/// The output path for compilation artifacts
pub const OPTION_OUTPUT_PATH: &'static str = "OutputPath";

/// The compilation mode to use, defaults to Source
pub const OPTION_COMPILATION_MODE: &'static str = "CompilationMode";

/// The parser type to generate, defaults to LALR1
pub const OPTION_PARSER_TYPE: &'static str = "ParserType";

/// The runtime to target, defaults to Net
pub const OPTION_RUNTIME: &'static str = "Runtime";

/// The namespace to use for the generated code
pub const OPTION_NAMESPACE: &'static str = "Namespace";

/// The access mode for the generated code, defaults to Internal
pub const OPTION_ACCESS_MODIFIER: &'static str = "AccessModifier";

/// The name of the default lexical context
pub const DEFAULT_CONTEXT_NAME: &'static str = "__default";
