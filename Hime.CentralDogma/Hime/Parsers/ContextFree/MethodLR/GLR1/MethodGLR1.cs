/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;

namespace Hime.Parsers.ContextFree.LR
{
    class MethodGLR1 : CFParserGenerator
    {
        public string Name { get { return "GLR(1)"; } }

        public MethodGLR1() { }

        public ParserData Build(Grammar grammar, Hime.Kernel.Reporting.Reporter reporter) { return Build((CFGrammar)grammar, reporter); }
        public ParserData Build(CFGrammar grammar, Hime.Kernel.Reporting.Reporter reporter)
        {
            reporter.Info("GLR(1)", "Constructing GLR(1) data ...");
            Graph Graph = ConstructGraph(grammar, reporter);
            // Output conflicts
            foreach (State Set in Graph.States)
                foreach (Conflict Conflict in Set.Conflicts)
                    reporter.Report(Conflict);
            reporter.Info("GLR(1)", Graph.States.Count.ToString() + " states explored.");
            reporter.Info("GLR(1)", "Done !");
            return new ParserDataGLR1(reporter, grammar, Graph);
        }

        public static Graph ConstructGraph(CFGrammar grammar, Hime.Kernel.Reporting.Reporter reporter)
        {
            Graph GraphLR1 = MethodLR1.ConstructGraph(grammar, reporter);
            foreach (State Set in GraphLR1.States)
                Set.BuildReductions(new StateReductionsGLR1());
            return GraphLR1;
        }
    }
}
