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
    class MethodLALR1 : BaseMethod
    {
        public override string Name { get { return "LALR(1)"; } }

        public MethodLALR1() { }

        public override ParserData Build(CFGrammar grammar, Reporter reporter)
        {
			base.Build(grammar, reporter);
            return new ParserDataLR1(reporter, grammar, graph);
        }
		
		protected override Graph BuildGraph (CFGrammar grammar)
		{
			return ConstructGraph(grammar);
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
