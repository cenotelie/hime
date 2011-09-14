/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
namespace Hime.Parsers
{
    public sealed class TerminalText : Terminal
    {
        private Automata.NFA nfa;
        private Grammar subGrammar;
        private string value;

        public Automata.NFA NFA
        {
            get { return nfa; }
            set { nfa = value; }
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
            this.nfa = nfa;
            this.subGrammar = subGrammar;
            this.value = name;
            if (this.value.StartsWith("@\""))
                this.value = this.value.Substring(2, this.value.Length - 3);
        }

        public override System.Xml.XmlNode GetXMLNode(System.Xml.XmlDocument document)
        {
            System.Xml.XmlNode node = document.CreateElement("SymbolTerminalText");
            node.Attributes.Append(document.CreateAttribute("SID"));
            node.Attributes.Append(document.CreateAttribute("Name"));
            node.Attributes.Append(document.CreateAttribute("Priority"));
            node.Attributes.Append(document.CreateAttribute("SubGrammar"));
            node.Attributes.Append(document.CreateAttribute("Value"));
            node.Attributes["SID"].Value = SID.ToString("X");
            node.Attributes["Name"].Value = localName.Replace("\"", "\\\"");
            node.Attributes["Priority"].Value = priority.ToString();
            if (subGrammar != null)
                node.Attributes["SubGrammar"].Value = subGrammar.CompleteName.ToString('_');
            node.Attributes["Value"].Value = value;
            return node;
        }

        public override string ToString() { return value; }
    }
}