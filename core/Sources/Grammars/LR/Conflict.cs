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

namespace Hime.SDK.Grammars.LR
{
	/// <summary>
	/// Represents a LR conflict
	/// </summary>
	public class Conflict : Error
	{
		/// <summary>
		/// The conflictuous lookahead
		/// </summary>
		private readonly Terminal lookahead;

		/// <summary>
		/// Gets the conflictuous terminal
		/// </summary>
		public Terminal ConflictSymbol { get { return lookahead; } }

		/// <summary>
		/// Initializes this conflict
		/// </summary>
		/// <param name="state">The state raising the conflict</param>
		/// <param name="type">The type of conflict</param>
		/// <param name="lookahead">The conflictuous lookahead</param>
		public Conflict(State state, ErrorType type, Terminal lookahead) : base(state, type)
		{
			this.lookahead = lookahead;
		}

		/// <summary>
		/// Initializes this conflict
		/// </summary>
		/// <param name="state">The state raising the state</param>
		/// <param name="type">The type of conflict</param>
		public Conflict(State state, ErrorType type) : base(state, type)
		{
			lookahead = null;
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Hime.SDK.Grammars.LR.Conflict"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents the current <see cref="Hime.SDK.Grammars.LR.Conflict"/>.
		/// </returns>
		public override string ToString()
		{
			System.Text.StringBuilder builder = new System.Text.StringBuilder("Conflict ");
			if (type == ErrorType.ConflictShiftReduce)
				builder.Append("Shift/Reduce");
			else
				builder.Append("Reduce/Reduce");
			builder.Append(" in ");
			builder.Append(errorState.ID);
			if (lookahead != null)
			{
				builder.Append(" on terminal '");
				builder.Append(lookahead.ToString());
				builder.Append("'");
			}
			builder.Append(" for items {");
			foreach (Item item in errorItems)
			{
				builder.Append(" ");
				builder.Append(item.ToString());
			}
			builder.Append(" }");
			return builder.ToString();
		}
	}
}
