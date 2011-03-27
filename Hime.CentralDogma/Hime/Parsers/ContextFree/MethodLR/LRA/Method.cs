using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class MethodLRA : CFParserGenerator
    {
        public string Name { get { return "LR(Automata)"; } }

        public MethodLRA() { }

        public ParserData Build(Grammar Grammar, Hime.Kernel.Reporting.Reporter Reporter) { return Build((CFGrammar)Grammar, Reporter); }
        public ParserData Build(CFGrammar Grammar, Hime.Kernel.Reporting.Reporter Reporter)
        {
            Reporter.Info("LR(Automata)", "LR(Automata) data ...");
            Graph graph = MethodLALR1.ConstructGraph(Grammar, Reporter);
            ConflictAnalyser analyser = new ConflictAnalyser(graph);
            Dictionary<State, DeciderGraph> deciders = new Dictionary<State, DeciderGraph>();
            // Output conflicts
            foreach (State Set in graph.Sets)
            {
                foreach (Conflict Conflict in Set.Conflicts)
                {
                    // Call analyser
                    Reporter.Report(Conflict);
                }
            }

            Reporter.Info("LR(Automata)", graph.Sets.Count.ToString() + " states explored.");
            Reporter.Info("LR(Automata)", "Done !");
            return new ParserDataRNGLR1(this, Grammar, graph);
        }
    }
}
