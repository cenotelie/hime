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

//! Module for build .Net assemblies

use std::fs;
use std::path::{Path, PathBuf};
use std::process::Command;

use crate::errors::Error;
use crate::grammars::Grammar;
use crate::output::helper;
use crate::{output, CompilationTask};

const MANIFEST: &[u8] = include_bytes!("assembly_net.csproj");

/// Build the .Net assembly for the specified units
pub fn build(task: &CompilationTask, units: &[(usize, &Grammar)]) -> Result<(), Error> {
    // build the project
    let project_folder = build_dotnet_project(task, units)?;
    // compile
    execute_dotnet_command(
        &project_folder,
        "restore",
        &[],
        task.output_target_runtime_path.as_deref()
    )?;
    execute_dotnet_command(
        &project_folder,
        "build",
        &["-c", "Release"],
        task.output_target_runtime_path.as_deref()
    )?;
    // copy the output
    let mut output_file = project_folder.clone();
    output_file.push("bin");
    output_file.push("Release");
    output_file.push("netstandard2.0");
    output_file.push("Hime.Generated.dll");
    if units.len() == 1 {
        // only one grammar => output for the grammar
        let path = task.get_output_path_for(units[0].1);
        let mut final_path = PathBuf::new();
        if let Some(path) = path {
            final_path.push(path);
        }
        final_path.push(format!(
            "{}.dll",
            helper::to_upper_camel_case(&units[0].1.name)
        ));
        fs::copy(output_file, final_path)?;
    } else {
        // output the package
        let mut final_path = PathBuf::new();
        if let Some(path) = task.output_path.as_ref() {
            final_path.push(path);
        }
        final_path.push("Parsers.dll");
        fs::copy(output_file, final_path)?;
    }
    // cleanup the temp folder
    fs::remove_dir_all(&project_folder)?;
    Ok(())
}

/// Builds the dotnet project
fn build_dotnet_project(
    task: &CompilationTask,
    units: &[(usize, &Grammar)]
) -> Result<PathBuf, Error> {
    let project_folder = output::temporary_folder();
    fs::create_dir_all(&project_folder)?;
    for (index, grammar) in units.iter() {
        for source in output::get_sources(task, grammar, *index)? {
            let mut target = project_folder.clone();
            target.push(source.file_name().unwrap());
            fs::copy(source, target)?;
        }
    }
    output::export_resource(&project_folder, "parser.csproj", MANIFEST)?;
    Ok(project_folder)
}

/// Execute a dotnet command
fn execute_dotnet_command(
    project_folder: &Path,
    verb: &str,
    args: &[&str],
    target_runtime: Option<&str>
) -> Result<(), Error> {
    let mut command = Command::new("dotnet");
    command.arg(verb).arg(project_folder.as_os_str()).args(args);
    if let Some(target_runtime) = target_runtime {
        command.env("HimeLocalNuget", target_runtime);
    }
    let output = command.output()?;
    let stdout = String::from_utf8(output.stdout).unwrap();
    let stderr = String::from_utf8(output.stderr).unwrap();
    if !stderr.is_empty() || stdout.contains("FAILED") {
        let mut log = stderr;
        log.push_str(&stdout);
        return Err(Error::Msg(log));
    }
    Ok(())
}
