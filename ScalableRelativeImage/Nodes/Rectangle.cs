using ScalableRelativeImage.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScalableRelativeImage.Nodes
{
    public class Rectangle : GraphicNode
    {
        public IntermediateValue X;
        public IntermediateValue Y;
        public IntermediateValue Width;
        public IntermediateValue Height;
        public IntermediateValue Size;
        public bool Fill = false;
        public IntermediateValue Foreground=null;
        public override Dictionary<string, string> GetValueSet()
        {
            Dictionary<string, string> dict = new()
            {
                { "X", X.ToString() },
                { "Y", Y.ToString() },
                { "Width", Width.ToString() },
                { "Height", Height.ToString() },
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
                    X = new IntermediateValue { Value = Value };
                    break;
                case "Y":
                    Y = new IntermediateValue { Value = Value };
                    break;
                case "Width":
                    Width = new IntermediateValue { Value = Value };
                    break;
                case "Height":
                    Height = new IntermediateValue { Value = Value };
                    break;
                case "Size":
                    Size = new IntermediateValue { Value = Value };
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
            float RealWidth = profile.FindAbsoluteSize(Size.GetFloat(profile.CurrentSymbols));
            var LT = profile.FindTargetPoint(X.GetFloat(profile.CurrentSymbols), Y.GetFloat(profile.CurrentSymbols));
            Color Color;
            if (Foreground != null) Color = Foreground.GetColor(profile.CurrentSymbols, "#" + profile.DefaultForeground.Value.ToArgb().ToString("X"));
            else Color = profile.DefaultForeground.Value;
            if (Fill is not true)
                TargetGraphics.DrawRectangle(new(Color, RealWidth), new System.Drawing.Rectangle(new System.Drawing.Point((int)LT.X, (int)LT.Y),
                    new Size((int)(Width.GetFloat(profile.CurrentSymbols) / profile.root.RelativeWidth * profile.TargetWidth), (int)(Height.GetFloat(profile.CurrentSymbols) / profile.root.RelativeHeight * profile.TargetHeight))));
            else
                TargetGraphics.FillRectangle(new SolidBrush(Color), new System.Drawing.Rectangle(new System.Drawing.Point((int)LT.X, (int)LT.Y),
                    new Size((int)(Width.GetFloat(profile.CurrentSymbols) / profile.root.RelativeWidth * profile.TargetWidth), (int)(Height.GetFloat(profile.CurrentSymbols) / profile.root.RelativeHeight * profile.TargetHeight))));

        }
    }
}
