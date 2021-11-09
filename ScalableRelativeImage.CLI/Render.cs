using CLUNL.ConsoleAppHelper;
using ScalableRelativeImage.Nodes;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace ScalableRelativeImage.CLI
{
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
                //Console.ForegroundColor = ConsoleColor.Red;
                //Output.Out("FATAL ERROR:");
                //Console.ResetColor();
                Output.OutLine(new ErrorMsg { Fallback = "Please specify a source file!", ID = "NoInpupt" });
                return;
            }
            var source = new FileInfo(SourceFile);
            if (!source.Exists)
            {
                //Console.ForegroundColor = ConsoleColor.Red;
                //Output.Out("FATAL ERROR:");
                //Console.ResetColor();
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
                //Console.ForegroundColor = ConsoleColor.Yellow;
                Output.OutLine(new WarnMsg { Fallback = $"Missing Backaground, using Transparent.", ID = "NoBG" });
                //Console.ResetColor();
            }
            if (F != null)
            {
                Foreground = (Color)cc.ConvertFromString((string)F);
            }
            else
            {
                //Console.ForegroundColor = ConsoleColor.Yellow;
                Output.OutLine(new WarnMsg { Fallback = $"Missing Foreground, using White.", ID = "NoFG" });
                //Console.ResetColor();
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
                        //Output.Out($"{item.ID}:");
                        Output.OutLine(new WarnMsg { Fallback = $"{item.ID }:{item.Message}",ID= item.ID }) ;
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
    [DependentFeature("SRI", "build", Description = "Render the SRI source file to an image. This function is preserved for those who perfer \"build\" instead of \"render\".",
    Options = new string[] { "O,Output", "Width", "Height", "B,Background", "F,Foreground" },
    OptionDescriptions = new string[] { "Output location", "Width of final rendered image.", "Height of final rendered image.", "Background color of the image.", "Foreground color of the image" })]

    public class Build: IFeature
    {

        public void Execute(ParameterList Parameters, string SourceFile)
        {
            Render r = new Render();
            r.Execute(Parameters, SourceFile);
        }
    }
}
