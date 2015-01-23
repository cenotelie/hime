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

namespace Hime.SDK.Grammars.LR
{
	/// <summary>
	/// Represents a set of reduction for a RNGLALR(1) state
	/// </summary>
	public class StateReductionsRNGLALR1 : StateReductions
	{
		/// <summary>
		/// Gets the set of the expected terminals in this set of reductions
		/// </summary>
		/// <remarks>This is always an empty set</remarks>
		public override TerminalSet ExpectedTerminals
		{
			get
			{
				TerminalSet result = new TerminalSet();
				foreach (StateActionReduce reduction in this)
					result.Add(reduction.Lookahead);
				return result;
			}
		}

		/// <summary>
		/// Initializes this set of reductions as empty
		/// </summary>
		public StateReductionsRNGLALR1() : base()
		{
		}

		/// <summary>
		/// Build this set from the given LR state
		/// </summary>
		/// <param name="state">A LR state</param>
		public override void Build(State state)
		{
			// Recutions dictionnary for the given set
			Dictionary<Terminal, ItemLALR1> reductions = new Dictionary<Terminal, ItemLALR1>();
			// Construct reductions
			foreach (ItemLALR1 item in state.Items)
			{
				// Check for right nulled reduction
				if (item.Action == LRActionCode.Shift && !item.BaseRule.Body.Choices[item.DotPosition].Firsts.Contains(Epsilon.Instance))
					continue;
				foreach (Terminal lookahead in item.Lookaheads)
				{
					// There is already a shift action for the lookahead => conflict
					if (state.HasTransition(lookahead))
						RaiseConflictShiftReduce(state, item, lookahead);
					// There is already a reduction action for the lookahead => conflict
					else if (reductions.ContainsKey(lookahead))
						RaiseConflictReduceReduce(state, item, reductions[lookahead], lookahead);
					else // No conflict
						reductions.Add(lookahead, item);
					StateActionRNReduce reduction = new StateActionRNReduce(lookahead, item.BaseRule, item.DotPosition);
					this.Add(reduction);
				}
			}
		}
	}
}
