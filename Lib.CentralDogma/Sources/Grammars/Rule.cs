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
    class Rule
    {
        public const string arrow = "→";

        protected Variable head;
        protected RuleBody body;
        protected bool replaceOnProduction;
        protected int id;
        protected int watermark;

        public Variable Head { get { return head; } }
        public RuleBody Body { get { return body; } }
        public bool ReplaceOnProduction { get { return replaceOnProduction; } }
        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        public int Watermark { get { return watermark; } }


        public Rule(Variable head, RuleBody body, bool replaceOnProduction)
        {
            this.head = head;
            this.body = body;
            this.replaceOnProduction = replaceOnProduction;
        }
        public Rule(Variable head, RuleBody body, bool replaceOnProduction, int watermark)
        {
            this.head = head;
            this.body = body;
            this.replaceOnProduction = replaceOnProduction;
            this.watermark = watermark;
        }

        public System.Xml.XmlNode GetXMLNode(System.Xml.XmlDocument document)
        {
            System.Xml.XmlNode node = document.CreateElement("Rule");
            node.Attributes.Append(document.CreateAttribute("HeadName"));
            node.Attributes.Append(document.CreateAttribute("HeadSID"));
            node.Attributes.Append(document.CreateAttribute("RuleID"));
            node.Attributes.Append(document.CreateAttribute("Replace"));
            node.Attributes["HeadName"].Value = head.Name;
            node.Attributes["HeadSID"].Value = head.SID.ToString("X");
            node.Attributes["RuleID"].Value = id.ToString("X");
            node.Attributes["Replace"].Value = replaceOnProduction.ToString();
            node.AppendChild(body.GetXMLNode(document));
            return node;
        }

        public override int GetHashCode() { return base.GetHashCode(); }
        public override bool Equals(object obj)
        {
            Rule rule = obj as Rule;
            if (this.head.SID != rule.head.SID)
                return false;
            if (this.replaceOnProduction != rule.replaceOnProduction)
                return false;
            return this.body.Equals(rule.body);
        }
        public override string ToString()
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            builder.Append(head.Name);
            builder.Append(" ");
            builder.Append(arrow);
            builder.Append(body.ToString());
            return builder.ToString();
        }
    }
}
