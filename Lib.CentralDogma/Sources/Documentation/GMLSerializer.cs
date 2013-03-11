/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;

namespace Hime.CentralDogma.Documentation
{
    class GMLSerializer
    {
        private string file;
        private System.Xml.XmlDocument xMLDoc;
        private System.Xml.XmlNode graphRoot;

        public GMLSerializer(string name, string file)
        {
            this.file = file;
            this.xMLDoc = new System.Xml.XmlDocument();
            xMLDoc.AppendChild(xMLDoc.CreateXmlDeclaration("1.0", "UTF-8", null));
            System.Xml.XmlNode root = xMLDoc.CreateElement("graphml");
            xMLDoc.AppendChild(root);
            root.Attributes.Append(xMLDoc.CreateAttribute("xmlns"));
            root.Attributes[0].Value = "http://graphml.graphdrawing.org/xmlns";
            
            graphRoot = xMLDoc.CreateElement("graph ");
            root.AppendChild(graphRoot);
            graphRoot.Attributes.Append(xMLDoc.CreateAttribute("id"));
            graphRoot.Attributes.Append(xMLDoc.CreateAttribute("edgedefault"));
            graphRoot.Attributes[0].Value = name;
            graphRoot.Attributes[0].Value = "directed";
        }

        public void WriteNode(string name)
        {
            System.Xml.XmlNode node = xMLDoc.CreateElement("node");
            node.Attributes.Append(xMLDoc.CreateAttribute("id"));
            node.Attributes[0].Value = name;
            graphRoot.AppendChild(node);
        }

        public void Close()
        {
            xMLDoc.Save(file);
        }
    }
}
