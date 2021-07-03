using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScalableRelativeImage.Nodes
{
    public class Text : GraphicNode
    {
        public string Content = "";
        public string FontFamily = "";
        public string FontStyle = "Regular";
        public float RelativeFontSize = -12;
        public Color? Foreground = null;
        public float X;
        public float Y;
        public float Width;
        public float Height;
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
                        FontFamily = Value;
                    }
                    break;
                case "FontStyle":
                    {
                        FontStyle = Value;
                    }
                    break;
                case "Size":
                    {
                        RelativeFontSize = float.Parse(Value);
                    }
                    break;
                case "X":
                    {
                        X = float.Parse(Value);
                    }
                    break;
                case "Y":
                    {
                        Y = float.Parse(Value);
                    }
                    break;
                case "Width":
                    {
                        Width = float.Parse(Value);
                    }
                    break;
                case "Height":
                    {
                        Height = float.Parse(Value);
                    }
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
            Dictionary<string, string> result = new();
            result.Add("Content", Content);
            result.Add("FontFamily", FontFamily);
            result.Add("FontStyle", FontStyle);
            result.Add("Size", RelativeFontSize.ToString());
            result.Add("X", X.ToString());
            result.Add("Y", Y.ToString());
            result.Add("Width", Width.ToString());
            result.Add("Height", Height.ToString());
            if (Foreground is not null)
                if (Foreground.HasValue is true)
                    result.Add("Color", "#" + Foreground.Value.ToArgb().ToString("X"));
            return result;
        }
        public override void Paint(ref Graphics TargetGraphics, RenderProfile profile)
        {
            var p0 = profile.FindTargetPoint(X, Y);
            var AW = Width / profile.root.RelativeWidth * profile.TargetWidth;
            var AH = Height / profile.root.RelativeHeight * profile.TargetHeight;
            float FS = RelativeFontSize > 0 ? RelativeFontSize * (profile.TargetHeight / profile.root.RelativeWidth) : -RelativeFontSize;
            TargetGraphics.DrawString(Content,
                new Font(FontFamily,FS, (FontStyle)Enum.Parse(typeof(FontStyle), FontStyle))
                , new SolidBrush((Foreground == null ? profile.DefaultForeground.Value : Foreground.Value)), new RectangleF(p0, new SizeF(AW, AH)));
        }
    }
}
