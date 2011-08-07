using System.Collections.Generic;

namespace Hime.Parsers
{
    public abstract class Symbol : Hime.Kernel.Symbol
    {
        protected string localName;
        protected Hime.Kernel.QualifiedName completeName;

        internal protected ushort SID { get; private set; }
        public override string LocalName { get { return localName; } }
        public override Hime.Kernel.QualifiedName CompleteName { get { return completeName; } }

        public Symbol(Grammar parent, ushort SID, string Name) : base()
        {
            this.Parent = parent;
            this.SID = SID;
            localName = Name;
            if (Parent == null)
                completeName = new Hime.Kernel.QualifiedName(localName);
            else
                completeName = Parent.CompleteName + localName;
        }

        protected override void SymbolSetParent(Hime.Kernel.Symbol Symbol)
        {
            if (Symbol is Grammar)
                this.Parent = (Grammar)Symbol;
            else
                throw new Kernel.WrongParentSymbolException(this, Symbol.GetType(), typeof(Grammar));
        }
        protected override void SymbolSetCompleteName(Hime.Kernel.QualifiedName Name) { completeName = Name; }
        public override void SymbolAddChild(Hime.Kernel.Symbol Symbol) { throw new Kernel.CannotAddChildException(this, Symbol); }
        public abstract System.Xml.XmlNode GetXMLNode(System.Xml.XmlDocument Doc);

        internal class Comparer : IEqualityComparer<Symbol>
        {
            public bool Equals(Symbol x, Symbol y) { return (x.SID == y.SID); }
            public int GetHashCode(Symbol obj) { return obj.SID; }
            private Comparer() { }
            private static Comparer instance = new Comparer();
            public static Comparer Instance { get { return instance; } }
        }
    }

    public sealed class TerminalText : Terminal
    {
        private Automata.NFA nFA;
        private Grammar subGrammar;
        private string value;

        public Automata.NFA NFA
        {
            get { return nFA; }
            set { nFA = value; }
        }
        public Grammar SubGrammar
        {
            get { return subGrammar; }
            set { subGrammar = value; }
        }
        public string Value { get { return value; } }

        public TerminalText(Grammar parent, ushort sid, string name, int priority, Automata.NFA nfa, Grammar subGrammar)
            : base(parent, sid, name, priority)
        {
            this.nFA = nfa;
            this.subGrammar = subGrammar;
            this.value = name;
            if (this.value.StartsWith("@\""))
                this.value = this.value.Substring(2, this.value.Length - 3);
        }

        public override System.Xml.XmlNode GetXMLNode(System.Xml.XmlDocument Doc)
        {
            System.Xml.XmlNode Node = Doc.CreateElement("SymbolTerminalText");
            Node.Attributes.Append(Doc.CreateAttribute("SID"));
            Node.Attributes.Append(Doc.CreateAttribute("Name"));
            Node.Attributes.Append(Doc.CreateAttribute("Priority"));
            Node.Attributes.Append(Doc.CreateAttribute("SubGrammar"));
            Node.Attributes.Append(Doc.CreateAttribute("Value"));
            Node.Attributes["SID"].Value = SID.ToString("X");
            Node.Attributes["Name"].Value = localName.Replace("\"", "\\\"");
            Node.Attributes["Priority"].Value = priority.ToString();
            if (subGrammar != null)
                Node.Attributes["SubGrammar"].Value = subGrammar.CompleteName.ToString('_');
            Node.Attributes["Value"].Value = value;
            return Node;
        }

        public override string ToString() { return value; }
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

    public sealed class TerminalBin : Terminal
    {
        private TerminalBinType type;
        private string value;

        public TerminalBinType Type
        {
            get { return type; }
            set { type = value; }
        }
        public string Value
        {
            get { return value; }
            set { this.value = value; }
        }

        public TerminalBin(Grammar Parent, ushort SID, string Name, int Priority, TerminalBinType Type, string Value)
            : base(Parent, SID, Name, Priority)
        {
            type = Type;
            value = Value;
        }
        public override System.Xml.XmlNode GetXMLNode(System.Xml.XmlDocument Doc)
        {
            System.Xml.XmlNode Node = Doc.CreateElement("SymbolTerminalBin");
            Node.Attributes.Append(Doc.CreateAttribute("SID"));
            Node.Attributes.Append(Doc.CreateAttribute("Name"));
            Node.Attributes.Append(Doc.CreateAttribute("Joker"));
            Node.Attributes.Append(Doc.CreateAttribute("LengthByte"));
            Node.Attributes.Append(Doc.CreateAttribute("LengthBit"));

            Node.Attributes["SID"].Value = SID.ToString("X");
            Node.Attributes["Name"].Value = localName;

            if ((type & TerminalBinType.FLAG_BINARY) == TerminalBinType.FLAG_BINARY)
            {
                Node.Attributes["LengthByte"].Value = "0";
                Node.Attributes["LengthBit"].Value = value.Length.ToString();
                if ((type & TerminalBinType.FLAG_JOKER) == TerminalBinType.FLAG_JOKER)
                {
                    Node.Attributes["Joker"].Value = "True";
                }
                else
                {
                    uint Val = System.Convert.ToUInt32(value, 2);
                    Node.Attributes["Joker"].Value = "False";
                    Node.Attributes.Append(Doc.CreateAttribute("Value"));
                    Node.Attributes["Value"].Value = "0x" + Val.ToString("X");
                }
            }
            else
            {
                Node.Attributes["LengthByte"].Value = ((int)(value.Length / 2)).ToString();
                Node.Attributes["LengthBit"].Value = "0";
                if ((type & TerminalBinType.FLAG_JOKER) == TerminalBinType.FLAG_JOKER)
                {
                    Node.Attributes["Joker"].Value = "True";
                }
                else
                {
                    Node.Attributes["Joker"].Value = "False";
                    Node.Attributes.Append(Doc.CreateAttribute("Value"));
                    Node.Attributes["Value"].Value = localName;
                }
            }
            return Node;
        }
    }

    public sealed class TerminalEpsilon : Terminal
    {
        private static TerminalEpsilon instance;
        private static readonly object _lock = new object();
        private TerminalEpsilon() : base(null, 1, "ε", 0) { }

        public static TerminalEpsilon Instance
        {
            get
            {
                lock (_lock)
                {
                    if (instance == null)
                        instance = new TerminalEpsilon();
                    return instance;
                }
            }
        }

        public override string ToString() { return "ε"; }
    }

    public sealed class TerminalDollar : Terminal
    {
        private static TerminalDollar instance;
        private static readonly object _lock = new object();
        private TerminalDollar() : base(null, 2, "$", 0) { }

        public static TerminalDollar Instance
        {
            get
            {
                lock (_lock)
                {
                    if (instance == null)
                        instance = new TerminalDollar();
                    return instance;
                }
            }
        }

        public override string ToString() { return "$"; }
    }

    public sealed class TerminalDummy : Terminal
    {
        private static TerminalDummy instance;
        private static readonly object _lock = new object();
        private TerminalDummy() : base(null, 0xFFFF, "#", -1) { }

        public static TerminalDummy Instance
        {
            get
            {
                lock (_lock)
                {
                    if (instance == null)
                        instance = new TerminalDummy();
                    return instance;
                }
            }
        }

        public override string ToString() { return "#"; }
    }

    public sealed class Virtual : Symbol
    {
        public Virtual(Grammar Parent, string Name) : base(Parent, 0, Name) { }

        public override System.Xml.XmlNode GetXMLNode(System.Xml.XmlDocument Doc)
        {
            System.Xml.XmlNode Node = Doc.CreateElement("SymbolVirtual");
            Node.Attributes.Append(Doc.CreateAttribute("Name"));
            Node.Attributes["Name"].Value = localName;
            return Node;
        }

        public override string ToString() { return "\"" + localName + "\""; }
    }

    public sealed class Action : Symbol
    {
        public Action(Grammar Parent, string Name) : base(Parent, 0, Name) { }

        public override System.Xml.XmlNode GetXMLNode(System.Xml.XmlDocument Doc)
        {
            System.Xml.XmlNode Node = Doc.CreateElement("SymbolAction");
            Node.Attributes.Append(Doc.CreateAttribute("Name"));
            Node.Attributes["Name"].Value = localName;
            return Node;
        }

        public override string ToString() { return "{" + localName + "}"; }
    }
}
