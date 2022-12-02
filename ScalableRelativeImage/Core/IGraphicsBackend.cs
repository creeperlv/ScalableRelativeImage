using System;
using System.Drawing;

namespace SRI.Core.Core
{
    public interface IGraphicsBackend : IDisposable
    {
        void DrawEllipse(Color color, float X, float Y, float W, float H, float Size, bool Fill);
        void DrawLine(Color color, float X1, float Y1, float X2, float Y2, float Size);
        void DrawRectangle(Color color, float X, float Y, float W, float H, float BorderSize, bool Filled);
    }
}
