using System.Collections.Generic;

namespace Hime.Redist.Parsers
{
    public abstract class Symbol
    {
        public abstract ushort SymbolID { get; }
        public abstract string Name { get; }

        public override string ToString() { return Name; }
    }

    public abstract class SymbolToken : Symbol
    {
        private string p_ClassName;
        private ushort p_ClassSID;

        public override ushort SymbolID { get { return p_ClassSID; } }
        public override string Name { get { return p_ClassName; } }
        public abstract object Value { get; }
        public SymbolToken(string ClassName, ushort ClassSID)
        {
            p_ClassName = ClassName;
            p_ClassSID = ClassSID;
        }

        public override int GetHashCode() { return base.GetHashCode(); }
        public override bool Equals(object obj)
        {
            if (!(obj is SymbolToken))
                return false;
            SymbolToken other = (SymbolToken)obj;
            return (this.p_ClassSID == other.p_ClassSID);
        }
    }

    public class SymbolTokenText : SymbolToken
    {
        private string p_Value;
        private int p_Line;
        private SyntaxTreeNode p_SubGrammarRoot;

        public override object Value { get { return p_Value; } }
        public string ValueText { get { return p_Value; } }
        public int Line { get { return p_Line; } }
        public SyntaxTreeNode SubGrammarRoot
        {
            get { return p_SubGrammarRoot; }
            set { p_SubGrammarRoot = value; }
        }

        public SymbolTokenText(string ClassName, ushort ClassSID, string Value, int Line)
            : base(ClassName, ClassSID)
        {
            p_Value = Value;
            p_Line = Line;
            p_SubGrammarRoot = null;
        }
    }
    public class SymbolTokenEpsilon : SymbolToken
    {
        public override object Value { get { return string.Empty; } }
        public SymbolTokenEpsilon() : base("ε", 1) { }
    }
    public class SymbolTokenDollar : SymbolToken
    {
        public override object Value { get { return "$"; } }
        public SymbolTokenDollar() : base("Dollar", 2) { }
    }

    public class SymbolTokenBits : SymbolToken
    {
        private byte p_Value;
        public override object Value { get { return p_Value; } }
        public byte ValueBits { get { return p_Value; } }
        public SymbolTokenBits(string ClassName, ushort ClassSID, byte Value) : base(ClassName, ClassSID) { p_Value = Value; }
    }
    public class SymbolTokenUInt8 : SymbolToken
    {
        private byte p_Value;
        public override object Value { get { return p_Value; } }
        public byte ValueUInt8 { get { return p_Value; } }
        public SymbolTokenUInt8(string ClassName, ushort ClassSID, byte Value) : base(ClassName, ClassSID) { p_Value = Value; }
    }
    public class SymbolTokenUInt16 : SymbolToken
    {
        private ushort p_Value;
        public override object Value { get { return p_Value; } }
        public ushort ValueUInt16 { get { return p_Value; } }
        public SymbolTokenUInt16(string ClassName, ushort ClassSID, ushort Value) : base(ClassName, ClassSID) { p_Value = Value; }
    }
    public class SymbolTokenUInt32 : SymbolToken
    {
        private uint p_Value;
        public override object Value { get { return p_Value; } }
        public uint ValueUInt32 { get { return p_Value; } }
        public SymbolTokenUInt32(string ClassName, ushort ClassSID, uint Value) : base(ClassName, ClassSID) { p_Value = Value; }
    }
    public class SymbolTokenUInt64 : SymbolToken
    {
        private ulong p_Value;
        public override object Value { get { return p_Value; } }
        public ulong ValueUInt64 { get { return p_Value; } }
        public SymbolTokenUInt64(string ClassName, ushort ClassSID, ulong Value) : base(ClassName, ClassSID) { p_Value = Value; }
    }

    public class SymbolVirtual : Symbol
    {
        private string p_Name;

        public override string Name { get { return p_Name; } }
        public override ushort SymbolID { get { return 0; } }

        public SymbolVirtual(string Name) { p_Name = Name; }

        public override int GetHashCode() { return base.GetHashCode(); }
        public override bool Equals(object obj)
        {
            if (!(obj is SymbolVirtual))
                return false;
            SymbolVirtual other = (SymbolVirtual)obj;
            return (this.p_Name == other.p_Name);
        }
    }

    public class SymbolAction : Symbol
    {
        public delegate void Callback(SyntaxTreeNode Subroot);

        private string p_Name;

        private Callback p_Callback;

        public override string Name { get { return p_Name; } }
        public override ushort SymbolID { get { return 0; } }

        public Callback Action { get { return p_Callback; } }

        public SymbolAction(string Name, Callback callback)
        {
            p_Name = Name;
            p_Callback = callback;
        }

        public override int GetHashCode() { return base.GetHashCode(); }
        public override bool Equals(object obj)
        {
            if (!(obj is SymbolAction))
                return false;
            SymbolAction other = (SymbolAction)obj;
            return (this.p_Name == other.p_Name);
        }
    }
    
    public class SymbolVariable : Symbol
    {
        private ushort p_SID;
        private string p_Name;

        public override string Name { get { return p_Name; } }
        public override ushort SymbolID { get { return p_SID; } }

        public SymbolVariable(ushort SID, string Name)
        {
            p_SID = SID;
            p_Name = Name;
        }

        public override int GetHashCode() { return base.GetHashCode(); }
        public override bool Equals(object obj)
        {
            if (!(obj is SymbolVariable))
                return false;
            SymbolVariable other = (SymbolVariable)obj;
            return (this.p_SID == other.p_SID);
        }
    }
}
