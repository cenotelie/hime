/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;
using Hime.Kernel.Reporting;

namespace Hime.Parsers.ContextFree.LR
{
    class MethodLRStar : BaseMethod
    {
        private Dictionary<State, DeciderLRStar> deciders;
        private Dictionary<State, List<ICollection<Terminal>>> lookaheads;

        public override string Name { get { return "LR(*)"; } }

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
            conflict.Component = "LR(*)";
        }

        private int CompareLookaheads(ICollection<Terminal> left, ICollection<Terminal> right)
        {
            return right.Count - left.Count;
        }

        public override ParserData Build(CFGrammar grammar, Reporter reporter)
        {
			base.Build(grammar, reporter);
            deciders = new Dictionary<State, DeciderLRStar>();
            lookaheads = new Dictionary<State, List<ICollection<Terminal>>>();
            Close();
            this.ReportInfo(graph.States.Count.ToString() + " LALR(1) states constructed.");
            this.ReportInfo("Done !");
            return new ParserDataLRStar(reporter, grammar, graph, deciders);
        }
		
		protected override Graph BuildGraph (CFGrammar grammar)
		{
			return MethodLALR1.ConstructGraph(grammar);
		}
    }
}
