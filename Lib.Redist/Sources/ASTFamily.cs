/**********************************************************************
* Copyright (c) 2013 Laurent Wouters and others
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
* 
* Contributors:
*     Laurent Wouters - lwouters@xowl.org
**********************************************************************/

using System.Collections;
using System.Collections.Generic;

namespace Hime.Redist
{
    /// <summary>
    /// Represents a family of children for an ASTNode
    /// </summary>
    public struct ASTFamily : IEnumerable<ASTNode>
    {
        /// <summary>
        /// The original parse tree
        /// </summary>
        private ParseTree tree;
        /// <summary>
        /// The index of the parent node in the parse tree
        /// </summary>
        private int parent;

        /// <summary>
        /// Gets the number of children
        /// </summary>
        public int Count { get { return tree.GetChildrenCountAt(parent); } }

        /// <summary>
        /// Gets the i-th child
        /// </summary>
        /// <param name="index">The index of the child</param>
        /// <returns>The child at the given index</returns>
        public ASTNode this[int index] { get { return tree.GetChildrenAt(parent, index); } }

        /// <summary>
        /// Gets an enumeration of the children
        /// </summary>
        /// <returns>An enumeration of the children</returns>
        public IEnumerator<ASTNode> GetEnumerator() { return tree.GetEnumeratorAt(parent); }

        /// <summary>
        /// Gets an enumeration of the children
        /// </summary>
        /// <returns>An enumeration of the children</returns>
        IEnumerator IEnumerable.GetEnumerator() { return tree.GetEnumeratorAt(parent); }

        /// <summary>
        /// Initializes this family
        /// </summary>
        /// <param name="tree">The parent parse tree</param>
        /// <param name="parent">The index of the parent node in the parse tree</param>
        internal ASTFamily(ParseTree tree, int parent)
        {
            this.tree = tree;
            this.parent = parent;
        }
    }
}
