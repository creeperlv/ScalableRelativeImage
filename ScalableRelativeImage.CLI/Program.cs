using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

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
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Scalable Relative Image CLI Tool");
            Console.WriteLine("Author: Creeper Lv");
            Console.WriteLine("This tool is licensed under The MIT License.");   
            Console.WriteLine("");   
            string SourceFile = "";
            string OutFile = "a.png";
            float Width = -1;
            float Height = -1;
            Color Foreground = Color.White;
            ColorConverter cc = new ColorConverter();
            if (args.Length == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("FATAL ERROR:");
                Console.ResetColor();
                Console.WriteLine("Please specify a source file!");
                return;
            }
            for (int i = 0; i < args.Length; i++)
            {
                var item = args[i];
                switch (item.ToUpper())
                {
                    case "-H":
                    case "-?":
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
                    default:
                        break;
                }
            }
            List<ExecutionWarning> warnings;
            Console.WriteLine("Resolving...");
            var Img = SRIEngine.Deserialize(new FileInfo(SourceFile), out warnings);
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
            Console.WriteLine("Rendering...");
            var bitmap = Img.Render(renderProfile);
            if (File.Exists(OutFile)) File.Delete(OutFile);
            bitmap.Save(OutFile);
            Console.WriteLine("Completed.");
        }
    }
}
