namespace Hime.Generators.Parsers
{
    /// <summary>
    /// Base class for a grammar symbol
    /// </summary>
    public abstract class Symbol : Hime.Kernel.Symbol
    {
        /// <summary>
        /// Parent symbol set grammar
        /// </summary>
        protected Grammar p_Parent;
        /// <summary>
        /// Symbol unique identifier
        /// </summary>
        protected ushort p_SID;
        /// <summary>
        /// Symbol local name
        /// </summary>
        protected string p_LocalName;
        /// <summary>
        /// Symbol complete name
        /// </summary>
        protected Hime.Kernel.QualifiedName p_CompleteName;

        /// <summary>
        /// Get the SID
        /// </summary>
        /// <value>The symbol unique ID</value>
        public ushort SID { get { return p_SID; } }
        /// <summary>
        /// Get the parent symbol
        /// </summary>
        /// <value>The symbol containing the current symbol</value>
        public override Hime.Kernel.Symbol Parent { get { return p_Parent; } }
        /// <summary>
        /// Get the local (naked) name
        /// </summary>
        /// <value>The symbol local name</value>
        public override string LocalName { get { return p_LocalName; } }
        /// <summary>
        /// Get the complete (fully qualified) name
        /// </summary>
        /// <value>The symbol complete name</value>
        public override Hime.Kernel.QualifiedName CompleteName { get { return p_CompleteName; } }

        public Symbol(Grammar Parent, ushort SID, string Name) : base()
        {
            p_Parent = Parent;
            p_SID = SID;
            p_LocalName = Name;
            if (Parent == null)
                p_CompleteName = new Hime.Kernel.QualifiedName(p_LocalName);
            else
                p_CompleteName = Parent.CompleteName + p_LocalName;
        }

        protected override void SymbolSetParent(Hime.Kernel.Symbol Symbol)
        {
            if (Symbol is Grammar)
                p_Parent = (Grammar)Symbol;
            else
                throw new Kernel.WrongParentSymbolException(this, Symbol.GetType(), typeof(Grammar));
        }
        protected override void SymbolSetCompleteName(Hime.Kernel.QualifiedName Name) { p_CompleteName = Name; }
        public override void SymbolAddChild(Hime.Kernel.Symbol Symbol) { throw new Kernel.CannotAddChildException(this, Symbol); }
        public abstract System.Xml.XmlNode GetXMLNode(System.Xml.XmlDocument Doc);
    }

    public abstract class Terminal : Symbol
    {
        protected int p_Priority;

        public int Priority
        {
            get { return p_Priority; }
            set { p_Priority = value; }
        }

        public Terminal(Grammar Parent, ushort SID, string Name, int Priority) : base(Parent, SID, Name)
        {
            p_Priority = Priority;
        }

        public override System.Xml.XmlNode GetXMLNode(System.Xml.XmlDocument Doc)
        {
            System.Xml.XmlNode Node = Doc.CreateElement("SymbolTerminal");
            Node.Attributes.Append(Doc.CreateAttribute("SID"));
            Node.Attributes.Append(Doc.CreateAttribute("Name"));
            Node.Attributes.Append(Doc.CreateAttribute("Priority"));
            Node.Attributes["SID"].Value = p_SID.ToString();
            Node.Attributes["Name"].Value = p_LocalName.Replace("\"", "\\\"");
            Node.Attributes["Priority"].Value = p_Priority.ToString();
            return Node;
        }
    }

    public class TerminalText : Terminal
    {
        protected Automata.NFA p_NFA;
        protected Grammar p_SubGrammar;

        public Automata.NFA NFA
        {
            get { return p_NFA; }
            set { p_NFA = value; }
        }
        public Grammar SubGrammar
        {
            get { return p_SubGrammar; }
            set { p_SubGrammar = value; }
        }

        public TerminalText(Grammar Parent, ushort SID, string Name, int Priority, Automata.NFA NFA, Grammar SubGrammar) : base(Parent, SID, Name, Priority) { p_NFA = NFA; p_SubGrammar = SubGrammar; }

        public override System.Xml.XmlNode GetXMLNode(System.Xml.XmlDocument Doc)
        {
            System.Xml.XmlNode Node = Doc.CreateElement("SymbolTerminalText");
            Node.Attributes.Append(Doc.CreateAttribute("SID"));
            Node.Attributes.Append(Doc.CreateAttribute("Name"));
            Node.Attributes.Append(Doc.CreateAttribute("Priority"));
            Node.Attributes.Append(Doc.CreateAttribute("SubGrammar"));
            Node.Attributes["SID"].Value = p_SID.ToString("X");
            Node.Attributes["Name"].Value = p_LocalName.Replace("\"", "\\\"");
            Node.Attributes["Priority"].Value = p_Priority.ToString();
            if (p_SubGrammar != null)
                Node.Attributes["SubGrammar"].Value = p_SubGrammar.CompleteName.ToString('_');
            return Node;
        }

        public override string ToString()
        {
            string name = p_LocalName;
            if (name.StartsWith("_T["))
                name = name.Substring(3, name.Length - 4);
            return name;
        }
    }

    [System.FlagsAttribute]
    public enum TerminalBinType : byte
    {
        OTHER = 0x00,
        SYMBOL_VALUE_UINT128 = 0x10,
        SYMBOL_JOKER_UINT128 = 0x22,
        SYMBOL_VALUE_UINT64 = 0x30,
        SYMBOL_JOKER_UINT64 = 0x42,
        SYMBOL_VALUE_UINT32 = 0x50,
        SYMBOL_JOKER_UINT32 = 0x62,
        SYMBOL_VALUE_UINT16 = 0x70,
        SYMBOL_JOKER_UINT16 = 0x82,
        SYMBOL_VALUE_UINT8 = 0x90,
        SYMBOL_JOKER_UINT8 = 0xA2,
        SYMBOL_VALUE_BINARY = 0xB1,
        SYMBOL_JOKER_BINARY = 0xC3,
        FLAG_BINARY = 0x01,
        FLAG_JOKER = 0x02
    }

    public class TerminalBin : Terminal
    {
        protected TerminalBinType p_Type;
        protected string p_Value;

        public TerminalBinType Type
        {
            get { return p_Type; }
            set { p_Type = value; }
        }
        public string Value
        {
            get { return p_Value; }
            set { p_Value = value; }
        }

        public TerminalBin(Grammar Parent, ushort SID, string Name, int Priority, TerminalBinType Type, string Value)
            : base(Parent, SID, Name, Priority)
        {
            p_Type = Type;
            p_Value = Value;
        }
        public override System.Xml.XmlNode GetXMLNode(System.Xml.XmlDocument Doc)
        {
            System.Xml.XmlNode Node = Doc.CreateElement("SymbolTerminalBin");
            Node.Attributes.Append(Doc.CreateAttribute("SID"));
            Node.Attributes.Append(Doc.CreateAttribute("Name"));
            Node.Attributes.Append(Doc.CreateAttribute("Joker"));
            Node.Attributes.Append(Doc.CreateAttribute("LengthByte"));
            Node.Attributes.Append(Doc.CreateAttribute("LengthBit"));

            Node.Attributes["SID"].Value = p_SID.ToString("X");
            Node.Attributes["Name"].Value = p_LocalName;

            if ((p_Type & TerminalBinType.FLAG_BINARY) == TerminalBinType.FLAG_BINARY)
            {
                Node.Attributes["LengthByte"].Value = "0";
                Node.Attributes["LengthBit"].Value = p_Value.Length.ToString();
                if ((p_Type & TerminalBinType.FLAG_JOKER) == TerminalBinType.FLAG_JOKER)
                {
                    Node.Attributes["Joker"].Value = "True";
                }
                else
                {
                    uint Val = System.Convert.ToUInt32(p_Value, 2);
                    Node.Attributes["Joker"].Value = "False";
                    Node.Attributes.Append(Doc.CreateAttribute("Value"));
                    Node.Attributes["Value"].Value = "0x" + Val.ToString("X");
                }
            }
            else
            {
                Node.Attributes["LengthByte"].Value = ((int)(p_Value.Length / 2)).ToString();
                Node.Attributes["LengthBit"].Value = "0";
                if ((p_Type & TerminalBinType.FLAG_JOKER) == TerminalBinType.FLAG_JOKER)
                {
                    Node.Attributes["Joker"].Value = "True";
                }
                else
                {
                    Node.Attributes["Joker"].Value = "False";
                    Node.Attributes.Append(Doc.CreateAttribute("Value"));
                    Node.Attributes["Value"].Value = p_LocalName;
                }
            }
            return Node;
        }
    }
    
    public class TerminalEpsilon : Terminal
    {
        private static TerminalEpsilon p_Instance;
        private static readonly object p_Lock = new object();
        private TerminalEpsilon() : base(null, 1, "ε", 0) { }

        public static TerminalEpsilon Instance
        {
            get
            {
                lock (p_Lock)
                {
                    if (p_Instance == null)
                        p_Instance = new TerminalEpsilon();
                    return p_Instance;
                }
            }
        }

        public override string ToString() { return "ε"; }
    }
    
    public class TerminalDollar : Terminal
    {
        private static TerminalDollar p_Instance;
        private static readonly object p_Lock = new object();
        private TerminalDollar() : base(null, 2, "$", 0) { }

        public static TerminalDollar Instance
        {
            get
            {
                lock (p_Lock)
                {
                    if (p_Instance == null)
                        p_Instance = new TerminalDollar();
                    return p_Instance;
                }
            }
        }

        public override string ToString() { return "$"; }
    }
    
    public class TerminalDummy : Terminal
    {
        private static TerminalDummy p_Instance;
        private static readonly object p_Lock = new object();
        private TerminalDummy() : base(null, 0xFFFF, "#", -1) { }

        public static TerminalDummy Instance
        {
            get
            {
                lock (p_Lock)
                {
                    if (p_Instance == null)
                        p_Instance = new TerminalDummy();
                    return p_Instance;
                }
            }
        }

        public override string ToString() { return "#"; }
    }
    
    public class Virtual : Symbol
    {
        public Virtual(Grammar Parent, string Name) : base(Parent, 0, Name) { }

        public override System.Xml.XmlNode GetXMLNode(System.Xml.XmlDocument Doc)
        {
            System.Xml.XmlNode Node = Doc.CreateElement("SymbolVirtual");
            Node.Attributes.Append(Doc.CreateAttribute("Name"));
            Node.Attributes["Name"].Value = p_LocalName;
            return Node;
        }
    }
    
    public class Action : Symbol
    {
        protected Hime.Kernel.QualifiedName p_ActionName;
        public Hime.Kernel.QualifiedName ActionName { get { return p_ActionName; } }

        public Action(Grammar Parent, string Name, Hime.Kernel.QualifiedName Action)
            : base(Parent, 0, Name)
        {
            p_ActionName = Action;
        }

        public override System.Xml.XmlNode GetXMLNode(System.Xml.XmlDocument Doc)
        {
            System.Xml.XmlNode Node = Doc.CreateElement("SymbolAction");
            Node.Attributes.Append(Doc.CreateAttribute("Name"));
            Node.Attributes["Name"].Value = p_LocalName;
            Node.InnerText = p_ActionName.ToString();
            return Node;
        }
    }

    public abstract class Variable : Symbol
    {
        public Variable(Grammar Parent, ushort SID, string Name) : base(Parent, SID, Name) { }

        public override System.Xml.XmlNode GetXMLNode(System.Xml.XmlDocument Doc)
        {
            System.Xml.XmlNode Node = Doc.CreateElement("SymbolVariable");
            Node.Attributes.Append(Doc.CreateAttribute("SID"));
            Node.Attributes.Append(Doc.CreateAttribute("Name"));
            Node.Attributes["SID"].Value = p_SID.ToString("X");
            Node.Attributes["Name"].Value = p_LocalName;
            return Node;
        }
    }



    public class TerminalSet : System.Collections.Generic.List<Terminal>
    {
        public TerminalSet() : base() { }
        public TerminalSet(TerminalSet Copied) : base(Copied) { }

        public new bool Add(Terminal item)
        {
            if (Contains(item))
                return false;
            base.Add(item);
            return true;
        }

        public new bool AddRange(System.Collections.Generic.IEnumerable<Terminal> collection)
        {
            bool mod = false;
            foreach (Terminal item in collection)
            {
                if (!Contains(item))
                {
                    mod = true;
                    base.Add(item);
                }
            }
            return mod;
        }

        public override string ToString()
        {
            System.Text.StringBuilder Builder = new System.Text.StringBuilder("{");
            for (int i = 0; i != Count; i++)
            {
                if (i != 0)
                    Builder.Append(", ");
                Builder.Append(this[i].LocalName);
            }
            Builder.Append("}");
            return Builder.ToString();
        }
    }
}