/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;

namespace Hime.Parsers.ContextFree.LR
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
            reporter.Info("LR(1)", graph.States.Count.ToString() + " states explored.");
            reporter.Info("LR(1)", "Done !");
            return new ParserDataLR1(reporter, grammar, graph);
        }

        public static Graph ConstructGraph(CFGrammar Grammar, Hime.Kernel.Reporting.Reporter Log)
        {
            // Create the first set
            CFVariable AxiomVar = Grammar.GetCFVariable("_Axiom_");
            ItemLR1 AxiomItem = new ItemLR1(AxiomVar.CFRules[0], 0, TerminalEpsilon.Instance);
            StateKernel AxiomKernel = new StateKernel();
            AxiomKernel.AddItem(AxiomItem);
            State AxiomSet = AxiomKernel.GetClosure();
            Graph Graph = new Graph();
            // Construct the graph
            Graph.States.Add(AxiomSet);
            for (int i = 0; i != Graph.States.Count; i++)
            {
                Graph.States[i].BuildGraph(Graph);
                Graph.States[i].ID = i;
            }
            foreach (State Set in Graph.States)
                Set.BuildReductions(new StateReductionsLR1());
            return Graph;
        }
    }
}
