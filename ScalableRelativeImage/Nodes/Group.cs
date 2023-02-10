using ScalableRelativeImage.Core;
using SRI.Core.Backend;
using SRI.Core.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScalableRelativeImage.Nodes
{
    /// <summary>
    /// Group a batch of nodes, if the Visible of a group is set to false, paint() in children will not be executed at all.
    /// </summary>
    public class Group : GraphicNode
    {
        public List<INode> Children = new();
        public IntermediateValue Visible = true;
        public override void SetValue(string Key, string Value, ref List<ExecutionWarning> executionWarnings)
        {
            switch (Key)
            {
                case "Visible":
                    {
                        Visible.Value = Value;
                    }
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
                { "Visible", Visible.ToString() }
            };
            return dict;
        }
        public void RemoveChildAt(int i)
        {
            Children.RemoveAt(i);
        }
        public void RemoveChild(INode n)
        {
            var node = (n as GraphicNode);
            node.Parent = this;
            Children.Remove(node);
        }
        public override List<INode> ListNodes()
        {
            return Children;
        }
        public override void AddNode(INode node, ref List<ExecutionWarning> executionWarnings)
        {
            Children.Add(node);
        }
        public override void Paint(ref DrawableImage TargetGraphics, RenderProfile profile)
        {
            if (Visible == null)
            {

                foreach (var item in Children)
                {
                    if (item is GraphicNode)
                    {
                        ((GraphicNode)item).Paint(ref TargetGraphics, profile);
                    }
                }
            }
            else
            if (Visible.GetBool(profile.CurrentSymbols,false) == true)
            {
                foreach (var item in Children)
                {
                    if (item is GraphicNode)
                    {
                        ((GraphicNode)item).Paint(ref TargetGraphics, profile);
                    }
                }

            }
        }
    }

}
