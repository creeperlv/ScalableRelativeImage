using System;
using System.Drawing;

namespace SRI.Core.Core
{
    public interface IGraphicsBackend : IDisposable
    {
        void DrawEllipse(Color color, float X, float Y, float W, float H, float Size, bool Fill);
        void DrawImage(IGraphicsBackend OtherImage, int x, int y, int Width, int Height);
        void DrawLine(Color color, float X1, float Y1, float X2, float Y2, float Size);
        void DrawLines(Color color, float Size, UniversalVector2[] Points);
        void DrawPath(Color color, UniversalVector2[] Points, byte[] types, float Size, bool Fill);
        void DrawRectangle(Color color, float X, float Y, float W, float H, float BorderSize, bool Filled);
        void DrawText(string text, string FontFamily, FontStyle style, float Size, Color color, float X, float Y, float W, float H, StringAlignment HorizontalAlignment, StringAlignment VerticalAlignment);
        void Init(int W, int H);
        void SaveToFile(string filename, UniversalImageFormat format);
        Bitmap ToBitmap();
    }
}
