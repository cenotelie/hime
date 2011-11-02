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
    class MethodRNGLALR1 : BaseMethod
    {
        public MethodRNGLALR1(Reporter reporter) : base("RNGLALR(1)", reporter)
		{
		}

        protected override void OnBeginState(State state)
        {
            foreach (Conflict conflict in state.Conflicts)
                conflict.IsError = false;
        }

		protected override Graph BuildGraph (CFGrammar grammar)
		{
			return ConstructGraph(grammar);
		}
		
		protected override ParserData BuildParserData (CFGrammar grammar)
		{
			return new ParserDataRNGLR1(this.reporter, grammar, this.graph);
		}
		
		// TODO: remove static methods
        public static Graph ConstructGraph(CFGrammar Grammar)
        {
            Graph GraphLALR1 = MethodLALR1.ConstructGraph(Grammar);
            foreach (State Set in GraphLALR1.States)
                Set.BuildReductions(new StateReductionsRNGLALR1());
            return GraphLALR1;
        }
    }
}
