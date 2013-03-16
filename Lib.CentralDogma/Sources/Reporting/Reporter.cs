/*
 * Author: Charles Hymans
 * Date: 17/07/2011
 * Time: 20:28
 * 
 */
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
        private Section topSection;
        private Section currentSection;
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
        /// <param name="type"></param>
        public Reporter(System.Type type)
        {
            Configure();
            log = log4net.LogManager.GetLogger(type);
            report = new Report();
        }

        /// <summary>
        /// Begins a new section in the log
        /// </summary>
        /// <param name="name">The new section's name</param>
        public void BeginSection(string name) { currentSection = report.AddSection(name); }
        /// <summary>
        /// Ends the current section in the log
        /// </summary>
        public void EndSection() { currentSection = null; }

        /// <summary>
        /// Adds a new info entry to the log
        /// </summary>
        /// <param name="component">The component's emiting the entry</param>
        /// <param name="message">The info message</param>
        public void Info(string component, string message)
        {
            AddEntry(new Entry(ELevel.Info, component, message));
            log.Info(component + ": " + message);
        }
        /// <summary>
        /// Adds a new warning entry in the log
        /// </summary>
        /// <param name="component">The component's emiting the entry</param>
        /// <param name="message">The info message</param>
        public void Warn(string component, string message)
        {
            AddEntry(new Entry(ELevel.Warning, component, message));
            log.Warn(component + ": " + message);
        }
        /// <summary>
        /// Adds a new error entry in the log
        /// </summary>
        /// <param name="component">The component's emiting the entry</param>
        /// <param name="message">The info message</param>
        public void Error(string component, string message)
        {
            AddEntry(new Entry(ELevel.Error, component, message));
            log.Error(component + ": " + message);
        }
        /// <summary>
        /// Adds a new fatal error in the log
        /// </summary>
        /// <param name="component">The component's emiting the entry</param>
        /// <param name="message">The info message</param>
        public void Fatal(string component, string message)
        {
            AddEntry(new Entry(ELevel.Error, component, message));
            log.Fatal(component + ": " + message);
        }
		
        /// <summary>
        /// Adds a new entry in the log
        /// </summary>
        /// <param name="entry">The entry to add</param>
        public void Report(Entry entry)
        {
            AddEntry(entry);
            switch (entry.Level)
            {
                case ELevel.Info: log.Info(entry.Component + ": " + entry.Message); break;
                case ELevel.Warning: log.Warn(entry.Component + ": " + entry.Message); break;
                case ELevel.Error: log.Error(entry.Component + ": " + entry.Message); break;
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

        private void AddEntry(Entry entry)
        {
			Section section = currentSection;
            if (section == null)
            {
				// TODO: shouldn't it be an invariant that it is never null?
                if (topSection == null)
                {
                    topSection = new Section("global");
                    report.Sections.Insert(0, topSection);
                }
                section = topSection;
            }
			section.AddEntry(entry);
        }

        /// <summary>
        /// Export the current log as an MHTML file
        /// </summary>
        /// <param name="fileName">Name of the file to export to</param>
        /// <param name="title">Title of the log</param>
        public void ExportMHTML(string fileName, string title)
        {
            XmlDocument document = report.ToXmlDocument(title);
            FileInfo File = new FileInfo(fileName);
			string xmlFileName = fileName + ".xml";
			string htmlFileName = fileName + ".html";
            document.Save(xmlFileName);

            using (ResourceAccessor session = new ResourceAccessor())
			{
	            session.AddCheckoutFile(xmlFileName);
    	        session.CheckOut("Transforms.Logs.xslt", File.DirectoryName + "Logs.xslt");

	            XslCompiledTransform Transform = new XslCompiledTransform();
    	        Transform.Load(File.DirectoryName + "Logs.xslt");
        	    Transform.Transform(xmlFileName, htmlFileName);
            	session.AddCheckoutFile(htmlFileName);

	            Documentation.MHTMLCompiler compiler = new Documentation.MHTMLCompiler(title);

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
