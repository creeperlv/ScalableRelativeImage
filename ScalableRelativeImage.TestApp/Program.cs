using System;
using System.Drawing;

namespace ScalableRelativeImage.TestApp
{
    class Program
    {
        static string ExampleXmlDocument = @"
<ScalableRelativeImage>
    <ImageNodeRoot RelativeWidth=""160"" RelativeHeight=""90"">
        <Line StartX=""1"" StartY=""0.2"" EndX=""159"" EndY=""0.2"" Size=""0.01"" Color=""#FFFFFF"">
        </Line>
        <OutlineCloseCurve Sizr=""0.01"" Color=""#FF2288EE"">
            <Point X=""10"" Y=""10""/>
            <Point X=""150"" Y=""10""/>
            <Point X=""150"" Y=""80""/>
            <Point X=""10"" Y=""80""/>
        </OutlineCloseCurve>
    </ImageNodeRoot>
</ScalableRelativeImage>
";
        static void Main(string[] args)
        {
            Color c = Color.FromArgb(10, 122, 32, 244);
            Console.WriteLine((new ColorConverter()).ConvertToString(c));
            Console.WriteLine(c.ToArgb().ToString("X"));
            Console.WriteLine((new ColorConverter()).ConvertFromString("#A7A20F4"));
            var image=SRIAnalyzer.Parse(ExampleXmlDocument);
            Console.WriteLine($"W={image.RelativeWidth},H={image.RelativeHeight}");
            var bitmap=image.Render(new RenderProfile() { TargetWidth = 1600, TargetHeight = 900 });
            bitmap.Save("Test.png");
        }
    }
}
