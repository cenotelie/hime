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

        public Dictionary<string, object> Properties { get { return properties; } }
        public Symbol Symbol { get { return symbol; } }
        public SyntaxTreeNodeAction Action { get { return action; } }
        public SyntaxTreeNode Parent { get { return parent; } }
        public System.Collections.ObjectModel.ReadOnlyCollection<SyntaxTreeNode> Children { get { return new System.Collections.ObjectModel.ReadOnlyCollection<SyntaxTreeNode>(children); } }

        public SyntaxTreeNode(Symbol Symbol)
        {
            properties = new Dictionary<string, object>();
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
            ApplyActions_DropReplace();
            return ApplyActions_Promote();
        }

        private void ApplyActions_DropReplace()
        {
            for (int i = 0; i != children.Count; i++)
            {
                if (children[i].action == SyntaxTreeNodeAction.Drop)
                {
                    children.RemoveAt(i);
                    i--;
                    continue;
                }
                if (children[i].symbol is SymbolTokenText)
                {
                    SymbolTokenText TokenText = (SymbolTokenText)children[i].symbol;
                    if (TokenText.SubGrammarRoot != null)
                        children[i] = TokenText.SubGrammarRoot;
                }

                children[i].ApplyActions_DropReplace();

                if (children[i].action == SyntaxTreeNodeAction.Replace)
                {
                    List<SyntaxTreeNode> NewChildren = children[i].children;
                    foreach (SyntaxTreeNode Child in NewChildren)
                        Child.parent = this;
                    children.RemoveAt(i);
                    children.InsertRange(i, NewChildren);
                    i += NewChildren.Count - 1;
                }
            }
        }

        private SyntaxTreeNode ApplyActions_Promote()
        {
            SyntaxTreeNode NewRoot = null;

            for (int i = 0; i != children.Count; i++)
                children[i] = children[i].ApplyActions_Promote();

            for (int i = 0; i != children.Count; i++)
            {
                if (children[i].action == SyntaxTreeNodeAction.Promote)
                {
                    if (NewRoot == null)
                    {
                        NewRoot = children[i];
                        NewRoot.children.InsertRange(0, children.GetRange(0, i));
                        if (i != children.Count - 1)
                            NewRoot.children.AddRange(children.GetRange(i + 1, children.Count - i - 1));
                    }
                    else
                    {
                        int CountOnRight = children.Count - i - 1;
                        int Index = NewRoot.children.Count - CountOnRight - 1;
                        children[i].children.Insert(0, NewRoot);
                        children[i].children.AddRange(NewRoot.children.GetRange(Index + 1, CountOnRight));
                        NewRoot.children.RemoveRange(Index, CountOnRight + 1);
                        NewRoot = children[i];
                    }
                    // Relink
                    foreach (SyntaxTreeNode Child in NewRoot.children)
                        Child.parent = NewRoot;
                    NewRoot.action = SyntaxTreeNodeAction.Nothing;
                }
            }

            if (NewRoot == null)
                return this;
            NewRoot.action = this.action;
            return NewRoot;
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
