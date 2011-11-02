/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;
using Hime.Kernel.Reporting;

namespace Hime.Parsers.ContextFree.LR
{
    internal class MethodLR1 : BaseMethod
    {
        public override string Name { get { return "LR(1)"; } }

        internal MethodLR1() { }

        public override ParserData Build(CFGrammar grammar, Reporter reporter)
        {
            this.reporter = reporter;
            reporter.Info("LR(1)", "Constructing LR(1) data ...");
            graph = ConstructGraph(grammar);
            Close();
            reporter.Info("LR(1)", graph.States.Count.ToString() + " states explored.");
            reporter.Info("LR(1)", "Done !");
            return new ParserDataLR1(reporter, grammar, graph);
        }
		
		// TODO: try to remove static methods
        internal static Graph ConstructGraph(CFGrammar grammar)
        {
            // Create the first set
            CFVariable AxiomVar = grammar.GetCFVariable("_Axiom_");
            ItemLR1 AxiomItem = new ItemLR1(AxiomVar.CFRules[0], 0, TerminalEpsilon.Instance);
            StateKernel AxiomKernel = new StateKernel();
            AxiomKernel.AddItem(AxiomItem);
            State AxiomSet = AxiomKernel.GetClosure();
            Graph graph = new Graph(AxiomSet);
            // Construct the graph
            foreach (State Set in graph.States)
			{
                Set.BuildReductions(new StateReductionsLR1());
			}
            return graph;
        }
    }
}
