namespace Hime.Parsers.CF.LR
{
    /// <summary>
    /// Implements the LALR(1) parsing method
    /// </summary>
    public class MethodLALR1 : CFParserGenerator
    {
        public string Name { get { return "LALR1"; } }

        /// <summary>
        /// Construct the method
        /// </summary>
        public MethodLALR1() { }

        public ParserData Build(Grammar Grammar, Hime.Kernel.Reporting.Reporter Reporter) { return Build((CFGrammar)Grammar, Reporter); }
        public ParserData Build(CFGrammar Grammar, Hime.Kernel.Reporting.Reporter Reporter)
        {
            Reporter.Info("LALR(1)", "Constructing LALR(1) data ...");
            Graph Graph = ConstructGraph(Grammar, Reporter);
            // Output conflicts
            bool Error = false;
            foreach (ItemSet Set in Graph.Sets)
            {
                foreach (Conflict Conflict in Set.Conflicts)
                {
                    Reporter.Report(Conflict);
                    Error = true;
                }
            }
            Reporter.Info("LALR(1)", "Done !");
            if (Error) return null;
            return new LR1ParserData(this, Grammar, Graph);
        }

        /// <summary>
        /// Constructs a LR Graph for the given grammar
        /// </summary>
        /// <param name="Grammar">The original grammar</param>
        /// <param name="Log">Log for output</param>
        /// <returns>Returns the constructed LR graph</returns>
        public static Graph ConstructGraph(CFGrammar Grammar, Hime.Kernel.Reporting.Reporter Log)
        {
            Graph GraphLR0 = MethodLR0.ConstructGraph(Grammar, Log);
            KernelGraph Kernels = new KernelGraph(GraphLR0);
            return Kernels.GetGraphLALR1();
        }
    }
}