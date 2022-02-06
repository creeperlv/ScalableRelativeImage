using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ScalableRelativeImage.Core
{
    public class IntermediateValue
    {
        static readonly Type IntT = typeof(int);
        public string Value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            return Value;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetInt(SymbolHelper s, int Fallback = 0)
        {

            if (int.TryParse(Value, out int i))
            {
                return i;
            }
            else
            {
                if (int.TryParse(s.Lookup(Value, Fallback.ToString()), out int r))
                {
                    return r;
                }

            }
            return Fallback;
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float GetFloat(SymbolHelper s, float Fallback = 0f)
        {

            if (float.TryParse(Value, out float i))
            {
                return i;
            }
            else
            {
                if (float.TryParse(s.Lookup(Value, Fallback.ToString()), out float r))
                {
                    return r;
                }

            }
            return Fallback;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double Getdouble(SymbolHelper s, double Fallback = 0f)
        {

            if (double.TryParse(Value, out double i))
            {
                return i;
            }
            else
            {
                if (double.TryParse(s.Lookup(Value, Fallback.ToString()), out double r))
                {
                    return r;
                }

            }
            return Fallback;
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
    }
}
