using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class MethodRNGLALR1 : BaseMethod, CFParserGenerator
    {
        public string Name { get { return "RNGLALR(1)"; } }

        public MethodRNGLALR1() { }

        protected override void OnBeginState(State state)
        {
            foreach (Conflict conflict in state.Conflicts)
                conflict.IsError = false;
        }

        public IParserData Build(Grammar grammar, Hime.Kernel.Reporting.Reporter reporter) { return Build((CFGrammar)grammar, reporter); }
        public IParserData Build(CFGrammar grammar, Hime.Kernel.Reporting.Reporter reporter)
        {
            this.reporter = reporter;
            reporter.Info("RNGLALR(1)", "Constructing RNGLALR(1) data ...");
            graph = ConstructGraph(grammar, reporter);
            Close();
            reporter.Info("RNGLALR(1)", graph.States.Count.ToString() + " states explored.");
            reporter.Info("RNGLALR(1)", "Done !");
            return new ParserDataRNGLR1(this, grammar, graph);
        }

        public static Graph ConstructGraph(CFGrammar Grammar, Hime.Kernel.Reporting.Reporter Log)
        {
            Graph GraphLALR1 = MethodLALR1.ConstructGraph(Grammar, Log);
            foreach (State Set in GraphLALR1.States)
                Set.BuildReductions(new StateReductionsRNGLALR1());
            return GraphLALR1;
        }
    }
}
