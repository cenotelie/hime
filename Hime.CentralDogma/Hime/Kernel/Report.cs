/*
 * Author: Charles Hymans
 * Date: 18/07/2011
 * Time: 19:06
 * 
 */
using System;
using System.Collections.Generic;

namespace Hime.Kernel.Reporting
{
	/// <summary>
	/// Description of Report.
	/// </summary>
	public class Report
	{
        protected List<Section> sections;
        public List<Section> Sections { get { return sections; } }

        public Report()
        {
            sections = new List<Section>();
        }

        public Section AddSection(string name)
        {
            Section section = new Section(name);
            sections.Add(section);
            return section;
        }

        public System.Xml.XmlDocument GetXML(string title)
        {
            System.Xml.XmlDocument Doc = new System.Xml.XmlDocument();
            Doc.AppendChild(Doc.CreateXmlDeclaration("1.0", "utf-8", "yes"));
            Doc.AppendChild(Doc.CreateElement("Log"));
            Doc.ChildNodes[1].Attributes.Append(Doc.CreateAttribute("title"));
            Doc.ChildNodes[1].Attributes["title"].Value = title;
            foreach (Section section in sections)
                Doc.ChildNodes[1].AppendChild(section.GetXMLNode(Doc));
            return Doc;
        }
        
        public bool HasErrors
        {
        	get {
	        	foreach (Section section in this.Sections)
	            {
	                foreach (Entry entry in section.Entries)
	                {
	                    if (entry.Level == Level.Error) return true;
	                }
	            }
	            return false;
        	}
        }
	}
}
