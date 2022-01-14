using ScalableRelativeImage.Core;
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
        public IntermediateValue Foreground = null;
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
                        Foreground = new IntermediateValue();
                        Foreground.Value = Value;
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
                dict.Add("Color", Foreground.Value);
            return dict;
        }
        public override void Paint(ref Graphics TargetGraphics, RenderProfile profile)
        {
            float _S = profile.FindAbsoluteSize(Size);
            var LT = profile.FindTargetPoint(X, Y);
            float D = _S / MathF.Sqrt(2);
            float R = D / 2;
            Color Color;
            if (Foreground != null) Color = Foreground.GetColor(profile.CurrentSymbols, "#" + profile.DefaultForeground.Value.ToArgb().ToString("X"));
            else Color = profile.DefaultForeground.Value;
            TargetGraphics.FillEllipse(new SolidBrush(Color), LT.X - R, LT.Y - R, D,  D);
        }
    }
}
