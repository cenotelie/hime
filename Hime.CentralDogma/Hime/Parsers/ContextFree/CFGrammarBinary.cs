/*
 * Author: Charles Hymans
 * Date: 18/07/2011
 * Time: 23:19
 * 
 */
using System;
using System.Collections.Generic;

namespace Hime.Parsers.ContextFree
{
    public sealed class CFGrammarBinary : CFGrammar
    {
        public CFGrammarBinary(string name) : base(name) { }

        public override void Inherit(CFGrammar parent)
        {
        	base.Inherit(parent);
            foreach (TerminalBin terminal in parent.Terminals)
                AddTerminalBin(terminal.Type, terminal.LocalName);
            foreach (Variable variable in parent.Variables)
                AddVariable(variable.LocalName);
            foreach (Virtual vir in parent.Virtuals)
                AddVirtual(vir.LocalName);
            foreach (Action action in parent.Actions)
                AddAction(action.LocalName);
            foreach (TemplateRule tRule in parent.TemplateRules)
                templateRules.Add(new TemplateRule(tRule, this));
            foreach (Variable variable in parent.Variables)
            {
                Variable clone = variables[variable.LocalName];
                foreach (CFRule rule in variable.Rules)
                {
                    List<RuleDefinitionPart> parts = new List<RuleDefinitionPart>();
                    CFRuleDefinition definition = new CFRuleDefinition();
                    foreach (RuleDefinitionPart part in rule.Definition.Parts)
                    {
                        Symbol symbol = null;
                        if (part.Symbol is Variable)
                            symbol = variables[part.Symbol.LocalName];
                        else if (part.Symbol is Terminal)
                            symbol = terminals[part.Symbol.LocalName];
                        else if (part.Symbol is Virtual)
                            symbol = virtuals[part.Symbol.LocalName];
                        else if (part.Symbol is Action)
                            symbol = actions[part.Symbol.LocalName];
                        parts.Add(new RuleDefinitionPart(symbol, part.Action));
                    }
                    clone.AddRule(new CFRule(clone, new CFRuleDefinition(parts), rule.ReplaceOnProduction));
                }
            }
        }
        // TODO: factor code with CFGrammarText
        public override CFGrammar Clone()
        {
            CFGrammar result = new CFGrammarBinary(name);
            result.Inherit(this);
            return result;
        }

        public override bool Build(CompilationTask options)
        {
            options.Reporter.BeginSection(name + " parser data generation");
            if (!Prepare_AddRealAxiom(options.Reporter)) { options.Reporter.EndSection(); return false; }
            if (!Prepare_ComputeFirsts(options.Reporter)) { options.Reporter.EndSection(); return false; }
            if (!Prepare_ComputeFollowers(options.Reporter)) { options.Reporter.EndSection(); return false; }
            options.Reporter.Info("Grammar", "Lexer DFA generated");

            //Generate lexer

            //Generate parser
            options.Reporter.Info("Grammar", "Parsing method is " + options.ParserGenerator.Name);
            ParserData data = options.ParserGenerator.Build(this, options.Reporter);
            if (data == null) { options.Reporter.EndSection(); return false; }
            bool result = data.Export(new List<Terminal>(this.Terminals), options);
            options.Reporter.EndSection();
            
            //Output data
            if (options.Documentation != null)
                Export_Documentation(data, options);
            return result;
        }
    }
}
