using System.Collections.Generic;

namespace Hime.Redist.Parsers
{
    public enum SyntaxTreeNodeAction
    {
        Promote,
        Drop,
        Replace,
        Nothing
    }

    public sealed class SyntaxTreeNode
    {
        private Dictionary<string, object> properties;
        private List<SyntaxTreeNode> children;
        private SyntaxTreeNode parent;
        private Symbol symbol;
        private SyntaxTreeNodeAction action;
        private System.Collections.ObjectModel.ReadOnlyCollection<SyntaxTreeNode> readonlyChildren;

        public Dictionary<string, object> Properties
        {
            get
            {
                if (properties == null)
                    properties = new Dictionary<string, object>();
                return properties;
            }
        }
        public Symbol Symbol { get { return symbol; } }
        public SyntaxTreeNode Parent { get { return parent; } }
        public IList<SyntaxTreeNode> Children
        {
            get
            {
                if (readonlyChildren == null)
                    readonlyChildren = new System.Collections.ObjectModel.ReadOnlyCollection<SyntaxTreeNode>(children);
                return readonlyChildren;
            }
        }

        public SyntaxTreeNode(Symbol Symbol)
        {
            children = new List<SyntaxTreeNode>();
            symbol = Symbol;
            action = SyntaxTreeNodeAction.Nothing;
        }
        public SyntaxTreeNode(Symbol Symbol, SyntaxTreeNodeAction Action)
        {
            properties = new Dictionary<string, object>();
            children = new List<SyntaxTreeNode>();
            symbol = Symbol;
            action = Action;
            readonlyChildren = new System.Collections.ObjectModel.ReadOnlyCollection<SyntaxTreeNode>(children);
        }

        public void AppendChild(SyntaxTreeNode Node)
        {
            if (Node.parent != null)
                Node.parent.children.Remove(Node);
            Node.parent = this;
            children.Add(Node);
        }
        public void AppendChild(SyntaxTreeNode Node, SyntaxTreeNodeAction Action)
        {
            if (Node.parent != null)
                Node.parent.children.Remove(Node);
            Node.parent = this;
            Node.action = Action;
            children.Add(Node);
        }
        public void AppendRange(ICollection<SyntaxTreeNode> Nodes)
        {
            List<SyntaxTreeNode> Temp = new List<SyntaxTreeNode>(Nodes);
            foreach (SyntaxTreeNode Node in Temp)
                AppendChild(Node);
        }

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

        public System.Xml.XmlNode GetXMLNode(System.Xml.XmlDocument Doc)
        {
            System.Xml.XmlNode Node = Doc.CreateElement(symbol.Name);
            if (symbol is SymbolToken)
            {
                SymbolToken Token = (SymbolToken)symbol;
                Node.AppendChild(Doc.CreateTextNode(Token.ToString()));
            }
            foreach (string Property in properties.Keys)
            {
                System.Xml.XmlAttribute Attribute = Doc.CreateAttribute(Property);
                Attribute.Value = properties[Property].ToString();
                Node.Attributes.Append(Attribute);
            }
            foreach (SyntaxTreeNode Child in children)
                Node.AppendChild(Child.GetXMLNode(Doc));
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
