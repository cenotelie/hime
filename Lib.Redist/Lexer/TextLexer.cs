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
        /// Lexer's automaton
        /// </summary>
        private TextLexerAutomaton lexAutomaton;
        /// <summary>
        /// The terminals that can be recognized by this lexer
        /// </summary>
        private Utils.SymbolDictionary<SymbolTerminal> lexTerminals;
        /// <summary>
        /// ID of the separator
        /// </summary>
        private ushort lexSeparator;

        /// <summary>
        /// Lexer's input
        /// </summary>
        private RewindableTextReader input;
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
        /// Gets the terminals matched by this lexer
        /// </summary>
        public Utils.SymbolDictionary<SymbolTerminal> Terminals { get { return lexTerminals; } }
        /// <summary>
        /// Gets the current line number
        /// </summary>
        public int CurrentLine { get { return currentLine; } }
        /// <summary>
        /// Gets the current column number
        /// </summary>
        public int CurrentColumn { get { return currentColumn; } }

        /// <summary>
        /// Events for lexical errors
        /// </summary>
        public event AddLexicalError OnError;

        /// <summary>
        /// Initializes a new instance of the TextLexer class with the given input
        /// </summary>
        /// <param name="automaton">DFA automaton for this lexer</param>
        /// <param name="terminals">Terminals recognized by this lexer</param>
        /// <param name="separator">SID of the separator token</param>
        /// <param name="input">Input to this lexer</param>
        protected TextLexer(TextLexerAutomaton automaton, SymbolTerminal[] terminals, ushort separator, System.IO.TextReader input)
        {
            this.lexAutomaton = automaton;
            this.lexTerminals = new Utils.SymbolDictionary<SymbolTerminal>(terminals);
            this.lexSeparator = separator;
            this.input = new RewindableTextReader(input);
            this.currentLine = 1;
            this.currentColumn = 1;
            this.isDollatEmited = false;
            this.bufferSize = 100;
            this.buffer = new char[this.bufferSize];
        }

        /// <summary>
        /// Gets the next token in the input
        /// </summary>
        /// <returns>The next token in the input</returns>
        public SymbolToken GetNextToken()
        {
            if (isDollatEmited)
                return SymbolTokenEpsilon.Instance;
            while (true)
            {
                SymbolTokenText token = GetNextToken_DFA();
                if (token == null)
                {
                    bool atend = false;
                    char c = input.Read(out atend);
                    if (atend)
                    {
                        isDollatEmited = true;
                        return SymbolTokenDollar.Instance;
                    }
                    else
                    {
                        OnError(new UnexpectedCharError(c, currentLine, currentColumn));
                        AdvanceStats(c.ToString());
                    }
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
            int matchedIndex = 0;
            int matchedLength = 0;
            int readCount = 0;
            ushort state = 0;

            while (state != 0xFFFF)
            {
                int offset = lexAutomaton.GetOffset(state);
                ushort terminal = lexAutomaton.GetTerminal(offset);
                if (terminal != 0xFFFF)
                {
                    matchedIndex = terminal;
                    matchedLength = readCount;
                }
                if (lexAutomaton.HasNoTransition(offset))
                    break;
                bool endOfInput = false;
                char current = input.Read(out endOfInput);
                if (endOfInput)
                    break;
                if (readCount == bufferSize)
                {
                    // buffer is too small, create larger buffer
                    bufferSize *= 2;
                    char[] temp = new char[bufferSize];
                    System.Array.Copy(buffer, temp, readCount);
                    buffer = temp;
                }
                buffer[readCount] = current;
                readCount++;
                if (current <= 255)
                    state = lexAutomaton.GetCachedTransition(offset + current + 2);
                else
                    state = lexAutomaton.GetFallbackTransition(offset, current);
            }
            input.Rewind(readCount - matchedLength);
            if (matchedLength == 0)
                return null;
            SymbolTerminal matched = lexTerminals[matchedIndex];
            return new SymbolTokenText(matched.SymbolID, matched.Name, new string(buffer, 0, matchedLength), currentLine, currentColumn);
        }
    }
}
