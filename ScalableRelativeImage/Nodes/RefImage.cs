using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScalableRelativeImage.Nodes
{
    public class RefImage : GraphicNode
    {
        public float X;
        public float Y;
        public float Width;
        public float Height;
        public float ScaledWidthRatio = 1f;
        public float ScaledHeightRatio = 1f;
        public Color? Background;
        public string Source = "";
        public override Dictionary<string, string> GetValueSet()
        {
            Dictionary<string, string> dict = new();
            dict.Add("X", X.ToString());
            dict.Add("Y", Y.ToString());
            dict.Add("Width", Width.ToString());
            dict.Add("Height", Height.ToString());
            dict.Add("ScaledWidthRatio", ScaledWidthRatio.ToString());
            dict.Add("ScaledHeightRatio", ScaledHeightRatio.ToString());
            dict.Add("Source", Source.ToString());
            if (Background is not null)
                if (Background.HasValue is true)
                    dict.Add("Background", "#" + Background.Value.ToArgb().ToString("X"));
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
                case "ScaledHeightRatio":
                    ScaledHeightRatio = float.Parse(Value);
                    break;
                case "ScaledWidthRatio":
                    ScaledWidthRatio = float.Parse(Value);
                    break;
                case "Background":
                    {
                        Background = (Color)SRIAnalyzer.cc.ConvertFromString(Value);
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
            var LT = profile.FindTargetPoint(X, Y);
            var rect = new System.Drawing.Rectangle(new System.Drawing.Point((int)LT.X, (int)LT.Y), new Size(
                    (int)(Width / profile.root._RelativeWidth * profile.TargetWidth), (int)(Height / profile.root._RelativeHeight * profile.TargetHeight)));
            var _rect = new System.Drawing.Rectangle(new System.Drawing.Point((int)LT.X, (int)LT.Y), new Size(
                    (int)(Width / profile.root._RelativeWidth * profile.TargetWidth * ScaledWidthRatio), (int)(Height / profile.root._RelativeHeight * profile.TargetHeight * ScaledHeightRatio)));

            var sri = SRIEngine.Deserialize(profile.FindFile(Source));
            var p = profile.Copy(sri);
            p.TargetWidth = rect.Width;
            p.TargetHeight = rect.Height;
            var img = sri.Render(p);
            TargetGraphics.DrawImage(img, rect, 0, 0, img.Width, img.Height, GraphicsUnit.Pixel);

        }
    }
}
