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

use std::process::Command;

fn main() {
    if let Ok(output) = Command::new("git").args(["rev-parse", "HEAD"]).output() {
        let value = String::from_utf8(output.stdout).unwrap();
        println!("cargo:rustc-env=GIT_HASH={value}");
    }
    if let Ok(output) = Command::new("git")
        .args(["tag", "-l", "--points-at", "HEAD"])
        .output()
    {
        let value = String::from_utf8(output.stdout).unwrap();
        println!("cargo:rustc-env=GIT_TAG={value}");
    }
}
