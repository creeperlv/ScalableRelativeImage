using ImageMagick;
using SRI.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRI.Core.Core
{
    public class DrawableImage : IGraphicsBackend
    {
        IGraphicsBackend backend;

        public void Dispose()
        {
            backend?.Dispose();
        }

        public void DrawEllipse(Color color, float X, float Y, float W, float H, float Size, bool Fill)
        {
            backend.DrawEllipse(color, X, Y, W, H, Size, Fill);
        }

        public void DrawLine(Color color, float X1, float Y1, float X2, float Y2, float Size)
        {
            backend.DrawLine(color, X1, Y1, X2, Y2, Size);
        }

        public void DrawRectangle(Color color, float X, float Y, float W, float H, float BorderSize, bool Filled)
        {
            backend.DrawRectangle(color, X, Y, W, H, BorderSize, Filled);
        }
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

        public void DrawEllipse(Color color, float X, float Y, float W, float H, float Size, bool Fill)
        {
            var d = new Drawables().StrokeColor(color.ToMagick());
            if (Fill)
            {
                d.FillColor(color.ToMagick());
            }
            d.StrokeWidth(Size);
            d.Circle(X, Y, X + W, Y + H);
            d.Draw(image);
        }
    }
    public class SystemGraphicsBackend : IGraphicsBackend
    {
        System.Drawing.Graphics graphics;
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
    }
}
