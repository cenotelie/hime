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

//! Module for utility APIs

pub mod iterable;
pub mod biglist;
pub mod bin;

/// Represents a reference to a structure that can be either mutable or immutable
pub enum EitherMut<'a, T: 'a> {
    /// The immutable reference
    Immutable(&'a T),
    /// The mutable reference
    Mutable(&'a mut T)
}

impl<'a, T: 'a> EitherMut<'a, T> {
    /// Gets a mutable reference
    pub fn get_mut(&mut self) -> Option<&mut T> {
        match self {
            &mut EitherMut::Mutable(ref mut data) => Some(data),
            &mut EitherMut::Immutable(ref _data) => None
        }
    }

    /// Get an immutable reference
    pub fn get(&self) -> &T {
        match self {
            &EitherMut::Mutable(ref data) => data,
            &EitherMut::Immutable(ref data) => data
        }
    }
}
