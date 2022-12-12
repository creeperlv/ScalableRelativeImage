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
    /// <summary>
    /// Reference to a pixel image.
    /// </summary>
    public class PixImg : GraphicNode
    {
        public IntermediateValue X = "0";
        public IntermediateValue Y = "0";
        public IntermediateValue Width = "0";
        public IntermediateValue Height = "0";
        public IntermediateValue Source = "";
        public override Dictionary<string, string> GetValueSet()
        {
            Dictionary<string, string> dict = new()
            {
                { "X", X.ToString() },
                { "Y", Y.ToString() },
                { "Width", Width.ToString() },
                { "Height", Height.ToString() },
                { "Source", Source.ToString() }
            };
            return dict;
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
                    Width.Value = Value;
                    break;
                case "Height":
                    Height.Value = Value;
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
        public override void Paint(ref DrawableImage TargetGraphics, RenderProfile profile)
        {
            using (DrawableImage drawableImage = profile.NewImage(profile.FindFile(Source.GetString(profile.CurrentSymbols)).FullName))
            {
                var LT = profile.FindTargetPoint(X.GetFloat(profile.CurrentSymbols), Y.GetFloat(profile.CurrentSymbols));
                //var rect = new System.Drawing.Rectangle(new System.Drawing.Point((int)LT.X, (int)LT.Y), new Size(
                //        (int)(Width.GetFloat(profile.CurrentSymbols) / profile.root.RelativeWidth * profile.TargetWidth),
                //        (int)(Height.GetFloat(profile.CurrentSymbols) / profile.root.RelativeHeight * profile.TargetHeight)));
                TargetGraphics.DrawImage(drawableImage, (int)LT.X, (int)LT.Y, (int)(Width.GetFloat(profile.CurrentSymbols) / profile.root.RelativeWidth * profile.TargetWidth),
                    (int)(Height.GetFloat(profile.CurrentSymbols) / profile.root.RelativeHeight * profile.TargetHeight));
            }

        }
    }
}
