using System;

namespace ScalableRelativeImage.Core
{
    public class CalcuatorTypeHelper<T>
    {
        public Func<Object, Object, T> Add;
        public Func<Object, Object, T> Sub;
        public Func<Object, Object, T> Mul;
        public Func<Object, Object, T> Div;
        public Func<string, SymbolHelper, TResult<T>> Convert;
    }
    public struct TResult<T>
    {
        public T Value;
        public bool Succeed;

        public TResult(T value, bool succeed)
        {
            Value = value;
            Succeed = succeed;
        }
        public override string ToString()
        {
            return $"(V:{Value},S:{Succeed})";
        }
    }
}
