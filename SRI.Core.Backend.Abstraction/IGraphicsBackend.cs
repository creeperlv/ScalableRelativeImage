using System;
using System.Drawing;
using System.IO;

namespace SRI.Core.Backend
{
    public interface IGraphicsBackend : IDisposable
    {
        int Width { get; }
        int Height { get; }
        /// <summary>
        /// Draw a closed curve. Should use cardinal spline.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="Size"></param>
        /// <param name="Points"></param>
        /// <param name="Filled"></param>
        void DrawClosedCurve(ColorF color, float Size, UniversalVector2[] Points, bool Filled);
        /// <summary>
        /// Draw curve. Should use cardinal spline.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="Size"></param>
        /// <param name="Points"></param>
        void DrawCurve(ColorF color, float Size, UniversalVector2[] Points);
        /// <summary>
        /// Draw ellipse
        /// </summary>
        /// <param name="color"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="W"></param>
        /// <param name="H"></param>
        /// <param name="Size"></param>
        /// <param name="Fill"></param>
        void DrawEllipse(ColorF color, float X, float Y, float W, float H, float Size, bool Fill);
        /// <summary>
        /// Draw an image.
        /// </summary>
        /// <param name="OtherImage"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        void DrawImage(IGraphicsBackend OtherImage, UniversalRectangle Dest,UniversalRectangle Src);
        /// <summary>
        /// Draw a line.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="X1"></param>
        /// <param name="Y1"></param>
        /// <param name="X2"></param>
        /// <param name="Y2"></param>
        /// <param name="Size"></param>
        void DrawLine(ColorF color, float X1, float Y1, float X2, float Y2, float Size);
        /// <summary>
        /// Draw lines.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="Size"></param>
        /// <param name="Points"></param>
        void DrawLines(ColorF color, float Size, UniversalVector2[] Points);
        /// <summary>
        /// Draw path. Should behave similar to System.Drawing's counterparts.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="Points"></param>
        /// <param name="types"></param>
        /// <param name="Size"></param>
        /// <param name="Fill"></param>
        void DrawPath(ColorF color, UniversalVector2[] Points, byte[] types, float Size, bool Fill);
        /// <summary>
        /// Draw a rectangle.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="W"></param>
        /// <param name="H"></param>
        /// <param name="BorderSize"></param>
        /// <param name="Filled"></param>
        void DrawRectangle(ColorF color, float X, float Y, float W, float H, float BorderSize, bool Filled);
        /// <summary>
        /// Draw some texts. Should support basic formatting.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="FontFamily"></param>
        /// <param name="style"></param>
        /// <param name="Size"></param>
        /// <param name="color"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="W"></param>
        /// <param name="H"></param>
        /// <param name="HorizontalAlignment"></param>
        /// <param name="VerticalAlignment"></param>
        void DrawText(string text, string FontFamily, FontStyle style, float Size, ColorF color, float X, float Y, float W, float H, StringAlignment HorizontalAlignment, StringAlignment VerticalAlignment);
        /// <summary>
        /// Initialize with given W and H.
        /// </summary>
        /// <param name="W"></param>
        /// <param name="H"></param>
        void Init(int W, int H);
        /// <summary>
        /// Initialize from file.
        /// </summary>
        /// <param name="File"></param>
        void Init(string File);
        /// <summary>
        /// Rotate 
        /// </summary>
        /// <param name="Deg"></param>
        void Rotate(float Deg);
        /// <summary>
        /// Save to a file.
        /// </summary>
        /// <param name="filename"></param>
        void Save(string filename);
        /// <summary>
        /// Write to a stream.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="format"></param>
        void Save(Stream stream, UniversalImageFormat format);
        /// <summary>
        /// Save to file.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="format"></param>
        void SaveToFile(string filename, UniversalImageFormat format);
        /// <summary>
        /// Convert to System.Drawing.Bitmap;
        /// </summary>
        /// <returns></returns>
        Bitmap ToBitmap();
    }
}
