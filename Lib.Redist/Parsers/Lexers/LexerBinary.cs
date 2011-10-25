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
        public int CurrentColumn { get { return input.Length; } }
        public OnErrorHandler OnError { set { } }

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