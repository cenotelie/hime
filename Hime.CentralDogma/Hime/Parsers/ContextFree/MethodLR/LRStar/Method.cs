using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class MethodLRStar : BaseMethod, CFParserGenerator
    {
        private Dictionary<State, DeciderLRStar> deciders;

        public string Name { get { return "LR(*)"; } }

        public MethodLRStar() { }

        protected override void OnState(State state)
        {
            DeciderLRStar decider = new DeciderLRStar(state);
            decider.Build(simulator);
            lock (deciders)
            {
                deciders.Add(state, decider);
            }
            foreach (Conflict conflict in state.Conflicts)
                if (decider.IsResolved(conflict))
                    conflict.IsResolved = true;
        }

        public ParserData Build(Grammar grammar, Hime.Kernel.Reporting.Reporter reporter) { return Build((CFGrammar)grammar, reporter); }
        public ParserData Build(CFGrammar grammar, Hime.Kernel.Reporting.Reporter reporter)
        {
            this.reporter = reporter;
            reporter.Info("LR(*)", "LR(*) data ...");
            graph = MethodLALR1.ConstructGraph(grammar, reporter);
            deciders = new Dictionary<State, DeciderLRStar>();
            Close();
            reporter.Info("LR(*)", graph.Sets.Count.ToString() + " LALR(1) states constructed.");
            reporter.Info("LR(*)", "Done !");
            return new ParserDataLRStar(this, grammar, graph, deciders);
        }
    }
}
