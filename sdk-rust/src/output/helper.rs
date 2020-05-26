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

//! Module for the helpers API for the emitters

use std::io::{self, Write};

const UPPER_A: u8 = 0x41;
const UPPER_Z: u8 = 0x5A;
const LOWER_A: u8 = 0x61;
const LOWER_Z: u8 = 0x7A;
const DIGIT0: u8 = 0x30;
const DIGIT9: u8 = 0x39;
const UNDERSCORE: u8 = 0x5F;
const TO_LOWER: u8 = LOWER_A - UPPER_A;

/// Converts a name to upper camel case
pub fn to_upper_camel_case(name: &str) -> String {
    let mut result = Vec::<u8>::new();
    let mut new_word = false;
    for (i, c) in name.bytes().enumerate() {
        if i == 0 {
            if c >= UPPER_A && c <= UPPER_Z {
                result.push(c);
            } else if c >= LOWER_A && c <= LOWER_Z {
                result.push(c - TO_LOWER);
            } else if c >= DIGIT0 && c <= DIGIT9 {
                result.push(UNDERSCORE);
                result.push(c);
                new_word = true;
            } else {
                new_word = true;
            }
        } else {
            let is_empty = result.is_empty();
            if c >= UPPER_A && c <= UPPER_Z {
                if is_empty || new_word {
                    result.push(c);
                } else {
                    let x = (&name as &dyn AsRef<[u8]>).as_ref()[i - 1];
                    if x >= UPPER_A && x <= UPPER_Z {
                        result.push(c + TO_LOWER);
                    } else {
                        result.push(c);
                    }
                }
                new_word = false;
            } else if c >= LOWER_A && c <= LOWER_Z {
                result.push(if new_word { c - TO_LOWER } else { c });
                new_word = false;
            } else if c >= DIGIT0 && c <= DIGIT9 {
                result.push(c);
                new_word = true;
            } else {
                new_word = true;
            }
        }
    }
    String::from_utf8(result).unwrap()
}

#[test]
fn test_upper_camel_case() {
    assert_eq!("", to_upper_camel_case(""));
    assert_eq!("UpperCamelCase", to_upper_camel_case("upper_camel_case"));
    assert_eq!("UpperCamelCase", to_upper_camel_case("UPPER_CAMEL_CASE"));
    assert_eq!("UpperCamelCase", to_upper_camel_case("UpperCamelCase"));
    assert_eq!("UpperCamelCase", to_upper_camel_case("upperCamelCase"));
    assert_eq!("UpperCamelCase", to_upper_camel_case("Upper Camel Case"));
}

/// Converts a name to lower camel case
pub fn to_lower_camel_case(name: &str) -> String {
    let mut result = Vec::<u8>::new();
    let mut new_word = false;
    for (i, c) in name.bytes().enumerate() {
        if i == 0 {
            if c >= UPPER_A && c <= UPPER_Z {
                result.push(c + TO_LOWER);
            } else if c >= LOWER_A && c <= LOWER_Z {
                result.push(c);
            } else if c >= DIGIT0 && c <= DIGIT9 {
                result.push(UNDERSCORE);
                result.push(c);
                new_word = true;
            }
        } else {
            let is_empty = result.is_empty();
            if c >= UPPER_A && c <= UPPER_Z {
                if is_empty {
                    result.push(c + TO_LOWER);
                } else if new_word {
                    result.push(c);
                } else {
                    let x = (&name as &dyn AsRef<[u8]>).as_ref()[i - 1];
                    if x >= UPPER_A && x <= UPPER_Z {
                        result.push(c + TO_LOWER);
                    } else {
                        result.push(c);
                    }
                }
                new_word = false;
            } else if c >= LOWER_A && c <= LOWER_Z {
                result.push(if new_word { c - TO_LOWER } else { c });
                new_word = false;
            } else if c >= DIGIT0 && c <= DIGIT9 {
                result.push(c);
                new_word = true;
            } else {
                new_word = true;
            }
        }
    }
    String::from_utf8(result).unwrap()
}

#[test]
fn test_lower_camel_case() {
    assert_eq!("", to_lower_camel_case(""));
    assert_eq!("lowerCamelCase", to_lower_camel_case("lower_camel_case"));
    assert_eq!("lowerCamelCase", to_lower_camel_case("LOWER_CAMEL_CASE"));
    assert_eq!("lowerCamelCase", to_lower_camel_case("LowerCamelCase"));
    assert_eq!("lowerCamelCase", to_lower_camel_case("lowerCamelCase"));
    assert_eq!("lowerCamelCase", to_lower_camel_case("Lower Camel Case"));
}

/// Converts a name to upper case
pub fn to_upper_case(name: &str) -> String {
    let mut result = Vec::<u8>::new();
    for (i, c) in name.bytes().enumerate() {
        if i == 0 {
            if c >= UPPER_A && c <= UPPER_Z {
                result.push(c);
            } else if c >= LOWER_A && c <= LOWER_Z {
                result.push(c - TO_LOWER);
            } else if c >= DIGIT0 && c <= DIGIT9 {
                result.push(UNDERSCORE);
                result.push(c);
            } else {
                result.push(UNDERSCORE);
            }
        } else if c >= UPPER_A && c <= UPPER_Z {
            let x = (&name as &dyn AsRef<[u8]>).as_ref()[i - 1];
            if (x >= LOWER_A && x <= LOWER_Z) || (x >= DIGIT0 && x <= DIGIT9) {
                // preceded by a lower-case character or a number, this is a new word
                result.push(UNDERSCORE);
            }
            result.push(c);
        } else if c >= LOWER_A && c <= LOWER_Z {
            result.push(c - TO_LOWER);
        } else if c >= DIGIT0 && c <= DIGIT9 {
            result.push(c);
        } else {
            result.push(UNDERSCORE);
        }
    }
    String::from_utf8(result).unwrap()
}

#[test]
fn test_upper_case() {
    assert_eq!("", to_upper_case(""));
    assert_eq!("UPPER_CASE", to_upper_case("upper_case"));
    assert_eq!("UPPER_CASE", to_upper_case("UPPER_CASE"));
    assert_eq!("UPPER_CASE", to_upper_case("UpperCase"));
    assert_eq!("UPPER_CASE", to_upper_case("upperCase"));
    assert_eq!("UPPER_CASE", to_upper_case("Upper Case"));
}

/// Converts a name to snake case
pub fn to_snake_case(name: &str) -> String {
    let mut result = Vec::<u8>::new();
    for (i, c) in name.bytes().enumerate() {
        if i == 0 {
            if c >= UPPER_A && c <= UPPER_Z {
                result.push(c + TO_LOWER);
            } else if c >= LOWER_A && c <= LOWER_Z {
                result.push(c);
            } else if c >= DIGIT0 && c <= DIGIT9 {
                result.push(UNDERSCORE);
                result.push(c);
            } else {
                result.push(UNDERSCORE);
            }
        } else if c >= UPPER_A && c <= UPPER_Z {
            let x = (&name as &dyn AsRef<[u8]>).as_ref()[i - 1];
            if (x >= LOWER_A && x <= LOWER_Z) || (x >= DIGIT0 && x <= DIGIT9) {
                // preceded by a lower-case character or a number, this is a new word
                result.push(UNDERSCORE);
            }
            result.push(c + TO_LOWER);
        } else if (c >= LOWER_A && c <= LOWER_Z) || (c >= DIGIT0 && c <= DIGIT9) {
            result.push(c);
        } else {
            result.push(UNDERSCORE);
        }
    }
    String::from_utf8(result).unwrap()
}

#[test]
fn test_snake_case() {
    assert_eq!("", to_snake_case(""));
    assert_eq!("snake_case", to_snake_case("snake_case"));
    assert_eq!("snake_case", to_snake_case("SNAKE_CASE"));
    assert_eq!("snake_case", to_snake_case("SnakeCase"));
    assert_eq!("snake_case", to_snake_case("snakeCase"));
    assert_eq!("snake_case", to_snake_case("Snake Case"));
}

/// writes a u16 to a byte stream
pub fn write_u16(writer: &mut dyn Write, value: u16) -> Result<(), io::Error> {
    let buffer: [u8; 2] = [(value & 0xFF) as u8, (value >> 8) as u8];
    writer.write_all(&buffer)
}

/// writes a u32 to a byte stream
pub fn write_u32(writer: &mut dyn Write, value: u32) -> Result<(), io::Error> {
    let buffer: [u8; 4] = [
        (value & 0xFF) as u8,
        ((value & 0x0000_FF00) >> 8) as u8,
        ((value & 0x00FF_0000) >> 16) as u8,
        (value >> 24) as u8
    ];
    writer.write_all(&buffer)
}
