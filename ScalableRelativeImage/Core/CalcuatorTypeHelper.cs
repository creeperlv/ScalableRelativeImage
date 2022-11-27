using System;

namespace ScalableRelativeImage.Core
{
    /// <summary>
    /// Basic operations for T.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CalcuatorTypeHelper<T>
    {
        public Func<T, T, T> Add;
        public Func<T, T, T> Sub;
        public Func<T, T, T> Mul;
        public Func<T, T, T> Div;
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
