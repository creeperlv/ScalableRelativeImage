using ScalableRelativeImage.Core;
using SRI.Core.Backend;
using SRI.Core.Utilities;
using SRI.Core.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScalableRelativeImage.Nodes
{
    /// <summary>
    /// SubImage: A SRI container.
    /// </summary>
    public class SubImage : GraphicNode, IContainer, ISoftCopyable<SubImage>
    {
        public List<GraphicNode> Children = new();
        public IntermediateValue X = 0;
        public IntermediateValue Y = 0;
        public float Width = 0;
        public float Height = 0;
        public IntermediateValue ScaledWidthRatio = 1f;
        public IntermediateValue ScaledHeightRatio = 1f;
        public ColorF? Background;
        public string Name = null;
        public IntermediateValue Rotation = 0;
        public float RelativeWidth { get => Width; set => throw new NotImplementedException(); }
        public float RelativeHeight { get => Height; set => throw new NotImplementedException(); }

        public float RelativeArea => Width * Height;

        public override Dictionary<string, string> GetValueSet()
        {
            Dictionary<string, string> dict = new()
            {
                { "X", X.ToString() },
                { "Y", Y.ToString() },
                { "Width", Width.ToString() },
                { "Height", Height.ToString() },
                { "ScaledWidthRatio", ScaledWidthRatio.ToString() },
                { "ScaledHeightRatio", ScaledHeightRatio.ToString() },
                { "Rotation", Rotation.ToString() }
            };
            if (Name is not null)
            {
                dict.Add("Name", Name);
            }
            if (Background is not null)
                if (Background.HasValue is true)
                    dict.Add("Background", "#" + Background.Value.ToString("X"));
            return dict;
        }
        public void RemoveChildAt(int i)
        {
            Children.RemoveAt(i);
        }
        public void RemoveChild(INode n)
        {
            var node = (n as GraphicNode);
            node.Parent = this;
            Children.Remove(node);
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
                    X.Value = Value;
                    break;
                case "Y":
                    Y.Value = Value;
                    break;
                case "Width":
                    Width = float.Parse(Value);
                    break;
                case "Height":
                    Height = float.Parse(Value);
                    break;
                case "ScaledHeightRatio":
                    ScaledHeightRatio.Value = Value;
                    break;
                case "ScaledWidthRatio":
                    ScaledWidthRatio.Value = Value;
                    break;
                case "Rotation":
                    Rotation.Value = Value;
                    break;
                case "Background":
                    {
                        Background = ((Color)SRIAnalyzer.cc.ConvertFromString(Value)).ToColorF();
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
        /// <summary>
        /// Create a copy of current render profile and render all children to a new bitmap picture then draw it to parent picture(Graphics).
        /// </summary>
        /// <param name="TargetGraphics"></param>
        /// <param name="profile"></param>
        public override void Paint(ref DrawableImage TargetGraphics, RenderProfile profile)
        {
            var LT = profile.FindTargetPoint(X.GetFloat(profile.CurrentSymbols), Y.GetFloat(profile.CurrentSymbols));
            var rect = new System.Drawing.Rectangle(new System.Drawing.Point((int)LT.X, (int)LT.Y), new Size(
                    (int)(Width / profile.root.RelativeWidth * profile.TargetWidth), (int)(Height / profile.root.RelativeHeight * profile.TargetHeight)));
            //Assumed size of the image.
            var _rect = new System.Drawing.Rectangle(new System.Drawing.Point((int)LT.X, (int)LT.Y), new Size(
                    (int)(Width / profile.root.RelativeWidth * profile.TargetWidth * ScaledWidthRatio.GetFloat(profile.CurrentSymbols)),
                    (int)(Height / profile.root.RelativeHeight * profile.TargetHeight * ScaledHeightRatio.GetFloat(profile.CurrentSymbols))));
            //Scaled size of the image.
            //Bitmap Bit = new Bitmap((int)profile.TargetWidth, (int)profile.TargetHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            DrawableImage b = profile.NewImage(rect.Width, rect.Height);
            //var b = new Bitmap(rect.Width, rect.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            var subProfile = profile.Copy(profile.root);
            //Keep the references.
            //subProfile.TargetWidth = rect.Width;
            //subProfile.TargetHeight = rect.Height;
            subProfile.WorkingBitmap = b;
            {
                //Graphics g = Graphics.FromImage(b);
                {
                    //g.SmoothingMode = profile.SmoothingMode;
                    //g.TextRenderingHint = profile.TextRenderingHint;
                    //g.InterpolationMode = profile.InterpolationMode;
                    foreach (var item in Children)
                    {
                        item.Paint(ref b, subProfile);
                    }
                    if (Background is not null)
                    {
                        TargetGraphics.DrawRectangle(Background.Value, (int)LT.X, (int)LT.Y,
                            (int)(Width / profile.root.RelativeWidth * profile.TargetWidth * ScaledWidthRatio.GetFloat(profile.CurrentSymbols)),
                    (int)(Height / profile.root.RelativeHeight * profile.TargetHeight * ScaledHeightRatio.GetFloat(profile.CurrentSymbols)), 0, true);
                    }
                    //g.Dispose();
                }
            }
            if (Rotation.GetFloat(profile.CurrentSymbols) == 0 || Rotation.GetFloat(profile.CurrentSymbols) == 360)
            {
                TargetGraphics.DrawImage(b, _rect.X, rect.Y, _rect.Width, _rect.Height) ;
            }
            else
            {
                var angle = Rotation.GetFloat(profile.CurrentSymbols);
                b.Rotate(angle);
                var The = MathHelper.Deg2Rad_P(Rotation.GetFloat(profile.CurrentSymbols));
                int W = (int)(Math.Abs(b.Width * Math.Cos(The)) + Math.Abs(b.Height * Math.Sin(The)));
                int H = (int)(Math.Abs(b.Height * Math.Cos(The)) + Math.Abs(b.Width * Math.Sin(The)));
                int _W = (int)(W / 1 * ScaledWidthRatio.GetFloat(profile.CurrentSymbols));
                int _H = (int)(H / 1 * ScaledHeightRatio.GetFloat(profile.CurrentSymbols));
                Trace.WriteLine($"Rotated:{W}x{H},{_W}x{_H}");
                _rect = new System.Drawing.Rectangle(new System.Drawing.Point((int)(LT.X - (_W - _rect.Width) / 2), (int)(LT.Y - (_H - _rect.Height) / 2)), new Size(_W, _H));

                b.Rotate(angle);
                TargetGraphics.DrawImage(b, _rect.X, _rect.Y, _rect.Width, _rect.Height);

                //Bitmap FB = new Bitmap(W, H);
                //using (Graphics g = Graphics.FromImage(FB))
                //{
                //    g.SmoothingMode = profile.SmoothingMode;
                //    g.TextRenderingHint = profile.TextRenderingHint;
                //    g.InterpolationMode = profile.InterpolationMode;
                //    g.TranslateTransform((float)W / 2, (float)H / 2);
                //    g.RotateTransform(Rotation.GetFloat(profile.CurrentSymbols));
                //    g.TranslateTransform(-(float)W / 2, -(float)H / 2);
                //    g.DrawImage(b, (W - b.Width) / 2, (H - b.Height) / 2);
                //    g.TranslateTransform((float)W / 2, (float)H / 2);
                //    g.RotateTransform(-Rotation.GetFloat(profile.CurrentSymbols));
                //    g.TranslateTransform(-(float)W / 2, -(float)H / 2);
                //}

                //TargetGraphics.DrawImage(FB, _rect, 0, 0, W, H, GraphicsUnit.Pixel);
            }
            b.Dispose();
        }

        public SubImage SoftCopy()
        {
            return new SubImage
            {
                Background = Background,
                Children = Children,
                Height = Height,
                Width = Width,
                Name = Name,
                Parent = Parent,
                root = root,
                Rotation = Rotation,
                ScaledHeightRatio = ScaledHeightRatio,
                ScaledWidthRatio = ScaledWidthRatio,
                X = X,
                Y = Y,
            };
        }
    }
}
