/**********************************************************************
* Copyright (c) 2013 Laurent Wouters and others
* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU Lesser General Public License as
* published by the Free Software Foundation, either version 3
* of the License, or (at your option) any later version.
* 
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU Lesser General Public License for more details.
* 
* You should have received a copy of the GNU Lesser General
* Public License along with this program.
* If not, see <http://www.gnu.org/licenses/>.
* 
* Contributors:
*     Laurent Wouters - lwouters@xowl.org
**********************************************************************/

using System.Collections.Generic;
using Hime.Redist;

namespace Hime.CentralDogma.Grammars.ContextFree
{
    class CFGrammar : Grammar
    {
        internal List<TemplateRule> templateRules;

        internal ICollection<TemplateRule> TemplateRules { get { return templateRules; } }

        public CFGrammar(string name)
            : base(name)
        {
            templateRules = new List<TemplateRule>();
        }

        public override void Inherit(Grammar parent)
        {
            InheritOptions(parent);
            InheritActions(parent);
            InheritVirtuals(parent);
            InheritTerminals(parent);
            InheritVariables(parent as CFGrammar);
            InheritTemplateRules(parent as CFGrammar);
        }

        protected void InheritTemplateRules(CFGrammar parent)
        {
            foreach (TemplateRule tRule in parent.TemplateRules)
                templateRules.Add(new TemplateRule(tRule, this));
        }

        protected void InheritVariables(CFGrammar parent)
        {
            foreach (CFVariable variable in parent.Variables)
                AddCFVariable(variable.Name);
            foreach (CFVariable variable in parent.Variables)
            {
                CFVariable clone = variables[variable.Name] as CFVariable;
                foreach (CFRule rule in variable.Rules)
                {
                    List<RuleBodyElement> parts = new List<RuleBodyElement>();
                    foreach (RuleBodyElement part in rule.CFBody.Parts)
                    {
                        Symbol symbol = null;
                        if (part.Symbol is Variable)
                            symbol = variables[part.Symbol.Name];
                        else if (part.Symbol is Terminal)
                        {
                            // If this is a named terminal, just pick the one with the same name
                            if (terminalsByName.ContainsKey(part.Symbol.Name))
                                symbol = terminalsByName[part.Symbol.Name];
                            else // this is an inline terminal, pick the one with the same value
                                symbol = terminalsByValue[(part.Symbol as TextTerminal).Value];
                        }
                        else if (part.Symbol is Virtual)
                            symbol = virtuals[part.Symbol.Name];
                        else if (part.Symbol is Action)
                            symbol = actions[part.Symbol.Name];
                        parts.Add(new RuleBodyElement(symbol, part.Action));
                    }
                    clone.AddRule(new CFRule(clone, new CFRuleBody(parts), rule.ReplaceOnProduction));
                }
            }
        }

        public CFVariable GetCFVariable(string name)
        {
            if (!variables.ContainsKey(name))
                return null;
            return variables[name] as CFVariable;
        }

        public override Variable AddVariable(string name) { return AddCFVariable(name); }
        public CFVariable AddCFVariable(string name)
        {
            if (variables.ContainsKey(name))
                return variables[name] as CFVariable;
            CFVariable var = new CFVariable(nextSID, name);
            variables.Add(name, var);
            nextSID++;
            return var;
        }

        public override Variable NewVariable() { return NewCFVariable(); }
        public CFVariable NewCFVariable()
        {
            string varName = "_v" + GenerateID();
            CFVariable var = new CFVariable(nextSID, varName);
            variables.Add(varName, var);
            nextSID++;
            return var;
        }

        public override Rule CreateRule(Variable head, List<RuleBodyElement> body)
        {
            return new CFRule(head as CFVariable, new CFRuleBody(body), false);
        }

        public TemplateRule AddTemplateRule(ASTNode ruleNode)
        {
            TemplateRule Rule = new TemplateRule(this, ruleNode);
            templateRules.Add(Rule);
            return Rule;
        }

        private Automata.DFA PrepareDFA(Reporting.Reporter log)
        {
            log.Info("Generating DFA for Terminals ...");

            // Construct a global NFA for all the terminals
            Automata.NFA final = Automata.NFA.NewMinimal();
            foreach (TextTerminal terminal in terminalsByName.Values)
            {
                Automata.NFA sub = terminal.NFA.Clone();
                final.InsertSubNFA(sub);
                final.StateEntry.AddTransition(Automata.NFA.Epsilon, sub.StateEntry);
            }
            // Construct the equivalent DFA and minimize it
            Automata.DFA finalDFA = new Automata.DFA(final);
            finalDFA = finalDFA.Minimize();
            finalDFA.RepackTransitions();

            log.Info("Done !");
            return finalDFA;
        }

        public override LexerData GetLexerData(Reporting.Reporter reporter)
        {
            Automata.DFA finalDFA = PrepareDFA(reporter);
            Terminal separator = null;
            if (options.ContainsKey("Separator"))
                separator = terminalsByName[options["Separator"]];
            return new TextLexerData(finalDFA, separator);
        }

        protected bool AddRealAxiom(Reporting.Reporter reporter)
        {
            reporter.Info("Creating axiom ...");

            // Search for Axiom option
            if (!options.ContainsKey("Axiom"))
            {
                reporter.Error("No axiom variable has been defined for grammar " + this.name);
                return false;
            }
            // Search for the variable specified as the Axiom
            string name = options["Axiom"];
            if (!variables.ContainsKey(name))
            {
                reporter.Error("The specified axiom variable " + name + " is undefined");
                return false;
            }

            // Create the real axiom rule variable and rule
            CFVariable axiom = AddCFVariable("_Axiom_");
            List<RuleBodyElement> parts = new List<RuleBodyElement>();
            parts.Add(new RuleBodyElement(variables[name], RuleBodyElementAction.Promote));
            parts.Add(new RuleBodyElement(Dollar.Instance, RuleBodyElementAction.Drop));
            axiom.AddRule(new CFRule(axiom, new CFRuleBody(parts), false));

            reporter.Info("Done !");
            return true;
        }

        protected bool ComputeFirsts(Reporting.Reporter reporter)
        {
            reporter.Info("Computing Firsts sets ...");

            bool mod = true;
            // While some modification has occured, repeat the process
            while (mod)
            {
                mod = false;
                foreach (CFVariable Var in variables.Values)
                    if (Var.ComputeFirsts())
                        mod = true;
            }

            reporter.Info("Done !");
            return true;
        }

        protected bool ComputeFollowers(Reporting.Reporter reporter)
        {
            reporter.Info("Computing Followers sets ...");

            bool mod = true;
            // Apply step 1 to each variable
            foreach (CFVariable Var in variables.Values)
                Var.ComputeFollowers_Step1();
            // Apply step 2 and 3 while some modification has occured
            while (mod)
            {
                mod = false;
                foreach (CFVariable Var in variables.Values)
                    if (Var.ComputeFollowers_Step23())
                        mod = true;
            }

            reporter.Info("Done !");
            return true;
        }

        public override ParserData GetParserData(Reporting.Reporter reporter, ParserGenerator generator)
        {
            AddRealAxiom(reporter);
            ComputeFirsts(reporter);
            ComputeFollowers(reporter);
            return generator.Build(this);
        }
    }
}
