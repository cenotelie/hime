namespace Hime.Parsers.CF.LR
{
    /// <summary>
    /// Implements the LR(0) parsing method
    /// </summary>
    class MethodLR0 : CFParserGenerator
    {
        public string Name { get { return "LR0"; } }

        /// <summary>
        /// Construct the method
        /// </summary>
        public MethodLR0() { }

        public ParserData Build(Grammar Grammar, Hime.Kernel.Reporting.Reporter Reporter) { return Build((CFGrammar)Grammar, Reporter); }
        public ParserData Build(CFGrammar Grammar, Hime.Kernel.Reporting.Reporter Reporter)
        {
            Reporter.Info("LR(0)", "Constructing LR(0) data ...");
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
            Reporter.Info("LR(0)", Graph.Sets.Count.ToString() + " states explored.");
            Reporter.Info("LR(0)", "Done !");
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
            // Create the first set
            CFVariable AxiomVar = Grammar.GetVariable("_Axiom_");
            ItemLR0 AxiomItem = new ItemLR0(AxiomVar.Rules[0], 0);
            ItemSetKernel AxiomKernel = new ItemSetKernel();
            AxiomKernel.AddItem(AxiomItem);
            ItemSet AxiomSet = AxiomKernel.GetClosure();
            Graph Graph = new Graph();
            // Construct the graph
            Graph.Sets.Add(AxiomSet);
            for (int i = 0; i != Graph.Sets.Count; i++)
            {
                Graph.Sets[i].BuildGraph(Graph);
                Graph.Sets[i].ID = i;
            }
            foreach (ItemSet Set in Graph.Sets)
                Set.BuildReductions(new ItemSetReductionsLR0());
            return Graph;
        }
    }
}