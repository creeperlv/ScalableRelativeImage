using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScalableRelativeImage.Core
{
    [Serializable]
    public class SRIIntPoint
    {
        public int X;
        public int Y;
        public SRIIntPoint()
        {
            X = 0;
            Y = 0;
        }
        public static SRIIntPoint Parse(string str)
        {
            SRIIntPoint p = new SRIIntPoint();
            var GRP = str.Split(',');
            if (GRP.Length == 1)
            {
                p.X = int.Parse(GRP[0]);
                p.Y = p.X;
            }
            else
            if (GRP.Length == 2)
            {
                p.X = int.Parse(GRP[0]);
                p.Y = int.Parse(GRP[0]);
            }
            else
            {
                throw new FormatException();
            }
            return p;
        }
        public override string ToString()
        {
            return $"{X},{Y}";
        }
    }
    [Serializable]
    public class SRIFloatPoint
    {
        public float X;
        public float Y;
        public SRIFloatPoint()
        {
            X = 0.0f;
            Y = 0.0f;
        }
        public static SRIFloatPoint Parse(string str)
        {
            SRIFloatPoint p = new SRIFloatPoint();
            var GRP = str.Split(',');
            if (GRP.Length == 1)
            {
                p.X = float.Parse(GRP[0]);
                p.Y = p.X;
            }
            else
            if (GRP.Length == 2)
            {
                p.X = float.Parse(GRP[0]);
                p.Y = float.Parse(GRP[1]);
            }
            else
            {
                throw new FormatException();
            }
            return p;
        }
        public override string ToString()
        {
            return $"{X},{Y}";
        }
    }
}
