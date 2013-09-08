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

using System.Xml;

namespace Hime.CentralDogma.Reporting
{
    /// <summary>
    /// Represents an entry in a report
    /// </summary>
    public class Entry
    {
		/// <summary>
        /// Gets the entry's level
        /// </summary>
        public virtual ELevel Level { get; set; }
        /// <summary>
        /// Gets the entry's message
        /// </summary>
        public virtual string Message { get; private set; }
		
        /// <summary>
        /// Initializes the entry
        /// </summary>
        /// <param name="level">The entry's level</param>
        /// <param name="message">The entry's message</param>
		public Entry(ELevel level, string message)
		{
            this.Level = level;
            this.Message = message;
        }

        /// <summary>
        /// Buils the XML node corresponding to the entry
        /// </summary>
        /// <param name="document">The parent XML document</param>
        /// <returns>The XML node</returns>
        public virtual XmlNode GetMessageNode(XmlDocument document)
        {
            XmlNode element = document.CreateElement("Message");
            element.InnerText = this.Message;
            return element;
        }
		
        /// <summary>
        /// Gets a string representation of this entry
        /// </summary>
        /// <returns>A string representation of this entry</returns>
		public override string ToString ()
		{
			return this.Level + ": " + this.Message;
		}
    }
}