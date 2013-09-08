/**********************************************************************
* Copyright (c) 2013 Laurent Wouters and others
* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU Lesser General Public License as
* published by the Free Software Foundation, either version 3
* of the License, or (at your option) any later version.
* 
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU Lesser General Public License for more details.
* 
* You should have received a copy of the GNU Lesser General
* Public License along with this program.
* If not, see <http://www.gnu.org/licenses/>.
* 
* Contributors:
*     Laurent Wouters - lwouters@xowl.org
**********************************************************************/

using System.Collections.Generic;

namespace Hime.CentralDogma.Grammars.ContextFree.LR
{
    class DeciderStateLRStar
    {
        private int id;
        private DeciderLRStar decider;
        private int decision;
        private Dictionary<int, GLRSimulatorState> choices; // Item |> {State}
        private Dictionary<Terminal, DeciderStateLRStar> transitions;
        private Dictionary<Terminal, List<DeciderStateLRStar>> incomings;

        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        public int Decision { get { return decision; } }
        public Dictionary<Terminal, DeciderStateLRStar> Transitions { get { return transitions; } }
        public Dictionary<Terminal, List<DeciderStateLRStar>> Incomings { get { return incomings; } }

        public DeciderStateLRStar(DeciderLRStar decider)
        {
            id = 0;
            this.decider = decider;
            decision = -1;
            choices = new Dictionary<int, GLRSimulatorState>();
            transitions = new Dictionary<Terminal, DeciderStateLRStar>();
            incomings = new Dictionary<Terminal, List<DeciderStateLRStar>>();
        }

        public DeciderStateLRStar(DeciderLRStar decider, Dictionary<int, GLRSimulatorState> data)
        {
            id = 0;
            this.decider = decider;
            decision = -1;
            choices = data;
            transitions = new Dictionary<Terminal, DeciderStateLRStar>();
            incomings = new Dictionary<Terminal, List<DeciderStateLRStar>>();
        }
        public DeciderStateLRStar(DeciderLRStar decider, int decision) {
            id = 0;
            this.decider = decider;
            this.decision = decision;
            choices = new Dictionary<int, GLRSimulatorState>();
            transitions = new Dictionary<Terminal, DeciderStateLRStar>();
            incomings = new Dictionary<Terminal, List<DeciderStateLRStar>>();
        }


        public Dictionary<Terminal, DeciderStateLRStar> ComputeNexts(GLRSimulator simulator)
        {
            Dictionary<Terminal, List<int>> conflictuous = ComputeConflictuous();
            Dictionary<Terminal, DeciderStateLRStar> results = new Dictionary<Terminal, DeciderStateLRStar>();
            foreach (Terminal conflict in conflictuous.Keys)
            {
                Dictionary<int, GLRSimulatorState> data = new Dictionary<int, GLRSimulatorState>();
                foreach (int item in conflictuous[conflict])
                {
                    GLRSimulatorState origins = choices[item];
                    GLRSimulatorState finals = simulator.Simulate(origins, conflict);
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

        public void AddTransition(Terminal t, DeciderStateLRStar next)
        {
            if (transitions.ContainsKey(t))
                return;
            transitions.Add(t, next);
            if (!next.incomings.ContainsKey(t))
                next.incomings.Add(t, new List<DeciderStateLRStar>());
            next.incomings[t].Add(this);
        }

        private List<Terminal> ComputeFollowers(GLRSimulatorState choice)
        {
            List<Terminal> result = new List<Terminal>();
            foreach (GLRStackNode node in choice.Nodes)
            {
                State state = node.State;
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


        public override int GetHashCode() { return base.GetHashCode(); }
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
                IList<GLRStackNode> l1 = this.choices[item].Nodes;
                IList<GLRStackNode> l2 = right.choices[item].Nodes;
                if (l1.Count != l2.Count)
                    return false;
                List<int> ids1 = new List<int>();
                List<int> ids2 = new List<int>();
                for (int i = 0; i != l1.Count; i++)
                {
                    ids1.Add(l1[i].State.ID);
                    ids2.Add(l2[i].State.ID);
                }
                foreach (int id in ids1)
                    if (!ids2.Contains(id))
                        return false;
            }
            return true;
        }
    }
}