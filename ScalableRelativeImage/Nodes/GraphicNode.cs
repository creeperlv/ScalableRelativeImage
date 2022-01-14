using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace ScalableRelativeImage.Nodes
{
    /// <summary>
    /// Basic Node, implemented INode.
    /// </summary>
    public class GraphicNode : INode
    {
        public ImageNodeRoot root;
        public virtual void AddNode(INode node,ref List<ExecutionWarning> executionWarnings)
        {
            executionWarnings.Add(new ShapeDisposedWarning(node));
#if DEBUG
            Trace.WriteLine($"Node has been disposed.");
#endif
        }

        public virtual Dictionary<string, string> GetValueSet()
        {
            return null;
        }

        public virtual List<INode> ListNodes()
        {
            return null;
        }

        public virtual void Paint(ref Graphics TargetGraphics, RenderProfile profile)
        {

        }

        public virtual void SetValue(string Key, string Value, ref List<ExecutionWarning> executionWarnings)
        {
            executionWarnings.Add(new DataDisposedWarning(Key, Value));
#if DEBUG
            Trace.WriteLine($"Value with key \"{Key}\" has been disposed.");
#endif
        }
    }
    public class CloseCurve : GraphicNode
    {
        public float Size = 0;
        public Color? Foreground = null;
        public bool Fill = false;
        public List<INode> Points = new List<INode>();

        public override void SetValue(string Key, string Value, ref List<ExecutionWarning> executionWarnings)
        {
            switch (Key)
            {
                case "Size":
                    Size = float.Parse(Value);
                    break;
                case "Fill":
                    Fill = bool.Parse(Value);
                    break;
                case "Color":
                    {
                        Foreground = (Color)SRIAnalyzer.cc.ConvertFromString(Value);
                    }
                    break;
                default:
                    base.SetValue(Key, Value,ref executionWarnings);
                    break;
            }
        }
        public override Dictionary<string, string> GetValueSet()
        {
            Dictionary<string, string> dict = new();
            dict.Add("Size", Size.ToString());
            dict.Add("Fill", Fill.ToString());
            if (Foreground is not null)
                if (Foreground.HasValue is true)
                    dict.Add("Color", "#" + Foreground.Value.ToArgb().ToString("X"));
            return dict;
        }
        public override void AddNode(INode node, ref List<ExecutionWarning> executionWarnings)
        {
            if (node is Point)
            {
                Points.Add(node);
            }
            else
            {
                executionWarnings.Add(new ShapeMismatchWarning(node, typeof(Point)));
            }
        }
        public override List<INode> ListNodes()
        {
            return Points;
        }
        public override void Paint(ref Graphics TargetGraphics, RenderProfile profile)
        {
            float RealWidth = profile.FindAbsoluteSize(Size);
            List<PointF> Points = new();
            foreach (var item in this.Points)
            {
                var P = item as Point;
                Points.Add(profile.FindTargetPoint(P.X, P.Y));
            }
            if (Fill is false)
                TargetGraphics.DrawClosedCurve(new((Foreground == null ? profile.DefaultForeground.Value : Foreground.Value), RealWidth), Points.ToArray());
            else
                TargetGraphics.FillClosedCurve(new SolidBrush((Foreground == null ? profile.DefaultForeground.Value : Foreground.Value)), Points.ToArray());
        }
    }
}