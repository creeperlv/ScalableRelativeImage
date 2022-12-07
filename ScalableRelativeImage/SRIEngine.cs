using ScalableRelativeImage.Nodes;
using SRI.Core.Backend;
using SRI.Core.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScalableRelativeImage
{
    /// <summary>
    /// SRI Engine, contains all method you need to serialize/deserialize a SRI Image.
    /// </summary>
    public static class SRIEngine
    {
        static SRIEngine()
        {
            BaseBackendFactory.Instance = new BackendFactory();
        }
        public static readonly string Flavor = "CreeperLv.SRI";
        public static readonly Version FormatVersion = new Version(1, 0, 0, 0);
        /// <summary>
        /// Set which backend to use.
        /// </summary>
        /// <param name="backend"></param>
        public static void SetBackend(BackendDefinition backend)
        {
            BaseBackendFactory.Instance = new BackendFactory();
            BackendFactory.UsingBackend = backend;
        }
        /// <summary>
        /// Deserialize the given string to an ImageNodeRoot.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static ImageNodeRoot Deserialize(string str)
        {
            return SRIAnalyzer.Parse(str, out _);
        }
        /// <summary>
        /// Deserialize the given string to an ImageNodeRoot, with collecting warnings.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="warnings"></param>
        /// <returns></returns>
        public static ImageNodeRoot Deserialize(string str, out List<ExecutionWarning> warnings)
        {
            return SRIAnalyzer.Parse(str, out warnings);
        }
        /// <summary>
        /// Deserialize the given file to an ImageNodeRoot.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static ImageNodeRoot Deserialize(FileInfo file)
        {
            return Deserialize(File.ReadAllText(file.FullName));
        }
        /// <summary>
        /// Deserialize the given file to an ImageNodeRoot, with collecting warnings.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="warnings"></param>
        /// <returns></returns>
        public static ImageNodeRoot Deserialize(FileInfo file, out List<ExecutionWarning> warnings)
        {
            return Deserialize(File.ReadAllText(file.FullName), out warnings);
        }
        /// <summary>
        /// Deserialize contents from given stream to an ImageNodeRoot.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static ImageNodeRoot Deserialize(Stream stream)
        {
            StreamReader streamReader = new StreamReader(stream);
            return Deserialize(streamReader.ReadToEnd());
        }
        /// <summary>
        /// Deserialize contents from given stream to an ImageNodeRoot, with collecting warnings.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="warnings"></param>
        /// <returns></returns>
        public static ImageNodeRoot Deserialize(Stream stream, out List<ExecutionWarning> warnings)
        {
            StreamReader streamReader = new StreamReader(stream);
            return Deserialize(streamReader.ReadToEnd(), out warnings);
        }
        /// <summary>
        /// Serialize an ImageNodeRoot to a XML string.
        /// </summary>
        /// <param name="imageRoot"></param>
        /// <returns></returns>
        public static string SerializeToString(ImageNodeRoot imageRoot)
        {
            return SRICompositor.ToXMLString(imageRoot);
        }
        /// <summary>
        /// Serialize an ImageNodeRoot to a XML file.
        /// </summary>
        /// <param name="imageRoot"></param>
        /// <param name="file"></param>
        public static void SerializeToFile(ImageNodeRoot imageRoot, FileInfo file)
        {
            string content = SRICompositor.ToXMLString(imageRoot);
            if (file.Exists)
                File.Delete(file.FullName);
            File.Create(file.FullName).Close();
            File.WriteAllText(file.FullName, content);
        }
        /// <summary>
        /// Serialize an ImageNodeRoot to a XML structure to given stream.
        /// </summary>
        /// <param name="root"></param>
        /// <param name="stream"></param>
        public static void SerializeToStream(ImageNodeRoot root, Stream stream)
        {
            SRICompositor.SerializeToStream(root, stream);
        }
    }
}
