using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class MethodLRStar : CFParserGenerator
    {
        public string Name { get { return "LR(*)"; } }

        public MethodLRStar() { }

        public ParserData Build(Grammar Grammar, Hime.Kernel.Reporting.Reporter Reporter) { return Build((CFGrammar)Grammar, Reporter); }
        public ParserData Build(CFGrammar Grammar, Hime.Kernel.Reporting.Reporter Reporter)
        {
            Reporter.Info("LR(*)", "LR(*) data ...");
            Graph graph = MethodLALR1.ConstructGraph(Grammar, Reporter);
            Reporter.Info("LR(*)", graph.Sets.Count.ToString() + " LALR(1) states constructed.");
            GLRSimulator simulator = new GLRSimulator(graph);
            Dictionary<State, DeciderLRStar> deciders = new Dictionary<State, DeciderLRStar>();
            // Output conflicts
            foreach (State state in graph.Sets)
            {
                DeciderLRStar decider = new DeciderLRStar(state);
                decider.Build(simulator);
                deciders.Add(state, decider);

                foreach (Conflict conflict in state.Conflicts)
                {
                    if (!decider.IsResolved(conflict))
                        Reporter.Report(conflict);
                }
            }
            Reporter.Info("LR(*)", "Done !");
            return new ParserDataLRStar(this, Grammar, graph, deciders);
        }
    }
}
