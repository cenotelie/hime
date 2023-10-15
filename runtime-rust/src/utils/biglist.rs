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

//! Module for the definition of `BigList`

use core::fmt::{self, Debug, Formatter};
use alloc::vec::Vec;
use core::ops::{Index, IndexMut};

/// The number of bits allocated to the lowest part of the index (within a chunk)
const UPPER_SHIFT: usize = 8;
/// The size of the chunks
const CHUNKS_SIZE: usize = 1 << UPPER_SHIFT;
/// Bit mask for the lowest part of the index (within a chunk)
const LOWER_MASK: usize = CHUNKS_SIZE - 1;
/// Initial size of the higher array (pointers to the chunks)
const INIT_CHUNK_COUNT: usize = CHUNKS_SIZE;

/// Represents a list of items that is efficient in storage and addition.
/// Items cannot be neither be removed nor inserted.
pub struct BigList<T> {
    /// The data
    chunks: Vec<[T; CHUNKS_SIZE]>,
    /// The index of the current chunk
    chunk_index: usize,
    /// The index of the next available cell within the current chunk
    cell_index: usize,
}

impl<T: Default + Copy> Default for BigList<T> {
    fn default() -> Self {
        let mut my_chunks = Vec::with_capacity(INIT_CHUNK_COUNT);
        my_chunks.push([T::default(); CHUNKS_SIZE]);
        BigList {
            chunks: my_chunks,
            chunk_index: 0,
            cell_index: 0,
        }
    }
}

impl<T> Debug for BigList<T> {
    fn fmt(&self, f: &mut Formatter<'_>) -> fmt::Result {
        f.debug_struct("BigList").field("len", &self.len()).finish()
    }
}

impl<T: Copy> Clone for BigList<T> {
    fn clone(&self) -> Self {
        Self {
            chunks: self.chunks.clone(),
            chunk_index: self.chunk_index,
            cell_index: self.cell_index,
        }
    }
}

impl<T> BigList<T> {
    /// Gets whether the list is empty
    #[must_use]
    pub fn is_empty(&self) -> bool {
        self.chunk_index == 0 && self.cell_index == 0
    }

    /// Gets the list total length
    #[must_use]
    pub fn len(&self) -> usize {
        (self.chunk_index * CHUNKS_SIZE) + self.cell_index
    }
}

/// Implementation of `BigList`
impl<T: Default + Copy> BigList<T> {
    /// Adds a value at the end of the list
    pub fn push(&mut self, value: T) -> usize {
        if self.cell_index == CHUNKS_SIZE {
            self.add_chunk();
        }
        self.chunks[self.chunk_index][self.cell_index] = value;
        let result = self.chunk_index << UPPER_SHIFT | self.cell_index;
        self.cell_index += 1;
        result
    }

    /// Adds a new chunk to this list
    fn add_chunk(&mut self) {
        if self.chunk_index == self.chunks.len() - 1 {
            // we are currently on the last chunk for the allocated ones
            // allocate a new one
            self.chunks.push([T::default(); CHUNKS_SIZE]);
        }
        self.chunk_index += 1;
        self.cell_index = 0;
    }

    /// Gets an iterator over the list
    #[must_use]
    pub fn iter(&self) -> BigListIterator<T> {
        BigListIterator {
            list: self,
            index: 0,
        }
    }
}

/// Implementation of the indexer operator for immutable `BigList`
impl<T: Copy> Index<usize> for BigList<T> {
    type Output = T;
    fn index(&self, index: usize) -> &T {
        &self.chunks[index >> UPPER_SHIFT][index & LOWER_MASK]
    }
}

/// Implementation of the indexer [] operator for mutable `BigList`
impl<T: Copy> IndexMut<usize> for BigList<T> {
    fn index_mut(&mut self, index: usize) -> &mut T {
        &mut self.chunks[index >> UPPER_SHIFT][index & LOWER_MASK]
    }
}

/// An iterator over a `BigList`
pub struct BigListIterator<'a, T: 'a> {
    /// The parent list
    list: &'a BigList<T>,
    /// The current index within the list
    index: usize,
}

/// Implementation of the `Iterator` trait for `BigListIterator`
impl<'a, T: 'a + Copy> Iterator for BigListIterator<'a, T> {
    type Item = T;
    fn next(&mut self) -> Option<Self::Item> {
        if self.index >= self.list.len() {
            None
        } else {
            let result = self.list[self.index];
            self.index += 1;
            Some(result)
        }
    }
}

#[test]
fn test_big_list() {
    let mut list = BigList::default();
    assert_eq!(list.len(), 0);
    list.push('t');
    assert_eq!(list.len(), 1);
    assert_eq!(list[0], 't');
    for x in list.iter() {
        assert_eq!(x, 't');
    }
}
