namespace Hime.Parsers.CF.LR
{
    /// <summary>
    /// LR conflict type
    /// </summary>
    public enum ConflictType
    {
        /// <summary>
        /// Shift/Reduce conflict
        /// </summary>
        ShiftReduce,
        /// <summary>
        /// Reduce/Reduce conflict
        /// </summary>
        ReduceReduce
    }

    /// <summary>
    /// Represents a conflict in a LR set graph
    /// </summary>
    public class Conflict : Hime.Kernel.Reporting.Entry
    {
        /// <summary>
        /// Parse method used
        /// </summary>
        protected System.Type p_MethodType;
        /// <summary>
        /// Conflict type
        /// </summary>
        protected ConflictType p_Type;
        /// <summary>
        /// The conflictuous lookahead
        /// </summary>
        private Terminal p_Lookahead;
        /// <summary>
        /// The conflictuous items
        /// </summary>
        private System.Collections.Generic.List<Item> p_Items;

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

        /// <summary>
        /// Get the used parse method
        /// </summary>
        /// <value>The method</value>
        public System.Type MethodType { get { return p_MethodType; } }
        /// <summary>
        /// Get the conflict type
        /// </summary>
        /// <value>The conflict type</value>
        public ConflictType ConflictType { get { return p_Type; } }
        /// <summary>
        /// Get the conflictuous symbol
        /// </summary>
        /// <value>The conflictuous symbol</value>
        public Terminal ConflictSymbol { get { return p_Lookahead; } }

        /// <summary>
        /// Add a conflictuous item
        /// </summary>
        /// <param name="Item">A conflictuous item</param>
        public void AddItem(Item Item) { p_Items.Add(Item); }

        /// <summary>
        /// Constructs the conflict
        /// </summary>
        /// <param name="Method">Parse method</param>
        /// <param name="IType">Conflict type</param>
        public Conflict(System.Type MethodType, ConflictType Type, Terminal Lookahead)
        {
            p_MethodType = MethodType;
            p_Type = Type;
            p_Lookahead = Lookahead;
            p_Items = new System.Collections.Generic.List<Item>();
        }
        public Conflict(System.Type MethodType, ConflictType Type)
        {
            p_MethodType = MethodType;
            p_Type = Type;
            p_Items = new System.Collections.Generic.List<Item>();
        }

        /// <summary>
        /// Determine if the given LR Item is in the current conflict
        /// </summary>
        /// <param name="Item">the LR Item</param>
        /// <returns>True if the given item is conflictuous</returns>
        public bool ContainsItem(Item Item) { return p_Items.Contains(Item); }

        /// <summary>
        /// Override the ToString method
        /// </summary>
        /// <returns>Returns a human readable message</returns>
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