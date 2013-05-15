using System.Collections.Generic;

namespace Hime.CentralDogma.Grammars.ContextFree.LR
{
    abstract class StateReductions
    {
        private List<StateActionReduce> content;
        protected List<Conflict> conflicts;
        public ICollection<Conflict> Conflicts { get { return conflicts; } }

        public StateReductions()
        {
            this.content = new List<StateActionReduce>();
            this.conflicts = new List<Conflict>();
        }

        public abstract TerminalSet ExpectedTerminals { get; }
        public abstract void Build(State state);

        public void Add(StateActionReduce action)
        {
            this.content.Add(action);
        }

        public IEnumerator<StateActionReduce> GetEnumerator()
        {
            return this.content.GetEnumerator();
        }

        public int Count { get { return this.content.Count; } }

        public StateActionReduce this[int index]
        {
            get { return this.content[index]; }
        }
    }
}