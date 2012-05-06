/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;

namespace Hime.Parsers.ContextFree.LR
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
        public override void CloseTo(List<Item> closure, Dictionary<CFRule, Dictionary<int, List<Item>>> map)
        {
            // Get the next symbol in the item
            GrammarSymbol next = NextSymbol;
            // No next symbol, the item was of the form [Var -> alpha .] (reduction)
            // => return
            if (next == null) return;
            // Here the item is of the form [Var -> alpha . Next beta]
            // If the next symbol is not a variable : do nothing
            // If the next symbol is a variable :
            Variable nextVar = next as Variable;
            if (nextVar == null) return;
            // Firsts is a copy of the Firsts set for beta (next choice)
            // Firsts will contains symbols that may follow Next
            // Firsts will therefore be the lookahead for child items
            TerminalSet firsts = new TerminalSet(NextChoice.Firsts);
            // If beta is nullifiable (contains ε) :
            if (firsts.Contains(TerminalEpsilon.Instance))
            {
                // Remove ε
                firsts.Remove(TerminalEpsilon.Instance);
                // Add the item's lookahead as possible symbol for firsts
                firsts.Add(lookahead);
            }
            // For each rule that has Next as a head variable :
            foreach (CFRule rule in nextVar.Rules)
            {
                if (!map.ContainsKey(rule))
                    map.Add(rule, new Dictionary<int, List<Item>>());
                Dictionary<int, List<Item>> sub = map[rule];
                if (!sub.ContainsKey(0))
                    sub.Add(0, new List<Item>());
                List<Item> previouses = sub[0];
                // For each symbol in Firsts : create the child with this symbol as lookahead
                foreach (Terminal first in firsts)
                {
                    // Child item creation and unique insertion
                    ushort sid = first.SID;
                    bool found = false;
                    foreach (Item previous in previouses)
                    {
                        if (previous.Lookaheads[0].SID == sid)
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        ItemLR1 New = new ItemLR1(rule, 0, first);
                        closure.Add(New);
                        previouses.Add(New);
                    }
                }
            }
        }

        public override bool ItemEquals(Item item)
        {
            ItemLR1 tested = item as ItemLR1;
            if (this.lookahead.SID != tested.lookahead.SID)
                return false;
            return Equals_Base(tested);
        }
		
        public override string ToString() { return ToString(false); }
        public override string ToString(bool ShowDecoration)
        {
            System.Text.StringBuilder Builder = new System.Text.StringBuilder("[");
            Builder.Append(rule.Head.ToString());
            Builder.Append(" " + CFRule.arrow);
            int i = 0;
            foreach (RuleBodyElement Part in definition.Parts)
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
