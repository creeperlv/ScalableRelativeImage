using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScalableRelativeImage.Nodes
{
    public class SubImage : GraphicNode
    {
        public List<GraphicNode> Children = new();
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
        public void RemoveChildAt(int i)
        {
            Children.RemoveAt(i);
        }
        public void RemoveChild(INode n)
        {
            Children.Remove(n as GraphicNode);
        }
        public override List<INode> ListNodes()
        {
            List<INode> nodes = new List<INode>();
            foreach (var item in Children)
            {
                nodes.Add(item);
            }
            return nodes;
        }
        public override void AddNode(INode node, ref List<ExecutionWarning> executionWarnings)
        {
            if (node is GraphicNode)
                Children.Add(node as GraphicNode);
            else executionWarnings.Add(new ShapeMismatchWarning(node, typeof(GraphicNode)));
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
            var LT = profile.FindTargetPoint(X, Y);
            var rect = new System.Drawing.Rectangle(new System.Drawing.Point((int)LT.X, (int)LT.Y), new Size(
                    (int)(Width / profile.root._RelativeWidth * profile.TargetWidth), (int)(Height / profile.root._RelativeHeight * profile.TargetHeight)));
            //Bitmap Bit = new Bitmap((int)profile.TargetWidth, (int)profile.TargetHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            var img = new Bitmap(rect.Width, rect.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            var subProfile = profile.Copy(this);
            subProfile.TargetWidth = rect.Width;
            subProfile.TargetHeight = rect.Height;
            Graphics g = Graphics.FromImage(img);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            foreach (var item in Children)
            {
                item.Paint(ref g, profile);
            }
            TargetGraphics.DrawImage(img, rect, 0, 0, img.Width, img.Height, GraphicsUnit.Pixel);

        }
    }
}
