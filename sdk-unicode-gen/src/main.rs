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

use std::collections::HashMap;
use std::error::Error;
use std::fs;
use std::io::{self, Write};

use hime_sdk::unicode::{Block, Category};
use regex::Regex;

/// The name of this program
pub const CRATE_NAME: &str = env!("CARGO_PKG_NAME");
/// The version of this program
pub const CRATE_VERSION: &str = env!("CARGO_PKG_VERSION");
/// The commit that was used to build the application
pub const GIT_HASH: &str = env!("GIT_HASH");
/// The git tag that was used to build the application
pub const GIT_TAG: &str = env!("GIT_TAG");

/// The URL of the latest specification of Unicode blocks
const URL_UNICODE_BLOCKS: &str = "http://www.unicode.org/Public/UCD/latest/ucd/Blocks.txt";
/// The URL of the latest specification of Unicode code points
const URL_UNICODE_DATA: &str = "http://www.unicode.org/Public/UCD/latest/ucd/UnicodeData.txt";

/// Gets the latest unicode blocks from the Unicode web site
fn get_latet_blocks() -> Result<Vec<Block>, Box<dyn Error>> {
    let content = reqwest::blocking::get(URL_UNICODE_BLOCKS)?.text()?;
    let mut blocks = Vec::new();
    let re = Regex::new(r"([0-9A-F]+)\.\.([0-9A-F]+); (([a-zA-Z0-9 ]|-)+)")?;
    for line in content.split('\n') {
        if line.is_empty() || line.starts_with('#') {
            continue;
        }
        if let Some(m) = re.captures(line) {
            let begin = u32::from_str_radix(&m[1], 16)?;
            let end = u32::from_str_radix(&m[2], 16)?;
            let name = &m[3];
            if name.contains("Surrogate") {
                continue;
            }
            let name = name.replace(' ', "");
            blocks.push(Block::new_owned(name, begin, end));
        }
    }
    blocks.sort_unstable_by(|a, b| a.name.cmp(&b.name));
    Ok(blocks)
}

/// Gets the latest unicode categories from the Unicode web site
fn get_latet_categories() -> Result<Vec<Category>, Box<dyn Error>> {
    let content = reqwest::blocking::get(URL_UNICODE_DATA)?.text()?;
    let mut categories = HashMap::new();
    let re = Regex::new(r"([0-9A-F]+);([^;]+);([^;]+);.*")?;
    let mut current_name: Option<String> = None;
    let mut current_span_begin: Option<u32> = None;
    let mut last_codepoint: Option<u32> = None;
    for line in content.split('\n') {
        if line.is_empty() || line.starts_with('#') {
            continue;
        }
        if let Some(m) = re.captures(line) {
            let cp = u32::from_str_radix(&m[1], 16)?;
            let cat = &m[3];
            if Some(cat) != current_name.as_deref() {
                if let Some(ref current_name) = &current_name {
                    let category = categories
                        .entry(current_name.clone())
                        .or_insert_with(|| Category::new_owned(current_name.clone()));
                    if let (Some(begin), Some(last)) = (current_span_begin, last_codepoint) {
                        if !(0xD800..0xE000).contains(&begin) && !(0xD800..0xE000).contains(&last) {
                            category.add_span(begin, last);
                        }
                    }
                }
                current_name = Some(cat.to_string());
                current_span_begin = Some(cp);
            }
            last_codepoint = Some(cp);
        }
    }
    if let (Some(ref current_name), Some(begin), Some(last)) =
        (&current_name, current_span_begin, last_codepoint)
    {
        let category = categories
            .entry(current_name.clone())
            .or_insert_with(|| Category::new_owned(current_name.clone()));
        category.add_span(begin, last);
    }
    let mut categories = categories.into_iter().map(|(_, v)| v).collect::<Vec<_>>();
    categories.sort_unstable_by(|a, b| a.name.cmp(&b.name));
    Ok(categories)
}

/// Generates the code for the Unicode blocks data
fn generate_blocks_db(blocks: &[Block]) -> Result<(), Box<dyn Error>> {
    let mut writer = io::BufWriter::new(fs::File::create("blocks.rs")?);
    writeln!(writer, "/*")?;
    writeln!(writer, " * WARNING: this file has been generated by")?;
    writeln!(writer, " * Hime Parser Generator")?;
    writeln!(writer, " */")?;
    writeln!(writer)?;
    writeln!(writer, "use crate::unicode::Block;")?;
    writeln!(writer, "use std::collections::HashMap;")?;
    writeln!(writer)?;
    writeln!(writer, "/// Gets all blocks")?;
    writeln!(
        writer,
        "pub fn get_blocks() -> HashMap<&'static str, Block> {{"
    )?;
    writeln!(writer, "    let mut db = HashMap::new();")?;
    for block in blocks {
        writeln!(
            writer,
            "    db.insert(\"{}\", Block::new(\"{}\", 0x{:X}, 0x{:X}));",
            &block.name,
            &block.name,
            block.span.begin.value(),
            block.span.end.value()
        )?;
    }
    writeln!(writer, "    db")?;
    writeln!(writer, "}}")?;
    Ok(())
}

/// Generates the code for the Unicode blocks tests
fn generate_blocks_tests(blocks: &[Block]) -> Result<(), Box<dyn Error>> {
    let mut writer = io::BufWriter::new(fs::File::create("UnicodeBlocks.suite")?);
    writeln!(writer, "fixture UnicodeBlocks")?;
    for block in blocks {
        let cs_name = block.name.replace('-', "");
        let value = block.span.begin.value();
        let value = if value <= 0xFFFF {
            format!("\\u{:04X}", value)
        } else {
            format!("\\u{:08X}", value)
        };
        writeln!(writer)?;
        writeln!(writer, "test Test_UnicodeBlock_{}_LeftBound:", &cs_name)?;
        writeln!(writer, "\tgrammar Test_UnicodeBlock_{}_LeftBound {{ options {{Axiom=\"e\";}} terminals {{X->ub{{{}}};}} rules {{ e->X; }} }}", &cs_name, &block.name)?;
        writeln!(writer, "\tparser LALR1")?;
        writeln!(writer, "\ton \"{}\"", &value)?;
        writeln!(writer, "\tyields e(X='{}')", &value)?;
        let value = block.span.end.value();
        let value = if value <= 0xFFFF {
            format!("\\u{:04X}", value)
        } else {
            format!("\\u{:08X}", value)
        };
        writeln!(writer)?;
        writeln!(writer, "test Test_UnicodeBlock_{}_RightBound:", &cs_name)?;
        writeln!(writer, "\tgrammar Test_UnicodeBlock_{}_RightBound {{ options {{Axiom=\"e\";}} terminals {{X->ub{{{}}};}} rules {{ e->X; }} }}", &cs_name, &block.name)?;
        writeln!(writer, "\tparser LALR1")?;
        writeln!(writer, "\ton \"{}\"", &value)?;
        writeln!(writer, "\tyields e(X='{}')", &value)?;
    }
    Ok(())
}

/// Generates the code for the Unicode categories data
fn generate_categories_db(categories: &[Category]) -> Result<(), Box<dyn Error>> {
    let mut aggragated = HashMap::new();
    for category in categories {
        let first = category.name[0..1].to_string();
        aggragated
            .entry(first)
            .or_insert_with(Vec::new)
            .push(category);
    }
    let mut writer = io::BufWriter::new(fs::File::create("categories.rs")?);
    writeln!(writer, "/*")?;
    writeln!(writer, " * WARNING: this file has been generated by")?;
    writeln!(writer, " * Hime Parser Generator")?;
    writeln!(writer, " */")?;
    writeln!(writer)?;
    writeln!(writer, "use crate::unicode::Category;")?;
    writeln!(writer, "use std::collections::HashMap;")?;
    writeln!(writer)?;
    writeln!(writer, "/// Gets all categories")?;
    writeln!(
        writer,
        "pub fn get_categories() -> HashMap<&'static str, Category> {{"
    )?;
    writeln!(writer, "    let mut db = HashMap::new();")?;
    for category in categories {
        let lower_case = category.name.to_lowercase();
        writeln!(
            writer,
            "    let {}cat_{} = Category::new(\"{}\");",
            if category.spans.is_empty() {
                ""
            } else {
                "mut "
            },
            &lower_case,
            &category.name
        )?;
        for span in &category.spans {
            writeln!(
                writer,
                "    cat_{}.add_span(0x{:X}, 0x{:X});",
                &lower_case,
                span.begin.value(),
                span.end.value()
            )?;
        }
    }
    for (key, categories) in aggragated.iter() {
        let lower_case = key.to_lowercase();
        writeln!(
            writer,
            "    let mut cat_{} = Category::new(\"{}\");",
            &lower_case, &key
        )?;
        for category in categories.iter() {
            let sub_lower_case = category.name.to_lowercase();
            writeln!(
                writer,
                "    cat_{}.aggregate(&cat_{});",
                &lower_case, &sub_lower_case
            )?;
        }
    }
    for category in categories {
        let lower_case = category.name.to_lowercase();
        writeln!(
            writer,
            "    db.insert(\"{}\", cat_{});",
            &category.name, &lower_case
        )?;
    }
    for key in aggragated.keys() {
        let lower_case = key.to_lowercase();
        writeln!(writer, "    db.insert(\"{}\", cat_{});", &key, &lower_case,)?;
    }
    writeln!(writer, "    db")?;
    writeln!(writer, "}}")?;
    Ok(())
}

pub fn main() {
    println!(
        "{} {} tag={} hash={}",
        CRATE_NAME, CRATE_VERSION, GIT_TAG, GIT_HASH
    );
    println!("Retrieving and building blocks db ...");
    let blocks = get_latet_blocks().unwrap();
    println!("Generating blocks db ...");
    generate_blocks_db(&blocks).unwrap();
    println!("Generating blocks tests ...");
    generate_blocks_tests(&blocks).unwrap();
    println!("Retrieving and building categories db ...");
    let categories = get_latet_categories().unwrap();
    println!("Generating categories db ...");
    generate_categories_db(&categories).unwrap();
}
