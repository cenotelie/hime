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
using System.Collections.Generic;

namespace Hime.Redist
{
	/// <summary>
	/// Represents the output of a lexer as a tokenized text
	/// </summary>
	public interface TokenizedText : Text, IEnumerable<Token>
	{
		/// <summary>
		/// Gets the number of tokens in this text
		/// </summary>
		int TokenCount { get; }

		/// <summary>
		/// Gets the token at the given index
		/// </summary>
		/// <param name="index">An index</param>
		/// <returns>The token</returns>
		Token this[int index] { get; }

		/// <summary>
		/// Gets the value of the token at the given index
		/// </summary>
		/// <param name="index">An index</param>
		/// <returns>The corresponding symbol</returns>
		Symbol GetSymbolAt(int index);

		/// <summary>
		/// Gets the value of the given token
		/// </summary>
		/// <param name="token">A token in this text</param>
		/// <returns>The token's value as a string</returns>
		Symbol GetSymbol(Token token);

		/// <summary>
		/// Gets the string value of the given token
		/// </summary>
		/// <param name="token">A token</param>
		/// <returns>The string value of the given token</returns>
		string GetValue(Token token);

		/// <summary>
		/// Gets the line number of the given token
		/// </summary>
		/// <param name="token">A token</param>
		/// <returns>The line number of the given token</returns>
		int GetLineOf(Token token);

		/// <summary>
		/// Gets the column number of the given token
		/// </summary>
		/// <param name="token">A token</param>
		/// <returns>The column number of the given token</returns>
		int GetColumnOf(Token token);

		/// <summary>
		/// Gets the position of the given token
		/// </summary>
		/// <param name="token">A token</param>
		/// <returns>The position (line and column) of the given token</returns>
		TextPosition GetPositionOf(Token token);
	}
}
