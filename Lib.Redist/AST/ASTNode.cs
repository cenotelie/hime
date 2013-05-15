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
