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
pub struct SppfImplNodeRef {
    /// The identifier of the node
    pub node_id: u32,
    /// The version to refer to
    pub version: u32
}

impl SppfImplNodeRef {
    /// Creates a new reference to a node version
    #[must_use]
    pub fn new(node_id: u32, version: u32) -> Self {
        Self { node_id, version }
    }

    /// Creates a new reference to a node version
    #[must_use]
    pub fn new_usize(node_id: usize, version: usize) -> Self {
        Self {
            node_id: node_id as u32,
            version: version as u32
        }
    }
}

/// The children for a SPPF node
#[derive(Debug, Default, Clone)]
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
    More(Vec<SppfImplNodeRef>)
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
            SppfImplNodeChildren::More(data) => data.len()
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
            SppfImplNodeChildren::None => todo!()
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
            end: self.len()
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
    end: usize
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
            _ => None
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
#[derive(Debug, Default, Clone)]
pub struct SppfImplNodeVersion {
    /// The label of the node for this version
    pub label: TableElemRef,
    /// The children of the node for this version
    pub children: SppfImplNodeChildren
}

impl SppfImplNodeVersion {
    /// Initializes this node version without children
    #[must_use]
    pub fn new(label: TableElemRef) -> SppfImplNodeVersion {
        SppfImplNodeVersion {
            label,
            children: SppfImplNodeChildren::None
        }
    }

    /// Initializes this node version
    #[must_use]
    pub fn from(
        label: TableElemRef,
        buffer: &[SppfImplNodeRef],
        count: usize
    ) -> SppfImplNodeVersion {
        if count == 0 {
            SppfImplNodeVersion {
                label,
                children: SppfImplNodeChildren::None
            }
        } else if count == 1 {
            SppfImplNodeVersion {
                label,
                children: SppfImplNodeChildren::Single(buffer[0])
            }
        } else if count == 2 {
            SppfImplNodeVersion {
                label,
                children: SppfImplNodeChildren::Two([buffer[0], buffer[1]])
            }
        } else if count == 3 {
            SppfImplNodeVersion {
                label,
                children: SppfImplNodeChildren::Three([buffer[0], buffer[1], buffer[2]])
            }
        } else if count == 4 {
            SppfImplNodeVersion {
                label,
                children: SppfImplNodeChildren::Four([buffer[0], buffer[1], buffer[2], buffer[3]])
            }
        } else {
            let mut children = Vec::with_capacity(count);
            for x in buffer.iter().take(count) {
                children.push(*x);
            }
            SppfImplNodeVersion {
                label,
                children: SppfImplNodeChildren::More(children)
            }
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
}

/// The different versions of a node in a Shared-Packed Parse Forest
#[derive(Debug, Clone)]
pub enum SppfImplNodeVersions {
    /// The node has a single version
    Single(SppfImplNodeVersion),
    /// The node has multiple versions
    Multiple(Vec<SppfImplNodeVersion>)
}

impl Default for SppfImplNodeVersions {
    fn default() -> Self {
        Self::Single(SppfImplNodeVersion::default())
    }
}

impl SppfImplNodeVersions {
    /// Adds a new version to this node
    #[must_use]
    pub fn with_new_version(
        self,
        label: TableElemRef,
        buffer: &[SppfImplNodeRef],
        count: usize
    ) -> (Self, usize) {
        match self {
            SppfImplNodeVersions::Single(first) => (
                SppfImplNodeVersions::Multiple(alloc::vec![
                    first,
                    SppfImplNodeVersion::from(label, buffer, count)
                ]),
                0
            ),
            SppfImplNodeVersions::Multiple(mut versions) => {
                let current = versions.len();
                versions.push(SppfImplNodeVersion::from(label, buffer, count));
                (SppfImplNodeVersions::Multiple(versions), current)
            }
        }
    }

    /// Gets the number of versions
    #[must_use]
    pub fn len(&self) -> usize {
        match self {
            SppfImplNodeVersions::Single(_) => 1,
            SppfImplNodeVersions::Multiple(versions) => versions.len()
        }
    }

    /// Gets whether there is no version
    /// Always return `false`
    #[must_use]
    pub fn is_empty(&self) -> bool {
        false
    }
}

impl Index<usize> for SppfImplNodeVersions {
    type Output = SppfImplNodeVersion;

    fn index(&self, index: usize) -> &Self::Output {
        match (self, index) {
            (SppfImplNodeVersions::Single(first), 0) => first,
            (SppfImplNodeVersions::Multiple(versions), index) => &versions[index],
            _ => panic!("version {index} does not exist")
        }
    }
}

impl<'a> IntoIterator for &'a SppfImplNodeVersions {
    type IntoIter = SppfImplNodeVersionsIterator<'a>;
    type Item = &'a SppfImplNodeVersion;

    fn into_iter(self) -> Self::IntoIter {
        SppfImplNodeVersionsIterator {
            versions: self,
            index: 0,
            end: self.len()
        }
    }
}

/// An iterator over the versions of a node
pub struct SppfImplNodeVersionsIterator<'a> {
    /// The version
    versions: &'a SppfImplNodeVersions,
    /// The next index
    index: usize,
    /// The last index (excluded)
    end: usize
}

impl<'a> Iterator for SppfImplNodeVersionsIterator<'a> {
    type Item = &'a SppfImplNodeVersion;

    fn next(&mut self) -> Option<Self::Item> {
        if self.index < self.end {
            Some(&self.versions[self.index])
        } else {
            None
        }
    }

    fn size_hint(&self) -> (usize, Option<usize>) {
        let c = self.end - self.index;
        (c, Some(c))
    }
}

impl<'a> DoubleEndedIterator for SppfImplNodeVersionsIterator<'a> {
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

impl<'a> ExactSizeIterator for SppfImplNodeVersionsIterator<'a> {}
impl<'a> FusedIterator for SppfImplNodeVersionsIterator<'a> {}

/// Represents the interface for a node in a Shared-Packed Parse Forest
pub trait SppfImplNodeTrait {
    /// Gets the original symbol for this node
    fn get_original_symbol(&self) -> TableElemRef;
}

/// Represents a node in a Shared-Packed Parse Forest
/// A node can have multiple versions
#[derive(Debug, Clone)]
pub struct SppfImplNodeNormal {
    /// The original label of this node
    pub original: TableElemRef,
    /// The different versions of this node
    pub versions: SppfImplNodeVersions
}

impl SppfImplNodeTrait for SppfImplNodeNormal {
    fn get_original_symbol(&self) -> TableElemRef {
        self.original
    }
}

impl SppfImplNodeNormal {
    /// Initializes this node
    #[must_use]
    pub fn new(label: TableElemRef) -> SppfImplNodeNormal {
        SppfImplNodeNormal {
            original: label,
            versions: SppfImplNodeVersions::Single(SppfImplNodeVersion::new(label))
        }
    }

    /// Initializes this node
    #[must_use]
    pub fn new_with_children(
        original: TableElemRef,
        label: TableElemRef,
        buffer: &[SppfImplNodeRef],
        count: usize
    ) -> SppfImplNodeNormal {
        SppfImplNodeNormal {
            original,
            versions: SppfImplNodeVersions::Single(SppfImplNodeVersion::from(label, buffer, count))
        }
    }

    /// Adds a new version to this node
    pub fn new_version(
        &mut self,
        label: TableElemRef,
        buffer: &[SppfImplNodeRef],
        count: usize
    ) -> usize {
        let result;
        (self.versions, result) =
            std::mem::take(&mut self.versions).with_new_version(label, buffer, count);
        result
    }
}

/// Represents a node in a Shared-Packed Parse Forest that can be replaced by its children
#[derive(Clone)]
pub struct SppfImplNodeReplaceable {
    /// The original label of this node
    pub original: TableElemRef,
    /// The children of this node
    pub children: Option<Vec<SppfImplNodeRef>>,
    /// The tree actions on the children of this node
    pub actions: Option<Vec<TreeAction>>
}

impl SppfImplNodeTrait for SppfImplNodeReplaceable {
    fn get_original_symbol(&self) -> TableElemRef {
        self.original
    }
}

impl SppfImplNodeReplaceable {
    /// Initializes this node
    #[must_use]
    pub fn new(
        label: TableElemRef,
        children_buffer: &[SppfImplNodeRef],
        actions_buffer: &[TreeAction],
        count: usize
    ) -> SppfImplNodeReplaceable {
        if count == 0 {
            SppfImplNodeReplaceable {
                original: label,
                children: None,
                actions: None
            }
        } else {
            let mut children = Vec::with_capacity(count);
            let mut actions = Vec::with_capacity(count);
            for i in 0..count {
                children.push(children_buffer[i]);
                actions.push(actions_buffer[i]);
            }
            SppfImplNodeReplaceable {
                original: label,
                children: Some(children),
                actions: Some(actions)
            }
        }
    }
}

/// Represents a node in a Shared-Packed Parse Forest
#[derive(Clone)]
pub enum SppfImplNode {
    /// A normal node
    Normal(SppfImplNodeNormal),
    /// A replaceable node
    Replaceable(SppfImplNodeReplaceable)
}

impl SppfImplNodeTrait for SppfImplNode {
    fn get_original_symbol(&self) -> TableElemRef {
        match self {
            SppfImplNode::Normal(node) => node.original,
            SppfImplNode::Replaceable(node) => node.original
        }
    }
}

impl SppfImplNode {
    /// Gets this node as a normal node
    ///
    /// # Panics
    ///
    /// Panics when the node is not a normal node, but a replaceable node
    #[must_use]
    pub fn as_normal(&self) -> &SppfImplNodeNormal {
        match self {
            SppfImplNode::Normal(node) => node,
            SppfImplNode::Replaceable(_node) => panic!("Expected a normal node")
        }
    }

    /// Gets this node as a normal node
    ///
    /// # Panics
    ///
    /// Panics when the node is not a normal node, but a replaceable node
    pub fn as_normal_mut(&mut self) -> &mut SppfImplNodeNormal {
        match self {
            SppfImplNode::Normal(node) => node,
            SppfImplNode::Replaceable(_node) => panic!("Expected a normal node")
        }
    }
}

/// Represents a Shared-Packed Parse Forest
#[derive(Default)]
pub struct SppfImpl {
    /// The nodes in the SPPF
    pub nodes: Vec<SppfImplNode>,
    /// The root, if any
    pub root: Option<usize>
}

impl SppfImpl {
    /// Stores the root of this tree
    pub fn store_root(&mut self, root: usize) {
        self.root = Some(root);
    }

    /// Gets whether a root has been defined for this SPPF
    #[must_use]
    pub fn has_root(&self) -> bool {
        self.root.is_some()
    }

    /// Gets the SPPF node for the specified identifier
    #[must_use]
    pub fn get_node(&self, identifier: usize) -> &SppfImplNode {
        &self.nodes[identifier]
    }

    /// Gets the SPPF node for the specified identifier
    #[must_use]
    pub fn get_node_mut(&mut self, identifier: usize) -> &mut SppfImplNode {
        &mut self.nodes[identifier]
    }

    /// Gets the node version for the specified reference
    #[must_use]
    pub fn get_node_version(&self, node_ref: SppfImplNodeRef) -> &SppfImplNodeVersion {
        let node = self.get_node(node_ref.node_id as usize);
        &node.as_normal().versions[node_ref.version as usize]
    }

    /// Creates a new single node in the SPPF
    pub fn new_normal_node(&mut self, label: TableElemRef) -> usize {
        let identifier = self.nodes.len();
        self.nodes
            .push(SppfImplNode::Normal(SppfImplNodeNormal::new(label)));
        identifier
    }

    /// Creates a new single node in the SPPF
    pub fn new_normal_node_with_children(
        &mut self,
        original: TableElemRef,
        label: TableElemRef,
        buffer: &[SppfImplNodeRef],
        count: usize
    ) -> usize {
        let identifier = self.nodes.len();
        self.nodes
            .push(SppfImplNode::Normal(SppfImplNodeNormal::new_with_children(
                original, label, buffer, count
            )));
        identifier
    }

    /// Creates a new replaceable node in the SPPF
    pub fn new_replaceable_node(
        &mut self,
        label: TableElemRef,
        children_buffer: &[SppfImplNodeRef],
        actions_buffer: &[TreeAction],
        count: usize
    ) -> usize {
        let identifier = self.nodes.len();
        self.nodes
            .push(SppfImplNode::Replaceable(SppfImplNodeReplaceable::new(
                label,
                children_buffer,
                actions_buffer,
                count
            )));
        identifier
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
    data: &'a SppfImpl
}

impl<'s, 't, 'a> Sppf<'s, 't, 'a> {
    /// Creates a new mutating SPPF front, with a mutably borrowed backend
    #[must_use]
    pub fn new(
        tokens: TokenRepository<'s, 't, 'a>,
        variables: &'a [Symbol<'s>],
        virtuals: &'a [Symbol<'s>],
        data: &'a SppfImpl
    ) -> Sppf<'s, 't, 'a> {
        Self {
            tokens,
            variables,
            virtuals,
            data
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
        let node = self.data.nodes[root].as_normal();
        SppfNode {
            sppf: self,
            node_id: root,
            node
        }
    }

    /// Gets the SPPF node (if any) that has the specified token as label
    #[must_use]
    pub fn find_node_for(&'a self, token: &Token) -> Option<SppfNodeVersion<'s, 't, 'a>> {
        let root = self.get_root();
        for version in root.versions() {
            if let Some(result) = self.traverse(
                version.version,
                version.get_ref(),
                |version, version_ref| {
                    if version.label.table_type() == TableType::Token
                        && version.label.index() == token.index
                    {
                        Some(version_ref)
                    } else {
                        None
                    }
                }
            ) {
                return Some(SppfNodeVersion::new(self, result));
            }
        }
        None
    }

    /// Gets the AST node (if any) that has
    /// a token label that contains the specified index in the input text
    #[must_use]
    pub fn find_node_at_index(&'a self, index: usize) -> Option<SppfNodeVersion<'s, 't, 'a>> {
        self.tokens
            .find_token_at(index)
            .and_then(|token| self.find_node_for(&token))
    }

    /// Gets the AST node (if any) that has
    /// a token label that contains the specified index in the input text
    #[must_use]
    pub fn find_node_at_position(
        &'a self,
        position: TextPosition
    ) -> Option<SppfNodeVersion<'s, 't, 'a>> {
        let index = self.tokens.text.get_line_index(position.line) + position.column - 1;
        self.tokens
            .find_token_at(index)
            .and_then(|token| self.find_node_for(&token))
    }

    /// Gets the parent of the specified node, if any
    #[must_use]
    pub fn find_parent_of(
        &'a self,
        node_ref: SppfImplNodeRef
    ) -> Option<SppfNodeVersion<'s, 't, 'a>> {
        let root = self.get_root();
        for version in root.versions() {
            if let Some(result) = self.traverse(
                version.version,
                version.get_ref(),
                |version, version_ref| {
                    if version.children.into_iter().any(|child| child == node_ref) {
                        Some(version_ref)
                    } else {
                        None
                    }
                }
            ) {
                return Some(SppfNodeVersion::new(self, result));
            }
        }
        None
    }

    /// Gets the total span of sub-tree given its root and its position
    #[must_use]
    pub fn get_total_position_and_span(
        &self,
        version: &SppfImplNodeVersion
    ) -> Option<(TextPosition, TextSpan)> {
        let mut total_span: Option<TextSpan> = None;
        let mut position = TextPosition {
            line: usize::MAX,
            column: usize::MAX
        };
        self.traverse(version, SppfImplNodeRef::default(), |current, _| {
            if let Some(p) = self.get_position_at(current) {
                if p < position {
                    position = p;
                }
            }
            if let Some(total_span) = total_span.as_mut() {
                if let Some(span) = self.get_span_at(current) {
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
                total_span = self.get_span_at(current);
            }
            Option::<()>::None
        });
        total_span.map(|span| (position, span))
    }

    /// Gets the total span of sub-tree given its root
    #[must_use]
    pub fn get_total_span(&self, version: &SppfImplNodeVersion) -> Option<TextSpan> {
        self.get_total_position_and_span(version)
            .map(|(_, span)| span)
    }

    /// Traverses the AST from the specified node
    fn traverse<R, F>(
        &self,
        from: &'a SppfImplNodeVersion,
        from_ref: SppfImplNodeRef,
        mut action: F
    ) -> Option<R>
    where
        F: FnMut(&'a SppfImplNodeVersion, SppfImplNodeRef) -> Option<R>
    {
        let mut stack = alloc::vec![(from, from_ref)];
        while let Some((current, current_ref)) = stack.pop() {
            for child_ref in current.children.into_iter().rev() {
                stack.push((self.data.get_node_version(child_ref), child_ref));
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
            _ => None
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
            _ => None
        }
    }
}

/// Represents a node in a Shared-Packed Parse Forest
#[derive(Copy, Clone)]
pub struct SppfNode<'s, 't, 'a> {
    /// The parent SPPF
    sppf: &'a Sppf<'s, 't, 'a>,
    /// The node's identifier
    node_id: usize,
    /// The underlying node in the SPPF implementation
    node: &'a SppfImplNodeNormal
}

impl<'s, 't, 'a> SppfNode<'s, 't, 'a> {
    /// Creates a new node
    #[must_use]
    fn new(sppf: &'a Sppf<'s, 't, 'a>, node_id: usize) -> SppfNode<'s, 't, 'a> {
        let node = sppf.data.get_node(node_id).as_normal();
        SppfNode {
            sppf,
            node_id,
            node
        }
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
            node_ref: SppfImplNodeRef::new_usize(self.node_id, index)
        }
    }

    /// Gets the different versions of this node
    #[must_use]
    pub fn versions(&self) -> SppfNodeVersions<'s, 't, 'a> {
        SppfNodeVersions {
            sppf: self.sppf,
            node_id: self.node_id,
            node: self.node
        }
    }

    /// Gets the number of versions for this node
    #[must_use]
    pub fn versions_count(&self) -> usize {
        self.node.versions.len()
    }
}

impl<'s, 't, 'a> Serialize for SppfNode<'s, 't, 'a> {
    fn serialize<S>(&self, serializer: S) -> Result<S::Ok, S::Error>
    where
        S: Serializer
    {
        let version = self.first_version();
        let mut state = serializer.serialize_struct("SppfNode", 5)?;
        state.serialize_field("symbol", &version.get_symbol())?;
        state.serialize_field("position", &version.get_position())?;
        state.serialize_field("span", &version.get_span())?;
        state.serialize_field("value", &version.get_value())?;
        state.serialize_field("children", &version.children())?;
        state.end()
    }
}

#[derive(Copy, Clone)]
pub struct SppfNodeVersions<'s, 't, 'a> {
    /// The parent SPPF
    sppf: &'a Sppf<'s, 't, 'a>,
    /// The node's identifier
    node_id: usize,
    /// The underlying node in the SPPF implementation
    node: &'a SppfImplNodeNormal
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

impl<'s, 't, 'a> IntoIterator for SppfNodeVersions<'s, 't, 'a> {
    type Item = SppfNodeVersion<'s, 't, 'a>;
    type IntoIter = SppfNodeVersionsIterator<'s, 't, 'a>;

    fn into_iter(self) -> Self::IntoIter {
        SppfNodeVersionsIterator {
            sppf: self.sppf,
            node_id: self.node_id,
            node: self.node,
            current: 0,
            end: self.node.versions.len()
        }
    }
}

pub struct SppfNodeVersionsIterator<'s, 't, 'a> {
    /// The parent SPPF
    sppf: &'a Sppf<'s, 't, 'a>,
    /// The node's identifier
    node_id: usize,
    /// The underlying node in the SPPF implementation
    node: &'a SppfImplNodeNormal,
    /// The current (next) index
    current: usize,
    /// The end index (excluded)
    end: usize
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
                node_ref: SppfImplNodeRef::new_usize(self.node_id, index)
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
                node_ref: SppfImplNodeRef::new_usize(self.node_id, self.end - 1)
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
    node_ref: SppfImplNodeRef
}

impl<'s, 't, 'a> SppfNodeVersion<'s, 't, 'a> {
    /// Creates a new version
    #[must_use]
    fn new(sppf: &'a Sppf<'s, 't, 'a>, node_ref: SppfImplNodeRef) -> SppfNodeVersion<'s, 't, 'a> {
        let node = sppf.data.get_node(node_ref.node_id as usize).as_normal();
        let version = &node.versions[node_ref.version as usize];
        SppfNodeVersion {
            sppf,
            version,
            node_ref
        }
    }

    /// Gets a reference to this node version
    #[must_use]
    pub fn get_ref(&self) -> SppfImplNodeRef {
        self.node_ref
    }

    /// Gets the index of the token born by this version, if any
    #[must_use]
    pub fn get_token_index(&self) -> Option<usize> {
        let label = self.version.label;
        match label.table_type() {
            TableType::Token => Some(label.index()),
            _ => None
        }
    }

    /// Gets the parent node of which this is a version
    #[must_use]
    pub fn node(&self) -> SppfNode<'s, 't, 'a> {
        SppfNode::new(self.sppf, self.node_ref.node_id as usize)
    }

    /// Gets the parent of this node, if any
    #[must_use]
    pub fn parent(&self) -> Option<SppfNodeVersion<'s, 't, 'a>> {
        self.sppf.find_parent_of(self.node_ref)
    }

    /// Gets the children for this version of the node
    #[must_use]
    pub fn children(&self) -> SppfNodeChildren<'s, 't, 'a> {
        SppfNodeChildren {
            sppf: self.sppf,
            version: self.version
        }
    }

    /// Gets the i-th child
    #[must_use]
    pub fn child(&self, index: usize) -> SppfNode<'s, 't, 'a> {
        let child_id = self.version.children[index].node_id as usize;
        SppfNode::new(self.sppf, child_id)
    }

    /// Gets the number of children
    #[must_use]
    pub fn children_count(&self) -> usize {
        self.version.children.len()
    }

    /// Gets the total span for the sub-tree at this node
    #[must_use]
    pub fn get_total_span(&self) -> Option<TextSpan> {
        self.sppf.get_total_span(self.version)
    }

    /// Gets the total position and span for the sub-tree at this node
    #[must_use]
    pub fn get_total_position_and_span(&self) -> Option<(TextPosition, TextSpan)> {
        self.sppf.get_total_position_and_span(self.version)
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
            _ => None
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
            _ => None
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
        S: Serializer
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
    version: &'a SppfImplNodeVersion
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
        let child_id = self.version.children[index].node_id as usize;
        SppfNode::new(self.sppf, child_id)
    }
}

impl<'s, 't, 'a> IntoIterator for SppfNodeChildren<'s, 't, 'a> {
    type Item = SppfNodeVersion<'s, 't, 'a>;
    type IntoIter = SppfNodeChildrenIterator<'s, 't, 'a>;

    fn into_iter(self) -> Self::IntoIter {
        SppfNodeChildrenIterator {
            sppf: self.sppf,
            version: self.version,
            index: 0,
            end: self.version.len()
        }
    }
}

impl<'s, 't, 'a> Serialize for SppfNodeChildren<'s, 't, 'a> {
    fn serialize<S>(&self, serializer: S) -> Result<S::Ok, S::Error>
    where
        S: Serializer
    {
        let mut seq = serializer.serialize_seq(Some(self.len()))?;
        for version in self.into_iter() {
            seq.serialize_element(&version)?;
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
    end: usize
}

impl<'s, 't, 'a> Iterator for SppfNodeChildrenIterator<'s, 't, 'a> {
    type Item = SppfNodeVersion<'s, 't, 'a>;

    fn next(&mut self) -> Option<Self::Item> {
        if self.index < self.end {
            let node_ref = self.version.children[self.index];
            self.index += 1;
            Some(SppfNodeVersion::new(self.sppf, node_ref))
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
            Some(SppfNodeVersion::new(self.sppf, node_ref))
        }
    }
}

impl<'s, 't, 'a> ExactSizeIterator for SppfNodeChildrenIterator<'s, 't, 'a> {}
impl<'s, 't, 'a> FusedIterator for SppfNodeChildrenIterator<'s, 't, 'a> {}
