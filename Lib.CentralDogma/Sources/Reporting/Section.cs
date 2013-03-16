/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System;
using System.Collections.Generic;
using System.Xml;

namespace Hime.CentralDogma.Reporting
{
    /// <summary>
    /// Represents a group of entries in a report
    /// </summary>
    public sealed class Section
    {
        private List<Entry> entries;
        private string name;

        /// <summary>
        /// Gets the collection of entries in this section
        /// </summary>
        public ICollection<Entry> Entries { get { return entries; } }
        /// <summary>
        /// Gets the name of this section
        /// </summary>
        public string Name { get { return name; } }

        /// <summary>
        /// Initializes a new section with the given name
        /// </summary>
        /// <param name="name">Name of the section</param>
        public Section(string name)
        {
            this.name = name;
            this.entries = new List<Entry>();
        }

        /// <summary>
        /// Adds a new entry in this section
        /// </summary>
        /// <param name="entry">Entry to add</param>
        public void AddEntry(Entry entry)
        {
            this.entries.Add(entry);
        }

        /// <summary>
        /// Gets the XML serialization of this section
        /// </summary>
        /// <param name="doc">The parent XML document</param>
        /// <returns>The XML node corresponding to this section</returns>
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
            nodeComponent.InnerText = "CentralDogma";
            node.AppendChild(nodeComponent);
            node.AppendChild(entry.GetMessageNode(doc));
            return node;
        }
    }
}
