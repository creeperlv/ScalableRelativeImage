using ScalableRelativeImage.Core;
using SRI.Core.Core;
using System.Collections.Generic;
using System.Drawing;

namespace ScalableRelativeImage.Nodes
{
    public class CloseCurve : GraphicNode
    {
        public IntermediateValue Size = 0;
        public IntermediateValue Foreground = null;
        public IntermediateValue Fill = false;
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
                        Foreground = new IntermediateValue();
                        Foreground.Value = Value;
                    }
                    break;
                default:
                    base.SetValue(Key, Value,ref executionWarnings);
                    break;
            }
        }
        public override Dictionary<string, string> GetValueSet()
        {
            Dictionary<string, string> dict = new()
            {
                { "Size", Size.ToString() },
                { "Fill", Fill.ToString() }
            };
            if (Foreground is not null)
                dict.Add("Color", Foreground.Value);
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
        public override void Paint(ref DrawableImage TargetGraphics, RenderProfile profile)
        {
            float RealWidth = profile.FindAbsoluteSize(Size.GetFloat(profile.CurrentSymbols));
            List<PointF> Points = new();
            foreach (var item in this.Points)
            {
                var P = item as Point;
                Points.Add(profile.FindTargetPoint(P.X.GetFloat(profile.CurrentSymbols), P.Y.GetFloat(profile.CurrentSymbols)));
            }
            Color Color;
            if (Foreground != null) Color = Foreground.GetColor(profile.CurrentSymbols, "#" + profile.DefaultForeground.Value.ToArgb().ToString("X"));
            else Color = profile.DefaultForeground.Value;
            //if (Fill.GetBool(profile.CurrentSymbols) is false)
            //    TargetGraphics.DrawClosedCurve(new(Color, RealWidth), Points.ToArray());
            //else
            //    TargetGraphics.FillClosedCurve(new SolidBrush(Color), Points.ToArray());
        }
    }
}