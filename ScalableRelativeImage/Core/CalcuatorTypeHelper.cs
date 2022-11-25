using System;

namespace ScalableRelativeImage.Core
{
    public class CalcuatorTypeHelper<T>
    {
        public Func<Object, Object, T> Add;
        public Func<Object, Object, T> Sub;
        public Func<Object, Object, T> Mul;
        public Func<Object, Object, T> Div;
        public Func<string, SymbolHelper, T> Convert;
    }
}
