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

namespace Hime.SDK.Grammars
{
	/// <summary>
	/// Represents a symbol for a semantic action in a grammar
	/// </summary>
	public class Action : Symbol
	{
		/// <summary>
		/// Initializes this symbol
		/// </summary>
		/// <param name="sid">The symbol's unique identifier</param>
		/// <param name="name">The symbol's name</param>
		public Action(int sid, string name) : base(sid, name)
		{
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Hime.SDK.Grammars.Action"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents the current <see cref="Hime.SDK.Grammars.Action"/>.
		/// </returns>
		public override string ToString()
		{
			return "{" + Name + "}";
		}
	}
}