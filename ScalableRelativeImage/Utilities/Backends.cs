using SRI.Core.Backend;
using SRI.Core.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRI.Core.Utilities
{
    public static class Backends
    {
        public static UniversalRectangle ToUR(this Rectangle r)
        {
            return new UniversalRectangle(r.X,r.Y,r.Width,r.Height);
        }
        public static ColorF ToColorF(this Color v)
        {
            return new ColorF(v.R, v.G, v.B, v.A);
        }
    }
}
