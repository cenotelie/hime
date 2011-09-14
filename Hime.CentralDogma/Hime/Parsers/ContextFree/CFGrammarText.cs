/*
 * Author: Charles Hymans
 * Date: 18/07/2011
 * Time: 23:10
 * 
 */
using System;
using System.Collections.Generic;
using Hime.Kernel.Reporting;

namespace Hime.Parsers.ContextFree
{
	public sealed class CFGrammarText : CFGrammar
    {
        private Automata.DFA finalDFA;

        public CFGrammarText(string name) : base(name) { }

        public override void Inherit(CFGrammar parent)
        {
        	base.Inherit(parent);
            foreach (TerminalText terminal in parent.Terminals)
            {
                TerminalText clone = AddTerminalText(terminal.LocalName, terminal.NFA.Clone(false), terminal.SubGrammar);
                clone.NFA.StateExit.Final = clone;
            }
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
                    CFRuleDefinition def = new CFRuleDefinition();
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
        public override CFGrammar Clone()
        {
            CFGrammar result = new CFGrammarText(name);
            result.Inherit(this);
            return result;
        }

        private bool Prepare_DFA(Hime.Kernel.Reporting.Reporter log)
        {
            log.Info("Grammar", "Generating DFA for Terminals ...");

            // Construct a global NFA for all the terminals
            Automata.NFA final = new Automata.NFA();
            final.StateEntry = final.AddNewState();
            foreach (TerminalText terminal in terminals.Values)
            {
                Automata.NFA sub = terminal.NFA.Clone();
                final.InsertSubNFA(sub);
                final.StateEntry.AddTransition(Automata.NFA.Epsilon, sub.StateEntry);
            }
            // Construct the equivalent DFA and minimize it
            finalDFA = new Automata.DFA(final);
            finalDFA = finalDFA.Minimize();
            finalDFA.RepackTransitions();

            log.Info("Grammar", "Done !");
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
