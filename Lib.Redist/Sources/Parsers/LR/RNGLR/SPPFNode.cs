namespace Hime.Redist.Parsers
{
    class SPPFNode
    {
        private const int maxChildrenCount = 128;

        public int originalSID;
        public ASTNode value;
        public LRTreeAction action;
        public SPPFNode[] children;
        public int count;
        
        public SPPFNode()
        {
            this.children = new SPPFNode[maxChildrenCount];
        }

        public SPPFNode(Symbols.Symbol symbol, LRTreeAction action)
        {
            this.originalSID = symbol.SymbolID;
            this.value = new ASTNode();
            this.action = action;
        }

        public void Init(Symbols.Symbol symbol, LRTreeAction action)
        {
            this.originalSID = symbol.SymbolID;
            this.value = new ASTNode();
            this.action = action;
        }
    }
}
