/*
 * Author: Charles Hymans
 * Date: 18/07/2011
 * Time: 19:06
 * 
 */
using System;
using System.Collections.Generic;
using System.Xml;

namespace Hime.CentralDogma.Reporting
{
	/// <summary>
	/// Description of Report.
	/// </summary>
	public sealed class Report
	{
		// TODO: maybe would be more convenient to have a dictionary from section name to sections
        /// <summary>
        /// Gets a list of the sections in this report
        /// </summary>
        public List<Section> Sections { get; private set; }

        /// <summary>
        /// Initializes a new report
        /// </summary>
        public Report()
        {
            this.Sections = new List<Section>();
        }

        /// <summary>
        /// Adds a new section to this report
        /// </summary>
        /// <param name="name">The new section's name</param>
        /// <returns>The new section</returns>
        public Section AddSection(string name)
        {
            Section section = new Section(name);
            this.Sections.Add(section);
            return section;
        }
		
		// TODO: maybe the title could be passed to the Report constructor instead? think about it
        /// <summary>
        /// Export the report to an XML document
        /// </summary>
        /// <param name="title">Title of the report</param>
        /// <returns>The XML document</returns>
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
        
        /// <summary>
        /// Gets whether the report contains errors
        /// </summary>
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
		
        /// <summary>
        /// Gets the number of errors in the report
        /// </summary>
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
		
        /// <summary>
        /// Gets a list of the errors in the report
        /// </summary>
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
