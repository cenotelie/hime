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
using Hime.Redist.Parsers;

namespace Hime.CentralDogma.Grammars.LR
{
	/// <summary>
	/// Represents a decider for solving conflicts in a LR(*) parsing method
	/// </summary>
	public class Decider
	{
		/// <summary>
		/// The LR state that has a conflict
		/// </summary>
		private State lrstate;
		/// <summary>
		/// The conflictuous items
		/// </summary>
		private List<Item> items;
		/// <summary>
		/// The states of this decider
		/// </summary>
		private List<DeciderState> states;
		/// <summary>
		/// The entry state in the decider depending on the conflict that we need to solve
		/// </summary>
		private Dictionary<Conflict, DeciderState> origins;
		/// <summary>
		/// Whether each conflict is resolved
		/// </summary>
		private Dictionary<Conflict, bool> isResolved;

		/// <summary>
		/// Gets the LR state for this decider
		/// </summary>
		public State LRState { get { return lrstate; } }
		/// <summary>
		/// Gets the decider states
		/// </summary>
		public ICollection<DeciderState> States { get { return states; } }
		/// <summary>
		/// Gets the conflicts attempted to be solved by this decider
		/// </summary>
		public ICollection<Conflict> Conflicts { get { return isResolved.Keys; } }
		/// <summary>
		/// Determines whether the given conflict is solved by this decider
		/// </summary>
		/// <param name="conflict">A conflict in the original LR state</param>
		/// <returns><c>true</c> if the specified conflict is solved; otherwise, <c>false</c></returns>
		public bool IsResolved(Conflict conflict)
		{
			return isResolved[conflict];
		}

		/// <summary>
		/// Initializes a decider for the specified LR state
		/// </summary>
		/// <param name="state">A LR state</param>
		public Decider(State state)
		{
			lrstate = state;
			items = new List<Item>(lrstate.Items);
			states = new List<DeciderState>();
			origins = new Dictionary<Conflict, DeciderState>();
			isResolved = new Dictionary<Conflict, bool>();
		}

		/// <summary>
		/// Gets the item indicated by the given decision
		/// </summary>
		/// <param name="decision">A decision</param>
		/// <returns>The item indicated by the decision</returns>
		public Item GetItem(int decision)
		{
			return items[decision];
		}

		/// <summary>
		/// Build this decider with the specified simulator
		/// </summary>
		/// <param name="simulator">A GLR simulator</param>
		public void Build(GLRSimulator simulator)
		{
			DeciderState first = new DeciderState(this);
			states.Add(first);

			List<Terminal> conflicts = new List<Terminal>();
			foreach (Conflict c in lrstate.Conflicts)
				conflicts.Add(c.ConflictSymbol);
			int i = 0;
			foreach (Item item in lrstate.Items)
			{
				if (item.Action == LRActionCode.Shift)
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

		private void BuildFirst_Shift(DeciderState first, Item item, int index, List<Terminal> conflicts)
		{
			Symbol symbol = item.GetNextSymbol();
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

		private void BuildFirst_Reduction(DeciderState first, Item item, int index, List<Terminal> conflicts)
		{
			foreach (Terminal t in item.Lookaheads)
			{
				if (!conflicts.Contains(t))
					first.AddDecision(index, t);
			}
		}

		private void BuildFirst_Conflict(DeciderState first, Conflict conflict, GLRSimulator simulator)
		{
			Dictionary<int, GLRGeneration> data = new Dictionary<int, GLRGeneration>();
			foreach (Item item in conflict.Items)
			{
				int index = items.IndexOf(item);
				GLRGeneration finals = simulator.Simulate(lrstate, item, conflict.ConflictSymbol);
				data.Add(index, finals);
			}
			DeciderState next = new DeciderState(this, data);
			states.Add(next);
			first.AddTransition(conflict.ConflictSymbol, next);
			origins.Add(conflict, next);
		}

		private void Close(GLRSimulator simulator)
		{
			for (int i = 1; i != states.Count; i++)
			{
				DeciderState current = states[i];
				current.ID = i;
				if (current.Decision != -1)
					continue;
				Dictionary<Terminal, DeciderState> nexts = states[i].ComputeNexts(simulator);
				foreach (Terminal t in nexts.Keys)
				{
					DeciderState next = nexts[t];
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
			List<DeciderState> children = new List<DeciderState>();
			List<int> children_ids = new List<int>();
			children.Add(origins[conflict]);
			children_ids.Add(origins[conflict].ID);
			for (int i = 0; i != children.Count; i++)
			{
				DeciderState current = children[i];
				foreach (Terminal t in current.Transitions.Keys)
				{
					DeciderState next = current.Transitions[t];
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

		internal DeciderState AddUnique(DeciderState candidate)
		{
			foreach (DeciderState potential in states)
				if (potential.Equals(candidate))
					return potential;
			states.Add(candidate);
			return candidate;
		}

		internal List<ICollection<Terminal>> GetUnsolvedPaths()
		{
			List<DeciderState> visited = new List<DeciderState>();
			List<ICollection<Terminal>> result = new List<ICollection<Terminal>>();
			for (int i = states.Count - 1; i != -1; i--)
			{
				DeciderState current = states[i];
				foreach (Terminal terminal in current.Transitions.Keys)
				{
					DeciderState next = current.Transitions[terminal];
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
			public DeciderState state;
			public ENode next;
			public Terminal transition;

			public ENode(DeciderState state, ENode next, Terminal transition)
			{
				this.state = state;
				this.next = next;
				this.transition = transition;
			}
		}

		private List<ICollection<Terminal>> GetPaths(DeciderState state)
		{
			Dictionary<int, SortedList<int, ENode>> visited = new Dictionary<int, SortedList<int, ENode>>();
			LinkedList<ENode> queue = new LinkedList<ENode>();
			List<ENode> goals = new List<ENode>();
			queue.AddFirst(new ENode(state, null, null));

			while (queue.Count != 0)
			{
				ENode current = queue.First.Value;
				queue.RemoveFirst();
				foreach (Terminal s in current.state.Incomings.Keys)
				{
					foreach (DeciderState previous in current.state.Incomings[s])
					{
						if (visited.ContainsKey(previous.ID))
						{
							if (visited[previous.ID].ContainsKey(s.ID))
								continue;
						}
						else
							visited.Add(previous.ID, new SortedList<int, ENode>());
						ENode pnode = new ENode(previous, current, s);
						visited[previous.ID].Add(s.ID, pnode);
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