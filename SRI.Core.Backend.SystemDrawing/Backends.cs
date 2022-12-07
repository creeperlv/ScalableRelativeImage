using SRI.Core.Backend;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRI.Core.Backend.SystemDrawing
{
    public static class Backends
    {
        static float B16B8Converter = 65535 / 255;
      
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
        public static PointF[] ToPointFArray(this UniversalVector2[] vs)
        {
            PointF[] f=new PointF[vs.Length];
            for (int i = 0; i < vs.Length; i++)
            {
                f[i] = vs[i].ToPointF();
            }
            return f;
        }
        public static PointF ToPointF(this UniversalVector2 v)
        {
            return new PointF(v.X, v.Y);
        }
        public static Color ToColor(this ColorF v)
        {
            return Color.FromArgb((int)v.A, (int)v.R, (int)v.G, (int)v.B);
        }
    }
}
