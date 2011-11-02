/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;

namespace Hime.Parsers.ContextFree.LR
{
    class MethodLR0 : BaseMethod
    {
        public override string Name { get { return "LR(0)"; } }

        public MethodLR0() { }

        public override ParserData Build(CFGrammar grammar, Hime.Kernel.Reporting.Reporter reporter)
        {
            this.reporter = reporter;
            reporter.Info("LR(0)", "Constructing LR(0) data ...");
            graph = ConstructGraph(grammar, reporter);
            Close();
            reporter.Info("LR(0)", graph.States.Count.ToString() + " states explored.");
            reporter.Info("LR(0)", "Done !");
            return new ParserDataLR0(reporter, grammar, graph);
        }


        public static Graph ConstructGraph(CFGrammar Grammar, Hime.Kernel.Reporting.Reporter Log)
        {
            // Create the first set
            CFVariable AxiomVar = Grammar.GetCFVariable("_Axiom_");
            ItemLR0 AxiomItem = new ItemLR0(AxiomVar.CFRules[0], 0);
            StateKernel AxiomKernel = new StateKernel();
            AxiomKernel.AddItem(AxiomItem);
            State AxiomSet = AxiomKernel.GetClosure();
            Graph graph = new Graph(AxiomSet);
            // Construct the graph
            foreach (State Set in graph.States)
                Set.BuildReductions(new StateReductionsLR0());
            return graph;
        }
    }
}
