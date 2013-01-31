/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:25
 * 
 */
using System.Collections.Generic;

namespace Hime.Redist.AST
{
    /// <summary>
    /// Represents a node in a Shared Packed Parse Forest
    /// </summary>
    public sealed class SPPFNode
    {
        private Symbols.Symbol symbol;
        private CSTAction action;
        private int generation;
        private List<SPPFFamily> families;

        /// <summary>
        /// Gets the symbol associated to this node
        /// </summary>
        public Symbols.Symbol Symbol { get { return symbol; } }
        /// <summary>
        /// Gets or sets the action for this node
        /// </summary>
        public CSTAction Action
        {
            get { return action; }
            set { action = value; }
        }
        /// <summary>
        /// Gets the generation of this node
        /// </summary>
        public int Generation { get { return generation; } }

        /// <summary>
        /// Gets the families for this node
        /// </summary>
        public List<SPPFFamily> Families { get { return families; } }

        /// <summary>
        /// Initializes a new instance of the SPPFNode class with the given symbol and generation
        /// </summary>
        /// <param name="symbol">The symbol represented by this node</param>
        /// <param name="gen">The generation of this node</param>
        public SPPFNode(Symbols.Symbol symbol, int gen)
        {
            this.symbol = symbol;
            this.action = CSTAction.Nothing;
            this.generation = gen;
            this.families = new List<SPPFFamily>();
        }
        /// <summary>
        /// Initializes a new instance of the SPPFNode class with the given symbol, generation and action
        /// </summary>
        /// <param name="symbol">The symbol represented by this node</param>
        /// <param name="gen">The generation of this node</param>
        /// <param name="action">The action for this node</param>
        public SPPFNode(Symbols.Symbol symbol, int gen, CSTAction action)
        {
            this.symbol = symbol;
            this.action = action;
            this.generation = gen;
            this.families = new List<SPPFFamily>();
        }

        /// <summary>
        /// Adds a new family for this node
        /// </summary>
        /// <param name="family">The new family for this node</param>
        public void AddFamily(SPPFFamily family)
        {
            foreach (SPPFFamily potential in families)
                if (potential.EquivalentTo(family))
                    return;
            families.Add(family);
        }

        /// <summary>
        /// Determines whether this node is equivalent to the given one
        /// </summary>
        /// <param name="node">The tested node</param>
        /// <returns>True if the given node is equivalent to this node, false otherwise</returns>
        public bool EquivalentTo(SPPFNode node)
        {
            if (this.symbol == null)
            {
                if (node.symbol != null)
                    return false;
                return (this.generation == node.generation);
            }
            else
            {
                if (!this.symbol.Equals(node.symbol))
                    return false;
                return (this.generation == node.generation);
            }
        }

        /// <summary>
        /// Gets the first tree stemming from this SPPF
        /// </summary>
        /// <returns>The first tree stemming from this SPPF</returns>
        public CSTNode GetFirstTree()
        {
            CSTNode me = new CSTNode(symbol, action);
            if (families.Count == 1)
            {
                CSTNode[] buffer = new CSTNode[families[0].Children.Count];
                foreach (SPPFNode child in families[0].Children)
                {
                    me.AppendChild(child.GetFirstTree());
                }
            }
            else if (families.Count >= 1)
            {
                // More than one solution => this is an error
                foreach (SPPFFamily family in families)
                {
                    CSTNode subroot = new CSTNode(null, CSTAction.Nothing);
                    me.AppendChild(subroot);
                }
            }
            return me;
        }
    }
}
