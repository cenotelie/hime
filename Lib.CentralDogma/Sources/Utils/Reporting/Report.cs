/*
 * Author: Charles Hymans
 * Date: 18/07/2011
 * Time: 19:06
 * 
 */
using System;
using System.Collections.Generic;
using System.Xml;

namespace Hime.Utils.Reporting
{
	/// <summary>
	/// Description of Report.
	/// </summary>
	public class Report
	{
		// TODO: maybe would be more convenient to have a dictionary from section name to sections
        public List<Section> Sections { get; private set; }

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
		
		// TODO: maybe the title could be passed to the Report constructor instead? think about it
        public XmlDocument ToXmlDocument(string title)
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
            result.ChildNodes[1].Attributes["title"].Value = title;
            return result;
        }
        
        public bool HasErrors
        {
        	get 
			{
	        	foreach (Section section in this.Sections)
	            {
	                foreach (Entry entry in section.Entries)
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
	                foreach (Entry entry in section.Entries)
	                {
	                    if (entry.Level == ELevel.Error) result++;
	                }
	            }
	            return result;
			}
		}
		
		public IEnumerable<Entry> Errors
		{
			get
			{
				foreach (Section section in this.Sections)
	            {
	                foreach (Entry entry in section.Entries)
	                {
	                    if (entry.Level == ELevel.Error) yield return entry;
	                }
	            }
			}
		}
	}
}
