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
	/// Represents a phrase that can be produced by grammar.
	/// It is essentially a list of terminals
	/// </summary>
	public sealed class Phrase : IEnumerable<Terminal>
	{
		/// <summary>
		/// The content
		/// </summary>
		private readonly List<Terminal> content;

		/// <summary>
		/// Gets the length of this phrase
		/// </summary>
		public int Length { get { return content.Count; } }
		/// <summary>
		/// Gets the terminal at the given index in this phrase
		/// </summary>
		/// <param name="index">An index in this phrase</param>
		public Terminal this[int index] { get { return content[index]; } }

		/// <summary>
		/// Initializes an empty phrase
		/// </summary>
		public Phrase()
		{
			content = new List<Terminal>();
		}

		/// <summary>
		/// Initializes a phrase a a copy of another
		/// </summary>
		/// <param name="copied">The phrase to copy</param>
		public Phrase(Phrase copied)
		{
			content = new List<Terminal>(copied.content);
		}

		/// <summary>
		/// Gets the enumerator of terminals in this phrase
		/// </summary>
		/// <returns>The enumerator</returns>
		public IEnumerator<Terminal> GetEnumerator()
		{
			return content.GetEnumerator();
		}
		/// <summary>
		/// Gets the enumerator of terminals in this phrase
		/// </summary>
		/// <returns>The enumerator</returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return content.GetEnumerator();
		}

		/// <summary>
		/// Appends the specified terminal to this phrase
		/// </summary>
		/// <param name="terminal">The terminal to append</param>
		public void Append(Terminal terminal)
		{
			content.Add(terminal);
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Hime.SDK.Grammars.Phrase"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents the current <see cref="Hime.SDK.Grammars.Phrase"/>.
		/// </returns>
		public override string ToString()
		{
			System.Text.StringBuilder builder = new System.Text.StringBuilder("\u3016");
			bool first = true;
			foreach (Terminal terminal in content)
			{
				if (!first)
					builder.Append(" ");
				first = false;
				builder.Append(terminal.ToString());
			}
			builder.Append("\u3017");
			return builder.ToString();
		}
	}
}