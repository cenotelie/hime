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

namespace Hime.Redist
{
	/// <summary>
	/// Specifies the type of error
	/// </summary>
	public enum ErrorType
	{
		/// <summary>
		/// Lexical error occuring when an unexpected character is encountered in the input preventing to match tokens
		/// </summary>
		UnexpectedChar,
		/// <summary>
		/// Syntactic error occuring when an unexpected token is encountered by the parser
		/// </summary>
		UnexpectedToken
	}
}
