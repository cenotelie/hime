/*******************************************************************************
 * Copyright (c) 2020 Association Cénotélie (cenotelie.fr)
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

//! Module for build Java assemblies

use crate::errors::Error;
use crate::grammars::Grammar;
use crate::output;
use crate::output::helper;
use crate::CompilationTask;
use std::fs;
use std::io::{BufRead, BufReader};
use std::path::PathBuf;
use std::process::Command;

const MANIFEST: &[u8] = include_bytes!("assembly_java.xml");

/// Build the Rust assembly for the specified units
pub fn build(task: &CompilationTask, units: &[(usize, &Grammar)]) -> Result<(), Error> {
    // build the project
    let project_folder = build_maven_project(task, units)?;
    // compile
    execute_mvn_command(&project_folder, &["package"])?;
    // copy the output
    let mut output_file = project_folder.clone();
    output_file.push("target");
    for item in fs::read_dir(&output_file)? {
        let candidate = item?;
        let file_name = candidate.file_name();
        if let Some(file_name) = file_name.to_str() {
            if file_name.starts_with("hime-generated-") && file_name.ends_with(".jar") {
                output_file.push(&file_name);
                break;
            }
        }
    }
    if units.len() == 1 {
        // only one grammar => output for the grammar
        let path = task.get_output_path_for(units[0].1);
        let mut final_path = PathBuf::new();
        if let Some(path) = path {
            final_path.push(path);
        }
        final_path.push(format!(
            "{}.jar",
            helper::to_upper_camel_case(&units[0].1.name)
        ));
        fs::rename(output_file, final_path)?;
    } else {
        // output the package
        let mut final_path = PathBuf::new();
        if let Some(path) = task.output_path.as_ref() {
            final_path.push(path);
        }
        final_path.push("Parsers.jar");
        fs::rename(output_file, final_path)?;
    }
    // cleanup the temp folder
    fs::remove_dir_all(&project_folder)?;
    Ok(())
}

/// Builds the cargo project
fn build_maven_project(
    task: &CompilationTask,
    units: &[(usize, &Grammar)]
) -> Result<PathBuf, Error> {
    let project_folder = output::temporary_folder();
    fs::create_dir_all(&project_folder)?;
    output::export_resource(&project_folder, "pom.xml", MANIFEST)?;

    let mut src_folder = project_folder.clone();
    src_folder.push("src");
    src_folder.push("main");
    src_folder.push("java");
    fs::create_dir_all(&src_folder)?;
    let mut resources_folder = project_folder.clone();
    resources_folder.push("src");
    resources_folder.push("main");
    resources_folder.push("resources");
    fs::create_dir_all(&resources_folder)?;

    for (index, grammar) in units.iter() {
        let nmspace = get_namespace(task, grammar);
        let parts: Vec<&str> = nmspace.split('.').collect();
        let target_src = create_namespace_folder(&src_folder, &parts)?;
        let target_resources = create_namespace_folder(&resources_folder, &parts)?;

        for source in output::get_sources(task, grammar, *index)?.into_iter() {
            let mut target = if let Some(extension) = source.extension() {
                if extension == "bin" {
                    target_resources.clone()
                } else {
                    target_src.clone()
                }
            } else {
                target_resources.clone()
            };
            target.push(source.file_name().unwrap());
            fs::copy(source, target)?;
        }
    }
    Ok(project_folder)
}

/// Creates folders for a namespace
fn create_namespace_folder(root: &PathBuf, parts: &[&str]) -> Result<PathBuf, Error> {
    let mut target = root.clone();
    for part in parts.iter() {
        target.push(part);
    }
    fs::create_dir_all(&target)?;
    Ok(target)
}

/// Get the Java namespace or a grammar
fn get_namespace(task: &CompilationTask, grammar: &Grammar) -> String {
    let nmspace = match task.get_output_namespace(grammar) {
        Some(nmspace) => nmspace,
        None => grammar.name.clone()
    };
    helper::get_namespace_java(&nmspace)
}

/// Execute a cargo command
fn execute_mvn_command(project_folder: &PathBuf, args: &[&str]) -> Result<(), Error> {
    let (executable, prefix_args) = get_mvn_command();
    let output = Command::new(executable)
        .current_dir(project_folder)
        .args(prefix_args)
        .args(args)
        .arg("-B")
        .output()?;
    for line in BufReader::<&[u8]>::new(output.stderr.as_ref()).lines() {
        let line = line?;
        if line.starts_with("[ERROR]") {
            return Err(Error::Msg(line));
        }
    }
    for line in BufReader::<&[u8]>::new(output.stdout.as_ref()).lines() {
        let line = line?;
        if line.starts_with("[ERROR]") {
            return Err(Error::Msg(line));
        }
    }
    Ok(())
}

/// Gets the command for maven
#[cfg(target_os = "linux")]
fn get_mvn_command() -> (&'static str, &'static [&'static str]) {
    ("mvn", &[])
}

/// Gets the command for maven
#[cfg(target_os = "macos")]
fn get_mvn_command() -> (&'static str, &'static [&'static str]) {
    ("mvn", &[])
}

/// Gets the command for maven
#[cfg(windows)]
fn get_mvn_command() -> (&'static str, &'static [&'static str]) {
    ("cmd.exe", &["/c", "mvn.cmd"])
}
