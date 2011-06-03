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
            Reporter.Info("LR(Automata)", graph.Sets.Count.ToString() + " LALR(1) states constructed.");
            GLRSimulator simulator = new GLRSimulator(graph);
            Dictionary<State, Decider> deciders = new Dictionary<State, Decider>();
            // Output conflicts
            foreach (State state in graph.Sets)
            {
                foreach (Conflict conflict in state.Conflicts)
                    Reporter.Report(conflict);
                Decider decider = new Decider(state);
                decider.Build(simulator);
                deciders.Add(state, decider);
            }
            Reporter.Info("LR(Automata)", "Done !");
            return new ParserDataLRA(this, Grammar, graph, deciders);
        }
    }
}
