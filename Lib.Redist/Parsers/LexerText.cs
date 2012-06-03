/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:25
 * 
 */
using System.Collections.Generic;

namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Represents a lexer for a text stream
    /// </summary>
    public abstract class LexerText : ILexer
    {
        /// <summary>
        /// Represents a callback for matching the content of a token to a grammar
        /// </summary>
        /// <param name="value">The text value of a token</param>
        /// <returns>The abstract syntax tree node representing the result of parsing the value of the token</returns>
        protected delegate SyntaxTreeNode MatchSubGrammar(string value);

        /// <summary>
        /// States of the DFA
        /// </summary>
        protected LexerDFAState[] states;
        /// <summary>
        /// Map associating a callback to symbol ids
        /// </summary>
        protected Dictionary<ushort, MatchSubGrammar> subGrammars;
        /// <summary>
        /// ID of the separator
        /// </summary>
        protected ushort separatorID;

        private AddLexicalError errorHandler;
        /// <summary>
        /// Lexer's input
        /// </summary>
        protected BufferedTextReader input;
        /// <summary>
        /// Current line number
        /// </summary>
        protected int currentLine;
        /// <summary>
        /// Current column number
        /// </summary>
        protected int currentColumn;
        /// <summary>
        /// True if the end of the input has been reached and the dollar token has been emited
        /// </summary>
        protected bool isDollatEmited;
        /// <summary>
        /// Buffer for currently read text
        /// </summary>
        protected char[] buffer;
        /// <summary>
        /// Size of the buffer
        /// </summary>
        protected int bufferSize;

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
        /// Sets up the inner data of the lexer
        /// </summary>
        protected abstract void setup();
        /// <summary>
        /// Gets a clone of this lexer
        /// </summary>
        /// <returns>A clone of this lexer</returns>
        public abstract ILexer Clone();

        /// <summary>
        /// Initializes a new instance of the LexerText class with the given input
        /// </summary>
        /// <param name="input">The input as a text reader</param>
        protected LexerText(System.IO.TextReader input)
        {
            setup();
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
        protected LexerText(LexerText original)
        {
            setup();
            this.input = original.input.Clone();
            this.currentLine = original.currentLine;
            this.currentColumn = original.currentColumn;
            this.isDollatEmited = original.isDollatEmited;
            this.bufferSize = 100;
            this.buffer = new char[this.bufferSize];
        }

        /// <summary>
        /// Events for lexical errors
        /// </summary>
        public event AddLexicalError OnError;
        /// <summary>
        /// Sets the error handler for this lexer
        /// </summary>
        /// <param name="handler"></param>
        public void SetErrorHandler(AddLexicalError handler) { this.errorHandler = handler; }

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
                    if (token.SymbolID != separatorID)
                    {
                        if (subGrammars.ContainsKey(token.SymbolID))
                            token.SubGrammarRoot = subGrammars[token.SymbolID](token.ValueText);
                        return token;
                    }
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
            LexerDFAState state = states[0];

            while (true)
            {
                if (state.terminal != null)
                {
                    matched = state.terminal;
                    matchedLength = count;
                }
                bool atend = false;
                char c = input.Peek(out atend);
                if (atend)
                    break;
                ushort UCV = System.Convert.ToUInt16(c);
                ushort nextState = 0xFFFF;
                for (int i = 0; i != state.transitions.Length; i++)
                {
                    ushort[] transition = state.transitions[i];
                    if (UCV >= transition[0] && UCV <= transition[1])
                        nextState = transition[2];
                }
                if (nextState == 0xFFFF)
                    break;
                state = states[nextState];
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