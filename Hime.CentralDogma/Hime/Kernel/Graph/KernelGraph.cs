namespace Hime.Kernel.Graph
{
    /// <summary>
    /// Interface for vertices inner data
    /// </summary>
    public interface IVertexData
    {
        /// <summary>
        /// Creates a new vertex using the given line height and character width
        /// </summary>
        /// <param name="lineHeight">Line height</param>
        /// <param name="characterWidth">Character width</param>
        /// <returns>Returns a new vertex representing the data</returns>
        Vertex CreateVertex(float lineHeight, float characterWidth);
        /// <summary>
        /// Draws the content of the vertex
        /// </summary>
        /// <param name="Visual">Vertex visual</param>
        /// <param name="Material">Graph visual</param>
        /// <param name="LineHeight">Line height</param>
        void DrawContent(VertexVisual visual, GraphVisual material, float lineHeight);
    }


    /// <summary>
    /// Represents a vertex in a graph
    /// </summary>
    public sealed class Vertex
    {
        /// <summary>
        /// Inner data
        /// </summary>
        private IVertexData p_Data;
        /// <summary>
        /// Graphical representation
        /// </summary>
        private VertexVisual p_Visual;
        /// <summary>
        /// List of in-bound edges
        /// </summary>
        private System.Collections.Generic.List<Edge> p_EdgesIn;
        /// <summary>
        /// List of out-bound edges
        /// </summary>
        private System.Collections.Generic.List<Edge> p_EdgesOut;

        /// <summary>
        /// Get or set the visual
        /// </summary>
        /// <value>Graphical representation of the vertex</value>
        public VertexVisual Visual
        {
            get { return p_Visual; }
            set { p_Visual = value; }
        }
        /// <summary>
        /// Get an enumeration of the in-bound edges
        /// </summary>
        /// <value>Enumeration of in-bound edges</value>
        public System.Collections.Generic.IEnumerable<Edge> InEdges { get { return p_EdgesIn; } }
        /// <summary>
        /// Get an enumeration of the out-bound edges
        /// </summary>
        /// <value>Enumeration of out-bound edges</value>
        public System.Collections.Generic.IEnumerable<Edge> OutEdges { get { return p_EdgesOut; } }

        /// <summary>
        /// Initializes a new instance of the Vertex class using data
        /// </summary>
        /// <param name="data"></param>
        public Vertex(IVertexData data)
        {
            p_Data = data;
            p_EdgesIn = new System.Collections.Generic.List<Edge>();
            p_EdgesOut = new System.Collections.Generic.List<Edge>();
        }

        /// <summary>
        /// Adds a new in-bound edge
        /// </summary>
        /// <param name="item">New in-bound edge</param>
        public void AddInEdge(Edge item) { p_EdgesIn.Add(item); }
        /// <summary>
        /// Adds a new out-bound edge
        /// </summary>
        /// <param name="item">New ou-bound edge</param>
        public void AddOutEdge(Edge item) { p_EdgesOut.Add(item); }

        /// <summary>
        /// Draw the vertex on the graph representation
        /// </summary>
        /// <param name="material">Graph representation</param>
        /// <param name="lineHeight">Line height</param>
        public void Draw(GraphVisual material, float lineHeight)
        {
            if (p_Visual != null)
            {
                p_Visual.Draw(material);
                p_Data.DrawContent(p_Visual, material, lineHeight);
            }
        }
    }

    /// <summary>
    /// Represents an edge between two vertices of the graph
    /// </summary>
    public sealed class Edge
    {
        /// <summary>
        /// Edge's starting vertex
        /// </summary>
        private Vertex p_Begin;
        /// <summary>
        /// Edge's ending vertex
        /// </summary>
        private Vertex p_End;
        /// <summary>
        /// Edge's value
        /// </summary>
        private object p_Value;
        /// <summary>
        /// Edge's visual representation
        /// </summary>
        private EdgeVisual p_Visual;

        /// <summary>
        /// Get the edge's starting vertex
        /// </summary>
        /// <value>Edge's starting vertex</value>
        public Vertex VertexBegin { get { return p_Begin; } }
        /// <summary>
        /// Get the edge's ending vertex
        /// </summary>
        /// <value>Edge's ending vertex</value>
        public Vertex VertexEnd { get { return p_End; } }
        /// <summary>
        /// Get the edge's value
        /// </summary>
        /// <value>Edge's value</value>
        public object Value { get { return p_Value; } }
        /// <summary>
        /// Get or set the edge's visual representation
        /// </summary>
        /// <value>Edge's visual representation</value>
        public EdgeVisual Visual
        {
            get { return p_Visual; }
            set { p_Visual = value; }
        }

        /// <summary>
        /// Initializes a new instance of the Edge class
        /// </summary>
        /// <param name="begin">Starting vertex</param>
        /// <param name="end">Ending vertex</param>
        /// <param name="value">Value</param>
        public Edge(Vertex begin, Vertex end, object value)
        {
            p_Begin = begin;
            p_End = end;
            p_Value = value;
        }

        /// <summary>
        /// Draw the edge's representation on the graph
        /// </summary>
        /// <param name="Material">Graph representation</param>
        public void Draw(GraphVisual material)
        {
            if (p_Visual != null)
                p_Visual.Draw(p_Begin.Visual, p_End.Visual, material, p_Value.ToString());
        }
    }


    /// <summary>
    /// Represents a graph
    /// </summary>
    public sealed class Graph
    {
        /// <summary>
        /// List of the vertices
        /// </summary>
        private System.Collections.Generic.List<Vertex> p_Vertices;
        /// <summary>
        /// List of the edges
        /// </summary>
        private System.Collections.Generic.List<Edge> p_Edges;
        /// <summary>
        /// Dictionnary associating data do its representating vertex
        /// </summary>
        private System.Collections.Generic.Dictionary<IVertexData, Vertex> p_Represented;
        /// <summary>
        /// Line height
        /// </summary>
        private float p_LineHeight;
        /// <summary>
        /// Character width
        /// </summary>
        private float p_CharWidth;
        /// <summary>
        /// Drawing width
        /// </summary>
        private float p_DrawWidth;
        /// <summary>
        /// Drawing height
        /// </summary>
        private float p_DrawHeight;
        
        /// <summary>
        /// Get an enumeration of all the vertices in the graph
        /// </summary>
        /// <value>Enumeration of all vertices</value>
        public System.Collections.Generic.IEnumerable<Vertex> Vertices { get { return p_Vertices; } }
        /// <summary>
        /// Get an enumeration of all edges in the graph
        /// </summary>
        /// <value>Enumeration of all edges</value>
        public System.Collections.Generic.IEnumerable<Edge> Edges { get { return p_Edges; } }
        /// <summary>
        /// Get the vertices count
        /// </summary>
        /// <value>Vertices count</value>
        public int VerticesCount { get { return p_Vertices.Count; } }
        /// <summary>
        /// Get the drawing width
        /// </summary>
        /// <value>Drawing width</value>
        public float DrawingWidth { get { return p_DrawWidth; } }
        /// <summary>
        /// Get the drawing height
        /// </summary>
        /// <value>Drawing height</value>
        public float DrawingHeight { get { return p_DrawHeight; } }

        /// <summary>
        /// Initializes a new instance of the Graph class
        /// </summary>
        /// <param name="lineHeight">Line Height</param>
        public Graph(float lineHeight)
        {
            p_Vertices = new System.Collections.Generic.List<Vertex>();
            p_Edges = new System.Collections.Generic.List<Edge>();
            p_Represented = new System.Collections.Generic.Dictionary<IVertexData, Vertex>();
            p_LineHeight = lineHeight;
            p_CharWidth = lineHeight * 2 / 3;
            p_DrawWidth = 0;
            p_DrawHeight = 0;
        }

        /// <summary>
        /// Get the representation of the data in the graph as a vertex
        /// </summary>
        /// <param name="data">Represented data</param>
        /// <returns>Returns the corresponding vertex or null if the data is not represented</returns>
        public Vertex GetRepresentationOf(IVertexData data)
        {
            if (p_Represented.ContainsKey(data))
                return p_Represented[data];
            return null;
        }
        /// <summary>
        /// Add a new vertex in the graph representing the given data
        /// </summary>
        /// <param name="data">Data to be represented</param>
        /// <returns>Returns the created vertex</returns>
        public Vertex AddVertex(IVertexData data)
        {
            Vertex NewVertex = data.CreateVertex(p_LineHeight, p_CharWidth);
            p_Vertices.Add(NewVertex);
            p_Represented.Add(data, NewVertex);
            return NewVertex;
        }
        /// <summary>
        /// Add a new edge in the graph
        /// </summary>
        /// <param name="begin">Starting vertex</param>
        /// <param name="end">Ending vertex</param>
        /// <param name="value">Value</param>
        /// <returns>Returns the created edge</returns>
        public Edge AddEdge(Vertex begin, Vertex end, object value)
        {
            Edge NewEdge = new Edge(begin, end, value);
            begin.AddOutEdge(NewEdge);
            end.AddInEdge(NewEdge);
            p_Edges.Add(NewEdge);
            return NewEdge;
        }

        /// <summary>
        /// Build a list of vertices of the sub graph containing init
        /// </summary>
        /// <param name="init">Initial vertex</param>
        /// <param name="originals">List of all possible vertices</param>
        /// <returns>Returns the sub graph containing init as a list of vertices</returns>
        /// <remarks>All the vertices of the returned value are removed from originals</remarks>
        private System.Collections.Generic.List<Vertex> Build_BuildSubGraph(Vertex init, System.Collections.Generic.List<Vertex> originals)
        {
            System.Collections.Generic.List<Vertex> Vertices = new System.Collections.Generic.List<Vertex>();
            Vertices.Add(init);
            originals.Remove(init);
            for (int i = 0; i != Vertices.Count; i++)
            {
                foreach (Edge Edge in Vertices[i].InEdges)
                {
                    if (!Vertices.Contains(Edge.VertexBegin))
                    {
                        Vertices.Add(Edge.VertexBegin);
                        originals.Remove(Edge.VertexBegin);
                    }
                }
                foreach (Edge Edge in Vertices[i].OutEdges)
                {
                    if (!Vertices.Contains(Edge.VertexEnd))
                    {
                        Vertices.Add(Edge.VertexEnd);
                        originals.Remove(Edge.VertexEnd);
                    }
                }
            }
            return Vertices;
        }

        /// <summary>
        /// Builds the graph using the given placement method
        /// </summary>
        /// <param name="method">Placement method</param>
        public void Build(IPlacementMethod method)
        {
            System.Collections.Generic.List<Vertex> Originals = new System.Collections.Generic.List<Vertex>(p_Vertices);
            System.Collections.Generic.List<System.Collections.Generic.List<Vertex>> SubGraphs = new System.Collections.Generic.List<System.Collections.Generic.List<Vertex>>();
            System.Collections.Generic.List<System.Drawing.PointF> SubGraphsMin = new System.Collections.Generic.List<System.Drawing.PointF>();

            while (Originals.Count != 0)
                SubGraphs.Add(Build_BuildSubGraph(Originals[0], Originals));

            foreach (System.Collections.Generic.List<Vertex> SubGraph in SubGraphs)
            {
                method.Place(SubGraph);
                float MinX = float.MaxValue;
                float MaxX = float.MinValue;
                float MinY = float.MaxValue;
                float MaxY = float.MinValue;
                foreach (Vertex V in SubGraph)
                {
                    if (V.Visual.WrappingBoxLeft < MinX) MinX = V.Visual.WrappingBoxLeft;
                    if (V.Visual.WrappingBoxRight > MaxX) MaxX = V.Visual.WrappingBoxRight;
                    if (V.Visual.WrappingBoxTop < MinY) MinY = V.Visual.WrappingBoxTop;
                    if (V.Visual.WrappingBoxBottom > MaxY) MaxY = V.Visual.WrappingBoxBottom;
                }
                Vertex SubGraphVertex = new Vertex(null);
                SubGraphVertex.Visual = new VertexVisualRectangle(MaxX - MinX, MaxY - MinY);
                Originals.Add(SubGraphVertex);
                SubGraphsMin.Add(new System.Drawing.PointF(MinX, MinY));
            }

            if (SubGraphs.Count == 1)
            {
                foreach (Vertex V in p_Vertices)
                    V.Visual.Translate(-SubGraphsMin[0].X, -SubGraphsMin[0].Y);
                p_DrawWidth = Originals[0].Visual.BoundingWidth;
                p_DrawHeight = Originals[0].Visual.BoundingHeight;
            }
            else
            {
                for (int i = 0; i != Originals.Count; i++)
                {
                    for (int j = i + 1; j != Originals.Count; j++)
                    {
                        Edge NewEdge = new Edge(Originals[i], Originals[j], null);
                        Originals[i].AddOutEdge(NewEdge);
                        Originals[j].AddInEdge(NewEdge);
                        NewEdge = new Edge(Originals[j], Originals[i], null);
                        Originals[j].AddOutEdge(NewEdge);
                        Originals[i].AddInEdge(NewEdge);
                    }
                }
                method.ResetParameters();
                method.Place(Originals);
                float MinX = float.MaxValue;
                float MaxX = float.MinValue;
                float MinY = float.MaxValue;
                float MaxY = float.MinValue;
                foreach (Vertex V in Originals)
                {
                    if (V.Visual.WrappingBoxLeft < MinX) MinX = V.Visual.WrappingBoxLeft;
                    if (V.Visual.WrappingBoxRight > MaxX) MaxX = V.Visual.WrappingBoxRight;
                    if (V.Visual.WrappingBoxTop < MinY) MinY = V.Visual.WrappingBoxTop;
                    if (V.Visual.WrappingBoxBottom > MaxY) MaxY = V.Visual.WrappingBoxBottom;
                }
                for (int i = 0; i != Originals.Count; i++)
                {
                    float OffsetX = Originals[i].Visual.X - SubGraphsMin[i].X - MinX;
                    float OffsetY = Originals[i].Visual.Y - SubGraphsMin[i].Y - MinY;
                    foreach (Vertex V in SubGraphs[i])
                        V.Visual.Translate(OffsetX, OffsetY);
                }
                p_DrawWidth = MaxX - MinX;
                p_DrawHeight = MaxY - MinY;
            }
        }

        /// <summary>
        /// Draws graph at given scale
        /// </summary>
        /// <param name="scale">Drawing scale</param>
        /// <returns>Returns a bitmap representation</returns>
        public System.Drawing.Bitmap Draw(float scale)
        {
            int Width = ((int)(p_DrawWidth * scale)) + 1;
            int Height = ((int)(p_DrawHeight * scale)) + 1;
            
            System.Drawing.Drawing2D.Matrix Transform = new System.Drawing.Drawing2D.Matrix();
            Transform.Scale(scale, scale, System.Drawing.Drawing2D.MatrixOrder.Append);
            GraphVisualImage Material = new GraphVisualImage(Width, Height, "Calibri", p_LineHeight, Transform);
            foreach (Vertex V in p_Vertices)
                V.Draw(Material, p_LineHeight);
            foreach (Vertex V in p_Vertices)
            {
                System.Collections.Generic.Dictionary<Vertex,System.Collections.Generic.List<Edge>> OutEdges = new System.Collections.Generic.Dictionary<Vertex,System.Collections.Generic.List<Edge>>();
                foreach (Edge E in V.OutEdges)
                {
                    if (!OutEdges.ContainsKey(E.VertexEnd))
                        OutEdges.Add(E.VertexEnd, new System.Collections.Generic.List<Edge>());
                    OutEdges[E.VertexEnd].Add(E);
                }
                foreach (Vertex End in OutEdges.Keys)
                {
                    string Value = OutEdges[End][0].Value.ToString();
                    for (int i = 1; i != OutEdges[End].Count; i++)
                        Value += "," + OutEdges[End][i].Value.ToString();
                    OutEdges[End][0].Visual.Draw(V.Visual, End.Visual, Material, Value);
                }
            }
            return (System.Drawing.Bitmap)Material.Image;
        }
    }
}