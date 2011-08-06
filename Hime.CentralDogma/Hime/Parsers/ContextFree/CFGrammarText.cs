/*
 * Author: Charles Hymans
 * Date: 18/07/2011
 * Time: 23:10
 * 
 */
using System;
using System.Collections.Generic;
using Hime.Kernel.Reporting;

// TODO: CF means ContextFree => should rename to ContextFree
namespace Hime.Parsers.CF
{
	public sealed class CFGrammarText : CFGrammar
    {
        private Automata.DFA finalDFA;

        public CFGrammarText(string Name) : base(Name) { }

        public override void Inherit(CFGrammar Parent)
        {
        	base.Inherit(Parent);
            foreach (TerminalText Terminal in Parent.Terminals)
            {
                TerminalText clone = AddTerminalText(Terminal.LocalName, Terminal.NFA.Clone(false), Terminal.SubGrammar);
                clone.NFA.StateExit.Final = clone;
            }
            foreach (CFVariable Variable in Parent.Variables)
                AddVariable(Variable.LocalName);
            foreach (Virtual Virtual in Parent.Virtuals)
                AddVirtual(Virtual.LocalName);
            foreach (Action Action in Parent.Actions)
                AddAction(Action.LocalName);
            foreach (CFGrammarTemplateRule TemplateRule in Parent.TemplateRules)
                templateRules.Add(new CFGrammarTemplateRule(TemplateRule, this));
            foreach (CFVariable Variable in Parent.Variables)
            {
                CFVariable Clone = variables[Variable.LocalName];
                foreach (CFRule R in Variable.Rules)
                {
                    List<RuleDefinitionPart> Parts = new List<RuleDefinitionPart>();
                    CFRuleDefinition Def = new CFRuleDefinition();
                    foreach (RuleDefinitionPart Part in R.Definition.Parts)
                    {
                        Symbol S = null;
                        if (Part.Symbol is CFVariable)
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
        public override CFGrammar Clone()
        {
            CFGrammar result = new CFGrammarText(name);
            result.Inherit(this);
            return result;
        }

        private bool Prepare_DFA(Hime.Kernel.Reporting.Reporter Log)
        {
            Log.Info("Grammar", "Generating DFA for Terminals ...");

            // Construct a global NFA for all the terminals
            Automata.NFA Final = new Automata.NFA();
            Final.StateEntry = Final.AddNewState();
            foreach (TerminalText Terminal in terminals.Values)
            {
                Automata.NFA Sub = Terminal.NFA.Clone();
                Final.InsertSubNFA(Sub);
                Final.StateEntry.AddTransition(Automata.NFA.Epsilon, Sub.StateEntry);
            }
            // Construct the equivalent DFA and minimize it
            finalDFA = new Automata.DFA(Final);
            finalDFA = finalDFA.Minimize();
            finalDFA.RepackTransitions();

            Log.Info("Grammar", "Done !");
            return true;
        }

        public override bool Build(CompilationTask options)
        {
        	Reporter reporter = options.Reporter;
            reporter.BeginSection(name + " parser data generation");
            if (!Prepare_AddRealAxiom(reporter)) { reporter.EndSection(); return false; }
            if (!Prepare_ComputeFirsts(reporter)) { reporter.EndSection(); return false; }
            if (!Prepare_ComputeFollowers(reporter)) { reporter.EndSection(); return false; }
            if (!Prepare_DFA(options.Reporter)) { reporter.EndSection(); return false; }
            reporter.Info("Grammar", "Lexer DFA generated");

            Terminal Separator = null;
            if (this.options.ContainsKey("Separator"))
                Separator = terminals[this.options["Separator"]];

            //Generate lexer
            Exporters.TextLexerExporter lexerExporter = new Exporters.TextLexerExporter(options.LexerWriter, options.Namespace, name, finalDFA, Separator);
            List<Terminal> expected = lexerExporter.Export(options.GeneratedCodeModifier);

            //Generate parser
            reporter.Info("Grammar", "Parsing method is " + options.ParserGenerator.Name);
            ParserData Data = options.ParserGenerator.Build(this,reporter);
            if (Data == null) { options.Reporter.EndSection(); return false; }
            bool result = Data.Export(expected, options);
            options.Reporter.EndSection();

            //Output data
            if (options.Documentation != null)
                Export_Documentation(Data, options);
            return result;
        }
    }
}
