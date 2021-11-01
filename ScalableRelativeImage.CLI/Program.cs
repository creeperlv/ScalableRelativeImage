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
        static void PrintHelp()
        {

        }
        static void PrintVersion()
        {
            Console.WriteLine($"Flavor: {SRIEngine.Flavor}");
            Console.WriteLine($"Format Version: {SRIEngine.FormatVersion}");
            Console.WriteLine($"");
            Console.WriteLine($"Available Shapes:");
            Console.WriteLine($"");
            //var asm = Assembly.GetAssembly(typeof(SRIEngine));
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                var TotalTypes = asm.GetTypes();
                foreach (var item in TotalTypes)
                {
                    if (item.IsAssignableTo(typeof(INode)))
                    {
                        if (item.Name != "ImageNodeRoot")
                            if (item.Name != "INode")
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Write($"\t{item.Name}");
                                Console.ResetColor();
                                Console.WriteLine($" in {item.Namespace}");

                            }

                    }
                }
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Copyright (C) 2021 Creeper Lv");
            Console.WriteLine("All rights reserved.");
            Console.WriteLine("");
            Console.WriteLine("Scalable Relative Image CLI Tool");
            Console.WriteLine("");
            Console.WriteLine("This tool is licensed under The MIT License.");
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("This software is still in active development, every behaviors may change without notification. Please do NOT use it in production environment.");
            Console.ResetColor();
            Console.WriteLine("");
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
            var Output = (string)Parameters.Query("O");
            var _Width = Parameters.Query("width");
            var _Height = Parameters.Query("height");
            var B = Parameters.Query<string>("B");
            var F = (string)Parameters.Query("F");
            ColorConverter cc = new ColorConverter();
            Color Foreground = Color.White;
            Color Background = Color.Transparent;
            if (B != null)
            {
                Background = (Color)cc.ConvertFromString((string)B);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Missing Backaground, using Transparent.");
                Console.ResetColor();
            }
            if (F != null)
            {
                Foreground = (Color)cc.ConvertFromString((string)F);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Missing Foreground, using White.");
                Console.ResetColor();
            }
            float Height;
            float Width;
            if (SourceFile == null || SourceFile == "")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("FATAL ERROR:");
                Console.ResetColor();
                Console.WriteLine("Please specify a source file!");
                return;
            }
            var source = new FileInfo(SourceFile);
            ImageNodeRoot Img;
            {
                List<ExecutionWarning> warnings;

                if (!source.Exists)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("FATAL ERROR:");
                    Console.ResetColor();
                    Console.WriteLine("Source file does not exist!");
                    return;
                }

                Console.WriteLine("Resolving...");
                Img = SRIEngine.Deserialize(source, out warnings);
                if (warnings.Count == 0)
                    Console.WriteLine("Completed.");
                else
                {
                    Console.WriteLine("Completed with warnings:");
                    foreach (var item in warnings)
                    {
                        Console.WriteLine($"{item.ID}:{item.Message}");
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
            Console.WriteLine("Rendering...");
            var bitmap = Img.Render(renderProfile);

            if (Output == null)
            {
                Output = source.DirectoryName;
                if (source.Name.IndexOf(".") > 0)
                {
                    Output = System.IO.Path.Combine(Output, source.Name.Substring(0, source.Name.LastIndexOf(".")) + ".png");

                }
                else
                {
                    Output = System.IO.Path.Combine(Output, source.Name + ".png");
                }

            }

            if (File.Exists(Output)) File.Delete(Output);
            bitmap.Save(Output);
            Console.WriteLine("Completed.");
        }
    }
}
