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

using System.Collections.Generic;

namespace Hime.Redist
{
    /// <summary>
    /// Represents an enumerator of AST nodes that are children of the same parent
    /// </summary>
    internal class ASTNodeEnumerator : IEnumerator<ASTNode>
    {
        private ParseTree tree;
        private int start;
        private int end;
        private int current;

        /// <summary>
        /// Gets the current node
        /// </summary>
        public ASTNode Current { get { return new ASTNode(tree, current); } }

        /// <summary>
        /// Gets the current node
        /// </summary>
        object System.Collections.IEnumerator.Current { get { return new ASTNode(tree, current); } }

        /// <summary>
        /// Intializes this enumerator
        /// </summary>
        /// <param name="tree">The parent's parse tree</param>
        /// <param name="start">Index of the first node to enumerate</param>
        /// <param name="end">Index of the last node to enumerator (excluded)</param>
        public ASTNodeEnumerator(ParseTree tree, int start, int end)
        {
            this.tree = tree;
            this.start = start;
            this.end = end;
            this.current = start;
        }

        /// <summary>
        /// Disposes this enumerator
        /// </summary>
        public void Dispose()
        {
            tree = null;
        }

        /// <summary>
        /// Moves to the next node
        /// </summary>
        /// <returns>true if there are more nodes</returns>
        public bool MoveNext()
        {
            current++;
            return (current != end);
        }

        /// <summary>
        /// Resets this enumerator to the beginning
        /// </summary>
        public void Reset()
        {
            current = start;
        }
    }
}