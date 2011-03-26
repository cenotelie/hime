using System.Collections.Generic;

namespace Hime.Parsers.CF
{
    public sealed class CFRuleDefinition : RuleDefinition
    {
        private CFRuleDefinitionSet p_Choices;
        private TerminalSet p_Firsts;

        public TerminalSet Firsts { get { return p_Firsts; } }

        public CFRuleDefinition() : base() { p_Firsts = new TerminalSet(); }
        public CFRuleDefinition(ICollection<RuleDefinitionPart> Parts) : base(Parts) { p_Firsts = new TerminalSet(); }
        public CFRuleDefinition(Symbol UniqueSymbol) : base(UniqueSymbol) { p_Firsts = new TerminalSet(); }


        public CFRuleDefinition GetChoiceAtIndex(int Index)
        {
            if (Index >= p_Choices.Count)
                return null;
            return p_Choices[Index];
        }

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

        private void ComputeChoices()
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

        private bool ComputeFirsts_Choice(int Index)
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

        public static CFRuleDefinition operator +(CFRuleDefinition Left, CFRuleDefinition Right)
        {
            CFRuleDefinition Result = new CFRuleDefinition();
            Result.p_Parts.AddRange(Left.p_Parts);
            Result.p_Parts.AddRange(Right.p_Parts);
            return Result;
        }

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

        public override bool Equals(object obj)
        {
            if (!(obj is CFRuleDefinition))
                return false;
            CFRuleDefinition right = (CFRuleDefinition)obj;
            return (this == right);
        }

        public override int GetHashCode() { return base.GetHashCode(); }
    }




    public sealed class CFRuleDefinitionSet : List<CFRuleDefinition>
    {
        public static CFRuleDefinitionSet operator +(CFRuleDefinitionSet Left, CFRuleDefinitionSet Right)
        {
            CFRuleDefinitionSet Result = new CFRuleDefinitionSet();
            Result.AddRange(Left);
            Result.AddRange(Right);
            return Result;
        }

        public static CFRuleDefinitionSet operator *(CFRuleDefinitionSet Left, CFRuleDefinitionSet Right)
        {
            CFRuleDefinitionSet Result = new CFRuleDefinitionSet();
            foreach (CFRuleDefinition DefLeft in Left)
                foreach (CFRuleDefinition DefRight in Right)
                    Result.Add(DefLeft + DefRight);
            return Result;
        }

        public void SetActionPromote()
        {
            foreach (RuleDefinition Def in this)
                foreach (RuleDefinitionPart Part in Def.Parts)
                    Part.Action = RuleDefinitionPartAction.Promote;
        }

        public void SetActionDrop()
        {
            foreach (RuleDefinition Def in this)
                foreach (RuleDefinitionPart Part in Def.Parts)
                    Part.Action = RuleDefinitionPartAction.Drop;
        }
    }



    public sealed class CFRule
    {
        private CFVariable p_Variable;
        private CFRuleDefinition p_Definition;
        private bool p_ReplaceOnProduction;
        private int p_ID;
        private int p_Watermark;

        public CFVariable Variable { get { return p_Variable; } }
        public CFRuleDefinition Definition { get { return p_Definition; } }
        public bool ReplaceOnProduction { get { return p_ReplaceOnProduction; } }
        public int ID
        {
            get { return p_ID; }
            set { p_ID = value; }
        }
        public int Watermark { get { return p_Watermark; } }


        public CFRule(CFVariable Variable, CFRuleDefinition Definition, bool ReplaceOnProduction)
        {
            p_Variable = Variable;
            p_Definition = Definition;
            p_ReplaceOnProduction = ReplaceOnProduction;
        }
        public CFRule(CFVariable Variable, CFRuleDefinition Definition, bool ReplaceOnProduction, int Watermark)
        {
            p_Variable = Variable;
            p_Definition = Definition;
            p_ReplaceOnProduction = ReplaceOnProduction;
            p_Watermark = Watermark;
        }

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

        public static bool operator ==(CFRule Left, CFRule Right)
        {
            if (Left.p_Variable != Right.p_Variable)
                return false;
            if (Left.p_ReplaceOnProduction != Right.p_ReplaceOnProduction)
                return false;
            return (Left.p_Definition == Right.p_Definition);
        }

        public static bool operator !=(CFRule Left, CFRule Right)
        {
            if (Left.p_Variable != Right.p_Variable)
                return true;
            if (Left.p_ReplaceOnProduction != Right.p_ReplaceOnProduction)
                return true;
            return (Left.p_Definition != Right.p_Definition);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is CFRule))
                return false;
            CFRule right = (CFRule)obj;
            return (this == right);
        }

        public override int GetHashCode() { return base.GetHashCode(); }

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
