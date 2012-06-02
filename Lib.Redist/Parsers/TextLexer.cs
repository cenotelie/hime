/*
 * Author: Laurent Wouters
 * Date: 02/06/2012
 * Time: 10:15
 * 
 */
using System.Collections.Generic;

namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Represents a lexer for a text stream
    /// </summary>
    public abstract class TextLexer : ILexer
    {
        /// <summary>
        /// Data structure for a text lexer automaton
        /// </summary>
        protected class Automaton
        {
            private Utils.BlobInt table;
            private Utils.BlobUShort states;
            public Automaton(System.IO.Stream stream)
            {
                System.IO.BinaryReader reader = new System.IO.BinaryReader(stream);
                int count = reader.ReadInt32();
                byte[] bt = new byte[count * 4];
                reader.Read(bt, 0, bt.Length);
                byte[] bd = new byte[stream.Length - bt.Length - 4];
                reader.Read(bd, 0, bd.Length);
                reader.Close();
                table = new Utils.BlobInt(bt);
                states = new Utils.BlobUShort(bd);
            }
            public ushort GetTerminal(ushort state) { return states.data[table.data[state]]; }
            public ushort GetTransitionsCount(ushort state) { return states.data[table.data[state] + 1]; }
            public ushort GetTranstion(ushort state, int index, out ushort begin, out ushort end)
            {
                int offset = table.data[state] + 2 + index * 3;
                begin = states.data[offset];
                end = states.data[offset + 1];
                return states.data[offset + 2];
            }
        }

        /// <summary>
        /// Lexer's automaton
        /// </summary>
        protected Automaton lexAutomaton;
        /// <summary>
        /// The terminals that can be recognized by this lexer
        /// </summary>
        protected SymbolTerminal[] lexTerminals;
        /// <summary>
        /// ID of the separator
        /// </summary>
        protected ushort lexSeparator;

        /// <summary>
        /// Error handler
        /// </summary>
        private OnErrorHandler errorHandler;
        /// <summary>
        /// Lexer's input
        /// </summary>
        private BufferedTextReader input;
        /// <summary>
        /// Current line number
        /// </summary>
        private int currentLine;
        /// <summary>
        /// Current column number
        /// </summary>
        private int currentColumn;
        /// <summary>
        /// True if the end of the input has been reached and the dollar token has been emited
        /// </summary>
        private bool isDollatEmited;
        /// <summary>
        /// Buffer for currently read text
        /// </summary>
        private char[] buffer;
        /// <summary>
        /// Size of the buffer
        /// </summary>
        private int bufferSize;

        /// <summary>
        /// Gets the text of the input that has already been read
        /// </summary>
        public string InputText { get { return input.GetReadText(); } }
        /// <summary>
        /// Gets the current line number
        /// </summary>
        public int CurrentLine { get { return currentLine; } }
        /// <summary>
        /// Gets the current column number
        /// </summary>
        public int CurrentColumn { get { return currentColumn; } }
        /// <summary>
        /// True if the lexer is at the end of the input
        /// </summary>
        public bool IsAtEnd { get { return input.AtEnd(); } }

        /// <summary>
        /// Gets a clone of this lexer
        /// </summary>
        /// <returns>A clone of this lexer</returns>
        public abstract ILexer Clone();

        protected static Automaton FindAutomaton(System.Type type, string resource)
        {
            System.Reflection.Assembly assembly = type.Assembly;
            System.IO.Stream stream = assembly.GetManifestResourceStream(resource);
            if (stream != null)
                return new Automaton(stream);
            string[] resources = assembly.GetManifestResourceNames();
            foreach (string existing in resources)
                if (existing.EndsWith(resource))
                    return new Automaton(assembly.GetManifestResourceStream(existing));
            return null;
        }

        /// <summary>
        /// Initializes a new instance of the LexerText class with the given input
        /// </summary>
        /// <param name="input">The input as a text reader</param>
        protected TextLexer(Automaton automaton, SymbolTerminal[] terminals, ushort separator, System.IO.TextReader input)
        {
            this.lexAutomaton = automaton;
            this.lexTerminals = terminals;
            this.lexSeparator = separator;
            this.input = new BufferedTextReader(input);
            this.currentLine = 1;
            this.currentColumn = 1;
            this.isDollatEmited = false;
            this.bufferSize = 100;
            this.buffer = new char[this.bufferSize];
        }
        /// <summary>
        /// Initializes a new instance of the LexerText class as a copy of the given lexer
        /// </summary>
        /// <param name="original">The lexer to copy</param>
        protected TextLexer(TextLexer original)
        {
            this.lexAutomaton = original.lexAutomaton;
            this.lexTerminals = original.lexTerminals;
            this.lexSeparator = original.lexSeparator;
            this.input = original.input.Clone();
            this.currentLine = original.currentLine;
            this.currentColumn = original.currentColumn;
            this.isDollatEmited = original.isDollatEmited;
            this.bufferSize = 100;
            this.buffer = new char[this.bufferSize];
        }

        /// <summary>
        /// Sets the error handler for this lexer
        /// </summary>
        /// <param name="handler"></param>
        public void SetErrorHandler(OnErrorHandler handler) { this.errorHandler = handler; }

        /// <summary>
        /// Gets the next token in the input
        /// </summary>
        /// <returns>The next token in the input</returns>
        public SymbolToken GetNextToken()
        {
            if (input.AtEnd())
            {
                if (isDollatEmited)
                    return SymbolTokenEpsilon.Instance;
                isDollatEmited = true;
                return SymbolTokenDollar.Instance;
            }

            while (true)
            {
                if (input.AtEnd())
                {
                    isDollatEmited = true;
                    return SymbolTokenDollar.Instance;
                }
                SymbolTokenText token = GetNextToken_DFA();
                if (token == null)
                {
                    bool atend = false;
                    char c = input.Read(out atend);
                    errorHandler(new UnexpectedCharError(c, currentLine, currentColumn));
                    AdvanceStats(c.ToString());
                }
                else
                {
                    AdvanceStats(token.ValueText);
                    if (token.SymbolID != lexSeparator)
                        return token;
                }
            }
        }

        private void AdvanceStats(string text)
        {
            foreach (char c in text)
            {
                if (c == '\n')
                {
                    currentLine++;
                    currentColumn = 1;
                }
                else
                    currentColumn++;
            }
        }

        private SymbolTokenText GetNextToken_DFA()
        {
            SymbolTerminal matched = null;
            int matchedLength = 0;
            int count = 0;
            ushort state = 0;

            while (true)
            {
                ushort terminal = lexAutomaton.GetTerminal(state);
                if (terminal != 0xFFFF)
                {
                    matched = lexTerminals[terminal];
                    matchedLength = count;
                }
                bool atend = false;
                char c = input.Peek(out atend);
                if (atend)
                    break;
                ushort UCV = System.Convert.ToUInt16(c);
                ushort nextState = 0xFFFF;
                ushort nb = lexAutomaton.GetTransitionsCount(state);
                for (int i = 0; i != nb; i++)
                {
                    ushort begin = 0, end = 0, next = 0;
                    next = lexAutomaton.GetTranstion(state, i, out begin, out end);
                    if (UCV >= begin && UCV <= end)
                    {
                        nextState = next;
                        break;
                    }
                }
                if (nextState == 0xFFFF)
                    break;
                state = nextState;
                c = input.Read(out atend);
                if (count >= bufferSize)
                {
                    // buffer is too small, create larger buffer
                    bufferSize *= 2;
                    char[] temp = new char[bufferSize];
                    System.Array.Copy(buffer, temp, count);
                    buffer = temp;
                }
                buffer[count] = c;
                count++;
            }

            input.Rewind(count - matchedLength);
            if (matched == null)
                return null;
            return new SymbolTokenText(matched.SymbolID, matched.Name, new string(buffer, 0, matchedLength), currentLine, currentColumn);
        }
    }
}
