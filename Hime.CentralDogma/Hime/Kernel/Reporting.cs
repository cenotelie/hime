using System;
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

        System.Xml.XmlNode GetMessageNode(System.Xml.XmlDocument doc);
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

        public System.Xml.XmlNode GetMessageNode(System.Xml.XmlDocument doc)
        {
            System.Xml.XmlNode element = doc.CreateElement("Message");
            element.InnerText = message;
            return element;
        }
    }

    public class ExceptionEntry : Entry
    {
        protected System.Exception exception;

        public Level Level { get { return Level.Error; } }
        public string Component { get { return "Compiler"; } }
        public string Message { get { return "Exception: " + exception.Message; } }

        public ExceptionEntry(System.Exception ex) { exception = ex; }

        public System.Xml.XmlNode GetMessageNode(System.Xml.XmlDocument doc)
        {
            System.Xml.XmlNode element = doc.CreateElement("Exception");
            System.Xml.XmlNode message = doc.CreateElement("Message");
            message.InnerText = Message;
            element.AppendChild(message);
            System.Xml.XmlNode method = doc.CreateElement("Method");
            method.InnerText = exception.TargetSite.ToString();
            element.AppendChild(method);
            System.Xml.XmlNode stack = doc.CreateElement("Stack");
            string data = exception.StackTrace;
            string[] lines = data.Split(new string[] { "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines)
            {
                System.Xml.XmlNode nl = doc.CreateElement("Line");
                nl.InnerText = line;
                stack.AppendChild(nl);
            }
            element.AppendChild(stack);
            return element;
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


        public System.Xml.XmlNode GetXMLNode(System.Xml.XmlDocument doc)
        {
            System.Xml.XmlNode node = doc.CreateElement("Section");
            node.Attributes.Append(doc.CreateAttribute("id"));
            node.Attributes.Append(doc.CreateAttribute("name"));
            node.Attributes["id"].Value = "section" + GetHashCode().ToString("X");
            node.Attributes["name"].Value = Name;
            foreach (Entry entry in entries)
                node.AppendChild(GetXMLNode_Entry(doc, entry));
            return node;
        }
        private System.Xml.XmlNode GetXMLNode_Entry(System.Xml.XmlDocument doc, Entry entry)
        {
            System.Xml.XmlNode node = doc.CreateElement("Entry");
            node.Attributes.Append(doc.CreateAttribute("mark"));
            node.Attributes["mark"].Value = entry.Level.ToString();
            System.Xml.XmlNode nodeComponent = doc.CreateElement("Component");
            nodeComponent.InnerText = entry.Component;
            node.AppendChild(nodeComponent);
            node.AppendChild(entry.GetMessageNode(doc));
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
}
