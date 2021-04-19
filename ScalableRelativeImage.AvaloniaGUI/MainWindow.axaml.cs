using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using AvaloniaEdit;
using AvaloniaEdit.Highlighting;
using AvaloniaEdit.Highlighting.Xshd;
using ScalableRelativeImage.Nodes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml;

namespace ScalableRelativeImage.AvaloniaGUI
{
    public class MainWindow : Window
    {
        public static XmlDocument GlobalXmlDocument = new XmlDocument();
        public static readonly string SRIContentTemplate = "<ScalableRelativeImage Flavor=\"CreeperLv.SRI\" FormatVersion=\"1.0.0.0\">" + "\n" +
"\t<ImageNodeRoot RelativeWidth=\"1920\" RelativeHeight=\"1080\">" + "\n" +
"\t\t<!--Shapes Here...-->" + "\n" +
"\t</ImageNodeRoot>" + "\n" +
"</ScalableRelativeImage>";
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
                {
                    XshdSyntaxDefinition xshd;
                    using (XmlTextReader reader = new XmlTextReader("Resources/Theme.xml"))
                    {
                        xshd = HighlightingLoader.LoadXshd(reader);
                        CentralEditor.SyntaxHighlighting = HighlightingLoader.Load(xshd, HighlightingManager.Instance);
                    }
                    CentralEditor.Text = SRIContentTemplate;

                }
                CurrentFile = null;
            };
            if (CloseWarningsViewButton is not null)
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
                             CurrentFile = filePick[0];
                             CentralEditor.Text = content;
                         }
                     }
                 };
            }
            if (SaveButton is not null)
            {
                SaveButton.Click += async (_, _) =>
                 {
                     if (CurrentFile is null) await SaveAs(); else Save();
                 };
            }
            if (SaveAsButton is not null)
            {
                SaveAsButton.Click += async (_, _) =>
                {
                    await SaveAs();
                };
            }
            if (ViewPortZoomBox is not null)
            {
                ViewPortZoomBox.KeyDown += (a, b) =>
                {
                    if (b.Key == Avalonia.Input.Key.Enter) ApplyZoomBox();
                };
            }
            if (ViewPortZoomIn is not null)
            {
                ViewPortZoomIn.Click += (_, _) =>
                {
                    if (ViewPortZoomBox is not null)
                    {
                        float v = float.Parse(ViewPortZoomBox.Text);
                        v += 10;
                        ViewPortZoomBox.Text = "" + v;
                        ApplyZoomBox();
                    }
                };
            }
            if (CloseShapeViewPort is not null)
            {
                CloseShapeViewPort.Click += (_, _) => { if (ShapesViewPort is not null) { ShapesViewPort.IsVisible = false; } };
            }
            if (ViewPortZoomOut is not null)
            {
                ViewPortZoomOut.Click += (_, _) =>
                {
                    if (ViewPortZoomBox is not null)
                    {
                        float v = float.Parse(ViewPortZoomBox.Text);
                        if (v - 10 > 0)
                            v -= 10;
                        ViewPortZoomBox.Text = "" + v;
                        ApplyZoomBox();
                    }
                };
            }
            if (ViewPortZoomApply is not null)
            {
                ViewPortZoomApply.Click += (_, _) =>
                {
                    ApplyZoomBox();
                };
            }
            if (ViewShapeButton is not null)
            {
                ViewShapeButton.Click += (_, _) =>
                {
                    if (ShapesViewPort is not null && ShapeList is not null && CentralEditor is not null)
                    {
                        ShapesViewPort.IsVisible = true;
                        ShapeList.Children.Clear();

                        foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
                        {
                            var TotalTypes = asm.GetTypes();
                            foreach (var item in TotalTypes)
                            {
                                if (item.IsAssignableTo(typeof(INode)))
                                {
                                    if (item.Name != "ImageNodeRoot")
                                        if (item.Name != "INode")
                                            if (item.Name != "IContainer")
                                                if (item.Name != "GraphicNode")
                                                {
                                                    try
                                                    {
                                                        ShapeButton shapeButton = new(item, CentralEditor);
                                                        ShapeList.Children.Add(shapeButton);
                                                    }
                                                    catch (Exception)
                                                    {
                                                    }
                                                }

                                }
                            }
                        }
                    }
                };
            }
            void ApplyZoomBox()
            {
                if (ViewPortZoomBox is not null)
                    try
                    {
                        float v = float.Parse(ViewPortZoomBox.Text);
                        PreviewScale = v / 100f;
                        ApplyPreviewZoom();
                    }
                    catch (System.Exception)
                    {
                    }
            }
            if (AboutPageButton is not null && CentralEditorView is not null && AboutView is not null)
            {
                AboutPageButton.Click += (_, _) => { CentralEditorView.IsVisible = false; AboutView.IsVisible = true; };
            }
            if (CloseAboutPageButton is not null && CentralEditorView is not null && AboutView is not null)
            {
                CloseAboutPageButton.Click += (_, _) => { CentralEditorView.IsVisible = true; AboutView.IsVisible = false; };
            }
            if (RenderImageButton is not null)
            {
                RenderImageButton.Click += async (_, _) =>
                {

                    SaveFileDialog sfd = new SaveFileDialog();
                    var result = await sfd.ShowAsync(this);
                    if (result is not null)
                    {
                        var VI = Compile();
                        if (WidthBox is not null && HeightBox is not null && VI is not null)
                        {

                            RenderProfile profile = new RenderProfile();
                            try
                            {
                                profile.TargetWidth = float.Parse(WidthBox.Text);

                            }
                            catch (System.Exception)
                            {
                                profile.TargetWidth = VI.RelativeWidth;
                            }
                            try
                            {
                                profile.TargetHeight = float.Parse(HeightBox.Text);
                            }
                            catch (System.Exception)
                            {
                                profile.TargetHeight = VI.RelativeHeight;
                            }
                            var img = VI.Render(profile);
                            img.Save(result);
                        }
                    }
                };
            }
            if (ColumnButton is not null && RowButton is not null && EditorGrid is not null && ViewerGrid is not null)
            {
                ColumnButton.Click += (_, _) =>
                {
                    Grid.SetColumn(EditorGrid, 0);
                    Grid.SetColumn(ViewerGrid, 1);
                    Grid.SetRow(EditorGrid, 0);
                    Grid.SetRow(ViewerGrid, 0);
                    Grid.SetRowSpan(EditorGrid, 2);
                    Grid.SetRowSpan(ViewerGrid, 2);
                    Grid.SetColumnSpan(EditorGrid, 1);
                    Grid.SetColumnSpan(ViewerGrid, 1);
                };
                RowButton.Click += (_, _) =>
                {
                    Grid.SetRow(EditorGrid, 1);
                    Grid.SetRow(ViewerGrid, 0);
                    Grid.SetColumn(EditorGrid, 0);
                    Grid.SetColumn(ViewerGrid, 0);
                    Grid.SetRowSpan(EditorGrid, 1);
                    Grid.SetRowSpan(ViewerGrid, 1);
                    Grid.SetColumnSpan(EditorGrid, 2);
                    Grid.SetColumnSpan(ViewerGrid, 2);
                };
            }
            async Task SaveAs()
            {

                SaveFileDialog sfd = new SaveFileDialog();
                var result = await sfd.ShowAsync(this);
                if (result is not null)
                {
                    CurrentFile = result;
                    Save();
                }
            }
            void Save()
            {
                if (CentralEditor is not null && CurrentFile is not null)
                    File.WriteAllText(CurrentFile, CentralEditor.Text);
            }
        }
        string? CurrentFile = null;
        float PreviewOriginWidth = 10;
        float PreviewOriginHeight = 10;
        float PreviewScale = 1f;
        public void ApplyPreviewZoom()
        {
            if (ImagePreview is not null)
            {
                ImagePreview.Width = PreviewOriginWidth * PreviewScale;
                ImagePreview.Height = PreviewOriginHeight * PreviewScale;
            }
        }
        private void RefreshButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Trace.WriteLine("Try Refresh");
            if (CentralEditor is not null && ImagePreview is not null && WidthBox is not null && HeightBox is not null && ScaleBox is not null)
            {
                try
                {
                    var vectorimg = Compile();
                    if (vectorimg is null) return;
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
                    PreviewOriginWidth = profile.TargetWidth;
                    PreviewOriginHeight = profile.TargetHeight;
                    ImagePreview.Width = profile.TargetWidth;
                    ImagePreview.Height = profile.TargetHeight;
                    ApplyPreviewZoom();
                    GC.Collect();
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
                    if (WarningsView is not null)
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
        Button? ViewPortZoomIn;
        Button? ViewPortZoomOut;
        Button? ViewPortZoomApply;
        Button? ViewShapeButton;
        Button? CloseShapeViewPort;
        Button? RenderImageButton;
        Button? AboutPageButton;
        Button? CloseAboutPageButton;
        Button? ColumnButton;
        Button? RowButton;
        TextEditor? CentralEditor;
        Image? ImagePreview;
        TextBox? WidthBox;
        TextBox? HeightBox;
        TextBox? ScaleBox;
        TextBox? ViewPortZoomBox;
        StackPanel? WarningsStackPanel;
        StackPanel? ShapeList;
        Grid? WarningsView;
        Grid? ShapesViewPort;
        Grid? CentralEditorView;
        Grid? AboutView;
        Grid? EditorGrid;
        Grid? ViewerGrid;
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            OpenButton = this.FindControl<Button>("OpenButton");
            CloseWarningsViewButton = this.FindControl<Button>("CloseWarningsViewButton");
            RenderImageButton = this.FindControl<Button>("RenderImageButton");
            SaveButton = this.FindControl<Button>("SaveButton");
            SaveAsButton = this.FindControl<Button>("SaveAsButton");
            NewButton = this.FindControl<Button>("NewButton");
            RefreshButton = this.FindControl<Button>("RefreshButton");
            ViewPortZoomIn = this.FindControl<Button>("ViewPortZoomIn");
            ViewPortZoomOut = this.FindControl<Button>("ViewPortZoomOut");
            ViewPortZoomApply = this.FindControl<Button>("ViewPortZoomApply");
            ViewShapeButton = this.FindControl<Button>("ViewShapeButton");
            CloseShapeViewPort = this.FindControl<Button>("CloseShapeViewPort");
            AboutPageButton = this.FindControl<Button>("AboutPageButton");
            CloseAboutPageButton = this.FindControl<Button>("CloseAboutPageButton");
            RowButton = this.FindControl<Button>("RowButton");
            ColumnButton = this.FindControl<Button>("ColumnButton");
            CentralEditor = this.FindControl<TextEditor>("CentralEditor");
            ImagePreview = this.FindControl<Image>("ImagePreview");
            WidthBox = this.FindControl<TextBox>("ImageWidthBox");
            HeightBox = this.FindControl<TextBox>("ImageHeightBox");
            ScaleBox = this.FindControl<TextBox>("PreviewImageScaleBox");
            ViewPortZoomBox = this.FindControl<TextBox>("ViewPortZoomBox");
            WarningsStackPanel = this.FindControl<StackPanel>("WarningsStackPanel");
            ShapeList = this.FindControl<StackPanel>("ShapeList");
            WarningsView = this.FindControl<Grid>("WarningsView");
            ShapesViewPort = this.FindControl<Grid>("ShapesViewPort");
            CentralEditorView = this.FindControl<Grid>("CentralEditorView");
            AboutView = this.FindControl<Grid>("AboutView");
            EditorGrid = this.FindControl<Grid>("EditorGrid");
            ViewerGrid = this.FindControl<Grid>("ViewerGrid");
        }
    }
}
