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
        private string className;
        private ushort classSID;

        public override ushort SymbolID { get { return classSID; } }
        public override string Name { get { return className; } }
        public abstract object Value { get; }
        public SymbolToken(string ClassName, ushort ClassSID)
        {
            className = ClassName;
            classSID = ClassSID;
        }

        public override int GetHashCode() { return base.GetHashCode(); }
        public override bool Equals(object obj)
        {
            if (!(obj is SymbolToken))
                return false;
            SymbolToken other = (SymbolToken)obj;
            return (this.classSID == other.classSID);
        }
    }

    public class SymbolTokenText : SymbolToken
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

        public SymbolTokenText(string ClassName, ushort ClassSID, string Value, int Line)
            : base(ClassName, ClassSID)
        {
            value = Value;
            line = Line;
            subGrammarRoot = null;
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
        private byte value;
        public override object Value { get { return value; } }
        public byte ValueBits { get { return value; } }
        public SymbolTokenBits(string ClassName, ushort ClassSID, byte Value) : base(ClassName, ClassSID) { value = Value; }
    }
    public class SymbolTokenUInt8 : SymbolToken
    {
        private byte value;
        public override object Value { get { return value; } }
        public byte ValueUInt8 { get { return value; } }
        public SymbolTokenUInt8(string ClassName, ushort ClassSID, byte Value) : base(ClassName, ClassSID) { value = Value; }
    }
    public class SymbolTokenUInt16 : SymbolToken
    {
        private ushort value;
        public override object Value { get { return value; } }
        public ushort ValueUInt16 { get { return value; } }
        public SymbolTokenUInt16(string ClassName, ushort ClassSID, ushort Value) : base(ClassName, ClassSID) { value = Value; }
    }
    public class SymbolTokenUInt32 : SymbolToken
    {
        private uint value;
        public override object Value { get { return value; } }
        public uint ValueUInt32 { get { return value; } }
        public SymbolTokenUInt32(string ClassName, ushort ClassSID, uint Value) : base(ClassName, ClassSID) { value = Value; }
    }
    public class SymbolTokenUInt64 : SymbolToken
    {
        private ulong value;
        public override object Value { get { return value; } }
        public ulong ValueUInt64 { get { return value; } }
        public SymbolTokenUInt64(string ClassName, ushort ClassSID, ulong Value) : base(ClassName, ClassSID) { value = Value; }
    }

    public class SymbolVirtual : Symbol
    {
        private string name;

        public override string Name { get { return name; } }
        public override ushort SymbolID { get { return 0; } }

        public SymbolVirtual(string Name) { name = Name; }

        public override int GetHashCode() { return base.GetHashCode(); }
        public override bool Equals(object obj)
        {
            if (!(obj is SymbolVirtual))
                return false;
            SymbolVirtual other = (SymbolVirtual)obj;
            return (this.name == other.name);
        }
    }

    public class SymbolAction : Symbol
    {
        public delegate void Callback(SyntaxTreeNode Subroot);

        private string name;

        private Callback callback;

        public override string Name { get { return name; } }
        public override ushort SymbolID { get { return 0; } }

        public Callback Action { get { return callback; } }

        public SymbolAction(string Name, Callback callback)
        {
            this.name = Name;
            this.callback = callback;
        }

        public override int GetHashCode() { return base.GetHashCode(); }
        public override bool Equals(object obj)
        {
            if (!(obj is SymbolAction))
                return false;
            SymbolAction other = (SymbolAction)obj;
            return (this.name == other.name);
        }
    }
    
    public class SymbolVariable : Symbol
    {
        private ushort sID;
        private string name;

        public override string Name { get { return name; } }
        public override ushort SymbolID { get { return sID; } }

        public SymbolVariable(ushort SID, string Name)
        {
            sID = SID;
            name = Name;
        }

        public override int GetHashCode() { return base.GetHashCode(); }
        public override bool Equals(object obj)
        {
            if (!(obj is SymbolVariable))
                return false;
            SymbolVariable other = (SymbolVariable)obj;
            return (this.sID == other.sID);
        }
    }
}
