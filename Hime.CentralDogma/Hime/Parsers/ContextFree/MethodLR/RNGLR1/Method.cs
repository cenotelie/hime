/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;

namespace Hime.Parsers.ContextFree.LR
{
    class MethodRNGLR1 : BaseMethod, CFParserGenerator
    {
        public string Name { get { return "RNGLR(1)"; } }

        public MethodRNGLR1() { }

        protected override void OnBeginState(State state)
        {
            foreach (Conflict conflict in state.Conflicts)
                conflict.IsError = false;
        }

        public ParserData Build(Grammar grammar, Hime.Kernel.Reporting.Reporter reporter) { return Build((CFGrammar)grammar, reporter); }
        public ParserData Build(CFGrammar grammar, Hime.Kernel.Reporting.Reporter reporter)
        {
            this.reporter = reporter;
            reporter.Info("RNGLR(1)", "Constructing RNGLR(1) data ...");
            graph = ConstructGraph(grammar, reporter);
            Close();
            reporter.Info("RNGLR(1)", graph.States.Count.ToString() + " states explored.");
            reporter.Info("RNGLR(1)", "Done !");
            return new ParserDataRNGLR1(this, grammar, graph);
        }

        public static Graph ConstructGraph(CFGrammar Grammar, Hime.Kernel.Reporting.Reporter Log)
        {
            Graph GraphLR1 = MethodLR1.ConstructGraph(Grammar, Log);
            foreach (State Set in GraphLR1.States)
                Set.BuildReductions(new StateReductionsRNGLR1());
            return GraphLR1;
        }
    }
}
