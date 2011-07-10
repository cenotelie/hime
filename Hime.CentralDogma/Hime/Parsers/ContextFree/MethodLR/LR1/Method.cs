using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class MethodLR1 : BaseMethod, CFParserGenerator
    {
        public string Name { get { return "LR(1)"; } }

        public MethodLR1() { }

        public ParserData Build(Grammar grammar, Hime.Kernel.Reporting.Reporter reporter) { return Build((CFGrammar)grammar, reporter); }
        public ParserData Build(CFGrammar grammar, Hime.Kernel.Reporting.Reporter reporter)
        {
            this.reporter = reporter;
            reporter.Info("LR(1)", "Constructing LR(1) data ...");
            graph = ConstructGraph(grammar, reporter);
            Close();
            reporter.Info("LR(1)", graph.Sets.Count.ToString() + " states explored.");
            reporter.Info("LR(1)", "Done !");
            return new ParserDataLR1(this, grammar, graph);
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
