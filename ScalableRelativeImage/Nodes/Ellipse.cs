﻿using ScalableRelativeImage.Core;
using SRI.Core.Backend;
using System.Collections.Generic;

namespace ScalableRelativeImage.Nodes
{
    /// <summary>
    /// Draw a ellipse.
    /// </summary>
    public class Ellipse : GraphicNode
    {
        public IntermediateValue X = 0;
        public IntermediateValue Y = 0;
        public IntermediateValue Width = 0;
        public IntermediateValue Height = 0;
        public IntermediateValue Size = 0;
        public IntermediateValue Fill = false;
        public IntermediateValue Foreground = null;
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
                    X.Value = Value;
                    break;
                case "Y":
                    Y.Value = Value;
                    break;
                case "Width":
                    Width.Value = Value;
                    break;
                case "Height":
                    Height.Value = Value;
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
            var LT = profile.FindTargetPoint(X.GetFloat(profile.CurrentSymbols), Y.GetFloat(profile.CurrentSymbols));
            ColorF Color;
            if (Foreground != null) Color = Foreground.GetColor(profile.CurrentSymbols, "#" + profile.DefaultForeground.Value.ToString("X"));
            else Color = profile.DefaultForeground.Value;
            bool b = Fill.GetBool(profile.CurrentSymbols);
            TargetGraphics.DrawEllipse(Color, LT.X, LT.Y, 
                (Width.GetFloat(profile.CurrentSymbols) / profile.root.RelativeWidth * profile.TargetWidth),
                (int)(Height.GetFloat(profile.CurrentSymbols) / profile.root.RelativeHeight * profile.TargetHeight), RealWidth, b);
        }
    }
}
