/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System;
using System.Collections.Generic;

namespace Hime.Kernel.Reporting
{
    public class Section
    {
        protected List<IEntry> entries;
        protected string name;

        public ICollection<IEntry> Entries { get { return entries; } }
        public string Name { get { return name; } }

        public Section(string name)
        {
            this.name = name;
            this.entries = new List<IEntry>();
        }

        public void AddEntry(IEntry entry)
        {
            this.entries.Add(entry);
        }


        public System.Xml.XmlNode GetXMLNode(System.Xml.XmlDocument doc)
        {
            System.Xml.XmlNode node = doc.CreateElement("Section");
            node.Attributes.Append(doc.CreateAttribute("id"));
            node.Attributes.Append(doc.CreateAttribute("name"));
            node.Attributes["id"].Value = "section" + GetHashCode().ToString("X");
            node.Attributes["name"].Value = Name;
            foreach (IEntry entry in entries)
                node.AppendChild(GetXMLNode_Entry(doc, entry));
            return node;
        }
        private System.Xml.XmlNode GetXMLNode_Entry(System.Xml.XmlDocument doc, IEntry entry)
        {
            System.Xml.XmlNode node = doc.CreateElement("Entry");
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
