using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace ScalableRelativeImage.Nodes
{
    public class GraphicNode : INode
    {
        public ImageNodeRoot root;
        public virtual void AddNode(INode node)
        {
#if DEBUG
            Trace.WriteLine($"Node has been disposed.");
#endif
        }

        public virtual Dictionary<string, string> GetValueSet()
        {
            return null;
        }

        public virtual List<INode> ListNodes()
        {
            return null;
        }

        public virtual void Paint(ref Graphics TargetGraphics, RenderProfile profile)
        {

        }

        public virtual void SetValue(string Key, string Value)
        {
#if DEBUG
            Trace.WriteLine($"Value with key \"{Key}\" has been disposed.");
#endif
        }
    }
    public class CloseCurve : GraphicNode
    {
        public float Size = 0;
        public Color? Foreground = null;
        public bool Fill = false;
        public List<INode> Points = new List<INode>();

        public override void SetValue(string Key, string Value)
        {
            switch (Key)
            {
                case "Size":
                    Size = float.Parse(Value);
                    break;
                case "Fill":
                    Fill = bool.Parse(Value);
                    break;
                case "Color":
                    {
                        Foreground = (Color)SRIAnalyzer.cc.ConvertFromString(Value);
                    }
                    break;
                default:
                    base.SetValue(Key, Value);
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
                    dict.Add("Color", "#" + Foreground.Value.ToArgb().ToString("X"));
            return dict;
        }
        public override void AddNode(INode node)
        {
            if (node is Point)
            {
                Points.Add(node);
            }
        }
        public override List<INode> ListNodes()
        {
            return Points;
        }
        public override void Paint(ref Graphics TargetGraphics, RenderProfile profile)
        {
            float RealWidth = profile.FindAbsoluteSize(Size);
            List<PointF> Points = new();
            foreach (var item in this.Points)
            {
                var P = item as Point;
                Points.Add(profile.FindTargetPoint(P.X, P.Y));
            }
            if (Fill is false)
                TargetGraphics.DrawClosedCurve(new((Foreground == null ? profile.DefaultForeground : Foreground.Value), RealWidth), Points.ToArray());
            else
                TargetGraphics.FillClosedCurve(new SolidBrush((Foreground == null ? profile.DefaultForeground : Foreground.Value)), Points.ToArray());
        }
    }
    public class Point : GraphicNode
    {
        public float X;
        public float Y;
        public override void SetValue(string Key, string Value)
        {
            switch (Key)
            {
                case "X":
                    X = float.Parse(Value);
                    break;
                case "Y":
                    Y = float.Parse(Value);
                    break;
                default:
                    base.SetValue(Key, Value);
                    break;
            }
        }
        public override Dictionary<string, string> GetValueSet()
        {
            Dictionary<string, string> dict = new();
            dict.Add("X", X.ToString());
            dict.Add("Y", Y.ToString());
            return dict;
        }
    }
    public class Line : GraphicNode
    {
        public float StartX = 0;
        public float StartY = 0;
        public float EndX = 0;
        public float EndY = 0;
        public float Size = 0;
        public Color? Foreground = null;
        public override Dictionary<string, string> GetValueSet()
        {
            Dictionary<string, string> dict = new();
            dict.Add("StartX", StartX.ToString());
            dict.Add("StartY", StartY.ToString());
            dict.Add("EndX", EndX.ToString());
            dict.Add("EndY", EndY.ToString());
            dict.Add("Size", Size.ToString());
            if (Foreground is not null)
                if (Foreground.HasValue is true)
                    dict.Add("Color", "#" + Foreground.Value.ToArgb().ToString("X"));
            return dict;
        }
        public override void SetValue(string Key, string Value)
        {
            switch (Key)
            {
                case "StartX":
                    StartX = float.Parse(Value);
                    break;
                case "StartY":
                    StartY = float.Parse(Value);
                    break;
                case "EndX":
                    EndX = float.Parse(Value);
                    break;
                case "EndY":
                    EndY = float.Parse(Value);
                    break;
                case "Size":
                    Size = float.Parse(Value);
                    break;
                case "Color":
                    {
                        Foreground = (Color)SRIAnalyzer.cc.ConvertFromString(Value);
                    }
                    break;
                default:
                    base.SetValue(Key, Value);
                    break;
            }
        }
        public override void Paint(ref Graphics TargetGraphics, RenderProfile profile)
        {
            float RealWidth = profile.FindAbsoluteSize(Size);
            TargetGraphics.DrawLine(new Pen((Foreground == null ? profile.DefaultForeground : Foreground.Value), RealWidth), profile.FindTargetPoint(StartX, StartY), profile.FindTargetPoint(EndX, EndY));
        }
    }
}