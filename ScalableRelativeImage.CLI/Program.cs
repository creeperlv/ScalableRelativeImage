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
            string SourceFile = "";
            string OutFile = "a.png";
            float Width = -1;
            float Height = -1;
            Color Foreground = Color.White;
            Color Background= Color.Transparent;
            ColorConverter cc = new ColorConverter();
            for (int i = 0; i < args.Length; i++)
            {
                var item = args[i];
                switch (item.ToUpper())
                {
                    case "-H":
                    case "-HELP":
                    case "-?":
                    case "--?":
                        PrintHelp();
                        return;
                    case "--V":
                    case "-VERSION":
                        PrintVersion();
                        return;
                    case "--S":
                    case "-SOURCE":
                        i++;
                        SourceFile = args[i];
                        break;
                    case "--O":
                    case "-OUTPUT":
                        i++;
                        OutFile = args[i];
                        break;
                    case "--W":
                    case "-WIDTH":
                        i++;
                        Width = float.Parse(args[i]);
                        break;
                    case "--H":
                    case "-HEIGHT":
                        i++;
                        Height = float.Parse(args[i]);
                        break;
                    case "--F":
                    case "-FOREGROUND":
                        i++;
                        Foreground = (Color)cc.ConvertFromString(args[i]);
                        break;
                    case "--B":
                    case "-BACKGROUND":
                        i++;
                        Background = (Color)cc.ConvertFromString(args[i]);
                        break;
                    case "--E":
                    case "-EXTENSION":
                        i++;
                        Assembly.LoadFile(args[i]);
                        break;
                    default:
                        break;
                }
            }
            List<ExecutionWarning> warnings;
            if (SourceFile == "")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("FATAL ERROR:");
                Console.ResetColor();
                Console.WriteLine("Please specify a source file!");
                return;
            }
            Console.WriteLine("Resolving...");
            var source = new FileInfo(SourceFile);
            var Img = SRIEngine.Deserialize(source, out warnings);
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
            if (Width == -1)
            {
                Width = Img.RelativeWidth;
            }
            if (Height == -1)
            {
                Height = Img.RelativeHeight;
            }
            RenderProfile renderProfile = new RenderProfile();
            renderProfile.TargetWidth = Width;
            renderProfile.TargetHeight = Height;
            renderProfile.DefaultForeground = Foreground;
            renderProfile.DefaultBackground = Background;
            renderProfile.WorkingDirectory = source.Directory.FullName;
            Console.WriteLine("Rendering...");
            var bitmap = Img.Render(renderProfile);
            if (File.Exists(OutFile)) File.Delete(OutFile);
            bitmap.Save(OutFile);
            Console.WriteLine("Completed.");
        }
    }
}
