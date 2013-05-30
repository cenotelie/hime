using System;
using System.Collections.Generic;

namespace Hime.Redist
{
    /// <summary>
    /// Represents an Abstract Syntax Tree produced by a parser
    /// </summary>
    public sealed class ParseTree
    {
        internal struct Cell
        {
            public Symbols.Symbol symbol;
            public int first;
            public int count;
        }

        private Cell[][] chunks;
        private int chunkIndex;
        private int cellIndex;
        private int root;

        /// <summary>
        /// Gets the root node of this tree
        /// </summary>
        public ASTNode Root { get { return new ASTNode(this, root); } }

        internal ParseTree()
        {
            this.chunks = new Cell[1024][];
            this.chunks[0] = new Cell[1024];
            this.chunkIndex = 0;
            this.cellIndex = 0;
            this.root = -1;
        }

        internal Symbols.Symbol GetSymbolAt(int index)
        {
            return chunks[index >> 10][index & 0x3FF].symbol;
        }

        internal int GetChildrenCountAt(int index)
        {
            return chunks[index >> 10][index & 0x3FF].count;
        }

        internal ASTNode GetChildrenAt(int pi, int ci)
        {
            return new ASTNode(this, chunks[pi >> 10][pi & 0x3FF].first + ci);
        }

        internal IEnumerator<ASTNode> GetEnumeratorAt(int index)
        {
            Cell cell = chunks[index >> 10][index & 0x3FF];
            return new Enumerator(this, cell.first - 1, cell.first + cell.count);
        }

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
