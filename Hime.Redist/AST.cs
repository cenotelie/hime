﻿namespace Hime.Redist.Parsers
{
    public enum SyntaxTreeNodeAction
    {
        Promote,
        Drop,
        Replace,
        Nothing
    }

    public class SyntaxTreeNode
    {
        protected System.Collections.Generic.Dictionary<string, object> p_Properties;
        protected SyntaxTreeNodeCollection p_Children;
        protected System.Collections.ObjectModel.ReadOnlyCollection<SyntaxTreeNode> p_ReadOnlyChildren;
        protected SyntaxTreeNode p_Parent;
        protected ISymbol p_Symbol;
        protected SyntaxTreeNodeAction p_Action;

        public System.Collections.Generic.Dictionary<string, object> Properties { get { return p_Properties; } }
        public ISymbol Symbol { get { return p_Symbol; } }
        public SyntaxTreeNode Parent { get { return p_Parent; } }
        public System.Collections.ObjectModel.ReadOnlyCollection<SyntaxTreeNode> Children { get { return p_ReadOnlyChildren; } }

        public SyntaxTreeNode(ISymbol Symbol)
        {
            p_Properties = new System.Collections.Generic.Dictionary<string, object>();
            p_Children = new SyntaxTreeNodeCollection();
            p_ReadOnlyChildren = new System.Collections.ObjectModel.ReadOnlyCollection<SyntaxTreeNode>(p_Children);
            p_Symbol = Symbol;
            p_Action = SyntaxTreeNodeAction.Nothing;
        }
        public SyntaxTreeNode(ISymbol Symbol, SyntaxTreeNodeAction Action)
        {
            p_Properties = new System.Collections.Generic.Dictionary<string, object>();
            p_Children = new SyntaxTreeNodeCollection();
            p_ReadOnlyChildren = new System.Collections.ObjectModel.ReadOnlyCollection<SyntaxTreeNode>(p_Children);
            p_Symbol = Symbol;
            p_Action = Action;
        }

        public void AppendChild(SyntaxTreeNode Node)
        {
            if (Node.p_Parent != null)
                Node.p_Parent.p_Children.Remove(Node);
            Node.p_Parent = this;
            p_Children.Add(Node);
        }
        public void AppendChild(SyntaxTreeNode Node, SyntaxTreeNodeAction Action)
        {
            if (Node.p_Parent != null)
                Node.p_Parent.p_Children.Remove(Node);
            Node.p_Parent = this;
            Node.p_Action = Action;
            p_Children.Add(Node);
        }
        public void AppendRange(System.Collections.Generic.IEnumerable<SyntaxTreeNode> Nodes)
        {
            System.Collections.Generic.List<SyntaxTreeNode> Temp = new System.Collections.Generic.List<SyntaxTreeNode>(Nodes);
            foreach (SyntaxTreeNode Node in Temp)
                AppendChild(Node);
        }

        public SyntaxTreeNode ApplyActions()
        {
            ApplyActions_DropReplace();
            return ApplyActions_Promote();
        }

        private void ApplyActions_DropReplace()
        {
            for (int i = 0; i != p_Children.Count; i++)
            {
                if (p_Children[i].p_Action == SyntaxTreeNodeAction.Drop)
                {
                    p_Children.RemoveAt(i);
                    i--;
                    continue;
                }
                if (p_Children[i].p_Symbol is SymbolTokenText)
                {
                    SymbolTokenText TokenText = (SymbolTokenText)p_Children[i].p_Symbol;
                    if (TokenText.SubGrammarRoot != null)
                        p_Children[i] = TokenText.SubGrammarRoot;
                }

                p_Children[i].ApplyActions_DropReplace();

                if (p_Children[i].p_Action == SyntaxTreeNodeAction.Replace)
                {
                    SyntaxTreeNodeCollection NewChildren = p_Children[i].p_Children;
                    foreach (SyntaxTreeNode Child in NewChildren)
                        Child.p_Parent = this;
                    p_Children.RemoveAt(i);
                    p_Children.InsertRange(i, NewChildren);
                    i += NewChildren.Count - 1;
                }
            }
        }

        private SyntaxTreeNode ApplyActions_Promote()
        {
            SyntaxTreeNode NewRoot = null;

            for (int i = 0; i != p_Children.Count; i++)
                p_Children[i] = p_Children[i].ApplyActions_Promote();

            for (int i = 0; i != p_Children.Count; i++)
            {
                if (p_Children[i].p_Action == SyntaxTreeNodeAction.Promote)
                {
                    if (NewRoot == null)
                    {
                        NewRoot = p_Children[i];
                        NewRoot.p_Children.InsertRange(0, p_Children.GetRange(0, i));
                        if (i != p_Children.Count - 1)
                            NewRoot.p_Children.AddRange(p_Children.GetRange(i + 1, p_Children.Count - i - 1));
                    }
                    else
                    {
                        int CountOnRight = p_Children.Count - i - 1;
                        int Index = NewRoot.p_Children.Count - CountOnRight - 1;
                        p_Children[i].p_Children.Insert(0, NewRoot);
                        p_Children[i].p_Children.AddRange(NewRoot.p_Children.GetRange(Index + 1, CountOnRight));
                        NewRoot.p_Children.RemoveRange(Index, CountOnRight + 1);
                        NewRoot = p_Children[i];
                    }
                    // Relink
                    foreach (SyntaxTreeNode Child in NewRoot.p_Children)
                        Child.p_Parent = NewRoot;
                    NewRoot.p_Action = SyntaxTreeNodeAction.Nothing;
                }
            }

            if (NewRoot == null)
                return this;
            NewRoot.p_Action = this.p_Action;
            return NewRoot;
        }

        public System.Xml.XmlNode GetXMLNode(System.Xml.XmlDocument Doc)
        {
            System.Xml.XmlNode Node = Doc.CreateElement(p_Symbol.Name);
            if (p_Symbol is SymbolToken)
            {
                SymbolToken Token = (SymbolToken)p_Symbol;
                Node.AppendChild(Doc.CreateTextNode(Token.ToString()));
            }
            foreach (string Property in p_Properties.Keys)
            {
                System.Xml.XmlAttribute Attribute = Doc.CreateAttribute(Property);
                Attribute.Value = p_Properties[Property].ToString();
                Node.Attributes.Append(Attribute);
            }
            foreach (SyntaxTreeNode Child in p_Children)
                Node.AppendChild(Child.GetXMLNode(Doc));
            return Node;
        }
    }

    public class SyntaxTreeNodeCollection : System.Collections.Generic.List<SyntaxTreeNode>
    {
        public SyntaxTreeNodeCollection() : base() { }
        public SyntaxTreeNodeCollection(System.Collections.Generic.IEnumerable<SyntaxTreeNode> collection) : base(collection) { }
    }
}