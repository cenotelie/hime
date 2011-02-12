namespace Hime.Redist.Parsers
{

    public class SPPFNode
    {
        protected ISymbol p_Symbol;
        protected System.Collections.Generic.List<SPPFNodeFamily> p_Families;

        public ISymbol Symbol { get { return p_Symbol; } }
        public System.Collections.ObjectModel.ReadOnlyCollection<SPPFNodeFamily> Families { get { return new System.Collections.ObjectModel.ReadOnlyCollection<SPPFNodeFamily>(p_Families); } }

        public SPPFNode(ISymbol symbol)
        {
            p_Symbol = symbol;
            p_Families = new System.Collections.Generic.List<SPPFNodeFamily>();
        }
    }

    public class SPPFNodeFamily
    {
        protected SPPFNode p_Parent;
        protected System.Collections.Generic.List<SPPFNode> p_Children;

        protected SPPFNode Parent { get { return p_Parent; } }
        public System.Collections.ObjectModel.ReadOnlyCollection<SPPFNode> Children { get { return new System.Collections.ObjectModel.ReadOnlyCollection<SPPFNode>(p_Children); } }

        public SPPFNodeFamily(SPPFNode parent)
        {
            p_Parent = parent;
            p_Children = new System.Collections.Generic.List<SPPFNode>();
        }
    }
}