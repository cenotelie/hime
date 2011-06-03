using System.Collections.Generic;

namespace Hime.Kernel.Reporting
{
    public enum Level
    {
        Info,
        Warning,
        Error
    }

    public interface Entry
    {
        Level Level { get; }
        string Component { get; }
        string Message { get; }
    }

    public class BaseEntry : Entry
    {
        protected Level level;
        protected string component;
        protected string message;

        public Level Level { get { return level; } }
        public string Component { get { return component; } }
        public string Message { get { return message; } }

        public BaseEntry(Level level, string component, string message)
        {
            this.level = level;
            this.component = component;
            this.message = message;
        }
    }

    public class Section
    {
        protected List<Entry> entries;
        protected string name;

        public ICollection<Entry> Entries { get { return entries; } }
        public string Name { get { return name; } }

        public Section(string name)
        {
            this.name = name;
            this.entries = new List<Entry>();
        }

        public void AddEntry(Entry entry)
        {
            this.entries.Add(entry);
        }


        public System.Xml.XmlNode GetXMLNode(System.Xml.XmlDocument Doc)
        {
            System.Xml.XmlNode node = Doc.CreateElement("Section");
            node.Attributes.Append(Doc.CreateAttribute("id"));
            node.Attributes.Append(Doc.CreateAttribute("name"));
            node.Attributes["id"].Value = "section" + GetHashCode().ToString("X");
            node.Attributes["name"].Value = Name;
            foreach (Entry entry in entries)
                node.AppendChild(GetXMLNode_Entry(Doc, entry));
            return node;
        }
        private System.Xml.XmlNode GetXMLNode_Entry(System.Xml.XmlDocument Doc, Entry Entry)
        {
            System.Xml.XmlNode node = Doc.CreateElement("Entry");
            node.Attributes.Append(Doc.CreateAttribute("mark"));
            node.Attributes["mark"].Value = Entry.Level.ToString();

            System.Xml.XmlNode Node1 = Doc.CreateElement("Data");
            Node1.InnerText = Entry.Component;
            node.AppendChild(Node1);

            System.Xml.XmlNode Node2 = Doc.CreateElement("Data");
            Node2.InnerText = Entry.Message;
            node.AppendChild(Node2);
            return node;
        }
    }

    public class Report
    {
        protected List<Section> sections;
        public List<Section> Sections { get { return sections; } }

        public Report()
        {
            sections = new List<Section>();
        }

        public Section AddSection(string name)
        {
            Section section = new Section(name);
            sections.Add(section);
            return section;
        }

        public System.Xml.XmlDocument GetXML(string title)
        {
            System.Xml.XmlDocument Doc = new System.Xml.XmlDocument();
            Doc.AppendChild(Doc.CreateXmlDeclaration("1.0", "utf-8", "yes"));
            Doc.AppendChild(Doc.CreateElement("Log"));
            Doc.ChildNodes[1].Attributes.Append(Doc.CreateAttribute("title"));
            Doc.ChildNodes[1].Attributes["title"].Value = title;
            foreach (Section section in sections)
                Doc.ChildNodes[1].AppendChild(section.GetXMLNode(Doc));
            return Doc;
        }
    }



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

        public Reporter(System.Type type)
        {
            Configure();
            log = log4net.LogManager.GetLogger(type);
            report = new Report();
        }

        public void BeginSection(string name) { currentSection = report.AddSection(name); }
        public void EndSection() { currentSection = null; }

        public void Info(string component, string message)
        {
            AddEntry(new BaseEntry(Level.Info, component, message));
            log.Info(component + ": " + message);
        }
        public void Warn(string component, string message)
        {
            AddEntry(new BaseEntry(Level.Warning, component, message));
            log.Warn(component + ": " + message);
        }
        public void Error(string component, string message)
        {
            AddEntry(new BaseEntry(Level.Error, component, message));
            log.Error(component + ": " + message);
        }
        public void Fatal(string component, string message)
        {
            AddEntry(new BaseEntry(Level.Error, component, message));
            log.Fatal(component + ": " + message);
        }
        public void Report(Entry entry)
        {
            AddEntry(entry);
            switch (entry.Level)
            {
                case Level.Info: log.Info(entry.Component + ": " + entry.Message); break;
                case Level.Warning: log.Warn(entry.Component + ": " + entry.Message); break;
                case Level.Error: log.Error(entry.Component + ": " + entry.Message); break;
            }
        }

        protected void AddEntry(Entry entry)
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
            Session.CheckOut("Transforms.Logs.LogXML.xslt", File.DirectoryName + "LogXML.xslt");

            System.Xml.Xsl.XslCompiledTransform Transform = new System.Xml.Xsl.XslCompiledTransform();
            Transform.Load(File.DirectoryName + "LogXML.xslt");
            Transform.Transform(fileName + ".xml", fileName + ".html");
            Session.AddCheckoutFile(fileName + ".html");

            Kernel.Documentation.MHTMLCompiler compiler = new Kernel.Documentation.MHTMLCompiler();
            compiler.Title = title;
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceFileText("text/html", "utf-8", "Grammar.html", fileName + ".html"));
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceStreamText("text/css", "utf-8", "hime_data/Hime.css", Session.GetStreamFor("Transforms.Hime.css")));
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
