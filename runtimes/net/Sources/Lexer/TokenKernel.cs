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

namespace Hime.Redist.Lexer
{
	/// <summary>
	/// Represents the kernel of a token, i.e. the identifying information of a token
	/// </summary>
	struct TokenKernel
	{
		/// <summary>
		/// The identifier of the matched terminal
		/// </summary>
		private readonly int terminalID;
		/// <summary>
		/// The token's index in its repository
		/// </summary>
		private readonly int index;

		/// <summary>
		/// Gets the identifier of the matched terminal
		/// </summary>
		public int TerminalID { get { return terminalID; } }

		/// <summary>
		/// Gets the token's index in its repository
		/// </summary>
		public int Index { get { return index; } }

		/// <summary>
		/// Initializes this kernel
		/// </summary>
		/// <param name="id">The identifier of the matched terminal</param>
		/// <param name="index">The token's index in its repository</param>
		public TokenKernel(int id, int index)
		{
			terminalID = id;
			this.index = index;
		}
	}
}
