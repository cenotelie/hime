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
        private System.IO.StreamWriter p_Writer;

        public DOTSerializer(string name, string file)
        {
            p_Writer = new System.IO.StreamWriter(file, false, System.Text.Encoding.ASCII);
            p_Writer.WriteLine("digraph " + name + " {");
        }

        public void WriteNode(string id)
        {
            p_Writer.WriteLine("    " + id + ";");
        }
        public void WriteNode(string id, string label)
        {
            p_Writer.WriteLine("    " + id + " [label=\"" + SanitizeString(label) + "\"];");
        }
        public void WriteNode(string id, string label, DOTNodeShape shape)
        {
            p_Writer.WriteLine("    " + id + " [label=\"" + SanitizeString(label) + "\",shape=" + shape.ToString() + "];");
        }

        public void WriteEdge(string tail, string head, string label)
        {
            p_Writer.WriteLine("    " + tail + " -> " + head + " [label=\"" + label + "\"];");
        }

        public void Close()
        {
            p_Writer.WriteLine("}");
            p_Writer.Close();
        }

        private string SanitizeString(string original)
        {
            return original.Replace("\"", "\\\"").Replace("\\", "\\\\");
        }
    }
}