using OpenCL.NetCore;
using ScalableRelativeImage.Core;
using SRI.Core.Backend;
using SRI.Core.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;

namespace ScalableRelativeImage.Nodes.MathNodes
{
    /// <summary>
    /// Multi-purpose single parameter function.
    /// </summary>
    public class SingleParameterFunction : GraphicNode
    {
        static float D2R = MathF.PI / 180f;
        static float R2D = 180f / MathF.PI;
        public string Symbol;
        public string Value;
        public string Function = "Sin";
        public override void SetValue(string Key, string Value, ref List<ExecutionWarning> executionWarnings)
        {
            switch (Key)
            {
                case "Out":
                case "out":
                case "Name":
                case "N":
                case "Symbol":
                case "S":
                    Symbol = Value;
                    break;
                case "In":
                case "in":
                case "Value":
                case "V":
                    this.Value = Value;
                    break;
                case "Func":
                case "F":
                case "Function":
                    this.Function = Value;
                    break;
                default:
                    base.SetValue(Key, Value, ref executionWarnings);
                    break;
            }
        }
        public override void Paint(ref DrawableImage TargetGraphics, RenderProfile profile)
        {
            var __in = IntermediateValue.GetFloat(Value, profile.CurrentSymbols);
            float result = __in;
            switch (Function.ToUpper())
            {
                case "SIN":
                    result = MathF.Sin(__in);
                    break;
                case "COS":
                    result = MathF.Cos(__in);
                    break;
                case "DEG":
                    result = __in * R2D;
                    break;
                case "TAN":
                    result = MathF.Tan(__in);
                    break;
                case "TANH":
                    result = MathF.Tanh(__in);
                    break;
                case "SINH":
                    result = MathF.Sinh(__in);
                    break;
                case "COSH":
                    result = MathF.Cosh(__in);
                    break;
                case "RAD":
                    result = __in * D2R;
                    break;
                default:
                    break;
            }
            profile.CurrentSymbols.Set(Symbol, result.ToString());

        }
        public override Dictionary<string, string> GetValueSet()
        {
            Dictionary<string, string> dict = new()
            {
                { "Out", Symbol },
                { "In", Value },
                {"Func",Function}
            };
            return dict;
        }
    }
}