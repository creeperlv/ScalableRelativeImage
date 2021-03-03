using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using AvaloniaEdit;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http.Headers;

namespace ScalableRelativeImage.AvaloniaGUI
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            if (RefreshButton is not null)
                RefreshButton.Click += RefreshButton_Click;
        }

        private void RefreshButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Trace.WriteLine("Try Refresh");
            if (CentralEditor is not null)
                if (ImagePreview is not null)
                    if (WidthBox is not null)
                        if (HeightBox is not null)
                            if (ScaleBox is not null)
                            {
                                try
                                {
                                    var vectorimg = SRIEngine.Deserialize(CentralEditor.Text, out _);
                                    RenderProfile profile = new RenderProfile();
                                    float Scale = 1.0f;
                                    try
                                    {
                                        Scale = float.Parse(ScaleBox.Text);
                                    }
                                    catch (System.Exception)
                                    {
                                    }
                                    try
                                    {
                                        profile.TargetWidth = float.Parse(WidthBox.Text);

                                    }
                                    catch (System.Exception)
                                    {
                                        profile.TargetWidth = vectorimg.RelativeWidth;
                                    }
                                    try
                                    {
                                        profile.TargetHeight = float.Parse(HeightBox.Text);
                                    }
                                    catch (System.Exception)
                                    {
                                        profile.TargetHeight= vectorimg.RelativeHeight;
                                    }
                                    profile.TargetWidth *= Scale;
                                    profile.TargetHeight *= Scale;
                                    var img = vectorimg.Render(profile);
                                    MemoryStream memoryStream = new MemoryStream();
                                    img.Save(memoryStream, ImageFormat.Png);
                                    memoryStream.Position = 0;
                                    Bitmap b = new Bitmap(memoryStream);
                                    ImagePreview.Source = b;
                                    return;
                                }
                                catch (System.Exception exception)
                                {
                                    Trace.WriteLine(exception);
                                }

                            }
            Trace.WriteLine("Something went wrong...");
        }

        Button? OpenButton;
        Button? SaveButton;
        Button? SaveAsButton;
        Button? NewButton;
        Button? RefreshButton;
        TextEditor? CentralEditor;
        Image? ImagePreview;
        TextBox? WidthBox;
        TextBox? HeightBox;
        TextBox? ScaleBox;
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            OpenButton = this.FindControl<Button>("OpenButton");
            SaveButton = this.FindControl<Button>("SaveButton");
            SaveAsButton = this.FindControl<Button>("SaveAsButton");
            NewButton = this.FindControl<Button>("NewButton");
            RefreshButton = this.FindControl<Button>("RefreshButton");
            CentralEditor = this.FindControl<TextEditor>("CentralEditor");
            ImagePreview = this.FindControl<Image>("ImagePreview");
            WidthBox = this.FindControl<TextBox>("ImageWidthBox");
            HeightBox = this.FindControl<TextBox>("ImageHeightBox");
            ScaleBox = this.FindControl<TextBox>("PreviewImageScaleBox");
        }
    }
}
