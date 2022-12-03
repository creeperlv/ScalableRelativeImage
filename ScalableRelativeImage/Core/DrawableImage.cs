using ImageMagick;
using SRI.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SRI.Core.Core
{
    public class DrawableImage : IGraphicsBackend
    {
        IGraphicsBackend backend;

        public void Init(int W, int H)
        {

            backend = BackendFactory.Instance.CreateBackend();
            backend.Init(W, H);
        }
        public void Dispose()
        {
            backend?.Dispose();
        }

        public void DrawEllipse(Color color, float X, float Y, float W, float H, float Size, bool Fill)
        {
            backend.DrawEllipse(color, X, Y, W, H, Size, Fill);
        }

        public void DrawImage(IGraphicsBackend OtherImage, int x, int y, int Width, int Height)
        {
            backend.DrawImage(OtherImage, x, y, Width, Height);
        }

        public void DrawLine(Color color, float X1, float Y1, float X2, float Y2, float Size)
        {
            backend.DrawLine(color, X1, Y1, X2, Y2, Size);
        }

        public void DrawRectangle(Color color, float X, float Y, float W, float H, float BorderSize, bool Filled)
        {
            backend.DrawRectangle(color, X, Y, W, H, BorderSize, Filled);
        }


        public Bitmap ToBitmap()
        {
            return backend.ToBitmap();
        }

        public void DrawLines(Color color, float Size, UniversalVector2[] Points)
        {
            backend.DrawLines(color, Size, Points);
        }

        public void DrawPath(Color color, UniversalVector2[] Points, byte[] types, float Size, bool Fill)
        {
            backend.DrawPath(color, Points, types, Size, Fill);
        }

        public void DrawText(string text, string FontFamily, FontStyle style, float Size, Color color, float X, float Y, float W, float H, StringAlignment HorizontalAlignment, StringAlignment VerticalAlignment)
        {
            backend.DrawText(text, FontFamily, style, Size, color, X, Y, W, H, HorizontalAlignment, VerticalAlignment);
        }

        public void SaveToFile(string filename, UniversalImageFormat format)
        {
            backend.SaveToFile(filename, format);
        }
    }
    public enum UniversalImageFormat
    {
        PNG, JPG, BMP
    }
    public class MagickGraphicsBackend : IGraphicsBackend
    {
        static bool isLibraryInited;
        MagickImage image = null;
        public void Init(int W, int H)
        {
            if (!isLibraryInited)
            {
                MagickNET.Initialize();
            }
            image = new MagickImage(MagickColors.Transparent, W, H);
        }
        public void DrawRectangle(Color color, float X, float Y, float W, float H, float BorderSize, bool Filled)
        {
            var d = new Drawables().StrokeColor(color.ToMagick());
            if (Filled)
            {
                d.FillColor(color.ToMagick());
            }
            d.StrokeWidth(BorderSize);
            d.Rectangle(X, Y, X + W, Y + H);
            d.Draw(image);
        }
        public void DrawLine(Color color, float X1, float Y1, float X2, float Y2, float Size)
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
            if (OtherImage is MagickGraphicsBackend backend)
            {
                var d = new Drawables();
                d.Composite((new MagickGeometryFactory()).Create(x, y, Width, Height), CompositeOperator.Alpha, backend.image);
                d.Draw(image);
            }
            else
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
        public void DrawLines(Color color, float Size, UniversalVector2[] Points)
        {
            Paths paths = new Paths();
            foreach (var point in Points)
            {
                paths.LineToAbs(point);
            }
            var d = new Drawables().StrokeColor(color.ToMagick());
            d = d.StrokeWidth(Size);
            d = d.Path(paths);
            d.Draw(image);
        }
        public void DrawPath(Color color, UniversalVector2[] Points, byte[] types, float Size, bool Fill)
        {
            Paths p = new Paths();
            List<IPath> paths = new List<IPath>();
            for (int i = 0; i < Points.Length; i++)
            {
                var V2 = Points[i];
                var T = types[i];
                switch (T)
                {
                    case 1:
                        {
                            p.LineToAbs(V2.X, V2.Y);
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
            d = d.Path(paths);
            d.Draw(image);
        }
        public void DrawText(string text, string FontFamily, FontStyle style, float Size, Color color, float X, float Y, float W, float H, StringAlignment HorizontalAlignment, StringAlignment VerticalAlignment)
        {
            var settings = new MagickReadSettings
            {
                Font = FontFamily,
                FillColor = color.ToMagick(),
                FontStyle = style.ToFontStyleType(),
                TextGravity = Backends.ToGravity(HorizontalAlignment, VerticalAlignment),
                BackgroundColor = MagickColors.Transparent,
                FontPointsize = Size,
                Height = (int)W,
                Width = (int)H
            };

            using (var text_img = new MagickImage(text, settings))
            {
                image.Composite(text_img, (int)X, (int)Y, CompositeOperator.Over);
            }
        }
        public void DrawEllipse(Color color, float X, float Y, float W, float H, float Size, bool Fill)
        {
            var d = new Drawables().StrokeColor(color.ToMagick());
            if (Fill)
            {
                d.FillColor(color.ToMagick());
            }
            d = d.StrokeWidth(Size);
            d = d.Ellipse(X, Y, W, H, 0, 360);
            d.Draw(image);
        }
    }
    public class SystemGraphicsBackend : IGraphicsBackend
    {
        Graphics graphics;
        Bitmap image = null;
        public void Init(int W, int H)
        {
            image = new Bitmap(W, H);
            graphics = Graphics.FromImage(image);
        }
        public void Dispose()
        {
            graphics.Dispose();
        }
        public void DrawEllipse(Color color, float X, float Y, float W, float H, float Size, bool Fill)
        {
            if (Fill)
            {
                graphics.FillEllipse(new SolidBrush(color), X, Y, W, H);
            }
            else
            {
                graphics.DrawEllipse(new Pen(color, Size), X, Y, W, H);
            }
        }
        public Bitmap ToBitmap()
        {
            Bitmap b;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                image.Save(memoryStream, ImageFormat.Png);
                memoryStream.Position = 0;
                b = new Bitmap(memoryStream);
            }
            GC.Collect();
            return b;
        }
        public void DrawRectangle(Color color, float X, float Y, float W, float H, float BorderSize, bool Filled)
        {
            if (!Filled)
                graphics.DrawRectangle(new Pen(color, BorderSize), X, Y, W, H);
            else
                graphics.FillRectangle(new SolidBrush(color), X, Y, W, H);
        }

        public void DrawLine(Color color, float X1, float Y1, float X2, float Y2, float Size)
        {
            graphics.DrawLine(new Pen(color, Size), X1, Y1, X2, Y2);
        }

        public void DrawImage(IGraphicsBackend OtherImage, int x, int y, int Width, int Height)
        {
            if (OtherImage is SystemGraphicsBackend backend)
            {
                graphics.DrawImage(backend.image, new Rectangle(x, y, Width, Height), 0, 0, backend.image.Width, backend.image.Height, GraphicsUnit.Pixel);
            }
            else
            {
                throw new NotSupportedException();
            }
        }
        public void DrawLines(Color color, float Size, UniversalVector2[] Points)
        {
            graphics.DrawLines(new Pen(color, Size), Points.ToPointFArray());
        }

        public void DrawPath(Color color, UniversalVector2[] Points, byte[] types, float Size, bool Fill)
        {
            if (Fill)
            {
                graphics.FillPath(new SolidBrush(color), new System.Drawing.Drawing2D.GraphicsPath(Points.ToPointFArray(), types));

            }
            else
                graphics.DrawPath(new Pen(color, Size), new System.Drawing.Drawing2D.GraphicsPath(Points.ToPointFArray(), types));
        }

        public void DrawText(string text, string FontFamily, FontStyle style, float Size, Color color, float X, float Y, float W, float H, StringAlignment HorizontalAlignment, StringAlignment VerticalAlignment)
        {
            graphics.DrawString(text, new Font(FontFamily, Size, style)
                , new SolidBrush(color), new RectangleF(new PointF(X, Y), new SizeF(W, H)), new StringFormat { Alignment = HorizontalAlignment, LineAlignment = VerticalAlignment });
        }

        public void SaveToFile(string filename, UniversalImageFormat format)
        {
            image.Save(filename, format.ToImageFormat());
        }
    }
}
