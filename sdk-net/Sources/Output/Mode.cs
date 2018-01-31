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

namespace Hime.SDK.Output
{
	/// <summary>
	/// Represents a grammar's compilation mode
	/// </summary>
	public enum Mode
	{
		/// <summary>
		/// Generates the source code for the lexer and parser
		/// </summary>
		Source,
		/// <summary>
		/// Generates the compiled assembly of the lexer and parser
		/// </summary>
		Assembly,
		/// <summary>
		/// Generates the source code for the lexer and parser and the compiled assembly
		/// </summary>
		SourceAndAssembly,
		/// <summary>
		/// Generates the source code for the lexer and parser, as well as the debug data
		/// </summary>
		Debug
	}
}
