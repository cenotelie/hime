using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class ItemLR1 : Item
    {
        protected Terminal lookahead;
        protected TerminalSet lookaheads;

        public Terminal Lookahead { get { return lookahead; } }

        public override TerminalSet Lookaheads { get { return lookaheads; } }

        public ItemLR1(CFRule Rule, int DotPosition, Terminal Lookahead) : base(Rule, DotPosition)
        {
            lookahead = Lookahead;
            lookaheads = new TerminalSet();
            lookaheads.Add(Lookahead);
        }

        public override Item GetChild()
        {
            if (Action == ItemAction.Reduce) return null;
            return new ItemLR1(rule, dotPosition + 1, lookahead);
        }
        public override void CloseTo(List<Item> Closure)
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
                    Firsts.Add(lookahead);
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
                            if (New.ItemEquals(Previous))
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

        public override bool ItemEquals(Item item)
        {
            ItemLR1 tested = (ItemLR1)item;
            if (tested.lookahead.SID != lookahead.SID)
                return false;
            return Equals_Base(tested);
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
                Builder.Append(lookahead.ToString());
            }
            Builder.Append("]");
            return Builder.ToString();
        }
    }
}
