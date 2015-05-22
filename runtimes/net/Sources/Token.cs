/**********************************************************************
* Copyright (c) 2014 Laurent Wouters and others
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

namespace Hime.Redist
{
	/// <summary>
	/// Represents a token as an output element of a lexer
	/// </summary>
	public struct Token : SemanticElement
	{
		/// <summary>
		/// The repository containing this token
		/// </summary>
		private readonly TokenRepository repository;
		/// <summary>
		/// The index of this token in the text
		/// </summary>
		private readonly int index;

		/// <summary>
		/// Gets the position in the input text of this token
		/// </summary>
		public TextPosition Position { get { return repository.GetPosition(index); } }

		/// <summary>
		/// Gets the span in the input text of this token
		/// </summary>
		public TextSpan Span { get { return repository.GetSpan(index); } }

		/// <summary>
		/// Gets the context of this token in the input
		/// </summary>
		public TextContext Context { get { return repository.GetContext(index); } }

		/// <summary>
		/// Gets the terminal associated to this token
		/// </summary>
		public Symbol Symbol { get { return repository.GetSymbol(index); } }

		/// <summary>
		/// Gets the value of this token
		/// </summary>
		public string Value { get { return repository.GetValue(index); } }

		/// <summary>
		/// Initializes this token
		/// </summary>
		/// <param name="repository">The repository containing the token</param>
		/// <param name="index">The token's index</param>
		internal Token(TokenRepository repository, int index)
		{
			this.repository = repository;
			this.index = index;
		}

		/// <summary>
		/// Gets a string representation of this token
		/// </summary>
		/// <returns>The string representation of the token</returns>
		public override string ToString()
		{
			string name = repository.GetSymbol(index).Name;
			string value = repository.GetValue(index);
			return name + " = " + value;
		}
	}
}
