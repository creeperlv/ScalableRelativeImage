﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScalableRelativeImage.Nodes
{
    public class SubImage : GraphicNode, IContainer
    {
        public List<GraphicNode> Children = new();
        public float X;
        public float Y;
        public float Width;
        public float Height;
        public float ScaledWidthRatio = 1f;
        public float ScaledHeightRatio = 1f;
        public Color? Background;
        public string Name = null;
        public float Rotation = 0;
        public float RelativeWidth { get => Width; set => throw new NotImplementedException(); }
        public float RelativeHeight { get => Height; set => throw new NotImplementedException(); }

        public float RelativeArea => Width * Height;

        public override Dictionary<string, string> GetValueSet()
        {
            Dictionary<string, string> dict = new();
            dict.Add("X", X.ToString());
            dict.Add("Y", Y.ToString());
            dict.Add("Width", Width.ToString());
            dict.Add("Height", Height.ToString());
            dict.Add("ScaledWidthRatio", ScaledWidthRatio.ToString());
            dict.Add("ScaledHeightRatio", ScaledHeightRatio.ToString());
            dict.Add("Rotation", Rotation.ToString());
            if (Name is not null)
            {
                dict.Add("Name", Name);
            }
            if (Background is not null)
                if (Background.HasValue is true)
                    dict.Add("Background", "#" + Background.Value.ToArgb().ToString("X"));
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
                case "ScaledHeightRatio":
                    ScaledHeightRatio = float.Parse(Value);
                    break;
                case "ScaledWidthRatio":
                    ScaledWidthRatio = float.Parse(Value);
                    break;
                case "Rotation":
                    Rotation = float.Parse(Value);
                    break;
                case "Background":
                    {
                        Background = (Color)SRIAnalyzer.cc.ConvertFromString(Value);
                    }
                    break;
                case "Name":
                    {
                        Name = Value;
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
                    (int)(Width / profile.root.RelativeWidth * profile.TargetWidth), (int)(Height / profile.root.RelativeHeight * profile.TargetHeight)));
            var _rect = new System.Drawing.Rectangle(new System.Drawing.Point((int)LT.X, (int)LT.Y), new Size(
                    (int)(Width / profile.root.RelativeWidth * profile.TargetWidth * ScaledWidthRatio), (int)(Height / profile.root.RelativeHeight * profile.TargetHeight * ScaledHeightRatio)));
            //Bitmap Bit = new Bitmap((int)profile.TargetWidth, (int)profile.TargetHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            var b = new Bitmap(rect.Width, rect.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            var subProfile = profile.Copy(this);
            subProfile.TargetWidth = rect.Width;
            subProfile.TargetHeight = rect.Height;
            {
                Graphics g = Graphics.FromImage(b);
                {
                    g.SmoothingMode = profile.SmoothingMode;
                    g.TextRenderingHint = profile.TextRenderingHint;
                    g.InterpolationMode = profile.InterpolationMode;
                    foreach (var item in Children)
                    {
                        item.Paint(ref g, profile);
                    }
                    if (Background is not null)
                    {
                        TargetGraphics.FillRectangle(new SolidBrush(Background.Value), _rect);
                    }
                    g.Dispose();
                }
            }
            if (Rotation == 0 || Rotation == 360)
            {
                TargetGraphics.DrawImage(b, _rect, 0, 0, b.Width, b.Height, GraphicsUnit.Pixel);
            }
            else
            {
                var The = MathHelper.Deg2Rad_P(Rotation);
                int W = (int)(Math.Abs(b.Width * Math.Cos(The)) + Math.Abs(b.Height * Math.Sin(The)));
                int H = (int)(Math.Abs(b.Height * Math.Cos(The)) + Math.Abs(b.Width * Math.Sin(The)));
                Bitmap FB = new Bitmap(W, H);
                using (Graphics g = Graphics.FromImage(FB))
                {
                    g.SmoothingMode = profile.SmoothingMode;
                    g.TextRenderingHint = profile.TextRenderingHint;
                    g.InterpolationMode = profile.InterpolationMode;
                    g.TranslateTransform((float)W / 2, (float)H / 2);
                    g.RotateTransform(Rotation);
                    g.TranslateTransform(-(float)W / 2, -(float)H / 2);
                    g.DrawImage(b, (W - b.Width) / 2, (H - b.Height) / 2);
                    g.TranslateTransform((float)W / 2, (float)H / 2);
                    g.RotateTransform(-Rotation);
                    g.TranslateTransform(-(float)W / 2, -(float)H / 2);
                }
                int _W = (int)(W / profile.root.RelativeWidth * profile.TargetWidth * ScaledWidthRatio);
                int _H = (int)(H / profile.root.RelativeHeight * profile.TargetHeight * ScaledHeightRatio);
                _rect = new System.Drawing.Rectangle(new System.Drawing.Point((int)(LT.X-(_W- _rect.Width)/2), (int)(LT.Y-(_H-_rect.Height)/2)), new Size(_W,_H));
                TargetGraphics.DrawImage(FB, _rect, 0, 0, W,H, GraphicsUnit.Pixel);
            }
            b.Dispose();
        }
    }
}
