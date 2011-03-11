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
        protected BufferedTextReader p_Input;
        protected int p_CurrentLine;
        protected int p_CurrentColumn;
        protected bool p_IsDollatEmited;

        public System.Collections.ObjectModel.ReadOnlyCollection<LexerTextError> Errors { get { return new System.Collections.ObjectModel.ReadOnlyCollection<LexerTextError>(p_Errors); } }
        public string InputText { get { return p_Input.GetReadText(); } }
        public int CurrentLine { get { return p_CurrentLine; } }
        public int CurrentColumn { get { return p_CurrentColumn; } }
        public bool IsAtEnd { get { return p_Input.AtEnd(); } }

        protected abstract void setup();
        public abstract ILexer Clone();

        protected LexerText(System.IO.TextReader input)
        {
            setup();
            p_Errors = new System.Collections.Generic.List<LexerTextError>();
            p_Input = new BufferedTextReader(input);
            p_CurrentLine = 1;
            p_CurrentColumn = 1;
            p_IsDollatEmited = false;
        }
        protected LexerText(LexerText original)
        {
            setup();
            p_Errors = new System.Collections.Generic.List<LexerTextError>(original.p_Errors);
            p_Input = original.p_Input.Clone();
            p_CurrentLine = original.p_CurrentLine;
            p_CurrentColumn = original.p_CurrentColumn;
            p_IsDollatEmited = original.p_IsDollatEmited;
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
            if (p_Input.AtEnd())
            {
                if (p_IsDollatEmited)
                    return new SymbolTokenEpsilon();
                p_IsDollatEmited = true;
                return new SymbolTokenDollar();
            }

            while (true)
            {
                if (p_Input.AtEnd())
                {
                    p_IsDollatEmited = true;
                    return new SymbolTokenDollar();
                }
                SymbolTokenText Token = GetNextToken_DFA();
                if (Token == null)
                {
                    bool atend = false;
                    char c = p_Input.Read(out atend);
                    p_Errors.Add(new LexerTextErrorDiscardedChar(c, p_CurrentLine, p_CurrentColumn));
                    AdvanceStats(c.ToString());
                }
                else
                {
                    AdvanceStats(Token.ValueText);
                    if (Token.SymbolID != p_SeparatorID)
                    {
                        if (p_SymbolsSubGrammars.ContainsKey(Token.SymbolID))
                            Token.SubGrammarRoot = p_SymbolsSubGrammars[Token.SymbolID](Token.ValueText);
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
                    p_CurrentLine++;
                    p_CurrentColumn = 1;
                }
                else
                    p_CurrentColumn++;
            }
        }

        private SymbolTokenText GetNextToken_DFA()
        {
            System.Collections.Generic.List<SymbolTokenText> MatchedTokens = new System.Collections.Generic.List<SymbolTokenText>();
            System.Text.StringBuilder Builder = new System.Text.StringBuilder();
            int count = 0;
            ushort State = 0;

            while (true)
            {
                if (p_Finals[State] != -1)
                {
                    string Value = Builder.ToString();
                    MatchedTokens.Add(new SymbolTokenText(p_SymbolsName[p_Finals[State]], p_SymbolsSID[p_Finals[State]], Value, p_CurrentLine));
                }
                bool atend = false;
                char c = p_Input.Peek(out atend);
                if (atend)
                    break;
                ushort UCV = System.Convert.ToUInt16(c);
                ushort NextState = 0xFFFF;
                for (int i = 0; i != p_Transitions[State].Length; i++)
                {
                    if (UCV >= p_Transitions[State][i][0] && UCV <= p_Transitions[State][i][1])
                        NextState = p_Transitions[State][i][2];
                }
                if (NextState == 0xFFFF)
                    break;
                State = NextState;
                Builder.Append(p_Input.Read(out atend));
                count++;
            }
            if (MatchedTokens.Count == 0)
            {
                p_Input.Rewind(count);
                return null;
            }
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