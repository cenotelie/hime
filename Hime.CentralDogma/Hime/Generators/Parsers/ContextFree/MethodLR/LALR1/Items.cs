namespace Hime.Generators.Parsers.ContextFree.LR
{
    /// <summary>
    /// Represents a LALR(1) item
    /// </summary>
    public class LRItemLALR1 : LRItem
    {
        /// <summary>
        /// The values of the lookahead
        /// </summary>
        protected TerminalSet p_Lookaheads;

        /// <summary>
        /// Get the lookahead value
        /// </summary>
        /// <value>The lookahead value</value>
        public TerminalSet Lookaheads { get { return p_Lookaheads; } }

        /// <summary>
        /// Construct the item from a rule, the dot position in the rule and the lookahead value
        /// </summary>
        /// <param name="Rule">The rule on which the item is based</param>
        /// <param name="DotPosition">The position of the dot in the rule</param>
        /// <param name="Lookaheads">The lookaheads values</param>
        public LRItemLALR1(CFRule Rule, int DotPosition, TerminalSet Lookaheads) : base(Rule, DotPosition) { p_Lookaheads = new TerminalSet(Lookaheads); }
        /// <summary>
        /// Constructs the item from another item
        /// </summary>
        /// <param name="Copied">The copied item</param>
        /// <remarks>The lookaheads are left empty</remarks>
        public LRItemLALR1(LRItem Copied) : base(Copied.BaseRule, Copied.DotPosition) { p_Lookaheads = new TerminalSet(); }

        /// <summary>
        /// Get the child of the current item
        /// </summary>
        /// <returns>Returns the child item or null if the item's action is Reduce</returns>
        public override LRItem GetChild()
        {
            if (Action == LRItemAction.Reduce) return null;
            return new LRItemLALR1(p_Rule, p_DotPosition + 1, new TerminalSet(p_Lookaheads));
        }
        /// <summary>
        /// Compute the closure for this item and add it to given list
        /// </summary>
        /// <param name="Closure">The closure of items being computed</param>
        public override void CloseTo(System.Collections.Generic.List<LRItem> Closure)
        {
            // Get the next symbol in the item
            Symbol Next = NextSymbol;
            // No next symbol, the item was of the form [Var -> alpha .] (reduction)
            // => return
            if (Next == null)
                return;
            // Here the item is of the form [Var -> alpha . Next beta]
            // If the next symbol is not a variable : do nothing
            // If the next symbol is a variable :
            if (Next is CFVariable)
            {
                CFVariable NextVar = (CFVariable)Next;
                // Firsts is a copy of the Firsts set for beta (next choice)
                // Firsts will contains symbols that may follow Next
                // Firsts will therefore be the lookahead for child items
                TerminalSet Firsts = new TerminalSet(NextChoice.Firsts);
                // If beta is nullifiable (contains ε) :
                if (Firsts.Contains(TerminalEpsilon.Instance))
                {
                    // Remove ε
                    Firsts.Remove(TerminalEpsilon.Instance);
                    // Add the item's lookaheads
                    Firsts.AddRange(p_Lookaheads);
                }
                // For each rule that has Next as a head variable :
                foreach (CFRule Rule in NextVar.Rules)
                {
                    LRItemLALR1 New = new LRItemLALR1(Rule, 0, Firsts);
                    // Tell if a previous item was found with same rule and dot position
                    bool FoundPrevious = false;
                    // For all items in Closure that are equal with the new child in the LR(0) way :
                    foreach (LRItemLALR1 Previous in Closure)
                    {
                        if (Previous.Equals_Base(New))
                        {
                            // Same item => Add new lookaheads
                            Previous.Lookaheads.AddRange(Firsts);
                            FoundPrevious = true;
                            break;
                        }
                    }
                    // If no previous was found => add new item
                    if (!FoundPrevious)
                        Closure.Add(New);
                }
            }
        }

        /// <summary>
        /// Test if lookaheads are the same
        /// </summary>
        /// <param name="Item">The item to test</param>
        /// <returns>Returns true if the lookaheads are equals, false otherwise</returns>
        protected bool Equals_Lookaheads(LRItemLALR1 Item)
        {
            if (p_Lookaheads.Count != Item.p_Lookaheads.Count)
                return false;
            foreach (Terminal Terminal in p_Lookaheads)
            {
                if (!Item.p_Lookaheads.Contains(Terminal))
                    return false;
            }
            return true;
        }
        /// <summary>
        /// Test if two objects are equals in a general meaning
        /// </summary>
        /// <param name="obj">The object to test</param>
        /// <returns>
        /// If obj is not of the same type as this, returns false;
        /// If obj is of the same type as this, returns true if the two items are equivalent, false otherwise
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is LRItemLALR1)
            {
                LRItemLALR1 Tested = (LRItemLALR1)obj;
                if (!Equals_Base(Tested)) return false;
                return Equals_Lookaheads(Tested);
            }
            return false;
        }

        /// <summary>
        /// Returns a string representing the item
        /// </summary>
        /// <returns>A string represeting the item</returns>
        public override string ToString() { return ToString(false); }
        /// <summary>
        /// Returns a string representing the item with decoration
        /// </summary>
        /// <param name="ShowDecoration">True to show the decoration (lookaheads and watermarks)</param>
        /// <returns>A string represeting the item</returns>
        public override string ToString(bool ShowDecoration)
        {
            System.Text.StringBuilder Builder = new System.Text.StringBuilder("[");
            Builder.Append(p_Rule.Variable.ToString());
            Builder.Append(" ->");
            int i = 0;
            foreach (RuleDefinitionPart Part in p_Definition.Parts)
            {
                if (i == p_DotPosition)
                    Builder.Append(" .");
                Builder.Append(" ");
                Builder.Append(Part.ToString());
                i++;
            }
            if (i == p_DotPosition)
                Builder.Append(" . ");
            if (ShowDecoration)
            {
                Builder.Append(", ");
                for (int j = 0; j != p_Lookaheads.Count; j++)
                {
                    if (j != 0) Builder.Append("/");
                    Builder.Append(p_Lookaheads[j].ToString());
                }
            }
            Builder.Append("]");
            return Builder.ToString();
        }
    }
}