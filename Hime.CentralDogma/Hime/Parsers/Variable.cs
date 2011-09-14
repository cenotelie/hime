/*
 * Author: Charles Hymans
 * Date: 07/08/2011
 * Time: 15:05
 * 
 */
using System;
using System.Xml;
using System.Collections.Generic;
using Hime.Parsers.ContextFree;

namespace Hime.Parsers
{
	public class Variable : Symbol
    {
        internal protected List<CFRule> Rules { get; private set; }
        internal protected TerminalSet Firsts { get; private set; }
        internal protected TerminalSet Followers { get; private set; }

        
        public Variable(Grammar parent, ushort sid, string name) : base(parent, sid, name) 
        { 
            this.Rules = new List<CFRule>();
            this.Firsts = new TerminalSet();
            this.Followers = new TerminalSet();
        }

        public bool AddRule(CFRule rule)
        {
            if (this.Rules.Contains(rule))
                return false;
            int ID = this.Rules.Count;
            rule.ID = ID;
            this.Rules.Add(rule);
            return true;
        }

        public bool ComputeFirsts()
        {
            bool mod = false;
            foreach (CFRule rule in this.Rules)
            {
                TerminalSet rulefirsts = rule.Definition.Firsts;
                if (rulefirsts != null)
                    if (this.Firsts.AddRange(rulefirsts))
                        mod = true;
                if (rule.Definition.ComputeFirsts())
                    mod = true;
            }
            return mod;
        }

        public void ComputeFollowers_Step1()
        {
            foreach (CFRule rule in this.Rules)
                rule.Definition.ComputeFollowers_Step1();
        }

        public bool ComputeFollowers_Step23()
        {
            bool mod = false;
            foreach (CFRule rule in this.Rules)
                if (rule.Definition.ComputeFollowers_Step23(this))
                    mod = true;
            return mod;
        }

        public override XmlNode GetXMLNode(XmlDocument document)
        {
            XmlNode node = document.CreateElement("SymbolVariable");
            node.Attributes.Append(document.CreateAttribute("SID"));
            node.Attributes.Append(document.CreateAttribute("Name"));
            node.Attributes["SID"].Value = SID.ToString("X");
            node.Attributes["Name"].Value = localName;
            foreach (CFRule rule in this.Rules)
            {
                node.AppendChild(rule.GetXMLNode(document));
            }
            return node;
        }
    }
}
