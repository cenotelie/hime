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
use core::iter::FusedIterator;
use core::ops::Index;

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
                children: SppfImplNodeChildren::Single(buffer[count])
            }
        } else if count == 2 {
            SppfImplNodeVersion {
                label,
                children: SppfImplNodeChildren::Two([buffer[count], buffer[count + 1]])
            }
        } else if count == 3 {
            SppfImplNodeVersion {
                label,
                children: SppfImplNodeChildren::Three([
                    buffer[count],
                    buffer[count + 1],
                    buffer[count + 2]
                ])
            }
        } else if count == 4 {
            SppfImplNodeVersion {
                label,
                children: SppfImplNodeChildren::Four([
                    buffer[count],
                    buffer[count + 1],
                    buffer[count + 2],
                    buffer[count + 3]
                ])
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
    /// Always return `false
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
    pub fn get_node_mut(&mut self, identifier: usize) -> &mut SppfImplNode {
        &mut self.nodes[identifier]
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
    pub fn get_root(&self) -> SppfNode {
        let node = self.data.nodes[self.data.root.expect("No root defined!")].as_normal();
        SppfNode { sppf: self, node }
    }

    /// Gets the SPPF node (if any) that has the specified token as label
    #[must_use]
    pub fn find_node_for(&self, token: &Token) -> Option<SppfNodeVersion> {
        self.data
            .nodes
            .iter()
            .find_map(|node| {
                let normal = node.as_normal();
                normal.versions.into_iter().find(|version| {
                    version.label.table_type() == TableType::Token
                        && version.label.index() == token.index
                })
            })
            .map(|version| SppfNodeVersion {
                sppf: self,
                version
            })
    }

    /// Gets the AST node (if any) that has
    /// a token label that contains the specified index in the input text
    #[must_use]
    pub fn find_node_at_index(&self, index: usize) -> Option<SppfNodeVersion> {
        self.tokens
            .find_token_at(index)
            .and_then(|token| self.find_node_for(&token))
    }

    /// Gets the AST node (if any) that has
    /// a token label that contains the specified index in the input text
    #[must_use]
    pub fn find_node_at_position(&self, position: TextPosition) -> Option<SppfNodeVersion> {
        let index = self.tokens.text.get_line_index(position.line) + position.column - 1;
        self.tokens
            .find_token_at(index)
            .and_then(|token| self.find_node_for(&token))
    }
}

/// Represents a node in a Shared-Packed Parse Forest
#[derive(Copy, Clone)]
pub struct SppfNode<'s, 't, 'a> {
    /// The parent SPPF
    sppf: &'a Sppf<'s, 't, 'a>,
    /// The underlying node in the SPPF implementation
    node: &'a SppfImplNodeNormal
}

impl<'s, 't, 'a> SppfNode<'s, 't, 'a> {
    /// Gets the first version of this node
    #[must_use]
    pub fn first_version(&self) -> SppfNodeVersion<'s, 't, 'a> {
        let version = &self.node.versions[0];
        SppfNodeVersion {
            sppf: self.sppf,
            version
        }
    }

    /// Gets the different versions of this node
    #[must_use]
    pub fn versions(&self) -> SppfNodeVersions<'s, 't, 'a> {
        SppfNodeVersions {
            sppf: self.sppf,
            node: self.node
        }
    }
}

#[derive(Copy, Clone)]
pub struct SppfNodeVersions<'s, 't, 'a> {
    /// The parent SPPF
    sppf: &'a Sppf<'s, 't, 'a>,
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
    /// Always return `false
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
            node: self.node,
            current: 0,
            end: self.node.versions.len()
        }
    }
}

#[derive(Copy, Clone)]
pub struct SppfNodeVersionsIterator<'s, 't, 'a> {
    /// The parent SPPF
    sppf: &'a Sppf<'s, 't, 'a>,
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
                version: &self.node.versions[index]
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
                version: &self.node.versions[self.end - 1]
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
    version: &'a SppfImplNodeVersion
}

impl<'s, 't, 'a> SppfNodeVersion<'s, 't, 'a> {
    /// Gets the children for this version of the node
    #[must_use]
    pub fn children(&self) -> SppfNodeChildren<'s, 't, 'a> {
        SppfNodeChildren {
            sppf: self.sppf,
            version: self.version
        }
    }
}

impl<'s, 't, 'a> SemanticElementTrait<'s, 'a> for SppfNodeVersion<'s, 't, 'a> {
    /// Gets the position in the input text of this element
    fn get_position(&self) -> Option<TextPosition> {
        let label = self.version.label;
        match label.table_type() {
            TableType::Token => {
                let token = self.sppf.tokens.get_token(label.index());
                token.get_position()
            }
            _ => None
        }
    }

    /// Gets the span in the input text of this element
    fn get_span(&self) -> Option<TextSpan> {
        let label = self.version.label;
        match label.table_type() {
            TableType::Token => {
                let token = self.sppf.tokens.get_token(label.index());
                token.get_span()
            }
            _ => None
        }
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
}

impl<'s, 't, 'a> IntoIterator for SppfNodeChildren<'s, 't, 'a> {
    type Item = SppfNode<'s, 't, 'a>;
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
    type Item = SppfNode<'s, 't, 'a>;

    fn next(&mut self) -> Option<Self::Item> {
        if self.index < self.end {
            let node = self
                .sppf
                .data
                .get_node(self.version.children[self.index].node_id as usize)
                .as_normal();
            let result = Some(SppfNode {
                sppf: self.sppf,
                node
            });
            self.index += 1;
            result
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
            let node = self
                .sppf
                .data
                .get_node(self.version.children[self.end - 1].node_id as usize)
                .as_normal();
            let result = Some(SppfNode {
                sppf: self.sppf,
                node
            });
            self.end -= 1;
            result
        }
    }
}

impl<'s, 't, 'a> ExactSizeIterator for SppfNodeChildrenIterator<'s, 't, 'a> {}
impl<'s, 't, 'a> FusedIterator for SppfNodeChildrenIterator<'s, 't, 'a> {}
