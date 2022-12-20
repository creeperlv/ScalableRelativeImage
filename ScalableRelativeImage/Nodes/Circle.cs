using ScalableRelativeImage.Core;
using SRI.Core.Backend;
using System.Collections.Generic;

namespace ScalableRelativeImage.Nodes
{
    /// <summary>
    /// Draw a circle.
    /// </summary>
    public class Circle : GraphicNode
    {
        public IntermediateValue X = 0;
        public IntermediateValue Y = 0;
        public IntermediateValue Radius = 0;
        public IntermediateValue Size = 0;
        public IntermediateValue Fill = false;
        public IntermediateValue Foreground = null;
        public override Dictionary<string, string> GetValueSet()
        {
            Dictionary<string, string> dict = new()
            {
                { "X", X.ToString() },
                { "Y", Y.ToString() },
                { "Radius", Radius.ToString() },
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
                case "X":
                    X.Value = Value;
                    break;
                case "Y":
                    Y.Value = Value;
                    break;
                case "Radius":
                    Radius.Value = Value;
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
            float _Radius=Radius.GetFloat(profile.CurrentSymbols);
            float _X=X.GetFloat(profile.CurrentSymbols);
            float _Y=Y.GetFloat(profile.CurrentSymbols);
            var LT = profile.FindTargetPoint(_X-_Radius, _Y-_Radius);
            ColorF Color;
            if (Foreground != null) Color = Foreground.GetColor(profile.CurrentSymbols, "#" + profile.DefaultForeground.Value.ToString("X"));
            else Color = profile.DefaultForeground.Value;
            bool b = Fill.GetBool(profile.CurrentSymbols);
            TargetGraphics.DrawEllipse(Color, LT.X, LT.Y,
                (_Radius * 2 / profile.root.RelativeWidth * profile.TargetWidth),
                (_Radius * 2 / profile.root.RelativeHeight * profile.TargetHeight), 
                RealWidth, b);
        }
    }
}
