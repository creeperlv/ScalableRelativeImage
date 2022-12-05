using ImageMagick;
using ScalableRelativeImage;
using SRI.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private IGraphicsBackend backend;
        public IGraphicsBackend Backend { get { return backend; } }
        public int Width => backend.Width;

        public int Height => backend.Height;

        public void Init(string File)
        {
            backend = BackendFactory.Instance.CreateBackend();
            backend.Init(File);
        }
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

        public void DrawLine(Color color, UniversalVector2 StartPoint, UniversalVector2 EndPoint, float Size)
        {
            backend.DrawLine(color, StartPoint.X, StartPoint.Y, EndPoint.X, EndPoint.Y, Size);
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
        public void Save(string filename)
        {
            backend.Save(filename);
        }
        public void Save(Stream stream, UniversalImageFormat format)
        {
            backend.Save(stream, format);
        }
        public void SaveToFile(string filename, UniversalImageFormat format)
        {
            backend.SaveToFile(filename, format);
        }

        public void DrawCurve(Color color, float Size, UniversalVector2[] Points)
        {
            backend.DrawCurve(color, Size, Points);
        }

        public void Rotate(float Deg)
        {
            backend.Rotate(Deg);
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
        public void DrawRectangle(Color color, float X, float Y, float W, float H, float BorderSize, bool Filled)
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
                    if(Scale_V==-1)
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
        public void DrawCurve(Color color, float Size, UniversalVector2[] Points)
        {
            Paths paths = new Paths();
            foreach (var point in Points)
            {
                paths.SmoothQuadraticCurveToAbs(point);
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
        public void DrawPath(Color color, UniversalVector2[] Points, byte[] types, float Size, bool Fill)
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
                            p.LineToAbs(V2);
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
        public void DrawEllipse(Color color, float X, float Y, float W, float H, float Size, bool Fill)
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
    }
    public class SystemGraphicsBackend : IGraphicsBackend
    {
        Graphics graphics;
        Bitmap image = null;

        public int Width => image.Width;

        public int Height => image.Height;

        public void Init(string File)
        {
            image = new Bitmap(File);
            graphics = Graphics.FromImage(image);
        }
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
            if (OtherImage is DrawableImage di)
            {
                if (di.Backend is SystemGraphicsBackend backend)
                {
                    graphics.DrawImage(backend.image, new Rectangle(x, y, Width, Height), 0, 0, backend.image.Width, backend.image.Height, GraphicsUnit.Pixel);
                    //var d = new Drawables().Composite((new MagickGeometryFactory()).Create(x, y, Width, Height), CompositeOperator.Alpha, backend.image);
                    //d.Draw(image);
                    return;
                }
            }
            {
                throw new NotSupportedException();
            }
            //if (OtherImage is SystemGraphicsBackend backend)
            //{
            //    graphics.DrawImage(backend.image, new Rectangle(x, y, Width, Height), 0, 0, backend.image.Width, backend.image.Height, GraphicsUnit.Pixel);
            //}
            //else
            //{
            //    throw new NotSupportedException();
            //}
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

        public void DrawCurve(Color color, float Size, UniversalVector2[] Points)
        {
            graphics.DrawCurve(new Pen(color, Size), Points.ToPointFArray());
        }

        public void Rotate(float Deg)
        {

            var The = MathHelper.Deg2Rad_P(Deg);
            int Ori_W = image.Width;
            int Ori_H = image.Height;
            int W = (int)(Math.Abs(Ori_W * Math.Cos(The)) + Math.Abs(Ori_H * Math.Sin(The)));
            int H = (int)(Math.Abs(Ori_W * Math.Cos(The)) + Math.Abs(Ori_W * Math.Sin(The)));
            Bitmap FB = new Bitmap(W, H);
            Graphics g = Graphics.FromImage(FB);
            {
                //g.SmoothingMode = profile.SmoothingMode;
                //g.TextRenderingHint = profile.TextRenderingHint;
                //g.InterpolationMode = profile.InterpolationMode;
                g.TranslateTransform((float)W / 2, (float)H / 2);
                g.RotateTransform(Deg);
                g.TranslateTransform(-(float)W / 2, -(float)H / 2);
                g.DrawImage(image, (W - Ori_W) / 2, (H - Ori_H) / 2);
                g.TranslateTransform((float)W / 2, (float)H / 2);
                g.RotateTransform(-Deg);
                g.TranslateTransform(-(float)W / 2, -(float)H / 2);
            }
            int _W = (int)(W / 1 * 1);
            int _H = (int)(H / 1 * 1);
#if TRACE
            Trace.WriteLine($"Rotated:{W}x{H},{_W}x{_H}");
#endif
            graphics.Dispose();
            image.Dispose();
            graphics = g;
            image = FB;
            //_rect = new System.Drawing.Rectangle(new System.Drawing.Point((int)(LT.X - (_W - _rect.Width) / 2), (int)(LT.Y - (_H - _rect.Height) / 2)), new Size(_W, _H));
            //TargetGraphics.DrawImage(FB, _rect, 0, 0, W, H, GraphicsUnit.Pixel);
        }

        public void Save(string filename)
        {
            image.Save(filename);
        }

        public void Save(Stream stream, UniversalImageFormat format)
        {
            image.Save(stream, format.ToImageFormat());
        }
    }
}
