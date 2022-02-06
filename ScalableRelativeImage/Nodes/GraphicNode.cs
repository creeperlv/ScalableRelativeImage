using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace ScalableRelativeImage.Nodes
{
    /// <summary>
    /// Basic Node, implemented INode.
    /// </summary>
    public class GraphicNode : INode
    {
        public INode Parent=null;
        public ImageNodeRoot root;
        public virtual void AddNode(INode node,ref List<ExecutionWarning> executionWarnings)
        {
            executionWarnings.Add(new ShapeDisposedWarning(node));
#if DEBUG
            Trace.WriteLine($"Node has been disposed.");
#endif
        }

        public virtual Dictionary<string, string> GetValueSet()
        {
            return null;
        }

        public virtual List<INode> ListNodes()
        {
            return null;
        }

        public virtual void Paint(ref Graphics TargetGraphics, RenderProfile profile)
        {

        }

        public virtual void SetValue(string Key, string Value, ref List<ExecutionWarning> executionWarnings)
        {
            executionWarnings.Add(new DataDisposedWarning(Key, Value));
#if DEBUG
            Trace.WriteLine($"Value with key \"{Key}\" has been disposed.");
#endif
        }
    }
}