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
        protected System.Type methodType;
        protected ConflictType type;
        private Terminal lookahead;
        private List<Item> items;

        public Hime.Kernel.Reporting.Level Level {
            get
            {
                if (methodType == typeof(MethodLR0)) return Hime.Kernel.Reporting.Level.Error;
                if (methodType == typeof(MethodLR1)) return Hime.Kernel.Reporting.Level.Error;
                if (methodType == typeof(MethodLALR1)) return Hime.Kernel.Reporting.Level.Error;
                return Hime.Kernel.Reporting.Level.Warning;
            }
        }
        public string Component { get { return methodType.Name; } }
        public string Message { get { return ToString(); } }

        public System.Type MethodType { get { return methodType; } }
        public ConflictType ConflictType { get { return type; } }
        public Terminal ConflictSymbol { get { return lookahead; } }

        public void AddItem(Item Item) { items.Add(Item); }

        public Conflict(System.Type MethodType, ConflictType Type, Terminal Lookahead)
        {
            methodType = MethodType;
            type = Type;
            lookahead = Lookahead;
            items = new List<Item>();
        }
        public Conflict(System.Type MethodType, ConflictType Type)
        {
            methodType = MethodType;
            type = Type;
            items = new List<Item>();
        }

        public bool ContainsItem(Item Item) { return items.Contains(Item); }

        public override string ToString()
        {
            System.Text.StringBuilder Builder = new System.Text.StringBuilder("Conflict ");
            if (type == ConflictType.ShiftReduce)
                Builder.Append("Shift/Reduce");
            else
                Builder.Append("Reduce/Reduce");
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
