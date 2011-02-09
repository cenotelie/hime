namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Exception thrown by lexers
    /// </summary>
    [System.Serializable]
    public class LexerException : System.Exception
    {
        /// <summary>
        /// Constructs empty exception
        /// </summary>
        public LexerException() : base() { }
        /// <summary>
        /// Constructs exception from the message
        /// </summary>
        /// <param name="message">Exception message</param>
        public LexerException(string message) : base(message) { }
        /// <summary>
        /// Constructs exception from the message and the given inner exception
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public LexerException(string message, System.Exception innerException) : base(message, innerException) { }
        protected LexerException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    /// <summary>
    /// Interface for lexer errors
    /// </summary>
    public interface LexerError
    {
        string Message { get; }
    }
    /// <summary>
    /// Text lexer error
    /// </summary>
    public abstract class LexerTextError : LexerError
    {
        /// <summary>
        /// Line of the error
        /// </summary>
        protected int p_Line;
        /// <summary>
        /// Column of the error
        /// </summary>
        protected int p_Column;

        /// <summary>
        /// Get the line of the error
        /// </summary>
        /// <value>The line of the error</value>
        public int Line { get { return p_Line; } }
        /// <summary>
        /// Get the column of the error
        /// </summary>
        /// <value>The column of the error</value>
        public int Column { get { return p_Column; } }

        public abstract string Message { get; }

        /// <summary>
        /// Constructs the lexer error
        /// </summary>
        /// <param name="line">Error line</param>
        /// <param name="column">Error column</param>
        protected LexerTextError(int line, int column)
        {
            p_Line = line;
            p_Column = column;
        }

        /// <summary>
        /// Get a string representation of the error
        /// </summary>
        /// <returns>Returns a string representation of the error</returns>
        public abstract override string ToString();
    }

    public class LexerTextErrorDiscardedChar : LexerTextError
    {
        protected char p_Discarded;
        protected string p_Message;

        public char Discarded { get { return p_Discarded; } }
        public override string Message { get { return p_Message; } }

        public LexerTextErrorDiscardedChar(char Discarded, int Line, int Column)
            : base(Line, Column)
        {
            p_Discarded = Discarded;
            System.Text.StringBuilder Builder = new System.Text.StringBuilder("Unrecognized character '");
            Builder.Append(p_Discarded.ToString());
            Builder.Append("' (0x");
            Builder.Append(System.Convert.ToInt32(p_Discarded).ToString("X"));
            Builder.Append(")");
            p_Message = Builder.ToString();
        }
        public override string ToString() { return p_Message; }
    }






    public interface ILexer
    {
        int CurrentLine { get; }
        ILexer Clone();
        SymbolToken GetNextToken();
        SymbolToken GetNextToken(ushort[] ids);
    }

    public abstract class LexerText : ILexer
    {
        protected delegate SyntaxTreeNode MatchSubGrammar(string TokenValue);

        // Lexer DFA data to setup on construction
        protected ushort[] p_SymbolsSID;
        protected string[] p_SymbolsName;
        protected System.Collections.Generic.Dictionary<ushort, MatchSubGrammar> p_SymbolsSubGrammars;
        protected ushort[][][] p_Transitions;
        protected int[] p_Finals;
        protected ushort p_SeparatorID;

        // Lexer state data
        protected System.Collections.Generic.List<LexerTextError> p_Errors;
        protected string p_Input;
        protected int p_Length;
        protected int p_CurrentPosition;
        protected int p_Line;

        public System.Collections.Generic.IEnumerable<LexerTextError> Errors { get { return p_Errors; } }
        public string InputText { get { return p_Input; } }
        public int InputLength { get { return p_Length; } }
        public int CurrentPosition { get { return p_CurrentPosition; } }
        public int CurrentLine { get { return p_Line; } }
        public int CurrentColumn
        {
            get
            {
                int Col = p_CurrentPosition;
                for (int i = p_CurrentPosition; i != 0; i--)
                {
                    if (p_Input[i] == '\n')
                    {
                        Col = p_CurrentPosition - i;
                        break;
                    }
                }
                return Col;
            }
        }

        protected abstract void setup();
        public abstract ILexer Clone();

        protected LexerText(string input)
        {
            setup();
            p_Errors = new System.Collections.Generic.List<LexerTextError>();
            p_Input = input;
            p_Length = input.Length;
            p_Line = 1;
        }
        protected LexerText(string input, int position, int line, System.Collections.Generic.List<LexerTextError> errors)
        {
            setup();
            p_Errors = new System.Collections.Generic.List<LexerTextError>(errors);
            p_CurrentPosition = position;
            p_Input = input;
            p_Length = input.Length;
            p_Line = line;
        }

        public string GetSymbolName(ushort SID)
        {
            for (int i = 0; i != p_SymbolsSID.Length; i++)
            {
                if (p_SymbolsSID[i] == SID)
                    return p_SymbolsName[i];
            }
            return null;
        }

        public SymbolToken GetNextToken(ushort[] IDs) { throw new LexerException("Text lexer does not support this method."); }
        public SymbolToken GetNextToken()
        {
            if (p_CurrentPosition == p_Length)
            {
                p_CurrentPosition++;
                return new SymbolTokenDollar();
            }
            if (p_CurrentPosition > p_Length)
                return new SymbolTokenEpsilon();

            while (true)
            {
                if (p_CurrentPosition == p_Length)
                {
                    p_CurrentPosition++;
                    return new SymbolTokenDollar();
                }
                SymbolTokenText Token = GetNextToken_DFA();
                if (Token == null)
                {
                    p_Errors.Add(new LexerTextErrorDiscardedChar(p_Input[p_CurrentPosition], p_Line, CurrentColumn));
                    p_CurrentPosition++;
                }
                else if (Token.SymbolID == p_SeparatorID)
                {
                    p_CurrentPosition += Token.ValueText.Length;
                    foreach (char c in Token.ValueText) { if (c == '\n') p_Line++; }
                }
                else
                {
                    if (p_SymbolsSubGrammars.ContainsKey(Token.SymbolID))
                    {
                        Token.SubGrammarRoot = p_SymbolsSubGrammars[Token.SymbolID](Token.ValueText);
                    }
                    p_CurrentPosition += Token.ValueText.Length;
                    foreach (char c in Token.ValueText) { if (c == '\n') p_Line++; }
                    return Token;
                }
            }
        }

        private SymbolTokenText GetNextToken_DFA()
        {
            System.Collections.Generic.List<SymbolTokenText> MatchedTokens = new System.Collections.Generic.List<SymbolTokenText>();
            int End = p_CurrentPosition;
            ushort State = 0;

            while (true)
            {
                if (p_Finals[State] != -1)
                {
                    string Value = p_Input.Substring(p_CurrentPosition, End - p_CurrentPosition);
                    MatchedTokens.Add(new SymbolTokenText(p_SymbolsName[p_Finals[State]], p_SymbolsSID[p_Finals[State]], Value, p_Line));
                }
                if (End == p_Length)
                    break;
                ushort Char = System.Convert.ToUInt16(p_Input[End]);
                ushort NextState = 0xFFFF;
                End++;
                for (int i = 0; i != p_Transitions[State].Length; i++)
                {
                    if (Char >= p_Transitions[State][i][0] && Char <= p_Transitions[State][i][1])
                        NextState = p_Transitions[State][i][2];
                }
                if (NextState == 0xFFFF)
                    break;
                State = NextState;
            }
            if (MatchedTokens.Count == 0)
                return null;
            return MatchedTokens[MatchedTokens.Count - 1];
        }
    }

    public abstract class LexerBinary : ILexer
    {
        protected delegate SymbolToken ApplyGetNextToken();

        protected static byte p_Flag1 = 0x01;
        protected static byte p_Flag2 = 0x03;
        protected static byte p_Flag3 = 0x07;
        protected static byte p_Flag4 = 0x0F;
        protected static byte p_Flag5 = 0x1F;
        protected static byte p_Flag6 = 0x3F;
        protected static byte p_Flag7 = 0x7F;
        protected static byte p_Flag8 = 0xFF;
        protected static byte[] p_Flags = { 0x00, p_Flag1, p_Flag2, p_Flag3, p_Flag4, p_Flag5, p_Flag6, p_Flag7, p_Flag8 };

        protected System.Collections.Generic.Dictionary<ushort, ApplyGetNextToken> p_GetNextTokens;
        protected Binary.DataInput p_Input;
        protected int p_CurrentBitLeft;
        protected bool p_DollarEmitted;

        public int CurrentLine { get { return 0; } }
        public int InputLength { get { return p_Input.Length; } }

        protected abstract void setup();
        public abstract ILexer Clone();

        protected LexerBinary(Binary.DataInput input)
        {
            setup();
            p_Input = input;
            p_CurrentBitLeft = 8;
        }

        public SymbolToken GetNextToken() { throw new LexerException("Binary lexer cannot match a token without an expected tokens list."); }

        public SymbolToken GetNextToken(ushort[] IDs)
        {
            if (p_DollarEmitted)
                return new SymbolTokenEpsilon();
            if (p_Input.IsAtEnd && p_CurrentBitLeft == 8)
            {
                p_DollarEmitted = true;
                return new SymbolTokenDollar();
            }

            foreach (ushort ID in IDs)
            {
                SymbolToken Temp = GetNextToken_Apply(ID);
                if (Temp != null) return Temp;
            }
            p_DollarEmitted = true;
            return new SymbolTokenDollar();
        }

        protected SymbolToken GetNextToken_Apply(ushort ID)
        {
            if (!p_GetNextTokens.ContainsKey(ID)) return null;
            return p_GetNextTokens[ID]();
        }



        protected SymbolToken GetNextToken_Apply_NB(ushort sid, string name, byte value, int length)
        {
            if (p_CurrentBitLeft < length) return null;
            if (((p_Input.ReadByte() >> (p_CurrentBitLeft - length)) & p_Flags[length]) != value) return null;
            p_CurrentBitLeft -= length;
            if (p_CurrentBitLeft == 0) { p_CurrentBitLeft = 8; p_Input.ReadAndAdvanceByte(); }
            return new SymbolTokenBits(name, sid, value);
        }
        protected SymbolToken GetNextToken_Apply_N8(ushort sid, string name, byte value)
        {
            if (p_CurrentBitLeft != 8) return null;
            if (!p_Input.CanRead(1)) return null;
            if (p_Input.ReadByte() == value)
            {
                p_Input.ReadAndAdvanceByte();
                return new SymbolTokenUInt8(name, sid, value);
            }
            return null;
        }
        protected SymbolToken GetNextToken_Apply_N16(ushort sid, string name, byte value)
        {
            if (p_CurrentBitLeft != 8) return null;
            if (!p_Input.CanRead(2)) return null;
            if (p_Input.ReadByte() == value)
            {
                p_Input.ReadAndAdvanceUInt16();
                return new SymbolTokenUInt16(name, sid, value);
            }
            return null;
        }
        protected SymbolToken GetNextToken_Apply_N32(ushort sid, string name, byte value)
        {
            if (p_CurrentBitLeft != 8) return null;
            if (!p_Input.CanRead(4)) return null;
            if (p_Input.ReadByte() == value)
            {
                p_Input.ReadAndAdvanceUInt32();
                return new SymbolTokenUInt32(name, sid, value);
            }
            return null;
        }
        protected SymbolToken GetNextToken_Apply_N64(ushort sid, string name, byte value)
        {
            if (p_CurrentBitLeft != 8) return null;
            if (!p_Input.CanRead(8)) return null;
            if (p_Input.ReadByte() == value)
            {
                p_Input.ReadAndAdvanceUInt64();
                return new SymbolTokenUInt64(name, sid, value);
            }
            return null;
        }

        protected SymbolToken GetNextToken_Apply_JB(ushort sid, string name, int length)
        {
            if (p_CurrentBitLeft < length) return null;
            SymbolToken Temp = new SymbolTokenBits(name, sid, (byte)((p_Input.ReadByte() >> (p_CurrentBitLeft - length)) & length));
            p_CurrentBitLeft -= length;
            if (p_CurrentBitLeft == 0) { p_CurrentBitLeft = 8; p_Input.ReadAndAdvanceByte(); }
            return Temp;
        }
        protected SymbolToken GetNextToken_Apply_J8(ushort sid, string name)
        {
            if (p_CurrentBitLeft != 8) return null;
            if (!p_Input.CanRead(1)) return null;
            return new SymbolTokenUInt8(name, sid, p_Input.ReadAndAdvanceByte());
        }
        protected SymbolToken GetNextToken_Apply_J16(ushort sid, string name)
        {
            if (p_CurrentBitLeft != 8) return null;
            if (!p_Input.CanRead(2)) return null;
            return new SymbolTokenUInt16(name, sid, p_Input.ReadAndAdvanceByte());
        }
        protected SymbolToken GetNextToken_Apply_J32(ushort sid, string name)
        {
            if (p_CurrentBitLeft != 8) return null;
            if (!p_Input.CanRead(4)) return null;
            return new SymbolTokenUInt32(name, sid, p_Input.ReadAndAdvanceByte());
        }
        protected SymbolToken GetNextToken_Apply_J64(ushort sid, string name)
        {
            if (p_CurrentBitLeft != 8) return null;
            if (!p_Input.CanRead(8)) return null;
            return new SymbolTokenUInt64(name, sid, p_Input.ReadAndAdvanceByte());
        }
    }
}