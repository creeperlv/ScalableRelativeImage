﻿using System;
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
        <Rectangle X=""10"" Y=""10"" Width=""100"" Height=""50"" Size=""0.05"" Fill=""True"" Color=""#22884E"" />
        <Rectangle X=""10"" Y=""10"" Width=""100"" Height=""50"" Size=""0.05"" Color=""#2288EE"" />
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

            var image = SRIAnalyzer.Parse(ExampleXmlDocument, out _);
            Console.WriteLine($"W={image.RelativeWidth},H={image.RelativeHeight}");

            var bitmap = image.Render(new RenderProfile() { TargetWidth = 1600, TargetHeight = 900 });
            bitmap.Save("Test.png");
            Console.WriteLine(SRICompositor.ToXMLString(image));
            Console.WriteLine("Done.");
        }
    }
}
