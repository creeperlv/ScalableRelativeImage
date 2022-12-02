using ImageMagick;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRI.Core.Utilities
{
    public static class Backends
    {
        static float B16B8Converter = 65535 / 255;
        public static MagickColor ToMagick(this Color c)
        {
            return new MagickColor(c.R * B16B8Converter, c.G * B16B8Converter, c.B * B16B8Converter, c.A * B16B8Converter);
        }
    }
}
