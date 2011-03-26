using System.Collections.Generic;

namespace Hime.Kernel.Graphs
{
    public sealed class GMLSerializer
    {
        private string p_File;
        private System.Xml.XmlDocument p_XMLDoc;
        private System.Xml.XmlNode p_GraphRoot;

        public GMLSerializer(string name, string file)
        {
            p_File = file;
            p_XMLDoc = new System.Xml.XmlDocument();
            p_XMLDoc.AppendChild(p_XMLDoc.CreateXmlDeclaration("1.0", "UTF-8", null));
            System.Xml.XmlNode root = p_XMLDoc.CreateElement("graphml");
            p_XMLDoc.AppendChild(root);
            root.Attributes.Append(p_XMLDoc.CreateAttribute("xmlns"));
            root.Attributes[0].Value = "http://graphml.graphdrawing.org/xmlns";
            
            p_GraphRoot = p_XMLDoc.CreateElement("graph ");
            root.AppendChild(p_GraphRoot);
            p_GraphRoot.Attributes.Append(p_XMLDoc.CreateAttribute("id"));
            p_GraphRoot.Attributes.Append(p_XMLDoc.CreateAttribute("edgedefault"));
            p_GraphRoot.Attributes[0].Value = name;
            p_GraphRoot.Attributes[0].Value = "directed";
        }

        public void WriteNode(string name)
        {
            System.Xml.XmlNode node = p_XMLDoc.CreateElement("node");
            node.Attributes.Append(p_XMLDoc.CreateAttribute("id"));
            node.Attributes[0].Value = name;
            p_GraphRoot.AppendChild(node);
        }

        public void Close()
        {
            p_XMLDoc.Save(p_File);
        }
    }
}
