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
        private SymbolTerminal[] lexTerminals;
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
            this.lexTerminals = terminals;
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
                bool endOfInput = false;
                char current = input.Read(out endOfInput);
                if (endOfInput)
                    break;
                if (count == bufferSize)
                {
                    // buffer is too small, create larger buffer
                    bufferSize *= 2;
                    char[] temp = new char[bufferSize];
                    System.Array.Copy(buffer, temp, count);
                    buffer = temp;
                }
                buffer[count] = current;
                count++;
                state = lexAutomaton.GetTransition(state, current);
                if (state == 0xFFFF)
                    break;
            }
            input.Rewind(count - matchedLength);
            if (matched == null)
                return null;
            return new SymbolTokenText(matched.SymbolID, matched.Name, new string(buffer, 0, matchedLength), currentLine, currentColumn);
        }
    }
}
