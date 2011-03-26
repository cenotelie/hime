namespace Hime.Parsers.CF.LR
{
    /// <summary>
    /// Implements the RNGLR(1) parsing method
    /// </summary>
    class MethodRNGLR1 : CFParserGenerator
    {
        public string Name { get { return "RNGLR(1)"; } }

        /// <summary>
        /// Construct the method
        /// </summary>
        public MethodRNGLR1() { }

        public ParserData Build(Grammar Grammar, Hime.Kernel.Reporting.Reporter Reporter) { return Build((CFGrammar)Grammar, Reporter); }
        public ParserData Build(CFGrammar Grammar, Hime.Kernel.Reporting.Reporter Reporter)
        {
            Reporter.Info("RNGLR(1)", "Constructing RNGLR(1) data ...");
            Graph Graph = ConstructGraph(Grammar, Reporter);
            // Output conflicts
            foreach (ItemSet Set in Graph.Sets)
                foreach (Conflict Conflict in Set.Conflicts)
                    Reporter.Report(Conflict);
            Reporter.Info("RNGLR(1)", Graph.Sets.Count.ToString() + " states explored.");
            Reporter.Info("RNGLR(1)", "Done !");
            return new RNGLR1ParserData(this, Grammar, Graph);
        }

        /// <summary>
        /// Constructs a LR Graph for the given grammar
        /// </summary>
        /// <param name="Grammar">The original grammar</param>
        /// <param name="Log">Log for output</param>
        /// <returns>Returns the constructed LR graph</returns>
        public static Graph ConstructGraph(CFGrammar Grammar, Hime.Kernel.Reporting.Reporter Log)
        {
            Graph GraphLR1 = MethodLR1.ConstructGraph(Grammar, Log);
            foreach (ItemSet Set in GraphLR1.Sets)
                Set.BuildReductions(new ItemSetReductionsRNGLR1());
            return GraphLR1;
        }
    }
}