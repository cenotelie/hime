namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Interface for parsers symbols
    /// </summary>
    public interface ISymbol
    {
        /// <summary>
        /// Get the symbol's identifier
        /// </summary>
        /// <value>Symbol's indentifier</value>
        ushort SymbolID { get; }
        /// <summary>
        /// Get the symbol's name
        /// </summary>
        /// <value>Symbol's name</value>
        string Name { get; }
    }

    /// <summary>
    /// Base class for token symbols (matched data in parsers)
    /// </summary>
    public abstract class SymbolToken : ISymbol
    {
        /// <summary>
        /// Token class' name
        /// </summary>
        private string p_ClassName;
        /// <summary>
        /// Token class' identifier
        /// </summary>
        private ushort p_ClassSID;

        /// <summary>
        /// Get the symbol's identifier
        /// </summary>
        /// <value>Symbol's indentifier</value>
        public ushort SymbolID { get { return p_ClassSID; } }
        /// <summary>
        /// Get the symbol's name
        /// </summary>
        /// <value>Symbol's name</value>
        public string Name { get { return p_ClassName; } }
        /// <summary>
        /// Get the token's matched value
        /// </summary>
        /// <value>The matched value</value>
        public abstract object Value { get; }
        /// <summary>
        /// Construct a token from its class' name and indentifier
        /// </summary>
        /// <param name="ClassName">Class name</param>
        /// <param name="ClassSID">Class ID</param>
        public SymbolToken(string ClassName, ushort ClassSID)
        {
            p_ClassName = ClassName;
            p_ClassSID = ClassSID;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is SymbolToken))
                return false;
            SymbolToken other = (SymbolToken)obj;
            return (this.p_ClassSID == other.p_ClassSID);
        }
    }

    /// <summary>
    /// Text Token in a parser
    /// </summary>
    public class SymbolTokenText : SymbolToken
    {
        /// <summary>
        /// Matched value
        /// </summary>
        private string p_Value;
        /// <summary>
        /// Matched line
        /// </summary>
        private int p_Line;
        /// <summary>
        /// Root of the syntax tree for the sub grammar match
        /// </summary>
        private SyntaxTreeNode p_SubGrammarRoot;

        /// <summary>
        /// Get the token's matched value
        /// </summary>
        /// <value>The matched value</value>
        public override object Value { get { return p_Value; } }
        /// <summary>
        /// Get the token's matched text
        /// </summary>
        /// <value>The matched text</value>
        public string ValueText { get { return p_Value; } }
        /// <summary>
        /// Get the token line in the original document
        /// </summary>
        /// <value>Token's line</value>
        public int Line { get { return p_Line; } }
        /// <summary>
        /// Get or set the root of the syntax tree for the sub grammar match
        /// </summary>
        public SyntaxTreeNode SubGrammarRoot
        {
            get { return p_SubGrammarRoot; }
            set { p_SubGrammarRoot = value; }
        }

        /// <summary>
        /// Constructs the text token
        /// </summary>
        /// <param name="ClassName">Token class name</param>
        /// <param name="ClassSID">Token class ID</param>
        /// <param name="Value">Matched text</param>
        /// <param name="Line">Line</param>
        public SymbolTokenText(string ClassName, ushort ClassSID, string Value, int Line)
            : base(ClassName, ClassSID)
        {
            p_Value = Value;
            p_Line = Line;
            p_SubGrammarRoot = null;
        }
    }
    /// <summary>
    /// Special token always matched after the Dollar token
    /// </summary>
    public class SymbolTokenEpsilon : SymbolToken
    {
        /// <summary>
        /// Get the token's matched value
        /// </summary>
        /// <value>The matched value</value>
        public override object Value { get { return string.Empty; } }
        /// <summary>
        /// Construct a new ε token
        /// </summary>
        public SymbolTokenEpsilon() : base("ε", 1) { }
    }
    /// <summary>
    /// Special token always matched at the end of an input
    /// </summary>
    public class SymbolTokenDollar : SymbolToken
    {
        /// <summary>
        /// Get the token's matched value
        /// </summary>
        /// <value>The matched value</value>
        public override object Value { get { return "$"; } }
        /// <summary>
        /// Construct a new dollar token
        /// </summary>
        public SymbolTokenDollar() : base("Dollar", 2) { }
    }

    /// <summary>
    /// Bit field binary token in a parser
    /// </summary>
    public class SymbolTokenBits : SymbolToken
    {
        /// <summary>
        /// Matched value
        /// </summary>
        private byte p_Value;
        /// <summary>
        /// Get the token's matched value
        /// </summary>
        /// <value>The matched value</value>
        public override object Value { get { return p_Value; } }
        /// <summary>
        /// Get the token's matched value
        /// </summary>
        /// <value>The matched value</value>
        public byte ValueBits { get { return p_Value; } }
        /// <summary>
        /// Constructs the token
        /// </summary>
        /// <param name="ClassName">Token class name</param>
        /// <param name="ClassSID">Token class ID</param>
        /// <param name="Value">Matched value</param>
        public SymbolTokenBits(string ClassName, ushort ClassSID, byte Value) : base(ClassName, ClassSID) { p_Value = Value; }
    }
    /// <summary>
    /// Byte binary token in a parser
    /// </summary>
    public class SymbolTokenUInt8 : SymbolToken
    {
        /// <summary>
        /// Matched value
        /// </summary>
        private byte p_Value;
        /// <summary>
        /// Get the token's matched value
        /// </summary>
        /// <value>The matched value</value>
        public override object Value { get { return p_Value; } }
        /// <summary>
        /// Get the token's matched value
        /// </summary>
        /// <value>The matched value</value>
        public byte ValueUInt8 { get { return p_Value; } }
        /// <summary>
        /// Constructs the token
        /// </summary>
        /// <param name="ClassName">Token class name</param>
        /// <param name="ClassSID">Token class ID</param>
        /// <param name="Value">Matched value</param>
        public SymbolTokenUInt8(string ClassName, ushort ClassSID, byte Value) : base(ClassName, ClassSID) { p_Value = Value; }
    }
    /// <summary>
    /// Unsigned 16bit long integer binary token in a parser
    /// </summary>
    public class SymbolTokenUInt16 : SymbolToken
    {
        /// <summary>
        /// Matched value
        /// </summary>
        private ushort p_Value;
        /// <summary>
        /// Get the token's matched value
        /// </summary>
        /// <value>The matched value</value>
        public override object Value { get { return p_Value; } }
        /// <summary>
        /// Get the token's matched value
        /// </summary>
        /// <value>The matched value</value>
        public ushort ValueUInt16 { get { return p_Value; } }
        /// <summary>
        /// Constructs the token
        /// </summary>
        /// <param name="ClassName">Token class name</param>
        /// <param name="ClassSID">Token class ID</param>
        /// <param name="Value">Matched value</param>
        public SymbolTokenUInt16(string ClassName, ushort ClassSID, ushort Value) : base(ClassName, ClassSID) { p_Value = Value; }
    }
    /// <summary>
    /// Unsigned 32bit long integer binary token in a parser
    /// </summary>
    public class SymbolTokenUInt32 : SymbolToken
    {
        /// <summary>
        /// Matched value
        /// </summary>
        private uint p_Value;
        /// <summary>
        /// Get the token's matched value
        /// </summary>
        /// <value>The matched value</value>
        public override object Value { get { return p_Value; } }
        /// <summary>
        /// Get the token's matched value
        /// </summary>
        /// <value>The matched value</value>
        public uint ValueUInt32 { get { return p_Value; } }
        /// <summary>
        /// Constructs the token
        /// </summary>
        /// <param name="ClassName">Token class name</param>
        /// <param name="ClassSID">Token class ID</param>
        /// <param name="Value">Matched value</param>
        public SymbolTokenUInt32(string ClassName, ushort ClassSID, uint Value) : base(ClassName, ClassSID) { p_Value = Value; }
    }
    /// <summary>
    /// Unsigned 64bit long integer binary token in a parser
    /// </summary>
    public class SymbolTokenUInt64 : SymbolToken
    {
        /// <summary>
        /// Matched value
        /// </summary>
        private ulong p_Value;
        /// <summary>
        /// Get the token's matched value
        /// </summary>
        /// <value>The matched value</value>
        public override object Value { get { return p_Value; } }
        /// <summary>
        /// Get the token's matched value
        /// </summary>
        /// <value>The matched value</value>
        public ulong ValueUInt64 { get { return p_Value; } }
        /// <summary>
        /// Constructs the token
        /// </summary>
        /// <param name="ClassName">Token class name</param>
        /// <param name="ClassSID">Token class ID</param>
        /// <param name="Value">Matched value</param>
        public SymbolTokenUInt64(string ClassName, ushort ClassSID, ulong Value) : base(ClassName, ClassSID) { p_Value = Value; }
    }

    /// <summary>
    /// Virtual symbol
    /// </summary>
    public class SymbolVirtual : ISymbol
    {
        /// <summary>
        /// Symbol name
        /// </summary>
        private string p_Name;

        /// <summary>
        /// Get the symbol's name
        /// </summary>
        /// <value>Symbol's name</value>
        public string Name { get { return p_Name; } }
        /// <summary>
        /// Get the symbol's identifier
        /// </summary>
        /// <value>Symbol's indentifier</value>
        public ushort SymbolID { get { return 0; } }

        /// <summary>
        /// Constructs the virtual symbol
        /// </summary>
        /// <param name="Name">Symbol name</param>
        public SymbolVirtual(string Name) { p_Name = Name; }

        public override bool Equals(object obj)
        {
            if (!(obj is SymbolVirtual))
                return false;
            SymbolVirtual other = (SymbolVirtual)obj;
            return (this.p_Name == other.p_Name);
        }
    }

    /// <summary>
    /// Action symbol
    /// </summary>
    public class SymbolAction : ISymbol
    {
        public delegate void Callback(SyntaxTreeNode Subroot);

        /// <summary>
        /// Symbol name
        /// </summary>
        private string p_Name;

        private Callback p_Callback;

        /// <summary>
        /// Get the symbol's name
        /// </summary>
        /// <value>Symbol's name</value>
        public string Name { get { return p_Name; } }
        /// <summary>
        /// Get the symbol's identifier
        /// </summary>
        /// <value>Symbol's indentifier</value>
        public ushort SymbolID { get { return 0; } }

        public Callback Action { get { return p_Callback; } }

        /// <summary>
        /// Constructs the virtual symbol
        /// </summary>
        /// <param name="Name">Symbol name</param>
        public SymbolAction(string Name, Callback callback)
        {
            p_Name = Name;
            p_Callback = callback;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is SymbolAction))
                return false;
            SymbolAction other = (SymbolAction)obj;
            return (this.p_Name == other.p_Name);
        }
    }
    
    /// <summary>
    /// Variable symbol
    /// </summary>
    public class SymbolVariable : ISymbol
    {
        /// <summary>
        /// Symbol ID
        /// </summary>
        private ushort p_SID;
        /// <summary>
        /// Symbol name
        /// </summary>
        private string p_Name;

        /// <summary>
        /// Get the symbol's name
        /// </summary>
        /// <value>Symbol's name</value>
        public string Name { get { return p_Name; } }
        /// <summary>
        /// Get the symbol's identifier
        /// </summary>
        /// <value>Symbol's indentifier</value>
        public ushort SymbolID { get { return p_SID; } }

        /// <summary>
        /// Constructs the variable
        /// </summary>
        /// <param name="SID">Symbol ID</param>
        /// <param name="Name">Symbol Name</param>
        public SymbolVariable(ushort SID, string Name)
        {
            p_SID = SID;
            p_Name = Name;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is SymbolVariable))
                return false;
            SymbolVariable other = (SymbolVariable)obj;
            return (this.p_SID == other.p_SID);
        }
    }
}