namespace Hime.Kernel.Graph
{
    /// <summary>
    /// Interface for placement methods
    /// </summary>
    interface IPlacementMethod
    {
        /// <summary>
        /// Resets the parameters
        /// </summary>
        void ResetParameters();
        /// <summary>
        /// Place in a 2D space the given vertices
        /// </summary>
        /// <param name="vertices">Vertices to place</param>
        void Place(System.Collections.Generic.List<Vertex> vertices);
    }

    /// <summary>
    /// Set of mathematical tool functions
    /// </summary>
    static class PlacementTools
    {
        /// <summary>
        /// Get the square distance between two points
        /// </summary>
        /// <param name="a">Point a</param>
        /// <param name="b">Point b</param>
        /// <returns>Returns the square distance between a and b</returns>
        public static float GetSquareDistance(System.Drawing.PointF a, System.Drawing.PointF b)
        {
            float OffsetX = b.X - a.X;
            float OffsetY = b.Y - a.Y;
            return OffsetX * OffsetX + OffsetY * OffsetY;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static float GetDistance(System.Drawing.PointF a, System.Drawing.PointF b)
        {
            float OffsetX = b.X - a.X;
            float OffsetY = b.Y - a.Y;
            return (float)System.Math.Sqrt(OffsetX * OffsetX + OffsetY * OffsetY);
        }

        public static float GetSquareDistanceNodeEdge(System.Drawing.PointF Node, System.Drawing.PointF SegA, System.Drawing.PointF SegB)
        {
            float ABdotAP = (SegB.X - SegA.X) * (Node.X - SegA.X) + (SegB.Y - SegA.Y) * (Node.Y - SegA.Y);
            if (ABdotAP <= 0)
                return GetDistance(Node, SegA);
            float BAdotBP = (SegA.X - SegB.X) * (Node.X - SegB.X) + (SegA.Y - SegB.Y) * (Node.Y - SegB.Y);
            if (BAdotBP <= 0)
                return GetDistance(Node, SegB);
            float AP2 = GetSquareDistance(SegA, Node);
            float BP2 = GetSquareDistance(SegB, Node);
            float AB2 = GetSquareDistance(SegA, SegB);
            float AB = GetDistance(SegA, SegB);
            float Temp = (AB2 + BP2 - AP2) / (2 * AB);
            return BP2 - Temp * Temp;
        }
        public static float GetDistanceNodeEdge(System.Drawing.PointF Node, System.Drawing.PointF SegA, System.Drawing.PointF SegB)
        {
            return (float)System.Math.Sqrt(GetSquareDistanceNodeEdge(Node, SegA, SegB));
        }

        public static bool SegmentsIntersects(System.Drawing.PointF Edge1Begin, System.Drawing.PointF Edge1End, System.Drawing.PointF Edge2Begin, System.Drawing.PointF Edge2End)
        {
            System.Drawing.PointF Intersection;
            return SegmentsIntersects(Edge1Begin, Edge1End, Edge2Begin, Edge2End, out Intersection);
        }
        public static bool SegmentsIntersects(System.Drawing.PointF Edge1Begin, System.Drawing.PointF Edge1End, System.Drawing.PointF Edge2Begin, System.Drawing.PointF Edge2End, out System.Drawing.PointF Intersection)
        {
            float a1 = (Edge1End.Y - Edge1Begin.Y) / (Edge1End.X - Edge1Begin.X);
            float a2 = (Edge2End.Y - Edge2Begin.Y) / (Edge2End.X - Edge2Begin.X);
            Intersection = System.Drawing.PointF.Empty;

            if (float.IsInfinity(a1))
            {
                if (float.IsInfinity(a2)) return false;
                float y2 = Edge2Begin.Y + a2 * (Edge1Begin.X - Edge2Begin.X);
                if (Edge2Begin.Y < Edge2End.Y)
                    return ((Edge2Begin.Y <= y2) && (Edge2End.Y >= y2));
                return ((Edge2End.Y <= y2) && (Edge2Begin.Y >= y2));
            }
            if (float.IsInfinity(a2))
            {
                if (float.IsInfinity(a1)) return false;
                float y1 = Edge1Begin.Y + a1 * (Edge2Begin.X - Edge1Begin.X);
                if (Edge1Begin.Y < Edge1End.Y)
                    return ((Edge1Begin.Y <= y1) && (Edge1End.Y >= y1));
                return ((Edge1End.Y <= y1) && (Edge1Begin.Y >= y1));
            }

            if (a1 == a2) return false; // Parallel segments
            float xi = (Edge1Begin.Y - Edge2Begin.Y - a1 * Edge1Begin.X + a2 * Edge2Begin.X) / (a2 - a1);
            float yi1 = Edge1Begin.Y + a1 * (xi - Edge1Begin.X);
            float yi2 = Edge2Begin.Y + a2 * (xi - Edge2Begin.X);
            if (yi1 == yi2)
            {
                Intersection = new System.Drawing.PointF(xi, yi1);
                return true;
            }
            return false;
        }
    }



    class PlacementState
    {
        public System.Collections.Generic.List<System.Drawing.RectangleF> VertexPositions;
        public System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<int, int>> Edges;

        public PlacementState(PlacementState Copied)
        {
            VertexPositions = new System.Collections.Generic.List<System.Drawing.RectangleF>();
            Edges = new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<int, int>>();
            foreach (System.Drawing.RectangleF Vertex in Copied.VertexPositions)
                VertexPositions.Add(new System.Drawing.RectangleF(Vertex.Location, Vertex.Size));
            foreach (System.Collections.Generic.KeyValuePair<int, int> Edge in Copied.Edges)
                Edges.Add(new System.Collections.Generic.KeyValuePair<int, int>(Edge.Key, Edge.Value));
        }
        public PlacementState(System.Collections.Generic.List<Vertex> Vertices)
        {
            VertexPositions = new System.Collections.Generic.List<System.Drawing.RectangleF>();
            Edges = new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<int, int>>();
            for (int i = 0; i != Vertices.Count; i++)
                foreach (Edge E in Vertices[i].OutEdges)
                    Edges.Add(new System.Collections.Generic.KeyValuePair<int, int>(i, Vertices.IndexOf(E.VertexEnd)));
        }
    }








    class PlacementAnnealingState : PlacementState
    {
        public float GraphAverageVertexRay;
        public float GraphExpectedSurface;
        public float[] VertexRay;
        public float[] VertexBoundingRay;
        public float[] EdgesLength;
        
        public PlacementAnnealingState(PlacementAnnealingState Copied) : base(Copied)
        {
            GraphAverageVertexRay = Copied.GraphAverageVertexRay;
            GraphExpectedSurface = Copied.GraphExpectedSurface;
            VertexRay = Copied.VertexRay;
            EdgesLength = new float[Edges.Count];
            for (int i = 0; i != Copied.EdgesLength.Length; i++)
                EdgesLength[i] = Copied.EdgesLength[i];
            VertexBoundingRay = new float[Copied.VertexBoundingRay.Length];
            for (int i = 0; i != Copied.VertexBoundingRay.Length; i++)
                VertexBoundingRay[i] = Copied.VertexBoundingRay[i];
        }
        public PlacementAnnealingState(System.Collections.Generic.List<Vertex> Vertices) : base(Vertices)
        {
            VertexRay = new float[Vertices.Count];
            for (int i = 0; i != Vertices.Count; i++)
            {
                VertexRay[i] = Vertices[i].Visual.BoundingRay;
                GraphAverageVertexRay += VertexRay[i];
            }
            GraphAverageVertexRay = GraphAverageVertexRay / Vertices.Count;
            float BorderLength = (float)System.Math.Sqrt(Vertices.Count) * GraphAverageVertexRay * 4;
            GraphExpectedSurface = BorderLength * BorderLength;
            BorderLength = BorderLength * 1.5f;

            System.Random Random = new System.Random();
            foreach (Vertex V in Vertices)
            {
                bool Intersect = true;
                while (Intersect)
                {
                    Intersect = false;
                    float X = ((float)Random.NextDouble() * BorderLength);
                    float Y = ((float)Random.NextDouble() * BorderLength);
                    System.Drawing.RectangleF New = new System.Drawing.RectangleF(X, Y, V.Visual.WrappingWidth, V.Visual.WrappingHeight);
                    foreach (System.Drawing.RectangleF Previous in VertexPositions)
                    {
                        if (Previous.IntersectsWith(New))
                        {
                            Intersect = true;
                            break;
                        }
                    }
                    if (!Intersect)
                        VertexPositions.Add(New);
                }
            }
        }

        public void RecomputeEdges()
        {
            EdgesLength = new float[Edges.Count];
            VertexBoundingRay = new float[VertexPositions.Count];
            for (int i = 0; i != Edges.Count; i++)
            {
                int Vertex = Edges[i].Key;
                float Length = PlacementTools.GetDistance(VertexPositions[Edges[i].Key].Location, VertexPositions[Edges[i].Value].Location);
                EdgesLength[i] = Length;
                if (VertexBoundingRay[Vertex] < Length)
                    VertexBoundingRay[Vertex] = Length;
            }
        }
    }

    class PlacementAnnealingMethod : IPlacementMethod
    {
        private static int index = 0;

        public class ParametersAnnealing
        {
            public int IterStageCount;
            public int IterTryCountByStageByVertex;
            public float CoolingParameter;
            public float ProbaParamEnergyPercent;
            public float ProbaParamFloor;

            public ParametersAnnealing()
            {
                IterStageCount = 20;
                IterTryCountByStageByVertex = 30;
                CoolingParameter = 0.80f;
                ProbaParamEnergyPercent = 0.05f;
                ProbaParamFloor = 0.30f;
            }
        }
        public class ParametersEnergy
        {
            public float ParamNodeDistribution;
            public float ParamEdgesLength;
            public float ParamEdgesCrossing;
            public float ParamNodeEdgeDistance;

            public ParametersEnergy()
            {
                ParamNodeDistribution = 4;
                ParamEdgesLength = 1;
                ParamEdgesCrossing = 5;
                ParamNodeEdgeDistance = 1;
            }
        }

        private System.Random p_Random;
        private ParametersAnnealing p_ParamsAnnealing;
        private ParametersEnergy p_ParamsEnergy;

        public ParametersAnnealing ParamsAnnealing { get { return p_ParamsAnnealing; } }
        public ParametersEnergy ParamsEnergy { get { return p_ParamsEnergy; } }

        public PlacementAnnealingMethod()
        {
            p_Random = new System.Random();
            p_ParamsEnergy = new ParametersEnergy();
            p_ParamsAnnealing = new ParametersAnnealing();
        }

        public void ResetParameters()
        {
            p_ParamsEnergy = new ParametersEnergy();
            p_ParamsAnnealing = new ParametersAnnealing();
        }

        private PlacementAnnealingState GetNeighbour(PlacementAnnealingState State, float Radius)
        {
            PlacementAnnealingState Neighbour = new PlacementAnnealingState(State);
            bool Intersect = true;
            while (Intersect)
            {
                Intersect = false;
                int Index = p_Random.Next(0, State.VertexPositions.Count - 1);
                double Angle = 2 * p_Random.NextDouble() * System.Math.PI;
                float OffsetX = (float)(Radius * System.Math.Cos(Angle));
                float OffsetY = (float)(Radius * System.Math.Sin(Angle));
                System.Drawing.RectangleF Current = State.VertexPositions[Index];
                System.Drawing.RectangleF Next = new System.Drawing.RectangleF(Current.X + OffsetX, Current.Y + OffsetY, Current.Width, Current.Height);
                for (int i = 0; i != Neighbour.VertexPositions.Count; i++)
                {
                    if (i != Index)
                    {
                        if (PlacementTools.GetDistance(Neighbour.VertexPositions[i].Location, Next.Location) <= Neighbour.VertexRay[i] + Neighbour.VertexRay[Index])
                        {
                            Intersect = true;
                            break;
                        }
                    }
                }
                if (!Intersect)
                {
                    Neighbour.VertexPositions[Index] = Next;
                    Neighbour.RecomputeEdges();
                }
            }
            return Neighbour;
        }

        private float ComputeEnergy_NodeDistribution(PlacementAnnealingState State)
        {
            float Energy = 0;
            for (int i = 0; i != State.VertexPositions.Count; i++)
            {
                System.Drawing.RectangleF Vertex1 = State.VertexPositions[i];
                for (int j = i + 1; j != State.VertexPositions.Count; j++)
                {
                    System.Drawing.RectangleF Vertex2 = State.VertexPositions[j];
                    Energy += (p_ParamsEnergy.ParamNodeDistribution * State.VertexRay[i] * State.VertexRay[j] * State.GraphExpectedSurface) / (State.GraphAverageVertexRay * State.GraphAverageVertexRay * PlacementTools.GetSquareDistance(Vertex1.Location, Vertex2.Location));
                }
            }
            Energy = Energy * 2 / (State.VertexPositions.Count * (State.VertexPositions.Count - 1));
            return Energy;
        }
        private float ComputeEnergy_EdgesLength(PlacementAnnealingState State)
        {
            float Energy = 0;
            for (int i = 0; i != State.EdgesLength.Length; i++)
            {
                if (State.Edges[i].Key == State.Edges[i].Value) continue;
                float ray1 = State.VertexRay[State.Edges[i].Key];
                float ray2 = State.VertexRay[State.Edges[i].Value];
                float scale = (State.EdgesLength[i] - (ray1 + ray2)) / (2 * State.GraphAverageVertexRay);
                if (scale < 1)
                    Energy += p_ParamsEnergy.ParamEdgesLength / (scale * scale) - p_ParamsEnergy.ParamEdgesLength;
                else
                    Energy += p_ParamsEnergy.ParamEdgesLength * scale * scale - p_ParamsEnergy.ParamEdgesLength;
            }
            Energy = Energy / State.EdgesLength.Length;
            return Energy;
        }
        private float ComputeEnergy_EdgesCrossing(PlacementAnnealingState State)
        {
            float Energy = 0;
            for (int i = 0; i != State.VertexPositions.Count; i++)
            {
                for (int j = i + 1; j != State.VertexPositions.Count; j++)
                {
                    float Distance = PlacementTools.GetDistance(State.VertexPositions[i].Location, State.VertexPositions[j].Location);
                    if (Distance < State.VertexBoundingRay[i] + State.VertexBoundingRay[j])
                    {
                        System.Collections.Generic.List<int> EdgesVertex1 = new System.Collections.Generic.List<int>();
                        System.Collections.Generic.List<int> EdgesVertex2 = new System.Collections.Generic.List<int>();
                        for (int k = 0; k != State.Edges.Count; k++)
                        {
                            if (State.Edges[k].Key == i)
                            {
                                foreach (int Index in EdgesVertex2)
                                {
                                    if (PlacementTools.SegmentsIntersects(State.VertexPositions[State.Edges[k].Key].Location, State.VertexPositions[State.Edges[k].Value].Location, State.VertexPositions[State.Edges[Index].Key].Location, State.VertexPositions[State.Edges[Index].Value].Location))
                                    {
                                        float offsetx1 = State.VertexPositions[State.Edges[k].Value].Left - State.VertexPositions[State.Edges[k].Key].Left;
                                        float offsety1 = State.VertexPositions[State.Edges[k].Value].Top - State.VertexPositions[State.Edges[k].Key].Top;
                                        float offsetx2 = State.VertexPositions[State.Edges[Index].Value].Left - State.VertexPositions[State.Edges[Index].Key].Left;
                                        float offsety2 = State.VertexPositions[State.Edges[Index].Value].Top - State.VertexPositions[State.Edges[Index].Key].Top;
                                        float cos = (offsetx1 * offsetx2 + offsety1 * offsety2) / (State.EdgesLength[k] * State.EdgesLength[Index]);
                                        if (cos < 0) cos = -cos;
                                        Energy += p_ParamsEnergy.ParamEdgesCrossing / cos;
                                    }
                                }
                                EdgesVertex1.Add(k);
                            }
                            else if (State.Edges[k].Key == j)
                            {
                                foreach (int Index in EdgesVertex1)
                                {
                                    if (PlacementTools.SegmentsIntersects(State.VertexPositions[State.Edges[k].Key].Location, State.VertexPositions[State.Edges[k].Value].Location, State.VertexPositions[State.Edges[Index].Key].Location, State.VertexPositions[State.Edges[Index].Value].Location))
                                    {
                                        float offsetx1 = State.VertexPositions[State.Edges[k].Value].Left - State.VertexPositions[State.Edges[k].Key].Left;
                                        float offsety1 = State.VertexPositions[State.Edges[k].Value].Top - State.VertexPositions[State.Edges[k].Key].Top;
                                        float offsetx2 = State.VertexPositions[State.Edges[Index].Value].Left - State.VertexPositions[State.Edges[Index].Key].Left;
                                        float offsety2 = State.VertexPositions[State.Edges[Index].Value].Top - State.VertexPositions[State.Edges[Index].Key].Top;
                                        float cos = (offsetx1 * offsetx2 + offsety1 * offsety2) / (State.EdgesLength[k] * State.EdgesLength[Index]);
                                        if (cos < 0) cos = -cos;
                                        Energy += p_ParamsEnergy.ParamEdgesCrossing / cos;
                                    }
                                }
                                EdgesVertex2.Add(k);
                            }
                        }
                    }
                }
            }
            return Energy * 2 / (State.EdgesLength.Length * (State.EdgesLength.Length - 1));
        }
        private float ComputeEnergy_NodeEdgeDistance(PlacementAnnealingState State)
        {
            float Energy = 0;
            int Card = 0;
            for (int i = 0; i != State.VertexPositions.Count; i++)
            {
                for (int j = 0; j != State.Edges.Count; j++)
                {
                    if (State.Edges[j].Key == i || State.Edges[j].Value == i)
                        continue;
                    float ray1 = State.VertexRay[State.Edges[j].Key];
                    float ray2 = State.VertexRay[State.Edges[j].Value];
                    float scale = PlacementTools.GetDistanceNodeEdge(State.VertexPositions[i].Location, State.VertexPositions[State.Edges[j].Key].Location, State.VertexPositions[State.Edges[j].Value].Location) / (2 * State.GraphAverageVertexRay);
                    Energy += p_ParamsEnergy.ParamNodeEdgeDistance / (scale * scale);
                    Card++;
                }
            }
            Energy = Energy / Card;
            return Energy;
        }
        private float ComputeEnergy(PlacementAnnealingState State, System.IO.StreamWriter Writer)
        {
            float EnergyNodeDistribution = ComputeEnergy_NodeDistribution(State);
            float EnergyEdgesLength = ComputeEnergy_EdgesLength(State);
            float EnergyEdgesCrossing = ComputeEnergy_EdgesCrossing(State);
            float EnergyNodeEdgeDistance = ComputeEnergy_NodeEdgeDistance(State);
            float EnergyTotal = EnergyNodeDistribution + EnergyEdgesLength + EnergyEdgesCrossing + EnergyNodeEdgeDistance;
            Writer.Write("{");
            Writer.Write(EnergyNodeDistribution);
            Writer.Write(" | ");
            Writer.Write(EnergyEdgesLength);
            Writer.Write(" | ");
            Writer.Write(EnergyEdgesCrossing);
            Writer.Write(" | ");
            Writer.Write(EnergyNodeEdgeDistance);
            Writer.Write(" => ");
            Writer.Write(EnergyTotal);
            Writer.WriteLine("}");
            return EnergyTotal;
        }

        public void Place(System.Collections.Generic.List<Vertex> Vertices)
        {
            System.IO.StreamWriter Writer = new System.IO.StreamWriter("data" + index.ToString() + ".txt");
            Writer.Write("{");
            Writer.Write("Distribution");
            Writer.Write(" | ");
            Writer.Write("Edges Length");
            Writer.Write(" | ");
            Writer.Write("Edges Crossing");
            Writer.Write(" | ");
            Writer.Write("Node Edge Distance");
            Writer.Write(" => ");
            Writer.Write("Total");
            Writer.WriteLine("}");
            index++;

            // Create a first state and compute energy
            PlacementAnnealingState CurrentState = new PlacementAnnealingState(Vertices);
            PlacementAnnealingState BestState = CurrentState;
            CurrentState.RecomputeEdges();
            float CurrentEnergy = ComputeEnergy(CurrentState, Writer);
            float BestEnergy = CurrentEnergy;
            float CurrentTemperature = 1 / (float)System.Math.Pow(p_ParamsAnnealing.CoolingParameter, p_ParamsAnnealing.IterStageCount);
            float InitTemperature = CurrentTemperature;
            float ProbaCorrector = (float)System.Math.Log(p_ParamsAnnealing.ProbaParamFloor, System.Math.E) / (-p_ParamsAnnealing.ProbaParamEnergyPercent * CurrentEnergy);

            for (int Stage = 0; Stage != p_ParamsAnnealing.IterStageCount; Stage++)
            {
                for (int i = 0; i != p_ParamsAnnealing.IterTryCountByStageByVertex * Vertices.Count; i++)
                {
                    PlacementAnnealingState Neighbour = GetNeighbour(CurrentState, CurrentTemperature);
                    float NeighbourEnergy = ComputeEnergy(Neighbour, Writer);
                    if (NeighbourEnergy < BestEnergy)
                    {
                        BestState = Neighbour;
                        BestEnergy = NeighbourEnergy;
                    }
                    float Proba = (float)System.Math.Exp((CurrentEnergy - NeighbourEnergy) * ProbaCorrector * InitTemperature / CurrentTemperature);
                    float Rand = (float)p_Random.NextDouble();
                    if (Proba > Rand)
                    {
                        CurrentState = Neighbour;
                        CurrentEnergy = NeighbourEnergy;
                    }
                }
                CurrentTemperature = CurrentTemperature * p_ParamsAnnealing.CoolingParameter;
            }

            for (int i = 0; i != BestState.VertexPositions.Count; i++)
                Vertices[i].Visual.SetPosition(BestState.VertexPositions[i].X, BestState.VertexPositions[i].Y);

            Writer.Close();
        }
    }
}