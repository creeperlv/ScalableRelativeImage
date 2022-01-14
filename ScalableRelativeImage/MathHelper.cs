﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScalableRelativeImage
{
    public struct MathHelper
    {
        public static float RAD = 0.01745329252f;
        public static float Deg2Rad(float Angle)
        {
            return Angle * RAD;
        }
        public static float Deg2Rad_P(float Angle)
        {
            return Angle * (MathF.PI/180);
        }
    }
}
