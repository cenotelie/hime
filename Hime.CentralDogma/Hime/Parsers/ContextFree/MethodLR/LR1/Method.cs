using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class MethodLR1 : CFParserGenerator
    {
        public string Name { get { return "LR(1)"; } }

        public MethodLR1() { }

        public ParserData Build(Grammar Grammar, Hime.Kernel.Reporting.Reporter Reporter) { return Build((CFGrammar)Grammar, Reporter); }
        public ParserData Build(CFGrammar Grammar, Hime.Kernel.Reporting.Reporter Reporter)
        {
            Reporter.Info("LR(1)", "Constructing LR(1) data ...");
            Graph Graph = ConstructGraph(Grammar, Reporter);
            // Output conflicts
            foreach (State set in Graph.Sets)
                foreach (Conflict conflict in set.Conflicts)
                    Reporter.Report(conflict);
            Reporter.Info("LR(1)", Graph.Sets.Count.ToString() + " states explored.");
            Reporter.Info("LR(1)", "Done !");
            return new ParserDataLR1(this, Grammar, Graph);
        }

        public static Graph ConstructGraph(CFGrammar Grammar, Hime.Kernel.Reporting.Reporter Log)
        {
            // Create the first set
            CFVariable AxiomVar = Grammar.GetVariable("_Axiom_");
            ItemLR1 AxiomItem = new ItemLR1(AxiomVar.Rules[0], 0, TerminalEpsilon.Instance);
            StateKernel AxiomKernel = new StateKernel();
            AxiomKernel.AddItem(AxiomItem);
            State AxiomSet = AxiomKernel.GetClosure();
            Graph Graph = new Graph();
            // Construct the graph
            Graph.Sets.Add(AxiomSet);
            for (int i = 0; i != Graph.Sets.Count; i++)
            {
                Graph.Sets[i].BuildGraph(Graph);
                Graph.Sets[i].ID = i;
            }
            foreach (State Set in Graph.Sets)
                Set.BuildReductions(new StateReductionsLR1());
            return Graph;
        }
    }
}
