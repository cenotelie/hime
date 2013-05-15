using System.Collections.Generic;

namespace Hime.CentralDogma.Grammars
{
    class RuleBodyElement
    {
        private Symbol symbol;
        private RuleBodyElementAction action;

        public Symbol Symbol { get { return symbol; } }
        public RuleBodyElementAction Action
        {
            get { return action; }
            set { action = value; }
        }

        public RuleBodyElement(Symbol symbol, RuleBodyElementAction action)
        {
            this.symbol = symbol;
            this.action = action;
        }

        public System.Xml.XmlNode GetXMLNode(System.Xml.XmlDocument document)
        {
            System.Xml.XmlNode node = symbol.GetXMLNode(document);
            node.Attributes.Append(document.CreateAttribute("Action"));
            node.Attributes.Append(document.CreateAttribute("ParserIndex"));
            node.Attributes["Action"].Value = action.ToString();
            return node;
        }

        public override int GetHashCode() { return base.GetHashCode(); }
        public override bool Equals(object obj)
        {
            RuleBodyElement part = obj as RuleBodyElement;
            if (this.symbol.SID != part.symbol.SID)
                return false;
            return (this.action == part.action);
        }
        public override string ToString()
        {
            string s = symbol.ToString();
            if (action == RuleBodyElementAction.Promote)
                return (s + "^");
            else if (action == RuleBodyElementAction.Drop)
                return (s + "!");
            else
                return s;
        }
    }
}