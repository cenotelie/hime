using System;
using System.Collections.Generic;

namespace Hime.Redist.Parsers
{
	/// <summary>
	/// Represents a node and its children families in a Shared-Packed Parse Forest
	/// </summary>
    class SPPF
    {
        private int originalSID;
        private ParseTree.Cell head;
        private TreeAction headAction;
        private List<ParseTree.Cell> children;
        private List<TreeAction> actions;

        /// <summary>
        /// Gets the original ID of this SPPF's node symbol
        /// </summary>
        public int OriginalSID { get { return originalSID; } }

        public SPPF(Symbols.Symbol symbol, TreeAction action)
        {
            this.originalSID = symbol.SymbolID;
            this.head.symbol = symbol;
            this.headAction = action;
            this.children = new List<ParseTree.Cell>();
            this.actions = new List<TreeAction>();
        }

        public void SetHeadAction(TreeAction action)
        {
            if (action != TreeAction.None)
                this.headAction = action;
        }
    }
}
