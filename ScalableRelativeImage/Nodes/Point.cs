using ScalableRelativeImage.Core;
using System.Collections.Generic;

namespace ScalableRelativeImage.Nodes
{
    public class Point : GraphicNode
    {
        public IntermediateValue X=new IntermediateValue { Value = "0" };
        public IntermediateValue Y=new IntermediateValue { Value = "0" };
        public override void SetValue(string Key, string Value, ref List<ExecutionWarning> executionWarnings)
        {
            switch (Key)
            {
                case "X":
                    X.Value=Value;
                    break;
                case "Y":
                    Y.Value = Value;
                    break;
                default:
                    base.SetValue(Key, Value,ref executionWarnings);
                    break;
            }
        }
        public override Dictionary<string, string> GetValueSet()
        {
            Dictionary<string, string> dict = new()
            {
                { "X", X.ToString() },
                { "Y", Y.ToString() }
            };
            return dict;
        }
    }
}