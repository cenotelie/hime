/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;

namespace Hime.Parsers.ContextFree
{
    class TemplateRule
    {
        private string headName;
        private List<string> parameters;
        private List<TemplateRuleInstance> instances;
        private CFGrammar grammar;
        private CFGrammarCompiler compiler;
        private Redist.Parsers.SyntaxTreeNode ruleNode;
        private Redist.Parsers.SyntaxTreeNode definitionNode;

        public string HeadName { get { return headName; } }
        public int ParametersCount { get { return parameters.Count; } }
        public List<string> Parameters { get { return parameters; } }
        public CFGrammarCompiler Compiler { get { return compiler; } }
        public Redist.Parsers.SyntaxTreeNode RuleNode { get { return ruleNode; } }
        public Redist.Parsers.SyntaxTreeNode DefinitionNode { get { return definitionNode; } }

        public TemplateRule(CFGrammar grammar, CFGrammarCompiler compiler, Redist.Parsers.SyntaxTreeNode ruleNode)
        {
            this.headName = ((Redist.Parsers.SymbolTokenText)ruleNode.Children[0].Symbol).ValueText;
            this.parameters = new List<string>();
            this.instances = new List<TemplateRuleInstance>();
            this.grammar = grammar;
            this.compiler = compiler;
            this.ruleNode = ruleNode;
            this.definitionNode = ruleNode.Children[2];
            foreach (Redist.Parsers.SyntaxTreeNode Node in ruleNode.Children[1].Children)
                this.parameters.Add(((Redist.Parsers.SymbolTokenText)Node.Symbol).ValueText);
        }

        public TemplateRule(TemplateRule copied, CFGrammar data)
        {
            headName = copied.headName;
            parameters = new List<string>(copied.parameters);
            instances = new List<TemplateRuleInstance>();
            grammar = data;
            compiler = copied.compiler;
            ruleNode = copied.ruleNode;
            definitionNode = copied.definitionNode;
            foreach (TemplateRuleInstance instance in copied.instances)
            {
                Variable headVar = data.GetVariable(instance.HeadVariable.LocalName);
                TemplateRuleParameter Params = new TemplateRuleParameter();
                foreach (Symbol symbol in instance.Parameters)
                    Params.Add(data.GetSymbol(symbol.LocalName));
                instances.Add(new TemplateRuleInstance(this, Params, headVar));
            }
        }

        public Variable GetVariable(CompilerContext context, TemplateRuleParameter parameters)
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
