using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class MethodLALR1 : CFParserGenerator
    {
        public string Name { get { return "LALR(1)"; } }

        public MethodLALR1() { }

        public ParserData Build(Grammar Grammar, Hime.Kernel.Reporting.Reporter Reporter) { return Build((CFGrammar)Grammar, Reporter); }
        public ParserData Build(CFGrammar Grammar, Hime.Kernel.Reporting.Reporter Reporter)
        {
            Reporter.Info("LALR(1)", "Constructing LALR(1) data ...");
            Graph graph = ConstructGraph(Grammar, Reporter);
            // Output conflicts
            ConflictExamplifier analyzer = new ConflictExamplifier(graph);
            foreach (State set in graph.Sets)
            {
                List<Terminal> example = null;
                if (set.Conflicts.Count != 0)
                    example = analyzer.GetExample(set);
                foreach (Conflict conflict in set.Conflicts)
                {
                    conflict.InputSample = example;
                    Reporter.Report(conflict);
                }
            }
            Reporter.Info("LALR(1)", graph.Sets.Count.ToString() + " states explored.");
            Reporter.Info("LALR(1)", "Done !");
            return new ParserDataLR1(this, Grammar, graph);
        }

        public static Graph ConstructGraph(CFGrammar Grammar, Hime.Kernel.Reporting.Reporter Log)
        {
            Graph GraphLR0 = MethodLR0.ConstructGraph(Grammar, Log);
            KernelGraph Kernels = new KernelGraph(GraphLR0);
            return Kernels.GetGraphLALR1();
        }
    }
}
