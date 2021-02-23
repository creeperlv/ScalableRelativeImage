using System;
namespace ScalableRelativeImage.TestApp
{
    class Program
    {
        static string ExampleXmlDocument = @"
<ScalableRelativeImage>
    <ImageNodeRoot RelativeWidth=""160"" RelativeHeight=""90"">
        <Line StartX=""1"" StartY=""0.2"" EndX=""159"" EndY=""0.2"" Size=""0.01"" Color=""#FFFFFF"">
        </Line>
    </ImageNodeRoot>
</ScalableRelativeImage>
";
        static void Main(string[] args)
        {
            var image=SRIAnalyzer.Parse(ExampleXmlDocument);
            Console.WriteLine($"W={image.RelativeWidth},H={image.RelativeHeight}");
            var bitmap=image.Render(new RenderProfile() { TargetWidth = 1600, TargetHeight = 900 });
            bitmap.Save("Test.png");
        }
    }
}
