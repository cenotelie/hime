/**********************************************************************
* Copyright (c) 2013 Laurent Wouters and others
* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU Lesser General Public License as
* published by the Free Software Foundation, either version 3
* of the License, or (at your option) any later version.
* 
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU Lesser General Public License for more details.
* 
* You should have received a copy of the GNU Lesser General
* Public License along with this program.
* If not, see <http://www.gnu.org/licenses/>.
* 
* Contributors:
*     Laurent Wouters - lwouters@xowl.org
**********************************************************************/

using System.Collections.Generic;

namespace Hime.CentralDogma.Grammars.ContextFree.LR
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
        public override void CloseTo(List<Item> closure, Dictionary<CFRule, Dictionary<int, List<Item>>> map)
        {
            // Get the next symbol in the item
            Symbol next = NextSymbol;
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
            if (firsts.Contains(Epsilon.Instance))
            {
                // Remove ε
                firsts.Remove(Epsilon.Instance);
                // Add the item's lookaheads
                firsts.AddRange(lookaheads);
            }
            // For each rule that has Next as a head variable :
            foreach (CFRule rule in nextVar.Rules)
            {
                if (!map.ContainsKey(rule))
                    map.Add(rule, new Dictionary<int, List<Item>>());
                Dictionary<int, List<Item>> sub = map[rule];
                if (sub.ContainsKey(0))
                {
                    List<Item> previouses = sub[0];
                    ItemLALR1 previous = previouses[0] as ItemLALR1;
                    previous.Lookaheads.AddRange(firsts);
                }
                else
                {
                    List<Item> items = new List<Item>();
                    sub.Add(0, items);
                    ItemLALR1 New = new ItemLALR1(rule, 0, firsts);
                    closure.Add(New);
                    items.Add(New);
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
