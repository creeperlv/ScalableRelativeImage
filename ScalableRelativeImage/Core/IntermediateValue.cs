using System;
using System.Drawing;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ScalableRelativeImage.Core
{
    /// <summary>
    /// Intermediate Value: use for both direct value or convert from symbol.
    /// </summary>
    public class IntermediateValue
    {
        static readonly Type IntT = typeof(int);
        public string Value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            return Value;
        }
        public static bool TryGetInt(string Value, SymbolHelper s, out int result, int Fallback = 0)
        {

            if (int.TryParse(Value, out int i))
            {
                result = i;
                return true;
            }
            else
            {
                var b = s.TryLookup(Value, out var I, Fallback.ToString());
                if (b == false)
                {
                    try
                    {
                        result = Calculator.Calcuate(Value, CalcuatorFunctions.IntCalcuator, s);
                        return true;
                    }
                    catch (Exception)
                    {
                    }

                }
                else
                {
                    if (int.TryParse(I, out int r))
                    {
                        result =r;
                        return true;
                    }
                    else
                    {
                        try
                        {

                            result = Calculator.Calcuate(I, CalcuatorFunctions.IntCalcuator, s);
                            return true;
                            //return Calculator.Calcuate(I, CalcuatorFunctions.FloatCalcuator, s);
                        }
                        catch (Exception)
                        {
                        }
                    }
                }

            }
            result = Fallback;
            return false;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetInt(string Value, SymbolHelper s, int Fallback = 0)
        {

            TryGetInt(Value, s, out var result, Fallback);
            return result;

        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetInt(SymbolHelper s, int Fallback = 0)
        {
            return GetInt(Value, s, Fallback);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool GetBool(SymbolHelper s, bool Fallback = false)
        {

            if (bool.TryParse(Value, out bool i))
            {
                return i;
            }
            else
            {
                if (bool.TryParse(s.Lookup(Value, Fallback.ToString()), out bool r))
                {
                    return r;
                }

            }
            return Fallback;
        }
        public static bool TryGetFloat(string Value, SymbolHelper s, out float result, float Fallback = 0)
        {

            if (float.TryParse(Value, out float i))
            {
                result = i;
                return true;
            }
            else
            {
                var b = s.TryLookup(Value, out var I, Fallback.ToString());
                //Console.WriteLine($"Hit:{b}\tValuie:{I}");
                if (b == false)
                {
                    try
                    {
                        result = Calculator.Calcuate(Value, CalcuatorFunctions.FloatCalcuator, s);
                        return true;
                    }
                    catch (Exception)
                    {
                    }

                }
                else
                {
                    if (float.TryParse(I, out float r))
                    {
                        result = r;
                        return true;
                    }
                    else
                    {
                        try
                        {

                            result = Calculator.Calcuate(I, CalcuatorFunctions.FloatCalcuator, s);
                            return true;
                            //return Calculator.Calcuate(I, CalcuatorFunctions.FloatCalcuator, s);
                        }
                        catch (Exception)
                        {
                        }
                    }
                }

            }
            result = Fallback;
            return false;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetFloat(string Value, SymbolHelper s, float Fallback = 0f)
        {
            TryGetFloat(Value, s, out var result, Fallback);
            return result;
     
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float GetFloat(SymbolHelper s, float Fallback = 0f)
        {
            return GetFloat(Value, s, Fallback);
        }
        public static bool TryGetDouble(string Value, SymbolHelper s, out double result, double Fallback = 0)
        {

            if (float.TryParse(Value, out float i))
            {
                result = i;
                return true;
            }
            else
            {
                var b = s.TryLookup(Value, out var I, Fallback.ToString());
                if (b == false)
                {
                    try
                    {
                        result = Calculator.Calcuate(Value, CalcuatorFunctions.DoubleCalcuator, s);
                        return true;
                    }
                    catch (Exception)
                    {
                    }

                }
                else
                {
                    if (double.TryParse(I, out double r))
                    {
                        result = r;
                        return true;
                    }
                    else
                    {
                        try
                        {

                            result = Calculator.Calcuate(I, CalcuatorFunctions.DoubleCalcuator, s);
                            return true;
                            //return Calculator.Calcuate(I, CalcuatorFunctions.FloatCalcuator, s);
                        }
                        catch (Exception)
                        {
                        }
                    }
                }

            }
            result = Fallback;
            return false;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double GetDouble(string Value, SymbolHelper s, double Fallback = 0f)
        {
            TryGetDouble(Value, s, out var result, Fallback);
            return result;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double Getdouble(SymbolHelper s, double Fallback = 0f)
        {
            return GetDouble(Value, s, Fallback);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetString(SymbolHelper s, string Fallback = "")
        {
            if (Value.StartsWith("#"))
            {
                return s.Lookup(Value.Substring(1), Fallback);
            }
            return Value;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Color GetColor(SymbolHelper s, string Fallback = "White")
        {

            try
            {
                var C = (Color)SRIAnalyzer.cc.ConvertFromString(Value);
                return C;
            }
            catch (Exception)
            {
                try
                {
                    var CC = (Color)SRIAnalyzer.cc.ConvertFromString(s.Lookup(Value, Fallback));
                    return CC;
                }
                catch (Exception)
                {
                }
            }
            return (Color)SRIAnalyzer.cc.ConvertFromString(Fallback);
        }
        public static implicit operator string(IntermediateValue v)
        {
            return v.ToString();
        }
        public static implicit operator IntermediateValue(string str)
        {
            return new IntermediateValue { Value = str };
        }
        public static implicit operator IntermediateValue(float v)
        {
            return new IntermediateValue { Value = v.ToString() };
        }
    }
}
