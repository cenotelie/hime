using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class MethodLRStar : BaseMethod, CFParserGenerator
    {
        private Dictionary<State, DeciderLRStar> deciders;
        private Dictionary<State, List<ICollection<Terminal>>> lookaheads;

        public string Name { get { return "LR(*)"; } }

        public MethodLRStar() { }

        protected override void OnBeginState(State state)
        {
            DeciderLRStar decider = new DeciderLRStar(state);
            decider.Build(simulator);
            lock (deciders)
            {
                deciders.Add(state, decider);
            }
            List<ICollection<Terminal>> temp = null;
            foreach (Conflict conflict in state.Conflicts)
            {
                if (decider.IsResolved(conflict))
                {
                    conflict.IsResolved = true;
                }
                else if (temp == null)
                {
                    temp = deciders[state].GetUnsolvedPaths();
                    temp.Sort(new System.Comparison<ICollection<Terminal>>(CompareLookaheads));
                    lookaheads.Add(state, temp);
                }
            }
        }
        protected override void OnConflictTreated(State state, Conflict conflict)
        {
            List<ICollection<Terminal>> temp = lookaheads[state];
            List<ICollection<Terminal>> relevant = new List<ICollection<Terminal>>();
            foreach (ICollection<Terminal> path in temp)
            {
                IEnumerator<Terminal> enumerator = path.GetEnumerator();
                enumerator.MoveNext();
                Terminal first = enumerator.Current;
                if (first == conflict.ConflictSymbol)
                    relevant.Add(path);
            }
            if (relevant.Count != 0)
            {
                foreach (ConflictExample example in conflict.Examples)
                {
                    LinkedList<Terminal> rest = new LinkedList<Terminal>(relevant[0]);
                    rest.RemoveFirst();
                    example.Rest.AddRange(rest);
                }
            }
            conflict.Component = "LR(*)";
        }

        private int CompareLookaheads(ICollection<Terminal> left, ICollection<Terminal> right)
        {
            return right.Count - left.Count;
        }

        public IParserData Build(Grammar grammar, Hime.Kernel.Reporting.Reporter reporter) { return Build((CFGrammar)grammar, reporter); }
        public IParserData Build(CFGrammar grammar, Hime.Kernel.Reporting.Reporter reporter)
        {
            this.reporter = reporter;
            reporter.Info("LR(*)", "LR(*) data ...");
            graph = MethodLALR1.ConstructGraph(grammar, reporter);
            deciders = new Dictionary<State, DeciderLRStar>();
            lookaheads = new Dictionary<State, List<ICollection<Terminal>>>();
            Close();
            reporter.Info("LR(*)", graph.States.Count.ToString() + " LALR(1) states constructed.");
            reporter.Info("LR(*)", "Done !");
            return new ParserDataLRStar(this, grammar, graph, deciders);
        }
    }
}
