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
using Hime.Redist.Parsers;

namespace Hime.SDK.Grammars.LR
{
	/// <summary>
	/// Represents a shift action in a LR state
	/// </summary>
	public class StateActionShift : StateAction
	{
		/// <summary>
		/// The trigger symbol
		/// </summary>
		private readonly Symbol symbol;
		/// <summary>
		/// The target of the shift action
		/// </summary>
		private readonly State target;

		/// <summary>
		///  Gets the type of action 
		/// </summary>
		[System.CLSCompliant(false)]
		public LRActionCode ActionType { get { return LRActionCode.Shift; } }

		/// <summary>
		///  Gets the trigger for the action 
		/// </summary>
		public Symbol OnSymbol { get { return symbol; } }

		/// <summary>
		/// Gets the target of this action
		/// </summary>
		public State Target { get { return target; } }

		/// <summary>
		/// Initializes this action
		/// </summary>
		/// <param name="trigger">The action's trigger</param>
		/// <param name="target">The shift target</param>
		public StateActionShift(Symbol trigger, State target)
		{
			symbol = trigger;
			this.target = target;
		}
	}
}