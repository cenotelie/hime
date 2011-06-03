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
            foreach (State Set in graph.Sets)
            {
                foreach (Conflict conflict in Set.Conflicts)
                    Reporter.Report(conflict);
                Decider decider = new Decider(Set);
                decider.Build(simulator);
                deciders.Add(Set, decider);
                Kernel.Graphs.DOTSerializer serializer = new Kernel.Graphs.DOTSerializer("State" + Set.ID.ToString(), "State" + Set.ID.ToString() + ".dot");
                Serialize(decider, serializer);
                serializer.Close();
                Reporter.Info("LR(Automata)", "Submachine for state " + Set.ID.ToString() + " constructed: " + decider.States.Count.ToString());
            }
            Reporter.Info("LR(Automata)", "Done !");
            return new ParserDataLRA(this, Grammar, graph, deciders);
        }

        private void Serialize(Decider machine, Kernel.Graphs.DOTSerializer serializer)
        {
            foreach (DeciderState state in machine.States)
            {
                string id = state.ID.ToString();
                string label = id;
                if (state.Decision != -1)
                {
                    Item item = machine.GetItem(state.Decision);
                    if (item.Action == ItemAction.Shift)
                        label = "SHIFT: " + machine.LRState.Children[item.NextSymbol].ID.ToString();
                    else
                        label = item.BaseRule.ToString();
                }
                serializer.WriteNode(id, label);
            }
            foreach (DeciderState state in machine.States)
                foreach (Terminal t in state.Transitions.Keys)
                    serializer.WriteEdge(state.ID.ToString(), state.Transitions[t].ID.ToString(), t.ToString());
        }
    }
}
