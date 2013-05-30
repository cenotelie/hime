using System.Collections.Generic;

namespace Hime.Redist
{
    /// <summary>
    /// Represents a family of children for an ASTNode
    /// </summary>
    public struct ASTFamily : IEnumerable<ASTNode>
    {
        private ParseTree tree;
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
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { return tree.GetEnumeratorAt(parent); }

        internal ASTFamily(ParseTree tree, int parent)
        {
            this.tree = tree;
            this.parent = parent;
        }
    }
}
