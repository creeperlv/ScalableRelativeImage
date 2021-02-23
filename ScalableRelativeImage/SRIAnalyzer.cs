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
        public Color DefaultForeground;
        public Color DefaultBackground;
        internal ImageNodeRoot root;
        public Point FindTargetPoint(float RX,float RY)
        {
            Point p = new Point((int)(RX / root._RelativeWidth * TargetWidth),(int)( RY / root._RelativeHeight * TargetHeight));
            return p;
        }
    }
    public class SRIAnalyzer
    {
        public static ImageNodeRoot Parse(string Content)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(Content);
            var l = xmlDocument.ChildNodes[0].ChildNodes;
            XmlNode ImageNodeRootNode = null;
            ImageNodeRoot ImageRoot = null;
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
                    ResolveRecursively(ImageRoot, item, ref references);
                }
            }
            else
            {
                throw new InvalidDataException();
            }
            return ImageRoot;
        }
        internal static void ResolveRecursively(INode Parent, XmlNode node,ref List<ImageReference> references)
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
                        var NodeAttr = GetAttributes(node);
                        foreach (var NodeAttrEntry in NodeAttr)
                        {
                            inode.SetValue(NodeAttrEntry.Key, NodeAttrEntry.Value);
                        }
                        var NodeSubNode = GetNodes(node);
                        foreach (var subnode in NodeSubNode)
                        {
                            ResolveRecursively(inode, subnode, ref references);
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
