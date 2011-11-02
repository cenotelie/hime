/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;

namespace Hime.Parsers.ContextFree.LR
{
    class MethodRNGLR1 : BaseMethod
    {
        public override string Name { get { return "RNGLR(1)"; } }

        public MethodRNGLR1() { }

        protected override void OnBeginState(State state)
        {
            foreach (Conflict conflict in state.Conflicts)
                conflict.IsError = false;
        }

        public override ParserData Build(CFGrammar grammar, Hime.Kernel.Reporting.Reporter reporter)
        {
			base.Build(grammar, reporter);
            this.ReportInfo("Constructing RNGLR(1) data ...");
            graph = ConstructGraph(grammar);
            Close();
            this.ReportInfo(graph.States.Count.ToString() + " states explored.");
            this.ReportInfo("Done !");
            return new ParserDataRNGLR1(reporter, grammar, graph);
        }

		// TODO: try to remove static methods
        public static Graph ConstructGraph(CFGrammar grammar)
        {
            Graph GraphLR1 = MethodLR1.ConstructGraph(grammar);
            foreach (State Set in GraphLR1.States)
                Set.BuildReductions(new StateReductionsRNGLR1());
            return GraphLR1;
        }
    }
}
