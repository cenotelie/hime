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

use std::io;

/// Defines the `Iterable` trait for structures that can be iterated over
pub trait Iterable<'a> {
    type Item;
    type IteratorType: Iterator<Item=Self::Item>;
    fn iter(&'a self) -> Self::IteratorType;
}

/// The number of bits allocated to the lowest part of the index (within a chunk)
const UPPER_SHIFT: usize = 8;
/// The size of the chunks
const CHUNKS_SIZE: usize = 1 << UPPER_SHIFT;
/// Bit mask for the lowest part of the index (within a chunk)
const LOWER_MASK: usize = CHUNKS_SIZE - 1;
/// Initial size of the higher array (pointers to the chunks)
const INIT_CHUNK_COUNT: usize = CHUNKS_SIZE;

/// Represents a list of items that is efficient in storage and addition.
/// Items cannot be removed or inserted.
pub struct BigList<T: Copy> {
    /// the neutral element
    neutral: T,
    /// The data
    chunks: Vec<[T; CHUNKS_SIZE]>,
    /// The index of the current chunk
    chunk_index: usize,
    /// The index of the next available cell within the current chunk
    cell_index: usize
}

/// Implementation of BigList
impl<T: Copy> BigList<T> {
    /// Creates a (empty) list
    pub fn new(neutral: T) -> BigList<T> {
        let mut my_chunks = Vec::<[T; CHUNKS_SIZE]>::with_capacity(INIT_CHUNK_COUNT);
        my_chunks.push([neutral; CHUNKS_SIZE]);
        BigList {
            neutral,
            chunks: my_chunks,
            chunk_index: 0,
            cell_index: 0
        }
    }

    pub fn size(&self) -> usize {
        (self.chunk_index * CHUNKS_SIZE) + self.cell_index
    }

    /// Adds a value at the end of the list
    pub fn add(&mut self, value: T) -> usize {
        if self.cell_index == CHUNKS_SIZE {
            self.add_chunk();
        }
        self.chunks[self.chunk_index][self.cell_index] = value;
        let result = self.chunk_index << UPPER_SHIFT | self.cell_index;
        self.cell_index = self.cell_index + 1;
        result
    }

    /// Adds a new chunk to this list
    fn add_chunk(&mut self) {
        if self.chunk_index == self.chunks.len() - 1 {
            // we are currently on the last chunk for the allocated ones
            // allocate a new one
            self.chunks.push([self.neutral; CHUNKS_SIZE]);
        }
        self.chunk_index = self.chunk_index + 1;
        self.cell_index = 0;
    }
}

/// Implementation of the indexer operator for immutable BigList
impl<T: Copy> ::std::ops::Index<usize> for BigList<T> {
    type Output = T;
    fn index(&self, index: usize) -> &T {
        &self.chunks[index >> UPPER_SHIFT][index & LOWER_MASK]
    }
}

/// Implementation of the indexer [] operator for mutable BigList
impl<T: Copy> ::std::ops::IndexMut<usize> for BigList<T> {
    fn index_mut(&mut self, index: usize) -> &mut T {
        &mut self.chunks[index >> UPPER_SHIFT][index & LOWER_MASK]
    }
}

/// An iterator over a BigList
pub struct BigListIterator<'a, T: 'a + Copy> {
    /// The parent list
    list: &'a BigList<T>,
    /// The current index within the list
    index: usize
}

/// Implementation of the `Iterator` trait for `BigListIterator`
impl<'a, T: 'a + Copy> Iterator for BigListIterator<'a, T> {
    type Item = T;
    fn next(&mut self) -> Option<Self::Item> {
        if self.index >= self.list.size() {
            None
        } else {
            let result = self.list[self.index];
            self.index = self.index + 1;
            Some(result)
        }
    }
}

/// Implementation of `Iterable` for `BigList`
impl<'a, T: 'a + Copy> Iterable<'a> for BigList<T> {
    type Item = T;
    type IteratorType = BigListIterator<'a, T>;
    fn iter(&'a self) -> Self::IteratorType {
        BigListIterator {
            list: &self,
            index: 0
        }
    }
}

#[test]
fn test_big_list() {
    let mut list = BigList::<char>::new('\0');
    assert_eq!(list.size(), 0);
    list.add('t');
    assert_eq!(list.size(), 1);
    assert_eq!(list[0], 't');
    for x in list.iter() {
        assert_eq!(x, 't');
    }
}


/// Provides an iterator of UTF-16 code points
/// over an input of bytes assumed to represent UTF-16 code points
pub struct Utf16IteratorRaw<'a> {
    /// whether to use big-endian or little-endian
    big_endian: bool,
    /// The input reader
    input: &'a mut io::Read
}

impl<'a> Iterator for Utf16IteratorRaw<'a> {
    type Item = super::Utf16C;
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
    next: Option<super::Utf16C>
}

impl<'a> Iterator for Utf16IteratorOverUtf8<'a> {
    type Item = super::Utf16C;
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
            return Some(c as super::Utf16C);
        }
        // we need to encode
        let temp = c - 0x10000;
        let lead = (temp >> 10) + 0xD800;
        let trail = (temp & 0x03FF) + 0xDC00;
        // store the trail and return the lead
        self.next = Some(trail as super::Utf16C);
        Some(lead as super::Utf16C)
    }
}

impl<'a> Utf16IteratorOverUtf8<'a> {
    /// Creates a new instance of the iterator
    pub fn new(input: &'a mut io::Read) -> Utf16IteratorOverUtf8 {
        Utf16IteratorOverUtf8 { input, next: None }
    }
}