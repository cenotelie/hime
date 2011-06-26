using System.Collections.Generic;

namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Represents a symbol in an abstract syntax tree
    /// </summary>
    public abstract class Symbol
    {
        /// <summary>
        /// symbol's unique ID
        /// </summary>
        protected ushort sid;
        /// <summary>
        /// symbol's name
        /// </summary>
        protected string name;

        /// <summary>
        /// Gets the symbol's unique ID
        /// </summary>
        public ushort SymbolID { get { return sid; } }
        /// <summary>
        /// Gets the symbol's name
        /// </summary>
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

    /// <summary>
    /// Represents a piece of data matched by a lexer
    /// </summary>
    public abstract class SymbolToken : Symbol
    {
        /// <summary>
        /// Gets the data represented by this symbol
        /// </summary>
        public abstract object Value { get; }
        /// <summary>
        /// Initializes a new instance of the SymbolToken class with a given name and id
        /// </summary>
        /// <param name="name">The name of the symbol</param>
        /// <param name="sid">The unique ID of the symbol</param>
        public SymbolToken(string name, ushort sid)
        {
            this.sid = sid;
            this.name = name;
        }
    }

    /// <summary>
    /// Represents a piece of text matched by a lexer
    /// </summary>
    public sealed class SymbolTokenText : SymbolToken
    {
        private string value;
        private int line;
        private SyntaxTreeNode subGrammarRoot;

        /// <summary>
        /// Gets the data represented by this symbol
        /// </summary>
        public override object Value { get { return value; } }
        /// <summary>
        /// Gets the text represented by this symbol
        /// </summary>
        public string ValueText { get { return value; } }
        /// <summary>
        /// Gets the line number where the text was found
        /// </summary>
        public int Line { get { return line; } }
        /// <summary>
        /// Gets or sets the root of the abstract syntax tree produced by parsing the text of this symbol
        /// </summary>
        public SyntaxTreeNode SubGrammarRoot
        {
            get { return subGrammarRoot; }
            set { subGrammarRoot = value; }
        }

        /// <summary>
        /// Initializes a new instance of the SymbolTokenText class with the given name, id, value and line number
        /// </summary>
        /// <param name="name">The name of the symbol</param>
        /// <param name="sid">The unique ID of the symbol</param>
        /// <param name="value">The text represented by the symbol</param>
        /// <param name="line">The line number where the text was found</param>
        public SymbolTokenText(string name, ushort sid, string value, int line)
            : base(name, sid)
        {
            this.value = value;
            this.line = line;
            this.subGrammarRoot = null;
        }
    }

    /// <summary>
    /// Represents a special token for the absence of data in a stream
    /// </summary>
    sealed class SymbolTokenEpsilon : SymbolToken
    {
        private static SymbolTokenEpsilon instance = new SymbolTokenEpsilon();
        public static SymbolTokenEpsilon Instance { get { return instance; } }
        public override object Value { get { return string.Empty; } }
        public SymbolTokenEpsilon() : base("ε", 1) { }
    }
    /// <summary>
    /// Represents a special token for the end of data stream
    /// </summary>
    sealed class SymbolTokenDollar : SymbolToken
    {
        private static SymbolTokenDollar instance = new SymbolTokenDollar();
        public static SymbolTokenDollar Instance { get { return instance; } }
        public override object Value { get { return "$"; } }
        public SymbolTokenDollar() : base("$", 2) { }
    }

    /// <summary>
    /// Represents a piece of binary data matched by a lexer
    /// </summary>
    public sealed class SymbolTokenBits : SymbolToken
    {
        private byte value;
        public override object Value { get { return value; } }
        public byte ValueBits { get { return value; } }
        public SymbolTokenBits(string ClassName, ushort ClassSID, byte Value) : base(ClassName, ClassSID) { value = Value; }
    }
    /// <summary>
    /// Represents an unsigned byte matched by a lexer
    /// </summary>
    public sealed class SymbolTokenUInt8 : SymbolToken
    {
        private byte value;
        public override object Value { get { return value; } }
        public byte ValueUInt8 { get { return value; } }
        public SymbolTokenUInt8(string ClassName, ushort ClassSID, byte Value) : base(ClassName, ClassSID) { value = Value; }
    }
    /// <summary>
    /// Represents an unsigned 16-bit integer matched by a lexer
    /// </summary>
    public sealed class SymbolTokenUInt16 : SymbolToken
    {
        private ushort value;
        public override object Value { get { return value; } }
        public ushort ValueUInt16 { get { return value; } }
        public SymbolTokenUInt16(string ClassName, ushort ClassSID, ushort Value) : base(ClassName, ClassSID) { value = Value; }
    }
    /// <summary>
    /// Represents an unsigned 32-bit integer matched by a lexer
    /// </summary>
    public sealed class SymbolTokenUInt32 : SymbolToken
    {
        private uint value;
        public override object Value { get { return value; } }
        public uint ValueUInt32 { get { return value; } }
        public SymbolTokenUInt32(string ClassName, ushort ClassSID, uint Value) : base(ClassName, ClassSID) { value = Value; }
    }
    /// <summary>
    /// Represents an unsigned 64-bit integer matched by a lexer
    /// </summary>
    public sealed class SymbolTokenUInt64 : SymbolToken
    {
        private ulong value;
        public override object Value { get { return value; } }
        public ulong ValueUInt64 { get { return value; } }
        public SymbolTokenUInt64(string ClassName, ushort ClassSID, ulong Value) : base(ClassName, ClassSID) { value = Value; }
    }

    /// <summary>
    /// Represents a synthetic symbol in an abstract syntax tree
    /// </summary>
    public sealed class SymbolVirtual : Symbol
    {
        /// <summary>
        /// Initializes a new instance of the SymbolVirtual class with a name
        /// </summary>
        /// <param name="name">The name of the virtual symbol</param>
        public SymbolVirtual(string name)
        {
            this.name = name;
            this.sid = 0;
        }
        public override int GetHashCode() { return base.GetHashCode(); }
        public override bool Equals(object obj)
        {
            SymbolVirtual other = obj as SymbolVirtual;
            if (other == null)
                return false;
            return (this.name == other.name);
        }
    }

    /// <summary>
    /// Represents an action symbol in a shared packed parse forest
    /// </summary>
    public sealed class SymbolAction : Symbol
    {
        /// <summary>
        /// Represents the method to call for executing the action
        /// </summary>
        /// <param name="Subroot">The syntax tree node on which the action is executed</param>
        public delegate void Callback(SyntaxTreeNode subroot);

        private Callback callback;

        /// <summary>
        /// Gets the callback represented by this symbol
        /// </summary>
        public Callback Action { get { return callback; } }

        /// <summary>
        /// Initializes a new instance of the SymbolAction class with a name and a callback
        /// </summary>
        /// <param name="name">The name of the action symbol</param>
        /// <param name="callback">The callback for the action</param>
        public SymbolAction(string name, Callback callback)
        {
            this.name = name;
            this.sid = 0;
            this.callback = callback;
        }

        public override int GetHashCode() { return base.GetHashCode(); }
        public override bool Equals(object obj)
        {
            SymbolAction other = obj as SymbolAction;
            if (other == null)
                return false;
            return (this.name == other.name);
        }
    }
    
    /// <summary>
    /// Represents a variable in an abstract syntax tree or a grammar
    /// </summary>
    public sealed class SymbolVariable : Symbol
    {
        /// <summary>
        /// Initializes a new instance of the SymbolVariable class with the given id and name
        /// </summary>
        /// <param name="sid">The unique ID of the symbol</param>
        /// <param name="name">The name of the symbol</param>
        public SymbolVariable(ushort sid, string name)
        {
            this.sid = sid;
            this.name = name;
        }
    }

    /// <summary>
    /// Represents a terminal in a grammar
    /// </summary>
    public sealed class SymbolTerminal : Symbol
    {
        /// <summary>
        /// Initializes a new instance of the SymbolTerminal class with the given id and name
        /// </summary>
        /// <param name="name">The name of the symbol</param>
        /// <param name="sid">The unique ID of the symbol</param>
        public SymbolTerminal(string name, ushort sid)
        {
            this.sid = sid;
            this.name = name;
        }
    }
}
