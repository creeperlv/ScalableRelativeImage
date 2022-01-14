using ScalableRelativeImage.Core;
using System.Collections.Generic;
using System.Drawing;

namespace ScalableRelativeImage.Nodes
{
    public class Line : GraphicNode
    {
        public float StartX = 0;
        public float StartY = 0;
        public float EndX = 0;
        public float EndY = 0;
        public float Size = 0;
        public IntermediateValue Foreground=null;
        public override Dictionary<string, string> GetValueSet()
        {
            Dictionary<string, string> dict = new();
            dict.Add("StartX", StartX.ToString());
            dict.Add("StartY", StartY.ToString());
            dict.Add("EndX", EndX.ToString());
            dict.Add("EndY", EndY.ToString());
            dict.Add("Size", Size.ToString());
            if (Foreground is not null)
                dict.Add("Color", Foreground.Value);
            return dict;
        }
        public override void SetValue(string Key, string Value, ref List<ExecutionWarning> executionWarnings)
        {
            switch (Key)
            {
                case "StartX":
                    StartX = float.Parse(Value);
                    break;
                case "StartY":
                    StartY = float.Parse(Value);
                    break;
                case "EndX":
                    EndX = float.Parse(Value);
                    break;
                case "EndY":
                    EndY = float.Parse(Value);
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
                    base.SetValue(Key, Value,ref executionWarnings);
                    break;
            }
        }
        public override void Paint(ref Graphics TargetGraphics, RenderProfile profile)
        {
            Color Color;
            if (Foreground != null) Color = Foreground.GetColor(profile.CurrentSymbols, "#" + profile.DefaultForeground.Value.ToArgb().ToString("X"));
            else Color = profile.DefaultForeground.Value;
            float RealWidth = profile.FindAbsoluteSize(Size);
            TargetGraphics.DrawLine(new Pen(Color, RealWidth), profile.FindTargetPoint(StartX, StartY), profile.FindTargetPoint(EndX, EndY));
        }
    }
}