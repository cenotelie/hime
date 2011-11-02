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
    class MethodGLALR1 : BaseMethod
    {
        public override string Name { get { return "GLALR(1)"; } }

        public MethodGLALR1() { }

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

		// TODO: remove static methods
        public static Graph ConstructGraph(CFGrammar grammar)
        {
            Graph GraphLALR1 = MethodLALR1.ConstructGraph(grammar);
            foreach (State Set in GraphLALR1.States)
                Set.BuildReductions(new StateReductionsGLALR1());
            return GraphLALR1;
        }
    }
}
