namespace Hime.Parsers.CF.LR
{
    /// <summary>
    /// Represents a LR(0) item
    /// </summary>
    public class ItemLR0 : Item
    {
        /// <summary>
        /// Construct the item from a rule, the dot position in the rule
        /// </summary>
        /// <param name="Rule">The rule on which the item is based</param>
        /// <param name="DotPosition">The position of the dot in the rule</param>
        public ItemLR0(CFRule Rule, int DotPosition) : base(Rule, DotPosition) { }

        /// <summary>
        /// Get the child of the current item
        /// </summary>
        /// <returns>Returns the child item or null if the item's action is Reduce</returns>
        public override Item GetChild()
        {
            if (Action == ItemAction.Reduce) return null;
            return new ItemLR0(p_Rule, p_DotPosition + 1);
        }
        /// <summary>
        /// Compute the closure for this item and add it to given list
        /// </summary>
        /// <param name="Closure">The closure of items being computed</param>
        public override void CloseTo(System.Collections.Generic.List<Item> Closure)
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

        /// <summary>
        /// Test if two objects are equals in a general meaning
        /// </summary>
        /// <param name="obj">The object to test</param>
        /// <returns>
        /// If obj is not of the same type as this, returns false;
        /// If obj is of the same type as this, returns true if the two items are equivalent, false otherwise
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is ItemLR0)
            {
                ItemLR0 Tested = (ItemLR0)obj;
                return Equals_Base(Tested);
            }
            return false;
        }

        /// <summary>
        /// Returns a string representing the item
        /// </summary>
        /// <param name="ShowLookaheads">True if the lookaheads should be displayed</param>
        /// <returns>A string representing the item</returns>
        public override string ToString() { return ToString(false); }
        /// <summary>
        /// Returns a string representing the item with decoration
        /// </summary>
        /// <param name="ShowDecoration">True to show the decoration (lookaheads and watermarks)</param>
        /// <returns>A string represeting the item</returns>
        public override string ToString(bool ShowDecoration)
        {
            System.Text.StringBuilder Builder = new System.Text.StringBuilder("[");
            Builder.Append(p_Rule.Variable.ToString());
            Builder.Append(" ->");
            int i = 0;
            foreach (RuleDefinitionPart Part in p_Definition.Parts)
            {
                if (i == p_DotPosition)
                    Builder.Append(" .");
                Builder.Append(" ");
                Builder.Append(Part.ToString());
                i++;
            }
            if (i == p_DotPosition)
                Builder.Append(" . ");
            Builder.Append("]");
            return Builder.ToString();
        }
    }
}