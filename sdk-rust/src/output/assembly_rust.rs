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

//! Module for build Rust assemblies

use std::fs::{self, File, OpenOptions};
use std::io::{BufWriter, Write};
use std::path::{Path, PathBuf};
use std::process::Command;

use crate::errors::Error;
use crate::grammars::Grammar;
use crate::output::helper;
use crate::{output, CompilationTask};

const MANIFEST: &[u8] = include_bytes!("assembly_rust.toml");

/// Build the Rust assembly for the specified units
pub fn build(task: &CompilationTask, units: &[(usize, &Grammar)]) -> Result<(), Error> {
    // build the project
    let project_folder = build_cargo_project(task, units)?;
    // compile
    execute_cargo_command(&project_folder, &["build", "--release"])?;
    execute_cargo_command(&project_folder, &["package", "--no-verify"])?;
    // copy the output
    let mut output_file = project_folder.clone();
    output_file.push("target");
    output_file.push("release");
    output_file.push(get_output_name());
    let mut output_file2 = project_folder.clone();
    output_file2.push("target");
    output_file2.push("package");
    output_file2 = output_file2
        .read_dir()?
        .find(|entry| match entry.as_ref() {
            Err(_) => false,
            Ok(entry) => {
                let file_name = entry.file_name();
                let name = file_name.to_str().unwrap();
                name.starts_with("hime_generated") && name.ends_with("crate")
            }
        })
        .unwrap()?
        .path();

    if units.len() == 1 {
        // only one grammar => output for the grammar
        let path = task.get_output_path_for(units[0].1);
        let mut final_path = PathBuf::new();
        if let Some(path) = path {
            final_path.push(path);
        }
        final_path.push(format!(
            "{}.{}",
            helper::to_upper_camel_case(&units[0].1.name),
            get_system_ext()
        ));
        fs::rename(output_file, &final_path)?;
        final_path.pop();
        final_path.push(format!(
            "{}.crate",
            helper::to_upper_camel_case(&units[0].1.name)
        ));
        fs::rename(output_file2, &final_path)?;
    } else {
        // output the package
        let mut final_path = PathBuf::new();
        if let Some(path) = task.output_path.as_ref() {
            final_path.push(path);
        }
        final_path.push(format!("parsers.{}", get_system_ext()));
        fs::rename(output_file, &final_path)?;
        final_path.pop();
        final_path.push("parsers.crate");
        fs::rename(output_file2, &final_path)?;
    }
    // cleanup the temp folder
    fs::remove_dir_all(&project_folder)?;
    Ok(())
}

/// Gets the output name for the assembly
#[cfg(target_os = "linux")]
fn get_output_name() -> &'static str {
    "libhime_generated.so"
}

/// Gets the output name for the assembly
#[cfg(target_os = "macos")]
fn get_output_name() -> &'static str {
    "libhime_generated.dylib"
}

/// Gets the output name for the assembly
#[cfg(windows)]
fn get_output_name() -> &'static str {
    "hime_generated.dll"
}

/// Gets the system extension for the system assembly
#[cfg(target_os = "linux")]
fn get_system_ext() -> &'static str {
    "so"
}

/// Gets the system extension for the system assembly
#[cfg(target_os = "macos")]
fn get_system_ext() -> &'static str {
    "dylib"
}

/// Gets the system extension for the system assembly
#[cfg(windows)]
fn get_system_ext() -> &'static str {
    "dll"
}

/// Builds the cargo project
fn build_cargo_project(
    task: &CompilationTask,
    units: &[(usize, &Grammar)]
) -> Result<PathBuf, Error> {
    let project_folder = output::temporary_folder();
    fs::create_dir_all(&project_folder)?;
    output::export_resource(&project_folder, "Cargo.toml", MANIFEST)?;
    if let Some(ref runtime) = task.output_target_runtime_path {
        let mut file_path = project_folder.clone();
        file_path.push("Cargo.toml");
        let mut writer = BufWriter::new(
            OpenOptions::new()
                .create(true)
                .append(true)
                .open(file_path)?
        );
        writeln!(writer)?;
        writeln!(writer)?;
        writeln!(writer, "[patch.crates-io]")?;
        writeln!(
            writer,
            "hime_redist = {{ path = \"{}\" }}",
            runtime.replace("\\", "\\\\")
        )?;
    }
    let mut src_folder = project_folder.clone();
    src_folder.push("src");
    fs::create_dir(&src_folder)?;
    write_lib_rs(&src_folder)?;
    for (index, grammar) in units.iter() {
        let module_name = get_module_name(task, grammar);
        let parts: Vec<&str> = module_name.split("::").collect();
        let mut current = src_folder.clone();
        for (i, part) in parts[0..(parts.len() - 1)].iter().enumerate() {
            let mut target = current.clone();
            target.push(part);
            if !target.exists() {
                // Creates the directory for the module
                fs::create_dir(&target)?;
                // register the module
                let filename = if i == 0 { "lib.rs" } else { "mod.rs" };
                let mut file_path = current.clone();
                file_path.push(filename);
                let mut writer = BufWriter::new(
                    OpenOptions::new()
                        .create(true)
                        .append(true)
                        .open(file_path)?
                );
                writeln!(writer, "pub mod {}", part)?;
                // create the module file
                let mut file_path = target.clone();
                file_path.push("mod.rs");
                File::create(file_path)?;
            }
            current = target;
        }
        let filename = if parts.len() == 1 { "lib.rs" } else { "mod.rs" };
        let mut file_path = current.clone();
        file_path.push(filename);
        let mut writer = BufWriter::new(
            OpenOptions::new()
                .create(true)
                .append(true)
                .open(file_path)?
        );
        writeln!(writer, "pub mod {};", parts[parts.len() - 1])?;
        for source in output::get_sources(task, grammar, *index)?.into_iter() {
            let mut target = current.clone();
            target.push(source.file_name().unwrap());
            fs::copy(source, target)?;
        }
    }
    Ok(project_folder)
}

/// Write the lib.rs file
fn write_lib_rs(src_folder: &Path) -> Result<(), Error> {
    let mut file_path = src_folder.to_path_buf();
    file_path.push("lib.rs");
    let mut writer = BufWriter::new(File::create(file_path)?);
    writeln!(writer, "//! Generated parsers")?;
    writeln!(writer, "extern crate hime_redist;")?;
    writeln!(writer)?;
    Ok(())
}

/// Get the Rust module name for a grammar
fn get_module_name(task: &CompilationTask, grammar: &Grammar) -> String {
    let nmspace = match task.get_output_namespace(grammar) {
        Some(nmspace) => nmspace,
        None => grammar.name.clone()
    };
    helper::get_namespace_rust(&nmspace)
}

/// Execute a cargo command
fn execute_cargo_command(project_folder: &Path, args: &[&str]) -> Result<(), Error> {
    let output = Command::new("cargo")
        .current_dir(project_folder)
        .args(args)
        .output()?;
    let stdout = String::from_utf8(output.stdout).unwrap();
    let stderr = String::from_utf8(output.stderr).unwrap();
    if stderr.starts_with("error") || stderr.contains("\nerror") {
        let mut log = stderr;
        log.push_str(&stdout);
        return Err(Error::Msg(log));
    }
    Ok(())
}
