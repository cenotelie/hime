using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class MethodLR0 : CFParserGenerator
    {
        public string Name { get { return "LR(0)"; } }

        public MethodLR0() { }

        public ParserData Build(Grammar Grammar, Hime.Kernel.Reporting.Reporter Reporter) { return Build((CFGrammar)Grammar, Reporter); }
        public ParserData Build(CFGrammar Grammar, Hime.Kernel.Reporting.Reporter Reporter)
        {
            Reporter.Info("LR(0)", "Constructing LR(0) data ...");
            Graph Graph = ConstructGraph(Grammar, Reporter);
            // Output conflicts
            foreach (State set in Graph.Sets)
                foreach (Conflict conflict in set.Conflicts)
                    Reporter.Report(conflict);
            Reporter.Info("LR(0)", Graph.Sets.Count.ToString() + " states explored.");
            Reporter.Info("LR(0)", "Done !");
            return new ParserDataLR1(this, Grammar, Graph);
        }


        public static Graph ConstructGraph(CFGrammar Grammar, Hime.Kernel.Reporting.Reporter Log)
        {
            // Create the first set
            CFVariable AxiomVar = Grammar.GetVariable("_Axiom_");
            ItemLR0 AxiomItem = new ItemLR0(AxiomVar.Rules[0], 0);
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
                Set.BuildReductions(new StateReductionsLR0());
            return Graph;
        }
    }
}
