/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:25
 * 
 */
using System;
using System.Collections.Generic;

namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Represents a rewindable stream of tokens
    /// </summary>
    public sealed class RewindableTokenReader
    {
        private ILexer lexer;
        private LinkedList<SymbolToken> readInput;
        private LinkedListNode<SymbolToken> currentNode;

        /// <summary>
        /// Initializes a new instance of the BufferedTokenReader class with the given lexer
        /// </summary>
        /// <param name="lexer">The lexer matching tokens</param>
        public RewindableTokenReader(ILexer lexer)
        {
            this.lexer = lexer;
            readInput = new LinkedList<SymbolToken>();
        }

        /// <summary>
        /// Gets the next token in the stream
        /// </summary>
        /// <returns>The next token in the stream</returns>
        public SymbolToken Read()
        {
            if (currentNode == null || currentNode.Next == null)
            {
                SymbolToken value = lexer.GetNextToken();
                currentNode = readInput.AddLast(value);
                return value;
            }
            currentNode = currentNode.Next;
            return currentNode.Value;
        }

        /// <summary>
        /// Rewinds the stream of a given number of tokens
        /// </summary>
        /// <param name="length">The number of tokens to rewind</param>
        public void Rewind(int length)
        {
            while (length != 0)
            {
                currentNode = currentNode.Previous;
                length--;
            }
            // Drop the beginning of the list
            while (readInput.First != currentNode)
                readInput.RemoveFirst();
        }
    }
}