using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class DeciderLRStar
    {
        private State lrstate;
        private List<Item> items;
        private List<DeciderStateLRStar> states;

        public State LRState { get { return lrstate; } }
        public ICollection<DeciderStateLRStar> States { get { return states; } }

        public DeciderLRStar(State state)
        {
            lrstate = state;
            items = new List<Item>(lrstate.Items);
            states = new List<DeciderStateLRStar>();
        }

        public Item GetItem(int decision) { return items[decision]; }

        public void Build(GLRSimulator simulator)
        {
            DeciderStateLRStar first = new DeciderStateLRStar(this);
            states.Add(first);

            List<Terminal> conflicts = new List<Terminal>();
            foreach (Conflict c in lrstate.Conflicts)
                conflicts.Add(c.ConflictSymbol);
            int i = 0;
            foreach (Item item in lrstate.Items)
            {
                if (item.Action == ItemAction.Shift)
                    BuildFirst_Shift(first, item, i, conflicts);
                else
                    BuildFirst_Reduction(first, item, i, conflicts);
                i++;
            }
            foreach (Conflict c in lrstate.Conflicts)
                BuildFirst_Conflict(first, c, simulator);
            Close(simulator);
        }
        private void BuildFirst_Shift(DeciderStateLRStar first, Item item, int index, List<Terminal> conflicts)
        {
            Symbol symbol = item.NextSymbol;
            if (symbol is Terminal)
            {
                Terminal t = (Terminal)symbol;
                if (!conflicts.Contains(t))
                {
                    first.AddDecision(index, t);
                    return;
                }
            }
        }
        private void BuildFirst_Reduction(DeciderStateLRStar first, Item item, int index, List<Terminal> conflicts)
        {
            foreach (Terminal t in item.Lookaheads)
            {
                if (!conflicts.Contains(t))
                    first.AddDecision(index, t);
            }
        }
        private void BuildFirst_Conflict(DeciderStateLRStar first, Conflict conflict, GLRSimulator simulator)
        {
            Dictionary<int, List<State>> data = new Dictionary<int, List<State>>();
            foreach (Item item in conflict.Items)
            {
                int index = items.IndexOf(item);
                List<State> finals = simulator.Simulate(lrstate, item, conflict.ConflictSymbol);
                data.Add(index, finals);
            }
            DeciderStateLRStar next = new DeciderStateLRStar(this, data);
            states.Add(next);
            first.Transitions.Add(conflict.ConflictSymbol, next);
        }

        private void Close(GLRSimulator simulator)
        {
            for (int i = 1; i != states.Count; i++)
            {
                DeciderStateLRStar current = states[i];
                current.ID = i;
                if (current.Decision != -1)
                    continue;
                Dictionary<Terminal, DeciderStateLRStar> nexts = states[i].ComputeNexts(simulator);
                foreach (Terminal t in nexts.Keys)
                {
                    DeciderStateLRStar next = nexts[t];
                    next = AddUnique(next);
                    current.Transitions.Add(t, next);
                }
            }
        }

        public DeciderStateLRStar AddUnique(DeciderStateLRStar candidate)
        {
            foreach (DeciderStateLRStar potential in states)
                if (potential.Equals(candidate))
                    return potential;
            states.Add(candidate);
            return candidate;
        }
    }

    class DeciderStateLRStar
    {
        private int id;
        private DeciderLRStar decider;
        private int decision;
        private Dictionary<int, List<State>> choices; // Item |> {State}
        private Dictionary<Terminal, DeciderStateLRStar> transitions;

        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        public int Decision { get { return decision; } }
        public Dictionary<Terminal, DeciderStateLRStar> Transitions { get { return transitions; } }

        public DeciderStateLRStar(DeciderLRStar decider)
        {
            id = 0;
            this.decider = decider;
            decision = -1;
            choices = new Dictionary<int, List<State>>();
            transitions = new Dictionary<Terminal, DeciderStateLRStar>();
        }

        public DeciderStateLRStar(DeciderLRStar decider, Dictionary<int, List<State>> data)
        {
            id = 0;
            this.decider = decider;
            decision = -1;
            choices = data;
            transitions = new Dictionary<Terminal, DeciderStateLRStar>();
        }
        public DeciderStateLRStar(DeciderLRStar decider, int decision) {
            id = 0;
            this.decider = decider;
            this.decision = decision;
            choices = new Dictionary<int, List<State>>();
            transitions = new Dictionary<Terminal, DeciderStateLRStar>();
        }


        public Dictionary<Terminal, DeciderStateLRStar> ComputeNexts(GLRSimulator simulator)
        {
            Dictionary<Terminal, List<int>> conflictuous = ComputeConflictuous();
            Dictionary<Terminal, DeciderStateLRStar> results = new Dictionary<Terminal, DeciderStateLRStar>();
            foreach (Terminal conflict in conflictuous.Keys)
            {
                Dictionary<int, List<State>> data = new Dictionary<int, List<State>>();
                foreach (int item in conflictuous[conflict])
                {
                    List<State> origins = choices[item];
                    List<State> finals = simulator.Simulate(origins, conflict);
                    data.Add(item, finals);
                }
                results.Add(conflict, new DeciderStateLRStar(decider, data));
            }
            return results;
        }

        private Dictionary<Terminal, List<int>> ComputeConflictuous()
        {
            Dictionary<int, List<Terminal>> followers = new Dictionary<int, List<Terminal>>();
            foreach (int item in choices.Keys)
                followers.Add(item, ComputeFollowers(choices[item]));

            List<int> items = new List<int>(followers.Keys);
            Dictionary<Terminal, List<int>> conflicts = new Dictionary<Terminal, List<int>>();
            for (int i = 0; i != items.Count; i++)
            {
                foreach (Terminal t in followers[items[i]])
                {
                    if (conflicts.ContainsKey(t))
                    {
                        List<int> temp = conflicts[t];
                        if (!temp.Contains(items[i]))
                            temp.Add(items[i]);
                        continue;
                    }
                    bool found = false;
                    for (int j = i + 1; j != items.Count; j++)
                    {
                        if (followers[items[j]].Contains(t))
                        {
                            found = true;
                            List<int> temp = new List<int>();
                            temp.Add(items[i]);
                            temp.Add(items[j]);
                            conflicts.Add(t, temp);
                            break;
                        }
                    }
                    if (!found)
                        AddDecision(items[i], t);
                }
            }
            return conflicts;
        }

        public void AddDecision(int item, Terminal t)
        {
            if (transitions.ContainsKey(t))
                return;
            DeciderStateLRStar decision = new DeciderStateLRStar(decider, item);
            transitions.Add(t, decider.AddUnique(decision));
        }

        private List<Terminal> ComputeFollowers(List<State> choice)
        {
            List<Terminal> result = new List<Terminal>();
            foreach (State state in choice)
            {
                foreach (Symbol s in state.Children.Keys)
                {
                    if (!(s is Terminal))
                        continue;
                    Terminal t = (Terminal)s;
                    if (!result.Contains(t))
                        result.Add(t);
                }
                foreach (Terminal t in state.Reductions.ExpectedTerminals)
                    if (!result.Contains(t))
                        result.Add(t);
            }
            return result;
        }



        public override bool Equals(object obj)
        {
            if (!(obj is DeciderStateLRStar))
                return false;
            DeciderStateLRStar right = (DeciderStateLRStar)obj;
            if (this.decision != -1)
                return (this.decision == right.decision);
            if (right.decision != -1)
                return false;
            if (this.choices == null)
                return false;
            if (right.choices == null)
                return false;
            if (this.choices.Count != right.choices.Count)
                return false;
            foreach (int item in this.choices.Keys)
            {
                if (!right.choices.ContainsKey(item))
                    return false;
                List<State> l1 = this.choices[item];
                List<State> l2 = right.choices[item];
                if (l1.Count != l2.Count)
                    return false;
                List<int> ids1 = new List<int>();
                List<int> ids2 = new List<int>();
                for (int i = 0; i != l1.Count; i++)
                {
                    ids1.Add(l1[i].ID);
                    ids2.Add(l2[i].ID);
                }
                foreach (int id in ids1)
                    if (!ids2.Contains(id))
                        return false;
            }
            return true;
        }
    }
}