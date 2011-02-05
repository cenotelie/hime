namespace Hime.Kernel.Parsers
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
        public SymbolTokenText(string ClassName, ushort ClassSID, string Value, int Line) : base(ClassName, ClassSID)
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
    /// Unsigned 16bit long integer binary token in a parser
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
    }

    /// <summary>
    /// Action symbol
    /// </summary>
    public class SymbolAction : ISymbol
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
        public SymbolAction(string Name) { p_Name = Name; }
    }
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
    }




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
    public abstract class LexerError : Hime.Kernel.Reporting.Entry
    {
        public Hime.Kernel.Reporting.Level Level { get { return Hime.Kernel.Reporting.Level.Error; } }
        public abstract string Component { get; }
        public abstract string Message { get; }
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
        protected string p_Component;
        protected char p_Discarded;
        protected string p_Message;

        public char Discarded { get { return p_Discarded; } }
        public override string Component { get { return p_Component; } }
        public override string Message { get { return p_Message; } }

        public LexerTextErrorDiscardedChar(char Discarded, int Line, int Column) : base(Line, Column)
        {
            p_Discarded = Discarded;
            System.Text.StringBuilder Builder = new System.Text.StringBuilder("Unrecognized character '");
            Builder.Append(p_Discarded.ToString());
            Builder.Append("' (0x");
            Builder.Append(System.Convert.ToInt32(p_Discarded).ToString("X"));
            Builder.Append(")");
            p_Message = Builder.ToString();
            p_Component = "Lexer";
        }
        public override string ToString() { return p_Message; }
    }






    public interface ILexer
    {
        SymbolToken GetNextToken();
        SymbolToken GetNextToken(ushort[] ids);
    }

    public abstract class LexerText : ILexer
    {
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
				for (int i=p_CurrentPosition; i!=0; i--)
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

        protected LexerText(string input)
		{
            p_Errors = new System.Collections.Generic.List<LexerTextError>();
			p_Input = input;
			p_Length = input.Length;
			p_Line = 1;
		}
        protected LexerText(string input, int position, int line, System.Collections.Generic.List<LexerTextError> errors)
        {
            p_Errors = new System.Collections.Generic.List<LexerTextError>(errors);
            p_CurrentPosition = position;
            p_Input = input;
            p_Length = input.Length;
            p_Line = line;
        }

        public abstract SymbolToken GetNextToken();
        public abstract SymbolToken GetNextToken(ushort[] ids);
    }

    public abstract class LexerBinary : ILexer
    {
        protected const byte p_Flag1 = 0x01;
        protected const byte p_Flag2 = 0x03;
        protected const byte p_Flag3 = 0x07;
        protected const byte p_Flag4 = 0x0F;
        protected const byte p_Flag5 = 0x1F;
        protected const byte p_Flag6 = 0x3F;
        protected const byte p_Flag7 = 0x7F;
        protected const byte p_Flag8 = 0xFF;

        protected Binary.DataInput p_Input;
        protected int p_CurrentBitLeft;
        protected bool p_DollarEmitted;
		
		public int InputLength { get { return p_Input.Length; } }

        protected LexerBinary(Binary.DataInput input)
		{
			p_Input = input;
			p_CurrentBitLeft = 8;
		}

        public SymbolToken GetNextToken() { throw new LexerException("Binary lexer cannot match a token without an expected tokens list."); }
		public abstract SymbolToken GetNextToken(ushort[] IDs);
    }





    public class ParserException : System.Exception
    {
        public ParserException() : base() { }
        public ParserException(string message) : base(message) { }
        public ParserException(string message, System.Exception innerException) : base(message, innerException) { }
        protected ParserException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
    public abstract class ParserError : Hime.Kernel.Reporting.Entry
    {
        public Hime.Kernel.Reporting.Level Level { get { return Hime.Kernel.Reporting.Level.Error; } }
        public abstract string Component { get; }
        public abstract string Message { get; }
    }
    public class ParserErrorUnexpectedToken : ParserError
    {
        private string p_Component;
        private SymbolToken p_Token;
        private System.Collections.ObjectModel.Collection<string> p_Expected;
        private System.Collections.ObjectModel.ReadOnlyCollection<string> p_ReadOnlyExpected;
        private string p_Message;

        public SymbolToken UnexpectedToken { get { return p_Token; } }
        public System.Collections.ObjectModel.ReadOnlyCollection<string> ExpectedTokens { get { return p_ReadOnlyExpected; } }
        public override string Component { get { return p_Component; } }
        public override string Message { get { return p_Message; } }

        public ParserErrorUnexpectedToken(SymbolToken Token, string[] Expected)
        {
            p_Token = Token;
            p_Expected = new System.Collections.ObjectModel.Collection<string>(Expected);
            p_ReadOnlyExpected = new System.Collections.ObjectModel.ReadOnlyCollection<string>(p_Expected);
            System.Text.StringBuilder Builder = new System.Text.StringBuilder("Unexpected token ");
            Builder.Append(p_Token.Value.ToString());
            Builder.Append(", expected : { ");
            for (int i = 0; i != p_Expected.Count; i++)
            {
                if (i != 0) Builder.Append(", ");
                Builder.Append(p_Expected[i]);
            }
            Builder.Append(" }.");
            p_Message = Builder.ToString();
            p_Component = "Parser";
        }
        public override string ToString() { return "Parser Error : unexpected token"; }
    }

    public enum SyntaxTreeNodeAction
    {
        Promote,
        Drop,
        Replace,
        Nothing
    }
    public class SyntaxTreeNode
    {
        protected System.Collections.Generic.Dictionary<string, object> p_Properties;
        protected SyntaxTreeNodeCollection p_Children;
        protected System.Collections.ObjectModel.ReadOnlyCollection<SyntaxTreeNode> p_ReadOnlyChildren;
        protected SyntaxTreeNode p_Parent;
        protected ISymbol p_Symbol;
        protected SyntaxTreeNodeAction p_Action;

        public System.Collections.Generic.Dictionary<string, object> Properties { get { return p_Properties; } }
        public ISymbol Symbol { get { return p_Symbol; } }
        public SyntaxTreeNode Parent { get { return p_Parent; } }
        public System.Collections.ObjectModel.ReadOnlyCollection<SyntaxTreeNode> Children { get { return p_ReadOnlyChildren; } }

        public SyntaxTreeNode(ISymbol Symbol)
        {
            p_Properties = new System.Collections.Generic.Dictionary<string, object>();
            p_Children = new SyntaxTreeNodeCollection();
            p_ReadOnlyChildren = new System.Collections.ObjectModel.ReadOnlyCollection<SyntaxTreeNode>(p_Children);
            p_Symbol = Symbol;
            p_Action = SyntaxTreeNodeAction.Nothing;
        }
        public SyntaxTreeNode(ISymbol Symbol, SyntaxTreeNodeAction Action)
        {
            p_Properties = new System.Collections.Generic.Dictionary<string, object>();
            p_Children = new SyntaxTreeNodeCollection();
            p_ReadOnlyChildren = new System.Collections.ObjectModel.ReadOnlyCollection<SyntaxTreeNode>(p_Children);
            p_Symbol = Symbol;
            p_Action = Action;
        }

        public void AppendChild(SyntaxTreeNode Node)
        {
            if (Node.p_Parent != null)
                Node.p_Parent.p_Children.Remove(Node);
            Node.p_Parent = this;
            p_Children.Add(Node);
        }
        public void AppendChild(SyntaxTreeNode Node, SyntaxTreeNodeAction Action)
        {
            if (Node.p_Parent != null)
                Node.p_Parent.p_Children.Remove(Node);
            Node.p_Parent = this;
            Node.p_Action = Action;
            p_Children.Add(Node);
        }
        public void AppendRange(System.Collections.Generic.IEnumerable<SyntaxTreeNode> Nodes)
        {
            System.Collections.Generic.List<SyntaxTreeNode> Temp = new System.Collections.Generic.List<SyntaxTreeNode>(Nodes);
            foreach (SyntaxTreeNode Node in Temp)
                AppendChild(Node);
        }

        public SyntaxTreeNode ApplyActions()
        {
            ApplyActions_DropReplace();
            return ApplyActions_Promote();
        }

        private void ApplyActions_DropReplace()
        {
            for (int i = 0; i != p_Children.Count; i++)
            {
                if (p_Children[i].p_Action == SyntaxTreeNodeAction.Drop)
                {
                    p_Children.RemoveAt(i);
                    i--;
                    continue;
                }
                if (p_Children[i].p_Symbol is SymbolTokenText)
                {
                    SymbolTokenText TokenText = (SymbolTokenText)p_Children[i].p_Symbol;
                    if (TokenText.SubGrammarRoot != null)
                        p_Children[i] = TokenText.SubGrammarRoot;
                }

                p_Children[i].ApplyActions_DropReplace();

                if (p_Children[i].p_Action == SyntaxTreeNodeAction.Replace)
                {
                    SyntaxTreeNodeCollection NewChildren = p_Children[i].p_Children;
                    foreach (SyntaxTreeNode Child in NewChildren)
                        Child.p_Parent = this;
                    p_Children.RemoveAt(i);
                    p_Children.InsertRange(i, NewChildren);
                    i += NewChildren.Count - 1;
                }
            }
        }

        private SyntaxTreeNode ApplyActions_Promote()
        {
            SyntaxTreeNode NewRoot = null;

            for (int i = 0; i != p_Children.Count; i++)
                p_Children[i] = p_Children[i].ApplyActions_Promote();

            for (int i = 0; i != p_Children.Count; i++)
            {
                if (p_Children[i].p_Action == SyntaxTreeNodeAction.Promote)
                {
                    if (NewRoot == null)
                    {
                        NewRoot = p_Children[i];
                        NewRoot.p_Children.InsertRange(0, p_Children.GetRange(0, i));
                        if (i != p_Children.Count - 1)
                            NewRoot.p_Children.AddRange(p_Children.GetRange(i + 1, p_Children.Count - i - 1));
                    }
                    else
                    {
                        int CountOnRight = p_Children.Count - i - 1;
                        int Index = NewRoot.p_Children.Count - CountOnRight - 1;
                        p_Children[i].p_Children.Insert(0, NewRoot);
                        p_Children[i].p_Children.AddRange(NewRoot.p_Children.GetRange(Index + 1, CountOnRight));
                        NewRoot.p_Children.RemoveRange(Index, CountOnRight + 1);
                        NewRoot = p_Children[i];
                    }
                    // Relink
                    foreach (SyntaxTreeNode Child in NewRoot.p_Children)
                        Child.p_Parent = NewRoot;
                    NewRoot.p_Action = SyntaxTreeNodeAction.Nothing;
                }
            }

            if (NewRoot == null)
                return this;
            NewRoot.p_Action = this.p_Action;
            return NewRoot;
        }

        public System.Xml.XmlNode GetXMLNode(System.Xml.XmlDocument Doc)
        {
            System.Xml.XmlNode Node = Doc.CreateElement(p_Symbol.Name);
            if (p_Symbol is SymbolToken)
            {
                SymbolToken Token = (SymbolToken)p_Symbol;
                Node.AppendChild(Doc.CreateTextNode(Token.ToString()));
            }
            foreach (string Property in p_Properties.Keys)
            {
                System.Xml.XmlAttribute Attribute = Doc.CreateAttribute(Property);
                Attribute.Value = p_Properties[Property].ToString();
                Node.Attributes.Append(Attribute);
            }
            foreach (SyntaxTreeNode Child in p_Children)
                Node.AppendChild(Child.GetXMLNode(Doc));
            return Node;
        }
    }

    public class SyntaxTreeNodeCollection : System.Collections.Generic.List<SyntaxTreeNode>
    {
        public SyntaxTreeNodeCollection() : base() { }
        public SyntaxTreeNodeCollection(System.Collections.Generic.IEnumerable<SyntaxTreeNode> collection) : base(collection) { }
    }



    public interface IParser
    {
        System.Collections.Generic.List<ParserError> Errors { get; }
        SyntaxTreeNode Analyse();
    }
}