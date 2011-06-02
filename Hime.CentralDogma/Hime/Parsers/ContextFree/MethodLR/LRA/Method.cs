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
            Dictionary<State, SubMachine> submachines = new Dictionary<State, SubMachine>();
            // Output conflicts
            foreach (State Set in graph.Sets)
            {
                SubMachine machine = new SubMachine();
                List<Terminal> conflictous = new List<Terminal>();
                foreach (Conflict conflict in Set.Conflicts)
                {
                    conflictous.Add(conflict.ConflictSymbol);
                    DeciderGraph dg = analyser.Analyse(Set, conflict.ConflictSymbol);
                    machine.AddDeciderGraph(dg);
                    Reporter.Report(conflict);
                }
                foreach (Item item in Set.Items)
                {
                    if (item.Action == ItemAction.Reduce)
                    {
                        foreach (Terminal t in item.Lookaheads)
                            if (!conflictous.Contains(t))
                                machine.AddDecision(t, item.BaseRule);
                    }
                }
                foreach (Symbol symbol in Set.Children.Keys)
                {
                    if (symbol is Terminal)
                    {
                        Terminal terminal = (Terminal)symbol;
                        if (!conflictous.Contains(terminal))
                            machine.AddDecision(terminal, Set.Children[symbol]);
                    }
                }
                machine.Close();
                submachines.Add(Set, machine);
                Kernel.Graphs.DOTSerializer serializer = new Kernel.Graphs.DOTSerializer("State" + Set.ID.ToString(), "State" + Set.ID.ToString() + ".dot");
                Serialize(machine, serializer);
                serializer.Close();
            }

            Reporter.Info("LR(Automata)", graph.Sets.Count.ToString() + " states explored.");
            Reporter.Info("LR(Automata)", "Done !");
            return new ParserDataLRA(this, Grammar, graph, submachines);
        }

        private void Serialize(SubMachine machine, Kernel.Graphs.DOTSerializer serializer)
        {
            foreach (SubState state in machine.States)
            {
                string id = state.ID.ToString();
                string label = id;
                if (state.ShiftDecision != null)
                    label = "SHIFT: " + state.ShiftDecision.ID.ToString();
                else if (state.RuleDecision != null)
                    label = state.RuleDecision.ToString();
                serializer.WriteNode(id, label);
            }
            foreach (SubState state in machine.States)
                foreach (Terminal t in state.Transitions.Keys)
                    serializer.WriteEdge(state.ID.ToString(), state.Transitions[t].ID.ToString(), t.ToString());
        }
    }
}
