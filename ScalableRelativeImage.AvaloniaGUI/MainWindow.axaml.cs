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
        string LastContent = "";
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
            RefreshButton.Click += RefreshButton_Click;
            {
                NewButton.Click += (_, _) =>
                {
                    NewDocument();
                };
            }
            this.Closing += (a, b) =>
            {
                Closing(null,()=> {
                    b.Cancel = true;
                });
            };
            DialogGrid.IsVisible = false;
            NewDocument();
            void NewDocument()
            {
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
            {
                CloseWarningsViewButton.Click += (_, _) =>
                {
                    WarningsView.IsVisible = false;
                };
            }
            {
                OpenButton.Click += async (a, b) =>
                 {
                     {
                         OpenFileDialog openFileDialog = new OpenFileDialog();
                         openFileDialog.AllowMultiple = false;
                         var filePick = await openFileDialog.ShowAsync(this);
                         if (filePick.Length > 0)
                         {
                             var content = File.ReadAllText(filePick[0]);
                             CurrentFile = filePick[0];
                             LastContent = content;
                             CentralEditor.Text = content;
                         }
                     }
                 };
            }
            {
                SaveButton.Click += async (_, _) =>
                 {
                     if (CurrentFile is null) await SaveAs(); else Save();
                 };
            }
            {
                SaveAsButton.Click += async (_, _) =>
                {
                    await SaveAs();
                };
            }
            {
                ViewPortZoomBox.KeyDown += (a, b) =>
                {
                    if (b.Key == Avalonia.Input.Key.Enter) ApplyZoomBox();
                };
            }
            {
                ViewPortZoomIn.Click += (_, _) =>
                {
                    {
                        float v = float.Parse(ViewPortZoomBox.Text);
                        v += 10;
                        ViewPortZoomBox.Text = "" + v;
                        ApplyZoomBox();
                    }
                };
            }
            {
                CloseShapeViewPort.Click += (_, _) => { { ShapesViewPort.IsVisible = false; } };
            }
            {
                ViewPortZoomOut.Click += (_, _) =>
                {
                    {
                        float v = float.Parse(ViewPortZoomBox.Text);
                        if (v - 10 > 0)
                            v -= 10;
                        ViewPortZoomBox.Text = "" + v;
                        ApplyZoomBox();
                    }
                };
            }
            {
                ViewPortZoomApply.Click += (_, _) =>
                {
                    ApplyZoomBox();
                };
            }
            {
                this.FindControl<Button>("GithubPageButton").Click += (_, _) =>
                {
                    //                    Process.Start("https://github.com/creeperlv/ScalableRelativeImage/");
                    Show3ButtonDialog("GitHub Repository", "Please visit:https://github.com/creeperlv/ScalableRelativeImage/", new CommandableButton("OK", null));
                };
                this.FindControl<Button>("CloseButton").Click += (_, _) =>
                {
                    Closing();
                };
            }
            {
                ViewShapeButton.Click += (_, _) =>
                {
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
            void Closing(Action Def = null, Action Addition = null)
            {

                if (CentralEditor.Text != SRIContentTemplate)
                {
                    if (LastContent != CentralEditor.Text)
                    {
                        //
                        Addition();
                        Show3ButtonDialog("File Changed", "File has changed since last save/load, do you want to save before you close?",
                            new CommandableButton("Save", Save), new CommandableButton("No", () => { Environment.Exit(0); }), new CommandableButton("Cancel", null));
                    }
                    else if (Def is not null) Def();
                }
                else if (Def is not null) Def();
            }
            void ApplyZoomBox()
            {
                try
                {
                    float v = float.Parse(ViewPortZoomBox.Text);
                    PreviewScale = v / 100f;
                    ApplyPreviewZoom();
                }
                catch (Exception)
                {
                }
            }
            {
                AboutPageButton.Click += (_, _) => { CentralEditorView.IsVisible = false; AboutView.IsVisible = true; };
            }
            {
                CloseAboutPageButton.Click += (_, _) => { CentralEditorView.IsVisible = true; AboutView.IsVisible = false; };
            }
            {
                RenderImageButton.Click += async (_, _) =>
                {

                    SaveFileDialog sfd = new SaveFileDialog();
                    var result = await sfd.ShowAsync(this);
                    if (result is not null)
                    {
                        var VI = Compile();
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
                {
                    CurrentFile = result;
                    Save();
                }
            }
            void Save()
            {
                {
                    LastContent = CentralEditor.Text;
                    File.WriteAllText(CurrentFile, CentralEditor.Text);
                }
            }
        }
        string CurrentFile = null;
        float PreviewOriginWidth = 10;
        float PreviewOriginHeight = 10;
        float PreviewScale = 1f;
        public void ApplyPreviewZoom()
        {
            {
                ImagePreview.Width = PreviewOriginWidth * PreviewScale;
                ImagePreview.Height = PreviewOriginHeight * PreviewScale;
            }
        }
        private void RefreshButton_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Trace.WriteLine("Try Refresh");
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
                   
                   
                    using (var img = vectorimg.Render(profile))
                    {
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
                        memoryStream.Dispose();
                        GC.Collect();
                    }
                    
                    return;
                }
                catch (System.Exception exception)
                {

                    Trace.WriteLine(exception);
                }

            }
            Trace.WriteLine("Something went wrong...");
        }
        public ImageNodeRoot Compile()
        {
            if (CentralEditor is null) return null;
            List<ExecutionWarning> Warnings;
            var vectorimg = SRIEngine.Deserialize(CentralEditor.Text, out Warnings);
            {
                WarningsStackPanel.Children.Clear();
                if (Warnings.Count is not 0)
                {
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

        Button OpenButton;
        Button CloseWarningsViewButton;
        Button SaveButton;
        Button SaveAsButton;
        Button NewButton;
        Button RefreshButton;
        Button ViewPortZoomIn;
        Button ViewPortZoomOut;
        Button ViewPortZoomApply;
        Button ViewShapeButton;
        Button CloseShapeViewPort;
        Button RenderImageButton;
        Button AboutPageButton;
        Button CloseAboutPageButton;
        Button ColumnButton;
        Button RowButton;
        TextEditor CentralEditor;
        Image ImagePreview;
        TextBox WidthBox;
        TextBox HeightBox;
        TextBox ScaleBox;
        TextBox ViewPortZoomBox;
        StackPanel WarningsStackPanel;
        StackPanel ShapeList;
        Grid WarningsView;
        Grid ShapesViewPort;
        Grid CentralEditorView;
        Grid AboutView;
        Grid EditorGrid;
        Grid ViewerGrid;
        Grid DialogGrid;
        Grid CentralViewPort;
        Button Button0;
        Button Button1;
        Button Button2;
        TextBlock DialogTitle;
        TextBlock DialogContent;
        ScrollViewer MenuArea;
        public void Show3ButtonDialog(string Title, string Content, CommandableButton Button0 = null, CommandableButton Button1 = null, CommandableButton Button2 = null)
        {
            DialogTitle.Text = Title;
            DialogContent.Text = Content;
            OnShowDialog();
            if (Button0 is not null)
            {
                this.Button0.IsVisible = true;
                this.Button0.Content = Button0.Content;
                this.Button0.Click += (_, _) =>
                {
                    OnHideDialog();
                    if (Button0.action is not null) Button0.action();
                };

            }
            else
                this.Button0.IsVisible = false;
            if (Button1 is not null)
            {
                this.Button1.IsVisible = true;
                this.Button1.Content = Button1.Content;
                this.Button1.Click += (_, _) =>
                {
                    OnHideDialog();
                    if (Button1.action is not null) Button1.action();
                };

            }
            else
                this.Button1.IsVisible = false;
            if (Button2 is not null)
            {
                this.Button2.IsVisible = true;
                this.Button2.Content = Button2.Content;
                this.Button2.Click += (_, _) =>
                {
                    OnHideDialog();
                    if (Button2.action is not null) Button2.action();
                };

            }
            else
                this.Button2.IsVisible = false;
        }
        void OnHideDialog()
        {
            CentralViewPort.Focusable = true;
            CentralViewPort.IsEnabled = true;
            MenuArea.IsEnabled = true;
            DialogGrid.IsVisible = false;
        }
        void OnShowDialog()
        {
            CentralViewPort.Focusable = false;
            CentralViewPort.IsEnabled = false;
            MenuArea.IsEnabled = false;
            DialogGrid.IsVisible = true;
        }
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

            CentralViewPort = this.FindControl<Grid>("CentralViewPort");
            MenuArea = this.FindControl<ScrollViewer>("MenuArea");

            DialogGrid = this.FindControl<Grid>("Dialogs");
            Button0 = this.FindControl<Button>("DialogButton0");
            Button1 = this.FindControl<Button>("DialogButton1");
            Button2 = this.FindControl<Button>("DialogButton2");
            DialogTitle = this.FindControl<TextBlock>("DialogTitle");
            DialogContent = this.FindControl<TextBlock>("DialogContent");
        }
    }
    public class CommandableButton
    {
        public string Content;
        public Action action;
        public CommandableButton(string Content, Action action = null)
        {
            this.Content = Content;
            this.action = action;
        }
    }
}
