/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;

namespace Hime.CentralDogma.Automata
{
    class NFAStateSet : List<NFAState>
    {
        public new void Add(NFAState item)
        {
            if (!base.Contains(item))
                base.Add(item);
        }
        public new void AddRange(IEnumerable<NFAState> items)
        {
            foreach (NFAState item in items)
            {
                if (!base.Contains(item))
                    base.Add(item);
            }
        }


        private void Close_Normal()
        {
            for (int i = 0; i != Count; i++)
                foreach (NFATransition transition in this[i].Transitions)
                    if (transition.span.Equals(NFA.Epsilon))
                        this.Add(transition.next);
        }
        public void Close()
        {
            // Close the set
            Close_Normal();

            NFAState statePositive = null;
            NFAState stateNegative = null;
            foreach (NFAState State in this)
            {
                if (State.Mark > 0) statePositive = State;
                if (State.Mark < 0) stateNegative = State;
            }
            if (statePositive != null && stateNegative != null)
                foreach (NFATransition transition in statePositive.Transitions)
                    if (transition.span.Equals(NFA.Epsilon))
                        this.Remove(transition.next);
        }

        public void Normalize()
        {
            // Trace if modification has occured
            bool modification = true;
            // Repeat while modifications occured
            while (modification)
            {
                modification = false;
                // For each NFA state in the set
                for (int s1 = 0; s1 != this.Count; s1++)
                {
                    // For each transition in this NFA state Set[s1]
                    for (int t1 = 0; t1 != this[s1].Transitions.Count; t1++)
                    {
                        // If this is an ε transition, go to next transition
                        if (this[s1].Transitions[t1].span.Equals(NFA.Epsilon))
                            continue;
                        //Confront to each transition in each NFA state of the set
                        for (int s2 = 0; s2 != this.Count; s2++)
                        {
                            for (int t2 = 0; t2 != this[s2].Transitions.Count; t2++)
                            {
                                if (this[s2].Transitions[t2].span.Equals(NFA.Epsilon))
                                    continue;
                                // If these are not the same transitions of the same state
                                if ((s1 != s2) || (t1 != t2))
                                {
                                    // If the two transition are equal : do nothing
                                    if (this[s1].Transitions[t1].span.Equals(this[s2].Transitions[t2].span))
                                        continue;
                                    // Get the intersection of the two spans
                                    CharSpan Inter = CharSpan.Intersect(this[s1].Transitions[t1].span, this[s2].Transitions[t2].span);
                                    // If no intersection : do nothing
                                    if (Inter.Length == 0)
                                        continue;

                                    // Split transition1 in 1, 2 or 3 transitions and modifiy the states accordingly
                                    CharSpan Part1;
                                    CharSpan Part2;
                                    Part1 = CharSpan.Split(this[s1].Transitions[t1].span, Inter, out Part2);
                                    this[s1].Transitions[t1] = new NFATransition(Inter, this[s1].Transitions[t1].next);
                                    if (Part1.Length != 0) this[s1].AddTransition(Part1, this[s1].Transitions[t1].next);
                                    if (Part2.Length != 0) this[s1].AddTransition(Part2, this[s1].Transitions[t1].next);

                                    // Split transition2 in 1, 2 or 3 transitions and modifiy the states accordingly
                                    Part1 = CharSpan.Split(this[s2].Transitions[t2].span, Inter, out Part2);
                                    this[s2].Transitions[t2] = new NFATransition(Inter, this[s2].Transitions[t2].next);
                                    if (Part1.Length != 0) this[s2].AddTransition(Part1, this[s2].Transitions[t2].next);
                                    if (Part2.Length != 0) this[s2].AddTransition(Part2, this[s2].Transitions[t2].next);
                                    modification = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        public Dictionary<CharSpan, NFAStateSet> GetTransitions()
        {
            Dictionary<CharSpan, NFAStateSet> transitions = new Dictionary<CharSpan, NFAStateSet>();
            // For each state
            foreach (NFAState State in this)
            {
                // For each transition
                foreach (NFATransition transition in State.Transitions)
                {
                    // If this is an ε-transition : pass
                    if (transition.span.Equals(NFA.Epsilon))
                        continue;
                    // Add the transition's target to set's transitions dictionnary
                    if (transitions.ContainsKey(transition.span))
                        transitions[transition.span].Add(transition.next);
                    else
                    {
                        // Create a new child
                        NFAStateSet set = new NFAStateSet();
                        set.Add(transition.next);
                        transitions.Add(transition.span, set);
                    }
                }
            }
            // Close all children
            foreach (NFAStateSet set in transitions.Values)
                set.Close();
            return transitions;
        }

        public List<FinalItem> GetFinals()
        {
            List<FinalItem> finals = new List<FinalItem>();
            foreach (NFAState state in this)
                if (state.Item != null)
                    finals.Add(state.Item);
            return finals;
        }

        public static bool operator ==(NFAStateSet left, NFAStateSet right)
        {
            if (left.Count != right.Count)
                return false;
            foreach (NFAState state in left)
            {
                if (!right.Contains(state))
                    return false;
            }
            return true;
        }
        public static bool operator !=(NFAStateSet left, NFAStateSet right)
        {
            if (left.Count != right.Count)
                return false;
            foreach (NFAState state in left)
            {
                if (!right.Contains(state))
                    return false;
            }
            return true;
        }
        public override bool Equals(object obj)
        {
            if (obj is NFAStateSet)
            {
                NFAStateSet set = (NFAStateSet)obj;
                return (this == set);
            }
            else
                return false;
        }

        public override int GetHashCode() { return base.GetHashCode(); }
    }
}