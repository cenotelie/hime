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
        public override string Name { get { return "RNGLALR(1)"; } }

        public MethodRNGLALR1() { }

        protected override void OnBeginState(State state)
        {
            foreach (Conflict conflict in state.Conflicts)
                conflict.IsError = false;
        }

        public override ParserData Build(CFGrammar grammar, Reporter reporter)
        {
			base.Build(grammar, reporter);
            Close();
            this.ReportInfo(graph.States.Count.ToString() + " states explored.");
            this.ReportInfo("Done !");
            return new ParserDataRNGLR1(reporter, grammar, graph);
        }
		
		protected override Graph BuildGraph (CFGrammar grammar)
		{
			return ConstructGraph(grammar);
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
