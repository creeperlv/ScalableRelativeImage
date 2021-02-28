using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScalableRelativeImage.Nodes
{
    public class Group : GraphicNode
    {
        public List<INode> Children=new();
        public void RemoveChildAt(int i)
        {
            Children.RemoveAt(i);
        }
        public void RemoveChild(INode n)
        {
            Children.Remove(n);
        }
        public override List<INode> ListNodes()
        {
            return Children;
        }
        public override void AddNode(INode node)
        {
            Children.Add(node);
        }
        public override void Paint(ref Graphics TargetGraphics, RenderProfile profile)
        {
            foreach (var item in Children)
            {
                if(item is GraphicNode)
                {
                    ((GraphicNode)item).Paint(ref TargetGraphics, profile);
                }
            }
        }
    }
}
