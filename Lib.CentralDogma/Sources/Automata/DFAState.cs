using System.Collections.Generic;

namespace Hime.CentralDogma.Automata
{
    class DFAState
    {
        private Dictionary<CharSpan, DFAState> transitions;
        private FinalItem item;
        private List<FinalItem> items;
        private int iD;

        public FinalItem TopItem { get { return item; } }
        public List<FinalItem> Items { get { return items; } }
        public Dictionary<CharSpan, DFAState> Transitions { get { return transitions; } }
        public int FinalsCount { get { return items.Count; } }
        public int TransitionsCount { get { return transitions.Count; } }
        public int ID
        {
            get { return iD; }
            set { iD = value; }
        }

        public DFAState()
        {
            items = new List<FinalItem>();
            transitions = new Dictionary<CharSpan, DFAState>();
        }

        public void AddFinal(FinalItem item)
        {
            if (!items.Contains(item))
            {
                items.Add(item);
                if (this.item == null)
                    this.item = item;
                else
                {
                    if (item.Priority > this.item.Priority)
                        this.item = item;
                }
            }
        }
        public void AddFinals(ICollection<FinalItem> items)
        {
            foreach (FinalItem item in items)
                AddFinal(item);
        }
        public void ClearFinals() { items.Clear(); item = null; }

        public void AddTransition(CharSpan value, DFAState next) { transitions.Add(value, next); }
        public void ClearTransitions() { transitions.Clear(); }

        public void RepackTransitions()
        {
            Dictionary<DFAState, List<CharSpan>> inverse = new Dictionary<DFAState, List<CharSpan>>();
            foreach (KeyValuePair<CharSpan, DFAState> transition in transitions)
            {
                if (!inverse.ContainsKey(transition.Value))
                    inverse.Add(transition.Value, new List<CharSpan>());
                inverse[transition.Value].Add(transition.Key);
            }
            transitions.Clear();
            foreach (DFAState child in inverse.Keys)
            {
                List<CharSpan> keys = inverse[child];
                keys.Sort(new System.Comparison<CharSpan>(CharSpan.Compare));
                for (int i = 0; i != keys.Count; i++)
                {
                    CharSpan k1 = keys[i];
                    for (int j = i + 1; j != keys.Count; j++)
                    {
                        CharSpan k2 = keys[j];
                        if (k2.Begin == k1.End + 1)
                        {
                            k1 = new CharSpan(k1.Begin, k2.End);
                            keys[i] = k1;
                            keys.RemoveAt(j);
                            j--;
                        }
                    }
                }
                foreach (CharSpan key in keys)
                    transitions.Add(key, child);
            }
        }
    }
}