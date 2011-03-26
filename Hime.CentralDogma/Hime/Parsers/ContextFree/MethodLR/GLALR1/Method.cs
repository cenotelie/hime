using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class MethodGLALR1 : CFParserGenerator
    {
        public string Name { get { return "GLALR(1)"; } }

        public MethodGLALR1() { }

        public ParserData Build(Grammar Grammar, Hime.Kernel.Reporting.Reporter Reporter) { return Build((CFGrammar)Grammar, Reporter); }
        public ParserData Build(CFGrammar Grammar, Hime.Kernel.Reporting.Reporter Reporter)
        {
            Reporter.Info("GLALR(1)", "Constructing GLALR(1) data ...");
            Graph Graph = ConstructGraph(Grammar, Reporter);
            // Output conflicts
            foreach (ItemSet Set in Graph.Sets)
                foreach (Conflict Conflict in Set.Conflicts)
                    Reporter.Report(Conflict);
            Reporter.Info("GLALR(1)", Graph.Sets.Count.ToString() + " states explored.");
            Reporter.Info("GLALR(1)", "Done !");
            return new GLR1ParserData(this, Grammar, Graph);
        }

        public static Graph ConstructGraph(CFGrammar Grammar, Hime.Kernel.Reporting.Reporter Log)
        {
            Graph GraphLALR1 = MethodLALR1.ConstructGraph(Grammar, Log);
            foreach (ItemSet Set in GraphLALR1.Sets)
                Set.BuildReductions(new ItemSetActionsGLALR1());
            return GraphLALR1;
        }
    }
}
