using System.Collections.Generic;

namespace Hime.Parsers
{
    public abstract class Symbol : Hime.Kernel.Symbol
    {
        protected Grammar parent;
        protected ushort sID;
        protected string localName;
        protected Hime.Kernel.QualifiedName completeName;

        public ushort SID { get { return sID; } }
        public override Hime.Kernel.Symbol Parent { get { return parent; } }
        public override string LocalName { get { return localName; } }
        public override Hime.Kernel.QualifiedName CompleteName { get { return completeName; } }

        public Symbol(Grammar Parent, ushort SID, string Name) : base()
        {
            parent = Parent;
            sID = SID;
            localName = Name;
            if (Parent == null)
                completeName = new Hime.Kernel.QualifiedName(localName);
            else
                completeName = Parent.CompleteName + localName;
        }

        protected override void SymbolSetParent(Hime.Kernel.Symbol Symbol)
        {
            if (Symbol is Grammar)
                parent = (Grammar)Symbol;
            else
                throw new Kernel.WrongParentSymbolException(this, Symbol.GetType(), typeof(Grammar));
        }
        protected override void SymbolSetCompleteName(Hime.Kernel.QualifiedName Name) { completeName = Name; }
        public override void SymbolAddChild(Hime.Kernel.Symbol Symbol) { throw new Kernel.CannotAddChildException(this, Symbol); }
        public abstract System.Xml.XmlNode GetXMLNode(System.Xml.XmlDocument Doc);
    }

    public abstract class Terminal : Symbol
    {
        protected int priority;

        public int Priority
        {
            get { return priority; }
            set { priority = value; }
        }

        public Terminal(Grammar Parent, ushort SID, string Name, int Priority) : base(Parent, SID, Name)
        {
            priority = Priority;
        }

        public override System.Xml.XmlNode GetXMLNode(System.Xml.XmlDocument Doc)
        {
            System.Xml.XmlNode Node = Doc.CreateElement("SymbolTerminal");
            Node.Attributes.Append(Doc.CreateAttribute("SID"));
            Node.Attributes.Append(Doc.CreateAttribute("Name"));
            Node.Attributes.Append(Doc.CreateAttribute("Priority"));
            Node.Attributes.Append(Doc.CreateAttribute("Value"));
            Node.Attributes["SID"].Value = sID.ToString();
            Node.Attributes["Name"].Value = localName.Replace("\"", "\\\"");
            Node.Attributes["Priority"].Value = priority.ToString();
            Node.Attributes["Value"].Value = this.ToString();
            return Node;
        }
    }

    public sealed class TerminalText : Terminal
    {
        private Automata.NFA nFA;
        private Grammar subGrammar;

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

        public TerminalText(Grammar Parent, ushort SID, string Name, int Priority, Automata.NFA NFA, Grammar SubGrammar) : base(Parent, SID, Name, Priority) { nFA = NFA; subGrammar = SubGrammar; }

        public override System.Xml.XmlNode GetXMLNode(System.Xml.XmlDocument Doc)
        {
            System.Xml.XmlNode Node = Doc.CreateElement("SymbolTerminalText");
            Node.Attributes.Append(Doc.CreateAttribute("SID"));
            Node.Attributes.Append(Doc.CreateAttribute("Name"));
            Node.Attributes.Append(Doc.CreateAttribute("Priority"));
            Node.Attributes.Append(Doc.CreateAttribute("SubGrammar"));
            Node.Attributes.Append(Doc.CreateAttribute("Value"));
            Node.Attributes["SID"].Value = sID.ToString("X");
            Node.Attributes["Name"].Value = localName.Replace("\"", "\\\"");
            Node.Attributes["Priority"].Value = priority.ToString();
            if (subGrammar != null)
                Node.Attributes["SubGrammar"].Value = subGrammar.CompleteName.ToString('_');
            Node.Attributes["Value"].Value = this.ToString();
            return Node;
        }

        public override string ToString()
        {
            string name = localName;
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

            Node.Attributes["SID"].Value = sID.ToString("X");
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

    public abstract class Variable : Symbol
    {
        public Variable(Grammar Parent, ushort SID, string Name) : base(Parent, SID, Name) { }

        public override System.Xml.XmlNode GetXMLNode(System.Xml.XmlDocument Doc)
        {
            System.Xml.XmlNode Node = Doc.CreateElement("SymbolVariable");
            Node.Attributes.Append(Doc.CreateAttribute("SID"));
            Node.Attributes.Append(Doc.CreateAttribute("Name"));
            Node.Attributes["SID"].Value = sID.ToString("X");
            Node.Attributes["Name"].Value = localName;
            return Node;
        }
    }



    public sealed class TerminalSet : IList<Terminal>
    {
        private SortedList<ushort, Terminal> content;

        public TerminalSet()
        {
            content = new SortedList<ushort, Terminal>();
        }
        public TerminalSet(TerminalSet copied)
        {
            content = new SortedList<ushort, Terminal>(copied.content);
        }

        public bool Add(Terminal item)
        {
            if (content.ContainsKey(item.SID))
                return false;
            content.Add(item.SID, item);
            return true;
        }

        public bool AddRange(IEnumerable<Terminal> collection)
        {
            bool mod = false;
            foreach (Terminal item in collection)
            {
                if (!content.ContainsKey(item.SID))
                {
                    mod = true;
                    content.Add(item.SID, item);
                }
            }
            return mod;
        }

        public int IndexOf(Terminal item) { return content.IndexOfKey(item.SID); }
        
        public void Insert(int index, Terminal item) { throw new System.NotImplementedException(); }
        
        public void RemoveAt(int index) { content.RemoveAt(index); }

        public Terminal this[int index]
        {
            get { return content.Values[index]; }
            set { throw new System.NotImplementedException(); }
        }

        void ICollection<Terminal>.Add(Terminal item)
        {
            if (content.ContainsKey(item.SID))
                return;
            content.Add(item.SID, item);
        }

        public void Clear() { content.Clear(); }

        public bool Contains(Terminal item) { return content.ContainsKey(item.SID); }

        public void CopyTo(Terminal[] array, int arrayIndex) { throw new System.NotImplementedException(); }

        public int Count { get { return content.Count; } }

        public bool IsReadOnly {  get { return false; } }

        public bool Remove(Terminal item) { return content.Remove(item.SID); }

        public IEnumerator<Terminal> GetEnumerator() { return content.Values.GetEnumerator(); }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { return content.Values.GetEnumerator(); }

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
