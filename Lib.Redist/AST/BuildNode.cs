namespace Hime.Redist.AST
{
    /// <summary>
    /// Represents a node in the raw AST produced by a LR(k) parser before tree actions
    /// Tree actions are directly executed on this structure, producing the final AST
    /// </summary>
    class BuildNode
    {
        /// <summary>
        /// The final AST node that would correspond to this node
        /// </summary>
        protected ASTNode value;
        /// <summary>
        /// The children of this node
        /// </summary>
        protected BuildNode[] children;
        /// <summary>
        /// The AST action for this node
        /// </summary>
        protected int action;

        /// <summary>
        /// Gets the final AST node for this node
        /// </summary>
        public ASTNode Value { get { return value; } }

        /// <summary>
        /// Initializes this build node with the given symbol
        /// </summary>
        /// <param name="symbol">The symbol for this node</param>
        public BuildNode(Symbols.Symbol symbol)
        {
            this.value = new ASTNode(symbol);
        }

        /// <summary>
        /// Sets the tree action for this node, provided it is not 0 (no action)
        /// </summary>
        /// <param name="action">The tree action for this node</param>
        public void SetAction(int action)
        {
            if (action != 0)
                this.action = action;
        }

        /// <summary>
        /// Builds the AST at this level with the given children
        /// </summary>
        /// <typeparam name="T">The type of children</typeparam>
        /// <param name="children">Array of children</param>
        /// <param name="length">Number of children</param>
        public void Build<T>(T[] children, int length) where T : BuildNode
        {
            if (action == Parsers.LRProduction.HeadReplace)
                BuildReplaceable(children, length);
            else
                BuildNormal(children, length);
        }

        /// <summary>
        /// Builds the AST at this level as a replaceable node
        /// </summary>
        /// <param name="children">Array of children</param>
        /// <param name="length">Number of children</param>
        private void BuildReplaceable<T>(T[] children, int length) where T : BuildNode
        {
            int size = 0;
            for (int i = 0; i != length; i++)
            {
                BuildNode child = children[i];
                switch (child.action)
                {
                    case Parsers.LRProduction.HeadReplace:
                        size += child.children.Length;
                        break;
                    case Parsers.LRBytecode.PopDrop:
                        break;
                    default:
                        size++;
                        break;
                }
            }
            BuildNode[] replacement = new BuildNode[size];
            int index = 0;
            for (int i = 0; i != length; i++)
            {
                BuildNode child = children[i];
                switch (child.action)
                {
                    case Parsers.LRProduction.HeadReplace:
                        System.Array.Copy(child.children, 0, replacement, index, child.children.Length);
                        index += child.children.Length;
                        child.children = null;
                        break;
                    case Parsers.LRBytecode.PopDrop:
                        break;
                    default:
                        replacement[index] = child;
                        index++;
                        break;
                }
            }
            this.children = replacement;
        }

        /// <summary>
        /// Builds the AST at this level as a normal node
        /// </summary>
        /// <param name="children">Array of children</param>
        /// <param name="length">Number of children</param>
        private void BuildNormal<T>(T[] children, int length) where T : BuildNode
        {
            bool firstPromote = true;
            for (int i = 0; i != length; i++)
            {
                BuildNode child = children[i];
                switch (child.action)
                {
                    case Parsers.LRProduction.HeadReplace:
                        foreach (BuildNode subchild in child.children)
                            firstPromote = ExecuteReplacement(subchild, firstPromote);
                        child.children = null;
                        break;
                    case Parsers.LRBytecode.PopDrop:
                        break;
                    case Parsers.LRBytecode.PopPromote:
                        if (firstPromote)
                        {
                            child.value.Children.InsertRange(0, this.value.Children);
                            this.value = child.value;
                            firstPromote = false;
                        }
                        else
                        {
                            child.value.Children.Insert(0, this.value);
                            this.value = child.value;
                        }
                        break;
                    default:
                        this.value.Children.Add(child.value);
                        break;
                }
            }
        }

        private bool ExecuteReplacement(BuildNode child, bool firstPromote)
        {
            if (child.action != Parsers.LRBytecode.PopPromote)
            {
                this.value.Children.Add(child.value);
                return firstPromote;
            }
            else if (firstPromote)
            {
                child.value.Children.InsertRange(0, this.value.Children);
                this.value = child.value;
            }
            else
            {
                child.value.Children.Insert(0, this.value);
                this.value = child.value;
            }
            return false;
        }
    }
}
