using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace SRI.Core.Backend.SystemDrawing
{
    /// <summary>
    /// Backend using System.Drawing.
    /// </summary>
    public class SystemGraphicsBackend : IGraphicsBackend
    {
        Graphics graphics;
        Bitmap image = null;

        public int Width => image.Width;

        public int Height => image.Height;

        public void Init(string File)
        {
            image = new Bitmap(File);
            try
            {

                graphics = Graphics.FromImage(image);
            }
            catch (Exception)
            {
            }
        }
        public void Init(int W, int H)
        {
            image = new Bitmap(W, H);
            graphics = Graphics.FromImage(image);
        }
        public void Dispose()
        {
            if (image != null)
                image.Dispose();
            if (graphics != null)
                graphics.Dispose();
        }
        public void DrawEllipse(ColorF ColorF, float X, float Y, float W, float H, float Size, bool Fill)
        {
            if (Fill)
            {
                graphics.FillEllipse(new SolidBrush(ColorF.ToColor()), X, Y, W, H);
            }
            else
            {
                graphics.DrawEllipse(new Pen(ColorF.ToColor(), Size), X, Y, W, H);
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
        public void DrawRectangle(ColorF ColorF, float X, float Y, float W, float H, float BorderSize, bool Filled)
        {
            if (!Filled)
                graphics.DrawRectangle(new Pen(ColorF.ToColor(), BorderSize), X, Y, W, H);
            else
                graphics.FillRectangle(new SolidBrush(ColorF.ToColor()), X, Y, W, H);
        }

        public void DrawLine(ColorF ColorF, float X1, float Y1, float X2, float Y2, float Size)
        {
            graphics.DrawLine(new Pen(ColorF.ToColor(), Size), X1, Y1, X2, Y2);
        }

        public void DrawImage(IGraphicsBackend OtherImage, UniversalRectangle Dest,UniversalRectangle Src)
        {
            if (OtherImage is DrawableImage di)
            {
                if (di.Backend is SystemGraphicsBackend backend)
                {
                    graphics.DrawImage(backend.image, Dest.ToRectangle(),Src.ToRectangle(), GraphicsUnit.Pixel);
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
        public void DrawLines(ColorF ColorF, float Size, UniversalVector2[] Points)
        {
            graphics.DrawLines(new Pen(ColorF.ToColor(), Size), Points.ToPointFArray());
        }

        public void DrawPath(ColorF ColorF, UniversalVector2[] Points, byte[] types, float Size, bool Fill)
        {
            if (Fill)
            {
                graphics.FillPath(new SolidBrush(ColorF.ToColor()), new System.Drawing.Drawing2D.GraphicsPath(Points.ToPointFArray(), types));

            }
            else
                graphics.DrawPath(new Pen(ColorF.ToColor(), Size), new System.Drawing.Drawing2D.GraphicsPath(Points.ToPointFArray(), types));
        }

        public void DrawText(string text, string FontFamily, FontStyle style, float Size, ColorF ColorF, float X, float Y, float W, float H, StringAlignment HorizontalAlignment, StringAlignment VerticalAlignment)
        {
            graphics.DrawString(text, new Font(FontFamily, Size, style)
                , new SolidBrush(ColorF.ToColor()), new RectangleF(new PointF(X, Y), new SizeF(W, H)), new StringFormat { Alignment = HorizontalAlignment, LineAlignment = VerticalAlignment });
        }

        public void SaveToFile(string filename, UniversalImageFormat format)
        {
            image.Save(filename, format.ToImageFormat());
        }

        public void DrawCurve(ColorF ColorF, float Size, UniversalVector2[] Points)
        {
            graphics.DrawCurve(new Pen(ColorF.ToColor(), Size), Points.ToPointFArray());
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

        public void DrawClosedCurve(ColorF ColorF, float Size, UniversalVector2[] Points, bool Filled)
        {
            if (Filled)
            {
                graphics.FillClosedCurve(new SolidBrush(ColorF.ToColor()), Points.ToPointFArray());
            }
            else
                graphics.DrawClosedCurve(new Pen(ColorF.ToColor(), Size), Points.ToPointFArray());
        }
    }
}
