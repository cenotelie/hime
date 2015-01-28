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
using Hime.Redist.Utils;

namespace Hime.Redist.Parsers
{
	/// <summary>
	/// Represents a parser
	/// </summary>
	public interface IParser
	{
		/// <summary>
		/// Gets the variable symbols used by this parser
		/// </summary>
		ROList<Symbol> SymbolVariables { get; }
		/// <summary>
		/// Gets the virtual symbols used by this parser
		/// </summary>
		ROList<Symbol> SymbolVirtuals { get; }
		/// <summary>
		/// Gets the action symbols used by this parser
		/// </summary>
		ROList<SemanticAction> SymbolActions { get; }

		/// <summary>
		/// Gets or sets whether the paser should try to recover from errors
		/// </summary>
		bool ModeRecoverErrors { get; set; }
		/// <summary>
		/// Gets or sets a value indicating whether this parser is in debug mode
		/// </summary>
		bool ModeDebug { get; set; }

		/// <summary>
		/// Parses the input and returns the result
		/// </summary>
		/// <returns>A ParseResult object containing the data about the result</returns>
		ParseResult Parse();
	}
}
