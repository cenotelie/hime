/*******************************************************************************
 * Copyright (c) 2021 Association Cénotélie (cenotelie.fr)
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

//! Main module

use hime_sdk::errors::Errors;
use model::FixtureDef;

mod loaders;
mod model;

/// The name of this program
pub const CRATE_NAME: &str = env!("CARGO_PKG_NAME");
/// The version of this program
pub const CRATE_VERSION: &str = env!("CARGO_PKG_VERSION");
/// The commit that was used to build the application
pub const GIT_HASH: &str = env!("GIT_HASH");
/// The git tag that was used to build the application
pub const GIT_TAG: &str = env!("GIT_TAG");

const FIXTURES: [FixtureDef; 8] = [
    FixtureDef(
        "ContextSensitive",
        include_bytes!("fixtures/ContextSensitive.suite")
    ),
    FixtureDef("Errors", include_bytes!("fixtures/Errors.suite")),
    FixtureDef(
        "GrammarOptions",
        include_bytes!("fixtures/GrammarOptions.suite")
    ),
    FixtureDef(
        "LexicalRules",
        include_bytes!("fixtures/LexicalRules.suite")
    ),
    FixtureDef("Regressions", include_bytes!("fixtures/Regressions.suite")),
    FixtureDef(
        "SyntacticRules",
        include_bytes!("fixtures/SyntacticRules.suite")
    ),
    FixtureDef("TreeActions", include_bytes!("fixtures/TreeActions.suite")),
    FixtureDef(
        "UnicodeBlocks",
        include_bytes!("fixtures/UnicodeBlocks.suite")
    )
];

/// Main entry point
fn main() {
    let args = std::env::args().collect::<Vec<_>>();
    let filter = match &args[..] {
        [_] => None,
        [_, filter] => Some(filter.as_str()),
        _ => {
            println!("Usage is: hime_tests_driver <filter>");
            return;
        }
    };

    println!("Loading ...");
    let fixtures = on_errors(loaders::parse_fixtures(&FIXTURES));
    println!("Building ...");
    on_errors(fixtures.build(filter));
    println!("Executing tests ...");
    match fixtures.execute(filter) {
        Ok(results) => {
            let stats = results.get_stats();
            println!(
                "{} successes, {} failures, {} errors / {} total",
                stats.success,
                stats.failure,
                stats.error,
                stats.total()
            );
            if let Err(e) = results.export_xml() {
                println!("{:?}", e);
            }
        }
        Err(e) => {
            println!("{:?}", e);
        }
    }
}

/// Handle errors
fn on_errors<T>(result: Result<T, Errors>) -> T {
    match result {
        Ok(r) => r,
        Err(errors) => {
            hime_sdk::errors::print::print_errors(&errors);
            std::process::exit(1);
        }
    }
}
