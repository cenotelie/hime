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
using System.Collections.Generic;
using System.Xml;

namespace Hime.CentralDogma.Reporting
{
	/// <summary>
	/// Represents a compilation report
	/// </summary>
	public sealed class Report
	{
		private List<Entry> infos;
		private List<Entry> warnings;
		private List<Entry> errors;
		private List<Entry> entries;
		private string title;

		/// <summary>
		/// Gets whether the report contains errors
		/// </summary>
		public bool HasErrors { get { return (errors.Count > 0); } }
		/// <summary>
		/// Gets the number of errors in the report
		/// </summary>
		public int ErrorCount { get { return errors.Count; } }
		/// <summary>
		/// Gets a list of the errors in the report
		/// </summary>
		public IEnumerable<Entry> Errors { get { return errors; } }
		/// <summary>
		/// Gets a list of all the entries in order
		/// </summary>
		public IEnumerable<Entry> Entries { get { return entries; } }
		/// <summary>
		/// Gets the report's title
		/// </summary>
		public string Title { get { return title; } }

		/// <summary>
		/// Initializes a new report
		/// </summary>
		public Report(string title)
		{
			this.infos = new List<Entry>();
			this.warnings = new List<Entry>();
			this.errors = new List<Entry>();
			this.entries = new List<Entry>();
			this.title = title;
		}

		/// <summary>
		/// Adds a new entry to the report
		/// </summary>
		/// <param name="entry">Entry to add</param>
		public void AddEntry(Entry entry)
		{
			this.entries.Add(entry);
			switch (entry.Level)
			{
				case ELevel.Info:
					infos.Add(entry);
					break;
				case ELevel.Warning:
					warnings.Add(entry);
					break;
				case ELevel.Error:
					errors.Add(entry);
					break;
			}
		}

		/// <summary>
		/// Export the report to an XML document
		/// </summary>
		/// <returns>The XML document</returns>
		public XmlDocument ExportXML()
		{
			XmlDocument result = new XmlDocument();
			result.AppendChild(result.CreateXmlDeclaration("1.0", "utf-8", "yes"));
			result.AppendChild(result.CreateElement("Log"));
			result.ChildNodes[1].Attributes.Append(result.CreateAttribute("title"));
			result.ChildNodes[1].Attributes["title"].Value = title;
			foreach (Entry entry in entries)
				result.ChildNodes[1].AppendChild(GetXMLNode_Entry(result, entry));
			return result;
		}

		private XmlNode GetXMLNode_Entry(XmlDocument doc, Entry entry)
		{
			XmlNode node = doc.CreateElement("Entry");
			node.Attributes.Append(doc.CreateAttribute("mark"));
			node.Attributes["mark"].Value = entry.Level.ToString();
			node.AppendChild(entry.GetMessageNode(doc));
			return node;
		}
	}
}
