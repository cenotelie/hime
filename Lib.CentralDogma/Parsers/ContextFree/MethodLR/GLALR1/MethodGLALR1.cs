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

        public override ParserData Build(CFGrammar grammar, Reporter reporter)
        {
			base.Build(grammar, reporter);
            Graph Graph = ConstructGraph(grammar);
            // Output conflicts
            foreach (State Set in Graph.States)
                foreach (Conflict Conflict in Set.Conflicts)
                    reporter.Report(Conflict);
            this.ReportInfo(Graph.States.Count.ToString() + " states explored.");
            this.ReportInfo("Done !");
            return new ParserDataGLR1(reporter, grammar, Graph);
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
