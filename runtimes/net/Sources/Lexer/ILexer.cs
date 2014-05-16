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

namespace Hime.Redist.Lexer
{
	/// <summary>
	/// Handler for lexical errors
	/// </summary>
	/// <param name="error">The new error</param>
	public delegate void AddLexicalError(ParseError error);


	/// <summary>
	/// Represents a lexer for a text stream
	/// </summary>
	public interface ILexer
	{
		/// <summary>
		/// Gets the terminals matched by this lexer
		/// </summary>
		IList<Symbol> Terminals { get; }

		/// <summary>
		/// Gets the lexer's output as a tokenized text
		/// </summary>
		TokenizedText Output { get; }

		/// <summary>
		/// Events for lexical errors
		/// </summary>
		event AddLexicalError OnError;

		/// <summary>
		/// Gets the next token in the input
		/// </summary>
		/// <returns>The next token in the input</returns>
		Token GetNextToken();
	}
}