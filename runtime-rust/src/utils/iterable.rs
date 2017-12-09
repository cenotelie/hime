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

/// Defines the `Iterable` trait for structures that can be iterated over.
/// Contrary to the `IntoIterator` trait, this trait does not mandate the underlying structure
/// be consumed in the process. This trait enables the user to retrieve an iterator to iterate
/// over the structure that implements it.
///
/// # Examples
///
/// Implementing `Iterable` for your type:
///
/// ```
/// extern crate hime_redist;
/// use ::hime_redist::utils::iterable::Iterable;
///
/// #[derive(Debug)]
/// struct MyCollection(Vec<i32>);
///
/// // Let's give it some methods so we can create one and add things
/// // to it.
/// impl MyCollection {
///     fn new() -> MyCollection {
///         MyCollection(Vec::new())
///     }
///
///     fn add(&mut self, elem: i32) {
///         self.0.push(elem);
///     }
/// }
///
/// // implement an iterator for this collection
/// struct MyCollectionIterator<'a> {
///     collection: &'a MyCollection,
///     index: usize
/// }
///
/// impl<'a> Iterator for MyCollectionIterator<'a> {
///     type Item = i32;
///     fn next(&mut self) -> Option<Self::Item> {
///         if self.index >= self.collection.0.len() {
///             None
///         } else {
///             let result = self.collection.0[self.index];
///             self.index = self.index + 1;
///             Some(result)
///         }
///     }
/// }
///
/// // and we'll implement Iterable
/// impl<'a> Iterable<'a> for MyCollection {
///     type Item = i32;
///     type IteratorType = MyCollectionIterator<'a>;
///     fn iter(&'a self) -> Self::IteratorType {
///         MyCollectionIterator {
///             collection: &self,
///             index: 0
///         }
///     }
/// }
///
/// fn main() {
///     // Now we can make a new collection...
///     let mut c = MyCollection::new();
///
///     // ... add some stuff to it ...
///     c.add(0);
///     c.add(1);
///     c.add(2);
///
///     // ... and then turn it into an Iterator:
///     for (i, n) in c.iter().enumerate() {
///         assert_eq!(i as i32, n);
///     }
/// }
/// ```
pub trait Iterable<'a> {
    /// The type of the elements being iterated over.
    type Item;
    /// The type of the iterator.
    type IteratorType: Iterator<Item=Self::Item>;
    /// Creates an iterator over this `Iterable` structure.
    fn iter(&'a self) -> Self::IteratorType;
}