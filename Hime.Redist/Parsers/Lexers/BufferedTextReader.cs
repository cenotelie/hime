/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:25
 * 
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Represents a rewindable stream of text
    /// </summary>
    public sealed class BufferedTextReader
    {
        private System.IO.TextReader reader;
        private LinkedList<char> readInput;
        private LinkedListNode<char> currentNode;

        /// <summary>
        /// Initializes a new instance of the BufferedTextReader class with the given text reader
        /// </summary>
        /// <param name="inner">The base text reader serving as input</param>
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

        /// <summary>
        /// Determines wether the buffer is at the end of the input stream
        /// </summary>
        /// <returns>True if the buffer is at the end of the input stream, false otherwise</returns>
        public bool AtEnd()
        {
            if (currentNode == null || currentNode.Next == null)
                return (reader.Peek() == -1);
            return false;
        }

        /// <summary>
        /// Gets all the text that has been read so far as a string
        /// </summary>
        /// <returns>The text that has been already read</returns>
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

        /// <summary>
        /// Gets the next character in the stream without advancing the stream
        /// </summary>
        /// <param name="atEnd">True if at the end of the stream</param>
        /// <returns>The next character in the stream</returns>
        public char Peek(out bool atEnd)
        {
            atEnd = false;
            if (currentNode == null || currentNode.Next == null)
            {
                int value = reader.Peek();
                if (value == -1)
                {
                    atEnd = true;
                    return char.MinValue;
                }
                else
                    return System.Convert.ToChar(value);
            }
            return currentNode.Value;
        }

        /// <summary>
        /// Gets the next character in the stream and advance the stream
        /// </summary>
        /// <param name="atEnd">True if at the end of the stream</param>
        /// <returns>The next character in the stream</returns>
        public char Read(out bool atEnd)
        {
            atEnd = false;
            if (currentNode == null || currentNode.Next == null)
            {
                int value = reader.Read();
                if (value == -1)
                {
                    atEnd = true;
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

        /// <summary>
        /// Rewind the read input of a given length
        /// </summary>
        /// <param name="length">The number of characters to rewind</param>
        public void Rewind(int length)
        {
            while (length != 0)
            {
                currentNode = currentNode.Previous;
                length--;
            }
        }

        /// <summary>
        /// Gets a clone of this buffered text reader
        /// </summary>
        /// <returns>A clone of this buffered text reader</returns>
        public BufferedTextReader Clone() { return new BufferedTextReader(this); }
    }
}
