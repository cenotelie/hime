/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;

namespace Hime.Parsers.ContextFree
{
    class CompilerContext
    {
        private CFGrammarCompiler compiler;
        private List<TemplateRule> templateRules;
        private Dictionary<string, Symbol> references;

        public CFGrammarCompiler Compiler { get { return compiler; } }

        public CompilerContext(CFGrammarCompiler compiler)
        {
            this.compiler = compiler;
            this.templateRules = new List<TemplateRule>();
            this.references = new Dictionary<string, Symbol>();
        }

        public CompilerContext(CompilerContext copied)
        {
            compiler = copied.Compiler;
            templateRules = new List<TemplateRule>(copied.templateRules);
            references = new Dictionary<string, Symbol>();
        }

        public void AddTemplateRule(TemplateRule templateRule) { templateRules.Add(templateRule); }

        public bool IsTemplateRule(string name, int paramCount)
        {
            foreach (TemplateRule tRule in templateRules)
                if ((tRule.HeadName == name) && (tRule.ParametersCount == paramCount))
                    return true;
            return false;
        }

        public Variable GetVariableFromMetaRule(string name, TemplateRuleParameter parameters, CompilerContext context)
        {
            foreach (TemplateRule tRule in templateRules)
            {
                if ((tRule.HeadName == name) && (tRule.ParametersCount == parameters.Count))
                    return tRule.GetVariable(context, parameters);
            }
            return null;
        }

        public void AddReference(string name, Symbol symbol) { references.Add(name, symbol); }
        public bool IsReference(string name) { return references.ContainsKey(name); }
        public Symbol GetReference(string name)
        {
            if (references.ContainsKey(name))
                return references[name];
            return null;
        }
    }
}
