using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SRI.Core.Backend
{
    /// <summary>
    /// Platform-irrelevant vector2.
    /// </summary>
    public struct UniversalVector2
    {
        public float X;
        public float Y;

        public override bool Equals(object? obj)
        {
            return obj is UniversalVector2 vector &&
                   X == vector.X &&
                   Y == vector.Y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
        public static UniversalVector2 operator +(UniversalVector2 L, UniversalVector2 R)
        {
            return new UniversalVector2 { X = L.X + R.X, Y = L.Y + R.Y };
        }
        public static UniversalVector2 operator *(UniversalVector2 L, float R)
        {
            return new UniversalVector2 { X = L.X * R, Y = L.Y * R };
        }
        public static UniversalVector2 operator /(UniversalVector2 L, float R)
        {
            return new UniversalVector2 { X = L.X / R, Y = L.Y / R };
        }
        public static UniversalVector2 operator -(UniversalVector2 L, UniversalVector2 R)
        {
            return new UniversalVector2 { X = L.X - R.X, Y = L.Y - R.Y };
        }
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
