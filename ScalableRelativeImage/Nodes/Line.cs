using ScalableRelativeImage.Core;
using System.Collections.Generic;
using System.Drawing;

namespace ScalableRelativeImage.Nodes
{
    /// <summary>
    /// A line from x0,y0 to x1,y1 with s size.
    /// </summary>
    public class Line : GraphicNode
    {
        public IntermediateValue StartX = new IntermediateValue() { Value = "0" };
        public IntermediateValue StartY = new IntermediateValue() { Value = "0" };
        public IntermediateValue EndX = new IntermediateValue() { Value = "0" };
        public IntermediateValue EndY = new IntermediateValue() { Value = "0" };
        public IntermediateValue Size = new IntermediateValue() { Value = "0" };
        public IntermediateValue Foreground = null;
        public override Dictionary<string, string> GetValueSet()
        {
            Dictionary<string, string> dict = new()
            {
                { "StartX", StartX.ToString() },
                { "StartY", StartY.ToString() },
                { "EndX", EndX.ToString() },
                { "EndY", EndY.ToString() },
                { "Size", Size.ToString() }
            };
            if (Foreground is not null)
                dict.Add("Color", Foreground.Value);
            return dict;
        }
        public override void SetValue(string Key, string Value, ref List<ExecutionWarning> executionWarnings)
        {
            switch (Key)
            {
                case "StartX":
                    StartX.Value=Value;
                    break;
                case "StartY":
                    StartY.Value = Value;
                    break;
                case "EndX":
                    EndX.Value = Value;
                    break;
                case "EndY":
                    EndY.Value = Value;
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
        public override void Paint(ref Graphics TargetGraphics, RenderProfile profile)
        {
            Color Color;
            if (Foreground != null) Color = Foreground.GetColor(profile.CurrentSymbols, "#" + profile.DefaultForeground.Value.ToArgb().ToString("X"));
            else Color = profile.DefaultForeground.Value;
            float RealWidth = profile.FindAbsoluteSize(Size.GetFloat(profile.CurrentSymbols));
            TargetGraphics.DrawLine(new Pen(Color, RealWidth),
                profile.FindTargetPoint(StartX.GetFloat(profile.CurrentSymbols),StartY.GetFloat(profile.CurrentSymbols)), 
                profile.FindTargetPoint(EndX.GetFloat(profile.CurrentSymbols), EndY.GetFloat(profile.CurrentSymbols)));
        }
    }
}