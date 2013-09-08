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

using System;
using System.Xml;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Hime.CentralDogma.Grammars
{
	class Variable : Symbol
    {
        protected List<Rule> rules;

        public IList<Rule> Rules { get { return rules; } }

        protected override string Type { get { return "Variable"; } }

        public Variable(ushort sid, string name) : base(sid, name) 
        {
            this.rules = new List<Rule>();
        }

        public Rule AddRule(Rule rule)
        {
            int index = rules.IndexOf(rule);
            if (index != -1)
                return rules[index];
            int ID = rules.Count;
            rule.ID = ID;
            rules.Add(rule);
            OnRuleAdd(rule);
            return rule;
        }
		
        protected virtual void OnRuleAdd(Rule rule) { }

        public XmlNode GetXMLNodeWithRules(XmlDocument document)
        {
            XmlNode node = base.GetXMLNode(document);
            foreach (Rule rule in this.Rules)
                node.AppendChild(rule.GetXMLNode(document));
            return node;
        }
    }
}
