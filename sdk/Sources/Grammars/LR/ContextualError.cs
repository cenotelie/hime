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

using System.Text;

namespace Hime.SDK.Grammars.LR
{
	/// <summary>
	/// Represents an error where a contextual terminal is expected but its context cannot be available at this point
	/// </summary>
	public class ContextualError : Error
	{
		/// <summary>
		/// Gets the problematic contextual terminal
		/// </summary>
		public Terminal Terminal { get { return errorItems[0].GetNextSymbol() as Terminal; } }

		/// <summary>
		/// Initializes this error
		/// </summary>
		/// <param name="state">The state raising the error</param>
		public ContextualError(State state) : base(state, ErrorType.ErrorContextualTerminal)
		{
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Hime.SDK.Grammars.LR.Error"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents the current <see cref="Hime.SDK.Grammars.LR.Error"/>.
		/// </returns>
		public override string ToString()
		{
			StringBuilder builder = new StringBuilder("Context error in ");
			builder.Append(errorState.ID);
			builder.Append(", the context ");
			builder.Append(Terminal.Context);
			builder.Append(" cannot be available for terminal '");
			builder.Append(Terminal.ToString());
			builder.Append("' in {");
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
