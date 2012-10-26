/*
 * Author: Laurent Wouters
 * Date: 26/10/2012
 * Time: 17:25
 * 
 */
using System.Collections.Generic;

namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Fast rewindable stream of token encapsulating a lexer
    /// </summary>
    public sealed class RewindableTokenStream
    {
        private const int ringSize = 1024;

        private ILexer lexer;
        private SymbolToken[] ring;
        private int ringStart;      // Start index of the ring in case the stream in rewinded
        private int ringNextEntry;  // Index for inserting new characters in the ring

        /// <summary>
        /// Initializes the rewindable stream with the given lexer
        /// </summary>
        /// <param name="lexer">The encapsulated lexer</param>
        public RewindableTokenStream(ILexer lexer)
        {
            this.lexer = lexer;
            this.ring = new SymbolToken[ringSize];
            this.ringStart = 0;
            this.ringNextEntry = 0;
        }

        private bool IsRingEmpty() { return (ringStart == ringNextEntry); }

        private SymbolToken ReadRing()
        {
            SymbolToken token = ring[ringStart++];
            if (ringStart == ring.Length)
                ringStart = 0;
            return token;
        }

        private void PushInRing(SymbolToken value)
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
        public SymbolToken GetNextToken()
        {
            if (!IsRingEmpty())
                return ReadRing();
            SymbolToken value = lexer.GetNextToken();
            PushInRing(value);
            return value;
        }
    }
}
