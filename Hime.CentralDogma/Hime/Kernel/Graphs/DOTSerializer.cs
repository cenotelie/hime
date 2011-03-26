using System.Collections.Generic;

namespace Hime.Kernel.Graphs
{
    public enum DOTNodeShape
    {
        box, polygon, ellipse, circle,
        point, egg, triangle, plaintext,
        diamond, trapezium, parallelogram, house,
        pentagon, hexagon, septagon, octagon,
        doublecircle, doubleoctagon, tripleoctagon, invtriangle,
        invtrapezium, invhouse, Mdiamond, Msquare,
        Mcircle, rect, rectangle, square,
        none, note, tab, folder,
        box3d, component
    }

    public sealed class DOTSerializer
    {
        private System.IO.StreamWriter writer;

        public DOTSerializer(string name, string file)
        {
            writer = new System.IO.StreamWriter(file, false, System.Text.Encoding.ASCII);
            writer.WriteLine("digraph " + name + " {");
        }

        public void WriteNode(string id)
        {
            writer.WriteLine("    " + id + ";");
        }
        public void WriteNode(string id, string label)
        {
            writer.WriteLine("    " + id + " [label=\"" + SanitizeString(label) + "\"];");
        }
        public void WriteNode(string id, string label, DOTNodeShape shape)
        {
            writer.WriteLine("    " + id + " [label=\"" + SanitizeString(label) + "\",shape=" + shape.ToString() + "];");
        }

        public void WriteEdge(string tail, string head, string label)
        {
            writer.WriteLine("    " + tail + " -> " + head + " [label=\"" + label + "\"];");
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
