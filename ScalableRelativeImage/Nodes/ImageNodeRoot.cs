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
        void AddNode(INode node);
        void SetValue(string Key, string Value);
        Dictionary<string, string> GetValueSet();
    }
    public class ImageNodeRoot : INode
    {
        public float RelativeWidth { get => _RelativeWidth; set { _RelativeWidth = value; RelativeArea = _RelativeHeight * _RelativeWidth; } }
        public float RelativeHeight { get => _RelativeHeight; set { _RelativeHeight = value; RelativeArea = _RelativeHeight * _RelativeWidth; } }
        internal float _RelativeWidth = 0;
        internal float _RelativeHeight = 0;
        internal float RelativeArea;
        public List<GraphicNode> Childs = new();
        internal ImageNodeRoot()
        {

        }

        public void AddNode(INode node)
        {
            Childs.Add(node as GraphicNode);
        }

        public List<INode> ListNodes()
        {
            List<INode> nodes = new List<INode>();
            foreach (var item in Childs)
            {
                nodes.Add(item);
            }
            return nodes;
        }

        public Bitmap Render(RenderProfile profile)
        {
            profile.root = this;
            Bitmap Bit = new Bitmap((int)profile.TargetWidth, (int)profile.TargetHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(Bit);
            g.FillRectangle(new SolidBrush(profile.DefaultBackground), new System.Drawing.Rectangle(0, 0, (int)profile.TargetWidth, (int)profile.TargetHeight));
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            foreach (var item in Childs)
            {
                item.Paint(ref g, profile);
            }

            return Bit;
        }
        public void SetValue(string Key, string Value)
        {
            switch (Key)
            {
                case "RelativeWidth":
                    _RelativeWidth = float.Parse(Value); RelativeArea = _RelativeHeight * _RelativeWidth;
                    break;
                case "RelativeHeight":
                    _RelativeHeight = float.Parse(Value); RelativeArea = _RelativeHeight * _RelativeWidth;
                    break;
                default:
                    break;
            }
        }

        public Dictionary<string, string> GetValueSet()
        {
            Dictionary<string, string> dict = new();
            dict.Add("RelativeWidth", _RelativeWidth.ToString());
            dict.Add("RelativeHeight", _RelativeHeight.ToString());
            return dict;
        }
    }
}
