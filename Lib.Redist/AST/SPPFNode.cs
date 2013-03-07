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
    internal class SPPFNode
    {
        internal CSTNode value;
        internal SPPFNode parent;
        internal CSTNode originalParent;
        private int action;
        private bool visited;
        private int generation;
        private List<SPPFFamily> families;

        /// <summary>
        /// Gets the symbol associated to this node
        /// </summary>
        public Symbols.Symbol Symbol { get { return value.Symbol; } }

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
            this.value = new CSTNode(symbol);
            this.generation = gen;
            this.families = new List<SPPFFamily>();
        }
        /// <summary>
        /// Initializes a new instance of the SPPFNode class with the given symbol, generation and action
        /// </summary>
        /// <param name="symbol">The symbol represented by this node</param>
        /// <param name="gen">The generation of this node</param>
        /// <param name="action">The action for this node</param>
        public SPPFNode(Symbols.Symbol symbol, int gen, int action)
        {
            this.value = new CSTNode(symbol);
            this.action = action;
            this.generation = gen;
            this.families = new List<SPPFFamily>();
        }

        /// <summary>
        /// Sets the action for this SPPF node
        /// </summary>
        /// <param name="action">The action value</param>
        public void SetAction(int action) { this.action = action; }

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
            if (!this.value.Symbol.Equals(node.value.Symbol))
                return false;
            return (this.generation == node.generation);
        }

        /// <summary>
        /// Gets the first tree stemming from this SPPF
        /// </summary>
        /// <returns>The first tree stemming from this SPPF</returns>
        public CSTNode GetFirstTree()
        {
            Stack<SPPFNode> stack = new Stack<SPPFNode>();
            stack.Push(this);
            SPPFNode current = null;
            while (stack.Count != 0)
            {
                current = stack.Peek();
                if (current.visited)
                {
                    stack.Pop();
                    // post-order
                    // Drop replaced node
                    if (current.action == Parsers.LRProduction.HeadReplace)
                        continue;
                    else if (current.action == Parsers.LRBytecode.PopPromote)
                    {
                        if (current.originalParent == current.parent.value)
                        {
                            // This is the first promote action found for this level
                            // move the parent's children onto self's children
                            current.value.Children.InsertRange(0, current.originalParent.Children);
                            // replace the parent by self
                            current.parent.value = current.value;
                        }
                        else
                        {
                            // The original parent is not the same as the current parent, a promote action has already been executed
                            // move the parent onto self's children
                            current.value.Children.Insert(0, current.parent.value);
                            // replace the parent by self
                            current.parent.value = current.value;
                        }
                    }
                    else if (current.parent != null) // current is not the root => setup as child
                        current.parent.value.Children.Add(current.value);
                }
                else
                {
                    current.visited = true;
                    // Pre-order
                    if (current.families.Count == 0)
                        continue;
                    for (int i = current.families[0].Count - 1; i != -1; i--)
                    {
                        SPPFNode child = current.families[0][i];
                        // prepare replace => setup parency
                        if (current.action == Parsers.LRProduction.HeadReplace)
                            child.parent = current.parent;
                        // if action is drop => drop the child now by not adding it to the stack
                        if (child.action == Parsers.LRBytecode.PopDrop)
                            continue;
                        stack.Push(child);
                    }
                }
            }
            return current.value;
        }
    }
}
