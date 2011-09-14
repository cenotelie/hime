/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;

namespace Hime.Parsers.ContextFree
{
    class CFGrammarTemplateRule
    {
        private string headName;
        private List<string> parameters;
        private List<CFGrammarTemplateRuleInstance> instances;
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

        public CFGrammarTemplateRule(CFGrammar Grammar, CFGrammarCompiler Compiler, Redist.Parsers.SyntaxTreeNode RuleNode)
        {
            headName = ((Redist.Parsers.SymbolTokenText)RuleNode.Children[0].Symbol).ValueText;
            parameters = new List<string>();
            instances = new List<CFGrammarTemplateRuleInstance>();
            grammar = Grammar;
            compiler = Compiler;
            ruleNode = RuleNode;
            definitionNode = RuleNode.Children[2];
            foreach (Redist.Parsers.SyntaxTreeNode Node in RuleNode.Children[1].Children)
                parameters.Add(((Redist.Parsers.SymbolTokenText)Node.Symbol).ValueText);
        }

        public CFGrammarTemplateRule(CFGrammarTemplateRule Copied, CFGrammar Data)
        {
            headName = Copied.headName;
            parameters = new List<string>(Copied.parameters);
            instances = new List<CFGrammarTemplateRuleInstance>();
            grammar = Data;
            compiler = Copied.compiler;
            ruleNode = Copied.ruleNode;
            definitionNode = Copied.definitionNode;
            foreach (CFGrammarTemplateRuleInstance Instance in Copied.instances)
            {
                Variable HeadVar = Data.GetVariable(Instance.HeadVariable.LocalName);
                CFGrammarTemplateRuleParameters Params = new CFGrammarTemplateRuleParameters();
                foreach (Symbol Symbol in Instance.Parameters)
                    Params.Add(Data.GetSymbol(Symbol.LocalName));
                instances.Add(new CFGrammarTemplateRuleInstance(this, Params, HeadVar));
            }
        }

        public Variable GetVariable(CFGrammarCompilerContext Context, CFGrammarTemplateRuleParameters Parameters)
        {
            foreach (CFGrammarTemplateRuleInstance Instance in instances)
            {
                if (Instance.MatchParameters(Parameters))
                    return Instance.HeadVariable;
            }
            CFGrammarTemplateRuleInstance NewInstance = new CFGrammarTemplateRuleInstance(this, Parameters, grammar);
            instances.Add(NewInstance);
            NewInstance.Compile(grammar, Context);
            return NewInstance.HeadVariable;
        }
    }

    class CFGrammarTemplateRuleInstance
    {
        private CFGrammarTemplateRule templateRule;
        private Variable variable;
        private CFGrammarTemplateRuleParameters parameters;

        public Variable HeadVariable { get { return variable; } }
        public CFGrammarTemplateRuleParameters Parameters { get { return parameters; } }

        public CFGrammarTemplateRuleInstance(CFGrammarTemplateRule TemplateRule, CFGrammarTemplateRuleParameters Parameters, CFGrammar Data)
        {
            // Build the head variable name
            System.Text.StringBuilder Builder = new System.Text.StringBuilder();
            Builder.Append(TemplateRule.HeadName);
            Builder.Append("<");
            for (int i = 0; i != Parameters.Count; i++)
            {
                if (i != 0) Builder.Append(", ");
                Builder.Append(Parameters[i].LocalName);
            }
            Builder.Append(">");
            string Name = Builder.ToString();
            // Create and add the variable to the grammar
            variable = Data.AddVariable(Name);
            // Copy parameters
            parameters = new CFGrammarTemplateRuleParameters(Parameters);
            // Set parent template rule
            templateRule = TemplateRule;
        }
        public CFGrammarTemplateRuleInstance(CFGrammarTemplateRule TemplateRule, CFGrammarTemplateRuleParameters Parameters, Variable Variable)
        {
            templateRule = TemplateRule;
            parameters = Parameters;
            variable = Variable;
        }

        public void Compile(CFGrammar Data, CFGrammarCompilerContext Context)
        {
            // Create a new context for recognizing the rule
            CFGrammarCompilerContext NewContext = new CFGrammarCompilerContext(Context);
            // Add the parameters as references in the new context
            for (int i = 0; i != parameters.Count; i++)
                NewContext.AddReference(templateRule.Parameters[i], parameters[i]);
            // Recognize the rule with the new context
            CFRuleDefinitionSet Set = NewContext.Compiler.Compile_Recognize_rule_definition(Data, NewContext, templateRule.DefinitionNode);
            // Add recognized rules to the variable
            foreach (CFRuleDefinition Def in Set)
                variable.AddRule(new CFRule(variable, Def, false));
        }

        public bool MatchParameters(CFGrammarTemplateRuleParameters Parameters)
        {
            if (parameters.Count != Parameters.Count)
                return false;
            for (int i = 0; i != parameters.Count; i++)
            {
                if (parameters[i].SID != Parameters[i].SID)
                    return false;
            }
            return true;
        }
    }

    class CFGrammarTemplateRuleParameters : List<Symbol>
    {
        public CFGrammarTemplateRuleParameters() : base() { }
        public CFGrammarTemplateRuleParameters(ICollection<Symbol> Symbols) : base(Symbols) { }
    }

    class CFGrammarCompilerContext
    {
        private CFGrammarCompiler compiler;
        private List<CFGrammarTemplateRule> templateRules;
        private Dictionary<string, Symbol> references;

        public CFGrammarCompiler Compiler { get { return compiler; } }

        public CFGrammarCompilerContext(CFGrammarCompiler Compiler)
        {
            compiler = Compiler;
            templateRules = new List<CFGrammarTemplateRule>();
            references = new Dictionary<string, Symbol>();
        }

        public CFGrammarCompilerContext(CFGrammarCompilerContext Copied)
        {
            compiler = Copied.Compiler;
            templateRules = new List<CFGrammarTemplateRule>(Copied.templateRules);
            references = new Dictionary<string, Symbol>();
        }

        public void AddTemplateRule(CFGrammarTemplateRule TemplateRule) { templateRules.Add(TemplateRule); }

        public bool IsTemplateRule(string Name, int ParamCount)
        {
            foreach (CFGrammarTemplateRule TemplateRule in templateRules)
                if ((TemplateRule.HeadName == Name) && (TemplateRule.ParametersCount == ParamCount))
                    return true;
            return false;
        }

        public Variable GetVariableFromMetaRule(string Name, CFGrammarTemplateRuleParameters Parameters, CFGrammarCompilerContext Context)
        {
            foreach (CFGrammarTemplateRule TemplateRule in templateRules)
            {
                if ((TemplateRule.HeadName == Name) && (TemplateRule.ParametersCount == Parameters.Count))
                    return TemplateRule.GetVariable(Context, Parameters);
            }
            return null;
        }

        public void AddReference(string Name, Symbol Symbol) { references.Add(Name, Symbol); }
        public bool IsReference(string Name) { return references.ContainsKey(Name); }
        public Symbol GetReference(string Name)
        {
            if (references.ContainsKey(Name))
                return references[Name];
            return null;
        }
    }
}
