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
using Hime.Redist;

namespace Hime.SDK.Automata
{
	/// <summary>
	/// Represents a state in an LR automaton
	/// </summary>
	public class LRState
	{
		/// <summary>
		/// The state's identifier
		/// </summary>
		private int id;
		/// <summary>
		/// The transitions from this state
		/// </summary>
		private List<LRTransition> transitions;
		/// <summary>
		/// The reductions in this state
		/// </summary>
		private List<LRReduction> reductions;
		/// <summary>
		/// Whether this state is an accepting state
		/// </summary>
		private bool accept;

		/// <summary>
		/// Gets this state's identifier
		/// </summary>
		public int ID { get { return id; } }
		/// <summary>
		/// Gets the transitions from this state
		/// </summary>
		public ROList<LRTransition> Transitions { get { return new ROList<LRTransition>(transitions); } }
		/// <summary>
		/// Gets the reductions in this state
		/// </summary>
		public ROList<LRReduction> Reductions { get { return new ROList<LRReduction>(reductions); } }
		/// <summary>
		/// Gets or sets whether this state is an accepting state
		/// </summary>
		public bool IsAccept
		{
			get { return accept; }
			set { accept = value; }
		}

		/// <summary>
		/// Initializes this LR state
		/// </summary>
		/// <param name="id">The state's identifier</param>
		public LRState(int id)
		{
			this.id = id;
			this.transitions = new List<LRTransition>();
			this.reductions = new List<LRReduction>();
		}

		/// <summary>
		/// Adds the specified transition to this state
		/// </summary>
		/// <param name="transition">A transition</param>
		public void AddTransition(LRTransition transition)
		{
			transitions.Add(transition);
		}

		/// <summary>
		/// Adds the specified reduction to this state
		/// </summary>
		/// <param name="reduction">A reduction</param>
		public void AddReduction(LRReduction reduction)
		{
			reductions.Add(reduction);
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Hime.SDK.Automata.LRState"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents the current <see cref="Hime.SDK.Automata.LRState"/>.
		/// </returns>
		public override string ToString()
		{
			return string.Format("({0})", id);
		}
	}
}