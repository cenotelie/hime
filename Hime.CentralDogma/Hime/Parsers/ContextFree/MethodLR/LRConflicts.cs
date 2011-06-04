using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    enum ConflictType
    {
        ShiftReduce,
        ReduceReduce,
        None
    }

    class Conflict : Hime.Kernel.Reporting.Entry
    {
        protected string component;
        protected State state;
        protected ConflictType type;
        private Terminal lookahead;
        private List<Item> items;

        public Hime.Kernel.Reporting.Level Level {
            get
            {
                if (component == "MethodLR0") return Hime.Kernel.Reporting.Level.Error;
                if (component == "MethodLR1") return Hime.Kernel.Reporting.Level.Error;
                if (component == "MethodLALR1") return Hime.Kernel.Reporting.Level.Error;
                return Hime.Kernel.Reporting.Level.Warning;
            }
        }
        public string Component { get { return component; } }
        public State State { get { return state; } }
        public string Message { get { return ToString(); } }

        public ConflictType ConflictType { get { return type; } }
        public Terminal ConflictSymbol { get { return lookahead; } }
        public ICollection<Item> Items { get { return items; } }

        public Conflict(string component, State state, ConflictType type, Terminal lookahead)
        {
            this.component = component;
            this.state = state;
            this.type = type;
            this.lookahead = lookahead;
            items = new List<Item>();
        }
        public Conflict(string component, State state, ConflictType type)
        {
            this.component = component;
            this.state = state;
            this.type = type;
            items = new List<Item>();
        }

        public void AddItem(Item Item) { items.Add(Item); }
        public bool ContainsItem(Item Item) { return items.Contains(Item); }

        public override string ToString()
        {
            System.Text.StringBuilder Builder = new System.Text.StringBuilder("Conflict ");
            if (type == ConflictType.ShiftReduce)
                Builder.Append("Shift/Reduce");
            else
                Builder.Append("Reduce/Reduce");
            Builder.Append(" in ");
            Builder.Append(state.ID.ToString("X"));
            if (lookahead != null)
            {
                Builder.Append(" on terminal '");
                Builder.Append(lookahead.ToString());
                Builder.Append("'");
            }
            Builder.Append(" for items {");
            foreach (Item Item in items)
            {
                Builder.Append(" ");
                Builder.Append(Item.ToString());
                Builder.Append(" ");
            }
            Builder.Append("}");
            return Builder.ToString();
        }
    }
}
