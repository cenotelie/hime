/*
 * Author: Charles Hymans
 * Date: 17/07/2011
 * Time: 20:28
 * 
 */
using System;

namespace Hime.Kernel.Reporting
{
    public class Reporter
    {
        protected Report report;
        protected Section topSection;
        protected Section currentSection;
        protected log4net.ILog log;

        public Report Result { get { return report; } }

        private static bool configured = false;
        private static void Configure()
        {
            if (configured)
                return;
            log4net.Layout.PatternLayout layout = new log4net.Layout.PatternLayout("%-5p: %m%n");
            log4net.Appender.ConsoleAppender appender = new log4net.Appender.ConsoleAppender();
            appender.Layout = layout;
            log4net.Config.BasicConfigurator.Configure(appender);
            configured = true;
        }

        public Reporter()
        {
            Configure();
            log = log4net.LogManager.GetLogger(typeof(Hime.Parsers.Compiler));
            report = new Report();
        }

        public void BeginSection(string name) { currentSection = report.AddSection(name); }
        public void EndSection() { currentSection = null; }

        public void Info(string component, string message)
        {
            AddEntry(new BaseEntry(ELevel.Info, component, message));
            log.Info(component + ": " + message);
        }
        public void Warn(string component, string message)
        {
            AddEntry(new BaseEntry(ELevel.Warning, component, message));
            log.Warn(component + ": " + message);
        }
        public void Error(string component, string message)
        {
            AddEntry(new BaseEntry(ELevel.Error, component, message));
            log.Error(component + ": " + message);
        }
        public void Fatal(string component, string message)
        {
            AddEntry(new BaseEntry(ELevel.Error, component, message));
            log.Fatal(component + ": " + message);
        }
        public void Report(IEntry entry)
        {
            AddEntry(entry);
            switch (entry.Level)
            {
                case ELevel.Info: log.Info(entry.Component + ": " + entry.Message); break;
                case ELevel.Warning: log.Warn(entry.Component + ": " + entry.Message); break;
                case ELevel.Error: log.Error(entry.Component + ": " + entry.Message); break;
            }
        }
        public void Report(System.Exception exception)
        {
            Report(new ExceptionEntry(exception));
        }

        protected void AddEntry(IEntry entry)
        {
            if (currentSection != null)
                currentSection.AddEntry(entry);
            else
            {
                if (topSection == null)
                {
                    topSection = new Section("global");
                    report.Sections.Insert(0, topSection);
                }
                topSection.AddEntry(entry);
            }
        }

        public void ExportMHTML(string fileName, string title)
        {
            System.Xml.XmlDocument Doc = report.GetXML(title);
            System.IO.FileInfo File = new System.IO.FileInfo(fileName);
            Doc.ChildNodes[1].Attributes["title"].Value = title;
            Doc.Save(fileName + ".xml");

            Resources.ResourceAccessor Session = new Resources.ResourceAccessor();
            Session.AddCheckoutFile(fileName + ".xml");
            Session.CheckOut("Transforms.Logs.xslt", File.DirectoryName + "Logs.xslt");

            System.Xml.Xsl.XslCompiledTransform Transform = new System.Xml.Xsl.XslCompiledTransform();
            Transform.Load(File.DirectoryName + "Logs.xslt");
            Transform.Transform(fileName + ".xml", fileName + ".html");
            Session.AddCheckoutFile(fileName + ".html");

            Kernel.Documentation.MHTMLCompiler compiler = new Kernel.Documentation.MHTMLCompiler();
            compiler.Title = title;
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceFileText("text/html", "utf-8", "Grammar.html", fileName + ".html"));
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceStreamText("text/css", "utf-8", "hime_data/Logs.css", Session.GetStreamFor("Transforms.Logs.css")));
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceStreamText("text/javascript", "utf-8", "hime_data/Hime.js", Session.GetStreamFor("Transforms.Hime.js")));

            compiler.AddSource(new Kernel.Documentation.MHTMLSourceStreamImage("image/gif", "hime_data/button_plus.gif", Session.GetStreamFor("Visuals.button_plus.gif")));
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceStreamImage("image/gif", "hime_data/button_minus.gif", Session.GetStreamFor("Visuals.button_minus.gif")));
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceStreamImage("image/png", "hime_data/Hime.Logo.png", Session.GetStreamFor("Visuals.Hime.Logo.png")));
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceStreamImage("image/png", "hime_data/Hime.Info.png", Session.GetStreamFor("Visuals.Hime.Info.png")));
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceStreamImage("image/png", "hime_data/Hime.Warning.png", Session.GetStreamFor("Visuals.Hime.Warning.png")));
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceStreamImage("image/png", "hime_data/Hime.Error.png", Session.GetStreamFor("Visuals.Hime.Error.png")));
            compiler.CompileTo(fileName);

            Session.Close();
        }
    }
}
