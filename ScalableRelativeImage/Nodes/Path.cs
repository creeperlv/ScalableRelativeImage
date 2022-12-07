using ScalableRelativeImage.Core;
using SRI.Core.Backend;
using SRI.Core.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScalableRelativeImage.Nodes
{
    public enum PathNodeType
    {
        Bezier = 3, Bezier3 = 3, CloseSubpath = 128, DashMode = 16, Line = 1, PathMarker = 32, PathTypeMask = 7, Start = 0
    }
    public class PathNode : GraphicNode
    {
        public IntermediateValue X="0";
        public IntermediateValue Y="0";
        public PathNodeType NodeType = PathNodeType.Line;

        public override Dictionary<string, string> GetValueSet()
        {
            Dictionary<string, string> _result = new()
            {
                { "X", X.ToString() },
                { "Y", X.ToString() },
                { "Type", NodeType.ToString() }
            };
            return _result;
        }

        public override List<INode> ListNodes()
        {
            return null;
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
                case "Type":
                    {
                        NodeType = (PathNodeType)Enum.Parse(typeof(PathNodeType), Value);
                    }
                    break;
                default:
                    base.SetValue(Key, Value, ref executionWarnings);
                    break;
            }
        }
    }
    public class Path : GraphicNode
    {

        public IntermediateValue Size = "0";
        public IntermediateValue Foreground = null;
        public bool Fill = false;
        public List<INode> Points = new List<INode>();

        public override void SetValue(string Key, string Value, ref List<ExecutionWarning> executionWarnings)
        {
            switch (Key)
            {
                case "Size":
                    Size.Value = Value;
                    break;
                case "Fill":
                    Fill = bool.Parse(Value);
                    break;
                case "Color":
                    {
                        Foreground = new IntermediateValue();
                        Foreground.Value= Value;
                    }
                    break;
                default:
                    base.SetValue(Key, Value, ref executionWarnings);
                    break;
            }
        }
        public override Dictionary<string, string> GetValueSet()
        {
            Dictionary<string, string> dict = new();
            dict.Add("Size", Size.ToString());
            dict.Add("Fill", Fill.ToString());
            if (Foreground is not null)
                    dict.Add("Color", Foreground.ToString());
            return dict;
        }
        public override void AddNode(INode node, ref List<ExecutionWarning> executionWarnings)
        {
            if (node is PathNode)
            {
                Points.Add(node);
            }
            else executionWarnings.Add(new ShapeMismatchWarning(node, typeof(PathNode)));
        }
        public override List<INode> ListNodes()
        {
            return Points;
        }
        public override void Paint(ref DrawableImage TargetGraphics, RenderProfile profile)
        {
            float RealWidth = profile.FindAbsoluteSize(Size.GetFloat(profile.CurrentSymbols));
            ColorF Color;
            if (Foreground != null) Color = Foreground.GetColor(profile.CurrentSymbols, "#" + profile.DefaultForeground.Value.ToString("X"));
            else Color = profile.DefaultForeground.Value;

            List<UniversalVector2> Points = new();
            List<byte> Types = new();
            foreach (var item in this.Points)
            {
                var P = item as PathNode;
                Points.Add(profile.FindTargetPointAsUniversalVector2(P.X.GetFloat(profile.CurrentSymbols), P.Y.GetFloat(profile.CurrentSymbols)));
                Types.Add((byte)(int)P.NodeType);
            }
            //if (Fill is false)
                TargetGraphics.DrawPath(Color,Points.ToArray(), Types.ToArray(),RealWidth,Fill);
            //else
            //    TargetGraphics.FillPath(new SolidBrush(Color), new(Points.ToArray(), Types.ToArray()));
        }
    }
}
