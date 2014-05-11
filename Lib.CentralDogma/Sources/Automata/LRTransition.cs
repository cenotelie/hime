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
using Hime.Redist;

namespace Hime.CentralDogma.Automata
{
	/// <summary>
	/// Represents a transition in a LR automaton
	/// </summary>
	public class LRTransition
	{
		/// <summary>
		/// The label on this transition
		/// </summary>
		private Symbol label;
		/// <summary>
		/// The transition's target
		/// </summary>
		private LRState target;

		/// <summary>
		/// Gets the label on this transition
		/// </summary>
		public Symbol Label { get { return label; } }
		/// <summary>
		/// Gets the target of this transition
		/// </summary>
		public LRState Target { get { return target; } }

		/// <summary>
		/// Initializes this transition
		/// </summary>
		/// <param name="label">The label on this transition</param>
		/// <param name="target">The target of this transition</param>
		public LRTransition(Symbol label, LRState target)
		{
			this.label = label;
			this.target = target;
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Hime.CentralDogma.Automata.LRTransition"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents the current <see cref="Hime.CentralDogma.Automata.LRTransition"/>.
		/// </returns>
		public override string ToString()
		{
			return string.Format("{0} -> {1}", label, target);
		}
	}
}