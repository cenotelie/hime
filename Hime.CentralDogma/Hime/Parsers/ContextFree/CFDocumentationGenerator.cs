/*
 * Author: Laurent Wouters
 * Date: 16/09/2011
 * Time: 09:16
 * 
 */
using System.Collections.Generic;
using System.Xml;

namespace Hime.Parsers.ContextFree
{
    public sealed class CFDocumentationGenerator
    {
        private string docFile;
        private bool exportVisuals;
        private string dotBinary;



        public void Export(ParserData data, CFGrammar grammar)
        {
            string directory = docFile + "_temp";
            System.IO.Directory.CreateDirectory(directory);

            Kernel.Resources.ResourceAccessor accessor = new Kernel.Resources.ResourceAccessor();
            Kernel.Documentation.MHTMLCompiler compiler = new Kernel.Documentation.MHTMLCompiler();
            compiler.Title = "Documentation " + grammar.LocalName;
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceStreamText("text/html", "utf-8", "index.html", accessor.GetStreamFor("Transforms.Doc.Index.html")));
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceStreamText("text/html", "utf-8", "GraphParser.html", accessor.GetStreamFor("Transforms.Doc.Parser.html")));
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceStreamText("text/css", "utf-8", "hime_data/Hime.css", accessor.GetStreamFor("Transforms.Hime.css")));
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceStreamText("text/javascript", "utf-8", "hime_data/Hime.js", accessor.GetStreamFor("Transforms.Hime.js")));
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceStreamImage("image/gif", "hime_data/button_plus.gif", accessor.GetStreamFor("Visuals.button_plus.gif")));
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceStreamImage("image/gif", "hime_data/button_minus.gif", accessor.GetStreamFor("Visuals.button_minus.gif")));
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceStreamImage("image/png", "hime_data/Hime.Logo.png", accessor.GetStreamFor("Visuals.Hime.Logo.png")));
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceStreamImage("image/png", "hime_data/Hime.GoTo.png", accessor.GetStreamFor("Visuals.Hime.GoTo.png")));
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceStreamImage("image/png", "hime_data/Hime.Info.png", accessor.GetStreamFor("Visuals.Hime.Info.png")));
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceStreamImage("image/png", "hime_data/Hime.Warning.png", accessor.GetStreamFor("Visuals.Hime.Warning.png")));
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceStreamImage("image/png", "hime_data/Hime.Error.png", accessor.GetStreamFor("Visuals.Hime.Error.png")));
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceStreamImage("image/png", "hime_data/Hime.Shift.png", accessor.GetStreamFor("Visuals.Hime.Shift.png")));
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceStreamImage("image/png", "hime_data/Hime.Reduce.png", accessor.GetStreamFor("Visuals.Hime.Reduce.png")));
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceStreamImage("image/png", "hime_data/Hime.None.png", accessor.GetStreamFor("Visuals.Hime.None.png")));
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceStreamImage("image/png", "hime_data/Hime.ShiftReduce.png", accessor.GetStreamFor("Visuals.Hime.ShiftReduce.png")));
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceStreamImage("image/png", "hime_data/Hime.ReduceReduce.png", accessor.GetStreamFor("Visuals.Hime.ReduceReduce.png")));

            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.AppendChild(GetData(doc, grammar));
            doc.Save(directory + "\\data.xml");
            accessor.AddCheckoutFile(directory + "\\data.xml");

            // generate header
            accessor.CheckOut("Transforms.Doc.Header.xslt", directory + "\\Header.xslt");
            System.Xml.Xsl.XslCompiledTransform transform = new System.Xml.Xsl.XslCompiledTransform();
            transform.Load(directory + "\\Header.xslt");
            transform.Transform(directory + "\\data.xml", directory + "\\header.html");
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceFileText("text/html", "utf-8", "header.html", directory + "\\header.html"));
            accessor.AddCheckoutFile(directory + "\\header.html");
            // generate grammar
            accessor.CheckOut("Transforms.Doc.Grammar.xslt", directory + "\\Grammar.xslt");
            transform = new System.Xml.Xsl.XslCompiledTransform();
            transform.Load(directory + "\\Grammar.xslt");
            transform.Transform(directory + "\\data.xml", directory + "\\grammar.html");
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceFileText("text/html", "utf-8", "grammar.html", directory + "\\grammar.html"));
            accessor.AddCheckoutFile(directory + "\\grammar.html");

            doc = new System.Xml.XmlDocument();
            List<System.Xml.XmlNode> nodes = new List<System.Xml.XmlNode>();
            System.Xml.XmlNode nodeGraph = data.SerializeXML(doc);
            foreach (System.Xml.XmlNode child in nodeGraph.ChildNodes)
                nodes.Add(child);

            // generate sets
            string tfile = "ParserData_LR1";
            if (data is LR.ParserDataLRStar)
            {
                if (exportVisuals) tfile = "ParserData_LRStarSVG";
                else tfile = "ParserData_LRStarDOT";
            }
            accessor.CheckOut("Transforms.Doc." + tfile + ".xslt", directory + "\\" + tfile + ".xslt");
            transform = new System.Xml.Xsl.XslCompiledTransform();
            transform.Load(directory + "\\" + tfile + ".xslt");
            foreach (System.Xml.XmlNode child in nodes)
            {
                string temp = directory + "\\Set_" + child.Attributes["SetID"].Value;
                while (doc.HasChildNodes)
                    doc.RemoveChild(doc.FirstChild);
                doc.AppendChild(child);
                doc.Save(temp + ".xml");
                accessor.AddCheckoutFile(temp + ".xml");
                transform.Transform(temp + ".xml", temp + ".html");
                compiler.AddSource(new Kernel.Documentation.MHTMLSourceFileText("text/html", "utf-8", "Set_" + child.Attributes["SetID"].Value + ".html", temp + ".html"));
                accessor.AddCheckoutFile(temp + ".html");
            }

            while (doc.HasChildNodes)
                doc.RemoveChild(doc.FirstChild);
            doc.AppendChild(doc.CreateXmlDeclaration("1.0", "utf-8", null));
            doc.AppendChild(nodeGraph);
            foreach (System.Xml.XmlNode child in nodes)
                nodeGraph.AppendChild(child);
            doc.Save(directory + "\\data.xml");
            // generate menu
            accessor.CheckOut("Transforms.Doc.Menu.xslt", directory + "\\Menu.xslt");
            transform = new System.Xml.Xsl.XslCompiledTransform();
            transform.Load(directory + "\\Menu.xslt");
            transform.Transform(directory + "\\data.xml", directory + "\\menu.html");
            compiler.AddSource(new Kernel.Documentation.MHTMLSourceFileText("text/html", "utf-8", "menu.html", directory + "\\menu.html"));
            accessor.AddCheckoutFile(directory + "\\menu.html");

            // export parser data
            List<string> files = data.SerializeVisuals(directory, exportVisuals, dotBinary);
            foreach (string file in files)
            {
                System.IO.FileInfo info = new System.IO.FileInfo(file);
                if (file.EndsWith(".svg"))
                    compiler.AddSource(new Kernel.Documentation.MHTMLSourceFileImage("image/svg+xml", info.Name, file));
                else
                    compiler.AddSource(new Kernel.Documentation.MHTMLSourceFileText("text/plain", "utf-8", info.Name, file));
                accessor.AddCheckoutFile(file);
            }

            compiler.CompileTo(docFile);
            accessor.Close();
            System.IO.Directory.Delete(directory, true);
        }

        private XmlNode GetData(XmlDocument document, CFGrammar grammar)
        {
            XmlNode root = document.CreateElement("CFGrammar");
            root.Attributes.Append(document.CreateAttribute("Name"));
            root.Attributes["Name"].Value = grammar.LocalName;
            foreach (Variable var in grammar.Variables)
                root.AppendChild(var.GetXMLNode(document));
            return root;
        }
    }
}