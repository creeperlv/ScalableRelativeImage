using ScalableRelativeImage.Core;
using SRI.Core.Backend;
using SRI.Core.Core;
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
        public IntermediateValue Size = new IntermediateValue { Value = "0" };
        public IntermediateValue Fill = false;
        public IntermediateValue Foreground = null;

        public override Dictionary<string, string> GetValueSet()
        {
            Dictionary<string, string> dict = new()
            {
                { "Point1", Point1.ToString() },
                { "Point2", Point2.ToString() },
                { "Point3", Point3.ToString() },
                { "Size", Size.ToString() },
                { "Fill", Fill.ToString() }
            };
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
                    Size.Value = Value;
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
        public override void Paint(ref DrawableImage TargetGraphics, RenderProfile profile)
        {
            float RealWidth = profile.FindAbsoluteSize(Size.GetFloat(profile.CurrentSymbols));
            ColorF Color;
            if (Foreground != null) Color = Foreground.GetColor(profile.CurrentSymbols, "#" + profile.DefaultForeground.Value.ToString("X"));
            else Color = profile.DefaultForeground.Value;
            List<UniversalVector2> Points = new();
            List<byte> Types = new();
            //foreach (var item in this.Points)
            //{
            //    var P = item as PathNode;
            //    Points.Add(profile.FindTargetPoint(P.X, P.Y));
            //    Types.Add((byte)(int)P.NodeType);
            //}
            {
                Points.Add(profile.FindTargetPointAsUniversalVector2(Point1.X.GetFloat(profile.CurrentSymbols), Point1.Y.GetFloat(profile.CurrentSymbols)));
                Types.Add((byte)PathPointType.Line);
            }
            {
                Points.Add(profile.FindTargetPointAsUniversalVector2(Point2.X.GetFloat(profile.CurrentSymbols), Point2.Y.GetFloat(profile.CurrentSymbols)));
                Types.Add((byte)PathPointType.Line);
            }
            {
                Points.Add(profile.FindTargetPointAsUniversalVector2(Point3.X.GetFloat(profile.CurrentSymbols), Point3.Y.GetFloat(profile.CurrentSymbols)));
                Types.Add((byte)PathPointType.Line);
            }
            {
                Points.Add(profile.FindTargetPointAsUniversalVector2(Point1.X.GetFloat(profile.CurrentSymbols), Point1.Y.GetFloat(profile.CurrentSymbols)));
                Types.Add((byte)PathPointType.Line);
            }
            //if (Fill.GetBool(profile.CurrentSymbols, false) is false)
            TargetGraphics.DrawPath(Color, Points.ToArray(), Types.ToArray(), RealWidth, Fill.GetBool(profile.CurrentSymbols, false));
        }
    }
}
