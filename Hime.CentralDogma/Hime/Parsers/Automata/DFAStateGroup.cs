using System.Collections.Generic;

namespace Hime.Parsers.Automata
{
    public sealed class DFAStateGroup
    {
        private List<DFAState> states;

        public ICollection<DFAState> States { get { return states; } }

        public DFAState Representative { get { return states[0]; } }

        public DFAStateGroup(DFAState init)
        {
            states = new List<DFAState>();
            states.Add(init);
        }

        public void AddState(DFAState state) { states.Add(state); }

        public DFAPartition Split(DFAPartition current)
        {
            DFAPartition partition = new DFAPartition();
            foreach (DFAState state in states)
                partition.AddState(state, current);
            return partition;
        }

        public bool Contains(DFAState state) { return states.Contains(state); }
    }
}