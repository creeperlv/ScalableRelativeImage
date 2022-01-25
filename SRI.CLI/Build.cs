using CLUNL.ConsoleAppHelper;
using SRI.Editor.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRI.CLI
{
    [DependentFeature("SRI", "Build", Description = "Build a project", Options = new string[] { "c" }, OptionDescriptions = new string[] { "Specify configuration." })]
    public class Build : IFeature
    {
        public void Execute(ParameterList Parameters, string MainParameter)
        {
            var c = Parameters.Query("c");
            if (MainParameter == "")
            {
                Output.OutLine(new ErrorMsg { ID = "PROJ_NOT_SPEC", Fallback = "Project File is not Specified." });
                return;
            }
            var PE = ProjectEngine.Load(new System.IO.FileInfo(MainParameter));
            string __configuration = c as string;
            if (c == null)
            {
                Output.OutLine(new WarnMsg { ID = "CONFIG_NOT_SPEC", Fallback = "Target Configuration is not specified, using first configuration." });
                __configuration = PE.CoreProject.BuildConfigurations.First().Name;
            }

            ProjectEngine.BuildSync(PE, __configuration, (i, t) =>
            {
                Output.OutLine($"[{i}]Building:{t.Name}");
            }, (i, t) =>
            {
                Output.OutLine($"[{i}]Done:{t.Name}");
            }, () =>
            {
                Output.OutLine("Done");
            }, (e) =>
            {
                Output.OutLine(new ErrorMsg { ID = "Failed.RDM", Fallback = $"Failed:{e}" });
            }, (i, w) =>
            {
                foreach (var item in w)
                {
                    Output.OutLine(new WarnMsg { ID = "Exec.Warn." + item.ID, Fallback = $"Warn:{item.Message}" });

                }
            });
        }
    }
}
