/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;

namespace Hime.Parsers
{
    public sealed class RuleBodyElement
    {
        private GrammarSymbol symbol;
        private RuleBodyElementAction action;

        public GrammarSymbol Symbol { get { return symbol; } }
        public RuleBodyElementAction Action
        {
            get { return action; }
            set { action = value; }
        }

        public RuleBodyElement(GrammarSymbol symbol, RuleBodyElementAction action)
        {
            this.symbol = symbol;
            this.action = action;
        }

        public System.Xml.XmlNode GetXMLNode(System.Xml.XmlDocument document)
        {
            System.Xml.XmlNode node = document.CreateElement("Symbol");
            node.Attributes.Append(document.CreateAttribute("Action"));
            node.Attributes.Append(document.CreateAttribute("SymbolType"));
            node.Attributes.Append(document.CreateAttribute("SymbolID"));
            node.Attributes.Append(document.CreateAttribute("SymbolName"));
            node.Attributes.Append(document.CreateAttribute("SymbolValue"));
            node.Attributes.Append(document.CreateAttribute("ParserIndex"));

            node.Attributes["Action"].Value = action.ToString();
            node.Attributes["SymbolID"].Value = symbol.SID.ToString("X");
            node.Attributes["SymbolName"].Value = symbol.LocalName;
            node.Attributes["SymbolValue"].Value = symbol.ToString();

            if (symbol is Terminal)
                node.Attributes["SymbolType"].Value = "Terminal";
            else if (symbol is Variable)
                node.Attributes["SymbolType"].Value = "Variable";
            else if (symbol is Virtual)
                node.Attributes["SymbolType"].Value = "Virtual";
            else if (symbol is Action)
                node.Attributes["SymbolType"].Value = "Action";
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