using CLUNL.ConsoleAppHelper;
using SRI.Editor.Core;
using SRI.Editor.Main.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRI.CLI
{
    [DependentFeature("SRI", "Build", Description = "Build a project", Options = new string[] { "c", "b" }, OptionDescriptions = new string[] { "Specify configuration.", "Specify backend." })]
    public class Build : IFeature
    {
        public void Execute(ParameterList Parameters, string MainParameter)
        {
            var c = Parameters.Query("c");
            var b = Parameters.Query("b");
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
            if (b != null)
            {
                if (int.TryParse((string)b, out var __b))
                {
                    EditorConfiguration.CurrentConfiguration.Backend = __b;
                    Output.OutLine($"Backend:{__b}");
                }
                else
                {
                    Output.OutLine($"Backend Cannot Be Accepted.");
                }
            }
            if (__configuration.ToUpper() == "(ALL)")
            {
                int CI = 0;
                foreach (var item in PE.CoreProject.BuildConfigurations)
                {
                    Output.OutLine($"<{CI}>Configuration:{item.Name}");
                    ProjectEngine.BuildSync(PE, item.Name, (i, t) =>
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
                    CI++;
                }
            }
            else
            {
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
}
