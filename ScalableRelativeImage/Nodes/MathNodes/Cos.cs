using OpenCL.NetCore;
using ScalableRelativeImage.Core;
using SRI.Core.Backend;
using SRI.Core.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;

namespace ScalableRelativeImage.Nodes.MathNodes
{
    /// <summary>
    /// Cos function.
    /// </summary>
    public class Cos : GraphicNode
    {
        public string Symbol;
        public IntermediateValue Value=new IntermediateValue { };
        public string _Type = "Float";
        public override void SetValue(string Key, string Value, ref List<ExecutionWarning> executionWarnings)
        {
            switch (Key)
            {
                case "Out":
                case "Name":
                case "N":
                case "Symbol":
                case "S":
                    Symbol = Value;
                    break;
                case "In":
                case "Value":
                case "V":
                    this.Value.Value=Value;
                    break;
                case "T":
                case "TYPE":
                    this._Type = Value;
                    break;
                default:
                    base.SetValue(Key, Value, ref executionWarnings);
                    break;
            }
        }
        public override void Paint(ref DrawableImage TargetGraphics, RenderProfile profile)
        {
            switch (_Type.ToUpper())
            {
                case "FLOAT":
                case "F":
                case "SINGLE":
                    profile.CurrentSymbols.Set(new Symbol { Name = Symbol, Value = MathF.Cos(Value.GetFloat(profile.CurrentSymbols)).ToString() });
                    break;
                case "DOUBLE":
                case "D":
                    profile.CurrentSymbols.Set(new Symbol { Name = Symbol, Value = Math.Cos(Value.GetDouble(profile.CurrentSymbols)).ToString() });
                    break;
                case "INT":
                case "I":
                case "INTEGER":
                    profile.CurrentSymbols.Set(new Symbol { Name = Symbol, Value = ((int)Math.Cos(Value.GetInt(profile.CurrentSymbols))).ToString()});
                    break;
                default:
                    profile.CurrentSymbols.Set(new Symbol { Name = Symbol, Value = Value });
                    break;
            }
        }
        public override Dictionary<string, string> GetValueSet()
        {
            Dictionary<string, string> dict = new()
            {
                { "Name", Symbol },
                { "Value", Value },
                {
                    "Type",_Type
                }
            };
            return dict;
        }
    }
}