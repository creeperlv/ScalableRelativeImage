using ScalableRelativeImage.Nodes;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;

namespace ScalableRelativeImage
{
    public class RenderProfile
    {
        public SymbolHelper CurrentSymbols = new SymbolHelper();
        public float TargetWidth;
        public float TargetHeight;
        public Color? DefaultForeground = null;
        public Color? DefaultBackground = null;
        public InterpolationMode InterpolationMode = InterpolationMode.HighQualityBicubic;
        public TextRenderingHint TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
        public SmoothingMode SmoothingMode = SmoothingMode.HighQuality;
        internal IContainer root;
        public string WorkingDirectory = Environment.CurrentDirectory;
        public SubImage Ref(string Name)
        {
            return Ref(root, Name);
        }
        SubImage Ref(INode node, string Name)
        {
            var l = node.ListNodes();
            if (l is not null)
            {
                foreach (var item in l)
                {
                    var d =
                    item.GetValueSet();
                    if (item is SubImage)
                    {

                        if (d.ContainsKey("Name"))
                        {
                            if (d["Name"] == Name)
                            {
                                return item as SubImage;
                            }
                        }
                    }
                    else if (item is IContainer || item is Group)
                    {
                        var a = Ref(item, Name);
                        if (a is not null) return a;
                    }
                }
            }
            return null;
        }
        public RenderProfile Copy(IContainer Root)
        {
            RenderProfile renderProfile = new RenderProfile();
            renderProfile.DefaultBackground = DefaultBackground;
            renderProfile.DefaultForeground = DefaultForeground;
            renderProfile.TargetHeight = TargetHeight;
            renderProfile.TargetWidth = TargetWidth;
            renderProfile.root = Root;
            renderProfile.CurrentSymbols = CurrentSymbols;
            renderProfile.WorkingDirectory = Environment.CurrentDirectory;
            return renderProfile;
        }
        public FileInfo FindFile(string FileName)
        {
            var path0 = System.IO.Path.Combine(WorkingDirectory, FileName);
            if (File.Exists(path0)) return new FileInfo(path0); else if (File.Exists(FileName)) return new FileInfo(FileName); else return null;
        }
        public SizeF FindSize(GraphicNode node, float W, float H)
        {
            if (node.Parent == null)
            {
                return new SizeF(W / root.RelativeWidth * TargetWidth, H / root.RelativeHeight * TargetHeight);
            }
            else
            {
                SizeF s = new SizeF(W / root.RelativeWidth * TargetWidth, H / root.RelativeHeight * TargetHeight);
                if (node.Parent is IScalable sc)
                {
                    s.Width *= sc.ScaledW;
                    s.Height *= sc.ScaledH;
                }
                return s;
            }
        }
        public PointF FindTargetPoint(float RX, float RY, GraphicNode node = null)
        {
            PointF p = new(((RX / root.RelativeWidth) * TargetWidth), ((RY / root.RelativeHeight) * TargetHeight));
            if (node != null)
            {
                if (node.Parent != null)
                {
                    if (node.Parent is ILayoutable l)
                    {
                        p = new((((RX + l.X) / root.RelativeWidth) * TargetWidth), (((RY + l.Y) / root.RelativeHeight) * TargetHeight));
                    }
                }
            }
            return p;
        }
        public float FindAbsoluteSize(float RelativeSize, GraphicNode node = null)
        {
            if (RelativeSize > 0)
            {
                if (node != null)
                {
                    if (node.Parent != null)
                    {
                        if (node.Parent is IScalable s)
                        {

                            return (RelativeSize / (MathF.Sqrt(root.RelativeArea) * MathF.Sqrt(s.ScaledH * s.ScaledW))) * MathF.Sqrt(TargetWidth * TargetHeight);
                        }
                    }
                }
                return (RelativeSize / MathF.Sqrt(root.RelativeArea)) * MathF.Sqrt(TargetWidth * TargetHeight);
            }
            else
            {
                return Math.Abs(RelativeSize);
            }
        }
    }
}
