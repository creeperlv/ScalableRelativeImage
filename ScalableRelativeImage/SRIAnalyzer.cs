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
        public string DLLFile = null;
    }
    public class RenderProfile
    {
        public float TargetWidth;
        public float TargetHeight;
        public Color? DefaultForeground = null;
        public Color? DefaultBackground = null;
        internal ImageNodeRoot root;
        public string WorkingDirectory = Environment.CurrentDirectory;
        public RenderProfile Copy(INode Root)
        {
            RenderProfile renderProfile = new RenderProfile();
            renderProfile.DefaultBackground = DefaultBackground;
            renderProfile.DefaultForeground= DefaultForeground;
            renderProfile.TargetHeight = TargetHeight;
            renderProfile.TargetWidth = TargetWidth;
            renderProfile.WorkingDirectory = Environment.CurrentDirectory;
            return renderProfile;
        }
        public FileInfo FindFile(string FileName)
        {
            var path0 = System.IO.Path.Combine(WorkingDirectory, FileName);
            if (File.Exists(path0)) return new FileInfo(path0); else if (File.Exists(FileName)) return new FileInfo(FileName); else return null;
        }
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
    public record ShapeDisposedWarning : ExecutionWarning
    {
        public ShapeDisposedWarning(INode node) : base("SRI005", $"Shape \"{node.GetType().Name}\" has been disposed.") { }
    }
    public record ShapeMismatchWarning : ExecutionWarning
    {
        public ShapeMismatchWarning(object receieved, Type Target) : base("SRI004", $"Shape {receieved.GetType().Name} does not match required shape \"{Target.Name}\"") { }
    }
    public record DataDisposedWarning : ExecutionWarning
    {
        public DataDisposedWarning(string Key, string Value) : base("SRI003", $"Data \"{Value}\"(\"{Key}\") has been disposed.") { }
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
        public static ImageNodeRoot Parse(string Content, out List<ExecutionWarning> executionWarnings, DirectoryInfo[] DllPaths = null)
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
                        }
                        else if (attr.Key == "DLLFile")
                        {
                            reference.DLLFile = attr.Value;
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
                if (item.DLLFile is not null or "")
                {
                    FileInfo file = new(item.DLLFile);
                    bool v = false;
                    foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
                    {

                        try
                        {
                            FileInfo fi = new(asm.FullName);
                            if (fi.Name == file.Name)
                            {
                                v = true;
                                break;
                            }
                            else
                            {

                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                    if (v == true)
                    {
                        if (file.Exists)
                        {
                            Assembly.LoadFrom(file.FullName);
                        }
                        else
                        {
                            if (DllPaths != null)
                            {
                                bool isMatch = false;
                                foreach (var libPath in DllPaths)
                                {

                                    foreach (var libFile in libPath.EnumerateFiles())
                                    {
                                        if (libFile.Name == file.Name)
                                        {
                                            isMatch = true;
                                            Assembly.LoadFrom(file.FullName);
                                            break;
                                        }
                                    }
                                    if (isMatch == true) break;
                                }
                                if (isMatch is false)
                                {
                                    ExecutionWarnings.Add(new ExecutionWarning("SRI002", $"Cannot find load target DLL file:{item.DLLFile}"));
                                }
                            }
                        }
                    }
                }
            }
            if (ImageNodeRootNode is not null)
            {
                ImageRoot = new();
                foreach (var attribute in GetAttributes(ImageNodeRootNode))
                {
                    ImageRoot.SetValue(attribute.Key, attribute.Value, ref ExecutionWarnings);
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
            bool v = false;
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
                            inode.SetValue(NodeAttrEntry.Key, NodeAttrEntry.Value, ref executionWarnings);
                        }
                        var NodeSubNode = GetNodes(node);
                        foreach (var subnode in NodeSubNode)
                        {
                            ResolveRecursively(root, inode, subnode, ref references, ref executionWarnings);
                        }
                        Parent.AddNode(inode, ref executionWarnings);
                        v = true;
                        break;
                    }
                }
                if (v is true) break;
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
