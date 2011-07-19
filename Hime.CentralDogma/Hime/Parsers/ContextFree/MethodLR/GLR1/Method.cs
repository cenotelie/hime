using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class MethodGLR1 : CFParserGenerator
    {
        public string Name { get { return "GLR(1)"; } }

        public MethodGLR1() { }

        public ParserData Build(Grammar Grammar, Hime.Kernel.Reporting.Reporter Reporter) { return Build((CFGrammar)Grammar, Reporter); }
        public ParserData Build(CFGrammar Grammar, Hime.Kernel.Reporting.Reporter Reporter)
        {
            Reporter.Info("GLR(1)", "Constructing GLR(1) data ...");
            Graph Graph = ConstructGraph(Grammar, Reporter);
            // Output conflicts
            foreach (State Set in Graph.States)
                foreach (Conflict Conflict in Set.Conflicts)
                    Reporter.Report(Conflict);
            Reporter.Info("GLR(1)", Graph.States.Count.ToString() + " states explored.");
            Reporter.Info("GLR(1)", "Done !");
            return new ParserDataGLR1(this, Grammar, Graph);
        }

        public static Graph ConstructGraph(CFGrammar Grammar, Hime.Kernel.Reporting.Reporter Log)
        {
            Graph GraphLR1 = MethodLR1.ConstructGraph(Grammar, Log);
            foreach (State Set in GraphLR1.States)
                Set.BuildReductions(new StateReductionsGLR1());
            return GraphLR1;
        }
    }
}
