using ImageMagick;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRI.Core.Core
{
    public struct UniversalVector2
    {
        public float X; 
        public float Y;
        public static implicit operator PointD(UniversalVector2 v)
        {
            return new PointD(v.X, v.Y);
        }
        public static implicit operator PointF(UniversalVector2 v)
        {
            return new PointF(v.X, v.Y);
        }
    }
}
