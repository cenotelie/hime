/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;
using System.Xml;

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
                            symbol = terminalsByName[part.Symbol.Name];
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

        public TemplateRule AddTemplateRule(Redist.AST.CSTNode ruleNode)
        {
            TemplateRule Rule = new TemplateRule(this, ruleNode);
            templateRules.Add(Rule);
            return Rule;
        }

        private Automata.DFA PrepareDFA(Reporting.Reporter log)
        {
            log.Info("Generating DFA for Terminals ...");

            // Construct a global NFA for all the terminals
            Automata.NFA final = new Automata.NFA();
            final.StateEntry = final.AddNewState();
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
                reporter.Error("Axiom option is undefined");
                return false;
            }
            // Search for the variable specified as the Axiom
            string name = options["Axiom"];
            if (!variables.ContainsKey(name))
            {
                reporter.Error("Cannot find axiom variable " + name);
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
