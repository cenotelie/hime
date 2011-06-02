using System;
using System.Collections.Generic;

namespace Hime.Redist.Parsers
{
    public sealed class BufferedTokenReader
    {
        private ILexer lexer;
        private LinkedList<SymbolToken> readInput;
        private LinkedListNode<SymbolToken> currentNode;

        public BufferedTokenReader(ILexer lexer)
        {
            this.lexer = lexer;
            readInput = new LinkedList<SymbolToken>();
        }

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
        public SymbolToken Read(ushort[] ids)
        {
            if (currentNode == null || currentNode.Next == null)
            {
                SymbolToken value = lexer.GetNextToken(ids);
                currentNode = readInput.AddLast(value);
                return value;
            }
            currentNode = currentNode.Next;
            return currentNode.Value;
        }

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