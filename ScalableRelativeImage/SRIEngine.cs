using ScalableRelativeImage.Nodes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScalableRelativeImage
{
    public class SRIEngine
    {
        public static readonly string Flavor = "CreeperLv.SRI";
        public static readonly Version FormatVersion = new Version(1, 0, 0, 0);
        public static ImageNodeRoot Deserialize(string str)
        {
            return SRIAnalyzer.Parse(str, out _);
        }
        public static ImageNodeRoot Deserialize(string str, out List<ExecutionWarning> warnings)
        {
            return SRIAnalyzer.Parse(str, out warnings);
        }
        public static ImageNodeRoot Deserialize(FileInfo file)
        {
            return Deserialize(File.ReadAllText(file.FullName));
        }
        public static ImageNodeRoot Deserialize(FileInfo file, out List<ExecutionWarning> warnings)
        {
            return Deserialize(File.ReadAllText(file.FullName), out warnings);
        }
        public static ImageNodeRoot Deserialize(Stream stream)
        {
            StreamReader streamReader = new StreamReader(stream);
            return Deserialize(streamReader.ReadToEnd());
        }
        public static ImageNodeRoot Deserialize(Stream stream, out List<ExecutionWarning> warnings)
        {
            StreamReader streamReader = new StreamReader(stream);
            return Deserialize(streamReader.ReadToEnd(),out warnings);
        }
        public static string SerializeToString(ImageNodeRoot imageRoot)
        {
            return SRICompositor.ToXMLString(imageRoot);
        }
        public static void SerializeToFile(ImageNodeRoot imageRoot,FileInfo file)
        {
            string content = SRICompositor.ToXMLString(imageRoot);
            File.Delete(file.FullName);
            File.Create(file.FullName).Close();
            File.WriteAllText(file.FullName, content);
        }
    }
}
