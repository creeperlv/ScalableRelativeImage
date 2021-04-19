using ScalableRelativeImage.Nodes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ScalableRelativeImage
{
    public static class SRICompositor
    {
        public static string ToXMLString(ImageNodeRoot nodeRoot)
        {
            string _R = "";
            List<ImageReference> references = new List<ImageReference>();

            XmlDocument xmlDocument = new XmlDocument();
            var rootN = xmlDocument.CreateNode(XmlNodeType.Element, "ScalableRelativeImage", null);
            {
                var Flavor = CreateAttribute(ref xmlDocument, "Flavor", SRIEngine.Flavor);
                var FormatVersion = CreateAttribute(ref xmlDocument, "FormatVersion", SRIEngine.FormatVersion.ToString());
                rootN.Attributes.Append(Flavor);
                rootN.Attributes.Append(FormatVersion);
            }
            {
                DepthSerialize(nodeRoot, ref xmlDocument, ref rootN, ref references);
            }
            foreach (var item in references)
            {
                var imgref = xmlDocument.CreateNode(XmlNodeType.Element, "ImageReference", null);
                imgref.Attributes.Append(CreateAttribute(ref xmlDocument, "Namespace", item.Namespace));
                if (item.DLLFile is not null)
                {
                    imgref.Attributes.Append(CreateAttribute(ref xmlDocument, "DLLFile", item.DLLFile));
                }
                rootN.PrependChild(imgref);
            }
            xmlDocument.AppendChild(rootN);
            StringWriter SWriter = new StringWriter();
            xmlDocument.Save(SWriter);
            _R = SWriter.ToString();
            return _R;
        }
        public static void SerializeToStream(ImageNodeRoot nodeRoot, Stream TargetStream)
        {

            List<ImageReference> references = new List<ImageReference>();

            XmlDocument xmlDocument = new XmlDocument();
            var rootN = xmlDocument.CreateNode(XmlNodeType.Element, "ScalableRelativeImage", null);
            {
                var Flavor = CreateAttribute(ref xmlDocument, "Flavor", SRIEngine.Flavor);
                var FormatVersion = CreateAttribute(ref xmlDocument, "FormatVersion", SRIEngine.FormatVersion.ToString());
                rootN.Attributes.Append(Flavor);
                rootN.Attributes.Append(FormatVersion);
            }
            {
                DepthSerialize(nodeRoot, ref xmlDocument, ref rootN, ref references);
            }
            foreach (var item in references)
            {
                var imgref = xmlDocument.CreateNode(XmlNodeType.Element, "ImageReference", null);
                imgref.Attributes.Append(CreateAttribute(ref xmlDocument, "Namespace", item.Namespace));
                if (item.DLLFile is not null)
                {
                    imgref.Attributes.Append(CreateAttribute(ref xmlDocument, "DLLFile", item.DLLFile));
                }
                rootN.PrependChild(imgref);
            }
            xmlDocument.AppendChild(rootN);
            xmlDocument.Save(TargetStream);
        }
        static XmlAttribute CreateAttribute(ref XmlDocument xmlDocument, string Name, string Value)
        {
            var attr = xmlDocument.CreateAttribute(Name);
            attr.Value = Value;
            return attr;
        }
        static void DepthSerialize(INode node, ref XmlDocument xmlDocument, ref XmlNode Parent, ref List<ImageReference> references)
        {
            Type t = node.GetType();
            bool RefExist = false;
            if (t.Namespace is not "ScalableRelativeImage.Nodes")
            {

                foreach (var item in references)
                {
                    if (item.Namespace == t.Namespace)
                    {
                        RefExist = true;
                        break;
                    }
                }
                if (RefExist is not true)
                {
                    references.Add(new ImageReference() { Namespace = t.Namespace });
                }
            }
            var imgNodeRoot = xmlDocument.CreateNode(XmlNodeType.Element, t.Name, null);
            foreach (var item in node.GetValueSet())
            {
                imgNodeRoot.Attributes.Append(CreateAttribute(ref xmlDocument, item.Key, item.Value));
            }
            var ChildNodes = node.ListNodes();
            if (ChildNodes is not null)
                foreach (var item in ChildNodes)
                {
                    DepthSerialize(item, ref xmlDocument, ref imgNodeRoot, ref references);
                }
            Parent.AppendChild(imgNodeRoot);
        }
    }
}
