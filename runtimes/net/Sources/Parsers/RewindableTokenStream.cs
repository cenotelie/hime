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

namespace Hime.Redist.Parsers
{
	/// <summary>
	/// Fast rewindable stream of token encapsulating a lexer
	/// </summary>
	class RewindableTokenStream
	{
		/// <summary>
		/// The size of the ring buffer
		/// </summary>
		private const int RING_SIZE = 32;

		/// <summary>
		/// The input lexer to read token from
		/// </summary>
		private Lexer.ILexer lexer;

		/// <summary>
		/// The ring buffer
		/// </summary>
		private Token[] ring;

		/// <summary>
		/// Start index of the ring in case the stream in rewinded
		/// </summary>
		private int ringStart;

		/// <summary>
		/// TIndex for inserting new characters in the ring
		/// </summary>
		private int ringNextEntry;

		/// <summary>
		/// Initializes the rewindable stream with the given lexer
		/// </summary>
		/// <param name="lexer">The encapsulated lexer</param>
		public RewindableTokenStream(Lexer.ILexer lexer)
		{
			this.lexer = lexer;
			this.ring = new Token[RING_SIZE];
			this.ringStart = 0;
			this.ringNextEntry = 0;
		}

		/// <summary>
		/// Determines whether the ring buffer is empty
		/// </summary>
		/// <returns><c>true</c> if the ring is empty; otherwise, <c>false</c>.</returns>
		private bool IsRingEmpty()
		{
			return (ringStart == ringNextEntry);
		}

		/// <summary>
		/// Reads a token from the ring
		/// </summary>
		/// <returns>The next token in the ring buffer</returns>
		private Token ReadRing()
		{
			Token token = ring[ringStart++];
			if (ringStart == ring.Length)
				ringStart = 0;
			return token;
		}

		/// <summary>
		/// Pushs the given token onto the ring
		/// </summary>
		/// <param name="value">The token to push</param>
		private void PushInRing(Token value)
		{
			ring[ringNextEntry++] = value;
			if (ringNextEntry == RING_SIZE)
				ringNextEntry = 0;
			ringStart = ringNextEntry;
		}

		/// <summary>
		/// Goes back in the stream
		/// </summary>
		/// <param name="count">Number of tokens to rewind</param>
		public void Rewind(int count)
		{
			ringStart -= count;
			if (ringStart < 0)
				ringStart += RING_SIZE;
		}

		/// <summary>
		/// Gets the next token in the stream
		/// </summary>
		/// <returns>The next token</returns>
		public Token GetNextToken()
		{
			if (!IsRingEmpty())
				return ReadRing();
			Token value = lexer.GetNextToken();
			PushInRing(value);
			return value;
		}
	}
}
