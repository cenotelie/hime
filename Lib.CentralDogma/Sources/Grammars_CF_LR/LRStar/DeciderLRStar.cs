/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;

namespace Hime.CentralDogma.Grammars.ContextFree.LR
{
    internal class DeciderLRStar
    {
        private State lrstate;
        private List<Item> items;
        private List<DeciderStateLRStar> states;
        private Dictionary<Conflict, DeciderStateLRStar> origins;
        private Dictionary<Conflict, bool> isResolved;

        internal State LRState { get { return lrstate; } }
        internal ICollection<DeciderStateLRStar> States { get { return states; } }
        internal ICollection<Conflict> Conflicts { get { return isResolved.Keys; } }

        internal bool IsResolved(Conflict conflict) { return isResolved[conflict]; }

        internal DeciderLRStar(State state)
        {
            lrstate = state;
            items = new List<Item>(lrstate.Items);
            states = new List<DeciderStateLRStar>();
            origins = new Dictionary<Conflict, DeciderStateLRStar>();
            isResolved = new Dictionary<Conflict, bool>();
        }

        internal Item GetItem(int decision) { return items[decision]; }

        internal void Build(GLRSimulator simulator)
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
            CheckConflicts();
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
            Dictionary<int, GLRSimulatorState> data = new Dictionary<int, GLRSimulatorState>();
            foreach (Item item in conflict.Items)
            {
                int index = items.IndexOf(item);
                GLRSimulatorState finals = simulator.Simulate(lrstate, item, conflict.ConflictSymbol);
                data.Add(index, finals);
            }
            DeciderStateLRStar next = new DeciderStateLRStar(this, data);
            states.Add(next);
            first.AddTransition(conflict.ConflictSymbol, next);
            origins.Add(conflict, next);
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
                    current.AddTransition(t, next);
                }
            }
        }

        private void CheckConflicts()
        {
            foreach (Conflict conflict in origins.Keys)
                isResolved.Add(conflict, CheckConflict(conflict));
        }
        private bool CheckConflict(Conflict conflict)
        {
            List<DeciderStateLRStar> children = new List<DeciderStateLRStar>();
            List<int> children_ids = new List<int>();
            children.Add(origins[conflict]);
            children_ids.Add(origins[conflict].ID);
            for (int i = 0; i != children.Count; i++)
            {
                DeciderStateLRStar current = children[i];
                foreach (Terminal t in current.Transitions.Keys)
                {
                    DeciderStateLRStar next = current.Transitions[t];
                    if (t == Dollar.Instance)
                    {
                        if (next.Decision == -1)
                            return false;
                    }
                    if (!children_ids.Contains(next.ID))
                    {
                        children.Add(next);
                        children_ids.Add(next.ID);
                    }
                }
            }
            return true;
        }

        internal DeciderStateLRStar AddUnique(DeciderStateLRStar candidate)
        {
            foreach (DeciderStateLRStar potential in states)
                if (potential.Equals(candidate))
                    return potential;
            states.Add(candidate);
            return candidate;
        }

        internal List<ICollection<Terminal>> GetUnsolvedPaths()
        {
            List<DeciderStateLRStar> visited = new List<DeciderStateLRStar>();
            List<ICollection<Terminal>> result = new List<ICollection<Terminal>>();
            for (int i = states.Count - 1; i != -1; i--)
            {
                DeciderStateLRStar current = states[i];
                foreach (Terminal terminal in current.Transitions.Keys)
                {
                    DeciderStateLRStar next = current.Transitions[terminal];
                    if (terminal == Dollar.Instance)
                    {
                        if (next.Decision == -1 && !visited.Contains(next))
                        {
                            visited.Add(next);
                            result.AddRange(GetPaths(next));
                        }
                    }
                }
            }
            return result;
        }

        private class ENode
        {
            public DeciderStateLRStar state;
            public ENode next;
            public Terminal transition;
            public ENode(DeciderStateLRStar state, ENode next, Terminal transition)
            {
                this.state = state;
                this.next = next;
                this.transition = transition;
            }
        }

        private List<ICollection<Terminal>> GetPaths(DeciderStateLRStar state)
        {
            Dictionary<int, SortedList<ushort, ENode>> visited = new Dictionary<int, SortedList<ushort, ENode>>();
            LinkedList<ENode> queue = new LinkedList<ENode>();
            List<ENode> goals = new List<ENode>();
            queue.AddFirst(new ENode(state, null, null));

            while (queue.Count != 0)
            {
                ENode current = queue.First.Value;
                queue.RemoveFirst();
                foreach (Terminal s in current.state.Incomings.Keys)
                {
                    foreach (DeciderStateLRStar previous in current.state.Incomings[s])
                    {
                        if (visited.ContainsKey(previous.ID))
                        {
                            if (visited[previous.ID].ContainsKey(s.SID))
                                continue;
                        }
                        else
                            visited.Add(previous.ID, new SortedList<ushort, ENode>());
                        ENode pnode = new ENode(previous, current, s);
                        visited[previous.ID].Add(s.SID, pnode);
                        if (previous.ID == 0)
                            goals.Add(pnode);
                        else
                            queue.AddLast(pnode);
                    }
                }
            }

            List<ICollection<Terminal>> paths = new List<ICollection<Terminal>>();
            foreach (ENode start in goals)
            {
                ENode node = start;
                LinkedList<Terminal> path = new LinkedList<Terminal>();
                while (node.next != null)
                {
                    path.AddLast(node.transition);
                    node = node.next;
                }
                paths.Add(path);
            }
            return paths;
        }
    }
}