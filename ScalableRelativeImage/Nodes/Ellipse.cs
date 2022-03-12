using ScalableRelativeImage.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScalableRelativeImage.Nodes
{
    public class Ellipse : GraphicNode
    {
        public float X;
        public float Y;
        public float Width;
        public float Height;
        public float Size;
        public bool Fill = false;
        public IntermediateValue Foreground = null;
        public override Dictionary<string, string> GetValueSet()
        {
            Dictionary<string, string> dict = new();
            dict.Add("X", X.ToString());
            dict.Add("Y", Y.ToString());
            dict.Add("Width", Width.ToString());
            dict.Add("Height", Height.ToString());
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
                case "X":
                    X = float.Parse(Value);
                    break;
                case "Y":
                    Y = float.Parse(Value);
                    break;
                case "Width":
                    Width = float.Parse(Value);
                    break;
                case "Height":
                    Height = float.Parse(Value);
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
            var LT = profile.FindTargetPoint(X, Y);
            Color Color;
            if (Foreground != null) Color = Foreground.GetColor(profile.CurrentSymbols, "#" + profile.DefaultForeground.Value.ToArgb().ToString("X"));
            else Color = profile.DefaultForeground.Value;
            if (Fill is not true)
                TargetGraphics.DrawEllipse(new(Color, RealWidth), new System.Drawing.Rectangle(new System.Drawing.Point((int)LT.X, (int)LT.Y), new Size(
                    (int)(Width / profile.root.RelativeWidth * profile.TargetWidth), (int)(Height / profile.root.RelativeHeight * profile.TargetHeight))));
            else
                TargetGraphics.FillEllipse(new SolidBrush(Color), new System.Drawing.Rectangle(new System.Drawing.Point((int)LT.X, (int)LT.Y), 
                    new Size((int)(Width / profile.root.RelativeWidth * profile.TargetWidth), (int)(Height / profile.root.RelativeHeight * profile.TargetHeight))));
        }
    }
}
