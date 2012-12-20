/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;
using Hime.Redist.AST;

namespace Hime.CentralDogma.Grammars.ContextFree
{
    class TemplateRule
    {
        private List<TemplateRuleInstance> instances;
        private CFGrammar grammar;

        internal string HeadName { get; private set; }
        internal List<string> Parameters { get; private set; }
        internal int ParametersCount { get { return this.Parameters.Count; } }
		internal CSTNode RuleNode { get; private set; }
        internal CSTNode DefinitionNode { get; private set; }

        public TemplateRule(CFGrammar grammar, CSTNode ruleNode)
        {
            this.HeadName = ((Hime.Redist.Symbols.TextToken)ruleNode.Children[0].Symbol).ValueText;
            this.Parameters = new List<string>();
            this.instances = new List<TemplateRuleInstance>();
            this.grammar = grammar;
            this.RuleNode = ruleNode;
            this.DefinitionNode = ruleNode.Children[2];
            foreach (CSTNode Node in ruleNode.Children[1].Children)
			{
                this.Parameters.Add(((Hime.Redist.Symbols.TextToken)Node.Symbol).ValueText);
			}
        }

        public TemplateRule(TemplateRule copied, CFGrammar data)
        {
            this.HeadName = copied.HeadName;
            this.Parameters = new List<string>(copied.Parameters);
            instances = new List<TemplateRuleInstance>();
            grammar = data;
            this.RuleNode = copied.RuleNode;
            this.DefinitionNode = copied.DefinitionNode;
            foreach (TemplateRuleInstance instance in copied.instances)
            {
                CFVariable headVar = data.GetCFVariable(instance.HeadVariable.Name);
                List<Symbol> Params = new List<Symbol>();
                foreach (Symbol symbol in instance.Parameters)
                    Params.Add(data.GetSymbol(symbol.Name));
                instances.Add(new TemplateRuleInstance(this, Params, headVar));
            }
        }

        public Variable GetVariable(CompilerContext context, List<Symbol> parameters)
        {
            foreach (TemplateRuleInstance instance in instances)
            {
                if (instance.MatchParameters(parameters))
                    return instance.HeadVariable;
            }
            TemplateRuleInstance newInstance = new TemplateRuleInstance(this, parameters, grammar);
            instances.Add(newInstance);
            newInstance.Compile(grammar, context);
            return newInstance.HeadVariable;
        }
    }
}
