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
        public Color? Foreground;
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
                if (Foreground.HasValue is true)
                    dict.Add("Color", "#" + Foreground.Value.ToArgb().ToString("X"));
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
                        Foreground = (Color)SRIAnalyzer.cc.ConvertFromString(Value);
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
            var LT=profile.FindTargetPoint(X, Y);
            if(Fill is not true)
            TargetGraphics.DrawEllipse(new((Foreground == null ? profile.DefaultForeground.Value : Foreground.Value), RealWidth), new System.Drawing.Rectangle(new System.Drawing.Point((int)LT.X, (int)LT.Y),new Size(
                (int)(Width / profile.root._RelativeWidth * profile.TargetWidth), (int)(Height / profile.root._RelativeHeight* profile.TargetHeight))));
            else
                TargetGraphics.FillEllipse(new SolidBrush((Foreground == null ? profile.DefaultForeground.Value : Foreground.Value)), new System.Drawing.Rectangle(new System.Drawing.Point((int)LT.X, (int)LT.Y), new Size(
    (int)(Width / profile.root._RelativeWidth * profile.TargetWidth), (int)(Height / profile.root._RelativeHeight * profile.TargetHeight))));

        }
    }
}
