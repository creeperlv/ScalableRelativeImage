using ScalableRelativeImage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRI.Editor.Core.Projects
{
    [Serializable]
    public class Project
    {
        public List<BuildConfiguration> BuildConfigurations = new List<BuildConfiguration>();
    }
    public class LoadedProject
    {
        public Project CoreProject;
        public DirectoryInfo WorkingDirectory;
        public FileInfo ProjectFile;
    }
    [Serializable]
    public class BuildConfiguration
    {
        public string Name;
        public string OutputDirectory;
        public List<Symbol> Symbols = new List<Symbol>();
        public List<BuildTarget> BuildTargets = new List<BuildTarget>();
    }
    [Serializable]
    public class BuildTarget
    {
        public string Name;
        public string File;
        public string OutputName;
        public string Background;
        public string Foreground;
        public int Width;
        public int Height;
        public List<Symbol> Symbols = new List<Symbol>();
    }
}
