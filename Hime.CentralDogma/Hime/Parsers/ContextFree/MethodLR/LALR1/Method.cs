using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class MethodLALR1 : BaseMethod, CFParserGenerator
    {
        public string Name { get { return "LALR(1)"; } }

        public MethodLALR1() { }

        public ParserData Build(Grammar grammar, Hime.Kernel.Reporting.Reporter reporter) { return Build((CFGrammar)grammar, reporter); }
        public ParserData Build(CFGrammar grammar, Hime.Kernel.Reporting.Reporter reporter)
        {
            this.reporter = reporter;
            reporter.Info("LALR(1)", "Constructing LALR(1) data ...");
            graph = ConstructGraph(grammar, reporter);
            Close();
            reporter.Info("LALR(1)", graph.States.Count.ToString() + " states explored.");
            reporter.Info("LALR(1)", "Done !");
            return new ParserDataLR1(this, grammar, graph);
        }

        public static Graph ConstructGraph(CFGrammar Grammar, Hime.Kernel.Reporting.Reporter Log)
        {
            Graph GraphLR0 = MethodLR0.ConstructGraph(Grammar, Log);
            KernelGraph Kernels = new KernelGraph(GraphLR0);
            return Kernels.GetGraphLALR1();
        }
    }
}
