/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;

namespace Hime.Parsers.ContextFree.LR
{
    class MethodGLR1 : BaseMethod
    {
        public override string Name { get { return "GLR(1)"; } }

        public MethodGLR1() { }

        public override ParserData Build(CFGrammar grammar, Hime.Kernel.Reporting.Reporter reporter)
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
