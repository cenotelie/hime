namespace Hime.Parsers.CF
{
    /// <summary>
    /// Represents a rule definiton
    /// </summary>
    public class CFRuleDefinition : RuleDefinition
    {
        /// <summary>
        /// List of the choices
        /// </summary>
        protected CFRuleDefinitionSet p_Choices;
        /// <summary>
        /// Firsts set for the current definition
        /// </summary>
        protected TerminalSet p_Firsts;

        /// <summary>
        /// Get the terminal set representing the Firsts set
        /// </summary>
        /// <value>The Firsts set</value>
        public TerminalSet Firsts { get { return p_Firsts; } }

        /// <summary>
        /// Constructs an empty definition
        /// </summary>
        public CFRuleDefinition() : base() { p_Firsts = new TerminalSet(); }
        /// <summary>
        /// Constructs a definition representing the given part list
        /// </summary>
        /// <param name="Parts">The parts to compose the definition</param>
        public CFRuleDefinition(System.Collections.Generic.IEnumerable<RuleDefinitionPart> Parts) : base(Parts) { p_Firsts = new TerminalSet(); }
        /// <summary>
        /// Constructs a definition containing a single symbol with no action
        /// </summary>
        /// <param name="UniqueSymbol">The unique symbol of the definition</param>
        public CFRuleDefinition(Symbol UniqueSymbol) : base(UniqueSymbol) { p_Firsts = new TerminalSet(); }


        /// <summary>
        /// Get the choice at the given index
        /// </summary>
        /// <param name="Index">Index of the choice to retrieve</param>
        /// <returns>Returns the choice at the given index of null if it cannot be retrieved</returns>
        public CFRuleDefinition GetChoiceAtIndex(int Index)
        {
            if (Index >= p_Choices.Count)
                return null;
            return p_Choices[Index];
        }

        /// <summary>
        /// Get the symbol at the given index
        /// </summary>
        /// <param name="Index">Index of the symbol to retrieve</param>
        /// <returns>Returns the symbol of null if it cannot be retrieved</returns>
        /// <remarks>The returned symbol cannot be a virtual symbol, it is always a terminal or a variable</remarks>
        public override Symbol GetSymbolAtIndex(int Index)
        {
            // If the definition does not contains choices (it may be a choice itself)
            if (p_Choices == null)
            {
                // Returns the symbol of the part at the given index
                if (Index >= p_Parts.Count)
                    return null;
                return p_Parts[Index].Symbol;
            }
            // Returns the symbol of the part at the given index in the first choice
            else
            {
                if (Index >= p_Choices[0].p_Parts.Count)
                    return null;
                return p_Choices[0].p_Parts[Index].Symbol;
            }
        }

        /// <summary>
        /// Compute the choices for the definition
        /// </summary>
        /// <remarks>
        /// If the definition is [a b c] and doest not contains virtual symbols nor action symbols, the choices will be :
        /// { [a b c], [b c], [c], [] }
        /// </remarks>
        protected void ComputeChoices()
        {
            // Create the choices set
            p_Choices = new CFRuleDefinitionSet();
            // For each part of the definition which is not a virtual symbol nor an action symbol
            foreach (RuleDefinitionPart Part in p_Parts)
            {
                if ((Part.Symbol is Virtual) || (Part.Symbol is Action))
                    continue;
                // Append the symbol to all the choices definition
                foreach (CFRuleDefinition Choice in p_Choices)
                    Choice.p_Parts.Add(new RuleDefinitionPart(Part.Symbol, RuleDefinitionPartAction.Nothing));
                // Create a new choice with only the symbol
                p_Choices.Add(new CFRuleDefinition(Part.Symbol));
            }
            // Create a new empty choice
            p_Choices.Add(new CFRuleDefinition());
            p_Firsts = p_Choices[0].p_Firsts;
        }

        /// <summary>
        /// Compute the Firsts set for the choice at the given index
        /// </summary>
        /// <param name="Index">Index of the choice</param>
        /// <returns>Returns true if changes occur, false otherwise</returns>
        protected bool ComputeFirsts_Choice(int Index)
        {
            CFRuleDefinition Choice = p_Choices[Index]; // Current choice
            
            // If the choice is empty : Add the ε to the Firsts and return
            if (Choice.Length == 0)
                return Choice.p_Firsts.Add(TerminalEpsilon.Instance);

            Symbol Symbol = Choice.p_Parts[0].Symbol;
            // If the first symbol in the choice is a terminal : Add terminal as first and return
            if (Symbol is Terminal)
                return Choice.p_Firsts.Add((Terminal)Symbol);

            // Here the first symbol in the current choice is a variable
            CFVariable Variable = (CFVariable)Symbol;
            bool mod = false; // keep track of modifications
            // foreach first in the Firsts set of the variable
            foreach (Terminal First in Variable.Firsts)
            {
                // If the symbol is ε
                if (First == TerminalEpsilon.Instance)
                    // Add the Firsts set of the next choice to the current Firsts set
                    mod = mod || Choice.p_Firsts.AddRange(p_Choices[Index + 1].p_Firsts);
                else
                    // Symbol is not ε : Add the symbol to the Firsts set
                    mod = mod || Choice.p_Firsts.Add(First);
            }
            return mod;
        }

        /// <summary>
        /// Compute the Firsts set for the definition
        /// </summary>
        /// <returns>Returns true if changes occur, false otherwise</returns>
        public bool ComputeFirsts()
        {
            if (p_Choices == null)
                ComputeChoices();

            bool mod = false;
            // for all choices in the reverse order : compute Firsts set for the choice
            for (int i = p_Choices.Count - 1; i != -1; i--)
                mod = mod || ComputeFirsts_Choice(i);
            return mod;
        }

        /// <summary>
        /// Compute the Followers set - step 1
        /// </summary>
        /// <remarks>
        /// If the definition is of the form [alpha V beta] where V is a variable,
        /// Add the Firsts of beta except ε to the Followers of V
        /// </remarks>
        public void ComputeFollowers_Step1()
        {
            // For all choices but the last (empty)
            for (int i = 0; i != p_Choices.Count - 1; i++)
            {
                // If the first symbol of the choice is a variable
                if (p_Choices[i].p_Parts[0].Symbol is CFVariable)
                {
                    CFVariable Var = (CFVariable)p_Choices[i].p_Parts[0].Symbol;
                    // Add the Firsts set of the next choice to the variable followers except ε
                    foreach (Terminal First in p_Choices[i + 1].p_Firsts)
                    {
                        if (First != TerminalEpsilon.Instance)
                            Var.Followers.Add(First);
                    }
                }
            }
        }

        /// <summary>
        /// Compute the Followers set - step 2 and 3
        /// </summary>
        /// <param name="RuleVar">The Variable head of the rule of the definition</param>
        /// <returns>Returns true if changes occur, false otherwise</returns>
        /// <remarks>
        /// If the defintion is part of a rule of the form [HeadVar -> alpha V beta]
        /// If ε is in the Firsts of beta :
        /// Add the followers of HeadVar to the Followers of V
        /// </remarks>
        public bool ComputeFollowers_Step23(CFVariable RuleVar)
        {
            bool mod = false;
            // For all choices but the last (empty)
            for (int i = 0; i != p_Choices.Count - 1; i++)
            {
                // If the first symbol of the choice is a variable
                if (p_Choices[i].p_Parts[0].Symbol is CFVariable)
                {
                    CFVariable Var = (CFVariable)p_Choices[i].p_Parts[0].Symbol;
                    // If the next choice Firsts set contains ε
                    // add the Followers of the head variable to the Followers of the found variable
                    if (p_Choices[i + 1].p_Firsts.Contains(TerminalEpsilon.Instance))
                        if (Var.Followers.AddRange(RuleVar.Followers))
                            mod = true;
                }
            }
            return mod;
        }

        /// <summary>
        /// Get the XML node representing the definition
        /// </summary>
        /// <param name="Doc">XML parent document</param>
        /// <returns>Returns the new node</returns>
        public System.Xml.XmlNode GetXMLNode(System.Xml.XmlDocument Doc)
        {
            System.Xml.XmlNode Node = Doc.CreateElement("RuleDefinition");
            Node.Attributes.Append(Doc.CreateAttribute("ParserLength"));
            Node.Attributes["ParserLength"].Value = p_Choices[0].p_Parts.Count.ToString();
            int i = 0;
            foreach (RuleDefinitionPart Part in p_Parts)
            {
                Node.AppendChild(Part.GetXMLNode(Doc));
                if ((Part.Symbol is Terminal) || (Part.Symbol is Variable))
                {
                    Node.LastChild.Attributes["ParserIndex"].Value = i.ToString();
                    i++;
                }
            }
            return Node;
        }

        /// <summary>
        /// Overload the + operator to concatenate two definitions
        /// </summary>
        /// <param name="Left">Left operand</param>
        /// <param name="Right">Right operand</param>
        /// <returns>Returns a new definition equivalent to the concatenation of the two operands</returns>
        public static CFRuleDefinition operator +(CFRuleDefinition Left, CFRuleDefinition Right)
        {
            CFRuleDefinition Result = new CFRuleDefinition();
            Result.p_Parts.AddRange(Left.p_Parts);
            Result.p_Parts.AddRange(Right.p_Parts);
            return Result;
        }

        /// <summary>
        /// Determine if two definitions are equal
        /// </summary>
        /// <param name="Left">Left operand</param>
        /// <param name="Right">Right operand</param>
        /// <returns>Returns true if the two defintions are equal, false otherwise</returns>
        public static bool operator ==(CFRuleDefinition Left, CFRuleDefinition Right)
        {
            if (Left.p_Parts.Count != Right.p_Parts.Count)
                return false;
            for (int i = 0; i != Left.p_Parts.Count; i++)
            {
                if (Left.p_Parts[i] != Right.p_Parts[i])
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Determine if two definitions are different
        /// </summary>
        /// <param name="Left">Left operand</param>
        /// <param name="Right">Right operand</param>
        /// <returns>Returns true if the two definitions are different, false otherwise</returns>
        public static bool operator !=(CFRuleDefinition Left, CFRuleDefinition Right)
        {
            if (Left.p_Parts.Count != Right.p_Parts.Count)
                return true;
            for (int i = 0; i != Left.p_Parts.Count; i++)
            {
                if (Left.p_Parts[i] != Right.p_Parts[i])
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Override the Equals function to use the == operator
        /// </summary>
        /// <param name="obj">Tested object</param>
        /// <returns>Returns true if the tested object is equal to the definition</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is CFRuleDefinition))
                return false;
            CFRuleDefinition right = (CFRuleDefinition)obj;
            return (this == right);
        }

        /// <summary>
        /// Override the GetHashCode, call the base GetHashCode function
        /// </summary>
        /// <returns>Returns the value of the base function</returns>
        public override int GetHashCode() { return base.GetHashCode(); }
    }




    /// <summary>
    /// Represents a set of rule definition
    /// </summary>
    public class CFRuleDefinitionSet : System.Collections.Generic.List<CFRuleDefinition>
    {
        /// <summary>
        /// Union of the two sets
        /// </summary>
        /// <param name="Left">Left operand</param>
        /// <param name="Right">Right operand</param>
        /// <returns>Returns the union of the two sets as a new set</returns>
        public static CFRuleDefinitionSet operator +(CFRuleDefinitionSet Left, CFRuleDefinitionSet Right)
        {
            CFRuleDefinitionSet Result = new CFRuleDefinitionSet();
            Result.AddRange(Left);
            Result.AddRange(Right);
            return Result;
        }

        /// <summary>
        /// Cartesian products of the two sets
        /// </summary>
        /// <param name="Left">Left operand</param>
        /// <param name="Right">Right operand</param>
        /// <returns>Returns the cartesian product of the two operands as a new set</returns>
        /// <remarks>The elements of the new set are the concatenation of an element of Left and an element of Right</remarks>
        public static CFRuleDefinitionSet operator *(CFRuleDefinitionSet Left, CFRuleDefinitionSet Right)
        {
            CFRuleDefinitionSet Result = new CFRuleDefinitionSet();
            foreach (CFRuleDefinition DefLeft in Left)
                foreach (CFRuleDefinition DefRight in Right)
                    Result.Add(DefLeft + DefRight);
            return Result;
        }

        /// <summary>
        /// Apply the Promote action to all parts of all the definitions within the set
        /// </summary>
        public void SetActionPromote()
        {
            foreach (RuleDefinition Def in this)
                foreach (RuleDefinitionPart Part in Def.Parts)
                    Part.Action = RuleDefinitionPartAction.Promote;
        }

        /// <summary>
        /// Apply the Drop action to all parts of all the definitions within the set
        /// </summary>
        public void SetActionDrop()
        {
            foreach (RuleDefinition Def in this)
                foreach (RuleDefinitionPart Part in Def.Parts)
                    Part.Action = RuleDefinitionPartAction.Drop;
        }
    }



    /// <summary>
    /// Represents a flat rule obtained from a tree-represented rule
    /// </summary>
    /// <remarks>This class represents a rule by the form [Variable -> Definition]</remarks>
    public class CFRule
    {
        /// <summary>
        /// Rule head variable (left part)
        /// </summary>
        private CFVariable p_Variable;
        /// <summary>
        /// Definition of the rule (right part)
        /// </summary>
        private CFRuleDefinition p_Definition;
        /// <summary>
        /// Define if the rule head variable has to be immediatly replace by the definition on tree construction during the parse phase
        /// </summary>
        private bool p_ReplaceOnProduction;
        /// <summary>
        /// ID of the rule used by the XML representation
        /// </summary>
        private int p_ID;
        /// <summary>
        /// Watermark for the rule
        /// </summary>
        /// <remarks>Watermark is used detect sub-rules that are mutualy exclusive</remarks>
        private int p_Watermark;

        /// <summary>
        /// Get the rule head variable
        /// </summary>
        /// <value>The head variable</value>
        public CFVariable Variable { get { return p_Variable; } }
        /// <summary>
        /// Get the rule definition
        /// </summary>
        /// <value>The rule definition</value>
        public CFRuleDefinition Definition { get { return p_Definition; } }
        /// <summary>
        /// Get a value indicating if the head variable has te be immediatly replaced by the definition on tree construction during the parse phase
        /// </summary>
        /// <value>True if the variable has to be replaced, false otherwise</value>
        public bool ReplaceOnProduction { get { return p_ReplaceOnProduction; } }
        /// <summary>
        /// Get or set the rule ID
        /// </summary>
        /// <value>The rule ID</value>
        public int ID
        {
            get { return p_ID; }
            set { p_ID = value; }
        }
        /// <summary>
        /// Get the watermark for the rule
        /// </summary>
        /// <value>Watermark for the rule</value>
        /// <remarks>Watermark is used detect sub-rules that are mutualy exclusive</remarks>
        public int Watermark { get { return p_Watermark; } }


        /// <summary>
        /// Construct the rule from the given head, definition and replace instruction
        /// </summary>
        /// <param name="Variable">The head variable</param>
        /// <param name="Definition">The rule definition</param>
        /// <param name="ReplaceOnProduction">True if the definition replace the variable on production</param>
        public CFRule(CFVariable Variable, CFRuleDefinition Definition, bool ReplaceOnProduction)
        {
            p_Variable = Variable;
            p_Definition = Definition;
            p_ReplaceOnProduction = ReplaceOnProduction;
        }
        /// <summary>
        /// Construct the rule from the given head, definition and replace instruction
        /// </summary>
        /// <param name="Variable">The head variable</param>
        /// <param name="Definition">The rule definition</param>
        /// <param name="ReplaceOnProduction">True if the definition replace the variable on production</param>
        /// <param name="Watermark">Watermark for the rule</param>
        public CFRule(CFVariable Variable, CFRuleDefinition Definition, bool ReplaceOnProduction, int Watermark)
        {
            p_Variable = Variable;
            p_Definition = Definition;
            p_ReplaceOnProduction = ReplaceOnProduction;
            p_Watermark = Watermark;
        }

        /// <summary>
        /// Get the XML node representing the rule
        /// </summary>
        /// <param name="Doc">The XML parent document</param>
        /// <returns>Returns the new node</returns>
        public System.Xml.XmlNode GetXMLNode(System.Xml.XmlDocument Doc)
        {
            System.Xml.XmlNode Node = Doc.CreateElement("Rule");
            Node.Attributes.Append(Doc.CreateAttribute("HeadName"));
            Node.Attributes.Append(Doc.CreateAttribute("HeadSID"));
            Node.Attributes.Append(Doc.CreateAttribute("RuleID"));
            Node.Attributes.Append(Doc.CreateAttribute("Replace"));
            Node.Attributes["HeadName"].Value = p_Variable.LocalName;
            Node.Attributes["HeadSID"].Value = p_Variable.SID.ToString("X");
            Node.Attributes["RuleID"].Value = p_ID.ToString("X");
            Node.Attributes["Replace"].Value = p_ReplaceOnProduction.ToString();
            Node.AppendChild(p_Definition.GetXMLNode(Doc));
            return Node;
        }

        /// <summary>
        /// Determine if two rules are equal
        /// </summary>
        /// <param name="Left">Left operand</param>
        /// <param name="Right">Right operand</param>
        /// <returns>True if the two operands are equal, false otherwise</returns>
        public static bool operator ==(CFRule Left, CFRule Right)
        {
            if (Left.p_Variable != Right.p_Variable)
                return false;
            if (Left.p_ReplaceOnProduction != Right.p_ReplaceOnProduction)
                return false;
            return (Left.p_Definition == Right.p_Definition);
        }

        /// <summary>
        /// Determine if two rules are different
        /// </summary>
        /// <param name="Left">Left operand</param>
        /// <param name="Right">Right operand</param>
        /// <returns>True if the two operands are differents, false otherwise</returns>
        public static bool operator !=(CFRule Left, CFRule Right)
        {
            if (Left.p_Variable != Right.p_Variable)
                return true;
            if (Left.p_ReplaceOnProduction != Right.p_ReplaceOnProduction)
                return true;
            return (Left.p_Definition != Right.p_Definition);
        }

        /// <summary>
        /// Override the Equals function to use the overloaded == operator
        /// </summary>
        /// <param name="obj">The tested object</param>
        /// <returns>Returns true if the tested object is equal to the rule, false otherwise</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is CFRule))
                return false;
            CFRule right = (CFRule)obj;
            return (this == right);
        }

        /// <summary>
        /// Override the GetHashCode function
        /// </summary>
        /// <returns>Returns the base GetHashCode function</returns>
        public override int GetHashCode() { return base.GetHashCode(); }

        /// <summary>
        /// Override the ToString function
        /// </summary>
        /// <returns>Returns a string describing the rule of the form "HeadName -> DefinitionString"</returns>
        public override string ToString()
        {
            System.Text.StringBuilder Builder = new System.Text.StringBuilder();
            Builder.Append(p_Variable.LocalName);
            Builder.Append(" ->");
            Builder.Append(p_Definition.ToString());
            return Builder.ToString();
        }
    }
}
