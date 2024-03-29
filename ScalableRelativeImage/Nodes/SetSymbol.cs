﻿using ScalableRelativeImage.Core;
using SRI.Core.Backend;
using SRI.Core.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;

namespace ScalableRelativeImage.Nodes
{
    /// <summary>
    /// Set a symbol and treat Value as expression when Type is int, float and double.
    /// </summary>
    public class SetSymbolExp : GraphicNode
    {
        public string Symbol;
        public string Value;
        public string _Type;
        public override void SetValue(string Key, string Value, ref List<ExecutionWarning> executionWarnings)
        {
            switch (Key)
            {
                case "Name":
                case "N":
                case "Symbol":
                case "S":
                    Symbol = Value;
                    break;
                case "Value":
                case "V":
                    this.Value = Value;
                    break;
                case "T":
                case "Type":
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
                    {
                        var v = IntermediateValue.GetFloat(Value, profile.CurrentSymbols).ToString();
                        profile.CurrentSymbols.Set(Symbol, v.ToString());
                    }
                    break;
                case "DOUBLE":
                case "D":
                    {
                        var v = IntermediateValue.GetDouble(Value, profile.CurrentSymbols).ToString();
                        profile.CurrentSymbols.Set(Symbol, v.ToString());
                    }
                    break;
                case "INT":
                case "I":
                case "INTEGER":
                    {
                        var v = IntermediateValue.GetInt(Value, profile.CurrentSymbols).ToString();
                        profile.CurrentSymbols.Set(Symbol, v.ToString());
                    }
                    break;
                default:
                    profile.CurrentSymbols.Set(Symbol, Value.ToString());
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
    /// <summary>
    /// Set symbol.
    /// </summary>
    public class SetSymbol : GraphicNode
    {
        public string Symbol;
        public string Value;
        public override void SetValue(string Key, string Value, ref List<ExecutionWarning> executionWarnings)
        {
            switch (Key)
            {
                case "Name":
                case "N":
                case "Symbol":
                case "S":
                    Symbol = Value;
                    break;
                case "Value":
                case "V":
                    this.Value = Value;
                    break;
                default:
                    base.SetValue(Key, Value, ref executionWarnings);
                    break;
            }
        }
        public override void Paint(ref DrawableImage TargetGraphics, RenderProfile profile)
        {
            profile.CurrentSymbols.Set(new Symbol { Name = Symbol, Value = Value });
        }
        public override Dictionary<string, string> GetValueSet()
        {
            Dictionary<string, string> dict = new()
            {
                { "Name", Symbol },
                { "Value", Value }
            };
            return dict;
        }
    }
}