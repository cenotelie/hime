using System.Collections.Generic;

namespace Hime.Redist.Parsers
{
    [System.Serializable]
    public class LexerException : System.Exception
    {
        public LexerException() : base() { }
        public LexerException(string message) : base(message) { }
        public LexerException(string message, System.Exception innerException) : base(message, innerException) { }
        protected LexerException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    public interface LexerError
    {
        string Message { get; }
    }
    public abstract class LexerTextError : LexerError
    {
        protected int line;
        protected int column;

        public int Line { get { return line; } }
        public int Column { get { return column; } }

        public abstract string Message { get; }

        protected LexerTextError(int line, int column)
        {
            this.line = line;
            this.column = column;
        }

        public abstract override string ToString();
    }

    public class LexerTextErrorDiscardedChar : LexerTextError
    {
        protected char discarded;
        protected string message;

        public char Discarded { get { return discarded; } }
        public override string Message { get { return message; } }

        public LexerTextErrorDiscardedChar(char Discarded, int Line, int Column)
            : base(Line, Column)
        {
            discarded = Discarded;
            System.Text.StringBuilder Builder = new System.Text.StringBuilder("Unrecognized character '");
            Builder.Append(discarded.ToString());
            Builder.Append("' (0x");
            Builder.Append(System.Convert.ToInt32(discarded).ToString("X"));
            Builder.Append(")");
            message = Builder.ToString();
        }
        public override string ToString() { return message; }
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
        protected ushort[] symbolsSID;
        protected string[] symbolsName;
        protected Dictionary<ushort, MatchSubGrammar> symbolsSubGrammars;
        protected ushort[][][] transitions;
        protected int[] finals;
        protected ushort separatorID;

        // Lexer state data
        protected List<LexerTextError> errors;
        protected BufferedTextReader input;
        protected int currentLine;
        protected int currentColumn;
        protected bool isDollatEmited;

        public System.Collections.ObjectModel.ReadOnlyCollection<LexerTextError> Errors { get { return new System.Collections.ObjectModel.ReadOnlyCollection<LexerTextError>(errors); } }
        public string InputText { get { return input.GetReadText(); } }
        public int CurrentLine { get { return currentLine; } }
        public int CurrentColumn { get { return currentColumn; } }
        public bool IsAtEnd { get { return input.AtEnd(); } }

        protected abstract void setup();
        public abstract ILexer Clone();

        protected LexerText(System.IO.TextReader input)
        {
            setup();
            errors = new List<LexerTextError>();
            this.input = new BufferedTextReader(input);
            currentLine = 1;
            currentColumn = 1;
            isDollatEmited = false;
        }
        protected LexerText(LexerText original)
        {
            setup();
            errors = new List<LexerTextError>(original.errors);
            input = original.input.Clone();
            currentLine = original.currentLine;
            currentColumn = original.currentColumn;
            isDollatEmited = original.isDollatEmited;
        }

        public string GetSymbolName(ushort SID)
        {
            for (int i = 0; i != symbolsSID.Length; i++)
            {
                if (symbolsSID[i] == SID)
                    return symbolsName[i];
            }
            return null;
        }

        public SymbolToken GetNextToken(ushort[] IDs) { throw new LexerException("Text lexer does not support this method."); }
        public SymbolToken GetNextToken()
        {
            if (input.AtEnd())
            {
                if (isDollatEmited)
                    return new SymbolTokenEpsilon();
                isDollatEmited = true;
                return new SymbolTokenDollar();
            }

            while (true)
            {
                if (input.AtEnd())
                {
                    isDollatEmited = true;
                    return new SymbolTokenDollar();
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
                        if (symbolsSubGrammars.ContainsKey(Token.SymbolID))
                            Token.SubGrammarRoot = symbolsSubGrammars[Token.SymbolID](Token.ValueText);
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
            List<SymbolTokenText> MatchedTokens = new List<SymbolTokenText>();
            System.Text.StringBuilder Builder = new System.Text.StringBuilder();
            int count = 0;
            ushort State = 0;

            while (true)
            {
                if (finals[State] != -1)
                {
                    string Value = Builder.ToString();
                    MatchedTokens.Add(new SymbolTokenText(symbolsName[finals[State]], symbolsSID[finals[State]], Value, currentLine));
                }
                bool atend = false;
                char c = input.Peek(out atend);
                if (atend)
                    break;
                ushort UCV = System.Convert.ToUInt16(c);
                ushort NextState = 0xFFFF;
                for (int i = 0; i != transitions[State].Length; i++)
                {
                    if (UCV >= transitions[State][i][0] && UCV <= transitions[State][i][1])
                        NextState = transitions[State][i][2];
                }
                if (NextState == 0xFFFF)
                    break;
                State = NextState;
                Builder.Append(input.Read(out atend));
                count++;
            }
            if (MatchedTokens.Count == 0)
            {
                input.Rewind(count);
                return null;
            }
            return MatchedTokens[MatchedTokens.Count - 1];
        }
    }

    public abstract class LexerBinary : ILexer
    {
        protected delegate SymbolToken ApplyGetNextToken();

        protected static byte flag1 = 0x01;
        protected static byte flag2 = 0x03;
        protected static byte flag3 = 0x07;
        protected static byte flag4 = 0x0F;
        protected static byte flag5 = 0x1F;
        protected static byte flag6 = 0x3F;
        protected static byte flag7 = 0x7F;
        protected static byte flag8 = 0xFF;
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
                return new SymbolTokenEpsilon();
            if (input.IsAtEnd && currentBitLeft == 8)
            {
                dollarEmitted = true;
                return new SymbolTokenDollar();
            }

            foreach (ushort ID in IDs)
            {
                SymbolToken Temp = GetNextToken_Apply(ID);
                if (Temp != null) return Temp;
            }
            dollarEmitted = true;
            return new SymbolTokenDollar();
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
