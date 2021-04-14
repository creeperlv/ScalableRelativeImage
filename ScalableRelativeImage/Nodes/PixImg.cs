using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScalableRelativeImage.Nodes
{
    public class PixImg : GraphicNode
    {
        public float X;
        public float Y;
        public float Width;
        public float Height;
        public Color? Foreground;
        public string Source = "";
        public override Dictionary<string, string> GetValueSet()
        {
            Dictionary<string, string> dict = new();
            dict.Add("X", X.ToString());
            dict.Add("Y", Y.ToString());
            dict.Add("Width", Width.ToString());
            dict.Add("Height", Height.ToString());
            dict.Add("Source", Source.ToString());
            if (Foreground is not null)
                if (Foreground.HasValue is true)
                    dict.Add("Color", "#" + Foreground.Value.ToArgb().ToString("X"));
            return dict;
        }
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
                case "Width":
                    Width = float.Parse(Value);
                    break;
                case "Height":
                    Height = float.Parse(Value);
                    break;
                case "Color":
                    {
                        Foreground = (Color)SRIAnalyzer.cc.ConvertFromString(Value);
                    }
                    break;
                case "Source":
                    {
                        Source = Value;
                    }
                    break;
                default:
                    base.SetValue(Key, Value, ref executionWarnings);
                    break;
            }
        }
        public override void Paint(ref Graphics TargetGraphics, RenderProfile profile)
        {
            var img = Bitmap.FromFile(Source);
            var LT = profile.FindTargetPoint(X, Y);
            var rect = new System.Drawing.Rectangle(new System.Drawing.Point((int)LT.X, (int)LT.Y), new Size(
                    (int)(Width / profile.root._RelativeWidth * profile.TargetWidth), (int)(Height / profile.root._RelativeHeight * profile.TargetHeight)));
            TargetGraphics.DrawImage(img, rect, 0, 0, img.Width, img.Height, GraphicsUnit.Pixel);

        }
    }
}
