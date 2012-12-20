/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;

namespace Hime.CentralDogma.Grammars.ContextFree.LR
{
    class MethodLALR1 : ParserGenerator
    {
        public MethodLALR1(Reporting.Reporter reporter)
            : base("LALR(1)", reporter)
		{ 
		}

		protected override Graph BuildGraph (CFGrammar grammar)
		{
			return ConstructGraph(grammar);
		}
		
		protected override ParserData BuildParserData (CFGrammar grammar)
		{
			return new ParserDataLRk(this.reporter, grammar, this.graph);
		}

		// TODO: remove static methods
        public static Graph ConstructGraph(CFGrammar Grammar)
        {
            Graph GraphLR0 = MethodLR0.ConstructGraph(Grammar);
            KernelGraph Kernels = new KernelGraph(GraphLR0);
            return Kernels.GetGraphLALR1();
        }
    }
}
