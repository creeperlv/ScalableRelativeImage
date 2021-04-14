using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScalableRelativeImage.Nodes
{
    public interface INode
    {
        List<INode> ListNodes();
        void AddNode(INode node, ref List<ExecutionWarning> executionWarnings);
        void SetValue(string Key, string Value, ref List<ExecutionWarning> executionWarnings);
        Dictionary<string, string> GetValueSet();
    }
    public class ImageNodeRoot : INode
    {
        public float RelativeWidth { get => _RelativeWidth; set { _RelativeWidth = value; RelativeArea = _RelativeHeight * _RelativeWidth; } }
        public float RelativeHeight { get => _RelativeHeight; set { _RelativeHeight = value; RelativeArea = _RelativeHeight * _RelativeWidth; } }
        public Color PreferredForeground { get => _PreferredForeground; set => _PreferredForeground = value; }
        public Color PreferredBackground { get => _PreferredForeground; set => _PreferredForeground = value; }
        internal float _RelativeWidth = 0;
        internal float _RelativeHeight = 0;
        internal Color _PreferredBackground = Color.White;
        internal Color _PreferredForeground = Color.Transparent;
        internal float RelativeArea;
        public List<GraphicNode> Children = new();
        public ImageNodeRoot()
        {

        }

        public void AddNode(INode node, ref List<ExecutionWarning> executionWarnings)
        {
            Children.Add(node as GraphicNode);
        }

        public List<INode> ListNodes()
        {
            List<INode> nodes = new List<INode>();
            foreach (var item in Children)
            {
                nodes.Add(item);
            }
            return nodes;
        }

        public Bitmap Render(RenderProfile profile)
        {
            profile.root = this;
            if (profile.DefaultBackground == null)
            {
                profile.DefaultBackground = _PreferredBackground;
            }
            if (profile.DefaultForeground == null)
            {
                profile.DefaultForeground = _PreferredForeground;
            }
            Bitmap Bit = new Bitmap((int)profile.TargetWidth, (int)profile.TargetHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(Bit);
            g.FillRectangle(new SolidBrush(profile.DefaultBackground.Value), new System.Drawing.Rectangle(0, 0, (int)profile.TargetWidth, (int)profile.TargetHeight));
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            foreach (var item in Children)
            {
                item.Paint(ref g, profile);
            }

            return Bit;
        }
        public void SetValue(string Key, string Value, ref List<ExecutionWarning> executionWarnings)
        {
            switch (Key)
            {
                case "RelativeWidth":
                    _RelativeWidth = float.Parse(Value); RelativeArea = _RelativeHeight * _RelativeWidth;
                    break;
                case "RelativeHeight":
                    _RelativeHeight = float.Parse(Value); RelativeArea = _RelativeHeight * _RelativeWidth;
                    break;
                case "Foreground":
                    _PreferredForeground = (Color)SRIAnalyzer.cc.ConvertFromString(Value);
                    break;
                case "Background":
                    _PreferredBackground = (Color)SRIAnalyzer.cc.ConvertFromString(Value);
                    break;
                default:
                    executionWarnings.Add(new DataDisposedWarning(Key, Value));
                    break;
            }
        }

        public Dictionary<string, string> GetValueSet()
        {
            Dictionary<string, string> dict = new();
            dict.Add("RelativeWidth", _RelativeWidth.ToString());
            dict.Add("RelativeHeight", _RelativeHeight.ToString());
            
            dict.Add("Foreground", "#" + _PreferredForeground.ToArgb().ToString("X"));
            dict.Add("Background", "#" + _PreferredBackground.ToArgb().ToString("X"));
            
            return dict;
        }
    }
}
