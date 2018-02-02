/*******************************************************************************
 * Copyright (c) 2018 Association Cénotélie (cenotelie.fr)
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

extern crate futures;
extern crate hime_sdk;
extern crate hyper;
extern crate regex;
extern crate tokio_core;

use futures::{Future, Stream};
use hime_sdk::output::helper;
use hime_sdk::unicode::UnicodeBlock;
use hime_sdk::unicode::UnicodeCategory;
use hime_sdk::unicode::UnicodeSpan;
use hyper::Client;
use regex::Regex;
use std::u32;
use tokio_core::reactor::Core;

use std::collections::HashMap;
use std::fs::File;
use std::io::Error;
use std::io::Write;

/// Main entry point
fn main() {
    let blocks = get_latest_blocks();
    generate_blocks_db(&blocks).unwrap();
    generate_blocks_tests(&blocks).unwrap();
    let categories = get_latest_categories();
}

/// The URL of the latest specification of Unicode blocks
const URL_UNICODE_BLOCKS: &'static str = "http://www.unicode.org/Public/UCD/latest/ucd/Blocks.txt";
/// The URL of the latest specification of Unicode code points
const URL_UNICODE_DATA: &'static str =
    "http://www.unicode.org/Public/UCD/latest/ucd/UnicodeData.txt";

/// Gets the latest unicode blocks from the Unicode web site
pub fn get_latest_blocks() -> Vec<UnicodeBlock> {
    let data = download(URL_UNICODE_BLOCKS);
    let content = String::from_utf8(data).unwrap();
    let exp = Regex::new("(?P<begin>[0-9A-F]+)\\.\\.(?P<end>[0-9A-F]+);\\s+(?P<name>(\\w|\\s|-)+)")
        .unwrap();
    let mut result = Vec::<UnicodeBlock>::new();
    for line in content.lines() {
        if line.len() == 0 || line.starts_with("#") {
            continue;
        }
        let captures = exp.captures(line);
        match captures {
            None => (),
            Some(captures) => {
                let begin = u32::from_str_radix(&captures["begin"], 16).unwrap();
                let end = u32::from_str_radix(&captures["end"], 16).unwrap();
                let name = &captures["name"];
                // filter out the Surrogate-related blocks
                if name.contains("Surrogate") {
                    continue;
                }
                result.push(UnicodeBlock::new(name.replace(" ", ""), begin, end));
            }
        }
    }
    result
}

/// Gets the latest unicode categories from the Unicode web site
pub fn get_latest_categories() -> Vec<UnicodeCategory> {
    let data = download(URL_UNICODE_DATA);
    let content = String::from_utf8(data).unwrap();
    let exp = Regex::new("(?P<code>[0-9A-F]+);([^;]+);(?P<cat>[^;]+);.*").unwrap();
    let mut categories = HashMap::new();
    let mut current: Option<(String, u32)> = None;
    let mut last_cp = 0;
    for line in content.lines() {
        if line.len() == 0 {
            continue;
        }
        let captures = exp.captures(line);
        match captures {
            None => (),
            Some(captures) => {
                let code = u32::from_str_radix(&captures["code"], 16).unwrap();
                let name = &captures["cat"];
                match &current {
                    &None => {
                        let category = UnicodeCategory::new(String::from(name));
                        categories.insert(String::from(name), category);
                    }
                    &Some((ref category, ref begin)) => {
                        if !category.eq(name) {
                            if (*begin < 0xD800 || *begin >= 0xE000)
                                && (last_cp < 0xD800 || last_cp >= 0xE000)
                            {
                                categories
                                    .get_mut(category)
                                    .unwrap()
                                    .spans
                                    .push(UnicodeSpan::new(*begin, last_cp));
                            }
                            if !categories.contains_key(name) {
                                let category = UnicodeCategory::new(String::from(name));
                                categories.insert(String::from(name), category);
                            }
                        }
                    }
                }
                last_cp = code;
                current = Some((String::from(name), code));
            }
        }
    }
    let mut result = Vec::<UnicodeCategory>::new();
    for entry in categories.into_iter() {
        result.push(entry.1);
    }
    result
}

/// Generates the code for the Unicode blocks data
pub fn generate_blocks_db(blocks: &Vec<UnicodeBlock>) -> Result<(), Error> {
    let mut file = File::create("blocks.rs").unwrap();
    write!(&mut file, "/*\n")?;
    write!(&mut file, " * WARNING: this file has been generated by\n")?;
    write!(&mut file, " * Hime Parser Generator\n")?;
    write!(&mut file, " */\n")?;
    write!(&mut file, "\n")?;
    write!(&mut file, "//! Contains the supported Unicode blocks\n")?;
    write!(&mut file, "\n")?;
    write!(&mut file, "use std::collections::HashMap;\n")?;
    write!(&mut file, "use super::UnicodeBlock;\n")?;
    write!(&mut file, "\n")?;
    for block in blocks {
        let name = helper::to_upper_case(&block.name);
        write!(&mut file, "/// The unicode block {0}\n", &block.name)?;
        write!(
            &mut file,
            "pub const {:}: &'static str = \"{:}\";\n",
            name, block.name
        )?;
        write!(&mut file, "\n")?;
    }
    write!(
        &mut file,
        "/// The database of Unicode blocks accessible by names\n"
    )?;
    write!(&mut file, "lazy_static! {{\n")?;
    write!(
        &mut file,
        "    static ref DATABASE: HashMap<&'static str, UnicodeBlock> = {{\n"
    )?;
    write!(&mut file, "        let mut m = HashMap::new();\n")?;
    for block in blocks {
        let name = helper::to_upper_case(&block.name);
        write!(
            &mut file,
            "        m.insert({:}, UnicodeBlock::new(\"{:}\".to_owned(), {:#X}, {:#X}));\n",
            name, block.name, block.span.begin.value, block.span.end.value
        )?;
    }
    write!(&mut file, "        m\n")?;
    write!(&mut file, "    }};\n")?;
    write!(&mut file, "}}\n")?;
    write!(&mut file, "\n")?;
    write!(&mut file, "/// Retrieves the definition of a block\n")?;
    write!(
        &mut file,
        "pub fn get_block(key: &str) -> Option<&UnicodeBlock> {{\n"
    )?;
    write!(&mut file, "    DATABASE.get(key)\n")?;
    write!(&mut file, "}}\n")?;
    file.flush()?;
    Ok(())
}

/// Generates the code for the Unicode blocks data
pub fn generate_blocks_tests(blocks: &Vec<UnicodeBlock>) -> Result<(), Error> {
    let mut file = File::create("UnicodeBlocks.suite").unwrap();
    for block in blocks.iter() {
        let name = block.name.replace("-", "");
        let len1 = if block.span.begin.value <= 0xFFFF {
            4
        } else {
            8
        };
        let len2 = if block.span.end.value <= 0xFFFF { 4 } else { 8 };
        write!(&mut file, "test Test_UnicodeBlock_{0}_LeftBound:\n", name)?;
        write!(&mut file, "\tgrammar Test_UnicodeBlock_{0}_LeftBound {{ options {{Axiom=\"e\";}} terminals {{X->ub{{{1}}};}} rules {{ e->X; }} }}\n", name, block.name)?;
        write!(&mut file, "\tparser LALR1\n")?;
        write!(
            &mut file,
            "\ton \"\\u{:01$X}\"\n",
            block.span.begin.value, len1
        )?;
        write!(
            &mut file,
            "\tyields e(X='\\u{:01$X}')\n",
            block.span.begin.value, len1
        )?;
        write!(&mut file, "test Test_UnicodeBlock_{0}_RightBound:\n", name)?;
        write!(&mut file, "\tgrammar Test_UnicodeBlock_{0}_RightBound {{ options {{Axiom=\"e\";}} terminals {{X->ub{{{1}}};}} rules {{ e->X; }} }}\n", name, block.name)?;
        write!(&mut file, "\tparser LALR1\n")?;
        write!(
            &mut file,
            "\ton \"\\u{:01$X}\"\n",
            block.span.end.value, len2
        )?;
        write!(
            &mut file,
            "\tyields e(X='\\u{:01$X}')\n",
            block.span.end.value, len2
        )?;
    }
    file.flush()?;
    Ok(())
}

/// Downloads the content at an URI
fn download(target: &str) -> Vec<u8> {
    let mut data = Vec::<u8>::new();
    download_into(target, &mut data);
    data
}

/// Downloads the content at an URI
fn download_into(target: &str, data: &mut Vec<u8>) {
    let mut core = Core::new().unwrap();
    let client = Client::new(&core.handle());
    let uri = target.parse().unwrap();
    let work = client.get(uri).and_then(|res| {
        res.body().for_each(|chunk| {
            for b in chunk.iter() {
                data.push(*b);
            }
            Result::Ok(())
        })
    });
    core.run(work).unwrap();
}
