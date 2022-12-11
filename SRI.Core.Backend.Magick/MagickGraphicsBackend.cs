using ImageMagick;
using System;
using System.Drawing;
using System.IO;
using System.Linq;

namespace SRI.Core.Backend.Magick
{
    public class MagickGraphicsBackend : IGraphicsBackend
    {
        static bool isLibraryInited;
        MagickImage image = null;
        public int Width => image.Width;
        public int Height => image.Height;
        public void Init(string File)
        {
            if (!isLibraryInited)
            {
                MagickNET.Initialize();
            }
            image = new MagickImage(new FileInfo(File));
        }
        public void Init(int W, int H)
        {
            if (!isLibraryInited)
            {
                MagickNET.Initialize();
            }
            image = new MagickImage(MagickColors.Transparent, W, H);
        }
        public void DrawRectangle(ColorF color, float X, float Y, float W, float H, float BorderSize, bool Filled)
        {
            var d = new Drawables().StrokeColor(color.ToMagick());
            if (Filled)
            {
                d = d.FillColor(color.ToMagick());
            }
            else
            {

                d = d.FillColor(MagickColors.Transparent);
                d = d.StrokeWidth(BorderSize);
            }
            d = d.Rectangle(X, Y, X + W, Y + H);
            d.Draw(image);
        }
        public void DrawLine(ColorF color, float X1, float Y1, float X2, float Y2, float Size)
        {
            var d = new Drawables().StrokeColor(color.ToMagick()).StrokeWidth(Size).Line(X1, Y1, X2, Y2);
            d.Draw(image);
        }
        public void Dispose()
        {
            image.Dispose();
        }
        public void DrawImage(IGraphicsBackend OtherImage, int x, int y, int Width, int Height)
        {
            if (OtherImage is DrawableImage di)
            {
                if (di.Backend is MagickGraphicsBackend backend)
                {
                    int Scale_H = 1;
                    int Scale_V = 1;
                    int W = Width;
                    int H = Height;
                    if (W < 0)
                    {
                        Scale_H = -1;
                        W = -W;
                    }
                    if (H < 0)
                    {
                        Scale_V = -1;
                        H = -H;
                    }
                    MagickGeometry magickGeometry = new MagickGeometry(W, H);
                    magickGeometry.IgnoreAspectRatio = true;
                    backend.image.Resize(magickGeometry);
                    if (Scale_H == -1)
                        backend.image.Flop();
                    if (Scale_V == -1)
                        backend.image.Flip();
                    //if (Scale_H != 1 || Scale_V != 1)
                    //    backend.image.Scale(Scale_H, Scale_V);
                    image.Composite(backend.image, x, y, CompositeOperator.Blend);
                    //var d = new Drawables().Composite((new MagickGeometryFactory()).Create(x, y, Width, Height), CompositeOperator.Alpha, backend.image);
                    //d.Draw(image);
                    return;
                }
            }
            {
                throw new NotSupportedException();
            }

        }
        public Bitmap ToBitmap()
        {
            Bitmap b;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                image.Write(memoryStream, MagickFormat.Png);
                memoryStream.Position = 0;
                b = new Bitmap(memoryStream);
            }
            GC.Collect();
            return b;
        }
        public void SaveToFile(string filename, UniversalImageFormat format)
        {
            image.Write(filename, format.ToMagickFormat());
        }
        public void DrawLines(ColorF color, float Size, UniversalVector2[] Points)
        {
            Paths paths = new Paths();
            foreach (var point in Points)
            {
                paths.LineToAbs(point.ToPointD());
            }
            var d = new Drawables().StrokeColor(color.ToMagick());
            d = d.StrokeWidth(Size);
            d = d.Path(paths);
            d.Draw(image);
        }
        public void DrawCurve(ColorF color, float Size, UniversalVector2[] Points)
        {
            Paths paths = new Paths();
            foreach (var point in Points)
            {
                paths.SmoothQuadraticCurveToAbs(point.ToPointD());
            }
            var d = new Drawables().StrokeColor(color.ToMagick());
            d = d.StrokeWidth(Size);
            d = d.Path(paths);
            d.Draw(image);
        }
        public void Rotate(float Deg)
        {
            image.Rotate(Deg);
        }
        public void DrawPath(ColorF color, UniversalVector2[] Points, byte[] types, float Size, bool Fill)
        {
            Paths p = new Paths();
            for (int i = 0; i < Points.Length; i++)
            {
                //var LastP = Points[i - 1];
                var V2 = Points[i];
                var T = types[i];
                switch (T)
                {
                    case 1:
                        {
                            p.LineToAbs(V2.ToPointD());
                        }
                        break;
                    case 3:
                        {
                            p.SmoothQuadraticCurveToAbs(V2.X, V2.Y);
                        }
                        break;
                    case 128:
                        {
                        }
                        break;
                    default:
                        break;
                }
            }

            var d = new Drawables().StrokeColor(color.ToMagick()).StrokeWidth(Size);
            if (Fill)
            {
                d = d.FillColor(color.ToMagick());
            }
            else
            {

                d = d.FillColor(MagickColors.Transparent);
            }
            d = d.Path(p);
            d.Draw(image);
        }
        public void DrawText(string text, string FontFamily, FontStyle style, float Size, ColorF color, float X, float Y, float W, float H, StringAlignment HorizontalAlignment, StringAlignment VerticalAlignment)
        {
            var settings = new MagickReadSettings
            {
                Font = FontFamily,
                FillColor = color.ToMagick(),
                FontStyle = style.ToFontStyleType(),
                TextGravity = Extensions.ToGravity(HorizontalAlignment, VerticalAlignment),
                BackgroundColor = MagickColors.Transparent,
                FontPointsize = Size,
                Width = (int)W,
                Height = (int)H,
            };

            using (var text_img = new MagickImage($"caption:{text}", settings))
            {
                //var d = new Drawables().Composite((new MagickGeometryFactory()).Create((int)X, (int)Y, (int)W, (int)H), CompositeOperator.Alpha, textimg);
                //d.Draw(image);
                image.Composite(text_img, (int)(X), (int)Y, CompositeOperator.Over);
            }
        }
        public void DrawEllipse(ColorF color, float X, float Y, float W, float H, float Size, bool Fill)
        {
            var d = new Drawables().StrokeColor(color.ToMagick());
            if (Fill)
            {
                d = d.FillColor(color.ToMagick());
            }
            else
            {

                d = d.FillColor(MagickColors.Transparent);
                d = d.StrokeWidth(Size);
            }
            var rX = W / 2;
            var rY = H / 2;
            d = d.Ellipse(X + rX, Y + rY, rX, rY, 0, 360);
            d.Draw(image);
        }

        public void Save(string filename)
        {
            image.Write(filename);
        }

        public void Save(Stream stream, UniversalImageFormat format)
        {
            image.Write(stream, format.ToMagickFormat());
        }

        public void DrawClosedCurve(ColorF color, float Size, UniversalVector2[] Points, bool Filled)
        {
            Paths paths = new Paths();
            foreach (var point in Points)
            {
                paths.SmoothQuadraticCurveToAbs(point.ToPointD());
            }
            if (Points.Last() != Points.First())
            {
                paths.SmoothQuadraticCurveToAbs(Points.First().ToPointD());
            }
            var d = new Drawables().StrokeColor(color.ToMagick());
            d = d.StrokeWidth(Size);
            d = d.Path(paths);
            d.Draw(image);
        }
    }
}
