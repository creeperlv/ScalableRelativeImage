using ScalableRelativeImage.Nodes;
using SRI.Core.Backend;
using SRI.Core.Core;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;

namespace ScalableRelativeImage
{
    /// <summary>
    /// Render profile, contains render target informations
    /// </summary>
    public class RenderProfile
    {
        public BaseBackendFactory FactoryInstance;
        public DrawableImage NewImage(int W,int H)
        {
            BaseBackendFactory.Instance = FactoryInstance;
            DrawableImage drawableImage = new DrawableImage();
            drawableImage.Init(W, H);
            return drawableImage;
        }
        public DrawableImage NewImage(string File)
        {
            BaseBackendFactory.Instance = FactoryInstance;
            DrawableImage drawableImage = new DrawableImage();
            drawableImage.Init(File);
            return drawableImage;
        }
        /// <summary>
        /// Environment that the SRI will render in.
        /// </summary>
        public SymbolHelper CurrentSymbols = new SymbolHelper();
        /// <summary>
        /// Target render width (absolute length);
        /// </summary>
        public float TargetWidth;
        /// <summary>
        /// Target render height (absolute length);
        /// </summary>
        public float TargetHeight;
        public ColorF? DefaultForeground = null;
        public ColorF? DefaultBackground = null;
        /// <summary>
        /// Controls how System.Drawing will perform interpolation.
        /// </summary>
        public InterpolationMode InterpolationMode = InterpolationMode.HighQualityBicubic;
        /// <summary>
        /// Controls text AA.
        /// </summary>
        public TextRenderingHint TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
        /// <summary>
        /// Controls smoothing
        /// </summary>
        public SmoothingMode SmoothingMode = SmoothingMode.HighQuality;
        /// <summary>
        /// Root node of the SRI for query other purpose.
        /// </summary>
        internal IContainer root;
        /// <summary>
        /// Working directory of the SRI file for referencing external files.
        /// </summary>
        public string WorkingDirectory = Environment.CurrentDirectory;
        /// <summary>
        /// Render options for CLUNL.Imaging library for blur option.
        /// </summary>
        public int RendererOptions = 3;
        /// <summary>
        /// The bipmap that is currently working on.
        /// </summary>
        public DrawableImage WorkingBitmap;
        /// <summary>
        /// Find a subimage.
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Make a copy of current render profile.
        /// </summary>
        /// <param name="Root"></param>
        /// <returns></returns>
        public RenderProfile Copy(IContainer Root)
        {
            RenderProfile renderProfile = new RenderProfile();
            renderProfile.DefaultBackground = DefaultBackground;
            renderProfile.DefaultForeground = DefaultForeground;
            renderProfile.TargetHeight = TargetHeight;
            renderProfile.TargetWidth = TargetWidth;
            renderProfile.root = Root;
            renderProfile.FactoryInstance = FactoryInstance;
            renderProfile.CurrentSymbols = CurrentSymbols;
            renderProfile.WorkingDirectory = this.WorkingDirectory;
            return renderProfile;
        }
        /// <summary>
        /// Try to find a relative file through current profile.
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public FileInfo FindFile(string FileName)
        {
            var path0 = System.IO.Path.Combine(WorkingDirectory, FileName);
            if (File.Exists(path0)) return new FileInfo(path0); else if (File.Exists(FileName)) return new FileInfo(FileName); else return null;
        }
        /// <summary>
        /// Find the absolute size of a relative size (2D vector: w * h).
        /// </summary>
        /// <param name="node"></param>
        /// <param name="W"></param>
        /// <param name="H"></param>
        /// <returns></returns>
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
        public UniversalVector2 FindTargetPointAsUniversalVector2(float RX, float RY, GraphicNode node = null)
        {
            UniversalVector2 p = new UniversalVector2() { X = ((RX / root.RelativeWidth) * TargetWidth), Y = ((RY / root.RelativeHeight) * TargetHeight) };
            if (node != null)
            {
                if (node.Parent != null)
                {
                    if (node.Parent is ILayoutable l)
                    {
                        p = new UniversalVector2()
                        {
                            X = ((RX + l.X) / root.RelativeWidth) * TargetWidth,
                            Y = (((RY + l.Y) / root.RelativeHeight) * TargetHeight)
                        };
                    }
                }
            }
            return p;
        }
        /// <summary>
        /// Find the absolute point of a relative point.
        /// </summary>
        /// <param name="RX"></param>
        /// <param name="RY"></param>
        /// <param name="node"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Find the absolute size of a relative size (diagonal length).
        /// </summary>
        /// <param name="node"></param>
        /// <param name="W"></param>
        /// <param name="H"></param>
        /// <returns></returns>
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
