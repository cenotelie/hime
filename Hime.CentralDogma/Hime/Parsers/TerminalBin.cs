namespace Hime.Parsers
{
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

        public TerminalBin(Grammar parent, ushort sid, string name, int priority, TerminalBinType type, string value)
            : base(parent, sid, name, priority)
        {
            this.type = type;
            this.value = value;
        }
        public override System.Xml.XmlNode GetXMLNode(System.Xml.XmlDocument document)
        {
            System.Xml.XmlNode node = document.CreateElement("SymbolTerminalBin");
            node.Attributes.Append(document.CreateAttribute("SID"));
            node.Attributes.Append(document.CreateAttribute("Name"));
            node.Attributes.Append(document.CreateAttribute("Joker"));
            node.Attributes.Append(document.CreateAttribute("LengthByte"));
            node.Attributes.Append(document.CreateAttribute("LengthBit"));

            node.Attributes["SID"].Value = SID.ToString("X");
            node.Attributes["Name"].Value = localName;

            if ((type & TerminalBinType.FLAG_BINARY) == TerminalBinType.FLAG_BINARY)
            {
                node.Attributes["LengthByte"].Value = "0";
                node.Attributes["LengthBit"].Value = value.Length.ToString();
                if ((type & TerminalBinType.FLAG_JOKER) == TerminalBinType.FLAG_JOKER)
                {
                    node.Attributes["Joker"].Value = "True";
                }
                else
                {
                    uint Val = System.Convert.ToUInt32(value, 2);
                    node.Attributes["Joker"].Value = "False";
                    node.Attributes.Append(document.CreateAttribute("Value"));
                    node.Attributes["Value"].Value = "0x" + Val.ToString("X");
                }
            }
            else
            {
                node.Attributes["LengthByte"].Value = ((int)(value.Length / 2)).ToString();
                node.Attributes["LengthBit"].Value = "0";
                if ((type & TerminalBinType.FLAG_JOKER) == TerminalBinType.FLAG_JOKER)
                {
                    node.Attributes["Joker"].Value = "True";
                }
                else
                {
                    node.Attributes["Joker"].Value = "False";
                    node.Attributes.Append(document.CreateAttribute("Value"));
                    node.Attributes["Value"].Value = localName;
                }
            }
            return node;
        }
    }
}