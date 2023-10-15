/*******************************************************************************
 * Copyright (c) 2023 Association Cénotélie (cenotelie.fr)
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

//! Module for Shared-Packed Parse Forest

use alloc::vec::Vec;
use core::fmt::{Display, Error, Formatter};
use core::iter::FusedIterator;
use core::ops::Index;

use serde::ser::{SerializeSeq, SerializeStruct};
use serde::{Serialize, Serializer};

use crate::ast::{TableElemRef, TableType};
use crate::parsers::TreeAction;
use crate::symbols::{SemanticElementTrait, Symbol};
use crate::text::{TextContext, TextPosition, TextSpan};
use crate::tokens::{Token, TokenRepository};

/// Represents a reference to a Shared-Packed Parse Forest node in a specific version
#[derive(Debug, Default, Copy, Clone, PartialEq, Eq, Hash)]
pub struct SppfImplNodeVersRef {
    /// The identifier of the node
    pub node_id: u32,
    /// The version to refer to
    pub version: u32,
}

impl SppfImplNodeVersRef {
    /// Creates a new reference to a node version
    #[must_use]
    pub const fn new(node_id: u32, version: u32) -> Self {
        Self { node_id, version }
    }

    /// Creates a new reference to a node version
    #[must_use]
    pub fn new_usize(node_id: usize, version: usize) -> Self {
        Self {
            node_id: node_id as u32,
            version: version as u32,
        }
    }

    /// Gets the identifier of the represented node
    #[must_use]
    pub fn node_id(self) -> usize {
        self.node_id as usize
    }
}

impl Display for SppfImplNodeVersRef {
    fn fmt(&self, f: &mut Formatter) -> Result<(), Error> {
        write!(f, "[{}+{}]", self.node_id, self.version)
    }
}

/// Represents a reference to a Shared-Packed Parse Forest node
#[derive(Debug, Default, Copy, Clone, PartialEq, Eq, Hash)]
pub struct SppfImplNodeRef {
    /// The identifier of the node
    pub node_id: u32,
}

/// The flag for replaceable nodes in a `SppfImplNodeRef`
const REPLACEABLE_FLAG: u32 = 0x8000_0000;

impl SppfImplNodeRef {
    /// Creates a new reference to a node version
    #[must_use]
    pub const fn new(node_id: u32) -> Self {
        Self { node_id }
    }

    /// Creates a new reference to a node version
    #[must_use]
    pub fn new_usize(node_id: usize) -> Self {
        Self {
            node_id: node_id as u32,
        }
    }

    /// Creates a new reference to a node version
    #[must_use]
    pub fn new_replaceable(index: usize) -> Self {
        Self {
            node_id: (index as u32) | REPLACEABLE_FLAG,
        }
    }

    /// Creates a reference to the node in a specific version
    #[must_use]
    pub fn with_version(self, version: u32) -> SppfImplNodeVersRef {
        SppfImplNodeVersRef {
            node_id: self.node_id,
            version,
        }
    }

    /// Creates a reference to the node in a specific version
    #[must_use]
    pub fn with_version_usize(self, version: usize) -> SppfImplNodeVersRef {
        SppfImplNodeVersRef {
            node_id: self.node_id,
            version: version as u32,
        }
    }

    /// Gets the identifier of the represented node
    #[must_use]
    pub fn node_id(self) -> usize {
        (self.node_id & !REPLACEABLE_FLAG) as usize
    }

    /// Gets whether the node is a replaceable node
    #[must_use]
    pub fn is_replaceable(self) -> bool {
        (self.node_id & REPLACEABLE_FLAG) != 0
    }
}

impl From<SppfImplNodeVersRef> for SppfImplNodeRef {
    fn from(value: SppfImplNodeVersRef) -> Self {
        Self {
            node_id: value.node_id,
        }
    }
}

impl Display for SppfImplNodeRef {
    fn fmt(&self, f: &mut Formatter) -> Result<(), Error> {
        write!(f, "[{}]", self.node_id)
    }
}

/// The children for a SPPF node
#[derive(Debug, Default, Clone, PartialEq, Eq)]
pub enum SppfImplNodeChildren {
    /// No children
    #[default]
    None,
    /// A single child
    Single(SppfImplNodeRef),
    /// Two children
    Two([SppfImplNodeRef; 2]),
    /// Three children
    Three([SppfImplNodeRef; 3]),
    /// Four children
    Four([SppfImplNodeRef; 4]),
    /// More than four children
    More(Vec<SppfImplNodeRef>),
}

impl From<&[SppfImplNodeRef]> for SppfImplNodeChildren {
    fn from(children: &[SppfImplNodeRef]) -> Self {
        let count = children.len();
        if count == 0 {
            SppfImplNodeChildren::None
        } else if count == 1 {
            SppfImplNodeChildren::Single(children[0])
        } else if count == 2 {
            SppfImplNodeChildren::Two([children[0], children[1]])
        } else if count == 3 {
            SppfImplNodeChildren::Three([children[0], children[1], children[2]])
        } else if count == 4 {
            SppfImplNodeChildren::Four([children[0], children[1], children[2], children[3]])
        } else {
            SppfImplNodeChildren::More(children.to_vec())
        }
    }
}

impl SppfImplNodeChildren {
    /// Gets the number of children
    #[must_use]
    pub fn len(&self) -> usize {
        match self {
            SppfImplNodeChildren::None => 0,
            SppfImplNodeChildren::Single(_) => 1,
            SppfImplNodeChildren::Two(_) => 2,
            SppfImplNodeChildren::Three(_) => 3,
            SppfImplNodeChildren::Four(_) => 4,
            SppfImplNodeChildren::More(data) => data.len(),
        }
    }

    /// Gets whether there are no children
    #[must_use]
    pub fn is_empty(&self) -> bool {
        matches!(self, SppfImplNodeChildren::None)
    }
}

impl Index<usize> for SppfImplNodeChildren {
    type Output = SppfImplNodeRef;

    fn index(&self, index: usize) -> &Self::Output {
        match self {
            SppfImplNodeChildren::Single(child) => child,
            SppfImplNodeChildren::Two(children) => &children[index],
            SppfImplNodeChildren::Three(children) => &children[index],
            SppfImplNodeChildren::Four(children) => &children[index],
            SppfImplNodeChildren::More(children) => &children[index],
            SppfImplNodeChildren::None => todo!(),
        }
    }
}

impl<'a> IntoIterator for &'a SppfImplNodeChildren {
    type Item = SppfImplNodeRef;
    type IntoIter = SppfImplNodeChildrenIterator<'a>;

    fn into_iter(self) -> Self::IntoIter {
        SppfImplNodeChildrenIterator {
            children: self,
            index: 0,
            end: self.len(),
        }
    }
}

/// Iterator over SPPF children nodes
pub struct SppfImplNodeChildrenIterator<'a> {
    /// The children
    children: &'a SppfImplNodeChildren,
    /// The next index
    index: usize,
    /// The last index (excluded)
    end: usize,
}

impl<'a> Iterator for SppfImplNodeChildrenIterator<'a> {
    type Item = SppfImplNodeRef;

    fn next(&mut self) -> Option<Self::Item> {
        match (&self.children, self.index) {
            (SppfImplNodeChildren::Single(child), 0) => {
                self.index += 1;
                Some(*child)
            }
            (SppfImplNodeChildren::Two(children), index) if index < self.end => {
                self.index += 1;
                Some(children[index])
            }
            (SppfImplNodeChildren::Three(children), index) if index < self.end => {
                self.index += 1;
                Some(children[index])
            }
            (SppfImplNodeChildren::Four(children), index) if index < self.end => {
                self.index += 1;
                Some(children[index])
            }
            (SppfImplNodeChildren::More(children), index) if index < self.end => {
                self.index += 1;
                Some(children[index])
            }
            _ => None,
        }
    }

    fn size_hint(&self) -> (usize, Option<usize>) {
        let c = self.end - self.index;
        (c, Some(c))
    }
}

impl<'a> DoubleEndedIterator for SppfImplNodeChildrenIterator<'a> {
    fn next_back(&mut self) -> Option<Self::Item> {
        if self.index >= self.end {
            None
        } else {
            let result = self.children[self.end - 1];
            self.end -= 1;
            Some(result)
        }
    }
}

impl<'a> ExactSizeIterator for SppfImplNodeChildrenIterator<'a> {}
impl<'a> FusedIterator for SppfImplNodeChildrenIterator<'a> {}

/// Represents a version of a node in a Shared-Packed Parse Forest
#[derive(Debug, Default, Clone, PartialEq, Eq)]
pub struct SppfImplNodeVersion {
    /// The label of the node for this version
    pub label: TableElemRef,
    /// The children of the node for this version
    pub children: SppfImplNodeChildren,
}

impl SppfImplNodeVersion {
    /// Initializes this node version without children
    #[must_use]
    pub fn new(label: TableElemRef) -> SppfImplNodeVersion {
        SppfImplNodeVersion {
            label,
            children: SppfImplNodeChildren::None,
        }
    }

    /// Initializes this node version
    #[must_use]
    pub fn from(label: TableElemRef, children: &[SppfImplNodeRef]) -> SppfImplNodeVersion {
        SppfImplNodeVersion {
            label,
            children: SppfImplNodeChildren::from(children),
        }
    }

    /// Creates a new version with added head and tail
    #[must_use]
    pub fn with_head_tail(&self, head: &[SppfImplNodeRef], tail: &[SppfImplNodeRef]) -> Self {
        let total = head.len() + self.len() + tail.len();
        let mut children = Vec::with_capacity(total);
        for &c in head {
            children.push(c);
        }
        for c in &self.children {
            children.push(c);
        }
        for &c in tail {
            children.push(c);
        }
        Self::from(self.label, &children)
    }

    /// Gets the number of children
    #[must_use]
    pub fn len(&self) -> usize {
        self.children.len()
    }

    /// Gets whether there are no children
    #[must_use]
    pub fn is_empty(&self) -> bool {
        self.children.is_empty()
    }

    /// Determines whether this version is the same as the given info
    #[must_use]
    pub fn is_same_as(&self, label: TableElemRef, children: &[SppfImplNodeRef]) -> bool {
        self.label == label
            && self.len() == children.len()
            && self
                .children
                .into_iter()
                .zip(children)
                .all(|(left, &right)| left == right)
    }

    /// Insert a series of children at the front
    pub fn insert_head(&mut self, head: &[SppfImplNodeRef]) {
        let mut result = Vec::with_capacity(head.len() + self.len());
        for &child in head {
            result.push(child);
        }
        for child in &self.children {
            result.push(child);
        }
        self.children = SppfImplNodeChildren::from(result.as_slice());
    }

    /// Adds a series of children at the back
    pub fn add_tail(&mut self, tail: &[SppfImplNodeRef]) {
        let mut result = Vec::with_capacity(self.len() + tail.len());
        for child in &self.children {
            result.push(child);
        }
        for &child in tail {
            result.push(child);
        }
        self.children = SppfImplNodeChildren::from(result.as_slice());
    }

    /// Format this node
    ///
    /// # Errors
    ///
    /// Propagates the error from `write!`
    pub fn fmt(
        &self,
        f: &mut Formatter,
        variables: &[Symbol],
        virtuals: &[Symbol],
    ) -> Result<(), Error> {
        self.label.fmt(f, variables, virtuals)?;
        if !self.is_empty() {
            write!(f, " ->")?;
            for child in &self.children {
                write!(f, " {child}")?;
            }
        }
        Ok(())
    }
}

/// The different versions of a node in a Shared-Packed Parse Forest
#[derive(Debug, Clone)]
pub enum SppfImplNodeVersions<T> {
    /// The node has a single version
    Single(T),
    /// The node has multiple versions
    Multiple(Vec<T>),
}

impl<T: Default> Default for SppfImplNodeVersions<T> {
    fn default() -> Self {
        Self::Single(T::default())
    }
}

impl<T: PartialEq> SppfImplNodeVersions<T> {
    /// Adds new versions
    #[must_use]
    pub fn with_new_versions(self, others: Self) -> Self {
        match (self, others) {
            (SppfImplNodeVersions::Single(left), SppfImplNodeVersions::Single(right)) => {
                if left == right {
                    SppfImplNodeVersions::Single(left)
                } else {
                    SppfImplNodeVersions::Multiple(alloc::vec![left, right])
                }
            }
            (SppfImplNodeVersions::Single(left), SppfImplNodeVersions::Multiple(mut right)) => {
                if right.contains(&left) {
                    SppfImplNodeVersions::Multiple(right)
                } else {
                    right.push(left);
                    SppfImplNodeVersions::Multiple(right)
                }
            }
            (SppfImplNodeVersions::Multiple(mut left), SppfImplNodeVersions::Single(right)) => {
                if left.contains(&right) {
                    SppfImplNodeVersions::Multiple(left)
                } else {
                    left.push(right);
                    SppfImplNodeVersions::Multiple(left)
                }
            }
            (
                SppfImplNodeVersions::Multiple(mut left),
                SppfImplNodeVersions::Multiple(mut right),
            ) => {
                left.append(&mut right);
                left.dedup();
                SppfImplNodeVersions::Multiple(left)
            }
        }
    }

    /// Gets the number of versions
    #[must_use]
    pub fn len(&self) -> usize {
        match self {
            SppfImplNodeVersions::Single(_) => 1,
            SppfImplNodeVersions::Multiple(versions) => versions.len(),
        }
    }

    /// Gets whether there is no version
    /// Always return `false`
    #[must_use]
    pub fn is_empty(&self) -> bool {
        false
    }

    /// Gets the first version
    #[must_use]
    pub fn first(&self) -> &T {
        match self {
            SppfImplNodeVersions::Single(version) => version,
            SppfImplNodeVersions::Multiple(versions) => &versions[0],
        }
    }

    /// Gets the last version
    ///
    /// # Panics
    ///
    /// Cannot panic.
    /// In the case of the `SppfImplNodeVersions::Multiple` variant,
    /// it is guaranteed that the vector is not empty.
    #[must_use]
    pub fn last(&self) -> &T {
        match self {
            SppfImplNodeVersions::Single(version) => version,
            SppfImplNodeVersions::Multiple(versions) => versions.last().unwrap(),
        }
    }
}

impl SppfImplNodeVersions<SppfImplNodeVersion> {
    /// Adds a new version to this node
    #[must_use]
    pub fn with_new_version(
        self,
        label: TableElemRef,
        children: &[SppfImplNodeRef],
    ) -> (Self, usize) {
        match self {
            SppfImplNodeVersions::Single(first) => {
                if first.is_same_as(label, children) {
                    (SppfImplNodeVersions::Single(first), 0)
                } else {
                    (
                        SppfImplNodeVersions::Multiple(alloc::vec![
                            first,
                            SppfImplNodeVersion::from(label, children)
                        ]),
                        0,
                    )
                }
            }
            SppfImplNodeVersions::Multiple(mut versions) => {
                if let Some((version, _)) = versions
                    .iter()
                    .enumerate()
                    .find(|(_, version)| version.is_same_as(label, children))
                {
                    (SppfImplNodeVersions::Multiple(versions), version)
                } else {
                    let current = versions.len();
                    versions.push(SppfImplNodeVersion::from(label, children));
                    (SppfImplNodeVersions::Multiple(versions), current)
                }
            }
        }
    }
}

impl<T> Index<usize> for SppfImplNodeVersions<T> {
    type Output = T;

    fn index(&self, index: usize) -> &Self::Output {
        match (self, index) {
            (SppfImplNodeVersions::Single(first), 0) => first,
            (SppfImplNodeVersions::Multiple(versions), index) => &versions[index],
            _ => panic!("version {index} does not exist"),
        }
    }
}

impl<'a, T: PartialEq> IntoIterator for &'a SppfImplNodeVersions<T> {
    type IntoIter = SppfImplNodeVersionsIterator<'a, T>;
    type Item = &'a T;

    fn into_iter(self) -> Self::IntoIter {
        SppfImplNodeVersionsIterator {
            versions: self,
            index: 0,
            end: self.len(),
        }
    }
}

/// An iterator over the versions of a node
pub struct SppfImplNodeVersionsIterator<'a, T> {
    /// The version
    versions: &'a SppfImplNodeVersions<T>,
    /// The next index
    index: usize,
    /// The last index (excluded)
    end: usize,
}

impl<'a, T> Iterator for SppfImplNodeVersionsIterator<'a, T> {
    type Item = &'a T;

    fn next(&mut self) -> Option<Self::Item> {
        if self.index < self.end {
            let result = &self.versions[self.index];
            self.index += 1;
            Some(result)
        } else {
            None
        }
    }

    fn size_hint(&self) -> (usize, Option<usize>) {
        let c = self.end - self.index;
        (c, Some(c))
    }
}

impl<'a, T> DoubleEndedIterator for SppfImplNodeVersionsIterator<'a, T> {
    fn next_back(&mut self) -> Option<Self::Item> {
        if self.index >= self.end {
            None
        } else {
            let result = &self.versions[self.end - 1];
            self.end -= 1;
            Some(result)
        }
    }
}

impl<'a, T> ExactSizeIterator for SppfImplNodeVersionsIterator<'a, T> {}
impl<'a, T> FusedIterator for SppfImplNodeVersionsIterator<'a, T> {}

/// Represents a node in a Shared-Packed Parse Forest
/// A node can have multiple versions
#[derive(Debug, Clone)]
pub struct SppfImplNode {
    /// The different versions of this node
    pub versions: SppfImplNodeVersions<SppfImplNodeVersion>,
}

impl SppfImplNode {
    /// Initializes this node
    #[must_use]
    pub fn new(label: TableElemRef) -> SppfImplNode {
        SppfImplNode {
            versions: SppfImplNodeVersions::Single(SppfImplNodeVersion::new(label)),
        }
    }

    /// Initializes this node
    #[must_use]
    pub fn new_with_children(label: TableElemRef, children: &[SppfImplNodeRef]) -> SppfImplNode {
        SppfImplNode {
            versions: SppfImplNodeVersions::Single(SppfImplNodeVersion::from(label, children)),
        }
    }

    /// Adds a new version to this node
    pub fn add_version(&mut self, label: TableElemRef, children: &[SppfImplNodeRef]) -> usize {
        let result;
        (self.versions, result) =
            core::mem::take(&mut self.versions).with_new_version(label, children);
        result
    }

    /// Adds new versions to this node
    pub fn add_versions(&mut self, versions: SppfImplNodeVersions<SppfImplNodeVersion>) {
        self.versions = core::mem::take(&mut self.versions).with_new_versions(versions);
    }

    /// Insert a series of children at the front
    pub fn insert_head(&mut self, children: &[SppfImplNodeRef]) {
        if !children.is_empty() {
            match &mut self.versions {
                SppfImplNodeVersions::Single(version) => version.insert_head(children),
                SppfImplNodeVersions::Multiple(versions) => {
                    for version in versions {
                        version.insert_head(children);
                    }
                }
            }
        }
    }

    /// Adds a series of children at the back
    pub fn add_tail(&mut self, children: &[SppfImplNodeRef]) {
        if !children.is_empty() {
            match &mut self.versions {
                SppfImplNodeVersions::Single(version) => version.add_tail(children),
                SppfImplNodeVersions::Multiple(versions) => {
                    for version in versions {
                        version.add_tail(children);
                    }
                }
            }
        }
    }

    /// Gets the first version
    #[must_use]
    pub fn first_version(&self) -> &SppfImplNodeVersion {
        self.versions.first()
    }

    /// Gets the last version
    #[must_use]
    pub fn last_version(&self) -> &SppfImplNodeVersion {
        self.versions.last()
    }

    /// Format this node
    ///
    /// # Errors
    ///
    /// Propagates the error from `write!`
    pub fn fmt(
        &self,
        f: &mut Formatter,
        variables: &[Symbol],
        virtuals: &[Symbol],
    ) -> Result<(), Error> {
        match &self.versions {
            SppfImplNodeVersions::Single(version) => {
                version.fmt(f, variables, virtuals)?;
            }
            SppfImplNodeVersions::Multiple(versions) => {
                for (index, version) in versions.iter().enumerate() {
                    if index > 0 {
                        write!(f, " | ")?;
                        version.fmt(f, variables, virtuals)?;
                    }
                }
            }
        }
        Ok(())
    }
}

/// Represents a node in a Shared-Packed Parse Forest that can be replaced by its children
#[derive(Clone)]
pub struct SppfImplNodeReplaceable {
    /// The different versions for this node
    pub versions: SppfImplNodeVersions<SppfImplNodeReplaceableVersion>,
}

impl SppfImplNodeReplaceable {
    /// Initializes this node
    #[must_use]
    pub fn new(
        label: TableElemRef,
        children: &[SppfImplNodeRef],
        actions: &[TreeAction],
    ) -> SppfImplNodeReplaceable {
        Self {
            versions: SppfImplNodeVersions::Single(if children.is_empty() {
                SppfImplNodeReplaceableVersion {
                    label,
                    children: Vec::new(),
                    actions: Vec::new(),
                }
            } else {
                SppfImplNodeReplaceableVersion {
                    label,
                    children: children.to_vec(),
                    actions: actions.to_vec(),
                }
            }),
        }
    }

    /// Adds a new version to this node
    pub fn add_version(
        &mut self,
        label: TableElemRef,
        children: &[SppfImplNodeRef],
        actions: &[TreeAction],
    ) -> usize {
        let result;
        (self.versions, result) =
            core::mem::take(&mut self.versions).with_new_version(label, children, actions);
        result
    }

    /// Format this node
    ///
    /// # Errors
    ///
    /// Propagates the error from `write!`
    pub fn fmt(
        &self,
        f: &mut Formatter,
        variables: &[Symbol],
        virtuals: &[Symbol],
    ) -> Result<(), Error> {
        match &self.versions {
            SppfImplNodeVersions::Single(version) => {
                version.fmt(f, variables, virtuals)?;
            }
            SppfImplNodeVersions::Multiple(versions) => {
                for (index, version) in versions.iter().enumerate() {
                    if index > 0 {
                        write!(f, " | ")?;
                        version.fmt(f, variables, virtuals)?;
                    }
                }
            }
        }
        Ok(())
    }
}

impl SppfImplNodeVersions<SppfImplNodeReplaceableVersion> {
    /// Adds a new version to this node
    #[must_use]
    pub fn with_new_version(
        self,
        label: TableElemRef,
        children: &[SppfImplNodeRef],
        actions: &[TreeAction],
    ) -> (Self, usize) {
        match self {
            SppfImplNodeVersions::Single(first) => {
                if first.is_same_as(label, children) {
                    (SppfImplNodeVersions::Single(first), 0)
                } else {
                    (
                        SppfImplNodeVersions::Multiple(alloc::vec![
                            first,
                            SppfImplNodeReplaceableVersion::from(label, children, actions)
                        ]),
                        0,
                    )
                }
            }
            SppfImplNodeVersions::Multiple(mut versions) => {
                if let Some((version, _)) = versions
                    .iter()
                    .enumerate()
                    .find(|(_, version)| version.is_same_as(label, children))
                {
                    (SppfImplNodeVersions::Multiple(versions), version)
                } else {
                    let current = versions.len();
                    versions.push(SppfImplNodeReplaceableVersion::from(
                        label, children, actions,
                    ));
                    (SppfImplNodeVersions::Multiple(versions), current)
                }
            }
        }
    }
}

/// A version for a node in a Shared-Packed Parse Forest that can be replaced by its children
#[derive(Debug, Default, Clone, PartialEq, Eq)]
pub struct SppfImplNodeReplaceableVersion {
    /// The original label of this node
    pub label: TableElemRef,
    /// The children of this node
    pub children: Vec<SppfImplNodeRef>,
    /// The tree actions on the children of this node
    pub actions: Vec<TreeAction>,
}

impl SppfImplNodeReplaceableVersion {
    /// Initializes this node version
    #[must_use]
    pub fn from(
        label: TableElemRef,
        children: &[SppfImplNodeRef],
        actions: &[TreeAction],
    ) -> SppfImplNodeReplaceableVersion {
        SppfImplNodeReplaceableVersion {
            label,
            children: children.to_vec(),
            actions: actions.to_vec(),
        }
    }

    /// Gets the number of children
    #[must_use]
    pub fn len(&self) -> usize {
        self.children.len()
    }

    /// Gets whether there are no children
    #[must_use]
    pub fn is_empty(&self) -> bool {
        self.children.is_empty()
    }

    /// Determines whether this version is the same as the given info
    #[must_use]
    pub fn is_same_as(&self, label: TableElemRef, children: &[SppfImplNodeRef]) -> bool {
        self.label == label
            && self.len() == children.len()
            && self
                .children
                .iter()
                .zip(children)
                .all(|(left, right)| left == right)
    }
    /// Format this node
    ///
    /// # Errors
    ///
    /// Propagates the error from `write!`
    pub fn fmt(
        &self,
        f: &mut Formatter,
        variables: &[Symbol],
        virtuals: &[Symbol],
    ) -> Result<(), Error> {
        self.label.fmt(f, variables, virtuals)?;
        if !self.children.is_empty() {
            write!(f, " ->")?;
            for child in &self.children {
                write!(f, " {child}")?;
            }
        }
        Ok(())
    }
}

/// Represents a Shared-Packed Parse Forest
#[derive(Default)]
pub struct SppfImpl {
    /// The nodes in the SPPF
    pub nodes: Vec<SppfImplNode>,
    /// The root, if any
    pub root: Option<usize>,
}

impl SppfImpl {
    /// Stores the root of this tree
    pub fn store_root(&mut self, root: SppfImplNodeRef) {
        self.root = Some(root.node_id());
    }

    /// Gets whether a root has been defined for this SPPF
    #[must_use]
    pub fn has_root(&self) -> bool {
        self.root.is_some()
    }

    /// Gets the SPPF node for the specified identifier
    #[must_use]
    pub fn get_node(&self, node_ref: SppfImplNodeRef) -> &SppfImplNode {
        &self.nodes[node_ref.node_id()]
    }

    /// Gets the SPPF node for the specified identifier
    #[must_use]
    pub fn get_node_mut(&mut self, node_ref: SppfImplNodeRef) -> &mut SppfImplNode {
        &mut self.nodes[node_ref.node_id()]
    }

    /// Gets the node version for the specified reference
    #[must_use]
    pub fn get_node_version(&self, node_ref: SppfImplNodeVersRef) -> &SppfImplNodeVersion {
        let node = self.get_node(node_ref.into());
        &node.versions[node_ref.version as usize]
    }

    /// Creates a new single node in the SPPF
    pub fn new_normal_node(&mut self, label: TableElemRef) -> SppfImplNodeRef {
        let identifier = self.nodes.len();
        self.nodes.push(SppfImplNode::new(label));
        SppfImplNodeRef::new_usize(identifier)
    }

    /// Creates a new single node in the SPPF
    pub fn new_normal_node_with_children(
        &mut self,
        label: TableElemRef,
        children: &[SppfImplNodeRef],
    ) -> SppfImplNodeRef {
        let identifier = self.nodes.len();
        self.nodes
            .push(SppfImplNode::new_with_children(label, children));
        SppfImplNodeRef::new_usize(identifier)
    }

    /// Creates a new single node in the SPPF as a promotion of another, with a head and a tail
    pub fn new_promoted_node(
        &mut self,
        previous: SppfImplNodeRef,
        head: &[SppfImplNodeRef],
        tail: &[SppfImplNodeRef],
    ) -> SppfImplNodeRef {
        let identifier = self.nodes.len();
        self.nodes
            .push(self.create_promoted_node(previous, head, tail));
        SppfImplNodeRef::new_usize(identifier)
    }

    /// Creates a new single node in the SPPF as a promotion of another, with a head and a tail
    #[must_use]
    pub fn create_promoted_node(
        &self,
        previous: SppfImplNodeRef,
        head: &[SppfImplNodeRef],
        tail: &[SppfImplNodeRef],
    ) -> SppfImplNode {
        let previous = self.get_node(previous);
        match &previous.versions {
            SppfImplNodeVersions::Single(version) => SppfImplNode {
                versions: SppfImplNodeVersions::Single(version.with_head_tail(head, tail)),
            },
            SppfImplNodeVersions::Multiple(versions) => SppfImplNode {
                versions: SppfImplNodeVersions::Multiple(
                    versions
                        .iter()
                        .map(|version| version.with_head_tail(head, tail))
                        .collect(),
                ),
            },
        }
    }
}

/// Structure to display an SPPF node
pub struct SppfImplNodeDisplay<'a, 's> {
    /// The SPPF
    pub sppf: &'a SppfImpl,
    /// The node of interest
    pub node_id: SppfImplNodeRef,
    /// The table of variables
    pub variables: &'a [Symbol<'s>],
    /// The table of virtuals
    pub virtuals: &'a [Symbol<'s>],
}

impl<'a, 's> Display for SppfImplNodeDisplay<'a, 's> {
    fn fmt(&self, f: &mut Formatter) -> Result<(), Error> {
        let root = self.sppf.get_node(self.node_id);
        write!(f, "{}: ", self.node_id)?;
        root.fmt(f, self.variables, self.virtuals)?;
        writeln!(f)?;
        Ok(())
    }
}

/// Represents a front for a mutable Shared-Packed Parse Forest,
/// i.e. a set of possible parse trees,
/// along with required data
pub struct Sppf<'s, 't, 'a> {
    /// The table of tokens
    tokens: TokenRepository<'s, 't, 'a>,
    /// The table of variables
    variables: &'a [Symbol<'s>],
    /// The table of virtuals
    virtuals: &'a [Symbol<'s>],
    /// The SPPF itself
    data: &'a SppfImpl,
}

impl<'s, 't, 'a> Sppf<'s, 't, 'a> {
    /// Creates a new mutating SPPF front, with a mutably borrowed backend
    #[must_use]
    pub fn new(
        tokens: TokenRepository<'s, 't, 'a>,
        variables: &'a [Symbol<'s>],
        virtuals: &'a [Symbol<'s>],
        data: &'a SppfImpl,
    ) -> Sppf<'s, 't, 'a> {
        Self {
            tokens,
            variables,
            virtuals,
            data,
        }
    }

    /// Gets whether a root has been defined for this AST
    #[must_use]
    pub fn has_root(&self) -> bool {
        self.data.has_root()
    }

    /// Gets the root for this SPPF
    ///
    /// # Panics
    ///
    /// Raise a panic when the SPPF has no root.
    /// This can happen when a `ParseResult` contains some errors,
    /// but the SPPF `get_sppf` is called to get an SPPF but it will have not root.
    #[must_use]
    pub fn get_root(&'a self) -> SppfNode<'s, 't, 'a> {
        let root = self.data.root.expect("No root defined!");
        let node = &self.data.nodes[root];
        SppfNode {
            sppf: self,
            node_ref: SppfImplNodeRef::new_usize(root),
            node,
        }
    }

    /// Gets the SPPF node (if any) that has the specified token as label
    #[must_use]
    pub fn find_node_for(&'a self, token: &Token) -> Option<SppfNode<'s, 't, 'a>> {
        let root = self.get_root();
        self.traverse(
            root.node,
            SppfImplNodeRef::new_usize(root.id()),
            |child, child_ref| {
                for (index, version) in child.versions.into_iter().enumerate() {
                    if version.label.table_type() == TableType::Token
                        && version.label.index() == token.index
                    {
                        return Some(child_ref.with_version_usize(index));
                    }
                }
                None
            },
        )
        .map(|node_ref| SppfNode::new(self, node_ref.into()))
    }

    /// Gets the AST node (if any) that has
    /// a token label that contains the specified index in the input text
    #[must_use]
    pub fn find_node_at_index(&'a self, index: usize) -> Option<SppfNode<'s, 't, 'a>> {
        self.tokens
            .find_token_at(index)
            .and_then(|token| self.find_node_for(&token))
    }

    /// Gets the AST node (if any) that has
    /// a token label that contains the specified index in the input text
    #[must_use]
    pub fn find_node_at_position(&'a self, position: TextPosition) -> Option<SppfNode<'s, 't, 'a>> {
        let index = self.tokens.text.get_line_index(position.line) + position.column - 1;
        self.tokens
            .find_token_at(index)
            .and_then(|token| self.find_node_for(&token))
    }

    /// Gets the parent of the specified node, if any
    #[must_use]
    pub fn find_parent_of(
        &'a self,
        node_ref: SppfImplNodeRef,
    ) -> Option<SppfNodeVersion<'s, 't, 'a>> {
        let root = self.get_root();
        self.traverse(
            root.node,
            SppfImplNodeRef::new_usize(root.id()),
            |child, child_ref| {
                for (index, version) in child.versions.into_iter().enumerate() {
                    if version.children.into_iter().any(|child| child == node_ref) {
                        return Some(child_ref.with_version_usize(index));
                    }
                }
                None
            },
        )
        .map(|node_ref| SppfNodeVersion::new(self, node_ref))
    }

    /// Gets the total span of sub-tree given its root and its position
    #[must_use]
    pub fn get_total_position_and_span(
        &self,
        node: &SppfImplNode,
    ) -> Option<(TextPosition, TextSpan)> {
        let mut total_span: Option<TextSpan> = None;
        let mut position = TextPosition {
            line: usize::MAX,
            column: usize::MAX,
        };
        self.get_total_position_and_span_accumulate(&mut total_span, &mut position, node);
        total_span.map(|span| (position, span))
    }

    /// Gets the total span of sub-tree given its root and its position
    #[must_use]
    pub fn get_total_position_and_span_version(
        &self,
        node: &SppfImplNodeVersion,
    ) -> Option<(TextPosition, TextSpan)> {
        let mut total_span: Option<TextSpan> = None;
        let mut position = TextPosition {
            line: usize::MAX,
            column: usize::MAX,
        };
        for child in &node.children {
            let child = self.data.get_node(child);
            self.get_total_position_and_span_accumulate(&mut total_span, &mut position, child);
        }
        total_span.map(|span| (position, span))
    }

    /// Gets the total span of sub-tree given its root and its position
    pub fn get_total_position_and_span_accumulate(
        &self,
        total_span: &mut Option<TextSpan>,
        position: &mut TextPosition,
        node: &SppfImplNode,
    ) {
        self.traverse(node, SppfImplNodeRef::default(), |current, _| {
            for version in &current.versions {
                if let Some(p) = self.get_position_at(version) {
                    if p < *position {
                        *position = p;
                    }
                }
                if let Some(total_span) = total_span.as_mut() {
                    if let Some(span) = self.get_span_at(version) {
                        if span.index + span.length > total_span.index + total_span.length {
                            let margin =
                                (span.index + span.length) - (total_span.index + total_span.length);
                            total_span.length += margin;
                        }
                        if span.index < total_span.index {
                            let margin = total_span.index - span.index;
                            total_span.length += margin;
                            total_span.index -= margin;
                        }
                    }
                } else {
                    *total_span = self.get_span_at(version);
                }
            }
            Option::<()>::None
        });
    }

    /// Gets the total span of sub-tree given its root
    #[must_use]
    pub fn get_total_span(&self, node: &SppfImplNode) -> Option<TextSpan> {
        self.get_total_position_and_span(node).map(|(_, span)| span)
    }

    /// Gets the total span of sub-tree given its root
    #[must_use]
    pub fn get_total_span_version(&self, version: &SppfImplNodeVersion) -> Option<TextSpan> {
        self.get_total_position_and_span_version(version)
            .map(|(_, span)| span)
    }

    /// Traverses the AST from the specified node
    fn traverse<R, F>(
        &self,
        from: &'a SppfImplNode,
        from_ref: SppfImplNodeRef,
        mut action: F,
    ) -> Option<R>
    where
        F: FnMut(&'a SppfImplNode, SppfImplNodeRef) -> Option<R>,
    {
        let mut stack = alloc::vec![(from, from_ref)];
        while let Some((current, current_ref)) = stack.pop() {
            for version in &current.versions {
                for child_ref in version.children.into_iter().rev() {
                    stack.push((self.data.get_node(child_ref), child_ref));
                }
            }

            if let Some(r) = action(current, current_ref) {
                return Some(r);
            }
        }
        None
    }

    /// Get the span of the symbol on a node's version
    #[must_use]
    fn get_span_at(&self, version: &SppfImplNodeVersion) -> Option<TextSpan> {
        let label = version.label;
        match label.table_type() {
            TableType::Token => {
                let token = self.tokens.get_token(label.index());
                token.get_span()
            }
            _ => None,
        }
    }

    /// Get the position of the symbol on a node's version
    #[must_use]
    fn get_position_at(&self, version: &SppfImplNodeVersion) -> Option<TextPosition> {
        let label = version.label;
        match label.table_type() {
            TableType::Token => {
                let token = self.tokens.get_token(label.index());
                token.get_position()
            }
            _ => None,
        }
    }
}

/// Represents a node in a Shared-Packed Parse Forest
#[derive(Copy, Clone)]
pub struct SppfNode<'s, 't, 'a> {
    /// The parent SPPF
    sppf: &'a Sppf<'s, 't, 'a>,
    /// The node's identifier
    node_ref: SppfImplNodeRef,
    /// The underlying node in the SPPF implementation
    node: &'a SppfImplNode,
}

impl<'s, 't, 'a> SppfNode<'s, 't, 'a> {
    /// Creates a new node
    #[must_use]
    fn new(sppf: &'a Sppf<'s, 't, 'a>, node_ref: SppfImplNodeRef) -> SppfNode<'s, 't, 'a> {
        let node = sppf.data.get_node(node_ref);
        SppfNode {
            sppf,
            node_ref,
            node,
        }
    }

    /// Gets the identifier of this node
    #[must_use]
    pub fn id(&self) -> usize {
        self.node_ref.node_id()
    }

    /// Gets the parent of this node, if any
    #[must_use]
    pub fn parent(&self) -> Option<SppfNodeVersion<'s, 't, 'a>> {
        self.sppf.find_parent_of(self.node_ref)
    }

    /// Gets the first version of this node
    #[must_use]
    pub fn first_version(&self) -> SppfNodeVersion<'s, 't, 'a> {
        self.version(0)
    }

    /// Gets i-th version of this node
    #[must_use]
    pub fn version(&self, index: usize) -> SppfNodeVersion<'s, 't, 'a> {
        let version = &self.node.versions[index];
        SppfNodeVersion {
            sppf: self.sppf,
            version,
            node_ref: self.node_ref.with_version_usize(index),
        }
    }

    /// Gets the different versions of this node
    #[must_use]
    pub fn versions(&self) -> SppfNodeVersions<'s, 't, 'a> {
        SppfNodeVersions {
            sppf: self.sppf,
            node_ref: self.node_ref,
            node: self.node,
        }
    }

    /// Gets the number of versions for this node
    #[must_use]
    pub fn versions_count(&self) -> usize {
        self.node.versions.len()
    }

    /// Gets the total span for the sub-tree at this node
    #[must_use]
    pub fn get_total_span(&self) -> Option<TextSpan> {
        self.sppf.get_total_span(self.node)
    }

    /// Gets the total position and span for the sub-tree at this node
    #[must_use]
    pub fn get_total_position_and_span(&self) -> Option<(TextPosition, TextSpan)> {
        self.sppf.get_total_position_and_span(self.node)
    }
}

impl<'s, 't, 'a> Serialize for SppfNode<'s, 't, 'a> {
    fn serialize<S>(&self, serializer: S) -> Result<S::Ok, S::Error>
    where
        S: Serializer,
    {
        let versions = self.versions();
        let mut state = serializer.serialize_struct("SppfNode", 1)?;
        state.serialize_field("versions", &versions)?;
        state.end()
    }
}

#[derive(Copy, Clone)]
pub struct SppfNodeVersions<'s, 't, 'a> {
    /// The parent SPPF
    sppf: &'a Sppf<'s, 't, 'a>,
    /// The node's identifier
    node_ref: SppfImplNodeRef,
    /// The underlying node in the SPPF implementation
    node: &'a SppfImplNode,
}

impl<'s, 't, 'a> SppfNodeVersions<'s, 't, 'a> {
    /// Gets the number of versions
    #[must_use]
    pub fn len(&self) -> usize {
        self.node.versions.len()
    }

    /// Gets whether there is no version
    /// Always return `false`
    #[must_use]
    pub fn is_empty(&self) -> bool {
        false
    }
}

impl<'s, 't, 'a> Serialize for SppfNodeVersions<'s, 't, 'a> {
    fn serialize<S>(&self, serializer: S) -> Result<S::Ok, S::Error>
    where
        S: Serializer,
    {
        let mut seq = serializer.serialize_seq(Some(self.len()))?;
        for version in self.into_iter() {
            seq.serialize_element(&version)?;
        }
        seq.end()
    }
}

impl<'s, 't, 'a> IntoIterator for SppfNodeVersions<'s, 't, 'a> {
    type Item = SppfNodeVersion<'s, 't, 'a>;
    type IntoIter = SppfNodeVersionsIterator<'s, 't, 'a>;

    fn into_iter(self) -> Self::IntoIter {
        SppfNodeVersionsIterator {
            sppf: self.sppf,
            node_ref: self.node_ref,
            node: self.node,
            current: 0,
            end: self.node.versions.len(),
        }
    }
}

pub struct SppfNodeVersionsIterator<'s, 't, 'a> {
    /// The parent SPPF
    sppf: &'a Sppf<'s, 't, 'a>,
    /// The node's identifier
    node_ref: SppfImplNodeRef,
    /// The underlying node in the SPPF implementation
    node: &'a SppfImplNode,
    /// The current (next) index
    current: usize,
    /// The end index (excluded)
    end: usize,
}

impl<'s, 't, 'a> Iterator for SppfNodeVersionsIterator<'s, 't, 'a> {
    type Item = SppfNodeVersion<'s, 't, 'a>;
    fn next(&mut self) -> Option<Self::Item> {
        if self.current < self.end {
            let index = self.current;
            self.current += 1;
            Some(SppfNodeVersion {
                sppf: self.sppf,
                version: &self.node.versions[index],
                node_ref: self.node_ref.with_version_usize(index),
            })
        } else {
            None
        }
    }

    fn size_hint(&self) -> (usize, Option<usize>) {
        let c = self.end - self.current;
        (c, Some(c))
    }
}

impl<'s, 't, 'a> DoubleEndedIterator for SppfNodeVersionsIterator<'s, 't, 'a> {
    fn next_back(&mut self) -> Option<Self::Item> {
        if self.current >= self.end {
            None
        } else {
            let result = SppfNodeVersion {
                sppf: self.sppf,
                version: &self.node.versions[self.end - 1],
                node_ref: self.node_ref.with_version_usize(self.end - 1),
            };
            self.end -= 1;
            Some(result)
        }
    }
}

impl<'s, 't, 'a> ExactSizeIterator for SppfNodeVersionsIterator<'s, 't, 'a> {}
impl<'s, 't, 'a> FusedIterator for SppfNodeVersionsIterator<'s, 't, 'a> {}

#[derive(Copy, Clone)]
pub struct SppfNodeVersion<'s, 't, 'a> {
    /// The parent SPPF
    sppf: &'a Sppf<'s, 't, 'a>,
    /// The underlying version in the SPPF implementation
    version: &'a SppfImplNodeVersion,
    /// The reference to this specific version
    node_ref: SppfImplNodeVersRef,
}

impl<'s, 't, 'a> SppfNodeVersion<'s, 't, 'a> {
    /// Creates a new version
    #[must_use]
    fn new(
        sppf: &'a Sppf<'s, 't, 'a>,
        node_ref: SppfImplNodeVersRef,
    ) -> SppfNodeVersion<'s, 't, 'a> {
        let version = sppf.data.get_node_version(node_ref);
        SppfNodeVersion {
            sppf,
            version,
            node_ref,
        }
    }

    /// Gets a reference to this node version
    #[must_use]
    pub fn get_ref(&self) -> SppfImplNodeVersRef {
        self.node_ref
    }

    /// Gets the index of the token born by this version, if any
    #[must_use]
    pub fn get_token_index(&self) -> Option<usize> {
        let label = self.version.label;
        match label.table_type() {
            TableType::Token => Some(label.index()),
            _ => None,
        }
    }

    /// Gets the parent node of which this is a version
    #[must_use]
    pub fn node(&self) -> SppfNode<'s, 't, 'a> {
        SppfNode::new(self.sppf, self.node_ref.into())
    }

    /// Gets the children for this version of the node
    #[must_use]
    pub fn children(&self) -> SppfNodeChildren<'s, 't, 'a> {
        SppfNodeChildren {
            sppf: self.sppf,
            version: self.version,
        }
    }

    /// Gets the i-th child
    #[must_use]
    pub fn child(&self, index: usize) -> SppfNode<'s, 't, 'a> {
        let child_id = self.version.children[index].node_id;
        SppfNode::new(self.sppf, SppfImplNodeRef { node_id: child_id })
    }

    /// Gets the number of children
    #[must_use]
    pub fn children_count(&self) -> usize {
        self.version.children.len()
    }

    /// Gets the total span for the sub-tree at this node
    #[must_use]
    pub fn get_total_span(&self) -> Option<TextSpan> {
        self.sppf.get_total_span_version(self.version)
    }

    /// Gets the total position and span for the sub-tree at this node
    #[must_use]
    pub fn get_total_position_and_span(&self) -> Option<(TextPosition, TextSpan)> {
        self.sppf.get_total_position_and_span_version(self.version)
    }
}

impl<'s, 't, 'a> SemanticElementTrait<'s, 'a> for SppfNodeVersion<'s, 't, 'a> {
    /// Gets the position in the input text of this element
    fn get_position(&self) -> Option<TextPosition> {
        self.sppf.get_position_at(self.version)
    }

    /// Gets the span in the input text of this element
    fn get_span(&self) -> Option<TextSpan> {
        self.sppf.get_span_at(self.version)
    }

    /// Gets the context of this element in the input
    fn get_context(&self) -> Option<TextContext<'a>> {
        let label = self.version.label;
        match label.table_type() {
            TableType::Token => {
                let token = self.sppf.tokens.get_token(label.index());
                token.get_context()
            }
            _ => None,
        }
    }

    /// Gets the grammar symbol associated to this element
    fn get_symbol(&self) -> Symbol<'s> {
        let label = self.version.label;
        match label.table_type() {
            TableType::Token => {
                let token = self.sppf.tokens.get_token(label.index());
                token.get_symbol()
            }
            TableType::Variable => self.sppf.variables[label.index()],
            TableType::Virtual => self.sppf.virtuals[label.index()],
            TableType::None => {
                // terminal epsilon
                self.sppf.tokens.terminals[0]
            }
        }
    }

    /// Gets the value of this element, if any
    fn get_value(&self) -> Option<&'a str> {
        let label = self.version.label;
        match label.table_type() {
            TableType::Token => {
                let token = self.sppf.tokens.get_token(label.index());
                token.get_value()
            }
            _ => None,
        }
    }
}

impl<'s, 't, 'a> Display for SppfNodeVersion<'s, 't, 'a> {
    fn fmt(&self, f: &mut Formatter) -> Result<(), Error> {
        let label = self.version.label;
        match label.table_type() {
            TableType::Token => {
                let token = self.sppf.tokens.get_token(label.index());
                let symbol = token.get_symbol();
                let value = token.get_value();
                write!(f, "{} = {}", symbol.name, value.unwrap())
            }
            TableType::Variable => {
                let symbol = self.sppf.variables[label.index()];
                write!(f, "{}", symbol.name)
            }
            TableType::Virtual => {
                let symbol = self.sppf.virtuals[label.index()];
                write!(f, "{}", symbol.name)
            }
            TableType::None => {
                let symbol = self.sppf.tokens.terminals[0];
                write!(f, "{}", symbol.name)
            }
        }
    }
}

impl<'s, 't, 'a> Serialize for SppfNodeVersion<'s, 't, 'a> {
    fn serialize<S>(&self, serializer: S) -> Result<S::Ok, S::Error>
    where
        S: Serializer,
    {
        let mut state = serializer.serialize_struct("SppfNodeVersion", 5)?;
        state.serialize_field("symbol", &self.get_symbol())?;
        state.serialize_field("position", &self.get_position())?;
        state.serialize_field("span", &self.get_span())?;
        state.serialize_field("value", &self.get_value())?;
        state.serialize_field("children", &self.children())?;
        state.end()
    }
}

#[derive(Copy, Clone)]
pub struct SppfNodeChildren<'s, 't, 'a> {
    /// The parent SPPF
    sppf: &'a Sppf<'s, 't, 'a>,
    /// The underlying version in the SPPF implementation
    version: &'a SppfImplNodeVersion,
}

impl<'s, 't, 'a> SppfNodeChildren<'s, 't, 'a> {
    /// Gets the number of children
    #[must_use]
    pub fn len(&self) -> usize {
        self.version.len()
    }

    /// Gets whether there are no children
    #[must_use]
    pub fn is_empty(&self) -> bool {
        self.version.is_empty()
    }

    /// Gets the i-th child
    #[must_use]
    pub fn at(&self, index: usize) -> SppfNode<'s, 't, 'a> {
        let child_id = self.version.children[index].node_id;
        SppfNode::new(self.sppf, SppfImplNodeRef { node_id: child_id })
    }
}

impl<'s, 't, 'a> IntoIterator for SppfNodeChildren<'s, 't, 'a> {
    type Item = SppfNode<'s, 't, 'a>;
    type IntoIter = SppfNodeChildrenIterator<'s, 't, 'a>;

    fn into_iter(self) -> Self::IntoIter {
        SppfNodeChildrenIterator {
            sppf: self.sppf,
            version: self.version,
            index: 0,
            end: self.version.len(),
        }
    }
}

impl<'s, 't, 'a> Serialize for SppfNodeChildren<'s, 't, 'a> {
    fn serialize<S>(&self, serializer: S) -> Result<S::Ok, S::Error>
    where
        S: Serializer,
    {
        let mut seq = serializer.serialize_seq(Some(self.len()))?;
        for child in self.into_iter() {
            seq.serialize_element(&child)?;
        }
        seq.end()
    }
}

/// Iterator over SPPF children nodes
pub struct SppfNodeChildrenIterator<'s, 't, 'a> {
    /// The parent SPPF
    sppf: &'a Sppf<'s, 't, 'a>,
    /// The underlying version in the SPPF implementation
    version: &'a SppfImplNodeVersion,
    /// The next index
    index: usize,
    /// The last index (excluded)
    end: usize,
}

impl<'s, 't, 'a> Iterator for SppfNodeChildrenIterator<'s, 't, 'a> {
    type Item = SppfNode<'s, 't, 'a>;

    fn next(&mut self) -> Option<Self::Item> {
        if self.index < self.end {
            let node_ref = self.version.children[self.index];
            self.index += 1;
            Some(SppfNode::new(self.sppf, node_ref))
        } else {
            None
        }
    }

    fn size_hint(&self) -> (usize, Option<usize>) {
        let c = self.end - self.index;
        (c, Some(c))
    }
}

impl<'s, 't, 'a> DoubleEndedIterator for SppfNodeChildrenIterator<'s, 't, 'a> {
    fn next_back(&mut self) -> Option<Self::Item> {
        if self.index >= self.end {
            None
        } else {
            let node_ref = self.version.children[self.end - 1];
            self.end -= 1;
            Some(SppfNode::new(self.sppf, node_ref))
        }
    }
}

impl<'s, 't, 'a> ExactSizeIterator for SppfNodeChildrenIterator<'s, 't, 'a> {}
impl<'s, 't, 'a> FusedIterator for SppfNodeChildrenIterator<'s, 't, 'a> {}
