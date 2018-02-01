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
use hime_sdk::unicode::UnicodeBlock;
use hime_sdk::unicode::UnicodeCategory;
use hime_sdk::unicode::UnicodeSpan;
use hyper::Client;
use regex::Regex;
use std::u32;
use tokio_core::reactor::Core;

use std::collections::HashMap;

/// Main entry point
fn main() {
    let blocks = get_latest_blocks();
    for block in blocks.iter() {
        println!("{0}", block.name);
    }
    let categories = get_latest_categories();
    for category in categories.iter() {
        println!("{0}", category.name);
    }
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
