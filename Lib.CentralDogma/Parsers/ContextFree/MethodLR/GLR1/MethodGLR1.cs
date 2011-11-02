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
    class MethodGLR1 : BaseMethod
    {
        public MethodGLR1(Reporter reporter) : base("GLR(1)", reporter)
		{
		}

		protected override Graph BuildGraph (CFGrammar grammar)
		{
			Graph result = ConstructGraph(grammar);
            // Output conflicts
            foreach (State Set in this.graph.States)
                foreach (Conflict Conflict in Set.Conflicts)
                    reporter.Report(Conflict);
			return result;
		}
		
		protected override ParserData BuildParserData (CFGrammar grammar)
		{
			return new ParserDataGLR1(this.reporter, grammar, this.graph);
		}
		
		// TODO: try to remove static methods
        public static Graph ConstructGraph(CFGrammar grammar)
        {
            Graph GraphLR1 = MethodLR1.ConstructGraph(grammar);
            foreach (State Set in GraphLR1.States)
                Set.BuildReductions(new StateReductionsGLR1());
            return GraphLR1;
        }
    }
}
