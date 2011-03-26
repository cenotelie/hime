using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class ItemLR0 : Item
    {
        private static TerminalSet p_EmptySet = new TerminalSet();

        public ItemLR0(CFRule Rule, int DotPosition) : base(Rule, DotPosition) { }

        public override TerminalSet Lookaheads { get { return p_EmptySet; } }

        public override Item GetChild()
        {
            if (Action == ItemAction.Reduce) return null;
            return new ItemLR0(p_Rule, p_DotPosition + 1);
        }
        public override void CloseTo(List<Item> Closure)
        {
            Symbol Next = NextSymbol;
            if (Next == null)
                return;
            if (Next is CFVariable)
            {
                CFVariable Var = (CFVariable)Next;
                foreach (CFRule R in Var.Rules)
                {
                    ItemLR0 New = new ItemLR0(R, 0);
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

        public override bool Equals(object obj)
        {
            if (obj is ItemLR0)
            {
                ItemLR0 Tested = (ItemLR0)obj;
                return Equals_Base(Tested);
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
            Builder.Append("]");
            return Builder.ToString();
        }
    }
}
