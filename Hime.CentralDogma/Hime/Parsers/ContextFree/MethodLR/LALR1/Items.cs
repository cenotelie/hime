using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class ItemLALR1 : Item
    {
        protected TerminalSet p_Lookaheads;

        public override TerminalSet Lookaheads { get { return p_Lookaheads; } }

        public ItemLALR1(CFRule Rule, int DotPosition, TerminalSet Lookaheads) : base(Rule, DotPosition) { p_Lookaheads = new TerminalSet(Lookaheads); }
        public ItemLALR1(Item Copied) : base(Copied.BaseRule, Copied.DotPosition) { p_Lookaheads = new TerminalSet(); }

        public override Item GetChild()
        {
            if (Action == ItemAction.Reduce) return null;
            return new ItemLALR1(p_Rule, p_DotPosition + 1, new TerminalSet(p_Lookaheads));
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
                    Firsts.AddRange(p_Lookaheads);
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

        protected bool Equals_Lookaheads(ItemLALR1 Item)
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
        public override bool Equals(object obj)
        {
            if (obj is ItemLALR1)
            {
                ItemLALR1 Tested = (ItemLALR1)obj;
                if (!Equals_Base(Tested)) return false;
                return Equals_Lookaheads(Tested);
            }
            return false;
        }
        public override int GetHashCode() { return base.GetHashCode(); }

        public override string ToString() { return ToString(false); }
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
