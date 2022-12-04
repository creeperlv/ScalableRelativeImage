using ImageMagick;
using SRI.Core.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRI.Core.Utilities
{
    public static class Backends
    {
        static float B16B8Converter = 65535 / 255;
        public static MagickColor ToMagick(this Color c)
        {
            return new MagickColor(c.R * B16B8Converter, c.G * B16B8Converter, c.B * B16B8Converter, c.A * B16B8Converter);
        }
        public static FontStyleType ToFontStyleType(this FontStyle style)
        {
            switch (style)
            {
                case FontStyle.Bold:
                    return FontStyleType.Bold;
                case FontStyle.Italic:
                    return FontStyleType.Italic;

            }
            return FontStyleType.Any;
        }

        public static Gravity ToGravity(StringAlignment H, StringAlignment V)
        {
            var Layout = (int)V * 3 + (int)H + 1;
            return (Gravity)Layout;
        }
        public static Gravity ToGravity(this StringAlignment alignment)
        {
            switch (alignment)
            {
                case StringAlignment.Near:
                    return Gravity.North;
                case StringAlignment.Center:
                    return Gravity.Center;
                case StringAlignment.Far:
                    return Gravity.South;
                default:
                    break;
            }
            return Gravity.Undefined;
        }
        public static ImageFormat ToImageFormat(this UniversalImageFormat format)
        {
            switch (format)
            {
                case UniversalImageFormat.PNG:
                    return ImageFormat.Png;
                case UniversalImageFormat.JPG:
                    return ImageFormat.Jpeg;
                case UniversalImageFormat.BMP:
                    return ImageFormat.Bmp;
                default:
                    break;
            }
            return ImageFormat.Png;
        }
        public static MagickFormat ToMagickFormat(this UniversalImageFormat format)
        {
            switch (format)
            {
                case UniversalImageFormat.PNG:
                    return MagickFormat.Png;
                case UniversalImageFormat.JPG:
                    return MagickFormat.Jpeg;
                case UniversalImageFormat.BMP:
                    return MagickFormat.Bmp;
            }
            return MagickFormat.Png;
        }
        public static TextAlignment ToTextAlignment(this StringAlignment alignment)
        {
            switch (alignment)
            {
                case StringAlignment.Near:
                    return TextAlignment.Left;
                case StringAlignment.Center:
                    return TextAlignment.Center;
                case StringAlignment.Far:
                    return TextAlignment.Right;
                default:
                    break;
            }
            return TextAlignment.Undefined;
        }
        public static PointF[] ToPointFArray(this UniversalVector2[] vs)
        {
            PointF[] f=new PointF[vs.Length];
            for (int i = 0; i < vs.Length; i++)
            {
                f[i] = vs[i];
            }
            return f;
        }
    }
}
