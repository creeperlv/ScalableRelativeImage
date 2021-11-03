using CLUNL.ConsoleAppHelper;
using ScalableRelativeImage.Nodes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace ScalableRelativeImage.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            Output.OutLine("Copyright (C) 2021 Creeper Lv");
            Output.OutLine("All rights reserved.");
            Output.OutLine("");
            Output.OutLine("Scalable Relative Image CLI Tool");
            Output.OutLine("");
            Output.OutLine("This tool is licensed under The MIT License.");
            Output.OutLine("");
            Console.ForegroundColor = ConsoleColor.Red;
            Output.OutLine(new ErrorMsg
            {
                Fallback = "This software is still in active development, every behaviors may change without notification. Please do NOT use it in production environment.",
                ID = "PREVIEW.NOTIFY"
            });
            Console.ResetColor();
            
            Output.OutLine("");
            ConsoleAppHelper.Init("SRI", "Scalable Relative Image CLI Tool");
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
    [DependentFeature("SRI", "render", Description = "Render the SRI source file to an image.",
        Options = new string[] { "O,Output", "Width", "Height", "B,Background", "F,Foreground" },
        OptionDescriptions = new string[] { "Output location", "Width of final rendered image.", "Height of final rendered image.", "Background color of the image.", "Foreground color of the image" })]
    public class Render : IFeature
    {
        public void Execute(ParameterList Parameters, string SourceFile)
        {
            var _Output = (string)Parameters.Query("O");
            var _Width = Parameters.Query("width");
            var _Height = Parameters.Query("height");
            var B = Parameters.Query<string>("B");
            var F = (string)Parameters.Query("F");
            ColorConverter cc = new ColorConverter();
            Color Foreground = Color.White;
            Color Background = Color.Transparent;
            if (SourceFile == null || SourceFile == "")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Output.Out("FATAL ERROR:");
                Console.ResetColor();
                Output.OutLine(new ErrorMsg { Fallback = "Please specify a source file!", ID = "NoInpupt" });
                return;
            }
            var source = new FileInfo(SourceFile);
            if (!source.Exists)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Output.Out("FATAL ERROR:");
                Console.ResetColor();
                Output.OutLine(new ErrorMsg { Fallback = "Source file does not exist!", ID = "InputNotFound" });
                return;
            }
            //Checks...
            if (B != null)
            {
                Background = (Color)cc.ConvertFromString((string)B);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Output.OutLine(new WarnMsg { Fallback = $"Missing Backaground, using Transparent.", ID = "NoBG" });
                Console.ResetColor();
            }
            if (F != null)
            {
                Foreground = (Color)cc.ConvertFromString((string)F);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Output.OutLine(new WarnMsg { Fallback = $"Missing Foreground, using White.", ID = "NoFG" });
                Console.ResetColor();
            }
            float Height;
            float Width;
            ImageNodeRoot Img;
            {
                List<ExecutionWarning> warnings;


                Output.OutLine("Resolving...");
                Img = SRIEngine.Deserialize(source, out warnings);
                if (warnings.Count == 0)
                    Output.OutLine("Completed.");
                else
                {
                    Output.OutLine("Completed with warnings:");
                    foreach (var item in warnings)
                    {
                        Output.Out($"{item.ID}:");
                        Output.OutLine(new WarnMsg { Fallback = item.Message,ID= item.ID }) ;
                    }
                }
                if (_Width == null)
                {
                    Width = Img.RelativeWidth;
                }
                else
                {
                    if (_Width.GetType() == typeof(int))
                    {
                        Width = (int)_Width;
                    }
                    else if (_Width.GetType() == typeof(float))
                    {
                        Width = (float)_Width;
                    }
                    else
                    {

                        Width = Img.RelativeWidth;
                    }
                }
                if (_Height == null)
                {
                    Height = Img.RelativeHeight;
                }
                else
                {
                    if (_Height.GetType() == typeof(int))
                    {
                        Height = (int)_Width;
                    }
                    else if (_Height.GetType() == typeof(float))
                    {
                        Height = (float)_Width;
                    }
                    else
                    {

                        Height = Img.RelativeWidth;
                    }
                }
            }



            RenderProfile renderProfile = new RenderProfile();
            renderProfile.TargetWidth = Width;
            renderProfile.TargetHeight = Height;
            renderProfile.DefaultForeground = Foreground;
            renderProfile.DefaultBackground = Background;
            renderProfile.WorkingDirectory = source.Directory.FullName;
            Output.OutLine("Rendering...");
            var bitmap = Img.Render(renderProfile);

            if (_Output == null)
            {
                _Output = source.DirectoryName;
                if (source.Name.IndexOf(".") > 0)
                {
                    _Output = System.IO.Path.Combine(_Output, source.Name.Substring(0, source.Name.LastIndexOf(".")) + ".png");

                }
                else
                {
                    _Output = System.IO.Path.Combine(_Output, source.Name + ".png");
                }

            }

            if (File.Exists(_Output)) File.Delete(_Output);
            bitmap.Save(_Output);
            Output.OutLine("Completed.");
        }
    }
}
