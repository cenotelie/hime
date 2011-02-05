namespace Hime.Generators.Parsers.ContextFree.LR
{
    /// <summary>
    /// LR conflict type
    /// </summary>
    public enum LRConflictType
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
    public class LRConflict : Hime.Kernel.Reporting.Entry
    {
        /// <summary>
        /// Parse method used
        /// </summary>
        protected GrammarParseMethod p_Method;
        /// <summary>
        /// Conflict type
        /// </summary>
        protected LRConflictType p_Type;
        /// <summary>
        /// The conflictuous lookahead
        /// </summary>
        private Terminal p_Lookahead;
        /// <summary>
        /// The conflictuous items
        /// </summary>
        private System.Collections.Generic.List<LRItem> p_Items;

        public Hime.Kernel.Reporting.Level Level {
            get
            {
                if (p_Method == GrammarParseMethod.LR0) return Hime.Kernel.Reporting.Level.Error;
                if (p_Method == GrammarParseMethod.LR1) return Hime.Kernel.Reporting.Level.Error;
                if (p_Method == GrammarParseMethod.LALR1) return Hime.Kernel.Reporting.Level.Error;
                return Hime.Kernel.Reporting.Level.Warning;
            }
        }
        public string Component { get { return p_Method.ToString(); } }
        public string Message { get { return ToString(); } }

        /// <summary>
        /// Get the used parse method
        /// </summary>
        /// <value>The method</value>
        public GrammarParseMethod Method { get { return p_Method; } }
        /// <summary>
        /// Get the conflict type
        /// </summary>
        /// <value>The conflict type</value>
        public LRConflictType ConflictType { get { return p_Type; } }
        /// <summary>
        /// Get the conflictuous symbol
        /// </summary>
        /// <value>The conflictuous symbol</value>
        public Terminal ConflictSymbol { get { return p_Lookahead; } }

        /// <summary>
        /// Add a conflictuous item
        /// </summary>
        /// <param name="Item">A conflictuous item</param>
        public void AddItem(LRItem Item) { p_Items.Add(Item); }

        /// <summary>
        /// Constructs the conflict
        /// </summary>
        /// <param name="Method">Parse method</param>
        /// <param name="IType">Conflict type</param>
        public LRConflict(GrammarParseMethod Method, LRConflictType Type, Terminal Lookahead)
        {
            p_Method = Method;
            p_Type = Type;
            p_Lookahead = Lookahead;
            p_Items = new System.Collections.Generic.List<LRItem>();
        }
        public LRConflict(GrammarParseMethod Method, LRConflictType Type)
        {
            p_Method = Method;
            p_Type = Type;
            p_Items = new System.Collections.Generic.List<LRItem>();
        }

        /// <summary>
        /// Determine if the given LR Item is in the current conflict
        /// </summary>
        /// <param name="Item">the LR Item</param>
        /// <returns>True if the given item is conflictuous</returns>
        public bool ContainsItem(LRItem Item) { return p_Items.Contains(Item); }

        /// <summary>
        /// Override the ToString method
        /// </summary>
        /// <returns>Returns a human readable message</returns>
        public override string ToString()
        {
            System.Text.StringBuilder Builder = new System.Text.StringBuilder("Conflict ");
            if (p_Type == LRConflictType.ShiftReduce)
                Builder.Append("Shift/Reduce");
            else
                Builder.Append("Reduce/Reduce");
            if (p_Lookahead != null)
            {
                Builder.Append(" on terminal ");
                Builder.Append(p_Lookahead.ToString());
            }
            Builder.Append(" for items {");
            foreach (LRItem Item in p_Items)
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