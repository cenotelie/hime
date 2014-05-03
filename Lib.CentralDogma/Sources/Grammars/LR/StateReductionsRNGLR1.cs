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
	/// Represents a set of reduction for a RNGLR(1) state
	/// </summary>
	public class StateReductionsRNGLR1 : StateReductions
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
		public StateReductionsRNGLR1()
		{
		}

		/// <summary>
		/// Build this set from the given LR state
		/// </summary>
		/// <param name="state">A LR state</param>
		public override void Build(State state)
		{
			// Construct reductions
			foreach (ItemLR1 item in state.Items)
			{
				// Check for right nulled reduction
				if (item.Action == LRActionCode.Shift && !item.BaseRule.Body.Choices[item.DotPosition].Firsts.Contains(Epsilon.Instance))
					continue;
				StateActionRNReduce reduction = new StateActionRNReduce(item.Lookahead, item.BaseRule, item.DotPosition);
				this.Add(reduction);
			}
		}
	}
}