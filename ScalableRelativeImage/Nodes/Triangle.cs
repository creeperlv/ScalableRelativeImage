using ScalableRelativeImage.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScalableRelativeImage.Nodes
{
    public class Triangle : GraphicNode
    {
        public SRIFloatPoint Point1 = new();
        public SRIFloatPoint Point2 = new();
        public SRIFloatPoint Point3 = new();
        public float Size;
        public bool Fill = false;
        public IntermediateValue Foreground = null;

        public override Dictionary<string, string> GetValueSet()
        {
            Dictionary<string, string> dict = new();
            dict.Add("Point1", Point1.ToString());
            dict.Add("Point2", Point2.ToString());
            dict.Add("Point3", Point3.ToString());
            dict.Add("Size", Size.ToString());
            dict.Add("Fill", Fill.ToString());
            if (Foreground is not null)
                dict.Add("Color", Foreground.Value);
            return dict;
        }
        public override void SetValue(string Key, string Value, ref List<ExecutionWarning> executionWarnings)
        {
            switch (Key)
            {
                case "Point1":
                    Point1 = SRIFloatPoint.Parse(Value);
                    break;
                case "Point2":
                    Point2 = SRIFloatPoint.Parse(Value);
                    break;
                case "Point3":
                    Point3 = SRIFloatPoint.Parse(Value);
                    break;
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
                    base.SetValue(Key, Value, ref executionWarnings);
                    break;
            }
        }
        public override void Paint(ref Graphics TargetGraphics, RenderProfile profile)
        {
            float RealWidth = profile.FindAbsoluteSize(Size);
            Color Color;
            if (Foreground != null) Color = Foreground.GetColor(profile.CurrentSymbols, "#" + profile.DefaultForeground.Value.ToArgb().ToString("X"));
            else Color = profile.DefaultForeground.Value;
            List<PointF> Points = new();
            List<byte> Types = new();
            //foreach (var item in this.Points)
            //{
            //    var P = item as PathNode;
            //    Points.Add(profile.FindTargetPoint(P.X, P.Y));
            //    Types.Add((byte)(int)P.NodeType);
            //}
            {
                Points.Add(profile.FindTargetPoint(Point1.X, Point1.Y));
                Types.Add((byte)PathPointType.Line);
            }
            {
                Points.Add(profile.FindTargetPoint(Point2.X, Point2.Y));
                Types.Add((byte)PathPointType.Line);
            }
            {
                Points.Add(profile.FindTargetPoint(Point3.X, Point3.Y));
                Types.Add((byte)PathPointType.Line);
            }
            {
                Points.Add(profile.FindTargetPoint(Point1.X, Point1.Y));
                Types.Add((byte)PathPointType.Line);
            }
            if (Fill is false)
                TargetGraphics.DrawPath(new(Color, RealWidth), new(Points.ToArray(), Types.ToArray()));
            else
                TargetGraphics.FillPath(new SolidBrush(Color), new(Points.ToArray(), Types.ToArray()));
        }
    }
}
