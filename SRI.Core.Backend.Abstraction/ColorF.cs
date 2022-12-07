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
            return String.Format($"{{3:{format}}}{{0:{format}}}{{1:{format}}}{{2:{format}}}", R, G, B, A);
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
