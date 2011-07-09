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
            Graph Graph = ConstructGraph(Grammar, Reporter);
            // Output conflicts
            bool Error = false;
            foreach (State Set in Graph.Sets)
            {
                foreach (Conflict Conflict in Set.Conflicts)
                {
                    Reporter.Report(Conflict);
                    Error = true;
                }
            }
            Reporter.Info("LALR(1)", Graph.Sets.Count.ToString() + " states explored.");
            Reporter.Info("LALR(1)", "Done !");
            return new ParserDataLR1(this, Grammar, Graph);
        }

        public static Graph ConstructGraph(CFGrammar Grammar, Hime.Kernel.Reporting.Reporter Log)
        {
            Graph GraphLR0 = MethodLR0.ConstructGraph(Grammar, Log);
            KernelGraph Kernels = new KernelGraph(GraphLR0);
            return Kernels.GetGraphLALR1();
        }
    }
}
