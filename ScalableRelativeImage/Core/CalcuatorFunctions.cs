namespace ScalableRelativeImage.Core
{
    /// <summary>
    /// Pre-defined helpers for different types.
    /// </summary>
    public static class CalcuatorFunctions
    {
        public static readonly CalcuatorTypeHelper<float> FloatCalcuator;
        public static readonly CalcuatorTypeHelper<double> DoubleCalcuator;
        public static readonly CalcuatorTypeHelper<int> IntCalcuator;
        static CalcuatorFunctions()
        {
            FloatCalcuator = new CalcuatorTypeHelper<float>
            {
                Add = (a, b) =>
                {
                    return a + b;
                },
                Sub = (a, b) =>
                {
                    return a - b;
                },
                Mul = (a, b) =>
                {
                    return a * b;
                },
                Div = (a, b) =>
                {
                    return a / b;
                },
                Convert = (a, s) =>
                {
                    var b = IntermediateValue.TryGetFloat(a, s, out var f);
                    return new TResult<float>(f, b);
                }
            };
            DoubleCalcuator = new CalcuatorTypeHelper<double>
            {
                Add = (a, b) =>
                {
                    return a + b;
                },
                Sub = (a, b) =>
                {
                    return a - b;
                },
                Mul = (a, b) =>
                {
                    return a * b;
                },
                Div = (a, b) =>
                {
                    return a / b;
                },
                Convert = (a, s) =>
                {
                    var b = IntermediateValue.TryGetDouble(a, s, out var f);
                    return new TResult<double>(f, b);
                }
            };
            IntCalcuator = new CalcuatorTypeHelper<int>
            {
                Add = (a, b) =>
                {
                    return a + b;
                },
                Sub = (a, b) =>
                {
                    return a - b;
                },
                Mul = (a, b) =>
                {
                    return a * b;
                },
                Div = (a, b) =>
                {
                    return a / b;
                },
                Convert = (a, s) =>
                {
                    var b = IntermediateValue.TryGetInt(a, s, out var f);
                    return new TResult<int>(f, b);
                }
            };
        }
    }
}
