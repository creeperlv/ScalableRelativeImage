using ScalableRelativeImage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRI.Editor.Core.Projects
{
    [Serializable]
    public class Project
    {
        public List<BuildTarget> BuildTargets = new List<BuildTarget>();
    }
    [Serializable]
    public class BuildTarget
    {
        public string Name;
        public string File;
        public List<Symbol> Symbols = new List<Symbol>();

    }
}
