using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScalableRelativeImage.Nodes
{
    public class Curve : GraphicNode
    {
        public float Size = 0;
        public Color? Foreground = null;
        public List<INode> Points = new List<INode>();

        public override void SetValue(string Key, string Value, ref List<ExecutionWarning> executionWarnings)
        {
            switch (Key)
            {
                case "Size":
                    Size = float.Parse(Value);
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
            else executionWarnings.Add(new ShapeMismatchWarning(node, typeof(Point)));
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
            TargetGraphics.DrawCurve(new((Foreground == null ? profile.DefaultForeground : Foreground.Value), RealWidth), Points.ToArray());

        }
    }
}
