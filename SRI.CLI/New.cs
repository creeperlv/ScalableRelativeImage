using CLUNL.ConsoleAppHelper;
using SRI.Editor.Core;
using System.IO;

namespace SRI.CLI
{
    [DependentFeature("SRI", "New", Description = "Create a new project", Options = new string[] { }, OptionDescriptions = new string[] { })]
    public class New : IFeature
    {
        public void Execute(ParameterList Parameters, string MainParameter)
        {

            if (MainParameter == "")
            {
                Output.OutLine(new ErrorMsg { ID = "PROJ_NOT_SPEC", Fallback = "Project File is not Specified." });
                return;
            }
            if (!File.Exists(MainParameter))
            {
                File.WriteAllText(MainParameter, ProjectEngine.NewEmptyProject());
                Output.OutLine("Done.");
            }
            else
            {
                Output.OutLine(new ErrorMsg { ID = "FILE_EXISTS", Fallback = "Target fils is already existed!" });
                return;
            }
        }
    }
}
