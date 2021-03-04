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
        public float X;
        public float Y;
        public PathNodeType NodeType = PathNodeType.Line;

        public override Dictionary<string, string> GetValueSet()
        {
            Dictionary<string, string> _result = new();
            _result.Add("X", X.ToString());
            _result.Add("Y", X.ToString());
            _result.Add("Type", NodeType.ToString());
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
                    X = float.Parse(Value);
                    break;
                case "Y":
                    Y = float.Parse(Value);
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

        public float Size = 0;
        public Color? Foreground = null;
        public bool Fill = false;
        public List<INode> Points = new List<INode>();

        public override void SetValue(string Key, string Value, ref List<ExecutionWarning> executionWarnings)
        {
            switch (Key)
            {
                case "Size":
                    Size = float.Parse(Value);
                    break;
                case "Fill":
                    Fill = bool.Parse(Value);
                    break;
                case "Foreground":
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
            Dictionary<string, string> dict = new();
            dict.Add("Size", Size.ToString());
            dict.Add("Fill", Fill.ToString());
            if (Foreground is not null)
                if (Foreground.HasValue is true)
                    dict.Add("Foreground", "#" + Foreground.Value.ToArgb().ToString("X"));
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
        public override void Paint(ref Graphics TargetGraphics, RenderProfile profile)
        {
            float RealWidth = profile.FindAbsoluteSize(Size);
            List<PointF> Points = new();
            List<byte> Types = new();
            foreach (var item in this.Points)
            {
                var P = item as PathNode;
                Points.Add(profile.FindTargetPoint(P.X, P.Y));
                Types.Add((byte)(int)P.NodeType);
            }
            if (Fill is false)
                TargetGraphics.DrawPath(new((Foreground == null ? profile.DefaultForeground.Value : Foreground.Value), RealWidth), new(Points.ToArray(), Types.ToArray()));
            else
                TargetGraphics.FillPath(new SolidBrush((Foreground == null ? profile.DefaultForeground.Value : Foreground.Value)), new(Points.ToArray(), Types.ToArray()));
        }
    }
}
