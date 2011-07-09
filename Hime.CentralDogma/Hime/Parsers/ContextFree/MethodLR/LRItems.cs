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
        public const string dot = "•";

        protected CFRule rule;
        protected CFRuleDefinition definition;
        protected int dotPosition;

        public CFRule BaseRule { get { return rule; } }
        public int DotPosition { get { return dotPosition; } }
        public ItemAction Action
        {
            get
            {
                if (dotPosition != definition.Length)
                    return ItemAction.Shift;
                return ItemAction.Reduce;
            }
        }
        
        public Symbol NextSymbol { get { return definition.GetSymbolAtIndex(dotPosition); } }
        public CFRuleDefinition NextChoice { get { return rule.Definition.GetChoiceAtIndex(dotPosition + 1); } }

        public abstract TerminalSet Lookaheads { get; }

        public Item(CFRule Rule, int DotPosition)
        {
            rule = Rule;
            definition = rule.Definition.GetChoiceAtIndex(0);
            dotPosition = DotPosition;
        }

        public bool Equals_Base(Item Item)
        {
            if (rule != Item.rule)
                return false;
            return (dotPosition == Item.dotPosition);
        }
        public abstract bool ItemEquals(Item item);

        public abstract Item GetChild();
        public abstract void CloseTo(List<Item> closure, Dictionary<CFRule, Dictionary<int, List<Item>>> map);

        public override bool Equals(object obj) { return ItemEquals(obj as Item); }
        public override int GetHashCode() { return base.GetHashCode(); }
        public abstract override string ToString();
        public abstract string ToString(bool ShowDecoration);
    }
}
