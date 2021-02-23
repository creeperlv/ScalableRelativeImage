using System;

namespace ScalableRelativeImage.TestApp
{
    class Program
    {
        static string ExampleXmlDocument = @"
<ScalableRelativeImage>
    <ImageNodeRoot RelativeWidth=""16"" RelativeHeight=""9"">
        <Line StartX=""Fuck"">
        </Line>
    </ImageNodeRoot>
</ScalableRelativeImage>
";
        static void Main(string[] args)
        {
            var image=SRIAnalyzer.Parse(ExampleXmlDocument);
            Console.WriteLine($"W={image.RelativeWidth},H={image.RelativeWidth}");
        }
    }
}
