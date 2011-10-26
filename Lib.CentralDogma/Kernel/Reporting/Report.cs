/*
 * Author: Charles Hymans
 * Date: 18/07/2011
 * Time: 19:06
 * 
 */
using System;
using System.Collections.Generic;
using System.Xml;

namespace Hime.Kernel.Reporting
{
	/// <summary>
	/// Description of Report.
	/// </summary>
	public class Report
	{
        public List<Section> Sections { get; set; }

        public Report()
        {
            this.Sections = new List<Section>();
        }

        public Section AddSection(string name)
        {
            Section section = new Section(name);
            this.Sections.Add(section);
            return section;
        }
		
		// TODO: rename into ToXmlDocument (Like ToString)
        public XmlDocument GetXML(string title)
        {
            XmlDocument result = new XmlDocument();
            result.AppendChild(result.CreateXmlDeclaration("1.0", "utf-8", "yes"));
            result.AppendChild(result.CreateElement("Log"));
            result.ChildNodes[1].Attributes.Append(result.CreateAttribute("title"));
            result.ChildNodes[1].Attributes["title"].Value = title;
            foreach (Section section in this.Sections)
			{
                result.ChildNodes[1].AppendChild(section.GetXMLNode(result));
			}
            return result;
        }
        
        public bool HasErrors
        {
        	get 
			{
	        	foreach (Section section in this.Sections)
	            {
	                foreach (IEntry entry in section.Entries)
	                {
	                    if (entry.Level == ELevel.Error) return true;
	                }
	            }
	            return false;
        	}
        }
		
		public int ErrorCount
		{
			get
			{
				int result = 0;
				foreach (Section section in this.Sections)
	            {
	                foreach (IEntry entry in section.Entries)
	                {
	                    if (entry.Level == ELevel.Error) result++;
	                }
	            }
	            return result;
			}
		}
	}
}
