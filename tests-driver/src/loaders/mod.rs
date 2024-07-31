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

//! Loaders for test data

use std::io::BufReader;

#[allow(dead_code)]
mod fixture;

pub use fixture::{ID_VARIABLE_TEST_MATCHES, ID_VARIABLE_TEST_NO_MATCH, ID_VARIABLE_TEST_OUTPUT};
use hime_redist::ast::AstImpl;
use hime_redist::errors::ParseErrorDataTrait;
use hime_redist::result::{ParseResult, ParseResultAst};
use hime_sdk::errors::{Error, Errors};
use hime_sdk::{InputReference, LoadedData, LoadedInput};

use crate::model::{Fixture, FixtureDef, Fixtures};

/// Parses all fixtures
pub fn parse_fixtures(fixtures: &[FixtureDef]) -> Result<Fixtures, Errors> {
    let results: Vec<ParseResultAst> = fixtures
        .iter()
        .map(|FixtureDef(_, content)| {
            let mut reader = BufReader::new(*content);
            fixture::parse_utf8_stream(&mut reader)
        })
        .collect();
    let is_ok = results.iter().all(ParseResult::<AstImpl>::is_success);
    if is_ok {
        Ok(Fixtures(results.into_iter().map(Fixture::from_content).collect()))
    } else {
        let errors: Vec<Error> = results
            .iter()
            .enumerate()
            .flat_map(|(index, r)| {
                r.errors.errors.iter().map(move |error| {
                    let position = error.get_position();
                    Error::Parsing(
                        InputReference {
                            input_index: index,
                            position,
                            length: error.get_length(),
                        },
                        error.to_string(),
                    )
                })
            })
            .collect();
        Err(Errors {
            context: LoadedData {
                inputs: results
                    .into_iter()
                    .zip(fixtures.iter())
                    .map(|(r, FixtureDef(name, _))| LoadedInput {
                        name: (*name).to_string(),
                        content: r.text,
                    })
                    .collect(),
                grammars: Vec::new(),
            },
            errors,
        })
    }
}
