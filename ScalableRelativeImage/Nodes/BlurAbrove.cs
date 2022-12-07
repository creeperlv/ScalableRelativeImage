using CLUNL.Imaging;
using SRI.Core.Backend;
using SRI.Core.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace ScalableRelativeImage.Nodes
{
    public class BlurAbrove : GraphicNode
    {
        public float RadiusValue = 25;
        public float PixelSkipCount=1;
        public float SamplePixelSkipCount=1;
        public int BlurMode = 0;
        public bool isRoundRange = true;
        public bool useWeight = true;
        public override void SetValue(string Key, string Value, ref List<ExecutionWarning> executionWarnings)
        {
            switch (Key)
            {
                case "RadiusValue":
                    RadiusValue = float.Parse(Value);
                    break;
                case "SamplePixelSkipCount":
                    SamplePixelSkipCount = float.Parse(Value);
                    break;
                case "PixelSkipCount":
                    PixelSkipCount = float.Parse(Value);
                    break;
                case "BlurMode":
                    BlurMode = int.Parse(Value);
                    break;
                case "useWeight":
                    useWeight = bool.Parse(Value);
                    break;
                case "isRoundRange":
                    isRoundRange = bool.Parse(Value);
                    break;
                default:
                    break;
            }
        }
        public override Dictionary<string, string> GetValueSet()
        {

            Dictionary<string, string> dict = new();
            dict.Add("RadiusValue", RadiusValue.ToString());
            dict.Add("SamplePixelSkipCount", SamplePixelSkipCount.ToString());
            dict.Add("PixelSkipCount", PixelSkipCount.ToString());
            dict.Add("BlurMode", BlurMode.ToString());
            dict.Add("useWeight", useWeight.ToString());
            dict.Add("isRoundRange", isRoundRange.ToString());
            return dict;
        }
        public override void Paint(ref DrawableImage TargetGraphics, RenderProfile profile)
        {
            //TargetGraphics.Flush();
            //TargetGraphics.Save();

            //float _S = profile.FindAbsoluteSize(RadiusValue);
            //using (var B2 = new Bitmap(profile.WorkingBitmap.Width, profile.WorkingBitmap.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
            //{

            //    ProcessorArguments arguments = new ProcessorArguments(_S, PixelSkipCount, SamplePixelSkipCount, BlurMode, profile.RendererOptions, isRoundRange, useWeight);
            //    BlurProcessor.CurrentBlurProcessor.ProcessImage(profile.WorkingBitmap, B2, arguments, () =>
            //    {
            //        Trace.WriteLine($"Done. W:{B2.Width},H:{B2.Height}");
            //        GC.Collect();
            //    });
            //    var R = new System.Drawing.Rectangle(0, 0, profile.WorkingBitmap.Width, profile.WorkingBitmap.Height);
            //    TargetGraphics.DrawImage(B2, R, 0, 0, B2.Width, B2.Height, GraphicsUnit.Pixel);
            //}

        }
    }
}
