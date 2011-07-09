using System.Collections.Generic;

namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Represents an exception in a lexer
    /// </summary>
    [System.Serializable]
    public class LexerException : System.Exception
    {
        /// <summary>
        /// Initializes a new instance of the LexerException class
        /// </summary>
        public LexerException() : base() { }
        /// <summary>
        /// Initializes a new instance of the LexerException class with the given message
        /// </summary>
        /// <param name="message">The message conveyed by this exception</param>
        public LexerException(string message) : base(message) { }
        /// <summary>
        /// Initializes a new instance of the LexerException class with the given message and exception
        /// </summary>
        /// <param name="message">The message conveyed by this exception</param>
        /// <param name="innerException">The inner catched exception</param>
        public LexerException(string message, System.Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// Represents an error in a lexer
    /// </summary>
    public interface LexerError
    {
        /// <summary>
        /// Gets the error's message
        /// </summary>
        string Message { get; }
    }

    /// <summary>
    /// Represents an error in the input stream of a lexer
    /// </summary>
    public abstract class LexerTextError : LexerError
    {
        /// <summary>
        /// Error's line number
        /// </summary>
        protected int line;
        /// <summary>
        /// Error's column
        /// </summary>
        protected int column;

        /// <summary>
        /// Gets the line number of the error
        /// </summary>
        public int Line { get { return line; } }
        /// <summary>
        /// Gets the column number of the error
        /// </summary>
        public int Column { get { return column; } }

        /// <summary>
        /// Gets the error's message
        /// </summary>
        public abstract string Message { get; }

        /// <summary>
        /// Initializes a new instance of the LexerTextError class at the given line and column number
        /// </summary>
        /// <param name="line">The line number of the error</param>
        /// <param name="column">The column number of the error</param>
        protected LexerTextError(int line, int column)
        {
            this.line = line;
            this.column = column;
        }

        /// <summary>
        /// Returns the string representation of this error
        /// </summary>
        /// <returns>The string representation of this error</returns>
        public abstract override string ToString();
    }

    /// <summary>
    /// Represents an unexpected character error in the input stream of a lexer
    /// </summary>
    public sealed class LexerTextErrorDiscardedChar : LexerTextError
    {
        private char discarded;
        private string message;

        /// <summary>
        /// Gets the character that caused the error
        /// </summary>
        public char Discarded { get { return discarded; } }
        /// <summary>
        /// Gets the error's message
        /// </summary>
        public override string Message { get { return message; } }

        /// <summary>
        /// Initializes a new instance of the LexerTextErrorDiscardedChar class for the given character at the given line and column number
        /// </summary>
        /// <param name="discarded">The errorneous character</param>
        /// <param name="line">The line number of the character</param>
        /// <param name="column">The column number of the character</param>
        public LexerTextErrorDiscardedChar(char discarded, int line, int column)
            : base(line, column)
        {
            this.discarded = discarded;
            System.Text.StringBuilder Builder = new System.Text.StringBuilder("Unrecognized character '");
            Builder.Append(discarded.ToString());
            Builder.Append("' (0x");
            Builder.Append(System.Convert.ToInt32(discarded).ToString("X"));
            Builder.Append(")");
            message = Builder.ToString();
        }

        /// <summary>
        /// Returns the string representation of this error
        /// </summary>
        /// <returns>The string representation of this error</returns>
        public override string ToString() { return message; }
    }





    /// <summary>
    /// Represents a lexer
    /// </summary>
    public interface ILexer
    {
        /// <summary>
        /// Gets the current line number in the input
        /// </summary>
        int CurrentLine { get; }
        /// <summary>
        /// Gets a clone of this lexer
        /// </summary>
        /// <returns>A clone of this lexer</returns>
        ILexer Clone();
        /// <summary>
        /// Gets the next token in the input
        /// </summary>
        /// <returns>The next token in the input</returns>
        SymbolToken GetNextToken();
        /// <summary>
        /// Get the next token in the input that has is of one of the provided IDs
        /// </summary>
        /// <param name="ids">The possible IDs of the next expected token</param>
        /// <returns>The next token in the input</returns>
        SymbolToken GetNextToken(ushort[] ids);
    }

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
        /// Represents a state of the DFA
        /// </summary>
        protected struct State
        {
            /// <summary>
            /// State's transitions
            /// </summary>
            public ushort[][] transitions;
            /// <summary>
            /// Terminal recognized at this state, or null if none is recognized
            /// </summary>
            public SymbolTerminal terminal;
            /// <summary>
            /// Initializes a new instance of the State structure with transitions and a terminal
            /// </summary>
            /// <param name="transitions">The transitions for the state</param>
            /// <param name="terminal">The terminal recognized at this state</param>
            public State(ushort[][] transitions, SymbolTerminal terminal)
            {
                this.transitions = transitions;
                this.terminal = terminal;
            }
        }

        /// <summary>
        /// States of the DFA
        /// </summary>
        protected State[] states;
        /// <summary>
        /// Map associating a callback to symbol ids
        /// </summary>
        protected Dictionary<ushort, MatchSubGrammar> subGrammars;
        /// <summary>
        /// ID of the separator
        /// </summary>
        protected ushort separatorID;

        // Lexer state data
        /// <summary>
        /// Lexer's errors
        /// </summary>
        protected List<LexerTextError> errors;
        /// <summary>
        /// Exposed read-only list of lexer's errors
        /// </summary>
        protected System.Collections.ObjectModel.ReadOnlyCollection<LexerTextError> readonlyErrors;
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
        /// Gets the errors encountered by the lexer
        /// </summary>
        public ICollection<LexerTextError> Errors { get { return readonlyErrors; } }
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
            errors = new List<LexerTextError>();
            readonlyErrors = new System.Collections.ObjectModel.ReadOnlyCollection<LexerTextError>(errors);
            this.input = new BufferedTextReader(input);
            currentLine = 1;
            currentColumn = 1;
            isDollatEmited = false;
            bufferSize = 100;
            buffer = new char[bufferSize];
        }
        /// <summary>
        /// Initializes a new instance of the LexerText class as a copy of the given lexer
        /// </summary>
        /// <param name="original">The lexer to copy</param>
        protected LexerText(LexerText original)
        {
            setup();
            errors = new List<LexerTextError>(original.errors);
            input = original.input.Clone();
            currentLine = original.currentLine;
            currentColumn = original.currentColumn;
            isDollatEmited = original.isDollatEmited;
            bufferSize = 100;
            buffer = new char[bufferSize];
        }

        /// <summary>
        /// Get the next token in the input that has is of one of the provided IDs
        /// </summary>
        /// <param name="ids">The possible IDs of the next expected token</param>
        /// <returns>The next token in the input</returns>
        public SymbolToken GetNextToken(ushort[] ids) { throw new LexerException("Text lexer does not support this method."); }

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
                SymbolTokenText Token = GetNextToken_DFA();
                if (Token == null)
                {
                    bool atend = false;
                    char c = input.Read(out atend);
                    errors.Add(new LexerTextErrorDiscardedChar(c, currentLine, currentColumn));
                    AdvanceStats(c.ToString());
                }
                else
                {
                    AdvanceStats(Token.ValueText);
                    if (Token.SymbolID != separatorID)
                    {
                        if (subGrammars.ContainsKey(Token.SymbolID))
                            Token.SubGrammarRoot = subGrammars[Token.SymbolID](Token.ValueText);
                        return Token;
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
            State state = states[0];

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

            if (matched == null)
            {
                input.Rewind(count);
                return null;
            }
            return new SymbolTokenText(matched.Name, matched.SymbolID, new string(buffer, 0, matchedLength), currentLine);
        }
    }

    /// <summary>
    /// Represents a lexer for a binary stream
    /// </summary>
    public abstract class LexerBinary : ILexer
    {
        /// <summary>
        /// Callback for reading a specific terminal
        /// </summary>
        /// <returns></returns>
        protected delegate SymbolToken ApplyGetNextToken();

        private static byte flag1 = 0x01;
        private static byte flag2 = 0x03;
        private static byte flag3 = 0x07;
        private static byte flag4 = 0x0F;
        private static byte flag5 = 0x1F;
        private static byte flag6 = 0x3F;
        private static byte flag7 = 0x7F;
        private static byte flag8 = 0xFF;
        /// <summary>
        /// Bit flags
        /// </summary>
        protected static byte[] flags = { 0x00, flag1, flag2, flag3, flag4, flag5, flag6, flag7, flag8 };

        protected Dictionary<ushort, ApplyGetNextToken> getNextTokens;
        protected Binary.DataInput input;
        protected int currentBitLeft;
        protected bool dollarEmitted;

        public int CurrentLine { get { return 0; } }
        public int InputLength { get { return input.Length; } }

        protected abstract void setup();
        public abstract ILexer Clone();

        protected LexerBinary(Binary.DataInput input)
        {
            setup();
            this.input = input;
            this.currentBitLeft = 8;
        }

        public SymbolToken GetNextToken() { throw new LexerException("Binary lexer cannot match a token without an expected tokens list."); }

        public SymbolToken GetNextToken(ushort[] IDs)
        {
            if (dollarEmitted)
                return SymbolTokenEpsilon.Instance;
            if (input.IsAtEnd && currentBitLeft == 8)
            {
                dollarEmitted = true;
                return SymbolTokenDollar.Instance;
            }

            foreach (ushort ID in IDs)
            {
                SymbolToken Temp = GetNextToken_Apply(ID);
                if (Temp != null) return Temp;
            }
            dollarEmitted = true;
            return SymbolTokenDollar.Instance;
        }

        protected SymbolToken GetNextToken_Apply(ushort ID)
        {
            if (!getNextTokens.ContainsKey(ID)) return null;
            return getNextTokens[ID]();
        }



        protected SymbolToken GetNextToken_Apply_NB(ushort sid, string name, byte value, int length)
        {
            if (currentBitLeft < length) return null;
            if (((input.ReadByte() >> (currentBitLeft - length)) & flags[length]) != value) return null;
            currentBitLeft -= length;
            if (currentBitLeft == 0) { currentBitLeft = 8; input.ReadAndAdvanceByte(); }
            return new SymbolTokenBits(name, sid, value);
        }
        protected SymbolToken GetNextToken_Apply_N8(ushort sid, string name, byte value)
        {
            if (currentBitLeft != 8) return null;
            if (!input.CanRead(1)) return null;
            if (input.ReadByte() == value)
            {
                input.ReadAndAdvanceByte();
                return new SymbolTokenUInt8(name, sid, value);
            }
            return null;
        }
        protected SymbolToken GetNextToken_Apply_N16(ushort sid, string name, byte value)
        {
            if (currentBitLeft != 8) return null;
            if (!input.CanRead(2)) return null;
            if (input.ReadByte() == value)
            {
                input.ReadAndAdvanceUInt16();
                return new SymbolTokenUInt16(name, sid, value);
            }
            return null;
        }
        protected SymbolToken GetNextToken_Apply_N32(ushort sid, string name, byte value)
        {
            if (currentBitLeft != 8) return null;
            if (!input.CanRead(4)) return null;
            if (input.ReadByte() == value)
            {
                input.ReadAndAdvanceUInt32();
                return new SymbolTokenUInt32(name, sid, value);
            }
            return null;
        }
        protected SymbolToken GetNextToken_Apply_N64(ushort sid, string name, byte value)
        {
            if (currentBitLeft != 8) return null;
            if (!input.CanRead(8)) return null;
            if (input.ReadByte() == value)
            {
                input.ReadAndAdvanceUInt64();
                return new SymbolTokenUInt64(name, sid, value);
            }
            return null;
        }

        protected SymbolToken GetNextToken_Apply_JB(ushort sid, string name, int length)
        {
            if (currentBitLeft < length) return null;
            SymbolToken Temp = new SymbolTokenBits(name, sid, (byte)((input.ReadByte() >> (currentBitLeft - length)) & length));
            currentBitLeft -= length;
            if (currentBitLeft == 0) { currentBitLeft = 8; input.ReadAndAdvanceByte(); }
            return Temp;
        }
        protected SymbolToken GetNextToken_Apply_J8(ushort sid, string name)
        {
            if (currentBitLeft != 8) return null;
            if (!input.CanRead(1)) return null;
            return new SymbolTokenUInt8(name, sid, input.ReadAndAdvanceByte());
        }
        protected SymbolToken GetNextToken_Apply_J16(ushort sid, string name)
        {
            if (currentBitLeft != 8) return null;
            if (!input.CanRead(2)) return null;
            return new SymbolTokenUInt16(name, sid, input.ReadAndAdvanceByte());
        }
        protected SymbolToken GetNextToken_Apply_J32(ushort sid, string name)
        {
            if (currentBitLeft != 8) return null;
            if (!input.CanRead(4)) return null;
            return new SymbolTokenUInt32(name, sid, input.ReadAndAdvanceByte());
        }
        protected SymbolToken GetNextToken_Apply_J64(ushort sid, string name)
        {
            if (currentBitLeft != 8) return null;
            if (!input.CanRead(8)) return null;
            return new SymbolTokenUInt64(name, sid, input.ReadAndAdvanceByte());
        }
    }
}
