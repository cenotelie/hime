using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class ItemLALR1 : Item
    {
        protected TerminalSet lookaheads;

        public override TerminalSet Lookaheads { get { return lookaheads; } }

        public ItemLALR1(CFRule Rule, int DotPosition, TerminalSet Lookaheads) : base(Rule, DotPosition) { lookaheads = new TerminalSet(Lookaheads); }
        public ItemLALR1(Item Copied) : base(Copied.BaseRule, Copied.DotPosition) { lookaheads = new TerminalSet(); }

        public override Item GetChild()
        {
            if (Action == ItemAction.Reduce) return null;
            return new ItemLALR1(rule, dotPosition + 1, new TerminalSet(lookaheads));
        }
        public override void CloseTo(List<Item> Closure)
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
                    Firsts.AddRange(lookaheads);
                }
                // For each rule that has Next as a head variable :
                foreach (CFRule Rule in NextVar.Rules)
                {
                    ItemLALR1 New = new ItemLALR1(Rule, 0, Firsts);
                    // Tell if a previous item was found with same rule and dot position
                    bool FoundPrevious = false;
                    // For all items in Closure that are equal with the new child in the LR(0) way :
                    foreach (ItemLALR1 Previous in Closure)
                    {
                        if (New.Equals_Base(Previous))
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

        protected bool Equals_Lookaheads(ItemLALR1 Item)
        {
            if (lookaheads.Count != Item.lookaheads.Count)
                return false;
            foreach (Terminal Terminal in lookaheads)
            {
                if (!Item.lookaheads.Contains(Terminal))
                    return false;
            }
            return true;
        }
        public override bool ItemEquals(Item item)
        {
            ItemLALR1 Tested = (ItemLALR1)item;
            if (!Equals_Base(Tested)) return false;
            return Equals_Lookaheads(Tested);
        }
        public override int GetHashCode() { return base.GetHashCode(); }

        public override string ToString() { return ToString(false); }
        public override string ToString(bool ShowDecoration)
        {
            System.Text.StringBuilder Builder = new System.Text.StringBuilder("[");
            Builder.Append(rule.Variable.ToString());
            Builder.Append(" " + CFRule.arrow);
            int i = 0;
            foreach (RuleDefinitionPart Part in definition.Parts)
            {
                if (i == dotPosition)
                    Builder.Append(" " + dot);
                Builder.Append(" ");
                Builder.Append(Part.ToString());
                i++;
            }
            if (i == dotPosition)
                Builder.Append(" " + dot);
            if (ShowDecoration)
            {
                Builder.Append(", ");
                for (int j = 0; j != lookaheads.Count; j++)
                {
                    if (j != 0) Builder.Append("/");
                    Builder.Append(lookaheads[j].ToString());
                }
            }
            Builder.Append("]");
            return Builder.ToString();
        }
    }
}
