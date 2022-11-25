namespace ScalableRelativeImage.Core
{
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
                    return ((float)a) + (float)b;
                },
                Sub = (a, b) =>
                {
                    return ((float)a) - (float)b;
                },
                Mul = (a, b) =>
                {
                    return ((float)a) * (float)b;
                },
                Div = (a, b) =>
                {
                    return ((float)a) / (float)b;
                },
                Convert = (a, s) =>
                {
                    var b=IntermediateValue.TryGetFloat(a, s,out var f);
                    return new TResult<float> (f,b);
                }
            };
            DoubleCalcuator = new CalcuatorTypeHelper<double>
            {
                Add = (a, b) =>
                {
                    return ((double)a) + (double)b;
                },
                Sub = (a, b) =>
                {
                    return ((double)a) - (double)b;
                },
                Mul = (a, b) =>
                {
                    return ((double)a) * (double)b;
                },
                Div = (a, b) =>
                {
                    return ((double)a) / (double)b;
                },
                Convert = (a, s) =>
                {
                    var b=IntermediateValue.TryGetDouble(a, s,out var f);
                    return new TResult<double> (f,b);    
                }
            };
            IntCalcuator= new CalcuatorTypeHelper<int>
            {
                Add = (a, b) =>
                {
                    return ((int)a) + (int)b;
                },
                Sub = (a, b) =>
                {
                    return ((int)a) - (int)b;
                },
                Mul = (a, b) =>
                {
                    return ((int)a) * (int)b;
                },
                Div = (a, b) =>
                {
                    return ((int)a) / (int)b;
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
