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
        private const int ringSize = 32;

        private Lexer.Lexer lexer;
        private Token[] ring;
        private int ringStart;      // Start index of the ring in case the stream in rewinded
        private int ringNextEntry;  // Index for inserting new characters in the ring

        /// <summary>
        /// Initializes the rewindable stream with the given lexer
        /// </summary>
        /// <param name="lexer">The encapsulated lexer</param>
        public RewindableTokenStream(Lexer.Lexer lexer)
        {
            this.lexer = lexer;
            this.ring = new Token[ringSize];
            this.ringStart = 0;
            this.ringNextEntry = 0;
        }

        private bool IsRingEmpty() { return (ringStart == ringNextEntry); }

        private Token ReadRing()
        {
            Token token = ring[ringStart++];
            if (ringStart == ring.Length)
                ringStart = 0;
            return token;
        }

        private void PushInRing(Token value)
        {
            ring[ringNextEntry++] = value;
            if (ringNextEntry == ringSize)
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
                ringStart += ringSize;
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
