/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;

namespace Hime.Parsers.ContextFree
{
    class TemplateRuleInstance
    {
        private TemplateRule templateRule;
        private CFVariable variable;
        private List<GrammarSymbol> parameters;

        public Variable HeadVariable { get { return variable; } }
        public List<GrammarSymbol> Parameters { get { return parameters; } }

        public TemplateRuleInstance(TemplateRule tRule, List<GrammarSymbol> parameters, CFGrammar data)
        {
            // Build the head variable name
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            builder.Append(tRule.HeadName);
            builder.Append("<");
            for (int i = 0; i != parameters.Count; i++)
            {
                if (i != 0) builder.Append(", ");
                builder.Append(parameters[i].LocalName);
            }
            builder.Append(">");
            string name = builder.ToString();
            // Create and add the variable to the grammar
            this.variable = data.AddCFVariable(name);
            // Copy parameters
            this.parameters = new List<GrammarSymbol>(parameters);
            // Set parent template rule
            this.templateRule = tRule;
        }
        public TemplateRuleInstance(TemplateRule tRule, List<GrammarSymbol> parameters, CFVariable variable)
        {
            this.templateRule = tRule;
            this.parameters = parameters;
            this.variable = variable;
        }

        public void Compile(CFGrammar data, CompilerContext context)
        {
            // Create a new context for recognizing the rule
            CompilerContext newContext = new CompilerContext(context);
            // Add the parameters as references in the new context
            for (int i = 0; i != parameters.Count; i++)
                newContext.AddReference(templateRule.Parameters[i], parameters[i]);
            // Recognize the rule with the new context
            CFRuleBodySet set = newContext.Compiler.Compile_Recognize_rule_definition(data, newContext, templateRule.DefinitionNode);
            // Add recognized rules to the variable
            foreach (CFRuleBody def in set)
                variable.AddRule(new CFRule(variable, def, false));
        }

        public bool MatchParameters(List<GrammarSymbol> parameters)
        {
            if (parameters.Count != parameters.Count)
                return false;
            for (int i = 0; i != parameters.Count; i++)
            {
                if (parameters[i].SID != parameters[i].SID)
                    return false;
            }
            return true;
        }
    }
}