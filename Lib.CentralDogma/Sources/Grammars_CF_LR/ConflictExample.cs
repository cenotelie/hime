using System.Collections.Generic;

namespace Hime.CentralDogma.Grammars.ContextFree.LR
{
    class ConflictExample
    {
        private List<Terminal> input;
        private Terminal lookahead;
        private List<Terminal> rest;

        public List<Terminal> Input { get { return input; } }
        public List<Terminal> Rest { get { return rest; } }

        public ConflictExample(Terminal l1)
        {
            input = new List<Terminal>();
            rest = new List<Terminal>();
            lookahead = l1;
        }

        public System.Xml.XmlNode GetXMLNode(System.Xml.XmlDocument doc)
        {
            System.Xml.XmlNode node = doc.CreateElement("Example");
            foreach (Terminal t in input)
                node.AppendChild(t.GetXMLNode(doc));
            node.AppendChild(doc.CreateElement("Dot"));
            node.AppendChild(lookahead.GetXMLNode(doc));
            foreach (Terminal t in rest)
                node.AppendChild(t.GetXMLNode(doc));
            return node;
        }
    }
}