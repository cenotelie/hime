using System.Collections.Generic;

namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Specifies the tree action for a given node
    /// </summary>
    public enum SyntaxTreeNodeAction
    {
        /// <summary>
        /// Promote the node to the immediately upper level in the tree
        /// </summary>
        Promote,
        /// <summary>
        /// Drop the node and all the children from the tree
        /// </summary>
        Drop,
        /// <summary>
        /// Replace the node by its children
        /// </summary>
        Replace,
        /// <summary>
        /// Default action for a node, do nothing
        /// </summary>
        Nothing
    }

    /// <summary>
    /// Represents an abstract syntax tree node
    /// </summary>
    public sealed class SyntaxTreeNode
    {
        private Dictionary<string, object> properties;
        private List<SyntaxTreeNode> children;
        private SyntaxTreeNode parent;
        private Symbol symbol;
        private SyntaxTreeNodeAction action;
        private System.Collections.ObjectModel.ReadOnlyCollection<SyntaxTreeNode> readonlyChildren;

        /// <summary>
        /// Gets a dictionary of user properties attached to this node
        /// </summary>
        public Dictionary<string, object> Properties
        {
            get
            {
                if (properties == null)
                    properties = new Dictionary<string, object>();
                return properties;
            }
        }
        /// <summary>
        /// Gets the symbol attached to this node
        /// </summary>
        public Symbol Symbol { get { return symbol; } }
        /// <summary>
        /// Gets the parent node
        /// </summary>
        public SyntaxTreeNode Parent { get { return parent; } }
        /// <summary>
        /// Gets a read-only list of the children nodes
        /// </summary>
        public IList<SyntaxTreeNode> Children
        {
            get
            {
                if (readonlyChildren == null)
                    readonlyChildren = new System.Collections.ObjectModel.ReadOnlyCollection<SyntaxTreeNode>(children);
                return readonlyChildren;
            }
        }

        /// <summary>
        /// Initilizes a new instance of the SyntaxTreeNode class with the given symbol
        /// </summary>
        /// <param name="symbol">The symbol to attach to this node</param>
        public SyntaxTreeNode(Symbol symbol)
        {
            this.children = new List<SyntaxTreeNode>();
            this.symbol = symbol;
            this.action = SyntaxTreeNodeAction.Nothing;
        }
        /// <summary>
        /// Initilizes a new instance of the SyntaxTreeNode class with the given symbol and action
        /// </summary>
        /// <param name="symbol">The symbol to attach to this node</param>
        /// <param name="action">The action for this node</param>
        public SyntaxTreeNode(Symbol symbol, SyntaxTreeNodeAction action)
        {
            this.children = new List<SyntaxTreeNode>();
            this.symbol = symbol;
            this.action = action;
        }

        /// <summary>
        /// Adds a node as a child after removing it from its original tree if needed
        /// </summary>
        /// <param name="node">The node to append</param>
        public void AppendChild(SyntaxTreeNode node)
        {
            if (node.parent != null)
                node.parent.children.Remove(node);
            node.parent = this;
            children.Add(node);
        }
        /// <summary>
        /// Adds a node as a child with the given action after removing it from its original tree if needed
        /// </summary>
        /// <param name="node">The node to append</param>
        /// <param name="action">The action for the node</param>
        public void AppendChild(SyntaxTreeNode node, SyntaxTreeNodeAction action)
        {
            if (node.parent != null)
                node.parent.children.Remove(node);
            node.parent = this;
            node.action = action;
            children.Add(node);
        }
        /// <summary>
        /// Adds a range of nodes as children
        /// </summary>
        /// <param name="nodes">The nodes to append</param>
        public void AppendRange(ICollection<SyntaxTreeNode> nodes)
        {
            List<SyntaxTreeNode> Temp = new List<SyntaxTreeNode>(nodes);
            foreach (SyntaxTreeNode Node in Temp)
                AppendChild(Node);
        }

        /// <summary>
        /// Apply actions to this node and all its children
        /// </summary>
        /// <returns>The new root</returns>
        internal SyntaxTreeNode ApplyActions()
        {
            Visit_DropReplace();
            return Visit_Promote();
        }

        private void Visit_DropReplace()
        {
            Stack<SyntaxTreeNode> nodes = new Stack<SyntaxTreeNode>();
            Stack<bool> visited = new Stack<bool>();
            nodes.Push(this);
            visited.Push(false);

            while (nodes.Count != 0)
            {
                SyntaxTreeNode current = nodes.Pop();
                bool isVisisted = visited.Pop();
                if (isVisisted)
                {
                    // post-order
                    // Drop replaced node
                    if (current.action != SyntaxTreeNodeAction.Replace)
                    {
                        // Redo binding by adding the children
                        if (current.parent != null)
                            current.parent.children.Add(current);
                    }
                }
                else
                {
                    nodes.Push(current);
                    visited.Push(true);
                    // Pre-order
                    for (int i = current.children.Count - 1; i != -1; i--)
                    {
                        SyntaxTreeNode child = current.children[i];
                        // prepare replace => setup parency
                        if (current.action == SyntaxTreeNodeAction.Replace)
                            child.parent = current.parent;
                        // if action is drop => drop the child now by not adding it to the stack
                        if (child.action == SyntaxTreeNodeAction.Drop)
                            continue;
                        else if (child.symbol is SymbolTokenText)
                        {
                            SymbolTokenText TokenText = (SymbolTokenText)child.symbol;
                            if (TokenText.SubGrammarRoot != null)
                            {
                                // there is a subgrammar => build parency and add to the stack
                                child = TokenText.SubGrammarRoot;
                                child.parent = current;
                            }
                        }
                        nodes.Push(child);
                        visited.Push(false);
                    }
                    // clear the children => rebuild in postorder
                    current.children.Clear();
                }
            }
        }

        private SyntaxTreeNode Visit_Promote()
        {
            LinkedList<SyntaxTreeNode> nodes = new LinkedList<SyntaxTreeNode>();
            LinkedList<bool> visited = new LinkedList<bool>();
            LinkedList<LinkedListNode<SyntaxTreeNode>> parents = new LinkedList<LinkedListNode<SyntaxTreeNode>>();
            
            nodes.AddLast(this);
            visited.AddLast(false);
            parents.AddLast(new LinkedListNode<SyntaxTreeNode>(null));
            SyntaxTreeNode current = null;

            while (nodes.Count != 0)
            {
                current = nodes.Last.Value;
                bool isVisisted = visited.Last.Value;
                if (isVisisted)
                {
                    LinkedListNode<SyntaxTreeNode> parentNode = parents.Last.Value;
                    SyntaxTreeNode oldParent = parentNode.Value;
                    nodes.RemoveLast();
                    visited.RemoveLast();
                    parents.RemoveLast();
                    // post-order
                    if (current.action == SyntaxTreeNodeAction.Promote)
                    {
                        current.action = oldParent.action;
                        if (current.parent == oldParent)
                        {
                            current.parent = oldParent.parent;
                            foreach (SyntaxTreeNode left in oldParent.children)
                                left.parent = current;
                            current.children.InsertRange(0, oldParent.children);
                        }
                        else
                        {
                            current.parent = oldParent.parent;
                            current.children.Insert(0, oldParent);
                            oldParent.parent = current;
                        }
                        parentNode.Value = current;
                    }
                    else
                    {
                        current.parent = oldParent;
                        if (oldParent != null)
                            current.parent.children.Add(current);
                    }
                }
                else
                {
                    LinkedListNode<SyntaxTreeNode> currentNode = nodes.Last;
                    visited.RemoveLast();
                    visited.AddLast(true);
                    // Pre-order
                    for (int i = current.children.Count - 1; i != -1; i--)
                    {
                        SyntaxTreeNode child = current.children[i];
                        nodes.AddLast(child);
                        visited.AddLast(false);
                        parents.AddLast(currentNode);
                    }
                    current.children.Clear();
                }
            }
            return current;
        }

        /// <summary>
        /// Builds an XML node representing this node in the context of the given XML document
        /// </summary>
        /// <param name="doc">The document that will own the XML node</param>
        /// <returns>The XML node representing this node</returns>
        public System.Xml.XmlNode GetXMLNode(System.Xml.XmlDocument doc)
        {
            System.Xml.XmlNode Node = doc.CreateElement(symbol.Name);
            if (symbol is SymbolToken)
            {
                SymbolToken Token = (SymbolToken)symbol;
                Node.AppendChild(doc.CreateTextNode(Token.ToString()));
            }
            foreach (string Property in properties.Keys)
            {
                System.Xml.XmlAttribute Attribute = doc.CreateAttribute(Property);
                Attribute.Value = properties[Property].ToString();
                Node.Attributes.Append(Attribute);
            }
            foreach (SyntaxTreeNode Child in children)
                Node.AppendChild(Child.GetXMLNode(doc));
            return Node;
        }

        public override string ToString()
        {
            if (symbol != null)
                return symbol.ToString();
            else
                return "null";
        }
    }
}
