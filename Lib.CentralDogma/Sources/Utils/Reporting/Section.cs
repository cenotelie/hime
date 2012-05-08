/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System;
using System.Collections.Generic;
using System.Xml;

namespace Hime.Utils.Reporting
{
    public sealed class Section
    {
        protected List<Entry> entries;
        protected string name;

        public ICollection<Entry> Entries { get { return entries; } }
        public string Name { get { return name; } }

        public Section(string name)
        {
            this.name = name;
            this.entries = new List<Entry>();
        }

        public void AddEntry(Entry entry)
        {
            this.entries.Add(entry);
        }


        public XmlNode GetXMLNode(XmlDocument doc)
        {
            XmlNode node = doc.CreateElement("Section");
            node.Attributes.Append(doc.CreateAttribute("id"));
            node.Attributes.Append(doc.CreateAttribute("name"));
            node.Attributes["id"].Value = "section" + GetHashCode().ToString("X");
            node.Attributes["name"].Value = Name;
            foreach (Entry entry in entries)
                node.AppendChild(GetXMLNode_Entry(doc, entry));
            return node;
        }
        private XmlNode GetXMLNode_Entry(XmlDocument doc, Entry entry)
        {
            XmlNode node = doc.CreateElement("Entry");
            node.Attributes.Append(doc.CreateAttribute("mark"));
            node.Attributes["mark"].Value = entry.Level.ToString();
            System.Xml.XmlNode nodeComponent = doc.CreateElement("Component");
            nodeComponent.InnerText = entry.Component;
            node.AppendChild(nodeComponent);
            node.AppendChild(entry.GetMessageNode(doc));
            return node;
        }
    }
}
