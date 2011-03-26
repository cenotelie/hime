using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    enum ItemAction
    {
        Shift,
        Reduce
    }



    abstract class Item
    {
        protected const string p_Arrow = "→";
        protected const string p_Dot = "•";

        protected CFRule p_Rule;
        protected CFRuleDefinition p_Definition;
        protected int p_DotPosition;

        public CFRule BaseRule { get { return p_Rule; } }
        public int DotPosition { get { return p_DotPosition; } }
        public ItemAction Action
        {
            get
            {
                if (p_DotPosition != p_Definition.Length)
                    return ItemAction.Shift;
                return ItemAction.Reduce;
            }
        }
        
        public Symbol NextSymbol { get { return p_Definition.GetSymbolAtIndex(p_DotPosition); } }
        public CFRuleDefinition NextChoice { get { return p_Rule.Definition.GetChoiceAtIndex(p_DotPosition + 1); } }

        public abstract TerminalSet Lookaheads { get; }

        public Item(CFRule Rule, int DotPosition)
        {
            p_Rule = Rule;
            p_Definition = p_Rule.Definition.GetChoiceAtIndex(0);
            p_DotPosition = DotPosition;
        }

        public bool Equals_Base(Item Item)
        {
            if (p_Rule != Item.p_Rule)
                return false;
            return (p_DotPosition == Item.p_DotPosition);
        }

        public abstract Item GetChild();
        public abstract void CloseTo(List<Item> Closure);

        public abstract override bool Equals(object obj);
        public override int GetHashCode() { return base.GetHashCode(); }
        public abstract override string ToString();
        public abstract string ToString(bool ShowDecoration);
    }
}
