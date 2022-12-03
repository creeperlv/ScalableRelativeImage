using ScalableRelativeImage.Core;
using SRI.Core.Core;
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
        public IntermediateValue X = new IntermediateValue { Value = "0" };
        public IntermediateValue Y = new IntermediateValue { Value = "0" };
        public IntermediateValue Size = new IntermediateValue { Value = "0" };
        public IntermediateValue Foreground = null;
        public override void SetValue(string Key, string Value, ref List<ExecutionWarning> executionWarnings)
        {
            switch (Key)
            {
                case "X":
                    X.Value = Value;
                    break;
                case "Y":
                    Y.Value = Value;
                    break;
                case "Size":
                    Size.Value = Value;
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
            Dictionary<string, string> dict = new()
            {
                { "X", X.ToString() },
                { "Y", Y.ToString() },
                { "Size", Size.ToString() }
            };
            if (Foreground is not null)
                dict.Add("Color", Foreground.Value);
            return dict;
        }
        public override void Paint(ref DrawableImage TargetGraphics, RenderProfile profile)
        {
            float _S = profile.FindAbsoluteSize(Size.GetFloat(profile.CurrentSymbols));
            var LT = profile.FindTargetPoint(X.GetFloat(profile.CurrentSymbols), Y.GetFloat(profile.CurrentSymbols));
            float D = _S / MathF.Sqrt(2);
            float R = D / 2;
            Color Color;
            if (Foreground != null) Color = Foreground.GetColor(profile.CurrentSymbols, "#" + profile.DefaultForeground.Value.ToArgb().ToString("X"));
            else Color = profile.DefaultForeground.Value;
            TargetGraphics.DrawEllipse(Color, LT.X - R, LT.Y - R, D, D, 0, true);
        }
    }
}
