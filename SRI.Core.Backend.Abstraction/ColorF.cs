using System.Diagnostics;

namespace SRI.Core.Backend
{
    public struct ColorF
    {
        public float R;
        public float G;
        public float B;
        public float A;
        public override string ToString()
        {
            return String.Format("{0},{1},{2},{3}",R,G,B,A);
        }
        public string ToString(string format)
        {
            var str = $"{{3:{format}2}}{{0:{format}2}}{{1:{format}2}}{{2:{format}2}}";
            Debug.WriteLine(str);
            return String.Format(str, (int)R, (int)G, (int)B, (int)A);
        }
        public ColorF(float r, float g, float b, float a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }
    }
}
