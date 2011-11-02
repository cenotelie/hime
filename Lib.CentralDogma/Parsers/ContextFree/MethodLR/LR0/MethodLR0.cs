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
        public MethodLR0() : base("LR(0)")
		{
		}
		
		protected override Graph BuildGraph (CFGrammar grammar)
		{
			return ConstructGraph(grammar);
		}
		
		protected override ParserData BuildParserData (CFGrammar grammar)
		{
			return new ParserDataLR0(this.reporter, grammar, this.graph);
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
