using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    /// <summary>
    /// Implements the LR(Automata) parsing method
    /// </summary>
    class MethodLRA : CFParserGenerator
    {
        public string Name { get { return "LR(Automata)"; } }

        /// <summary>
        /// Construct the method
        /// </summary>
        public MethodLRA() { }

        public ParserData Build(Grammar Grammar, Hime.Kernel.Reporting.Reporter Reporter) { return Build((CFGrammar)Grammar, Reporter); }
        public ParserData Build(CFGrammar Grammar, Hime.Kernel.Reporting.Reporter Reporter)
        {
            Reporter.Info("LR(Automata)", "LR(Automata) data ...");
            Graph graph = MethodLALR1.ConstructGraph(Grammar, Reporter);
            ConflictAnalyser analyser = new ConflictAnalyser(graph);
            Dictionary<ItemSet, DeciderGraph> deciders = new Dictionary<ItemSet, DeciderGraph>();
            // Output conflicts
            foreach (ItemSet Set in graph.Sets)
            {
                foreach (Conflict Conflict in Set.Conflicts)
                {
                    // Call analyser
                    Reporter.Report(Conflict);
                }
            }

            Reporter.Info("LR(Automata)", graph.Sets.Count.ToString() + " states explored.");
            Reporter.Info("LR(Automata)", "Done !");
            return new RNGLR1ParserData(this, Grammar, graph);
        }
    }
}
