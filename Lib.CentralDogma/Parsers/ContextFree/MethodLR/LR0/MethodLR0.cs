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
    class MethodLR0 : BaseMethod
    {
        public override string Name { get { return "LR(0)"; } }

        public MethodLR0() { }

        public override ParserData Build(CFGrammar grammar, Reporter reporter)
        {
			base.Build(grammar, reporter);
            return new ParserDataLR0(reporter, grammar, graph);
        }
		
		protected override Graph BuildGraph (CFGrammar grammar)
		{
			return ConstructGraph(grammar);
		}


        public static Graph ConstructGraph(CFGrammar Grammar)
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
