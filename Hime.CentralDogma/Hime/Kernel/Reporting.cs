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
        protected Level p_Level;
        protected string p_Component;
        protected string p_Message;

        public Level Level { get { return p_Level; } }
        public string Component { get { return p_Component; } }
        public string Message { get { return p_Message; } }

        public BaseEntry(Level level, string component, string message)
        {
            p_Level = level;
            p_Component = component;
            p_Message = message;
        }
    }

    public class Section
    {
        protected System.Collections.Generic.List<Entry> p_Entries;
        protected string p_Name;

        public System.Collections.Generic.ICollection<Entry> Entries { get { return p_Entries; } }
        public string Name { get { return p_Name; } }

        public Section(string name)
        {
            p_Name = name;
            p_Entries = new System.Collections.Generic.List<Entry>();
        }

        public void AddEntry(Entry entry)
        {
            p_Entries.Add(entry);
        }


        public System.Xml.XmlNode GetXMLNode(System.Xml.XmlDocument Doc)
        {
            System.Xml.XmlNode node = Doc.CreateElement("Section");
            node.Attributes.Append(Doc.CreateAttribute("id"));
            node.Attributes.Append(Doc.CreateAttribute("name"));
            node.Attributes["id"].Value = "section" + GetHashCode().ToString("X");
            node.Attributes["name"].Value = Name;
            foreach (Entry entry in p_Entries)
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
        protected System.Collections.Generic.List<Section> p_Sections;
        public System.Collections.Generic.List<Section> Sections { get { return p_Sections; } }

        public Report()
        {
            p_Sections = new System.Collections.Generic.List<Section>();
        }

        public Section AddSection(string name)
        {
            Section section = new Section(name);
            p_Sections.Add(section);
            return section;
        }

        public System.Xml.XmlDocument GetXML(string title)
        {
            System.Xml.XmlDocument Doc = new System.Xml.XmlDocument();
            Doc.AppendChild(Doc.CreateXmlDeclaration("1.0", "utf-8", "yes"));
            Doc.AppendChild(Doc.CreateElement("Log"));
            Doc.ChildNodes[1].Attributes.Append(Doc.CreateAttribute("title"));
            Doc.ChildNodes[1].Attributes["title"].Value = title;
            foreach (Section section in p_Sections)
                Doc.ChildNodes[1].AppendChild(section.GetXMLNode(Doc));
            return Doc;
        }
    }



    public class Reporter
    {
        protected Report p_Report;
        protected Section p_TopSection;
        protected Section p_CurrentSection;
        protected log4net.ILog p_Log;

        public Report Result { get { return p_Report; } }

        private static bool p_Configured = false;
        private static void Configure()
        {
            if (p_Configured)
                return;
            log4net.Layout.PatternLayout layout = new log4net.Layout.PatternLayout("%-5p: %m%n");
            log4net.Appender.ConsoleAppender appender = new log4net.Appender.ConsoleAppender();
            appender.Layout = layout;
            log4net.Config.BasicConfigurator.Configure(appender);
            p_Configured = true;
        }

        public Reporter(System.Type type)
        {
            Configure();
            p_Log = log4net.LogManager.GetLogger(type);
            p_Report = new Report();
        }

        public void BeginSection(string name) { p_CurrentSection = p_Report.AddSection(name); }
        public void EndSection() { p_CurrentSection = null; }

        public void Info(string component, string message)
        {
            AddEntry(new BaseEntry(Level.Info, component, message));
            p_Log.Info(component + ": " + message);
        }
        public void Warn(string component, string message)
        {
            AddEntry(new BaseEntry(Level.Warning, component, message));
            p_Log.Warn(component + ": " + message);
        }
        public void Error(string component, string message)
        {
            AddEntry(new BaseEntry(Level.Error, component, message));
            p_Log.Error(component + ": " + message);
        }
        public void Fatal(string component, string message)
        {
            AddEntry(new BaseEntry(Level.Error, component, message));
            p_Log.Fatal(component + ": " + message);
        }
        public void Report(Entry entry)
        {
            AddEntry(entry);
            switch (entry.Level)
            {
                case Level.Info: p_Log.Info(entry.Component + ": " + entry.Message); break;
                case Level.Warning: p_Log.Warn(entry.Component + ": " + entry.Message); break;
                case Level.Error: p_Log.Error(entry.Component + ": " + entry.Message); break;
            }
        }

        protected void AddEntry(Entry entry)
        {
            if (p_CurrentSection != null)
                p_CurrentSection.AddEntry(entry);
            else
            {
                if (p_TopSection == null)
                {
                    p_TopSection = new Section("global");
                    p_Report.Sections.Insert(0, p_TopSection);
                }
                p_TopSection.AddEntry(entry);
            }
        }

        public void ExportHTML(string fileName, string title)
        {
            System.Xml.XmlDocument Doc = p_Report.GetXML(title);
            System.IO.FileInfo File = new System.IO.FileInfo(fileName);
            Doc.ChildNodes[1].Attributes["title"].Value = title;
            Doc.Save(fileName + ".xml");
            System.IO.Directory.CreateDirectory(File.DirectoryName + "\\hime_data");

            Resources.ResourceAccessor Session = new Resources.ResourceAccessor();
            Session.AddCheckoutFile(fileName + ".xml");
            Session.CheckOut("Transforms.Logs.LogXML.xslt", File.DirectoryName + "LogXML.xslt");
            Session.Export("Transforms.Hime.css", File.DirectoryName + "\\hime_data\\Hime.css");
            Session.Export("Transforms.Hime.js", File.DirectoryName + "\\hime_data\\Hime.js");
            Session.Export("Visuals.button_plus.gif", File.DirectoryName + "\\hime_data\\button_plus.gif");
            Session.Export("Visuals.button_minus.gif", File.DirectoryName + "\\hime_data\\button_minus.gif");
            Session.Export("Visuals.Hime.Error.png", File.DirectoryName + "\\hime_data\\Hime.Error.png");
            Session.Export("Visuals.Hime.Warning.png", File.DirectoryName + "\\hime_data\\Hime.Warning.png");
            Session.Export("Visuals.Hime.Info.png", File.DirectoryName + "\\hime_data\\Hime.Info.png");
            Session.Export("Visuals.Hime.Logo.png", File.DirectoryName + "\\hime_data\\Hime.Logo.png");

            System.Xml.Xsl.XslCompiledTransform Transform = new System.Xml.Xsl.XslCompiledTransform();
            Transform.Load(File.DirectoryName + "LogXML.xslt");
            Transform.Transform(fileName + ".xml", fileName);

            Session.Close();
        }
    }
}