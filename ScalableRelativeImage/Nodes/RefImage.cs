using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        class PresudoRoot : IContainer
        {
            public float RelativeWidth { get; set; } = 1;
            public float RelativeHeight { get; set; } = 1;

            public float RelativeArea => RelativeWidth * RelativeHeight;
            List<INode> Children = new();
            public void AddNode(INode node, ref List<ExecutionWarning> executionWarnings)
            {
                Children.Add(node);
            }

            public Dictionary<string, string> GetValueSet()
            {
                return null;
            }

            public List<INode> ListNodes()
            {
                return Children;
            }

            public void SetValue(string Key, string Value, ref List<ExecutionWarning> executionWarnings)
            {
            }
        }
        public override void Paint(ref Graphics TargetGraphics, RenderProfile profile)
        {
            var LT = profile.FindTargetPoint(X, Y);
            var rect = new System.Drawing.Rectangle(new System.Drawing.Point((int)LT.X, (int)LT.Y), new Size(
                    (int)(Width / profile.root.RelativeWidth * profile.TargetWidth), (int)(Height / profile.root.RelativeHeight * profile.TargetHeight)));
            var _rect = new System.Drawing.Rectangle(new System.Drawing.Point((int)LT.X, (int)LT.Y), new Size(
                    (int)(Width / profile.root.RelativeWidth * profile.TargetWidth * ScaledWidthRatio), (int)(Height / profile.root.RelativeHeight * profile.TargetHeight * ScaledHeightRatio)));
            if (Source.StartsWith("Ref:"))
            {
                var Name = Source.Substring(4);
                var sub = profile.Ref(Name);
                var __LT = profile.FindTargetPoint(sub.X, sub.Y);
                var ___rect = new System.Drawing.Rectangle(new System.Drawing.Point((int)__LT.X, (int)__LT.Y),
                    new Size((int)(sub.Width / profile.root.RelativeWidth * profile.TargetWidth), (int)(sub.Height / profile.root.RelativeHeight * profile.TargetHeight)));
                var wR = ((float)_rect.Width) / (float)___rect.Width;
                var hR = ((float)_rect.Height) / (float)___rect.Height;
                var pR = new PresudoRoot() { };
                {
                    pR.RelativeWidth = profile.root.RelativeWidth;
                    pR.RelativeHeight = profile.root.RelativeHeight;
                }
                //var p = profile.Copy(pR);
                var p = profile.Copy(profile.root);//Keep reference
                p.TargetWidth = profile.TargetWidth * wR;
                p.TargetHeight = profile.TargetHeight * hR;
                var _LT = p.FindTargetPoint(sub.X, sub.Y);
                var __rect = new System.Drawing.Rectangle(new System.Drawing.Point((int)_LT.X, (int)_LT.Y), new Size(
                        (int)(sub.Width / p.root.RelativeWidth * p.TargetWidth), (int)(sub.Height / p.root.RelativeHeight * p.TargetHeight)));
                Bitmap Bit = new Bitmap((int)p.TargetWidth, (int)p.TargetHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                Graphics g = Graphics.FromImage(Bit);
                if (Background is not null)
                    g.FillRectangle(new SolidBrush(Background.Value), new System.Drawing.Rectangle(0, 0, (int)p.TargetWidth, (int)p.TargetHeight));
                else
                    g.FillRectangle(new SolidBrush(p.DefaultBackground.Value), new System.Drawing.Rectangle(0, 0, (int)p.TargetWidth, (int)p.TargetHeight));
                g.SmoothingMode = profile.SmoothingMode;
                g.TextRenderingHint = profile.TextRenderingHint;
                g.InterpolationMode = profile.InterpolationMode;
                sub.Paint(ref g, p);
                TargetGraphics.DrawImage(Bit, _rect, __rect, GraphicsUnit.Pixel);
                g.Dispose();
                Bit.Dispose();

            }
            else
            {

                var sri = SRIEngine.Deserialize(profile.FindFile(Source));
                var p = profile.Copy(sri);
                if (Background is not null) p.DefaultBackground = Background;
                p.TargetWidth = rect.Width;
                p.TargetHeight = rect.Height;
                var img = sri.Render(p);
                TargetGraphics.DrawImage(img, rect, 0, 0, img.Width, img.Height, GraphicsUnit.Pixel);
                img.Dispose();
            }
        }
    }
}
