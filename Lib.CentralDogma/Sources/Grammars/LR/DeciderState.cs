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

namespace Hime.CentralDogma.Grammars.LR
{
	/// <summary>
	/// Represents a state in a decider automaton for the LR(*) parsing method
	/// </summary>
	public class DeciderState
	{
		/// <summary>
		/// The state's identifier
		/// </summary>
		private int id;
		/// <summary>
		/// The parent decider
		/// </summary>
		private Decider decider;
		/// <summary>
		/// The decision, as an index selecting the correct item in a LR state
		/// </summary>
		private int decision;
		/// <summary>
		/// The choices mapping indices of items in a LR state to a GLR generation
		/// </summary>
		private Dictionary<int, GLRGeneration> choices;
		/// <summary>
		/// The outgoing transitions in this state
		/// </summary>
		private Dictionary<Terminal, DeciderState> transitions;
		/// <summary>
		/// The incomings transitions in this state
		/// </summary>
		private Dictionary<Terminal, List<DeciderState>> incomings;

		/// <summary>
		/// Gets or sets the identifier of this state
		/// </summary>
		public int ID
		{
			get { return id; }
			set { id = value; }
		}
		/// <summary>
		/// Gets the decision, or -1 if none
		/// </summary>
		public int Decision { get { return decision; } }
		/// <summary>
		/// Gets the outgoing transitions
		/// </summary>
		public Dictionary<Terminal, DeciderState> Transitions { get { return transitions; } }
		/// <summary>
		/// Gets the incomings transitions
		/// </summary>
		public Dictionary<Terminal, List<DeciderState>> Incomings { get { return incomings; } }

		/// <summary>
		/// Initializes this state with the parent decider
		/// </summary>
		/// <param name="decider">The parent decider</param>
		public DeciderState(Decider decider)
		{
			this.id = 0;
			this.decider = decider;
			this.decision = -1;
			this.choices = new Dictionary<int, GLRGeneration>();
			this.transitions = new Dictionary<Terminal, DeciderState>();
			this.incomings = new Dictionary<Terminal, List<DeciderState>>();
		}

		/// <summary>
		/// Initializes this state with the parent decider
		/// </summary>
		/// <param name="decider">The parent decider</param>
		/// <param name="data">The choices for this state</param>
		public DeciderState(Decider decider, Dictionary<int, GLRGeneration> data)
		{
			this.id = 0;
			this.decider = decider;
			this.decision = -1;
			this.choices = data;
			this.transitions = new Dictionary<Terminal, DeciderState>();
			this.incomings = new Dictionary<Terminal, List<DeciderState>>();
		}

		/// <summary>
		/// Initializes this state with the parent decider
		/// </summary>
		/// <param name="decider">The parent decider</param>
		/// <param name="decision">The decision made at this state</param>
		public DeciderState(Decider decider, int decision)
		{
			this.id = 0;
			this.decider = decider;
			this.decision = decision;
			this.choices = new Dictionary<int, GLRGeneration>();
			this.transitions = new Dictionary<Terminal, DeciderState>();
			this.incomings = new Dictionary<Terminal, List<DeciderState>>();
		}

		/// <summary>
		/// Computes the next states based on the specified simulator
		/// </summary>
		/// <param name="simulator">A GLR simulator</param>
		/// <returns>The possible children of this state according to the simulator</returns>
		public Dictionary<Terminal, DeciderState> ComputeNexts(GLRSimulator simulator)
		{
			Dictionary<Terminal, List<int>> conflictuous = ComputeConflictuous();
			Dictionary<Terminal, DeciderState> results = new Dictionary<Terminal, DeciderState>();
			foreach (Terminal conflict in conflictuous.Keys)
			{
				Dictionary<int, GLRGeneration> data = new Dictionary<int, GLRGeneration>();
				foreach (int item in conflictuous[conflict])
				{
					GLRGeneration origins = choices[item];
					GLRGeneration finals = simulator.Simulate(origins, conflict);
					data.Add(item, finals);
				}
				results.Add(conflict, new DeciderState(decider, data));
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
			DeciderState decision = new DeciderState(decider, item);
			transitions.Add(t, decider.AddUnique(decision));
		}

		public void AddTransition(Terminal t, DeciderState next)
		{
			if (transitions.ContainsKey(t))
				return;
			transitions.Add(t, next);
			if (!next.incomings.ContainsKey(t))
				next.incomings.Add(t, new List<DeciderState>());
			next.incomings[t].Add(this);
		}

		private List<Terminal> ComputeFollowers(GLRGeneration choice)
		{
			List<Terminal> result = new List<Terminal>();
			foreach (GLRStackNode node in choice.Nodes)
			{
				State state = node.State;
				foreach (Symbol s in state.Transitions)
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

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (!(obj is DeciderState))
				return false;
			DeciderState right = (DeciderState)obj;
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
				ROList<GLRStackNode> l1 = this.choices[item].Nodes;
				ROList<GLRStackNode> l2 = right.choices[item].Nodes;
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