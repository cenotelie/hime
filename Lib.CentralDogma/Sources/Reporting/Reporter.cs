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
using log4net;
using System.Xml;
using System.Xml.Xsl;
using System.IO;

namespace Hime.CentralDogma.Reporting
{
    /// <summary>
    /// Represents a logger for the CentralDogma assembly
    /// </summary>
    public sealed class Reporter
    {
        private Report report;
        private ILog log;

        /// <summary>
        /// Gets the current log
        /// </summary>
        public Report Result { get { return report; } }

        private static bool configured = false;
        private static void Configure()
        {
            if (configured) return;
            log4net.Layout.PatternLayout layout = new log4net.Layout.PatternLayout("%-5p: %m%n");
            log4net.Appender.ConsoleAppender appender = new log4net.Appender.ConsoleAppender();
            appender.Layout = layout;
            log4net.Config.BasicConfigurator.Configure(appender);
            configured = true;
        }

        /// <summary>
        /// Initializes the reporter for the given type
        /// </summary>
        /// <param name="type">The reporting component's type</param>
        /// <param name="title">The report's title</param>
        public Reporter(System.Type type, string title)
        {
            Configure();
            log = log4net.LogManager.GetLogger(type);
            report = new Report(title);
        }

        /// <summary>
        /// Adds a new info entry to the log
        /// </summary>
        /// <param name="message">The info message</param>
        public void Info(string message)
        {
            report.AddEntry(new Entry(ELevel.Info, message));
            log.Info(message);
        }
        /// <summary>
        /// Adds a new warning entry in the log
        /// </summary>
        /// <param name="message">The info message</param>
        public void Warn(string message)
        {
            report.AddEntry(new Entry(ELevel.Warning, message));
            log.Warn(message);
        }
        /// <summary>
        /// Adds a new error entry in the log
        /// </summary>
        /// <param name="message">The info message</param>
        public void Error(string message)
        {
            report.AddEntry(new Entry(ELevel.Error, message));
            log.Error(message);
        }
		
        /// <summary>
        /// Adds a new entry in the log
        /// </summary>
        /// <param name="entry">The entry to add</param>
        public void Report(Entry entry)
        {
            report.AddEntry(entry);
            switch (entry.Level)
            {
                case ELevel.Info: log.Info(entry.Message); break;
                case ELevel.Warning: log.Warn(entry.Message); break;
                case ELevel.Error: log.Error(entry.Message); break;
            }
        }
		
        /// <summary>
        /// Adds a new entry reporting an exception in the log
        /// </summary>
        /// <param name="exception">The exception to report</param>
        public void Report(Exception exception)
        {
            Report(new ExceptionEntry(exception));
        }

        /// <summary>
        /// Export the current log as an MHTML file
        /// </summary>
        /// <param name="fileName">Name of the file to export to</param>
        public void ExportMHTML(string fileName)
        {
            XmlDocument document = report.ExportXML();
            FileInfo fileInfo = new FileInfo(fileName);
			string xmlFileName = fileName + ".xml";
			string htmlFileName = fileName + ".html";
            document.Save(xmlFileName);

            using (ResourceAccessor session = new ResourceAccessor())
			{
	            session.AddCheckoutFile(xmlFileName);
    	        session.CheckOut("Transforms.Logs.xslt", fileInfo.DirectoryName + "Logs.xslt");

	            XslCompiledTransform transform = new XslCompiledTransform();
    	        transform.Load(fileInfo.DirectoryName + "Logs.xslt");
        	    transform.Transform(xmlFileName, htmlFileName);
            	session.AddCheckoutFile(htmlFileName);

	            Documentation.MHTMLCompiler compiler = new Documentation.MHTMLCompiler(report.Title);

                compiler.AddSource(new Documentation.MHTMLSource("text/html", "Grammar.html", htmlFileName));
                compiler.AddSource(new Documentation.MHTMLSource("text/css", "hime_data/Hime.css", session.GetStreamFor("Transforms.Hime.css")));
                compiler.AddSource(new Documentation.MHTMLSource("text/javascript", "hime_data/Hime.js", session.GetStreamFor("Transforms.Hime.js")));

                compiler.AddSource(new Documentation.MHTMLSource("image/gif", "hime_data/button_plus.gif", session.GetStreamFor("Visuals.button_plus.gif")));
                compiler.AddSource(new Documentation.MHTMLSource("image/gif", "hime_data/button_minus.gif", session.GetStreamFor("Visuals.button_minus.gif")));
                compiler.AddSource(new Documentation.MHTMLSource("image/png", "hime_data/Hime.Logo.png", session.GetStreamFor("Visuals.Hime.Logo.png")));
                compiler.AddSource(new Documentation.MHTMLSource("image/png", "hime_data/Hime.Info.png", session.GetStreamFor("Visuals.Hime.Info.png")));
                compiler.AddSource(new Documentation.MHTMLSource("image/png", "hime_data/Hime.Warning.png", session.GetStreamFor("Visuals.Hime.Warning.png")));
                compiler.AddSource(new Documentation.MHTMLSource("image/png", "hime_data/Hime.Error.png", session.GetStreamFor("Visuals.Hime.Error.png")));
            	compiler.CompileTo(fileName);
			}
        }
    }
}
