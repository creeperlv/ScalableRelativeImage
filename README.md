# ScalableRelativeImage

An implementation of vector image, using a markup language which is based on XML and the syntax is like XAML.

**This library/software is still in active development, all things may change without proper notification.**

## Library

The main library is `ScalableRelativeImage`. To use it, just refer it to your project.

### Resolve the vector image and render it.

The use of it is quite simple, use `SRIEngine.Deserialize(string Content,out List<ExecutionWarning> warnings)` and you can obtain an abstract image.
o get the final picture, you need to use `Render(RendProfile profile)`.

Here is an example:

```CSharp
    var image = SRIAnalyzer.Parse("<!--Contents Here...-->", out _);
    var bitmap = image.Render(new RenderProfile() { TargetWidth = 1920, TargetHeight = 1080 });
```

### Create a vector image and get the XML text

To do it, you need to create an `ImageNodeRoot` and use `SRIEngine.SerializeToString(ImageNodeRoot imageRoot)` to obtain the XML text.

Here is an example:

```CSharp
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
```
The result should be
```XML
<?xml version="1.0" encoding="utf-16"?>
<ScalableRelativeImage Flavor="CreeperLv.SRI" FormatVersion="1.0.0.0">
  <ImageNodeRoot RelativeWidth="192" RelativeHeight="108">
    <Text Content="Hello, world!" FontFamily="Arial" FontStyle="Regular" Size="0.2" X="1" Y="1" Width="15" Height="5" />
  </ImageNodeRoot>
</ScalableRelativeImage>
```

## CLI Tool

Currently, the CLI tool I made works like a compiler. It directly reads a SRI file and render then write it to a pixel format file.

Example:

`ScalableRelativeImage.CLI.exe --S Example.SRI --W 1920 --H 1080 --F White --B Black --O Example.png`

## Extend Shapes

This is quite sample. If you wish to add a shape that is not in SRI library, just create a libaray targeting `net5.0` then create a class which implements `INode`.

To use your own shape, we assume your shapes are in a library named `foo.dll`, when using the CLI tool, added `--E foo.dll` in your command line arguments.

Example Command Line:

`ScalableRelativeImage.CLI.exe --S Example.SRI --W 1920 --H 1080 --F White --B Black --O Example.png --E foo.dll`
