using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScalableRelativeImage.Nodes
{
    /// <summary>
    /// Node interface, contains all methods that requires to serialize/deserialize SRI Image.
    /// </summary>
    public interface INode
    {
        List<INode> ListNodes();
        /// <summary>
        /// If the node is unable to hold children, the node should dispose all passed nodes and add a "ShapeDisposedWarning" warning.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="executionWarnings"></param>
        void AddNode(INode node, ref List<ExecutionWarning> executionWarnings);
        /// <summary>
        /// If certain key is not applicable to the node, the node should dispose the data and add a "DataDisposedWarning" warning.
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        /// <param name="executionWarnings"></param>
        void SetValue(string Key, string Value, ref List<ExecutionWarning> executionWarnings);
        Dictionary<string, string> GetValueSet();
    }
    public interface IPivottable
    {
        float PivotX { get; set; }
        float PivotY { get; set; }
    }
    public interface IScalable
    {
        float ScaledW { get; set; }
        float ScaledH { get; set; }
    }
    public interface ILayoutable
    {
        float X { get; set; }
        float Y { get; set; }
    }
    /// <summary>
    /// Relative Container interface.
    /// </summary>
    public interface IContainer : INode
    {
        float RelativeWidth { get; set; }
        float RelativeHeight { get; set; }
        float RelativeArea { get; }
    }
    /// <summary>
    /// Image Node Root, root node of all real nodes. Contains some basic infromation of the image.
    /// </summary>
    public class ImageNodeRoot : IContainer
    {
        public float RelativeWidth { get => _RelativeWidth; set { _RelativeWidth = value; RelativeArea = _RelativeHeight * _RelativeWidth; } }
        public float RelativeHeight { get => _RelativeHeight; set { _RelativeHeight = value; RelativeArea = _RelativeHeight * _RelativeWidth; } }
        public Color PreferredForeground { get => _PreferredForeground; set => _PreferredForeground = value; }
        public Color PreferredBackground { get => _PreferredForeground; set => _PreferredForeground = value; }
        internal float _RelativeWidth = 0;
        internal float _RelativeHeight = 0;
        internal Color _PreferredBackground = Color.White;
        internal Color _PreferredForeground = Color.Transparent;
        public SymbolHelper Symbols = new SymbolHelper();
        public float RelativeArea { get; internal set; }
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
            profile.CurrentSymbols = Symbols;
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
            g.SmoothingMode = profile.SmoothingMode;
            g.TextRenderingHint = profile.TextRenderingHint;
            g.InterpolationMode = profile.InterpolationMode;
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
