namespace Hime.Kernel.Graph
{
    public abstract class VertexVisual
    {
        protected float p_X;
        protected float p_Y;
        protected float p_BoundingWidth;
        protected float p_BoundingHeight;
        protected System.Drawing.Pen p_MaterialContourPen;
        protected System.Drawing.Brush p_MaterialContentBrush;

        public float X { get { return p_X; } }
        public float Y { get { return p_Y; } }
        public System.Drawing.PointF Location { get { return new System.Drawing.PointF(p_X, p_Y); } }
        public System.Drawing.PointF Center { get { return new System.Drawing.PointF(p_X + p_BoundingWidth / 2, p_Y + p_BoundingHeight / 2); } }
        public virtual float WrappingBoxLeft { get { return p_X - 20; } }
        public virtual float WrappingBoxRight { get { return p_X + p_BoundingWidth + 20; } }
        public virtual float WrappingBoxTop { get { return p_Y - 20; } }
        public virtual float WrappingBoxBottom { get { return p_Y + p_BoundingHeight + 20; } }
        public virtual float WrappingWidth { get { return p_BoundingWidth + 40; } }
        public virtual float WrappingHeight { get { return p_BoundingHeight + 40; } }
        public virtual float BoundingWidth { get { return p_BoundingWidth; } }
        public virtual float BoundingHeight { get { return p_BoundingHeight; } }
        public virtual float BoundingRay { get { return (float)System.Math.Sqrt(p_BoundingWidth * p_BoundingWidth + p_BoundingHeight * p_BoundingHeight) / 2; } }
        public System.Drawing.Pen MaterialContourPen
        {
            get { return p_MaterialContourPen; }
            set { p_MaterialContourPen = value; }
        }
        public System.Drawing.Brush MaterialContentBrush
        {
            get { return p_MaterialContentBrush; }
            set { p_MaterialContentBrush = value; }
        }

        public VertexVisual()
        {
            p_MaterialContourPen = new System.Drawing.Pen(System.Drawing.Color.Black);
            p_MaterialContentBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Transparent);
        }

        public void SetPosition(float X, float Y) { p_X = X; p_Y = Y; }
        public void Translate(float OffsetX, float OffsetY) { p_X += OffsetX; p_Y += OffsetY; }
        public abstract void Draw(GraphVisual Material);
        public abstract System.Drawing.PointF GetConnectorForEdgeTo(VertexVisual Target);
    }


    public class VertexVisualRectangle : VertexVisual
    {
        public VertexVisualRectangle(float Width, float Height)
        {
            p_BoundingWidth = Width;
            p_BoundingHeight = Height;
        }

        public override void Draw(GraphVisual Material)
        {
            Material.Graphic.FillRectangle(p_MaterialContentBrush, p_X, p_Y, p_BoundingWidth, p_BoundingHeight);
            Material.Graphic.DrawRectangle(p_MaterialContourPen, p_X, p_Y, p_BoundingWidth, p_BoundingHeight);
        }
        public override System.Drawing.PointF GetConnectorForEdgeTo(VertexVisual Target)
        {
            if (this != Target)
            {
                float VectorX = Target.Center.X - Center.X;
                float VectorY = Target.Center.Y - Center.Y;
                float ConnectorBeginX = 0;
                float ConnectorBeginY = 0;
                if (System.Math.Abs(VectorX) > System.Math.Abs(VectorY))
                {
                    ConnectorBeginY = Center.Y;
                    if (VectorX > 0)
                        ConnectorBeginX = p_X + p_BoundingWidth;
                    else
                        ConnectorBeginX = p_X;
                }
                else
                {
                    ConnectorBeginX = Center.X;
                    if (VectorY > 0)
                        ConnectorBeginY = p_Y + p_BoundingHeight;
                    else
                        ConnectorBeginY = p_Y;
                }
                return new System.Drawing.PointF(ConnectorBeginX, ConnectorBeginY);
            }
            else
            {
                return new System.Drawing.PointF(Center.X, p_Y);
            }
        }
    }

    public class VertexVisualCircle : VertexVisual
    {
        public override float BoundingRay { get { return p_BoundingWidth / 2; } }

        public VertexVisualCircle(float Ray)
        {
            p_BoundingWidth = Ray * 2;
            p_BoundingHeight = p_BoundingWidth;
        }

        public override void Draw(GraphVisual Material)
        {
            Material.Graphic.FillEllipse(p_MaterialContentBrush, p_X, p_Y, p_BoundingWidth, p_BoundingHeight);
            Material.Graphic.DrawEllipse(p_MaterialContourPen, p_X, p_Y, p_BoundingWidth, p_BoundingHeight);
        }
        public override System.Drawing.PointF GetConnectorForEdgeTo(VertexVisual Target)
        {
            if (this != Target)
            {
                float VectorX = Target.Center.X - Center.X;
                float VectorY = Target.Center.Y - Center.Y;
                float Cos = VectorX / PlacementTools.GetDistance(Center, Target.Center);
                double Angle = System.Math.Acos(Cos);
                if (VectorY < 0) Angle = -Angle;
                return new System.Drawing.PointF(Center.X + p_BoundingWidth * Cos / 2, Center.Y + p_BoundingWidth * (float)System.Math.Sin(Angle) / 2);
            }
            else
            {
                return new System.Drawing.PointF(Center.X, p_Y);
            }
        }
    }


    public enum EdgeVisualStyle
    {
        Straight,
        Curve
    }

    public abstract class EdgeVisual
    {
        protected EdgeVisualStyle p_Style;
        protected float p_EdgeCurveCoef;
        protected float p_EdgeCurveTension;
        protected System.Drawing.Pen p_MaterialContourPen;
        protected System.Drawing.Brush p_MaterialContentBrush;

        public EdgeVisualStyle Style
        {
            get { return p_Style; }
            set { p_Style = value; }
        }
        public System.Drawing.Pen MaterialContourPen
        {
            get { return p_MaterialContourPen; }
            set { p_MaterialContourPen = value; }
        }
        public System.Drawing.Brush MaterialContentBrush
        {
            get { return p_MaterialContentBrush; }
            set { p_MaterialContentBrush = value; }
        }

        public EdgeVisual()
        {
            p_Style = EdgeVisualStyle.Curve;
            p_EdgeCurveCoef = 20;
            p_EdgeCurveTension = 0.75f;
            p_MaterialContourPen = new System.Drawing.Pen(System.Drawing.Color.Black);
            p_MaterialContentBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Transparent);
        }

        public abstract void Draw(VertexVisual Begin, VertexVisual End, GraphVisual Material, string Value);
    }

    public class EdgeVisualArrow : EdgeVisual
    {
        protected float p_ArrowLength;
        protected float p_ArrowWidth;

        public float ArrowLength
        {
            get { return p_ArrowLength; }
            set { p_ArrowLength = value; }
        }
        public float ArrowWidth
        {
            get { return p_ArrowWidth; }
            set { p_ArrowWidth = value; }
        }

        public EdgeVisualArrow()
        {
            p_ArrowLength = 6;
            p_ArrowWidth = 12;
        }

        public override void Draw(VertexVisual Begin, VertexVisual End, GraphVisual Material, string Value)
        {
            if (Begin == End)
                Draw_Loop(Begin, Material, Value);
            else
                Draw_Edge(Begin, End, Material, Value);
        }
        private void Draw_Loop(VertexVisual Begin, GraphVisual Material, string Value)
        {
            System.Drawing.PointF Connector = Begin.GetConnectorForEdgeTo(Begin);
            float OffsetX = Connector.X - Begin.Center.X;
            float OffsetY = Connector.Y - Begin.Center.Y;
            float length = (float)System.Math.Sqrt(OffsetX * OffsetX + OffsetY * OffsetY);
            float ray = Material.FontSmall.Size * 0.75f;
            float LoopCenterX = Connector.X + ray * OffsetX / length;
            float LoopCenterY = Connector.Y + ray * OffsetY / length;
            Material.Graphic.DrawEllipse(p_MaterialContourPen, LoopCenterX - ray, LoopCenterY - ray, ray * 2, ray * 2);
            Material.Graphic.DrawString(Value, Material.FontSmall, new System.Drawing.SolidBrush(System.Drawing.Color.Blue), LoopCenterX - ray, LoopCenterY - ray);
        }
        private void Draw_Edge(VertexVisual Begin, VertexVisual End, GraphVisual Material, string Value)
        {
            System.Drawing.PointF PointBegin = Begin.GetConnectorForEdgeTo(End);
            System.Drawing.PointF PointEnd = End.GetConnectorForEdgeTo(Begin);
            System.Drawing.PointF PointConnector = PointEnd;
            // Calculate vector data
            float OffsetX = PointEnd.X - PointBegin.X;
            float OffsetY = PointEnd.Y - PointBegin.Y;
            float length = PlacementTools.GetDistance(PointBegin, PointEnd);
            // Reduce edge length at end and recompute vector data
            PointEnd = new System.Drawing.PointF(PointBegin.X + OffsetX * (length - p_ArrowLength) / length, PointBegin.Y + OffsetY * (length - p_ArrowLength) / length);
            OffsetX = PointEnd.X - PointBegin.X;
            OffsetY = PointEnd.Y - PointBegin.Y;
            length = PlacementTools.GetDistance(PointBegin, PointEnd);

            if (p_Style == EdgeVisualStyle.Curve)
            {
                // Compute point F
                float h = p_EdgeCurveCoef / (2 * length / (Begin.BoundingRay + End.BoundingRay));
                float Xf = PointBegin.X + h / (float)System.Math.Sqrt(1 + ((OffsetX * OffsetX) / (OffsetY * OffsetY)));
                float Yf = PointBegin.Y - OffsetX * (Xf - PointBegin.X) / OffsetY;
                float OffsetXf = Xf - PointBegin.X;
                float OffsetYf = Yf - PointBegin.Y;
                if (OffsetX * OffsetYf - OffsetY * OffsetXf < 0)
                {
                    Xf = PointBegin.X - h / (float)System.Math.Sqrt(1 + ((OffsetX * OffsetX) / (OffsetY * OffsetY)));
                    Yf = PointBegin.Y - OffsetX * (Xf - PointBegin.X) / OffsetY;
                    OffsetXf = Xf - PointBegin.X;
                    OffsetYf = Yf - PointBegin.Y;
                }
                Xf = PointBegin.X + OffsetX / 2 + OffsetXf;
                Yf = PointBegin.Y + OffsetY / 2 + OffsetYf;
                // Build curve
                System.Drawing.PointF[] Curve = new System.Drawing.PointF[3];
                Curve[0] = PointBegin;
                Curve[1] = new System.Drawing.PointF(Xf, Yf);
                Curve[2] = PointEnd;
                Material.Graphic.DrawCurve(p_MaterialContourPen, Curve, p_EdgeCurveTension);
                Material.Graphic.DrawString(Value, Material.FontSmall, new System.Drawing.SolidBrush(System.Drawing.Color.Blue), Curve[1]);
                Draw_EdgeArrow(Material, PointEnd, PointConnector);
            }
            else if (p_Style == EdgeVisualStyle.Straight)
            {
                // Compute label position
                float LabelX = PointBegin.X + OffsetX * 0.3f;
                float LabelY = PointBegin.Y + OffsetY * 0.3f;
                // Trace
                Material.Graphic.DrawLine(p_MaterialContourPen, PointBegin, PointEnd);
                Material.Graphic.DrawString(Value, Material.FontSmall, new System.Drawing.SolidBrush(System.Drawing.Color.Blue), LabelX, LabelY);
                Draw_EdgeArrow(Material, PointEnd, PointConnector);
            }
        }
        private void Draw_EdgeArrow(GraphVisual Material, System.Drawing.PointF Begin, System.Drawing.PointF End)
        {
            float OffsetX = End.X - Begin.X;
            float OffsetY = End.Y - Begin.Y;
            float XPart = (float)System.Math.Sqrt(p_ArrowWidth / (2 * (1 + ((OffsetX * OffsetX) / (OffsetY * OffsetY)))));
            float XF1 = Begin.X + XPart;
            float YF1 = Begin.Y + (OffsetX * (Begin.X - XF1) / OffsetY);
            float XF2 = Begin.X - XPart;
            float YF2 = Begin.Y + (OffsetX * (Begin.X - XF2) / OffsetY);
            Material.Graphic.DrawLine(p_MaterialContourPen, XF1, YF1, XF2, YF2);
            Material.Graphic.DrawLine(p_MaterialContourPen, XF1, YF1, End.X, End.Y);
            Material.Graphic.DrawLine(p_MaterialContourPen, XF2, YF2, End.X, End.Y);
        }
    }



    public abstract class GraphVisual
    {
        protected System.Drawing.Font p_FontNormal;
        protected System.Drawing.Font p_FontSmall;
        protected System.Drawing.StringFormat p_FormatLeftClip;
        protected System.Drawing.StringFormat p_FormatLeftNoClip;
        protected System.Drawing.StringFormat p_FormatCenterClip;
        protected System.Drawing.StringFormat p_FormatCenterNoClip;

        public abstract System.Drawing.Graphics Graphic { get; }
        public System.Drawing.Font FontNormal { get { return p_FontNormal; } }
        public System.Drawing.Font FontSmall { get { return p_FontSmall; } }
        public System.Drawing.StringFormat FormatLeftClip { get { return p_FormatLeftClip; } }
        public System.Drawing.StringFormat FormatLeftNoClip { get { return p_FormatLeftNoClip; } }
        public System.Drawing.StringFormat FormatCenterClip { get { return p_FormatCenterClip; } }
        public System.Drawing.StringFormat FormatCenterNoClip { get { return p_FormatCenterNoClip; } }

        public GraphVisual(string FontName, float FontHeight)
        {
            p_FontNormal = new System.Drawing.Font(FontName, FontHeight, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            p_FontSmall = new System.Drawing.Font(FontName, FontHeight - 4, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            p_FormatLeftClip = new System.Drawing.StringFormat();
            p_FormatLeftNoClip = new System.Drawing.StringFormat(System.Drawing.StringFormatFlags.NoClip);
            p_FormatCenterClip = new System.Drawing.StringFormat();
            p_FormatCenterNoClip = new System.Drawing.StringFormat(System.Drawing.StringFormatFlags.NoClip);
            p_FormatCenterClip.Alignment = System.Drawing.StringAlignment.Center;
            p_FormatCenterNoClip.Alignment = System.Drawing.StringAlignment.Center;
        }
    }

    public class GraphVisualImage : GraphVisual
    {
        protected System.Drawing.Bitmap p_Image;
        protected System.Drawing.Graphics p_Graphic;

        public System.Drawing.Bitmap Image { get { return p_Image; } }
        public override System.Drawing.Graphics Graphic { get { return p_Graphic; } }

        public GraphVisualImage(int Width, int Height, string FontName, float FontHeight, System.Drawing.Drawing2D.Matrix Transform) : base(FontName, FontHeight)
        {
            p_Image = new System.Drawing.Bitmap(Width, Height);
            p_Graphic = System.Drawing.Graphics.FromImage(p_Image);
            p_Graphic.Transform = Transform;
            p_Graphic.Clear(System.Drawing.Color.White);
            p_Graphic.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
        }
    }
}