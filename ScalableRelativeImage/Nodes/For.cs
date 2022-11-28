using OpenCL.NetCore;
using ScalableRelativeImage.Core;
using System.Collections.Generic;
using System.Drawing;

namespace ScalableRelativeImage.Nodes
{
    /// <summary>
    /// For, act like for (float i=0; i<10;i+=1){ ... }
    /// </summary>
    public class For : GraphicNode
    {
        /// <summary>
        /// Name of variable
        /// </summary>
        public IntermediateValue Variable = "i";
        public IntermediateValue InitialValue = 0;
        public IntermediateValue EndValue = 10;
        public IntermediateValue Step = 1;
        public List<GraphicNode> Nodes = new List<GraphicNode>();


        public override void SetValue(string Key, string Value, ref List<ExecutionWarning> executionWarnings)
        {
            switch (Key)
            {
                case "Var":
                case "Name":
                case "Symbol":
                case "Sym":
                case "SymbolName":
                case "Variable":
                    Variable = Value;
                    break;
                case "InitialValue":
                    InitialValue = Value;
                    break;
                case "EndValue":
                    EndValue = Value;
                    break;
                case "Step":
                    Step = Value;
                    break;
                default:
                    base.SetValue(Key, Value, ref executionWarnings);
                    break;
            }
        }
        public override Dictionary<string, string> GetValueSet()
        {
            Dictionary<string, string> dict = new()
            {
                { "Variable", Variable },
                { "InitialValue", InitialValue },
                { "EndValue", EndValue },
                { "Step", Step }
            };
            return dict;
        }
        public override void AddNode(INode node, ref List<ExecutionWarning> executionWarnings)
        {
            if (node is GraphicNode)
                Nodes.Add(node as GraphicNode);
            else executionWarnings.Add(new ShapeMismatchWarning(node, typeof(GraphicNode)));
        }
        public override List<INode> ListNodes()
        {
            List<INode> nodes = new List<INode>();
            foreach (var item in Nodes)
            {
                nodes.Add(item);
            }
            return nodes;
        }
        public override void Paint(ref Graphics TargetGraphics, RenderProfile profile)
        {
            var L = InitialValue.GetFloat(profile.CurrentSymbols);
            var R = EndValue.GetFloat(profile.CurrentSymbols);
            var Delta = Step.GetFloat(profile.CurrentSymbols);
            for (float i = L; i <= R; i += Delta)
            {
                profile.CurrentSymbols.Set(Variable.GetString(profile.CurrentSymbols), i.ToString());
                foreach (var item in Nodes)
                {
                    item.Paint(ref TargetGraphics, profile);
                }
            }
        }
    }
}
