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
	/// Represents a set of reduction for a LR(0) state
	/// </summary>
	public class StateReductionsLR0 : StateReductions
	{
		/// <summary>
		/// Gets the set of the expected terminals in this set of reductions
		/// </summary>
		/// <remarks>This is always an empty set</remarks>
		public override TerminalSet ExpectedTerminals { get { return new TerminalSet(); } }

		/// <summary>
		/// Initializes this set of reductions as empty
		/// </summary>
		public StateReductionsLR0() : base()
		{
		}

		/// <summary>
		/// Build this set from the given LR state
		/// </summary>
		/// <param name="state">A LR state</param>
		public override void Build(State state)
		{
			Item reduce = null;
			// Look for Reduce actions
			foreach (Item item in state.Items)
			{
				// Ignore Shift actions
				if (item.Action == LRActionCode.Shift)
					continue;
				if (state.Transitions.Count != 0)
				{
					// Conflict Shift/Reduce
					RaiseConflictShiftReduce(state, item, NullTerminal.Instance);
				}
				if (reduce != null)
				{
					// Conflict Reduce/Reduce
					RaiseConflictReduceReduce(state, item, reduce, NullTerminal.Instance);
				}
				else
				{
					this.Add(new StateActionReduce(NullTerminal.Instance, item.BaseRule));
					reduce = item;
				}
			}
		}
	}
}
