namespace Hime.Parsers.CF.LR
{
    /// <summary>
    /// Implements the RNGLR(1) parsing method
    /// </summary>
    class MethodRNGLALR1 : CFParserGenerator
    {
        public string Name { get { return "RNGLALR1"; } }

        /// <summary>
        /// Construct the method
        /// </summary>
        public MethodRNGLALR1() { }

        public ParserData Build(Grammar Grammar, Hime.Kernel.Reporting.Reporter Reporter) { return Build((CFGrammar)Grammar, Reporter); }
        public ParserData Build(CFGrammar Grammar, Hime.Kernel.Reporting.Reporter Reporter)
        {
            Reporter.Info("RNGLALR(1)", "Constructing RNGLALR(1) data ...");
            Graph Graph = ConstructGraph(Grammar, Reporter);
            // Output conflicts
            foreach (ItemSet Set in Graph.Sets)
                foreach (Conflict Conflict in Set.Conflicts)
                    Reporter.Report(Conflict);
            Reporter.Info("RNGLALR(1)", Graph.Sets.Count.ToString() + " states explored.");
            Reporter.Info("RNGLALR(1)", "Done !");
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
            Graph GraphLALR1 = MethodLALR1.ConstructGraph(Grammar, Log);
            foreach (ItemSet Set in GraphLALR1.Sets)
                Set.BuildReductions(new ItemSetReductionsRNGLALR1());
            return GraphLALR1;
        }
    }
}
