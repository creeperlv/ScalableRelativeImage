using ScalableRelativeImage.Core;
using SRI.Core.Backend;
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
    public class RefImage : GraphicNode
    {
        public IntermediateValue X=0;
        public IntermediateValue Y=0;
        public IntermediateValue Width = 0;
        public IntermediateValue Height = 0;
        public IntermediateValue ScaledWidthRatio = 1f;
        public IntermediateValue ScaledHeightRatio = 1f;
        public IntermediateValue Rotation = 0;
        public IntermediateValue Background = null;
        public string Source = "";
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
                { "Source", Source.ToString() }
            };
            if (Rotation is not null)
                dict.Add("Rotation", Rotation.Value);
            if (Background is not null)
                dict.Add("Background", Background.Value);
            return dict;
        }
        public override void SetValue(string Key, string Value, ref List<ExecutionWarning> executionWarnings)
        {
            switch (Key)
            {
                case "X":
                    X.Value= Value;
                    break;
                case "Y":
                    Y.Value=Value;
                    break;
                case "Width":
                    Width.Value = Value;
                    break;
                case "Height":
                    Height.Value = Value;
                    break;
                case "ScaledHeightRatio":
                    ScaledHeightRatio.Value = Value;
                    break;
                case "ScaledWidthRatio":
                    ScaledWidthRatio.Value = Value;
                    break;
                case "Background":
                    {
                        Background = new IntermediateValue
                        {
                            Value = Value
                        };
                    }
                    break;
                case "Rotation":
                    {
                        Rotation = new IntermediateValue();
                        Rotation.Value = Value;
                    }
                    break;
                case "Source":
                    {
                        Source = Value;
                    }
                    break;
                default:
                    base.SetValue(Key, Value, ref executionWarnings);
                    break;
            }
        }
        class PresudoRoot : IContainer
        {
            public float RelativeWidth { get; set; } = 1;
            public float RelativeHeight { get; set; } = 1;

            public float RelativeArea => RelativeWidth * RelativeHeight;
            List<INode> Children = new();
            public void AddNode(INode node, ref List<ExecutionWarning> executionWarnings)
            {
                Children.Add(node);
            }

            public Dictionary<string, string> GetValueSet()
            {
                return null;
            }

            public List<INode> ListNodes()
            {
                return Children;
            }

            public void SetValue(string Key, string Value, ref List<ExecutionWarning> executionWarnings)
            {
            }
        }
        public override void Paint(ref DrawableImage TargetGraphics, RenderProfile profile)
        {
            var LT = profile.FindTargetPoint(X.GetFloat(profile.CurrentSymbols), Y.GetFloat(profile.CurrentSymbols));
            var rect = new System.Drawing.Rectangle(new System.Drawing.Point((int)LT.X, (int)LT.Y), new Size(
                    (int)(Width.GetFloat(profile.CurrentSymbols) / profile.root.RelativeWidth * profile.TargetWidth), (int)(Height.GetFloat(profile.CurrentSymbols) / profile.root.RelativeHeight * profile.TargetHeight)));
            var _rect = new System.Drawing.Rectangle(new System.Drawing.Point((int)LT.X, (int)LT.Y), new Size(
                    (int)(Width.GetFloat(profile.CurrentSymbols) / profile.root.RelativeWidth * profile.TargetWidth * ScaledWidthRatio.GetFloat(profile.CurrentSymbols)), (int)(Height.GetFloat(profile.CurrentSymbols) / profile.root.RelativeHeight * profile.TargetHeight * ScaledHeightRatio.GetFloat(profile.CurrentSymbols))));
            if (Source.StartsWith("Ref:"))
            {
                var Name = Source.Substring(4);
                var sub = profile.Ref(Name).SoftCopy();
                if (Rotation is not null)
                {
                    sub.Rotation = Rotation.GetFloat(profile.CurrentSymbols, 0f);
                }
                var __LT = profile.FindTargetPoint(sub.X.GetFloat(profile.CurrentSymbols), sub.Y.GetFloat(profile.CurrentSymbols));
                var ___rect = new System.Drawing.Rectangle(new System.Drawing.Point((int)__LT.X, (int)__LT.Y),
                    new Size((int)(sub.Width / profile.root.RelativeWidth * profile.TargetWidth), (int)(sub.Height / profile.root.RelativeHeight * profile.TargetHeight)));
                var wR = ((float)_rect.Width) / (float)___rect.Width;
                var hR = ((float)_rect.Height) / (float)___rect.Height;
                var pR = new PresudoRoot() { };
                {
                    pR.RelativeWidth = profile.root.RelativeWidth;
                    pR.RelativeHeight = profile.root.RelativeHeight;
                }
                //var p = profile.Copy(pR);
                var p = profile.Copy(profile.root);//Keep reference
                p.TargetWidth = profile.TargetWidth * wR;
                p.TargetHeight = profile.TargetHeight * hR;
                var _LT = p.FindTargetPoint(sub.X.GetFloat(profile.CurrentSymbols), sub.Y.GetFloat(profile.CurrentSymbols));
                var __rect = new System.Drawing.Rectangle(new System.Drawing.Point((int)_LT.X, (int)_LT.Y), new Size(
                        (int)(sub.Width / p.root.RelativeWidth * p.TargetWidth), (int)(sub.Height / p.root.RelativeHeight * p.TargetHeight)));
                DrawableImage Bit = new DrawableImage();
                Bit.Init((int)p.TargetWidth, (int)p.TargetHeight);
                p.WorkingBitmap = Bit;
                {
                    if (Background is not null)
                        Bit.DrawRectangle((Background.GetColor(profile.CurrentSymbols)), 0, 0, (int)p.TargetWidth, (int)p.TargetHeight,0,true);
                    else
                        Bit.DrawRectangle((p.DefaultBackground.Value), 0, 0, (int)p.TargetWidth, (int)p.TargetHeight, 0, true);
                }
                //Graphics g = Graphics.FromImage(Bit);
                
                //g.SmoothingMode = profile.SmoothingMode;
                //g.TextRenderingHint = profile.TextRenderingHint;
                //g.InterpolationMode = profile.InterpolationMode;
                sub.Paint(ref Bit, p);
                TargetGraphics.DrawImage(Bit, _rect.X,_rect.Y, __rect.Width,_rect.Height);
                //g.Dispose();
                Bit.Dispose();

            }
            else
            {

                var sri = SRIEngine.Deserialize(profile.FindFile(Source));
                var p = profile.Copy(sri);
                if (Background is not null) p.DefaultBackground = Background.GetColor(profile.CurrentSymbols);
                p.TargetWidth = rect.Width;
                p.TargetHeight = rect.Height;
                var img = sri.Render(p);
                TargetGraphics.DrawImage(img, rect.X,rect.Y,rect.Width,rect.Height);
                img.Dispose();
            }
        }
    }
}
