namespace Hime.Parsers.CF.LR
{
    /// <summary>
    /// Implements the GLALR(1) parsing method
    /// </summary>
    public class MethodGLALR1 : CFParserGenerator
    {
        public string Name { get { return "GLALR1"; } }

        /// <summary>
        /// Construct the method
        /// </summary>
        public MethodGLALR1() { }

        public ParserData Build(Grammar Grammar, Hime.Kernel.Reporting.Reporter Reporter) { return Build((CFGrammar)Grammar, Reporter); }
        public ParserData Build(CFGrammar Grammar, Hime.Kernel.Reporting.Reporter Reporter)
        {
            Reporter.Info("GLALR(1)", "Constructing GLALR(1) data ...");
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
            Reporter.Info("GLALR(1)", Graph.Sets.Count.ToString() + " states explored.");
            Reporter.Info("GLALR(1)", "Done !");
            return new GLR1ParserData(this, Grammar, Graph);
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
                Set.BuildReductions(new ItemSetActionsGLALR1());
            return GraphLALR1;
        }
    }
}