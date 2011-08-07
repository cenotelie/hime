using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class ItemLR0 : Item
    {
        internal static TerminalSet EmptySet = new TerminalSet();

        public ItemLR0(CFRule Rule, int DotPosition) : base(Rule, DotPosition) { }

        public override TerminalSet Lookaheads { get { return EmptySet; } }

        public override Item GetChild()
        {
            if (Action == ItemAction.Reduce) return null;
            return new ItemLR0(rule, dotPosition + 1);
        }
        public override void CloseTo(List<Item> closure, Dictionary<CFRule, Dictionary<int, List<Item>>> map)
        {
            Symbol next = NextSymbol;
            if (next == null)
                return;
            Variable nextVar = next as Variable;
            if (nextVar == null) return;
            foreach (CFRule rule in nextVar.Rules)
            {
                if (!map.ContainsKey(rule))
                    map.Add(rule, new Dictionary<int, List<Item>>());
                Dictionary<int, List<Item>> sub = map[rule];
                if (sub.ContainsKey(0))
                    continue;
                List<Item> list = new List<Item>();
                sub.Add(0, list);

                ItemLR0 New = new ItemLR0(rule, 0);
                closure.Add(New);
                list.Add(New);
            }
        }

        public override bool ItemEquals(Item item) { return Equals_Base(item); }
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
            Builder.Append("]");
            return Builder.ToString();
        }
    }
}
