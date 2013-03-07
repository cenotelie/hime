/*
 * Author: Laurent Wouters
 * Date: 07/03/2013
 * Time: 17:25
 * 
 */
using System.Collections.Generic;

namespace Hime.Redist.AST
{
    class BuildNode
    {
        private CSTNode value;
        private BuildNode parent;
        private CSTNode originalParent;
        private int action;
        private bool visited;
        private List<BuildNode> children;

        public Symbols.Symbol Symbol { get { return value.Symbol; } }

        public BuildNode(Symbols.Symbol symbol)
        {
            this.value = new CSTNode(symbol);
            this.children = new List<BuildNode>();
        }

        public void SetAction(int action)
        {
            if (action != 0)
                this.action = action;
        }

        public void AppendChild(BuildNode child)
        {
            child.parent = this;
            child.originalParent = this.value;
            this.children.Add(child);
        }

        public CSTNode GetTree()
        {
            Stack<BuildNode> stack = new Stack<BuildNode>();
            stack.Push(this);
            BuildNode current = null;
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
                    for (int i = current.children.Count - 1; i != -1; i--)
                    {
                        BuildNode child = current.children[i];
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
