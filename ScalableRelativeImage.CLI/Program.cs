using CLUNL.ConsoleAppHelper;
using ScalableRelativeImage.Core;
using System;
using System.Reflection;

namespace ScalableRelativeImage.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleAppHelper.Init("SRI", "Scalable Relative Image CLI Tool");
            ConsoleAppHelper.Colorful = true;
            ConsoleAppHelper.PreExecution = () => {
                Output.OutLine("Copyright (C) 2021 Creeper Lv");
                Output.OutLine("All rights reserved.");
                Output.OutLine("");
                Output.OutLine("Scalable Relative Image CLI Tool");
                Output.OutLine("");
                Output.OutLine("This tool is licensed under The MIT License.");
                Output.OutLine("");
                Console.ForegroundColor = ConsoleColor.Red;
                Output.OutLine(new WarnMsg
                {
                    Fallback = "This software is still in active development, every behaviors may change without notification. Please do NOT use it in production environment.",
                    ID = "PREVIEW.NOTIFY"
                });
                Console.ResetColor();
                Output.OutLine("");
            };
            ConsoleAppHelper.Execute(args);
        }
    }
    [DependentVersion("SRI")]
    public class VerInfo : IFeatureCollectionVersion
    {
        public string GetVersionString()
        {
            return $"{SRIEngine.FormatVersion}-{SRIEngine.Flavor}";
        }
    }
}
