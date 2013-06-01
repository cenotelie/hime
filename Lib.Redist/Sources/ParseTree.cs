using System;
using System.Collections.Generic;

namespace Hime.Redist
{
    /// <summary>
    /// Represents an Abstract Syntax Tree produced by a parser
    /// </summary>
    public sealed class ParseTree
    {
        /// <summary>
        /// Represents the data of a node in this AST
        /// </summary>
        internal struct Cell
        {
            public Symbols.Symbol symbol;
            public int first;
            public int count;
        }

        private Cell[][] chunks; // The content of the tree
        private int chunkIndex;  // The index of the current chunck being filled
        private int cellIndex;   // The index of the next cell to be filled (relatively to the selected chunck)
        private int root;        // The index of the AST's root

        /// <summary>
        /// Gets the root node of this tree
        /// </summary>
        public ASTNode Root { get { return new ASTNode(this, root); } }

        /// <summary>
        /// Initializes the AST's internal data
        /// </summary>
        internal ParseTree()
        {
            this.chunks = new Cell[1024][];
            this.chunks[0] = new Cell[1024];
            this.chunkIndex = 0;
            this.cellIndex = 0;
            this.root = -1;
        }

        /// <summary>
        /// Gets the symbol of the node at the given index
        /// </summary>
        /// <param name="index">The node's index</param>
        /// <returns>The node's symbol</returns>
        internal Symbols.Symbol GetSymbolAt(int index)
        {
            return chunks[index >> 10][index & 0x3FF].symbol;
        }

        /// <summary>
        /// Gets the number of children of the node at the given index
        /// </summary>
        /// <param name="index">The node's index</param>
        /// <returns>The node's numer of children</returns>
        internal int GetChildrenCountAt(int index)
        {
            return chunks[index >> 10][index & 0x3FF].count;
        }

        /// <summary>
        /// Gets the i-th child of a node
        /// </summary>
        /// <param name="parentIndex">The index of the parent node</param>
        /// <param name="i">The child's number</param>
        /// <returns>The i-th child</returns>
        internal ASTNode GetChildrenAt(int parentIndex, int i)
        {
            return new ASTNode(this, chunks[parentIndex >> 10][parentIndex & 0x3FF].first + i);
        }

        /// <summary>
        /// Gets an enumerator for the children of the node at the given index
        /// </summary>
        /// <param name="index">The node's index</param>
        /// <returns>An enumerator for the children</returns>
        internal IEnumerator<ASTNode> GetEnumeratorAt(int index)
        {
            Cell cell = chunks[index >> 10][index & 0x3FF];
            return new Enumerator(this, cell.first - 1, cell.first + cell.count);
        }

        /// <summary>
        /// Stores new nodes in this tree
        /// </summary>
        /// <param name="cells">An array of items</param>
        /// <param name="index">The starting index of the items to store</param>
        /// <param name="length">The number of items to store</param>
        /// <returns>The index within this tree at which the items have been inserted</returns>
        internal int Store(Cell[] cells, int index, int length)
        {
            int start = (chunkIndex << 10 | cellIndex);
            while (cellIndex + length > 1024)
            {
                int count = 1024 - cellIndex;
                if (count == 0)
                {
                    AddChunk();
                    continue;
                }
                Array.Copy(cells, index, chunks[chunkIndex], cellIndex, count);
                index += count;
                length -= count;
                AddChunk();
            }
            Array.Copy(cells, index, chunks[chunkIndex], cellIndex, length);
            cellIndex += length;
            return start;
        }

        /// <summary>
        /// Setups the root of this tree
        /// </summary>
        /// <param name="cell">The root</param>
        internal void StoreRoot(Cell cell)
        {
            if (cellIndex == 1024)
                AddChunk();
            chunks[chunkIndex][cellIndex] = cell;
            root = (chunkIndex << 10 | cellIndex);
            cellIndex++;
        }

        private void AddChunk()
        {
            Cell[] t = new Cell[1024];
            if (chunkIndex == chunks.Length - 1)
            {
                Cell[][] r = new Cell[chunks.Length + 1024][];
                Array.Copy(chunks, r, chunks.Length);
                chunks = r;
            }
            chunks[++chunkIndex] = t;
            cellIndex = 0;
        }

        private class Enumerator : IEnumerator<ASTNode>
        {
            private ParseTree tree;
            private int start;
            private int end;
            private int current;

            public ASTNode Current { get { return new ASTNode(tree, current); } }
            object System.Collections.IEnumerator.Current { get { return new ASTNode(tree, current); } }

            public Enumerator(ParseTree tree, int start, int end)
            {
                this.tree = tree;
                this.start = start;
                this.end = end;
                this.current = start;
            }

            public void Dispose()
            {
                tree = null;
            }

            public bool MoveNext()
            {
                current++;
                return (current != end);
            }

            public void Reset()
            {
                current = start;
            }
        }
    }
}
