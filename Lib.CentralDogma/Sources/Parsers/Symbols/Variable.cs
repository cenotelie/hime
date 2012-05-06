/*
 * Author: Charles Hymans
 * Date: 07/08/2011
 * Time: 15:05
 * 
 */
using System;
using System.Xml;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Hime.Parsers
{
	class Variable : GrammarSymbol
    {
        protected List<Rule> rules;
        protected ReadOnlyCollection<Rule> roRules;

        public IList<Rule> Rules { get { return roRules; } }

        public Variable(ushort sid, string name) : base(sid, name) 
        {
            this.rules = new List<Rule>();
            this.roRules = new ReadOnlyCollection<Rule>(this.rules);
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
