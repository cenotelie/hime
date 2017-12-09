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

use std::ops::Index;
use std::ops::IndexMut;
use super::iterable::Iterable;


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
impl<T: Copy> Index<usize> for BigList<T> {
    type Output = T;
    fn index(&self, index: usize) -> &T {
        &self.chunks[index >> UPPER_SHIFT][index & LOWER_MASK]
    }
}

/// Implementation of the indexer [] operator for mutable BigList
impl<T: Copy> IndexMut<usize> for BigList<T> {
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
