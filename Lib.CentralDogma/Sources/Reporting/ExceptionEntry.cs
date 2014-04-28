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
using System.Xml;

namespace Hime.CentralDogma.Reporting
{
	/// <summary>
	/// Represents an entry corresponding to an exception
	/// </summary>
	public sealed class ExceptionEntry : Entry
	{
		private Exception exception;

		/// <summary>
		/// Initializes the entry with an exception
		/// </summary>
		/// <param name="exception">The exception to report</param>
		public ExceptionEntry(Exception exception) : base(ELevel.Error, "Exception " + exception.Message)
		{
			this.exception = exception;
		}

		/// <summary>
		/// Buils the XML node corresponding to the entry
		/// </summary>
		/// <param name="doc">The parent XML document</param>
		/// <returns>The XML node</returns>
		public override XmlNode GetMessageNode(XmlDocument doc)
		{
			XmlNode element = doc.CreateElement("Exception");
			element.Attributes.Append(doc.CreateAttribute("EID"));
			element.Attributes["EID"].Value = exception.GetHashCode().ToString();
			XmlNode message = doc.CreateElement("Message");
			message.InnerText = Message;
			element.AppendChild(message);
			XmlNode method = doc.CreateElement("Method");
			method.InnerText = exception.TargetSite.ToString();
			element.AppendChild(method);
			XmlNode stack = doc.CreateElement("Stack");
			string data = exception.StackTrace;
			string[] lines = data.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
			foreach (string line in lines)
			{
				XmlNode nl = doc.CreateElement("Line");
				nl.InnerText = line;
				stack.AppendChild(nl);
			}
			element.AppendChild(stack);
			return element;
		}
	}
}