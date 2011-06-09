using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class MethodRNGLALR1 : CFParserGenerator
    {
        public string Name { get { return "RNGLALR(1)"; } }

        public MethodRNGLALR1() { }

        public ParserData Build(Grammar Grammar, Hime.Kernel.Reporting.Reporter Reporter) { return Build((CFGrammar)Grammar, Reporter); }
        public ParserData Build(CFGrammar Grammar, Hime.Kernel.Reporting.Reporter Reporter)
        {
            Reporter.Info("RNGLALR(1)", "Constructing RNGLALR(1) data ...");
            Graph Graph = ConstructGraph(Grammar, Reporter);
            // Output conflicts
            foreach (State set in Graph.Sets)
            {
                foreach (Conflict conflict in set.Conflicts)
                {
                    conflict.IsError = false;
                    Reporter.Report(conflict);
                }
            }
            Reporter.Info("RNGLALR(1)", Graph.Sets.Count.ToString() + " states explored.");
            Reporter.Info("RNGLALR(1)", "Done !");
            return new ParserDataRNGLR1(this, Grammar, Graph);
        }

        public static Graph ConstructGraph(CFGrammar Grammar, Hime.Kernel.Reporting.Reporter Log)
        {
            Graph GraphLALR1 = MethodLALR1.ConstructGraph(Grammar, Log);
            foreach (State Set in GraphLALR1.Sets)
                Set.BuildReductions(new StateReductionsRNGLALR1());
            return GraphLALR1;
        }
    }
}
