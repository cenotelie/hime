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

using System;
using System.Collections.Generic;

namespace Hime.Redist
{
    /// <summary>
    /// Represents an Abstract Syntax Tree produced by a parser
    /// </summary>
    class ParseTree
    {
        /// <summary>
        /// Represents the data of a node in this AST
        /// </summary>
        public struct Cell
        {
            public Symbols.Symbol symbol;
            public int first;
            public int count;

            public Cell(Symbols.Symbol symbol)
            {
                this.symbol = symbol;
                this.first = 0;
                this.count = 0;
            }

            public Cell(Symbols.Symbol symbol, int first, int count)
            {
                this.symbol = symbol;
                this.first = first;
                this.count = count;
            }
        }

        private Utils.BigList<Cell> cells;
        private int root;

        /// <summary>
        /// Gets the root node of this tree
        /// </summary>
        public ASTNode Root { get { return new ASTNode(this, root); } }

        /// <summary>
        /// Initializes the AST's internal data
        /// </summary>
        public ParseTree()
        {
            this.cells = new Utils.BigList<Cell>();
            this.root = -1;
        }

        /// <summary>
        /// Gets the symbol of the node at the given index
        /// </summary>
        /// <param name="index">The node's index</param>
        /// <returns>The node's symbol</returns>
        public Symbols.Symbol GetSymbolAt(int index)
        {
            return cells[index].symbol;
        }

        /// <summary>
        /// Gets the number of children of the node at the given index
        /// </summary>
        /// <param name="index">The node's index</param>
        /// <returns>The node's numer of children</returns>
        public int GetChildrenCountAt(int index)
        {
            return cells[index].count;
        }

        /// <summary>
        /// Gets the i-th child of a node
        /// </summary>
        /// <param name="parentIndex">The index of the parent node</param>
        /// <param name="i">The child's number</param>
        /// <returns>The i-th child</returns>
        public ASTNode GetChildrenAt(int parentIndex, int i)
        {
            return new ASTNode(this, cells[parentIndex].first + i);
        }

        /// <summary>
        /// Gets an enumerator for the children of the node at the given index
        /// </summary>
        /// <param name="index">The node's index</param>
        /// <returns>An enumerator for the children</returns>
        public IEnumerator<ASTNode> GetEnumeratorAt(int index)
        {
            Cell cell = cells[index];
            return new ASTNodeEnumerator(this, cell.first - 1, cell.first + cell.count);
        }

        /// <summary>
        /// Stores new nodes in this tree
        /// </summary>
        /// <param name="cells">An array of items</param>
        /// <param name="index">The starting index of the items to store</param>
        /// <param name="length">The number of items to store</param>
        /// <returns>The index within this tree at which the items have been inserted</returns>
        public int Store(Cell[] cells, int index, int length)
        {
            return this.cells.Add(cells, index, length);
        }

        /// <summary>
        /// Setups the root of this tree
        /// </summary>
        /// <param name="cell">The root</param>
        public void StoreRoot(Cell cell)
        {
            this.root = cells.Add(cell);
        }
    }
}
