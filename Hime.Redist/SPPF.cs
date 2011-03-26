using System.Collections.Generic;

namespace Hime.Redist.Parsers
{
    public class SPPFNode
    {
        protected Symbol symbol;
        protected SyntaxTreeNodeAction action;
        protected int generation;
        protected List<SPPFNodeFamily> families;

        public Symbol Symbol { get { return symbol; } }
        public SyntaxTreeNodeAction Action
        {
            get { return action; }
            set { action = value; }
        }
        public int Generation { get { return generation; } }
        public System.Collections.ObjectModel.ReadOnlyCollection<SPPFNodeFamily> Families { get { return new System.Collections.ObjectModel.ReadOnlyCollection<SPPFNodeFamily>(families); } }

        public SPPFNode(Symbol symbol, int gen)
        {
            this.symbol = symbol;
            this.action = SyntaxTreeNodeAction.Nothing;
            this.generation = gen;
            this.families = new List<SPPFNodeFamily>();
        }
        public SPPFNode(Symbol symbol, int gen, SyntaxTreeNodeAction action)
        {
            this.symbol = symbol;
            this.action = action;
            this.generation = gen;
            this.families = new List<SPPFNodeFamily>();
        }

        public void AddFamily(SPPFNodeFamily family) { families.Add(family); }
        public void AddFamily(List<SPPFNode> nodes) { families.Add(new SPPFNodeFamily(this, nodes)); }

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

        public bool HasEquivalentFamily(SPPFNodeFamily family)
        {
            foreach (SPPFNodeFamily potential in families)
                if (potential.EquivalentTo(family))
                    return true;
            return false;
        }

        public SyntaxTreeNode GetFirstTree()
        {
            SyntaxTreeNode me = new SyntaxTreeNode(symbol, action);
            if (families.Count == 1)
            {
                foreach (SPPFNode child in families[0].Children)
                {
                    if (child.Symbol is SymbolAction)
                        ((SymbolAction)child.Symbol).Action.Invoke(me);
                    else
                        me.AppendChild(child.GetFirstTree());
                }
            }
            else if (families.Count >= 1)
            {
                foreach (SPPFNodeFamily family in families)
                {
                    SyntaxTreeNode subroot = new SyntaxTreeNode(null, SyntaxTreeNodeAction.Nothing);
                    me.AppendChild(subroot);
                }
            }
            return me;
        }
    }

    public class SPPFNodeFamily
    {
        protected SPPFNode parent;
        protected List<SPPFNode> children;

        protected SPPFNode Parent { get { return parent; } }
        public System.Collections.ObjectModel.ReadOnlyCollection<SPPFNode> Children { get { return new System.Collections.ObjectModel.ReadOnlyCollection<SPPFNode>(children); } }

        public SPPFNodeFamily(SPPFNode parent)
        {
            this.parent = parent;
            this.children = new List<SPPFNode>();
        }
        public SPPFNodeFamily(SPPFNode parent, List<SPPFNode> nodes)
        {
            this.parent = parent;
            this.children = new List<SPPFNode>(nodes);
        }

        public void AddChild(SPPFNode child) { children.Add(child); }

        public bool EquivalentTo(SPPFNodeFamily family)
        {
            if (children.Count != family.children.Count)
                return false;
            for (int i = 0; i != children.Count; i++)
                if (!children[i].EquivalentTo(family.children[i]))
                    return false;
            return true;
        }
    }
}
