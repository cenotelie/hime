/*
 * Author: Charles Hymans
 * Date: 18/07/2011
 * Time: 23:19
 * 
 */
using System;
using System.Collections.Generic;

namespace Hime.Parsers.CF
{
    public sealed class CFGrammarBinary : CFGrammar
    {
        public CFGrammarBinary(string Name) : base(Name) { }

        public override void Inherit(CFGrammar Parent)
        {
        	base.Inherit(Parent);
            foreach (TerminalBin Terminal in Parent.Terminals)
                AddTerminalBin(Terminal.Type, Terminal.LocalName);
            foreach (Variable Variable in Parent.Variables)
                AddVariable(Variable.LocalName);
            foreach (Virtual Virtual in Parent.Virtuals)
                AddVirtual(Virtual.LocalName);
            foreach (Action Action in Parent.Actions)
                AddAction(Action.LocalName);
            foreach (CFGrammarTemplateRule TemplateRule in Parent.TemplateRules)
                templateRules.Add(new CFGrammarTemplateRule(TemplateRule, this));
            foreach (Variable Variable in Parent.Variables)
            {
                Variable Clone = variables[Variable.LocalName];
                foreach (CFRule R in Variable.Rules)
                {
                    List<RuleDefinitionPart> Parts = new List<RuleDefinitionPart>();
                    CFRuleDefinition Def = new CFRuleDefinition();
                    foreach (RuleDefinitionPart Part in R.Definition.Parts)
                    {
                        Symbol S = null;
                        if (Part.Symbol is Variable)
                            S = variables[Part.Symbol.LocalName];
                        else if (Part.Symbol is Terminal)
                            S = terminals[Part.Symbol.LocalName];
                        else if (Part.Symbol is Virtual)
                            S = virtuals[Part.Symbol.LocalName];
                        else if (Part.Symbol is Action)
                            S = actions[Part.Symbol.LocalName];
                        Parts.Add(new RuleDefinitionPart(S, Part.Action));
                    }
                    Clone.AddRule(new CFRule(Clone, new CFRuleDefinition(Parts), R.ReplaceOnProduction));
                }
            }
        }
        // TODO: factor code with CFGrammarText
        public override CFGrammar Clone()
        {
            CFGrammar Result = new CFGrammarBinary(name);
            Result.Inherit(this);
            return Result;
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
