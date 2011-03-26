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
        protected System.Type p_MethodType;
        protected ConflictType p_Type;
        private Terminal p_Lookahead;
        private List<Item> p_Items;

        public Hime.Kernel.Reporting.Level Level {
            get
            {
                if (p_MethodType == typeof(MethodLR0)) return Hime.Kernel.Reporting.Level.Error;
                if (p_MethodType == typeof(MethodLR1)) return Hime.Kernel.Reporting.Level.Error;
                if (p_MethodType == typeof(MethodLALR1)) return Hime.Kernel.Reporting.Level.Error;
                return Hime.Kernel.Reporting.Level.Warning;
            }
        }
        public string Component { get { return p_MethodType.Name; } }
        public string Message { get { return ToString(); } }

        public System.Type MethodType { get { return p_MethodType; } }
        public ConflictType ConflictType { get { return p_Type; } }
        public Terminal ConflictSymbol { get { return p_Lookahead; } }

        public void AddItem(Item Item) { p_Items.Add(Item); }

        public Conflict(System.Type MethodType, ConflictType Type, Terminal Lookahead)
        {
            p_MethodType = MethodType;
            p_Type = Type;
            p_Lookahead = Lookahead;
            p_Items = new List<Item>();
        }
        public Conflict(System.Type MethodType, ConflictType Type)
        {
            p_MethodType = MethodType;
            p_Type = Type;
            p_Items = new List<Item>();
        }

        public bool ContainsItem(Item Item) { return p_Items.Contains(Item); }

        public override string ToString()
        {
            System.Text.StringBuilder Builder = new System.Text.StringBuilder("Conflict ");
            if (p_Type == ConflictType.ShiftReduce)
                Builder.Append("Shift/Reduce");
            else
                Builder.Append("Reduce/Reduce");
            if (p_Lookahead != null)
            {
                Builder.Append(" on terminal '");
                Builder.Append(p_Lookahead.ToString());
                Builder.Append("'");
            }
            Builder.Append(" for items {");
            foreach (Item Item in p_Items)
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
