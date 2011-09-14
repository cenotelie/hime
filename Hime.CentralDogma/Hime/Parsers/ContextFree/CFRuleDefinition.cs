/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;

namespace Hime.Parsers.ContextFree
{
    public sealed class CFRuleDefinition : RuleDefinition
    {
        private CFRuleDefinitionSet choices;
        private TerminalSet firsts;

        public TerminalSet Firsts { get { return firsts; } }

        public CFRuleDefinition() : base() { firsts = new TerminalSet(); }
        public CFRuleDefinition(ICollection<RuleDefinitionPart> parts) : base(parts) { firsts = new TerminalSet(); }
        public CFRuleDefinition(Symbol symbol) : base(symbol) { firsts = new TerminalSet(); }


        public CFRuleDefinition GetChoiceAt(int index)
        {
            if (index >= choices.Count)
                return null;
            return choices[index];
        }

        public override Symbol GetSymbolAt(int index)
        {
            // If the definition does not contains choices (it may be a choice itself)
            if (choices == null)
            {
                // Returns the symbol of the part at the given index
                if (index >= parts.Count)
                    return null;
                return parts[index].Symbol;
            }
            // Returns the symbol of the part at the given index in the first choice
            else
            {
                if (index >= choices[0].parts.Count)
                    return null;
                return choices[0].parts[index].Symbol;
            }
        }

        private void ComputeChoices()
        {
            // Create the choices set
            choices = new CFRuleDefinitionSet();
            // For each part of the definition which is not a virtual symbol nor an action symbol
            foreach (RuleDefinitionPart part in parts)
            {
                if ((part.Symbol is Virtual) || (part.Symbol is Action))
                    continue;
                // Append the symbol to all the choices definition
                foreach (CFRuleDefinition choice in choices)
                    choice.parts.Add(new RuleDefinitionPart(part.Symbol, RuleDefinitionPartAction.Nothing));
                // Create a new choice with only the symbol
                choices.Add(new CFRuleDefinition(part.Symbol));
            }
            // Create a new empty choice
            choices.Add(new CFRuleDefinition());
            firsts = choices[0].firsts;
        }

        private bool ComputeFirsts_Choice(int index)
        {
            CFRuleDefinition choice = choices[index]; // Current choice

            // If the choice is empty : Add the ε to the Firsts and return
            if (choice.Length == 0)
                return choice.firsts.Add(TerminalEpsilon.Instance);

            Symbol symbol = choice.parts[0].Symbol;
            // If the first symbol in the choice is a terminal : Add terminal as first and return
            if (symbol is Terminal)
                return choice.firsts.Add((Terminal)symbol);

            // Here the first symbol in the current choice is a variable
            Variable variable = (Variable)symbol;
            bool mod = false; // keep track of modifications
            // foreach first in the Firsts set of the variable
            foreach (Terminal first in variable.Firsts)
            {
                // If the symbol is ε
                if (first == TerminalEpsilon.Instance)
                    // Add the Firsts set of the next choice to the current Firsts set
                    mod = mod || choice.firsts.AddRange(choices[index + 1].firsts);
                else
                    // Symbol is not ε : Add the symbol to the Firsts set
                    mod = mod || choice.firsts.Add(first);
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
                // TODO: is and casts are not nice => try to remove all of them. They shouldn't be necessary
                // If the first symbol of the choice is a variable
                if (choices[i].parts[0].Symbol is Variable)
                {
                    Variable var = (Variable)choices[i].parts[0].Symbol;
                    // Add the Firsts set of the next choice to the variable followers except ε
                    foreach (Terminal first in choices[i + 1].firsts)
                    {
                        if (first != TerminalEpsilon.Instance)
                            var.Followers.Add(first);
                    }
                }
            }
        }

        public bool ComputeFollowers_Step23(Variable ruleVar)
        {
            bool mod = false;
            // For all choices but the last (empty)
            for (int i = 0; i != choices.Count - 1; i++)
            {
                // If the first symbol of the choice is a variable
                if (choices[i].parts[0].Symbol is Variable)
                {
                    Variable var = (Variable)choices[i].parts[0].Symbol;
                    // If the next choice Firsts set contains ε
                    // add the Followers of the head variable to the Followers of the found variable
                    if (choices[i + 1].firsts.Contains(TerminalEpsilon.Instance))
                        if (var.Followers.AddRange(ruleVar.Followers))
                            mod = true;
                }
            }
            return mod;
        }

        public System.Xml.XmlNode GetXMLNode(System.Xml.XmlDocument document)
        {
            System.Xml.XmlNode node = document.CreateElement("RuleDefinition");
            node.Attributes.Append(document.CreateAttribute("ParserLength"));
            node.Attributes["ParserLength"].Value = choices[0].parts.Count.ToString();
            int i = 0;
            foreach (RuleDefinitionPart Part in parts)
            {
                node.AppendChild(Part.GetXMLNode(document));
                if ((Part.Symbol is Terminal) || (Part.Symbol is Variable))
                {
                    node.LastChild.Attributes["ParserIndex"].Value = i.ToString();
                    i++;
                }
            }
            return node;
        }

        public static CFRuleDefinition operator +(CFRuleDefinition left, CFRuleDefinition right)
        {
            CFRuleDefinition Result = new CFRuleDefinition();
            Result.parts.AddRange(left.parts);
            Result.parts.AddRange(right.parts);
            return Result;
        }
    }
}