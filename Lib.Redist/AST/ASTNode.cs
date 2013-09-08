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

namespace Hime.Redist.AST
{
    /// <summary>
    /// Represents a node in an Abstract Syntax Tree
    /// </summary>
    public sealed class ASTNode
    {
        private Symbols.Symbol symbol;
        private List<ASTNode> children;
        
        /// <summary>
        /// Gets the symbol attached to this node
        /// </summary>
        public Symbols.Symbol Symbol { get { return symbol; } }
        /// <summary>
        /// Gets a list of the children nodes
        /// </summary>
        public List<ASTNode> Children { get { return children; } }

        /// <summary>
        /// Initializes a new instance of the ASTNode class with the given symbol
        /// </summary>
        /// <param name="symbol">The symbol represented by this node</param>
        public ASTNode(Symbols.Symbol symbol)
        {
            this.symbol = symbol;
            this.children = new List<ASTNode>();
        }

        /// <summary>
        /// Gets a string representation of this node
        /// </summary>
        /// <returns>The name of the current node's symbol; or "null" if the node does not have a symbol</returns>
        public override string ToString()
        {
            if (symbol != null)
                return symbol.ToString();
            else
                return "null";
        }
    }
}
