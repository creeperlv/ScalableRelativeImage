using Microsoft.VisualBasic;
using ScalableRelativeImage.Core;
using SRI.Core.Backend;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ScalableRelativeImage.Nodes
{
    public class Compare : GraphicNode
    {
        string L;
        string R;
        string T;
        string M = "EQ";
        string VType = "Float";
        public override Dictionary<string, string> GetValueSet()
        {
            Dictionary<string, string> valueSet = new Dictionary<string, string>
            {
                { "Left", L },
                { "Right", R },
                { "Target", T },
                { "Method", T },
                { "ValueType", VType }
            };
            return valueSet;
        }
        public override void SetValue(string Key, string Value, ref List<ExecutionWarning> executionWarnings)
        {
            switch (Key.ToUpper())
            {
                case "L":
                case "LEFT":
                    L = Value;
                    break;
                case "R":
                case "RIGHT":
                    R = Value;
                    break;
                case "T":
                case "TARGET":
                    T = Value;
                    break;
                case "TYPE":
                case "VALUETYPE":
                    VType = Value;
                    break;
                case "M":
                case "METHOD":
                    M = Value;
                    break;
                default:
                    base.SetValue(Key, Value, ref executionWarnings);
                    break;
            }
        }
        enum CompMethod
        {
            EQ, NE, LT, GT, GE, LE,
        }

        public override void Paint(ref DrawableImage TargetGraphics, RenderProfile profile)
        {
            IntermediateValue iL = L;
            IntermediateValue iR = R;
            Trace.WriteLine($"Left:{L},{iL}");
            Trace.WriteLine($"Right:{R},{iR}");
            CompMethod m = CompMethod.EQ;
            switch (M.ToUpper())
            {
                case "EQ":
                case "EQUALS":
                    m = CompMethod.EQ;
                    break;
                case "NE":
                case "NOTE":
                case "NOTEQUALS":
                    m = CompMethod.NE;
                    break;
                case "LT":
                case "LESS":
                case "LESSTHAN":
                    m = CompMethod.LT;
                    break;
                case "GT":
                case "GREATER":
                case "GREATERTHAN":
                    m = CompMethod.GT;
                    break;
                case "LE":
                case "LESSOREQUAL":
                case "LESS_OR_EQUAL":
                case "LESSEQUAL":
                    m = CompMethod.LE;
                    break;
                case "GE":
                case "GREATEROREQUAL":
                case "GREATER_OR_EQUAL":
                case "GREATEREQUAL":
                    m = CompMethod.GE;
                    break;
                default:
                    break;
            }
            switch (VType.ToUpper())
            {
                case "BOOL":
                    {
                        switch (m)
                        {
                            case CompMethod.EQ:
                                profile.CurrentSymbols.Set(new Symbol { Name = T, Value = (iL.GetBool(profile.CurrentSymbols) == iR.GetBool(profile.CurrentSymbols)).ToString() });
                                break;
                            case CompMethod.NE:
                                profile.CurrentSymbols.Set(new Symbol { Name = T, Value = (iL.GetBool(profile.CurrentSymbols) != iR.GetBool(profile.CurrentSymbols)).ToString() });
                                break;
                            case CompMethod.LT:
                                break;
                            case CompMethod.GT:
                                break;
                            case CompMethod.GE:
                                break;
                            case CompMethod.LE:
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case "INT":
                case "INT32":
                case "INTEGER":
                    {
                        switch (m)
                        {
                            case CompMethod.EQ:
                                profile.CurrentSymbols.Set(new Symbol { Name = T, Value = (iL.GetInt(profile.CurrentSymbols) == iR.GetInt(profile.CurrentSymbols)).ToString() });
                                break;
                            case CompMethod.NE:
                                profile.CurrentSymbols.Set(new Symbol { Name = T, Value = (iL.GetInt(profile.CurrentSymbols) != iR.GetInt(profile.CurrentSymbols)).ToString() });
                                break;
                            case CompMethod.LT:
                                profile.CurrentSymbols.Set(new Symbol { Name = T, Value = (iL.GetInt(profile.CurrentSymbols) < iR.GetInt(profile.CurrentSymbols)).ToString() });
                                break;
                            case CompMethod.GT:
                                profile.CurrentSymbols.Set(new Symbol { Name = T, Value = (iL.GetInt(profile.CurrentSymbols) > iR.GetInt(profile.CurrentSymbols)).ToString() });
                                break;
                            case CompMethod.GE:
                                profile.CurrentSymbols.Set(new Symbol { Name = T, Value = (iL.GetInt(profile.CurrentSymbols) >= iR.GetInt(profile.CurrentSymbols)).ToString() });
                                break;
                            case CompMethod.LE:
                                profile.CurrentSymbols.Set(new Symbol { Name = T, Value = (iL.GetInt(profile.CurrentSymbols) <= iR.GetInt(profile.CurrentSymbols)).ToString() });
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case "FLOAT":
                case "SINGLE":
                    {
                        switch (m)
                        {
                            case CompMethod.EQ:
                                profile.CurrentSymbols.Set(new Symbol { Name = T, Value = (iL.GetFloat(profile.CurrentSymbols) == iR.GetFloat(profile.CurrentSymbols)).ToString() });
                                break;
                            case CompMethod.NE:
                                profile.CurrentSymbols.Set(new Symbol { Name = T, Value = (iL.GetFloat(profile.CurrentSymbols) != iR.GetFloat(profile.CurrentSymbols)).ToString() });
                                break;
                            case CompMethod.LT:
                                profile.CurrentSymbols.Set(new Symbol { Name = T, Value = (iL.GetFloat(profile.CurrentSymbols) < iR.GetFloat(profile.CurrentSymbols)).ToString() });
                                break;
                            case CompMethod.GT:
                                profile.CurrentSymbols.Set(new Symbol { Name = T, Value = (iL.GetFloat(profile.CurrentSymbols) > iR.GetFloat(profile.CurrentSymbols)).ToString() });
                                break;
                            case CompMethod.GE:
                                profile.CurrentSymbols.Set(new Symbol { Name = T, Value = (iL.GetFloat(profile.CurrentSymbols) >= iR.GetFloat(profile.CurrentSymbols)).ToString() });
                                break;
                            case CompMethod.LE:
                                profile.CurrentSymbols.Set(new Symbol { Name = T, Value = (iL.GetFloat(profile.CurrentSymbols) <= iR.GetFloat(profile.CurrentSymbols)).ToString() });
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case "DOUBLE":
                    {
                        switch (m)
                        {
                            case CompMethod.EQ:
                                profile.CurrentSymbols.Set(new Symbol { Name = T, Value = (iL.GetDouble(profile.CurrentSymbols) == iR.GetDouble(profile.CurrentSymbols)).ToString() });
                                break;
                            case CompMethod.NE:
                                profile.CurrentSymbols.Set(new Symbol { Name = T, Value = (iL.GetDouble(profile.CurrentSymbols) != iR.GetDouble(profile.CurrentSymbols)).ToString() });
                                break;
                            case CompMethod.LT:
                                profile.CurrentSymbols.Set(new Symbol { Name = T, Value = (iL.GetDouble(profile.CurrentSymbols) < iR.GetDouble(profile.CurrentSymbols)).ToString() });
                                break;
                            case CompMethod.GT:
                                profile.CurrentSymbols.Set(new Symbol { Name = T, Value = (iL.GetDouble(profile.CurrentSymbols) > iR.GetDouble(profile.CurrentSymbols)).ToString() });
                                break;
                            case CompMethod.GE:
                                profile.CurrentSymbols.Set(new Symbol { Name = T, Value = (iL.GetDouble(profile.CurrentSymbols) >= iR.GetDouble(profile.CurrentSymbols)).ToString() });
                                break;
                            case CompMethod.LE:
                                profile.CurrentSymbols.Set(new Symbol { Name = T, Value = (iL.GetDouble(profile.CurrentSymbols) <= iR.GetDouble(profile.CurrentSymbols)).ToString() });
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                default:
                    break;
            }

        }
    }

}
