/**********************************************************************
* Copyright (c) 2013 Laurent Wouters and others
* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU Lesser General Public License as
* published by the Free Software Foundation, either version 3
* of the License, or (at your option) any later version.
* 
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU Lesser General Public License for more details.
* 
* You should have received a copy of the GNU Lesser General
* Public License along with this program.
* If not, see <http://www.gnu.org/licenses/>.
* 
* Contributors:
*     Laurent Wouters - lwouters@xowl.org
**********************************************************************/

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