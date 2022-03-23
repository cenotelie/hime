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
use std::path::PathBuf;
use std::process::Command;
use std::time::Instant;

use hime_redist::ast::AstNode;
use hime_redist::result::ParseResult;
use hime_redist::symbols::SemanticElementTrait;
use hime_sdk::errors::{Error, Errors};
use hime_sdk::grammars::{BuildData, Grammar, OPTION_METHOD};
use hime_sdk::output::helper::{to_snake_case, to_upper_camel_case};
use hime_sdk::{CompilationTask, InputReference, LoadedData, LoadedInput, Mode, Modifier, Runtime};
use results::{TestResultOnRuntime, TestResultStatus};

use self::results::{ExecutionResults, FixtureResults, TestResult};

/// A fixture definition
pub struct FixtureDef(pub &'static str, pub &'static [u8]);

/// All the test fixtures
pub struct Fixtures(pub Vec<Fixture>);

impl Fixtures {
    /// Build the parsers for the fixtures
    pub fn build(&self, filter: Option<&str>) -> Result<(), Errors> {
        let mut errors: Vec<Error> = Vec::new();

        // Load all grammars
        let mut grammars: Vec<Grammar> = Vec::new();
        for (index, fixture) in self.0.iter().enumerate() {
            match fixture.grammars(index, filter) {
                Ok(mut fixture_grammars) => grammars.append(&mut fixture_grammars),
                Err(mut fixture_errors) => errors.append(&mut fixture_errors)
            }
        }

        // Build all parser data
        let mut grammars_data = Vec::new();
        for (index, grammar) in grammars.iter_mut().enumerate() {
            match grammar.build(None, index) {
                Ok(data) => grammars_data.push(data),
                Err(mut errs) => errors.append(&mut errs)
            }
        }

        if errors.is_empty() {
            self.build_net(&grammars, &grammars_data, &mut errors);
            self.build_java(&grammars, &grammars_data, &mut errors);
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
        println!("Building Rust test parsers");
        if let Err(e) = self.build_rust_inner(grammars, grammars_data, errors) {
            errors.push(e);
        }
    }

    /// Build the Rust parsers for the fixtures
    fn build_rust_inner(
        &self,
        grammars: &[Grammar],
        grammars_data: &[BuildData],
        errors: &mut Vec<Error>
    ) -> Result<(), Error> {
        let temp_dir = hime_sdk::output::temporary_folder();
        fs::create_dir_all(&temp_dir)?;
        let mut runtime_path = get_repo_root();
        runtime_path.push("runtime-rust");

        let task = CompilationTask {
            mode: Some(Mode::SourcesAndAssembly),
            output_target: Some(Runtime::Rust),
            output_path: Some(temp_dir.to_str().unwrap().to_string()),
            output_target_runtime_path: runtime_path.to_str().map(|s| s.to_string()),
            ..CompilationTask::default()
        };

        // Build all parser data
        for (index, (grammar, data)) in grammars.iter().zip(grammars_data.iter()).enumerate() {
            if let Err(mut errs) =
                hime_sdk::output::output_grammar_artifacts(&task, grammar, index, data)
            {
                errors.append(&mut errs);
            }
        }

        let units: Vec<(usize, &Grammar)> = grammars.iter().enumerate().collect();
        hime_sdk::output::build_assembly(&task, &units, Runtime::Rust)?;

        // export the result to the local dir
        let mut path_result = temp_dir.clone();
        path_result.push("parsers");
        path_result.set_extension(get_system_ext_dl());
        let mut path_target = get_local_dir();
        path_target.push("parsers-rust");
        path_target.set_extension(get_system_ext_dl());
        std::fs::copy(path_result, path_target)?;
        // cleanup
        std::fs::remove_dir_all(temp_dir)?;
        Ok(())
    }

    /// Build the .Net parsers for the fixtures
    fn build_net(
        &self,
        grammars: &[Grammar],
        grammars_data: &[BuildData],
        errors: &mut Vec<Error>
    ) {
        println!("Building .Net test parsers");
        if let Err(e) = self.build_net_inner(grammars, grammars_data, errors) {
            errors.push(e);
        }
    }

    /// Build the .Net parsers for the fixtures
    fn build_net_inner(
        &self,
        grammars: &[Grammar],
        grammars_data: &[BuildData],
        errors: &mut Vec<Error>
    ) -> Result<(), Error> {
        let temp_dir = hime_sdk::output::temporary_folder();
        fs::create_dir_all(&temp_dir)?;
        let mut runtime_path = get_repo_root();
        runtime_path.push("tests-results");

        let task = CompilationTask {
            mode: Some(Mode::SourcesAndAssembly),
            output_target: Some(Runtime::Net),
            output_path: Some(temp_dir.to_str().unwrap().to_string()),
            output_target_runtime_path: runtime_path.to_str().map(|s| s.to_string()),
            ..CompilationTask::default()
        };

        // Build all parser data
        for (index, (grammar, data)) in grammars.iter().zip(grammars_data.iter()).enumerate() {
            if let Err(mut errs) =
                hime_sdk::output::output_grammar_artifacts(&task, grammar, index, data)
            {
                errors.append(&mut errs);
            }
        }

        let units: Vec<(usize, &Grammar)> = grammars.iter().enumerate().collect();
        hime_sdk::output::build_assembly(&task, &units, Runtime::Net)?;

        // export the result to the local dir
        let mut path_result = temp_dir.clone();
        path_result.push("Parsers.dll");
        let mut path_target = get_local_dir();
        path_target.push("parsers-net.dll");
        std::fs::copy(path_result, path_target)?;
        // cleanup
        std::fs::remove_dir_all(temp_dir)?;
        Ok(())
    }

    /// Build the Java parsers for the fixtures
    fn build_java(
        &self,
        grammars: &[Grammar],
        grammars_data: &[BuildData],
        errors: &mut Vec<Error>
    ) {
        println!("Building Java test parsers");
        if let Err(e) = self.build_java_inner(grammars, grammars_data, errors) {
            errors.push(e);
        }
    }

    /// Build the Java parsers for the fixtures
    fn build_java_inner(
        &self,
        grammars: &[Grammar],
        grammars_data: &[BuildData],
        errors: &mut Vec<Error>
    ) -> Result<(), Error> {
        let temp_dir = hime_sdk::output::temporary_folder();
        fs::create_dir_all(&temp_dir)?;
        let mut runtime_path = get_repo_root();
        runtime_path.push("runtime-java");

        let task = CompilationTask {
            mode: Some(Mode::SourcesAndAssembly),
            output_target: Some(Runtime::Java),
            output_modifier: Some(Modifier::Public),
            output_path: Some(temp_dir.to_str().unwrap().to_string()),
            output_target_runtime_path: runtime_path.to_str().map(|s| s.to_string()),
            java_maven_repository: match std::env::var("HOME") {
                Ok(home) => Some(format!("{}/.m2/repository", home)),
                Err(_) => None
            },
            ..CompilationTask::default()
        };

        // Build all parser data
        for (index, (grammar, data)) in grammars.iter().zip(grammars_data.iter()).enumerate() {
            if let Err(mut errs) =
                hime_sdk::output::output_grammar_artifacts(&task, grammar, index, data)
            {
                errors.append(&mut errs);
            }
        }

        let units: Vec<(usize, &Grammar)> = grammars.iter().enumerate().collect();
        hime_sdk::output::build_assembly(&task, &units, Runtime::Java)?;

        // export the result to the local dir
        let mut path_result = temp_dir.clone();
        path_result.push("Parsers.jar");
        let mut path_target = get_local_dir();
        path_target.push("parsers-java.jar");
        std::fs::copy(path_result, path_target)?;
        // cleanup
        std::fs::remove_dir_all(temp_dir)?;
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

    /// Execute this fixture
    pub fn execute(&self, filter: Option<&str>) -> Result<ExecutionResults, Error> {
        let results = self
            .0
            .iter()
            .map(|fixture| fixture.execute(filter))
            .collect::<Result<Vec<_>, _>>()?;
        Ok(ExecutionResults(results))
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
    pub fn grammars(&self, index: usize, filter: Option<&str>) -> Result<Vec<Grammar>, Vec<Error>> {
        let ast = self.content.get_ast();
        let root = ast.get_root();
        let roots: Vec<(usize, AstNode)> = root
            .into_iter()
            .filter(|test_node| match filter {
                None => true,
                Some(filter) => test_node
                    .child(0)
                    .get_value()
                    .map(|name| name.contains(filter))
                    .unwrap_or(true)
            })
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
    pub fn execute(&self, filter: Option<&str>) -> Result<FixtureResults, Error> {
        let results = self
            .tests
            .iter()
            .filter(|test| test.is_selected(filter))
            .map(|test| test.execute())
            .collect::<Result<Vec<_>, _>>()?;
        Ok(FixtureResults {
            name: self.name.clone(),
            tests: results
        })
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
    /// Checks whether this test should be executed
    pub fn is_selected(&self, filter: Option<&str>) -> bool {
        match filter {
            None => true,
            Some(filter) => match self {
                Test::Output(inner) => inner.name.contains(filter),
                Test::Parsing(inner) => inner.name.contains(filter)
            }
        }
    }

    /// Execute this test
    pub fn execute(&self) -> Result<TestResult, Error> {
        match self {
            Test::Output(inner) => inner.execute(),
            Test::Parsing(inner) => inner.execute()
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
        let input = hime_sdk::loaders::replace_escapees(node.child(3).get_value().unwrap());
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
    pub fn execute(&self) -> Result<TestResult, Error> {
        {
            let mut input_file = BufWriter::new(File::create("input.txt")?);
            write!(input_file, "{}", &self.input[1..self.input.len() - 1])?;
            input_file.flush()?;
        }
        {
            let mut expected_file = BufWriter::new(File::create("expected.txt")?);
            for line in self.output.iter() {
                writeln!(expected_file, "{}", &line[1..line.len() - 1])?;
            }
            expected_file.flush()?;
        }
        let result_net = self.execute_net()?;
        let result_java = self.execute_java()?;
        let result_rust = self.execute_rust()?;
        Ok(TestResult {
            name: self.name.clone(),
            dot_net: Some(result_net),
            java: Some(result_java),
            rust: Some(result_rust)
        })
    }

    /// Execute this test on the .Net runtime
    fn execute_net(&self) -> Result<TestResultOnRuntime, Error> {
        let name = format!(
            "{}.{}Parser",
            to_upper_camel_case(&self.name),
            to_upper_camel_case(&self.name)
        );
        execute_command(
            Runtime::Net,
            "mono",
            &["executor-net.exe", &name, "outputs"]
        )
    }

    /// Execute this test on the Java runtime
    fn execute_java(&self) -> Result<TestResultOnRuntime, Error> {
        let name = format!(
            "{}.{}Parser",
            to_snake_case(&self.name),
            to_upper_camel_case(&self.name)
        );
        execute_command(
            Runtime::Java,
            "java",
            &["-jar", "executor-java.jar", &name, "outputs"]
        )
    }

    /// Execute this test on the Rust runtime
    fn execute_rust(&self) -> Result<TestResultOnRuntime, Error> {
        let mut program = get_local_dir();
        program.push("executor-rust");
        let ext = get_system_ext_exe();
        if !ext.is_empty() {
            program.set_extension(ext);
        }
        let name = to_snake_case(&self.name);
        execute_command(
            Runtime::Rust,
            program.to_str().unwrap(),
            &[&name, "outputs"]
        )
    }
}

/// The verb for a parsing test
pub enum ParsingTestVerb {
    /// The produced AST matches the expected one
    Matches,
    /// The produced AST does NOT match the provided one
    NoMatch,
    /// The parsing fails
    Fails
}

impl ParsingTestVerb {
    /// Gets the string representation for the verb
    pub fn as_str(&self) -> &'static str {
        match self {
            ParsingTestVerb::Matches => "matches",
            ParsingTestVerb::NoMatch => "nomatches",
            ParsingTestVerb::Fails => "fails"
        }
    }
}

/// A parsing test
pub struct ParsingTest {
    /// The test name
    pub name: String,
    /// The index within the fixture
    pub index: usize,
    /// The verb for this test
    pub verb: ParsingTestVerb,
    /// The input for the parser
    pub input: String,
    /// The string serialization of the reference AST
    pub tree: String
}

impl ParsingTest {
    /// Builds an output test from the specified AST
    pub fn from_ast(node: AstNode, index: usize) -> ParsingTest {
        let name = node.child(0).get_value().unwrap();
        let verb = match node.get_symbol().id {
            crate::loaders::ID_VARIABLE_TEST_MATCHES => ParsingTestVerb::Matches,
            crate::loaders::ID_VARIABLE_TEST_NO_MATCH => ParsingTestVerb::NoMatch,
            _ => ParsingTestVerb::Fails
        };
        let input = hime_sdk::loaders::replace_escapees(node.child(3).get_value().unwrap());
        let tree = if node.children_count() >= 5 {
            let mut buffer = String::new();
            ParsingTest::serialize_tree(node.child(4), &mut buffer);
            buffer
        } else {
            String::new()
        };
        ParsingTest {
            name,
            index,
            verb,
            input,
            tree
        }
    }

    /// Serializes a tree into a string
    fn serialize_tree(node: AstNode, buffer: &mut String) {
        if let Some(value) = node.get_value() {
            buffer.push_str(&value);
        }
        let node_check = node.child(0);
        if node_check.children_count() > 0 {
            buffer.push_str(&node_check.child(0).get_value().unwrap());
            let value = node_check.child(1).get_value().unwrap();
            // Decode the read value by replacing all the escape sequences
            let value = hime_sdk::loaders::replace_escapees(value);
            // Reset escape sequences for single quotes and backslashes
            let value = value[1..(value.len() - 1)]
                .replace('\\', "\\\\")
                .replace('\'', "\\'");
            buffer.push('\'');
            buffer.push_str(&value);
            buffer.push('\'');
        }
        let node_children = node.child(1);
        if node_children.children_count() > 0 {
            buffer.push_str(" (");
            for (index, child) in node_children.children().iter().enumerate() {
                if index > 0 {
                    buffer.push(' ');
                }
                ParsingTest::serialize_tree(child, buffer);
            }
            buffer.push(')');
        }
    }

    /// Execute this test
    pub fn execute(&self) -> Result<TestResult, Error> {
        {
            let mut input_file = BufWriter::new(File::create("input.txt")?);
            write!(input_file, "{}", &self.input[1..self.input.len() - 1])?;
            input_file.flush()?;
        }
        if !self.tree.is_empty() {
            let mut expected_file = BufWriter::new(File::create("expected.txt")?);
            write!(expected_file, "{}", &self.tree)?;
            expected_file.flush()?;
        }
        let result_net = self.execute_net()?;
        let result_java = self.execute_java()?;
        let result_rust = self.execute_rust()?;
        Ok(TestResult {
            name: self.name.clone(),
            dot_net: Some(result_net),
            java: Some(result_java),
            rust: Some(result_rust)
        })
    }

    /// Execute this test on the .Net runtime
    fn execute_net(&self) -> Result<TestResultOnRuntime, Error> {
        let name = format!(
            "{}.{}Parser",
            to_upper_camel_case(&self.name),
            to_upper_camel_case(&self.name)
        );
        execute_command(
            Runtime::Net,
            "mono",
            &["executor-net.exe", &name, self.verb.as_str()]
        )
    }

    /// Execute this test on the Java runtime
    fn execute_java(&self) -> Result<TestResultOnRuntime, Error> {
        let name = format!(
            "{}.{}Parser",
            to_snake_case(&self.name),
            to_upper_camel_case(&self.name)
        );
        execute_command(
            Runtime::Java,
            "java",
            &["-jar", "executor-java.jar", &name, self.verb.as_str()]
        )
    }

    /// Execute this test on the Rust runtime
    fn execute_rust(&self) -> Result<TestResultOnRuntime, Error> {
        let mut program = get_local_dir();
        program.push("executor-rust");
        let ext = get_system_ext_exe();
        if !ext.is_empty() {
            program.set_extension(ext);
        }
        let name = to_snake_case(&self.name);
        execute_command(
            Runtime::Rust,
            program.to_str().unwrap(),
            &[&name, self.verb.as_str()]
        )
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
    let stdout = String::from_utf8(output.stdout).unwrap();
    let stderr = String::from_utf8(output.stderr).unwrap();
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
        stdout,
        stderr
    })
}

/// Gets the system extension for the system executable
#[cfg(target_os = "linux")]
fn get_system_ext_exe() -> &'static str {
    ""
}

/// Gets the system extension for the system executable
#[cfg(target_os = "macos")]
fn get_system_ext_exe() -> &'static str {
    ""
}

/// Gets the system extension for the system executable
#[cfg(windows)]
fn get_system_ext_exe() -> &'static str {
    "exe"
}

/// Gets the system extension for the system assembly
#[cfg(target_os = "linux")]
fn get_system_ext_dl() -> &'static str {
    "so"
}

/// Gets the system extension for the system assembly
#[cfg(target_os = "macos")]
fn get_system_ext_dl() -> &'static str {
    "dylib"
}

/// Gets the system extension for the system assembly
#[cfg(windows)]
fn get_system_ext_dl() -> &'static str {
    "dll"
}

/// Gets the local directory
fn get_local_dir() -> PathBuf {
    let mut path = std::env::current_exe().unwrap();
    path.pop(); // get the directory of the executable
    path
}

/// Gets the path to the repository's root
fn get_repo_root() -> PathBuf {
    let mut path = get_local_dir();
    if path.file_name().map(|s| s.to_str()) == Some(Some("tests-results")) {
        // we are in tests-results
        path.pop();
    }
    path
}
