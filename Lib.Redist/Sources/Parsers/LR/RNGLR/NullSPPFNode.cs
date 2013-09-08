using System.Collections.Generic;

namespace Hime.Redist.Parsers
{
    class NullSPPFNode
    {
        private Symbols.Symbol headSymbol;
        private TreeAction headAction;
        private NullSPPFNode[] children;
        private TreeAction[] actions;

        public NullSPPFNode(Symbols.Symbol symbol, TreeAction action)
        {
            this.headSymbol = symbol;
            this.headAction = action;
        }
    }
}
