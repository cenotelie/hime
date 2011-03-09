using System;
using System.Collections.Generic;
using System.Text;

namespace Hime.Redist.Parsers
{
    public sealed class BufferedTextReader
    {
        private System.IO.TextReader p_Reader;
        private LinkedList<char> p_ReadInput;
        private LinkedListNode<char> p_CurrentNode;

        public BufferedTextReader(System.IO.TextReader inner)
        {
            p_Reader = inner;
            p_ReadInput = new LinkedList<char>();
        }
        private BufferedTextReader(BufferedTextReader original)
        {
            p_Reader = original.p_Reader;
            p_ReadInput = original.p_ReadInput;
            p_CurrentNode = original.p_CurrentNode;
        }

        public bool AtEnd()
        {
            if (p_CurrentNode == null || p_CurrentNode.Next == null)
                return (p_Reader.Peek() == -1);
            return false;
        }

        public string GetReadText()
        {
            if (p_ReadInput.Count == 0)
                return string.Empty;
            StringBuilder builder = new StringBuilder();
            LinkedListNode<char> node = p_ReadInput.First;
            while (node != p_CurrentNode)
            {
                builder.Append(node.Value);
                node = node.Next;
            }
            builder.Append(p_CurrentNode.Value);
            return builder.ToString();
        }

        public char Peek(out bool AtEnd)
        {
            AtEnd = false;
            if (p_CurrentNode == null || p_CurrentNode.Next == null)
            {
                int value = p_Reader.Peek();
                if (value == -1)
                {
                    AtEnd = true;
                    return char.MinValue;
                }
                else
                    return System.Convert.ToChar(value);
            }
            return p_CurrentNode.Value;
        }

        public char Read(out bool AtEnd)
        {
            AtEnd = false;
            if (p_CurrentNode == null || p_CurrentNode.Next == null)
            {
                int value = p_Reader.Read();
                if (value == -1)
                {
                    AtEnd = true;
                    return char.MinValue;
                }
                else
                {
                    char c = System.Convert.ToChar(value);
                    p_CurrentNode = p_ReadInput.AddLast(c);
                    return c;
                }
            }
            p_CurrentNode = p_CurrentNode.Next;
            return p_CurrentNode.Value;
        }

        public void Rewind(int length)
        {
            while (length != 0)
            {
                p_CurrentNode = p_CurrentNode.Previous;
                length--;
            }
        }

        public BufferedTextReader Clone() { return new BufferedTextReader(this); }
    }
}
