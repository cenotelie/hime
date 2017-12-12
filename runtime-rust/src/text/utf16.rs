/*******************************************************************************
 * Copyright (c) 2017 Association Cénotélie (cenotelie.fr)
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

//! Hime parsers only works on UTF-16 encoded text.
//! This module provides facilities for interactions with UTF-16 text.

use std::io;

use super::Utf16C;

/// `Utf16IteratorRaw` provides an iterator of UTF-16 code units
/// over an input of bytes assumed to represent UTF-16 code units
pub struct Utf16IteratorRaw<'a> {
    /// whether to use big-endian or little-endian
    big_endian: bool,
    /// The input reader
    input: &'a mut io::Read
}

impl<'a> Iterator for Utf16IteratorRaw<'a> {
    type Item = Utf16C;
    fn next(&mut self) -> Option<Self::Item> {
        // read two bytes
        let mut bytes: [u8; 2] = [0; 2];
        let read = self.input.read(&mut bytes);
        if read.is_err() || read.unwrap() < 2 {
            return None;
        }
        if self.big_endian {
            Some((bytes[1] as u16) << 8 | (bytes[0] as u16))
        } else {
            Some((bytes[0] as u16) << 8 | (bytes[1] as u16))
        }
    }
}

impl<'a> Utf16IteratorRaw<'a> {
    /// Creates a new instance of the iterator
    pub fn new(input: &'a mut io::Read, big_endian: bool) -> Utf16IteratorRaw {
        Utf16IteratorRaw { big_endian, input }
    }
}

/// Provides an iterator of UTF-16 code points
/// over an input of bytes assumed to represent UTF-8 code points
pub struct Utf16IteratorOverUtf8<'a> {
    /// The input reader
    input: &'a mut io::Read,
    /// The next UTF-16 code point, if any
    next: Option<Utf16C>
}

impl<'a> Iterator for Utf16IteratorOverUtf8<'a> {
    type Item = Utf16C;
    fn next(&mut self) -> Option<Self::Item> {
        // do we have a cached
        if !self.next.is_none() {
            let result = self.next;
            self.next = None;
            return result;
        }
        // read the next byte
        let mut bytes: [u8; 1] = [0; 1];
        {
            let read = self.input.read(&mut bytes);
            if read.is_err() || read.unwrap() < 3 { return None; }
        }
        let b0 = bytes[0] as u8;

        let c = match b0 {
            _ if b0 >> 3 == 0b11110 => {
                // this is 4 bytes encoding
                let mut others: [u8; 3] = [0; 3];
                let read = self.input.read(&mut others);
                if read.is_err() || read.unwrap() < 3 { return None; }
                ((b0 as u32) & 0b00000111) << 18
                    | ((others[0] as u32) & 0b00111111) << 12
                    | ((others[1] as u32) & 0b00111111) << 6
                    | ((others[0] as u32) & 0b00111111)
            }
            _ if b0 >> 4 == 0b1110 => {
                // this is a 3 bytes encoding
                let mut others: [u8; 2] = [0; 2];
                let read = self.input.read(&mut others);
                if read.is_err() || read.unwrap() < 2 { return None; }
                ((b0 as u32) & 0b00001111) << 12
                    | ((others[1] as u32) & 0b00111111) << 6
                    | ((others[0] as u32) & 0b00111111)
            }
            _ if b0 >> 5 == 0b110 => {
                // this is a 2 bytes encoding
                let read = self.input.read(&mut bytes);
                if read.is_err() || read.unwrap() < 1 { return None; }
                ((b0 as u32) & 0b00011111) << 6
                    | ((bytes[0] as u32) & 0b00111111)
            }
            _ if b0 >> 7 == 0 => {
                // this is a 1 byte encoding
                b0 as u32
            }
            _ => {
                return None;
            }
        };

        // now we have the decoded unicode character
        // encode it in UTF-16
        if (c >= 0xD7FF && c < 0xE000) || c >= 0x110000 {
            // not a valid unicode character
            return None;
        }
        if c <= 0xFFFF {
            // simple case
            return Some(c as Utf16C);
        }
        // we need to encode
        let temp = c - 0x10000;
        let lead = (temp >> 10) + 0xD800;
        let trail = (temp & 0x03FF) + 0xDC00;
        // store the trail and return the lead
        self.next = Some(trail as Utf16C);
        Some(lead as Utf16C)
    }
}

impl<'a> Utf16IteratorOverUtf8<'a> {
    /// Creates a new instance of the iterator
    pub fn new(input: &'a mut io::Read) -> Utf16IteratorOverUtf8 {
        Utf16IteratorOverUtf8 { input, next: None }
    }
}