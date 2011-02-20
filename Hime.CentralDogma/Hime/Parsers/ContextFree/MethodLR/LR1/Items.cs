namespace Hime.Parsers.CF.LR
{
    /// <summary>
    /// Represents a LR(1) item
    /// </summary>
    public class ItemLR1 : Item
    {
        /// <summary>
        /// The values of the lookahead
        /// </summary>
        protected Terminal p_Lookahead;

        /// <summary>
        /// Get the lookahead value
        /// </summary>
        /// <value>The lookahead value</value>
        public Terminal Lookahead { get { return p_Lookahead; } }

        /// <summary>
        /// Construct the item from a rule, the dot position in the rule and the lookahead value
        /// </summary>
        /// <param name="Rule">The rule on which the item is based</param>
        /// <param name="DotPosition">The position of the dot in the rule</param>
        /// <param name="OnSymbol">The lookahead value</param>
        public ItemLR1(CFRule Rule, int DotPosition, Terminal Lookahead) : base(Rule, DotPosition) { p_Lookahead = Lookahead; }

        /// <summary>
        /// Get the child of the current item
        /// </summary>
        /// <returns>Returns the child item or null if the item's action is Reduce</returns>
        public override Item GetChild()
        {
            if (Action == ItemAction.Reduce) return null;
            return new ItemLR1(p_Rule, p_DotPosition + 1, p_Lookahead);
        }
        /// <summary>
        /// Compute the closure for this item and add it to given list
        /// </summary>
        /// <param name="Closure">The closure of items being computed</param>
        public override void CloseTo(System.Collections.Generic.List<Item> Closure)
        {
            // Get the next symbol in the item
            Symbol Next = NextSymbol;
            // No next symbol, the item was of the form [Var -> alpha .] (reduction)
            // => return
            if (Next == null) return;
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
                    // Add the item's lookahead as possible symbol for firsts
                    Firsts.Add(p_Lookahead);
                }
                // For each rule that has Next as a head variable :
                foreach (CFRule R in NextVar.Rules)
                {
                    // For each symbol in Firsts : create the child with this symbol as lookahead
                    foreach (Terminal First in Firsts)
                    {
                        // Child item creation and unique insertion
                        ItemLR1 New = new ItemLR1(R, 0, First);
                        bool Found = false;
                        foreach (Item Previous in Closure)
                        {
                            if (Previous.Equals(New))
                            {
                                Found = true;
                                break;
                            }
                        }
                        if (!Found) Closure.Add(New);
                    }
                }
            }
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
            if (obj is ItemLR1)
            {
                ItemLR1 Tested = (ItemLR1)obj;
                if (!Equals_Base(Tested)) return false;
                return (Tested.p_Lookahead.SID == p_Lookahead.SID);
            }
            return false;
        }
        public override int GetHashCode() { return base.GetHashCode(); }

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
            Builder.Append(" " + p_Arrow);
            int i = 0;
            foreach (RuleDefinitionPart Part in p_Definition.Parts)
            {
                if (i == p_DotPosition)
                    Builder.Append(" " + p_Dot);
                Builder.Append(" ");
                Builder.Append(Part.ToString());
                i++;
            }
            if (i == p_DotPosition)
                Builder.Append(" " + p_Dot);
            if (ShowDecoration)
            {
                Builder.Append(", ");
                Builder.Append(p_Lookahead.ToString());
            }
            Builder.Append("]");
            return Builder.ToString();
        }
    }
}