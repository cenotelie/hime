using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class MethodLR0 : BaseMethod, CFParserGenerator
    {
        public string Name { get { return "LR(0)"; } }

        public MethodLR0() { }

        public ParserData Build(Grammar grammar, Hime.Kernel.Reporting.Reporter reporter) { return Build((CFGrammar)grammar, reporter); }
        public ParserData Build(CFGrammar grammar, Hime.Kernel.Reporting.Reporter reporter)
        {
            this.reporter = reporter;
            reporter.Info("LR(0)", "Constructing LR(0) data ...");
            graph = ConstructGraph(grammar, reporter);
            Close();
            reporter.Info("LR(0)", graph.Sets.Count.ToString() + " states explored.");
            reporter.Info("LR(0)", "Done !");
            return new ParserDataLR1(this, grammar, graph);
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
