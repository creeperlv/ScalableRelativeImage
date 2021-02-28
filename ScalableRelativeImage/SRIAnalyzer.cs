using ScalableRelativeImage.Nodes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Xml;

namespace ScalableRelativeImage
{
    public class ImageReference
    {
        public string Namespace;
    }
    public class RenderProfile
    {
        public float TargetWidth;
        public float TargetHeight;
        public Color DefaultForeground = Color.White;
        public Color DefaultBackground = Color.Transparent;
        internal ImageNodeRoot root;

        public PointF FindTargetPoint(float RX, float RY)
        {
            PointF p = new(((RX / root._RelativeWidth) * TargetWidth), ((RY / root._RelativeHeight) * TargetHeight));
            return p;
        }
        public float FindAbsoluteSize(float RelativeSize)
        {
            if (RelativeSize > 0)
            {
                return (RelativeSize / MathF.Sqrt(root.RelativeArea)) * MathF.Sqrt(TargetWidth * TargetHeight);
            }
            else
            {
                return Math.Abs(RelativeSize);
            }
        }
    }
    public record ExecutionWarning
    {
        public string ID;
        public string Message;
        public ExecutionWarning(string ID, string Message)
        {
            this.ID = ID;
            this.Message = Message;
        }
    }
    public class SRIAnalyzer
    {
        internal static ColorConverter cc = new ColorConverter();
        public static ImageNodeRoot Parse(string Content, out List<ExecutionWarning> executionWarnings)
        {
            List<ExecutionWarning> ExecutionWarnings = new List<ExecutionWarning>();
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(Content);

            XmlNode RealRoot = null;
            for (int i = 0; i < xmlDocument.ChildNodes.Count; i++)
            {
                if (xmlDocument.ChildNodes[i].Name == "ScalableRelativeImage")
                {
                    RealRoot = xmlDocument.ChildNodes[i]; break;
                }
            }
            {
                //Check File Attributes;
                var attr = GetAttributes(RealRoot);
                foreach (var item in attr)
                {
                    switch (item.Key)
                    {
                        case "Flavor":
                            if (item.Value != SRIEngine.Flavor)
                            {
                                ExecutionWarnings.Add(new ExecutionWarning("SRI000", "File flavor does not match the flavor that this engine uses, result may vary."));
                            }
                            break;
                        case "FormatVersion":
                            if (Version.Parse(item.Value) > SRIEngine.FormatVersion)
                            {
                                ExecutionWarnings.Add(new ExecutionWarning("SRI001", "File format version is higher than the version that engine supports, result may broken."));
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            var l = RealRoot.ChildNodes;
            XmlNode ImageNodeRootNode = null;
            ImageNodeRoot ImageRoot;
            List<ImageReference> references = new List<ImageReference>();
            for (int i = 0; i < l.Count; i++)
            {
                var item = l.Item(i);
                if (item.Name == "ImageNodeRoot")
                {
                    ImageNodeRootNode = item;

                }
                else if (item.Name == "ImageReference")
                {
                    ImageReference reference = new ImageReference();
                    foreach (var attr in GetAttributes(item))
                    {
                        if (attr.Key == "Namespace")
                        {
                            reference.Namespace = attr.Value;
                            references.Add(reference);
                            break;
                        }
                    }
                    //var NS= item.Attributes["Namespace"];
                    //reference.Namespace= NS.Value;
                }
            }
            references.Add(new ImageReference() { Namespace = "ScalableRelativeImage.Nodes" });
            foreach (var item in references)
            {
                Trace.WriteLine("R:" + item.Namespace);
            }
            if (ImageNodeRootNode is not null)
            {
                ImageRoot = new();
                foreach (var attribute in GetAttributes(ImageNodeRootNode))
                {
                    ImageRoot.SetValue(attribute.Key, attribute.Value);
                }
                foreach (var item in GetNodes(ImageNodeRootNode))
                {
                    ResolveRecursively(ImageRoot, ImageRoot, item, ref references, ref ExecutionWarnings);
                }
            }
            else
            {
                throw new InvalidDataException();
            }
            executionWarnings = ExecutionWarnings;
            return ImageRoot;
        }
        internal static void ResolveRecursively(ImageNodeRoot root, INode Parent, XmlNode node, ref List<ImageReference> references, ref List<ExecutionWarning> executionWarnings)
        {

            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var reference in references)
                {
                    var t =
                    asm.GetType(reference.Namespace + "." + node.Name);
                    if (t is null) break;
                    else
                    {
                        var inode = (INode)Activator.CreateInstance(t);
                        if (inode is GraphicNode)
                        {
                            (inode as GraphicNode).root = root;
                        }
                        var NodeAttr = GetAttributes(node);
                        foreach (var NodeAttrEntry in NodeAttr)
                        {
                            inode.SetValue(NodeAttrEntry.Key, NodeAttrEntry.Value);
                        }
                        var NodeSubNode = GetNodes(node);
                        foreach (var subnode in NodeSubNode)
                        {
                            ResolveRecursively(root, inode, subnode, ref references, ref executionWarnings);
                        }
                        Parent.AddNode(inode);
                    }
                }
            }
        }
        internal static List<XmlNode> GetNodes(XmlNode node)
        {
            List<XmlNode> l = new();

            string Prefix = node.Name + ".";
            for (int i = 0; i < node.ChildNodes.Count; i++)
            {
                var item = node.ChildNodes[i];
                if (!item.Name.StartsWith(Prefix))
                {
                    l.Add(item);
                }
            }
            return l;
        }
        internal static Dictionary<string, string> GetAttributes(XmlNode node)
        {
            Dictionary<string, string> dict = new();
            for (int i = 0; i < node.Attributes.Count; i++)
            {
                var item = node.Attributes[i];
                dict.Add(item.Name, item.Value);
            }
            string Prefix = node.Name + ".";
            for (int i = 0; i < node.ChildNodes.Count; i++)
            {
                var item = node.ChildNodes[i];
                if (item.Name.StartsWith(Prefix))
                {
                    dict.Add(item.Name.Substring(Prefix.Length), item.InnerText.Trim());
                }
            }
            return dict;
        }
    }
}
