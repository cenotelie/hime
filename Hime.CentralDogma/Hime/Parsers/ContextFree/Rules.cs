using System.Collections.Generic;

namespace Hime.Parsers.CF
{
    public sealed class CFRuleDefinition : RuleDefinition
    {
        private CFRuleDefinitionSet choices;
        private TerminalSet firsts;

        public TerminalSet Firsts { get { return firsts; } }

        public CFRuleDefinition() : base() { firsts = new TerminalSet(); }
        public CFRuleDefinition(ICollection<RuleDefinitionPart> Parts) : base(Parts) { firsts = new TerminalSet(); }
        public CFRuleDefinition(Symbol UniqueSymbol) : base(UniqueSymbol) { firsts = new TerminalSet(); }


        public CFRuleDefinition GetChoiceAtIndex(int Index)
        {
            if (Index >= choices.Count)
                return null;
            return choices[Index];
        }

        public override Symbol GetSymbolAtIndex(int Index)
        {
            // If the definition does not contains choices (it may be a choice itself)
            if (choices == null)
            {
                // Returns the symbol of the part at the given index
                if (Index >= parts.Count)
                    return null;
                return parts[Index].Symbol;
            }
            // Returns the symbol of the part at the given index in the first choice
            else
            {
                if (Index >= choices[0].parts.Count)
                    return null;
                return choices[0].parts[Index].Symbol;
            }
        }

        private void ComputeChoices()
        {
            // Create the choices set
            choices = new CFRuleDefinitionSet();
            // For each part of the definition which is not a virtual symbol nor an action symbol
            foreach (RuleDefinitionPart Part in parts)
            {
                if ((Part.Symbol is Virtual) || (Part.Symbol is Action))
                    continue;
                // Append the symbol to all the choices definition
                foreach (CFRuleDefinition Choice in choices)
                    Choice.parts.Add(new RuleDefinitionPart(Part.Symbol, RuleDefinitionPartAction.Nothing));
                // Create a new choice with only the symbol
                choices.Add(new CFRuleDefinition(Part.Symbol));
            }
            // Create a new empty choice
            choices.Add(new CFRuleDefinition());
            firsts = choices[0].firsts;
        }

        private bool ComputeFirsts_Choice(int Index)
        {
            CFRuleDefinition Choice = choices[Index]; // Current choice
            
            // If the choice is empty : Add the ε to the Firsts and return
            if (Choice.Length == 0)
                return Choice.firsts.Add(TerminalEpsilon.Instance);

            Symbol Symbol = Choice.parts[0].Symbol;
            // If the first symbol in the choice is a terminal : Add terminal as first and return
            if (Symbol is Terminal)
                return Choice.firsts.Add((Terminal)Symbol);

            // Here the first symbol in the current choice is a variable
            CFVariable Variable = (CFVariable)Symbol;
            bool mod = false; // keep track of modifications
            // foreach first in the Firsts set of the variable
            foreach (Terminal First in Variable.Firsts)
            {
                // If the symbol is ε
                if (First == TerminalEpsilon.Instance)
                    // Add the Firsts set of the next choice to the current Firsts set
                    mod = mod || Choice.firsts.AddRange(choices[Index + 1].firsts);
                else
                    // Symbol is not ε : Add the symbol to the Firsts set
                    mod = mod || Choice.firsts.Add(First);
            }
            return mod;
        }

        public bool ComputeFirsts()
        {
            if (choices == null)
                ComputeChoices();

            bool mod = false;
            // for all choices in the reverse order : compute Firsts set for the choice
            for (int i = choices.Count - 1; i != -1; i--)
                mod = mod || ComputeFirsts_Choice(i);
            return mod;
        }

        public void ComputeFollowers_Step1()
        {
            // For all choices but the last (empty)
            for (int i = 0; i != choices.Count - 1; i++)
            {
                // If the first symbol of the choice is a variable
                if (choices[i].parts[0].Symbol is CFVariable)
                {
                    CFVariable Var = (CFVariable)choices[i].parts[0].Symbol;
                    // Add the Firsts set of the next choice to the variable followers except ε
                    foreach (Terminal First in choices[i + 1].firsts)
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
            for (int i = 0; i != choices.Count - 1; i++)
            {
                // If the first symbol of the choice is a variable
                if (choices[i].parts[0].Symbol is CFVariable)
                {
                    CFVariable Var = (CFVariable)choices[i].parts[0].Symbol;
                    // If the next choice Firsts set contains ε
                    // add the Followers of the head variable to the Followers of the found variable
                    if (choices[i + 1].firsts.Contains(TerminalEpsilon.Instance))
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
            Node.Attributes["ParserLength"].Value = choices[0].parts.Count.ToString();
            int i = 0;
            foreach (RuleDefinitionPart Part in parts)
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
            Result.parts.AddRange(Left.parts);
            Result.parts.AddRange(Right.parts);
            return Result;
        }
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
        public const string arrow = "→";

        private CFVariable variable;
        private CFRuleDefinition definition;
        private bool replaceOnProduction;
        private int iD;
        private int watermark;

        public CFVariable Variable { get { return variable; } }
        public CFRuleDefinition Definition { get { return definition; } }
        public bool ReplaceOnProduction { get { return replaceOnProduction; } }
        public int ID
        {
            get { return iD; }
            set { iD = value; }
        }
        public int Watermark { get { return watermark; } }


        public CFRule(CFVariable Variable, CFRuleDefinition Definition, bool ReplaceOnProduction)
        {
            variable = Variable;
            definition = Definition;
            replaceOnProduction = ReplaceOnProduction;
        }
        public CFRule(CFVariable Variable, CFRuleDefinition Definition, bool ReplaceOnProduction, int Watermark)
        {
            variable = Variable;
            definition = Definition;
            replaceOnProduction = ReplaceOnProduction;
            watermark = Watermark;
        }

        public System.Xml.XmlNode GetXMLNode(System.Xml.XmlDocument Doc)
        {
            System.Xml.XmlNode Node = Doc.CreateElement("Rule");
            Node.Attributes.Append(Doc.CreateAttribute("HeadName"));
            Node.Attributes.Append(Doc.CreateAttribute("HeadSID"));
            Node.Attributes.Append(Doc.CreateAttribute("RuleID"));
            Node.Attributes.Append(Doc.CreateAttribute("Replace"));
            Node.Attributes["HeadName"].Value = variable.LocalName;
            Node.Attributes["HeadSID"].Value = variable.SID.ToString("X");
            Node.Attributes["RuleID"].Value = iD.ToString("X");
            Node.Attributes["Replace"].Value = replaceOnProduction.ToString();
            Node.AppendChild(definition.GetXMLNode(Doc));
            return Node;
        }

        public override int GetHashCode() { return base.GetHashCode(); }
        public override bool Equals(object obj)
        {
            CFRule rule = obj as CFRule;
            if (this.variable.SID != rule.variable.SID)
                return false;
            if (this.replaceOnProduction != rule.replaceOnProduction)
                return false;
            return this.definition.Equals(rule.definition);
        }
        public override string ToString()
        {
            System.Text.StringBuilder Builder = new System.Text.StringBuilder();
            Builder.Append(variable.LocalName);
            Builder.Append(" ");
            Builder.Append(arrow);
            Builder.Append(definition.ToString());
            return Builder.ToString();
        }

        internal class Comparer : IEqualityComparer<CFRule>
        {
            public bool Equals(CFRule x, CFRule y)
            {
                if (x.variable.SID != y.variable.SID) return false;
                return (x.iD == y.iD);
            }
            public int GetHashCode(CFRule obj) { return ((obj.variable.SID << 16) + obj.iD); }
            private Comparer() { }
            private static Comparer instance = new Comparer();
            public static Comparer Instance { get { return instance; } }
        }
    }
}
