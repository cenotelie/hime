namespace Hime.Parsers.CF
{
    public class CFGrammarTemplateRule
    {
        private string p_HeadName;
        private System.Collections.Generic.List<string> p_Parameters;
        private System.Collections.Generic.List<CFGrammarTemplateRuleInstance> p_Instances;
        private CFGrammar p_Grammar;
        private CFGrammarCompiler p_Compiler;
        private Hime.Kernel.Parsers.SyntaxTreeNode p_RuleNode;
        private Hime.Kernel.Parsers.SyntaxTreeNode p_DefinitionNode;

        public string HeadName { get { return p_HeadName; } }
        public int ParametersCount { get { return p_Parameters.Count; } }
        public System.Collections.Generic.List<string> Parameters { get { return p_Parameters; } }
        public CFGrammarCompiler Compiler { get { return p_Compiler; } }
        public Hime.Kernel.Parsers.SyntaxTreeNode RuleNode { get { return p_RuleNode; } }
        public Hime.Kernel.Parsers.SyntaxTreeNode DefinitionNode { get { return p_DefinitionNode; } }

        public CFGrammarTemplateRule(CFGrammar Grammar, CFGrammarCompiler Compiler, Hime.Kernel.Parsers.SyntaxTreeNode RuleNode)
        {
            p_HeadName = ((Hime.Kernel.Parsers.SymbolTokenText)RuleNode.Children[0].Symbol).ValueText;
            p_Parameters = new System.Collections.Generic.List<string>();
            p_Instances = new System.Collections.Generic.List<CFGrammarTemplateRuleInstance>();
            p_Grammar = Grammar;
            p_Compiler = Compiler;
            p_RuleNode = RuleNode;
            p_DefinitionNode = RuleNode.Children[2];
            foreach (Hime.Kernel.Parsers.SyntaxTreeNode Node in RuleNode.Children[1].Children)
                p_Parameters.Add(((Hime.Kernel.Parsers.SymbolTokenText)Node.Symbol).ValueText);
        }

        public CFGrammarTemplateRule(CFGrammarTemplateRule Copied, CFGrammar Data)
        {
            p_HeadName = Copied.p_HeadName;
            p_Parameters = new System.Collections.Generic.List<string>(Copied.p_Parameters);
            p_Instances = new System.Collections.Generic.List<CFGrammarTemplateRuleInstance>();
            p_Grammar = Data;
            p_Compiler = Copied.p_Compiler;
            p_RuleNode = Copied.p_RuleNode;
            p_DefinitionNode = Copied.p_DefinitionNode;
            foreach (CFGrammarTemplateRuleInstance Instance in Copied.p_Instances)
            {
                CFVariable HeadVar = Data.GetVariable(Instance.HeadVariable.LocalName);
                CFGrammarTemplateRuleParameters Params = new CFGrammarTemplateRuleParameters();
                foreach (Symbol Symbol in Instance.Parameters)
                    Params.Add(Data.GetSymbol(Symbol.LocalName));
                p_Instances.Add(new CFGrammarTemplateRuleInstance(this, Params, HeadVar));
            }
        }

        public CFVariable GetVariable(CFGrammarCompilerContext Context, CFGrammarTemplateRuleParameters Parameters)
        {
            foreach (CFGrammarTemplateRuleInstance Instance in p_Instances)
            {
                if (Instance.MatchParameters(Parameters))
                    return Instance.HeadVariable;
            }
            CFGrammarTemplateRuleInstance NewInstance = new CFGrammarTemplateRuleInstance(this, Parameters, p_Grammar);
            p_Instances.Add(NewInstance);
            NewInstance.Compile(p_Grammar, Context);
            return NewInstance.HeadVariable;
        }
    }

    public class CFGrammarTemplateRuleInstance
    {
        private CFGrammarTemplateRule p_TemplateRule;
        private CFVariable p_Variable;
        private CFGrammarTemplateRuleParameters p_Parameters;

        public CFVariable HeadVariable { get { return p_Variable; } }
        public CFGrammarTemplateRuleParameters Parameters { get { return p_Parameters; } }

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
            p_Variable = Data.AddVariable(Name);
            // Copy parameters
            p_Parameters = new CFGrammarTemplateRuleParameters(Parameters);
            // Set parent template rule
            p_TemplateRule = TemplateRule;
        }
        public CFGrammarTemplateRuleInstance(CFGrammarTemplateRule TemplateRule, CFGrammarTemplateRuleParameters Parameters, CFVariable Variable)
        {
            p_TemplateRule = TemplateRule;
            p_Parameters = Parameters;
            p_Variable = Variable;
        }

        public void Compile(CFGrammar Data, CFGrammarCompilerContext Context)
        {
            // Create a new context for recognizing the rule
            CFGrammarCompilerContext NewContext = new CFGrammarCompilerContext(Context);
            // Add the parameters as references in the new context
            for (int i = 0; i != p_Parameters.Count; i++)
                NewContext.AddReference(p_TemplateRule.Parameters[i], p_Parameters[i]);
            // Recognize the rule with the new context
            CFRuleDefinitionSet Set = NewContext.Compiler.Compile_Recognize_rule_definition(Data, NewContext, p_TemplateRule.DefinitionNode);
            // Add recognized rules to the variable
            foreach (CFRuleDefinition Def in Set)
                p_Variable.AddRule(new CFRule(p_Variable, Def, false));
        }

        public bool MatchParameters(CFGrammarTemplateRuleParameters Parameters)
        {
            if (p_Parameters.Count != Parameters.Count)
                return false;
            for (int i = 0; i != p_Parameters.Count; i++)
            {
                if (p_Parameters[i].SID != Parameters[i].SID)
                    return false;
            }
            return true;
        }
    }

    public class CFGrammarTemplateRuleParameters : System.Collections.Generic.List<Symbol>
    {
        public CFGrammarTemplateRuleParameters() : base() { }
        public CFGrammarTemplateRuleParameters(System.Collections.Generic.IEnumerable<Symbol> Symbols) : base(Symbols) { }
    }

    public class CFGrammarCompilerContext
    {
        private CFGrammarCompiler p_Compiler;
        private System.Collections.Generic.List<CFGrammarTemplateRule> p_TemplateRules;
        private System.Collections.Generic.Dictionary<string, Symbol> p_References;

        public CFGrammarCompiler Compiler { get { return p_Compiler; } }

        public CFGrammarCompilerContext(CFGrammarCompiler Compiler)
        {
            p_Compiler = Compiler;
            p_TemplateRules = new System.Collections.Generic.List<CFGrammarTemplateRule>();
            p_References = new System.Collections.Generic.Dictionary<string, Symbol>();
        }

        public CFGrammarCompilerContext(CFGrammarCompilerContext Copied)
        {
            p_Compiler = Copied.Compiler;
            p_TemplateRules = new System.Collections.Generic.List<CFGrammarTemplateRule>(Copied.p_TemplateRules);
            p_References = new System.Collections.Generic.Dictionary<string, Symbol>();
        }

        public void AddTemplateRule(CFGrammarTemplateRule TemplateRule) { p_TemplateRules.Add(TemplateRule); }

        public bool IsTemplateRule(string Name, int ParamCount)
        {
            foreach (CFGrammarTemplateRule TemplateRule in p_TemplateRules)
                if ((TemplateRule.HeadName == Name) && (TemplateRule.ParametersCount == ParamCount))
                    return true;
            return false;
        }

        public CFVariable GetVariableFromMetaRule(string Name, CFGrammarTemplateRuleParameters Parameters, CFGrammarCompilerContext Context)
        {
            foreach (CFGrammarTemplateRule TemplateRule in p_TemplateRules)
            {
                if ((TemplateRule.HeadName == Name) && (TemplateRule.ParametersCount == Parameters.Count))
                    return TemplateRule.GetVariable(Context, Parameters);
            }
            return null;
        }

        public void AddReference(string Name, Symbol Symbol) { p_References.Add(Name, Symbol); }
        public bool IsReference(string Name) { return p_References.ContainsKey(Name); }
        public Symbol GetReference(string Name)
        {
            if (p_References.ContainsKey(Name))
                return p_References[Name];
            return null;
        }
    }
}