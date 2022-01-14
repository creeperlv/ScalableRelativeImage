using System.Collections.Generic;

namespace ScalableRelativeImage.Nodes
{
    public class Point : GraphicNode
    {
        public float X;
        public float Y;
        public override void SetValue(string Key, string Value, ref List<ExecutionWarning> executionWarnings)
        {
            switch (Key)
            {
                case "X":
                    X = float.Parse(Value);
                    break;
                case "Y":
                    Y = float.Parse(Value);
                    break;
                default:
                    base.SetValue(Key, Value,ref executionWarnings);
                    break;
            }
        }
        public override Dictionary<string, string> GetValueSet()
        {
            Dictionary<string, string> dict = new();
            dict.Add("X", X.ToString());
            dict.Add("Y", Y.ToString());
            return dict;
        }
    }
}