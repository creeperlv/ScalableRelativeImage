using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using AvaloniaEdit;
using ScalableRelativeImage.Nodes;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http.Headers;

namespace ScalableRelativeImage.AvaloniaGUI
{
    public class MainWindow : Window
    {
        public static readonly string Template = @"<ScalableRelativeImage Flavor=""CreeperLv.SRI"" FormatVersion=""1.0.0.0"">
  <ImageNodeRoot RelativeWidth=""1920"" RelativeHeight=""1080"">
   <!-Shapes Here...-->
  </ImageNodeRoot>
</ScalableRelativeImage>";
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            if (RefreshButton is not null)
                RefreshButton.Click += RefreshButton_Click;
            if (NewButton is not null)
            {
                NewButton.Click += (_, _) =>
                {
                    NewDocument();
                };
            }
            if (CentralEditor is not null) NewDocument();
            void NewDocument()
            {
                if (CentralEditor is not null)
                    CentralEditor.Text = Template;
            };
            if(CloseWarningsViewButton is not null)
            {
                CloseWarningsViewButton.Click += (_, _) =>
                {
                    if (WarningsView is not null) WarningsView.IsVisible = false;
                };
            }
            if (OpenButton is not null)
            {
                OpenButton.Click += async (a, b) =>
                 {
                     if (CentralEditor is not null)
                     {
                         OpenFileDialog openFileDialog = new OpenFileDialog();
                         openFileDialog.AllowMultiple = false;
                         var filePick = await openFileDialog.ShowAsync(this);
                         if (filePick.Length > 0)
                         {
                             var content = File.ReadAllText(filePick[0]);
                             CentralEditor.Text = content;
                         }
                     }
                 };
            }
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
                                    var vectorimg = Compile();
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
                                        profile.TargetHeight = vectorimg.RelativeHeight;
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
        public ImageNodeRoot? Compile()
        {
            if (CentralEditor is null) return null;
            List<ExecutionWarning> Warnings = new List<ExecutionWarning>();
            var vectorimg = SRIEngine.Deserialize(CentralEditor.Text, out Warnings);
            if (WarningsStackPanel is not null)
            {
                WarningsStackPanel.Children.Clear();
                if (Warnings.Count is not 0)
                {
                    if(WarningsView is not null)
                    WarningsView.IsVisible = true;
                    foreach (var item in Warnings)
                    {

                        TextBlock textBlock = new TextBlock();
                        textBlock.Text = item.ID + ">" + item.Message;
                        WarningsStackPanel.Children.Add(textBlock);
                    }
                }

            }
            return vectorimg;
        }

        Button? OpenButton;
        Button? CloseWarningsViewButton;
        Button? SaveButton;
        Button? SaveAsButton;
        Button? NewButton;
        Button? RefreshButton;
        TextEditor? CentralEditor;
        Image? ImagePreview;
        TextBox? WidthBox;
        TextBox? HeightBox;
        TextBox? ScaleBox;
        StackPanel? WarningsStackPanel;
        Grid? WarningsView;
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            OpenButton = this.FindControl<Button>("OpenButton");
            CloseWarningsViewButton = this.FindControl<Button>("CloseWarningsViewButton");
            SaveButton = this.FindControl<Button>("SaveButton");
            SaveAsButton = this.FindControl<Button>("SaveAsButton");
            NewButton = this.FindControl<Button>("NewButton");
            RefreshButton = this.FindControl<Button>("RefreshButton");
            CentralEditor = this.FindControl<TextEditor>("CentralEditor");
            ImagePreview = this.FindControl<Image>("ImagePreview");
            WidthBox = this.FindControl<TextBox>("ImageWidthBox");
            HeightBox = this.FindControl<TextBox>("ImageHeightBox");
            ScaleBox = this.FindControl<TextBox>("PreviewImageScaleBox");
            WarningsStackPanel = this.FindControl<StackPanel>("WarningsStackPanel");
            WarningsView = this.FindControl<Grid>("WarningsView");
        }
    }
}
