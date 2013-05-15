using System.Collections.Generic;

namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Fast rewindable stream of token encapsulating a lexer
    /// </summary>
    public sealed class RewindableTokenStream : Lexer.ITokenStream
    {
        private const int ringSize = 32;

        private Lexer.ILexer lexer;
        private Symbols.Token[] ring;
        private int ringStart;      // Start index of the ring in case the stream in rewinded
        private int ringNextEntry;  // Index for inserting new characters in the ring

        /// <summary>
        /// Initializes the rewindable stream with the given lexer
        /// </summary>
        /// <param name="lexer">The encapsulated lexer</param>
        public RewindableTokenStream(Lexer.ILexer lexer)
        {
            this.lexer = lexer;
            this.ring = new Symbols.Token[ringSize];
            this.ringStart = 0;
            this.ringNextEntry = 0;
        }

        private bool IsRingEmpty() { return (ringStart == ringNextEntry); }

        private Symbols.Token ReadRing()
        {
            Symbols.Token token = ring[ringStart++];
            if (ringStart == ring.Length)
                ringStart = 0;
            return token;
        }

        private void PushInRing(Symbols.Token value)
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
        public Symbols.Token GetNextToken()
        {
            if (!IsRingEmpty())
                return ReadRing();
            Symbols.Token value = lexer.GetNextToken();
            PushInRing(value);
            return value;
        }
    }
}
