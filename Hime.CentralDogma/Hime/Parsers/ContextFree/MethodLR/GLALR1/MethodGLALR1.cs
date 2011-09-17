/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;

namespace Hime.Parsers.ContextFree.LR
{
    class MethodGLALR1 : BaseMethod
    {
        public override string Name { get { return "GLALR(1)"; } }

        public MethodGLALR1() { }

        public override ParserData Build(CFGrammar grammar, Hime.Kernel.Reporting.Reporter reporter)
        {
            reporter.Info("GLALR(1)", "Constructing GLALR(1) data ...");
            Graph Graph = ConstructGraph(grammar, reporter);
            // Output conflicts
            foreach (State Set in Graph.States)
                foreach (Conflict Conflict in Set.Conflicts)
                    reporter.Report(Conflict);
            reporter.Info("GLALR(1)", Graph.States.Count.ToString() + " states explored.");
            reporter.Info("GLALR(1)", "Done !");
            return new ParserDataGLR1(reporter, grammar, Graph);
        }

        public static Graph ConstructGraph(CFGrammar grammar, Hime.Kernel.Reporting.Reporter reporter)
        {
            Graph GraphLALR1 = MethodLALR1.ConstructGraph(grammar, reporter);
            foreach (State Set in GraphLALR1.States)
                Set.BuildReductions(new StateReductionsGLALR1());
            return GraphLALR1;
        }
    }
}
