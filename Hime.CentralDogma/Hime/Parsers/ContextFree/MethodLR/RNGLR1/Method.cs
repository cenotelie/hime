using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class MethodRNGLR1 : CFParserGenerator
    {
        public string Name { get { return "RNGLR(1)"; } }

        public MethodRNGLR1() { }

        public ParserData Build(Grammar Grammar, Hime.Kernel.Reporting.Reporter Reporter) { return Build((CFGrammar)Grammar, Reporter); }
        public ParserData Build(CFGrammar Grammar, Hime.Kernel.Reporting.Reporter Reporter)
        {
            Reporter.Info("RNGLR(1)", "Constructing RNGLR(1) data ...");
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
            Reporter.Info("RNGLR(1)", Graph.Sets.Count.ToString() + " states explored.");
            Reporter.Info("RNGLR(1)", "Done !");
            return new ParserDataRNGLR1(this, Grammar, Graph);
        }

        public static Graph ConstructGraph(CFGrammar Grammar, Hime.Kernel.Reporting.Reporter Log)
        {
            Graph GraphLR1 = MethodLR1.ConstructGraph(Grammar, Log);
            foreach (State Set in GraphLR1.Sets)
                Set.BuildReductions(new StateReductionsRNGLR1());
            return GraphLR1;
        }
    }
}
