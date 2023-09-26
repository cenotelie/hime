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

use core::ops::{Deref, DerefMut};

pub mod biglist;
pub mod bin;

/// Represents a reference to a structure that can be either mutable or immutable
pub enum EitherMut<'a, T: 'a> {
    /// The immutable reference
    Immutable(&'a T),
    /// The mutable reference
    Mutable(&'a mut T)
}

impl<'a, T: 'a> Deref for EitherMut<'a, T> {
    type Target = T;

    fn deref(&self) -> &Self::Target {
        match self {
            EitherMut::Mutable(data) => data,
            EitherMut::Immutable(data) => data
        }
    }
}

impl<'a, T: 'a> DerefMut for EitherMut<'a, T> {
    fn deref_mut(&mut self) -> &mut T {
        match self {
            EitherMut::Mutable(data) => data,
            EitherMut::Immutable(_) => panic!("Expected a mutable reference")
        }
    }
}

/// Represents a resource that is either owned,
/// or exclusively held with a a mutable reference
pub enum OwnOrMut<'a, T> {
    /// The resource is directly owned
    Owned(T),
    /// The resource is held through a mutable reference
    MutRef(&'a mut T)
}

impl<'a, T> Deref for OwnOrMut<'a, T> {
    type Target = T;

    fn deref(&self) -> &Self::Target {
        match self {
            OwnOrMut::Owned(data) => data,
            OwnOrMut::MutRef(data) => data
        }
    }
}

impl<'a, T> DerefMut for OwnOrMut<'a, T> {
    fn deref_mut(&mut self) -> &mut Self::Target {
        match self {
            OwnOrMut::Owned(data) => data,
            OwnOrMut::MutRef(data) => data
        }
    }
}
