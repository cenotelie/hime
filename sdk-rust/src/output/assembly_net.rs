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
use std::io::{BufRead, BufReader};
use std::path::PathBuf;
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
    execute_dotnet_command(&project_folder, "restore", &[])?;
    execute_dotnet_command(&project_folder, "build", &["-c", "Release"])?;
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
        fs::rename(output_file, final_path)?;
    } else {
        // output the package
        let mut final_path = PathBuf::new();
        if let Some(path) = task.output_path.as_ref() {
            final_path.push(path);
        }
        final_path.push("Parsers.dll");
        fs::rename(output_file, final_path)?;
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
        for source in output::get_sources(task, grammar, *index)?.into_iter() {
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
    project_folder: &PathBuf,
    verb: &str,
    args: &[&str]
) -> Result<(), Error> {
    let output = Command::new("dotnet")
        .arg(verb)
        .arg(project_folder.as_os_str())
        .args(args)
        .output()?;
    let mut lines = BufReader::<&[u8]>::new(output.stderr.as_ref()).lines();
    if let Some(line) = lines.next() {
        return Err(Error::Msg(line?));
    }
    for line in BufReader::<&[u8]>::new(output.stdout.as_ref()).lines() {
        let line = line?;
        if line.contains("FAILED") {
            return Err(Error::Msg(line));
        }
    }
    Ok(())
}
