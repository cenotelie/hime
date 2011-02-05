namespace Hime.Generators.Parsers.ContextFree.LR
{
    /// <summary>
    /// The action to be taken for a LR item
    /// </summary>
    public enum LRItemAction
    {
        /// <summary>
        /// Shift to a chid item
        /// </summary>
        Shift,
        /// <summary>
        /// Apply a grammar rule
        /// </summary>
        Reduce
    }



    /// <summary>
    /// Represents a watermark for a LR item
    /// </summary>
    public class LRItemWatermark
    {
        private System.Collections.Generic.List<int> p_Marks;

        public System.Collections.Generic.List<int> Marks { get { return p_Marks; } }
        public bool IsNeutral { get { return (p_Marks.Count == 1 && p_Marks[0] == 0); } }

        public LRItemWatermark(int BaseMark)
        {
            p_Marks = new System.Collections.Generic.List<int>();
            p_Marks.Add(BaseMark);
        }
        public LRItemWatermark(LRItemWatermark Copied)
        {
            p_Marks = new System.Collections.Generic.List<int>(Copied.p_Marks);
        }

        public void AddMark(int Mark)
        {
            if (!p_Marks.Contains(Mark))
                p_Marks.Add(Mark);
        }
        public void AddWatermark(LRItemWatermark Watermark)
        {
            foreach (int Mark in Watermark.p_Marks)
                AddMark(Mark);
        }

        public override string ToString()
        {
            System.Text.StringBuilder Builder = new System.Text.StringBuilder("{");
            for (int i = 0; i != p_Marks.Count; i++)
            {
                if (i != 0)
                    Builder.Append("|");
                Builder.Append(p_Marks[i].ToString());
            }
            Builder.Append("}");
            return Builder.ToString();
        }
    }



    /// <summary>
    /// Represents a LR item
    /// </summary>
    /// <remarks>
    /// Item is of the form : [Var -> alpha . NextSymbol NextChoice]
    /// Where [Var -> alpha NextSymbol NextChoice] is a rule for Var
    /// </remarks>
    public abstract class LRItem
    {
        /// <summary>
        /// The rule used by the current item
        /// </summary>
        protected CFRule p_Rule;
        /// <summary>
        /// The rule definition without virtual symbols
        /// </summary>
        protected CFRuleDefinition p_Definition;
        /// <summary>
        /// The position of the item dot in the rule
        /// </summary>
        protected int p_DotPosition;
        /// <summary>
        /// Watermark for the item
        /// </summary>
        protected LRItemWatermark p_Watermark;

        /// <summary>
        /// Get the rule on which this item is based
        /// </summary>
        /// <value>The grammar rule</value>
        public CFRule BaseRule { get { return p_Rule; } }
        /// <summary>
        /// Get the dot position in the base rule
        /// </summary>
        /// <value>The dot position</value>
        public int DotPosition { get { return p_DotPosition; } }
        /// <summary>
        /// Get the action taken by this item
        /// </summary>
        /// <value>The action for the item</value>
        public LRItemAction Action
        {
            get
            {
                if (p_DotPosition != p_Definition.Length)
                    return LRItemAction.Shift;
                return LRItemAction.Reduce;
            }
        }
        /// <summary>
        /// Get the watermark for the item
        /// </summary>
        /// <value>The watermark for the item</value>
        public LRItemWatermark Watermark { get { return p_Watermark; } }
        
        /// <summary>
        /// Get the symbol that is right after the dot in the rule definition
        /// </summary>
        /// <value>The symbol following the dot, may be null</value>
        public Symbol NextSymbol { get { return p_Definition.GetSymbolAtIndex(p_DotPosition); } }
        /// <summary>
        /// Get the choice in the rule definiton after the symbol following the dot
        /// </summary>
        /// <value>The choice at the dot next position</value>
        public CFRuleDefinition NextChoice { get { return p_Rule.Definition.GetChoiceAtIndex(p_DotPosition + 1); } }

        /// <summary>
        /// Construct the item from a rule, the dot position in the rule
        /// </summary>
        /// <param name="Rule">The rule on which the item is based</param>
        /// <param name="DotPosition">The position of the dot in the rule</param>
        public LRItem(CFRule Rule, int DotPosition)
        {
            p_Rule = Rule;
            p_Definition = p_Rule.Definition.GetChoiceAtIndex(0);
            p_DotPosition = DotPosition;
            p_Watermark = new LRItemWatermark(Rule.Watermark);
        }
        /// <summary>
        /// Construct the item from a rule, the dot position in the rule
        /// </summary>
        /// <param name="Rule">The rule on which the item is based</param>
        /// <param name="DotPosition">The position of the dot in the rule</param>
        /// <param name="Watermark">A watermark to propagate</param>
        public LRItem(CFRule Rule, int DotPosition, LRItemWatermark Watermark)
        {
            p_Rule = Rule;
            p_Definition = p_Rule.Definition.GetChoiceAtIndex(0);
            p_DotPosition = DotPosition;
            if (Watermark.IsNeutral)
            {
                p_Watermark = new LRItemWatermark(Rule.Watermark);
            }
            else
            {
                p_Watermark = new LRItemWatermark(Watermark);
                if (Rule.Watermark != 0)
                    p_Watermark.AddMark(Rule.Watermark);
            }
        }

        /// <summary>
        /// Test if the two items are equals in the LR(0) meaning
        /// </summary>
        /// <param name="Item">The item to test</param>
        /// <returns>Returns true if the given Item is equal to the current one, false otherwise</returns>
        /// <remarks>Test is made uppon the rule and the dot position</remarks>
        public bool Equals_Base(LRItem Item)
        {
            if (p_Rule.Variable.SID != Item.p_Rule.Variable.SID)
                return false;
            if (p_Rule.ID != Item.p_Rule.ID)
                return false;
            return (p_DotPosition == Item.p_DotPosition);
        }

        /// <summary>
        /// Get the child of the current item
        /// </summary>
        /// <returns>Returns the child item or null if the item's action is Reduce</returns>
        public abstract LRItem GetChild();
        /// <summary>
        /// Compute the closure for this item and add it to given list
        /// </summary>
        /// <param name="Closure">The closure of items being computed</param>
        public abstract void CloseTo(System.Collections.Generic.List<LRItem> Closure);

        /// <summary>
        /// Test if two objects are equals in a general meaning
        /// </summary>
        /// <param name="obj">The object to test</param>
        /// <returns>
        /// If obj is not of the same type as this, returns false;
        /// If obj is of the same type as this, returns true if the two items are equivalent, false otherwise
        /// </returns>
        public abstract override bool Equals(object obj);
        /// <summary>
        /// Get the hash code for this object
        /// </summary>
        /// <returns>Returns the value from the base function</returns>
        public override int GetHashCode() { return base.GetHashCode(); }
        /// <summary>
        /// Returns a string representing the item
        /// </summary>
        /// <returns>A string represeting the item</returns>
        public abstract override string ToString();
        /// <summary>
        /// Returns a string representing the item with decoration
        /// </summary>
        /// <param name="ShowDecoration">True to show the decoration (lookaheads and watermarks)</param>
        /// <returns>A string represeting the item</returns>
        public abstract string ToString(bool ShowDecoration);
    }
}