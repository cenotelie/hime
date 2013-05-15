using System.Collections.Generic;

namespace Hime.CentralDogma.Grammars.ContextFree.LR
{
    class MethodLRStar : ParserGenerator
    {
        private Dictionary<State, DeciderLRStar> deciders;
        private Dictionary<State, List<ICollection<Terminal>>> lookaheads;

        public MethodLRStar(Reporting.Reporter reporter)
            : base("LR(*)", reporter)
		{
		}

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
		
		// TODO: code not nice, too complex
		protected override void OnConflictTreated(State state, Conflict conflict)
        {
            List<ICollection<Terminal>> temp = lookaheads[state];
            List<ICollection<Terminal>> relevant = new List<ICollection<Terminal>>();
            foreach (ICollection<Terminal> path in temp)
            {
                using (IEnumerator<Terminal> enumerator = path.GetEnumerator())
				{
                	enumerator.MoveNext();
                	Terminal first = enumerator.Current;
                	if (first == conflict.ConflictSymbol) relevant.Add(path);
				}
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
        }

        private int CompareLookaheads(ICollection<Terminal> left, ICollection<Terminal> right)
        {
            return right.Count - left.Count;
        }
		
		protected override Graph BuildGraph (CFGrammar grammar)
		{
			Graph result = MethodLALR1.ConstructGraph(grammar);
            deciders = new Dictionary<State, DeciderLRStar>();
            lookaheads = new Dictionary<State, List<ICollection<Terminal>>>();
			return result;
		}
		
		protected override ParserData BuildParserData (CFGrammar grammar)
		{
			return new ParserDataLRStar(this.reporter, grammar, this.graph, this.deciders);
		}
    }
}
