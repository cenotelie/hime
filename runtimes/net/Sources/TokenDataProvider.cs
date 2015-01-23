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
	interface TokenDataProvider
	{
		/// <summary>
		/// Gets the position in the input text of the given token
		/// </summary>
		/// <param name="token">A token's index</param>
		/// <returns>The position in the text</returns>
		TextPosition GetPosition(int token);

		/// <summary>
		/// Gets the span in the input text of the given token
		/// </summary>
		/// <param name="token">A token's index</param>
		/// <returns>The span in the text</returns>
		TextSpan GetSpan(int token);

		/// <summary>
		/// Gets the context in the input of the given token
		/// </summary>
		/// <param name="node">A token's index</param>
		/// <returns>The context</returns>
		TextContext GetContext(int token);

		/// <summary>
		/// Gets the grammar symbol associated to the given token
		/// </summary>
		/// <param name="token">A token's index</param>
		/// <returns>The associated symbol</returns>
		Symbol GetSymbol(int token);

		/// <summary>
		/// Gets the value of the given token
		/// </summary>
		/// <param name="token">A token's index</param>
		/// <returns>The associated value</returns>
		string GetValue(int token);
	}
}
