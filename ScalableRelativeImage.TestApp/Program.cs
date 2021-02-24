using ScalableRelativeImage.Nodes;
using System;
using System.Drawing;

namespace ScalableRelativeImage.TestApp
{
    class Program
    {
        static string ExampleXmlDocument = @"
<ScalableRelativeImage>
    <!-- Example Code-->
    <ImageNodeRoot RelativeWidth=""160"" RelativeHeight=""90"">
        <Line StartX=""1"" StartY=""0.2"" EndX=""159"" EndY=""0.2"" Size=""0.01"" Color=""#FFFFFF"">
        </Line>
        <Rectangle X=""10"" Y=""10"" Width=""100"" Height=""50"" Size=""0.05"" Fill=""True"" Color=""#22884E"" />
        <Rectangle X=""10"" Y=""10"" Width=""100"" Height=""50"" Size=""0.05"" Color=""#2288EE"" />
        <Ellipse X=""10"" Y=""10"" Width=""100"" Height=""50"" Size=""0.05"" Fill=""True"" Color=""#12188E"" />
        <Ellipse X=""10"" Y=""10"" Width=""100"" Height=""50"" Size=""0.05"" Color=""#2288EE"" />
        <Curve Size=""0.01"" Color=""#FF2288EE"">
            <Point X=""10"" Y=""10""/>
            <Point X=""150"" Y=""10""/>
            <Point X=""150"" Y=""80""/>
            <Point X=""10"" Y=""80""/>
        </Curve>
        <Text Content=""Hello, SRI"" FontFamily=""Consolas"" FontStyle=""Bold"" Size=""0.5"" X=""10"" Y=""10"" Width=""100"" Height=""80"" Foreground=""#2288EE"" />
    </ImageNodeRoot>
</ScalableRelativeImage>
";
        static void Main(string[] args)
        {
            {
                ImageNodeRoot image = new ImageNodeRoot();
                image.RelativeWidth = 192;
                image.RelativeHeight = 108;
                {
                    Text text = new Text();
                    text.X = 1;
                    text.Y = 1;
                    text.Width = 15;
                    text.Height = 5;
                    text.Content = "Hello, world!";
                    text.FontFamily = "Arial";
                    text.RelativeFontSize=0.2f;
                    image.AddNode(text);
                }
                var XMLOutcome=SRIEngine.SerializeToString(image);
                Console.WriteLine(XMLOutcome);
                var bitmap= image.Render(new RenderProfile() { TargetWidth = 1600, TargetHeight = 900 });
                bitmap.Save("HelloWorld.png");
            }
            {
                var image = SRIAnalyzer.Parse(ExampleXmlDocument, out _);
                Console.WriteLine($"W={image.RelativeWidth},H={image.RelativeHeight}");

                var bitmap = image.Render(new RenderProfile() { TargetWidth = 1600, TargetHeight = 900 });
                bitmap.Save("Test.png");
                Console.WriteLine(SRICompositor.ToXMLString(image));
                Console.WriteLine("Done.");
            }
        }
    }
}
