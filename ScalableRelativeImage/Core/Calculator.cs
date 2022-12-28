using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace ScalableRelativeImage.Core
{

    [System.Serializable]
    public class ExpressionSegmentNotResolvableException : System.Exception
    {
        public ExpressionSegmentNotResolvableException() { }
        protected ExpressionSegmentNotResolvableException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// A simple calcuator that is able to perform +-*/ and ().
    /// </summary>
    public static class Calculator
    {
        static string pattern = @"([^\w\.])|[\w\.]+";
        /// <summary>
        /// Resolve a input string to an object list.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        /// <exception cref="ExpressionSegmentNotResolvableException"></exception>
        public static List<object> Resolve(string input,SymbolHelper s)
        {
            var m = Regex.Matches(input, pattern);
            if (m.Count == 1)
            {
                if (s.TryLookup(m[0].Value,out _) == false)
                {
                    throw new ExpressionSegmentNotResolvableException();
                }
            }
            List<object> result = new List<object>();
            foreach (Match item in m)
            {
                result.Add(item.Value);
            }
            return result;
        }
        public static float CalcuateFloat(string input, SymbolHelper s)
        {
            return CalcuateFloat(Resolve(input,s), s).Item1;
        }
        /// <summary>
        /// Calcuate 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="Helper"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static T Calcuate<T>(string input,CalcuatorTypeHelper<T>Helper, SymbolHelper s)
        {
            return CalcuateT<T>(Resolve(input,s), s, Helper).Item1;
        }
        public static (T, int) CalcuateT<T>(List<object> inputs, SymbolHelper s, CalcuatorTypeHelper<T> H, int Index = 0)
        {

            List<object> NewStack = new List<object>();
            List<object> NewStack2 = new List<object>();
            for (int i = Index; i < inputs.Count; i++)
            {
                var item = inputs[i];
                if (item is string str)
                {
                    if (str[0] == '(')
                    {
                        var res = CalcuateT(inputs, s, H, i + 1);
                        NewStack.Add(res.Item1);
                        i += res.Item2;
                    }
                    else
                    {
                        NewStack.Add(item);
                    }
                }
                else
                {
                    NewStack.Add(item);
                }
            }
            bool stop = false;
            int count = 0;

            for (int i = 0; i < NewStack.Count && !stop; i++)
            {
                var item = NewStack[i];
                if (item is string str)
                {
                    switch (str[0])
                    {
                        case '*':
                            {
                                T LV;
                                var LS = NewStack2.Last();
                                if (LS is T f)
                                {
                                    LV = f;
                                }
                                else
                                {
                                    var res = H.Convert(LS as string, s);// IntermediateValue.GetFloat(LS as string, s);
                                    if(res.Succeed)
                                    {

                                        LV = res.Value;
                                    }
                                    else
                                    {
                                        throw new ExpressionSegmentNotResolvableException();
                                    }
                                }

                                T RV;
                                var RS = NewStack[i + 1];
                                if (RS is T f2)
                                {
                                    RV = f2;
                                }
                                else
                                {
                                    var res = H.Convert(RS as string, s);
                                    if (res.Succeed)
                                    {

                                        RV = res.Value;
                                    }
                                    else
                                    {
                                        throw new ExpressionSegmentNotResolvableException();
                                    }
                                }
                                NewStack2[NewStack2.Count - 1] = H.Mul(LV, RV);
                                i++;
                                count++;
                            }
                            break;
                        case '/':
                            {
                                T LV;
                                var LS = NewStack2.Last();
                                if (LS is T f)
                                {
                                    LV = f;
                                }
                                else
                                {
                                    var res = H.Convert(LS as string, s);// IntermediateValue.GetFloat(LS as string, s);
                                    if (res.Succeed)
                                    {

                                        LV = res.Value;
                                    }
                                    else
                                    {
                                        throw new ExpressionSegmentNotResolvableException();
                                    }
                                }
                                T RV;
                                var RS = NewStack[i + 1];
                                if (RS is T f2)
                                {
                                    RV = f2;
                                }
                                else
                                {
                                    var res = H.Convert(RS as string, s);
                                    if (res.Succeed)
                                    {

                                        RV = res.Value;
                                    }
                                    else
                                    {
                                        throw new ExpressionSegmentNotResolvableException();
                                    }
                                }
                                NewStack2[NewStack2.Count - 1] = H.Div(LV, RV);
                                i++;
                                count++;
                            }
                            break;
                        case ')':
                            stop = true;
                            break;
                        default:
                            NewStack2.Add(item);
                            break;
                    }
                }
                else
                {

                    NewStack2.Add(item);
                }
                count++;
            }
            T L = default;
            //Console.WriteLine($"NewStack2:{NewStack2.Count}");
            //NewStack2.ForEach((x) => Console.Write($"{x}({x.GetType()})\t"));
            //Console.ReadLine();
            if (NewStack2[0] is T vvv)
            {
                L = vvv;
            }
            else
            {
                var res = H.Convert(NewStack2[0] as string, s);// IntermediateValue.GetFloat(LS as string, s);
                //Console.WriteLine(res);
                if (res.Succeed)
                {
                    L = res.Value;
                }
                else
                {
                    throw new ExpressionSegmentNotResolvableException();
                }
            }
            for (int i = 1; i < NewStack2.Count; i++)
            {
                var item = NewStack2[i];
                if (item is string str)
                {
                    switch (str[0])
                    {
                        case '+':
                            {

                                T RV;
                                var RS = NewStack2[i + 1];
                                if (RS is T f2)
                                {
                                    RV = f2;
                                }
                                else
                                {

                                    //RV = H.Convert(RS as string, s);
                                    var res = H.Convert(RS as string, s);
                                    if (res.Succeed)
                                    {

                                        RV = res.Value;
                                    }
                                    else
                                    {
                                        throw new ExpressionSegmentNotResolvableException();
                                    }
                                }
                                L = H.Add(L, RV);
                                i++;
                            }
                            break;
                        case '-':
                            {

                                T RV;
                                var RS = NewStack2[i + 1];
                                if (RS is T f2)
                                {
                                    RV = f2;
                                }
                                else
                                {
                                    //RV = H.Convert(RS as string, s);

                                    var res = H.Convert(RS as string, s);
                                    if (res.Succeed)
                                    {

                                        RV = res.Value;
                                    }
                                    else
                                    {
                                        throw new ExpressionSegmentNotResolvableException();
                                    }
                                }
                                L = H.Sub(L, RV);
                                i++;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            return (L, count);
        }
        public static (float, int) CalcuateFloat(List<object> inputs, SymbolHelper s, int Index = 0)
        {
            List<object> NewStack = new List<object>();
            List<object> NewStack2 = new List<object>();
            for (int i = Index; i < inputs.Count; i++)
            {
                var item = inputs[i];
                if (item is string str)
                {
                    if (str[0] == '(')
                    {
                        var res = CalcuateFloat(inputs, s, i + 1);
                        NewStack.Add(res.Item1);
                        i += res.Item2;
                    }
                    else
                    {
                        NewStack.Add(item);
                    }
                }
                else
                {
                    NewStack.Add(item);
                }
            }
            bool stop = false;
            int count = 0;

            for (int i = 0; i < NewStack.Count && !stop; i++)
            {
                var item = NewStack[i];
                if (item is string str)
                {
                    switch (str[0])
                    {
                        case '*':
                            {
                                float LV;
                                var LS = NewStack2.Last();
                                if (LS is float f)
                                {
                                    LV = f;
                                }
                                else
                                {
                                    LV = IntermediateValue.GetFloat(LS as string, s);
                                }

                                float RV;
                                var RS = NewStack[i + 1];
                                if (RS is float f2)
                                {
                                    RV = f2;
                                }
                                else
                                {
                                    RV = IntermediateValue.GetFloat(RS as string, s);
                                }
                                NewStack2[NewStack2.Count - 1] = LV * RV;
                                i++;
                                count++;
                            }
                            break;
                        case '/':
                            {
                                float LV;
                                var LS = NewStack2.Last();
                                if (LS is float f)
                                {
                                    LV = f;
                                }
                                else
                                {
                                    LV = IntermediateValue.GetFloat(LS as string, s);
                                }
                                float RV;
                                var RS = NewStack[i + 1];
                                if (RS is float f2)
                                {
                                    RV = f2;
                                }
                                else
                                {
                                    RV = IntermediateValue.GetFloat(RS as string, s);
                                }
                                NewStack2[NewStack2.Count - 1] = LV / RV;
                                i++;
                                count++;
                            }
                            break;
                        case ')':
                            stop = true;
                            break;
                        default:
                            NewStack2.Add(item);
                            break;
                    }
                }
                else
                {

                    NewStack2.Add(item);
                }
                count++;
            }
            float L = 0;
            if (NewStack2[0] is float vvv)
            {
                L = vvv;
            }
            else
            {
                L = IntermediateValue.GetFloat(NewStack2[0] as string, s);
            }
            for (int i = 1; i < NewStack2.Count; i++)
            {
                var item = NewStack2[i];
                if (item is string str)
                {
                    switch (str[0])
                    {
                        case '+':
                            {

                                float RV;
                                var RS = NewStack2[i + 1];
                                if (RS is float f2)
                                {
                                    RV = f2;
                                }
                                else
                                {

                                    RV = IntermediateValue.GetFloat(RS as string, s);
                                }
                                L += RV;
                                i++;
                            }
                            break;
                        case '-':
                            {

                                float RV;
                                var RS = NewStack2[i + 1];
                                if (RS is float f2)
                                {
                                    RV = f2;
                                }
                                else
                                {
                                    RV = IntermediateValue.GetFloat(RS as string, s);
                                }
                                L -= RV;
                                i++;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            return (L, count);
        }
    }
}
