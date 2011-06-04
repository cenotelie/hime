using System.Collections.Generic;

namespace Hime.Kernel.Documentation
{
    public class DocHeader
    {
        private string title;
        private MHTMLSource icon;

        public string Title { get { return title; } }
        public MHTMLSource Icon { get { return icon; } }

        public DocHeader(string title, MHTMLSource icon)
        {
            this.title = title;
            this.icon = icon;
        }

        public System.Xml.XmlNode GetXMLNode(System.Xml.XmlDocument doc)
        {
            System.Xml.XmlNode node = doc.CreateElement("Header");
            node.Attributes.Append(doc.CreateAttribute("title"));
            node.Attributes.Append(doc.CreateAttribute("icon"));
            node.Attributes["title"].Value = title;
            node.Attributes["icon"].Value = icon.ContentLocation;
            return node;
        }
    }

    public class DocMenuItem
    {
        private string name;
        private MHTMLSource page;
        private MHTMLSource icon;

        public string Name { get { return name; } }
        public MHTMLSource Page { get { return page; } }
        public MHTMLSource Icon { get { return icon; } }

        public DocMenuItem(string name, MHTMLSource page, MHTMLSource icon)
        {
            this.name = name;
            this.page = page;
            this.icon = icon;
        }

        public System.Xml.XmlNode GetXMLNode(System.Xml.XmlDocument doc)
        {
            System.Xml.XmlNode node = doc.CreateElement("MenuItem");
            node.Attributes.Append(doc.CreateAttribute("name"));
            node.Attributes.Append(doc.CreateAttribute("page"));
            node.Attributes.Append(doc.CreateAttribute("icon"));
            node.Attributes["name"].Value = name;
            node.Attributes["page"].Value = page.ContentLocation;
            node.Attributes["icon"].Value = icon.ContentLocation;
            return node;
        }
    }

    public class DocMenu
    {
        private List<DocMenuItem> items;

        public ICollection<DocMenuItem> Items { get { return items; } }

        public DocMenu() { items = new List<DocMenuItem>(); }

        public System.Xml.XmlNode GetXMLNode(System.Xml.XmlDocument doc)
        {
            System.Xml.XmlNode node = doc.CreateElement("Menu");
            foreach (DocMenuItem item in items)
                node.AppendChild(item.GetXMLNode(doc));
            return node;
        }
    }

    public class Documentation
    {
        private DocHeader header;
        private DocMenu menu;
        private List<MHTMLSource> pages;
        private List<MHTMLSource> resources;

        public Documentation(string title, MHTMLSource icon)
        {
            header = new DocHeader(title, icon);
            menu = new DocMenu();
            pages = new List<MHTMLSource>();
            resources = new List<MHTMLSource>();
            resources.Add(icon);
        }

        public void AddResource(MHTMLSource source)
        {
            if (!resources.Contains(source))
                resources.Add(source);
        }

        public void AddPage(string name, MHTMLSource source, MHTMLSource icon)
        {
            DocMenuItem item = new DocMenuItem(name, source, icon);
            menu.Items.Add(item);
            if (!pages.Contains(source))
                pages.Add(source);
            if (!resources.Contains(icon))
                resources.Add(icon);
        }

        public void CompileTo(string file)
        {
            
        }
    }
}