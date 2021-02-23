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
    public class Line : GraphicNode
    {
        public float StartX=0;
        public float StartY=0;
        public float EndX=0;
        public float EndY=0;
        public float Size=0;
        public Color? Foreground=null;
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
                        ColorConverter cc = new ColorConverter();
                        Foreground=(Color)cc.ConvertFromString(Value);
                    }
                    break;
                default:
                    base.SetValue(Key, Value);
                    break;
            }
        }
        public override void Paint(ref Graphics TargetGraphics, RenderProfile profile)
        {
            Console.WriteLine($"Properties:SX={StartX},SY={StartY},EX={EndX},EY={EndY}");
            float RealWidth = (Size / (root.RelativeArea)) * (profile.TargetWidth * profile.TargetHeight);
            TargetGraphics.DrawLine(new Pen((Foreground==null?profile.DefaultForeground:Foreground.Value), RealWidth), profile.FindTargetPoint(StartX, StartY), profile.FindTargetPoint(EndX, EndY));
        }
    }
}