using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SRI.Core.Backend
{
    public struct UniversalVector2
    {
        public float X;
        public float Y;
        public static bool operator ==(UniversalVector2 L, UniversalVector2 R)
        {
            return L.X == R.X && L.Y == R.Y;
        }
        public static bool operator !=(UniversalVector2 L, UniversalVector2 R)
        {
            return L.X != R.X || L.Y != R.Y;
        }
    }
}
