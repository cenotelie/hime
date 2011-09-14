/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;

namespace Hime.Parsers
{
    public sealed class RuleDefinitionPart
    {
        private Symbol symbol;
        private RuleDefinitionPartAction action;

        public Symbol Symbol { get { return symbol; } }
        public RuleDefinitionPartAction Action
        {
            get { return action; }
            set { action = value; }
        }

        public RuleDefinitionPart(Symbol symbol, RuleDefinitionPartAction action)
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
            RuleDefinitionPart part = obj as RuleDefinitionPart;
            if (this.symbol.SID != part.symbol.SID)
                return false;
            return (this.action == part.action);
        }
        public override string ToString()
        {
            string s = symbol.ToString();
            if (action == RuleDefinitionPartAction.Promote)
                return (s + "^");
            else if (action == RuleDefinitionPartAction.Drop)
                return (s + "!");
            else
                return s;
        }
    }
}