using System.Collections.Generic;

namespace Hime.Redist.Parsers
{
    public abstract class Symbol
    {
        protected ushort sid;
        protected string name;

        public ushort SymbolID { get { return sid; } }
        public string Name { get { return name; } }

        public override string ToString() { return name; }
        public override int GetHashCode() { return base.GetHashCode(); }
        public override bool Equals(object obj)
        {
            Symbol symbol = obj as Symbol;
            if (symbol == null)
                return false;
            return this.sid == symbol.sid;
        }
    }

    public abstract class SymbolToken : Symbol
    {
        public abstract object Value { get; }
        public SymbolToken(string name, ushort sid)
        {
            this.sid = sid;
            this.name = name;
        }
    }

    public sealed class SymbolTokenText : SymbolToken
    {
        private string value;
        private int line;
        private SyntaxTreeNode subGrammarRoot;

        public override object Value { get { return value; } }
        public string ValueText { get { return value; } }
        public int Line { get { return line; } }
        public SyntaxTreeNode SubGrammarRoot
        {
            get { return subGrammarRoot; }
            set { subGrammarRoot = value; }
        }

        public SymbolTokenText(string name, ushort sid, string value, int line)
            : base(name, sid)
        {
            this.value = value;
            this.line = line;
            this.subGrammarRoot = null;
        }
    }
    public sealed class SymbolTokenEpsilon : SymbolToken
    {
        private static SymbolTokenEpsilon instance = new SymbolTokenEpsilon();
        public static SymbolTokenEpsilon Instance { get { return instance; } }
        public override object Value { get { return string.Empty; } }
        public SymbolTokenEpsilon() : base("ε", 1) { }
    }
    public sealed class SymbolTokenDollar : SymbolToken
    {
        private static SymbolTokenDollar instance = new SymbolTokenDollar();
        public static SymbolTokenDollar Instance { get { return instance; } }
        public override object Value { get { return "$"; } }
        public SymbolTokenDollar() : base("$", 2) { }
    }

    public sealed class SymbolTokenBits : SymbolToken
    {
        private byte value;
        public override object Value { get { return value; } }
        public byte ValueBits { get { return value; } }
        public SymbolTokenBits(string ClassName, ushort ClassSID, byte Value) : base(ClassName, ClassSID) { value = Value; }
    }
    public sealed class SymbolTokenUInt8 : SymbolToken
    {
        private byte value;
        public override object Value { get { return value; } }
        public byte ValueUInt8 { get { return value; } }
        public SymbolTokenUInt8(string ClassName, ushort ClassSID, byte Value) : base(ClassName, ClassSID) { value = Value; }
    }
    public sealed class SymbolTokenUInt16 : SymbolToken
    {
        private ushort value;
        public override object Value { get { return value; } }
        public ushort ValueUInt16 { get { return value; } }
        public SymbolTokenUInt16(string ClassName, ushort ClassSID, ushort Value) : base(ClassName, ClassSID) { value = Value; }
    }
    public sealed class SymbolTokenUInt32 : SymbolToken
    {
        private uint value;
        public override object Value { get { return value; } }
        public uint ValueUInt32 { get { return value; } }
        public SymbolTokenUInt32(string ClassName, ushort ClassSID, uint Value) : base(ClassName, ClassSID) { value = Value; }
    }
    public sealed class SymbolTokenUInt64 : SymbolToken
    {
        private ulong value;
        public override object Value { get { return value; } }
        public ulong ValueUInt64 { get { return value; } }
        public SymbolTokenUInt64(string ClassName, ushort ClassSID, ulong Value) : base(ClassName, ClassSID) { value = Value; }
    }

    public sealed class SymbolVirtual : Symbol
    {
        public SymbolVirtual(string name)
        {
            this.name = name;
            this.sid = 0;
        }
        public override bool Equals(object obj)
        {
            SymbolVirtual other = obj as SymbolVirtual;
            if (other == null)
                return false;
            return (this.name == other.name);
        }
    }

    public sealed class SymbolAction : Symbol
    {
        public delegate void Callback(SyntaxTreeNode Subroot);

        private Callback callback;

        public Callback Action { get { return callback; } }

        public SymbolAction(string name, Callback callback)
        {
            this.name = name;
            this.sid = 0;
            this.callback = callback;
        }

        public override bool Equals(object obj)
        {
            SymbolAction other = obj as SymbolAction;
            if (other == null)
                return false;
            return (this.name == other.name);
        }
    }
    
    public sealed class SymbolVariable : Symbol
    {
        public SymbolVariable(ushort sid, string name)
        {
            this.sid = sid;
            this.name = name;
        }
    }

    public sealed class SymbolTerminal : Symbol
    {
        public SymbolTerminal(string name, ushort sid)
        {
            this.sid = sid;
            this.name = name;
        }
    }
}
