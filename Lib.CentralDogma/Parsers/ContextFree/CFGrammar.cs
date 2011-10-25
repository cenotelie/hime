/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;
using Hime.Kernel.Reporting;
using System.Xml;

namespace Hime.Parsers.ContextFree
{
    public abstract class CFGrammar : Grammar
    {
        internal List<TemplateRule> templateRules;

        internal ICollection<TemplateRule> TemplateRules { get { return templateRules; } }

        public CFGrammar(string name)
            : base(name)
        {
            templateRules = new List<TemplateRule>();
        }

        protected void InheritTemplateRules(CFGrammar parent)
        {
            foreach (TemplateRule tRule in parent.TemplateRules)
                templateRules.Add(new TemplateRule(tRule, this));
        }

        protected void InheritVariables(CFGrammar parent)
        {
            foreach (CFVariable variable in parent.Variables)
                AddCFVariable(variable.LocalName);
            foreach (CFVariable variable in parent.Variables)
            {
                CFVariable clone = variables[variable.LocalName] as CFVariable;
                foreach (CFRule rule in variable.Rules)
                {
                    List<RuleBodyElement> parts = new List<RuleBodyElement>();
                    foreach (RuleBodyElement part in rule.CFBody.Parts)
                    {
                        GrammarSymbol symbol = null;
                        if (part.Symbol is Variable)
                            symbol = variables[part.Symbol.LocalName];
                        else if (part.Symbol is Terminal)
                            symbol = terminals[part.Symbol.LocalName];
                        else if (part.Symbol is Virtual)
                            symbol = virtuals[part.Symbol.LocalName];
                        else if (part.Symbol is Action)
                            symbol = actions[part.Symbol.LocalName];
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
            CFVariable var = new CFVariable(this, nextSID, name);
            children.Add(name, var);
            variables.Add(name, var);
            nextSID++;
            return var;
        }

        public override Variable NewVariable() { return NewCFVariable(); }
        public CFVariable NewCFVariable()
        {
            string varName = "_v" + nextSID;
            CFVariable var = new CFVariable(this, nextSID, varName);
            children.Add(varName, var);
            variables.Add(varName, var);
            nextSID++;
            return var;
        }

        public override Rule CreateRule(Variable head, List<RuleBodyElement> body)
        {
            return new CFRule(head as CFVariable, new CFRuleBody(body), false);
        }

        internal TemplateRule AddTemplateRule(Redist.Parsers.SyntaxTreeNode ruleNode)
        {
            TemplateRule Rule = new TemplateRule(this, ruleNode);
            templateRules.Add(Rule);
            return Rule;
        }

        protected bool AddRealAxiom(Reporter reporter)
        {
            reporter.Info("Grammar", "Creating axiom ...");

            // Search for Axiom option
            if (!options.ContainsKey("Axiom"))
            {
                reporter.Error("Grammar", "Axiom option is undefined");
                return false;
            }
            // Search for the variable specified as the Axiom
            string name = options["Axiom"];
            if (!variables.ContainsKey(name))
            {
                reporter.Error("Grammar", "Cannot find axiom variable " + name);
                return false;
            }

            // Create the real axiom rule variable and rule
            CFVariable axiom = AddCFVariable("_Axiom_");
            List<RuleBodyElement> parts = new List<RuleBodyElement>();
            parts.Add(new RuleBodyElement(variables[name], RuleBodyElementAction.Promote));
            parts.Add(new RuleBodyElement(TerminalDollar.Instance, RuleBodyElementAction.Drop));
            axiom.AddRule(new CFRule(axiom, new CFRuleBody(parts), false));

            reporter.Info("Grammar", "Done !");
            return true;
        }

        protected bool ComputeFirsts(Reporter reporter)
        {
            reporter.Info("Grammar", "Computing Firsts sets ...");

            bool mod = true;
            // While some modification has occured, repeat the process
            while (mod)
            {
                mod = false;
                foreach (CFVariable Var in variables.Values)
                    if (Var.ComputeFirsts())
                        mod = true;
            }

            reporter.Info("Grammar", "Done !");
            return true;
        }

        protected bool ComputeFollowers(Reporter reporter)
        {
            reporter.Info("Grammar", "Computing Followers sets ...");

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

            reporter.Info("Grammar", "Done !");
            return true;
        }

        public override ParserData GetParserData(Reporter reporter, ParserGenerator generator)
        {
            AddRealAxiom(reporter);
            ComputeFirsts(reporter);
            ComputeFollowers(reporter);
            return generator.Build(this, reporter);
        }
    }
}
