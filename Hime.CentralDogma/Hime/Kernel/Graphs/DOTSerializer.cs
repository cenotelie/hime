using System.Collections.Generic;

namespace Hime.Kernel.Graphs
{
    public class DOTSerializer
    {
        private System.IO.StreamWriter writer;

        public DOTSerializer(string name, string file)
        {
            writer = new System.IO.StreamWriter(file, false, System.Text.Encoding.UTF8);
            writer.WriteLine("digraph " + name + " {");
        }

        public void WriteNode(string id)
        {
            writer.WriteLine("    _" + id + " [label=\"" + SanitizeString(id) + "\"];");
        }
        public void WriteNode(string id, string label)
        {
            writer.WriteLine("    _" + id + " [label=\"" + SanitizeString(label) + "\"];");
        }
        public void WriteNode(string id, string label, string url)
        {
            writer.WriteLine("    _" + id + " [label=\"" + SanitizeString(label) + "\", URL=\"" + url + "\"];");
        }
        public void WriteNode(string id, string label, DOTNodeShape shape)
        {
            writer.WriteLine("    _" + id + " [label=\"" + SanitizeString(label) + "\",shape=" + shape.ToString() + "];");
        }
        public void WriteNode(string id, string label, DOTNodeShape shape, string url)
        {
            writer.WriteLine("    _" + id + " [label=\"" + SanitizeString(label) + "\", URL=\"" + url + "\",shape=" + shape.ToString() + "];");
        }

        public void WriteEdge(string tail, string head, string label)
        {
            writer.WriteLine("    _" + tail + " -> _" + head + " [label=\"" + label + "\"];");
        }

        public void Close()
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
