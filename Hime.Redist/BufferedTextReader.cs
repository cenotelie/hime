using System;
using System.Collections.Generic;
using System.Text;

namespace Hime.Redist.Parsers
{
    public sealed class BufferedTextReader
    {
        private System.IO.TextReader reader;
        private LinkedList<char> readInput;
        private LinkedListNode<char> currentNode;

        public BufferedTextReader(System.IO.TextReader inner)
        {
            reader = inner;
            readInput = new LinkedList<char>();
        }
        private BufferedTextReader(BufferedTextReader original)
        {
            reader = original.reader;
            readInput = original.readInput;
            currentNode = original.currentNode;
        }

        public bool AtEnd()
        {
            if (currentNode == null || currentNode.Next == null)
                return (reader.Peek() == -1);
            return false;
        }

        public string GetReadText()
        {
            if (readInput.Count == 0)
                return string.Empty;
            StringBuilder builder = new StringBuilder();
            LinkedListNode<char> node = readInput.First;
            while (node != currentNode)
            {
                builder.Append(node.Value);
                node = node.Next;
            }
            builder.Append(currentNode.Value);
            return builder.ToString();
        }

        public char Peek(out bool AtEnd)
        {
            AtEnd = false;
            if (currentNode == null || currentNode.Next == null)
            {
                int value = reader.Peek();
                if (value == -1)
                {
                    AtEnd = true;
                    return char.MinValue;
                }
                else
                    return System.Convert.ToChar(value);
            }
            return currentNode.Value;
        }

        public char Read(out bool AtEnd)
        {
            AtEnd = false;
            if (currentNode == null || currentNode.Next == null)
            {
                int value = reader.Read();
                if (value == -1)
                {
                    AtEnd = true;
                    return char.MinValue;
                }
                else
                {
                    char c = System.Convert.ToChar(value);
                    currentNode = readInput.AddLast(c);
                    return c;
                }
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
        }

        public BufferedTextReader Clone() { return new BufferedTextReader(this); }
    }
}
