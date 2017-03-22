/*******************************************************************************
 * Copyright (c) 2017 Association Cénotélie (cenotelie.fr)
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
 ******************************************************************************/

using Hime.Redist.Parsers;

namespace Hime.SDK.Grammars.LR
{
	/// <summary>
	/// Represents a reduction action in a LR state
	/// </summary>
	public class StateActionReduce : StateAction
	{
		/// <summary>
		/// The lookahead to reduce on
		/// </summary>
		private readonly Terminal lookahead;
		/// <summary>
		/// The rule to reduce
		/// </summary>
		private readonly Rule toReduce;

		/// <summary>
		///  Gets the type of action 
		/// </summary>
		[System.CLSCompliant(false)]
		public LRActionCode ActionType { get { return LRActionCode.Shift; } }

		/// <summary>
		///  Gets the trigger for the action 
		/// </summary>
		public Symbol OnSymbol { get { return lookahead; } }

		/// <summary>
		/// Gets the lookahead for this action
		/// </summary>
		public Terminal Lookahead { get { return lookahead; } }

		/// <summary>
		/// Gets the rule to reduce
		/// </summary>
		public Rule ToReduceRule { get { return toReduce; } }

		/// <summary>
		/// Initializes this action
		/// </summary>
		/// <param name="lookahead">The lookahead to reduce on</param>
		/// <param name="rule">The rule to reduce</param>
		public StateActionReduce(Terminal lookahead, Rule rule)
		{
			this.lookahead = lookahead;
			toReduce = rule;
		}
	}
}