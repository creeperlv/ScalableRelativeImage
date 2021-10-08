using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScalableRelativeImage.Nodes
{
    public class Dot : GraphicNode
    {
        public float X;
        public float Y;
        public float Size;
        public Color? Foreground = null;
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
                case "Size":
                    Size = float.Parse(Value);
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
        public override Dictionary<string, string> GetValueSet()
        {
            Dictionary<string, string> dict = new();
            dict.Add("X", X.ToString());
            dict.Add("Y", Y.ToString());
            dict.Add("Size", Size.ToString());
            if (Foreground is not null)
                if (Foreground.HasValue is true)
                    dict.Add("Color", "#" + Foreground.Value.ToArgb().ToString("X"));
            return dict;
        }
        public override void Paint(ref Graphics TargetGraphics, RenderProfile profile)
        {
            float _S = profile.FindAbsoluteSize(Size);
            var LT = profile.FindTargetPoint(X, Y);
            float D = _S / MathF.Sqrt(2);
            float R = D / 2;
            TargetGraphics.FillEllipse(new SolidBrush((Foreground == null ? profile.DefaultForeground.Value : Foreground.Value)), LT.X - R, LT.Y - R, D,  D);
        }
    }
}
