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
            Dictionary<State, Dictionary<Terminal, DeciderGraph>> deciders = new Dictionary<State, Dictionary<Terminal, DeciderGraph>>();
            // Output conflicts
            foreach (State Set in graph.Sets)
            {
                if (Set.Conflicts.Count != 0)
                    deciders.Add(Set, new Dictionary<Terminal, DeciderGraph>());
                foreach (Conflict Conflict in Set.Conflicts)
                {
                    DeciderGraph dg =  analyser.Analyse(Set, Conflict.ConflictSymbol);
                    deciders[Set].Add(Conflict.ConflictSymbol, dg);
                    Kernel.Graphs.DOTSerializer serializer = new Kernel.Graphs.DOTSerializer("Decider", "Decider" + Set.ID.ToString() + "_" + Conflict.ConflictSymbol.ToString() + ".dot");
                    Serialize(dg, serializer);
                    serializer.Close();
                    Reporter.Report(Conflict);
                }
            }

            Reporter.Info("LR(Automata)", graph.Sets.Count.ToString() + " states explored.");
            Reporter.Info("LR(Automata)", "Done !");
            return new ParserDataLRA(this, Grammar, graph);
        }

        private void Serialize(DeciderGraph graph, Kernel.Graphs.DOTSerializer serializer)
        {
            foreach (DeciderState state in graph.States)
            {
                serializer.WriteNode(state.ID.ToString());
                int i = 0;
                foreach (Item item in state.Decisions.Keys)
                {
                    serializer.WriteNode(state.ID.ToString() + "_" + i.ToString(), item.ToString(), Kernel.Graphs.DOTNodeShape.ellipse);
                    i++;
                }
            }
            foreach (DeciderState state in graph.States)
            {
                int i = 0;
                foreach (Item item in state.Decisions.Keys)
                {
                    foreach (Terminal t in state.Decisions[item])
                        serializer.WriteEdge(state.ID.ToString(), state.ID.ToString() + "_" + i.ToString(), t.ToString());
                    i++;
                }
                foreach (Terminal t in state.Transitions.Keys)
                    serializer.WriteEdge(state.ID.ToString(), state.Transitions[t].ID.ToString(), t.ToString());
            }
        }
    }
}
