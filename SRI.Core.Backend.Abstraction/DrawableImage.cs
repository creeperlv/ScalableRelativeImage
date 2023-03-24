using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SRI.Core.Backend
{
    public abstract class BaseBackendFactory
    {
        public static BaseBackendFactory Instance;
        public abstract IGraphicsBackend CreateBackend();
    }
    public class DrawableImage : IGraphicsBackend
    {
        private IGraphicsBackend backend;
        public IGraphicsBackend Backend { get { return backend; } }
        public int Width => backend.Width;

        public int Height => backend.Height;

        public void Init(string File)
        {
            backend = BaseBackendFactory.Instance.CreateBackend();
            backend.Init(File);
        }
        public void Init(int W, int H)
        {

            backend = BaseBackendFactory.Instance.CreateBackend();
            backend.Init(W, H);
        }
        public void Dispose()
        {
            backend?.Dispose();
        }

        public void DrawEllipse(ColorF color, float X, float Y, float W, float H, float Size, bool Fill)
        {
            backend.DrawEllipse(color, X, Y, W, H, Size, Fill);
        }

        public void DrawImage(IGraphicsBackend OtherImage, UniversalRectangle Dest,UniversalRectangle Src)
        {
            backend.DrawImage(OtherImage, Dest,Src);
        }
        public void Mask(IGraphicsBackend Src,IGraphicsBackend Mask)
        {
            backend.Mask(Src, Mask);
        }
        public void DrawLine(ColorF color, UniversalVector2 StartPoint, UniversalVector2 EndPoint, float Size)
        {
            backend.DrawLine(color, StartPoint.X, StartPoint.Y, EndPoint.X, EndPoint.Y, Size);
        }
        public void DrawLine(ColorF color, float X1, float Y1, float X2, float Y2, float Size)
        {
            backend.DrawLine(color, X1, Y1, X2, Y2, Size);
        }

        public void DrawRectangle(ColorF color, float X, float Y, float W, float H, float BorderSize, bool Filled)
        {
            backend.DrawRectangle(color, X, Y, W, H, BorderSize, Filled);
        }


        public Bitmap ToBitmap()
        {
            return backend.ToBitmap();
        }

        public void DrawLines(ColorF color, float Size, UniversalVector2[] Points)
        {
            backend.DrawLines(color, Size, Points);
        }

        public void DrawPath(ColorF color, UniversalVector2[] Points, byte[] types, float Size, bool Fill)
        {
            backend.DrawPath(color, Points, types, Size, Fill);
        }

        public void DrawText(string text, string FontFamily, FontStyle style, float Size, ColorF color, float X, float Y, float W, float H, StringAlignment HorizontalAlignment, StringAlignment VerticalAlignment)
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

        public void DrawCurve(ColorF color, float Size, UniversalVector2[] Points)
        {
            backend.DrawCurve(color, Size, Points);
        }

        public void Rotate(float Deg)
        {
            backend.Rotate(Deg);
        }

        public void DrawClosedCurve(ColorF color, float Size, UniversalVector2[] Points, bool Filled)
        {
            backend.DrawClosedCurve(color, Size, Points, Filled);
        }
    }
}
