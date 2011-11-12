/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System;
using System.Collections.Generic;
using System.IO;

namespace Hime.Kernel.Graphs
{
    internal class DOTSerializer
    {
        private StreamWriter writer;

        internal DOTSerializer(string name, string file)
        {
            writer = new System.IO.StreamWriter(file, false, System.Text.Encoding.UTF8);
            writer.WriteLine("digraph " + name + " {");
        }

        internal void WriteNode(string id)
        {
			this.WriteNode(id, id);
        }
		
        internal void WriteNode(string id, string label)
        {
            writer.WriteLine("    _" + id + " [label=\"" + SanitizeString(label) + "\"];");
        }
		
        internal void WriteNodeURL(string id, string url)
        {
            writer.WriteLine("    _" + id + " [label=\"" + SanitizeString(id) + "\", URL=\"" + url + "\"];");
        }
		
        internal void WriteNode(string id, string label, DOTNodeShape shape)
        {
            writer.WriteLine("    _" + id + " [label=\"" + SanitizeString(label) + "\",shape=" + shape.ToString() + "];");
        }

        internal void WriteEdge(string tail, string head, string label)
        {
            writer.WriteLine("    _" + tail + " -> _" + head + " [label=\"" + label + "\"];");
        }

        internal void Close()
        {
            writer.WriteLine("}");
            writer.Close();
        }

        private string SanitizeString(string original)
        {
            return original.Replace("\"", "\\\"").Replace("\\", "\\\\");
        }
    }
}
