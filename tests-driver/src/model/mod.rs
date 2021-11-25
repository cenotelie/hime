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

pub mod results;

use std::fs::{self, File};
use std::io::{BufWriter, Write};
use std::process::Command;
use std::time::Instant;

use hime_redist::ast::AstNode;
use hime_redist::result::ParseResult;
use hime_redist::symbols::SemanticElementTrait;
use hime_sdk::errors::{Error, Errors};
use hime_sdk::grammars::{BuildData, Grammar, OPTION_METHOD};
use hime_sdk::output::helper::{to_snake_case, to_upper_case};
use hime_sdk::{CompilationTask, InputReference, LoadedData, LoadedInput, Runtime};
use results::{TestResultOnRuntime, TestResultStatus};

use self::results::TestResult;

/// A fixture definition
pub struct FixtureDef(pub &'static str, pub &'static [u8]);

/// All the test fixtures
pub struct Fixtures(pub Vec<Fixture>);

impl Fixtures {
    /// Build the parsers for the fixtures
    pub fn build(&self) -> Result<(), Errors> {
        let mut errors: Vec<Error> = Vec::new();

        // Load all grammars
        let mut grammars: Vec<Grammar> = Vec::new();
        for (index, fixture) in self.0.iter().enumerate() {
            match fixture.grammars(index) {
                Ok(mut fixture_grammars) => grammars.append(&mut fixture_grammars),
                Err(mut fixture_errors) => errors.append(&mut fixture_errors)
            }
        }
        let mut grammars_data = Vec::new();
        // Build all parser data
        for (index, grammar) in grammars.iter_mut().enumerate() {
            match grammar.build(None, index) {
                Ok(data) => grammars_data.push(data),
                Err(mut errs) => errors.append(&mut errs)
            }
        }

        if errors.is_empty() {
            println!("Building Rust ...");
            self.build_rust(&grammars, &grammars_data, &mut errors);
        }
        if errors.is_empty() {
            Ok(())
        } else {
            Err(self.build_errors(grammars, errors))
        }
    }

    /// Build the Rust parsers for the fixtures
    fn build_rust(
        &self,
        grammars: &[Grammar],
        grammars_data: &[BuildData],
        errors: &mut Vec<Error>
    ) {
        let temp_dir = hime_sdk::output::temporary_folder();
        fs::create_dir_all(&temp_dir).unwrap();
        let mut path = std::env::current_dir().unwrap();
        path.push("runtime-rust");

        let mut task = CompilationTask::default();
        task.output_target = Some(Runtime::Rust);
        task.output_path = Some(temp_dir.to_str().unwrap().to_string());
        task.output_target_runtime_path = path.to_str().map(|s| s.to_string());

        // Build all parser data
        for (index, (grammar, data)) in grammars.iter().zip(grammars_data.iter()).enumerate() {
            if let Err(mut errs) =
                hime_sdk::output::output_grammar_artifacts(&task, grammar, index, &data)
            {
                errors.append(&mut errs);
            }
        }

        let units: Vec<(usize, &Grammar)> = grammars.iter().enumerate().collect();
        hime_sdk::output::build_assembly(&task, &units, Runtime::Rust).unwrap();
    }

    /// Build the .Net parsers for the fixtures
    fn build_net(&self) -> Result<(), Errors> {
        let mut errors: Vec<Error> = Vec::new();
        let mut task = CompilationTask::default();
        let temp_dir = hime_sdk::output::temporary_folder();
        fs::create_dir_all(&temp_dir).unwrap();
        task.output_path = Some(temp_dir.to_str().unwrap().to_string());
        println!("Write to: {:?}", &temp_dir);

        // Load all grammars
        let mut grammars: Vec<Grammar> = Vec::new();
        for (index, fixture) in self.0.iter().enumerate() {
            match fixture.grammars(index) {
                Ok(mut fixture_grammars) => grammars.append(&mut fixture_grammars),
                Err(mut fixture_errors) => errors.append(&mut fixture_errors)
            }
        }

        // Build all parser data
        for (index, grammar) in grammars.iter_mut().enumerate() {
            match grammar.build(None, index) {
                Ok(data) => {
                    if let Err(mut errs) =
                        hime_sdk::output::output_grammar_artifacts(&task, grammar, index, &data)
                    {
                        errors.append(&mut errs);
                    }
                }
                Err(mut errs) => errors.append(&mut errs)
            }
        }

        let units: Vec<(usize, &Grammar)> = grammars.iter().enumerate().collect();

        // Build .Net
        let mut path = std::env::current_dir().unwrap();
        path.push("runtime-net/bin/Release/netstandard2.0");
        task.output_target_runtime_path = path.to_str().map(|s| s.to_string());
        hime_sdk::output::build_assembly(&task, &units, Runtime::Net).unwrap();

        if errors.is_empty() {
            Ok(())
        } else {
            Err(self.build_errors(grammars, errors))
        }
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

    /// Execute this fixture
    pub fn execute(&self) -> Result<(), Error> {
        for fixture in self.0.iter() {
            fixture.execute()?;
        }
        Ok(())
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

    /// Execute this fixture
    pub fn execute(&self) -> Result<(), Error> {
        for test in self.tests.iter() {
            test.execute(&self.name)?;
        }
        Ok(())
    }
}

/// A test in a fixture
pub enum Test {
    /// An output test
    Output(OutputTest),
    /// A parsing test
    Parsing(ParsingTest)
}

impl Test {
    /// Execute this test
    pub fn execute(&self, fixture_name: &str) -> Result<TestResult, Error> {
        match self {
            Test::Output(inner) => inner.execute(fixture_name),
            Test::Parsing(inner) => inner.execute(fixture_name)
        }
    }
}

/// An output test
pub struct OutputTest {
    /// The test name
    pub name: String,
    /// The index within the fixture
    pub index: usize,
    /// The input for the parser
    pub input: String,
    /// The expected output
    pub output: Vec<String>
}

impl OutputTest {
    /// Builds an output test from the specified AST
    pub fn from_ast(node: AstNode, index: usize) -> OutputTest {
        let name = node.child(0).get_value().unwrap();
        let input = node.child(3).get_value().unwrap();
        let output = node
            .children()
            .iter()
            .skip(4)
            .map(|c| {
                let line = c.get_value().unwrap();
                let line = hime_sdk::loaders::replace_escapees(line);
                line.replace("\\\"", "\"")
            })
            .collect();
        OutputTest {
            name,
            index,
            input,
            output
        }
    }

    /// Execute this test
    pub fn execute(&self, fixture_name: &str) -> Result<TestResult, Error> {
        {
            let mut input_file = BufWriter::new(File::create("input.txt")?);
            write!(input_file, "{}", &self.input)?;
            input_file.flush()?;
        }
        {
            let mut expected_file = BufWriter::new(File::create("expected.txt")?);
            for line in self.output.iter() {
                writeln!(expected_file, "{}", line)?;
            }
            expected_file.flush()?;
        }
        let result_net = self.execute_net(fixture_name)?;
        let result_java = self.execute_java(fixture_name)?;
        let result_rust = self.execute_rust()?;
        Ok(TestResult {
            test_name: self.name.clone(),
            dot_net: Some(result_net),
            java: Some(result_java),
            rust: Some(result_rust)
        })
    }

    /// Execute this test on the .Net runtime
    fn execute_net(&self, fixture_name: &str) -> Result<TestResultOnRuntime, Error> {
        let name = format!(
            "Hime.Tests.Generated.{}.{}Parser",
            to_upper_case(fixture_name),
            self.name
        );
        execute_command(
            Runtime::Java,
            "mono",
            &["executor-net.exe", "executor-java.jar", &name, "outputs"]
        )
    }

    /// Execute this test on the Java runtime
    fn execute_java(&self, fixture_name: &str) -> Result<TestResultOnRuntime, Error> {
        let name = format!(
            "hime.tests.generated.{}.{}Parser",
            to_snake_case(fixture_name),
            self.name
        );
        execute_command(
            Runtime::Java,
            "java",
            &["-jar", "executor-java.jar", &name, "outputs"]
        )
    }

    /// Execute this test on the Rust runtime
    fn execute_rust(&self) -> Result<TestResultOnRuntime, Error> {
        let program = format!("executor-rust{}", get_system_ext());
        let name = to_snake_case(&self.name);
        execute_command(Runtime::Rust, &program, &[&name, "outputs"])
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
    /// Builds an output test from the specified AST
    pub fn from_ast(node: AstNode, index: usize) -> ParsingTest {
        let name = node.child(0).get_value().unwrap();
        ParsingTest { name, index }
    }

    /// Execute this test
    pub fn execute(&self, fixture_name: &str) -> Result<TestResult, Error> {
        Ok(TestResult {
            test_name: self.name.clone(),
            dot_net: None,
            java: None,
            rust: None
        })
    }
}

/// Execute s a command
fn execute_command(
    runtime: Runtime,
    program: &str,
    args: &[&str]
) -> Result<TestResultOnRuntime, Error> {
    let mut command = Command::new(program);
    command.args(args);
    let start_time = Instant::now();
    let output = command.output()?;
    let end_time = Instant::now();
    let spent_time = end_time - start_time;
    let status = match output.status.code() {
        Some(0) => TestResultStatus::Success,
        Some(1) => TestResultStatus::Failure,
        Some(2) => TestResultStatus::Error,
        _ => TestResultStatus::Error
    };
    Ok(TestResultOnRuntime {
        runtime,
        start_time,
        spent_time,
        status,
        output: String::from_utf8(output.stdout).unwrap()
    })
}

/// Gets the system extension for the system executable
#[cfg(target_os = "linux")]
fn get_system_ext() -> &'static str {
    ""
}

/// Gets the system extension for the system executable
#[cfg(target_os = "macos")]
fn get_system_ext() -> &'static str {
    ""
}

/// Gets the system extension for the system executable
#[cfg(windows)]
fn get_system_ext() -> &'static str {
    ".exe"
}
