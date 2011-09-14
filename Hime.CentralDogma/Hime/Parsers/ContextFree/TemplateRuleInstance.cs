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
        private Variable variable;
        private TemplateRuleParameter parameters;

        public Variable HeadVariable { get { return variable; } }
        public TemplateRuleParameter Parameters { get { return parameters; } }

        public TemplateRuleInstance(TemplateRule tRule, TemplateRuleParameter parameters, CFGrammar data)
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
            variable = data.AddVariable(name);
            // Copy parameters
            parameters = new TemplateRuleParameter(parameters);
            // Set parent template rule
            templateRule = tRule;
        }
        public TemplateRuleInstance(TemplateRule tRule, TemplateRuleParameter parameters, Variable variable)
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
            CFRuleDefinitionSet set = newContext.Compiler.Compile_Recognize_rule_definition(data, newContext, templateRule.DefinitionNode);
            // Add recognized rules to the variable
            foreach (CFRuleDefinition def in set)
                variable.AddRule(new CFRule(variable, def, false));
        }

        public bool MatchParameters(TemplateRuleParameter parameters)
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