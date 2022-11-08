using System.Collections.Generic;
using System.Drawing;

namespace ScalableRelativeImage.Nodes
{
    public class SetSymbol: GraphicNode
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
        public override void Paint(ref Graphics TargetGraphics, RenderProfile profile)
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