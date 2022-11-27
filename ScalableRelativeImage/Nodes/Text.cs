using Microsoft.VisualBasic;
using ScalableRelativeImage.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScalableRelativeImage.Nodes
{
    /// <summary>
    /// Render a text
    /// </summary>
    public class Text : GraphicNode
    {
        public IntermediateValue Content = "";
        public IntermediateValue FontFamily = "Arial";
        public IntermediateValue FontStyle = "Regular";
        public IntermediateValue RelativeFontSize = -12;
        public IntermediateValue Foreground = null;
        public IntermediateValue X = 0;
        public IntermediateValue Y = 0;
        public IntermediateValue Width = 0;
        public IntermediateValue Height = 0;
        public StringAlignment Align = StringAlignment.Near;
        public StringAlignment VerticalAlign = StringAlignment.Near;

        public override void SetValue(string Key, string Value, ref List<ExecutionWarning> executionWarnings)
        {
            switch (Key)
            {
                case "Content":
                    {
                        Content = Value;
                    }
                    break;
                case "FontFamily":
                    {
                        FontFamily.Value = Value;
                    }
                    break;
                case "FontStyle":
                    {
                        FontStyle.Value = Value;
                    }
                    break;
                case "Size":
                    {
                        RelativeFontSize.Value=Value;
                    }
                    break;
                case "X":
                    {
                        X.Value= Value;
                    }
                    break;
                case "Y":
                    {
                        Y.Value = Value;
                    }
                    break;
                case "Width":
                    {
                        Width.Value = Value;
                    }
                    break;
                case "Height":
                    {
                        Height.Value = Value;
                    }
                    break;
                case "Color":
                    {
                        Foreground = new();
                        Foreground.Value = Value;
                    }
                    break;
                case "Align":
                    {
                        Align = Enum.Parse<StringAlignment>(Value);
                    }
                    break;
                case "VerticalAlign":
                    {
                        VerticalAlign = Enum.Parse<StringAlignment>(Value);
                    }
                    break;
                default:
                    base.SetValue(Key, Value, ref executionWarnings);
                    break;
            }
        }
        public override Dictionary<string, string> GetValueSet()
        {
            Dictionary<string, string> result = new()
            {
                { "Content", Content.ToString() },
                { "FontFamily", FontFamily.ToString() },
                { "FontStyle", FontStyle.ToString() },
                { "Size", RelativeFontSize.ToString() },
                { "X", X.ToString() },
                { "Y", Y.ToString() },
                { "Width", Width.ToString() },
                { "Height", Height.ToString() },
                { "Align", Align.ToString() },
                { "VerticalAlign", VerticalAlign.ToString() }
            };
            if (Foreground is not null)
                result.Add("Color", Foreground.Value);
            return result;
        }
        public override void Paint(ref Graphics TargetGraphics, RenderProfile profile)
        {
            var p0 = profile.FindTargetPoint(X.GetFloat(profile.CurrentSymbols), Y.GetFloat(profile.CurrentSymbols));
            var AW = Width.GetFloat(profile.CurrentSymbols) / profile.root.RelativeWidth * profile.TargetWidth;
            var AH = Height.GetFloat(profile.CurrentSymbols) / profile.root.RelativeHeight * profile.TargetHeight;
            float FS =  RelativeFontSize.GetFloat(profile.CurrentSymbols) > 0 ? profile.FindAbsoluteSize(RelativeFontSize.GetFloat(profile.CurrentSymbols)) : -RelativeFontSize.GetFloat(profile.CurrentSymbols);
            Color Color;
            if (Foreground != null) Color = Foreground.GetColor(profile.CurrentSymbols, "#" + profile.DefaultForeground.Value.ToArgb().ToString("X"));
            else Color = profile.DefaultForeground.Value;
            TargetGraphics.DrawString(Content.GetString(profile.CurrentSymbols),
                new Font(FontFamily.GetString(profile.CurrentSymbols, "Arial"), FS, (FontStyle)Enum.Parse(typeof(FontStyle), FontStyle.GetString(profile.CurrentSymbols)))
                , new SolidBrush(Color), new RectangleF(p0, new SizeF(AW, AH)), new StringFormat { Alignment = Align,LineAlignment= VerticalAlign });
        }
    }
}
