using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class ItemLR1 : Item
    {
        protected Terminal p_Lookahead;
        protected TerminalSet p_Lookaheads;

        public Terminal Lookahead { get { return p_Lookahead; } }

        public override TerminalSet Lookaheads { get { return p_Lookaheads; } }

        public ItemLR1(CFRule Rule, int DotPosition, Terminal Lookahead) : base(Rule, DotPosition)
        {
            p_Lookahead = Lookahead;
            p_Lookaheads = new TerminalSet();
            p_Lookaheads.Add(Lookahead);
        }

        public override Item GetChild()
        {
            if (Action == ItemAction.Reduce) return null;
            return new ItemLR1(p_Rule, p_DotPosition + 1, p_Lookahead);
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
                Builder.Append(p_Lookahead.ToString());
            }
            Builder.Append("]");
            return Builder.ToString();
        }
    }
}
