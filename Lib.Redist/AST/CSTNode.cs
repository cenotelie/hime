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
    /// Represents an abstract syntax tree node
    /// </summary>
    public sealed class CSTNode
    {
        private Dictionary<string, object> properties;
        private List<CSTNode> children;
        private CSTNode parent;
        private Symbols.Symbol symbol;
        private CSTAction action;
        private System.Collections.ObjectModel.ReadOnlyCollection<CSTNode> readonlyChildren;

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
        public Symbols.Symbol Symbol { get { return symbol; } }
        /// <summary>
        /// Gets the parent node
        /// </summary>
        public CSTNode Parent { get { return parent; } }
        /// <summary>
        /// Gets a read-only list of the children nodes
        /// </summary>
        public IList<CSTNode> Children
        {
            get
            {
                if (readonlyChildren == null)
                    readonlyChildren = new System.Collections.ObjectModel.ReadOnlyCollection<CSTNode>(children);
                return readonlyChildren;
            }
        }

        /// <summary>
        /// Initilizes a new instance of the SyntaxTreeNode class with the given symbol
        /// </summary>
        /// <param name="symbol">The symbol to attach to this node</param>
        public CSTNode(Symbols.Symbol symbol)
        {
            this.children = new List<CSTNode>();
            this.symbol = symbol;
            this.action = CSTAction.Nothing;
        }
        /// <summary>
        /// Initilizes a new instance of the SyntaxTreeNode class with the given symbol and action
        /// </summary>
        /// <param name="symbol">The symbol to attach to this node</param>
        /// <param name="action">The action for this node</param>
        public CSTNode(Symbols.Symbol symbol, CSTAction action)
        {
            this.children = new List<CSTNode>();
            this.symbol = symbol;
            this.action = action;
        }

        /// <summary>
        /// Adds a node as a child after removing it from its original tree if needed
        /// </summary>
        /// <param name="node">The node to append</param>
        public void AppendChild(CSTNode node)
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
        public void AppendChild(CSTNode node, CSTAction action)
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
        public void AppendRange(ICollection<CSTNode> nodes)
        {
            List<CSTNode> Temp = new List<CSTNode>(nodes);
            foreach (CSTNode Node in Temp)
                AppendChild(Node);
        }

        private class StackNode
        {
            public CSTNode astNode;
            public bool visited;
            public StackNode parentNode;
            public StackNode(CSTNode ast, StackNode parent)
            {
                this.astNode = ast;
                this.visited = false;
                this.parentNode = parent;
            }
        }

        /// <summary>
        /// Apply actions to this node and all its children
        /// </summary>
        /// <returns>The new root</returns>
        internal CSTNode ApplyActions()
        {
            LinkedList<StackNode> stack = new LinkedList<StackNode>();
            stack.AddLast(new StackNode(this, null));
            StackNode current = null;

            while (stack.Count != 0)
            {
                current = stack.Last.Value;
                if (current.visited)
                {
                    stack.RemoveLast();
                    // post-order
                    // Drop replaced node
                    if (current.astNode.action == CSTAction.Replace)
                        continue;
                    if (current.astNode.action == CSTAction.Promote)
                    {
                        StackNode parentNode = current.parentNode;
                        CSTNode oldParent = parentNode.astNode;
                        current.astNode.action = oldParent.action;
                        if (current.astNode.parent == oldParent)
                        {
                            current.astNode.parent = oldParent.parent;
                            foreach (CSTNode left in oldParent.children)
                                left.parent = current.astNode;
                            current.astNode.children.InsertRange(0, oldParent.children);
                        }
                        else
                        {
                            current.astNode.parent = oldParent.parent;
                            current.astNode.children.Insert(0, oldParent);
                            oldParent.parent = current.astNode;
                        }
                        parentNode.astNode = current.astNode;
                    }
                    else
                    {
                        if (current.parentNode != null)
                        {
                            current.astNode.parent = current.parentNode.astNode;
                            current.astNode.parent.children.Add(current.astNode);
                        }
                    }
                }
                else
                {
                    current.visited = true;
                    // Pre-order
                    for (int i = current.astNode.children.Count - 1; i != -1; i--)
                    {
                        StackNode parentToPush = current;
                        CSTNode child = current.astNode.children[i];
                        // prepare replace => setup parency
                        if (current.astNode.action == CSTAction.Replace)
                        {
                            child.parent = current.astNode.parent;
                            parentToPush = current.parentNode;
                        }
                        // if action is drop => drop the child now by not adding it to the stack
                        if (child.action == CSTAction.Drop)
                            continue;
                        else if (child.symbol is Symbols.TextToken)
                        {
                            Symbols.TextToken TokenText = (Symbols.TextToken)child.symbol;
                            if (TokenText.SubGrammarRoot != null)
                            {
                                // there is a subgrammar => build parency and add to the stack
                                child = TokenText.SubGrammarRoot;
                                child.parent = current.astNode;
                            }
                        }
                        stack.AddLast(new StackNode(child, parentToPush));
                    }
                    // clear the children => rebuild in postorder
                    current.astNode.children.Clear();
                }
            }
            return current.astNode;
        }


        /// <summary>
        /// Builds an XML node representing this node in the context of the given XML document
        /// </summary>
        /// <param name="doc">The document that will own the XML node</param>
        /// <returns>The XML node representing this node</returns>
        public System.Xml.XmlNode GetXMLNode(System.Xml.XmlDocument doc)
        {
            System.Xml.XmlNode Node = doc.CreateElement(symbol.Name);
            if (symbol is Symbols.Token)
            {
                Symbols.Token Token = (Symbols.Token)symbol;
                Node.AppendChild(doc.CreateTextNode(Token.ToString()));
            }
            foreach (string Property in properties.Keys)
            {
                System.Xml.XmlAttribute Attribute = doc.CreateAttribute(Property);
                Attribute.Value = properties[Property].ToString();
                Node.Attributes.Append(Attribute);
            }
            foreach (CSTNode Child in children)
                Node.AppendChild(Child.GetXMLNode(doc));
            return Node;
        }

        /// <summary>
        /// Gets a string representation of this node
        /// </summary>
        /// <returns>The name of the current node's symbol; or "null" if there the node does not have a symbol</returns>
        public override string ToString()
        {
            if (symbol != null)
                return symbol.ToString();
            else
                return "null";
        }
    }
}
