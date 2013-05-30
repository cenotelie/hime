using System.Collections.Generic;

namespace Hime.Redist
{
    /// <summary>
    /// Represents a node in an Abstract Syntax Tree
    /// </summary>
    public struct ASTNode
    {
        private ParseTree tree;
        private int index;

        /// <summary>
        /// Gets the symbol in this node
        /// </summary>
        public Symbols.Symbol Symbol { get { return tree.GetSymbolAt(index); } }

        /// <summary>
        /// Gets the children of this node
        /// </summary>
        public ASTFamily Children { get { return new ASTFamily(tree, index); } }

        internal ASTNode(ParseTree tree, int index)
        {
            this.tree = tree;
            this.index = index;
        }

        /// <summary>
        /// Gets a string representation of this node
        /// </summary>
        /// <returns>The name of the current node's symbol; or "null" if the node does not have a symbol</returns>
        public override string ToString()
        {
            Symbols.Symbol symbol = tree.GetSymbolAt(index);
            if (symbol != null)
                return symbol.ToString();
            else
                return "null";
        }
    }
}
