/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
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