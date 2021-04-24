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

//! Tests model

use hime_redist::ast::AstNode;
use hime_redist::result::ParseResult;
use hime_redist::symbols::SemanticElementTrait;
use hime_sdk::errors::{Error, Errors};
use hime_sdk::grammars::{Grammar, OPTION_METHOD};
use hime_sdk::{CompilationTask, InputReference, LoadedData, LoadedInput, Mode, Runtime};

/// A fixture definition
pub struct FixtureDef(pub &'static str, pub &'static [u8]);

/// All the test fixtures
pub struct Fixtures(pub Vec<Fixture>);

impl Fixtures {
    /// Build the parsers for the fixtures
    pub fn build(&self) -> Result<(), Errors> {
        let mut grammars: Vec<Grammar> = Vec::new();
        let mut errors: Vec<Error> = Vec::new();
        for (index, fixture) in self.0.iter().enumerate() {
            match fixture.grammars(index) {
                Ok(mut fixture_grammars) => grammars.append(&mut fixture_grammars),
                Err(mut fixture_errors) => errors.append(&mut fixture_errors)
            }
        }
        if !errors.is_empty() {
            return Err(self.build_errors(grammars, errors));
        }

        let mut task = CompilationTask::default();
        task.mode = Some(Mode::Assembly);
        if let Err(errors) = task.prepare_grammars(&mut grammars) {
            return Err(self.build_errors(grammars, errors));
        }

        let units: Vec<(usize, &Grammar)> = grammars.iter().enumerate().collect();
        if let Err(error) = hime_sdk::output::build_assembly(&task, &units, Runtime::Net) {
            return Err(self.build_errors(grammars, vec![error]));
        }
        if let Err(error) = hime_sdk::output::build_assembly(&task, &units, Runtime::Java) {
            return Err(self.build_errors(grammars, vec![error]));
        }
        if let Err(error) = hime_sdk::output::build_assembly(&task, &units, Runtime::Rust) {
            return Err(self.build_errors(grammars, vec![error]));
        }
        Ok(())
    }

    /// Build loaded inputs
    fn get_loaded_inputs(&self) -> Vec<LoadedInput> {
        self.0
            .iter()
            .map(|fixture| LoadedInput {
                name: fixture.name.clone(),
                content: fixture.content.text.clone()
            })
            .collect()
    }

    /// Build errors
    fn build_errors(&self, grammars: Vec<Grammar>, errors: Vec<Error>) -> Errors {
        Errors {
            data: LoadedData {
                inputs: self.get_loaded_inputs(),
                grammars
            },
            errors
        }
    }
}

/// A test fixture
pub struct Fixture {
    /// The fixture's name
    pub name: String,
    /// The inner content for this ficture
    content: ParseResult<'static, 'static>,
    /// The tests in the fixture
    pub tests: Vec<Test>
}

impl Fixture {
    /// Build a fixture from parsed content
    pub fn from_content(content: ParseResult<'static, 'static>) -> Fixture {
        let ast = content.get_ast();
        let root = ast.get_root();
        let name = root.get_value().unwrap();
        let mut tests = Vec::new();
        for (index, node) in root.into_iter().enumerate() {
            tests.push(match node.get_symbol().id {
                crate::loaders::ID_VARIABLE_TEST_OUTPUT => {
                    Test::Output(OutputTest::from_ast(node, index))
                }
                _ => Test::Parsing(ParsingTest::from_ast(node, index))
            });
        }
        Fixture {
            name,
            content,
            tests
        }
    }

    /// Gets the grammars to build for this fixture
    pub fn grammars(&self, index: usize) -> Result<Vec<Grammar>, Vec<Error>> {
        let ast = self.content.get_ast();
        let root = ast.get_root();
        let roots: Vec<(usize, AstNode)> = root
            .into_iter()
            .map(|test_node| (index, test_node.child(1)))
            .collect();
        let mut grammars = hime_sdk::loaders::load_parsed(&roots)?;
        for (grammar, test_node) in grammars.iter_mut().zip(root.into_iter()) {
            let method_node = test_node.child(2);
            let reference = InputReference::from(index, &method_node);
            grammar.add_option(
                reference,
                reference,
                OPTION_METHOD.to_string(),
                method_node.get_value().unwrap().to_lowercase()
            )
        }
        Ok(grammars)
    }
}

/// A test in a fixture
pub enum Test {
    /// An output test
    Output(OutputTest),
    /// A parsing test
    Parsing(ParsingTest)
}

/// An output test
pub struct OutputTest {
    /// The test name
    pub name: String,
    /// The index within the fixture
    pub index: usize
}

impl OutputTest {
    pub fn from_ast(node: AstNode, index: usize) -> OutputTest {
        let name = node.child(0).get_value().unwrap();
        OutputTest { name, index }
    }
}

/// A parsing test
pub struct ParsingTest {
    /// The test name
    pub name: String,
    /// The index within the fixture
    pub index: usize
}

impl ParsingTest {
    pub fn from_ast(node: AstNode, index: usize) -> ParsingTest {
        let name = node.child(0).get_value().unwrap();
        ParsingTest { name, index }
    }
}
