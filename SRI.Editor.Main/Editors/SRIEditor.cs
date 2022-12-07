using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using ScalableRelativeImage;
using ScalableRelativeImage.Nodes;
using SRI.Core.Backend;
using SRI.Core.Core;
using SRI.Editor.Main.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRI.Editor.Main.Editors
{
    public partial class SRIEditor
    {
        Button ViewPortZoomIn;
        Button ViewPortZoomOut;
        Button ViewPortZoomApply;
        Button ColumnButton;
        Button RowButton;
        Button ExportButton;
        Button RefreshButton;
        ToggleButton CodeView;
        ToggleButton ImageView;
        Grid EditorGrid;
        Grid ViewerGrid;

        Avalonia.Controls.Image ImagePreview;
        TextBox WidthBox;
        TextBox HeightBox;
        TextBox ScaleBox;
        TextBox ViewPortZoomBox;

        ToggleSwitch ApplyDesignSymbol;
        int ViewMode = 0;

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
        void InitControls()
        {

            RefreshButton = this.FindControl<Button>("RefreshButton");
            ViewPortZoomIn = this.FindControl<Button>("ViewPortZoomIn");
            ViewPortZoomOut = this.FindControl<Button>("ViewPortZoomOut");
            ViewPortZoomApply = this.FindControl<Button>("ViewPortZoomApply");
            RowButton = this.FindControl<Button>("RowButton");
            ColumnButton = this.FindControl<Button>("ColumnButton");
            ExportButton = this.FindControl<Button>("ExportButton");
            ImagePreview = this.FindControl<Image>("ImagePreview");
            WidthBox = this.FindControl<TextBox>("WidthBox");
            HeightBox = this.FindControl<TextBox>("HeightBox");
            ScaleBox = this.FindControl<TextBox>("ScaleBox");
            ViewPortZoomBox = this.FindControl<TextBox>("ViewPortZoomBox");
            EditorGrid = this.FindControl<Grid>("EditorGrid");
            ViewerGrid = this.FindControl<Grid>("ViewerGrid");

            CodeView = this.FindControl<ToggleButton>("CodeView");
            ImageView = this.FindControl<ToggleButton>("ImageView");
            ApplyDesignSymbol = this.FindControl<ToggleSwitch>("ApplyDesignSymbol");
        }
        void RowButtonClick()
        {
            ViewMode = 0;
            Grid.SetRow(EditorGrid, 1);
            Grid.SetRow(ViewerGrid, 0);
            Grid.SetColumn(EditorGrid, 0);
            Grid.SetColumn(ViewerGrid, 0);
            Grid.SetRowSpan(EditorGrid, 1);
            Grid.SetRowSpan(ViewerGrid, 1);
            Grid.SetColumnSpan(EditorGrid, 2);
            Grid.SetColumnSpan(ViewerGrid, 2);
        }
        private void ColumnButtonClick()
        {
            ViewMode = 1;
            Grid.SetColumn(EditorGrid, 0);
            Grid.SetColumn(ViewerGrid, 1);
            Grid.SetRow(EditorGrid, 0);
            Grid.SetRow(ViewerGrid, 0);
            Grid.SetRowSpan(EditorGrid, 2);
            Grid.SetRowSpan(ViewerGrid, 2);
            Grid.SetColumnSpan(EditorGrid, 1);
            Grid.SetColumnSpan(ViewerGrid, 1);
        }
        public ImageNodeRoot Compile()
        {
            BackendFactory.UsingBackend = (BackendDefinition)EditorConfiguration.CurrentConfiguration.Backend;
            if (CentralEditor is null) return null;
            List<ExecutionWarning> Warnings;
            var vectorimg = SRIEngine.Deserialize(CentralEditor.Text, out Warnings);
            {
                //WarningsStackPanel.Children.Clear();
                //if (Warnings.Count is not 0)
                //{
                //    foreach (var item in Warnings)
                //    {
                //        OutputConsole(item.ID + ">" + item.Message);
                //        //TextBlock textBlock = new TextBlock();
                //        //textBlock.Text = ;
                //        //WarningsStackPanel.Children.Add(textBlock);
                //    }
                //}

            }
            return vectorimg;
        }
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
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            {
                try
                {
                    BackendFactory.UsingBackend = (BackendDefinition)EditorConfiguration.CurrentConfiguration.Backend;
                    var vectorimg = Compile();
                    if (vectorimg is null) return;
                    RenderProfile profile = new RenderProfile();
                    if (OpenedFile!= null)
                        {
                            profile.WorkingDirectory = OpenedFile.DirectoryName;
                        }
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
                    if (ApplyDesignSymbol.IsChecked == true)
                    {
                        vectorimg.Symbols.Set(new Symbol { Name = "DESIGN", Value = "True" });
                    }

                    using (var img = vectorimg.Render(profile))
                    {
                        MemoryStream memoryStream = new MemoryStream();
                        img.Save(memoryStream, UniversalImageFormat.PNG);
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
        void ReArrange()
        {
            if (ViewMode == 0)
            {
                RowButtonClick();
            }
            else
            {
                ColumnButtonClick();
            }
            if (ViewerGrid.IsVisible == false)
            {
                Grid.SetColumnSpan(EditorGrid, 2);
                Grid.SetRowSpan(EditorGrid, 2);
                if (ViewMode == 0)
                {
                    Grid.SetColumn(EditorGrid, 0);
                    Grid.SetRow(EditorGrid, 0);

                }
            }
            else
            {

                if (EditorGrid.IsVisible == false)
                {
                    Grid.SetColumnSpan(ViewerGrid, 2);
                    Grid.SetRowSpan(ViewerGrid, 2);
                    Grid.SetColumn(ViewerGrid, 0);
                    Grid.SetRow(ViewerGrid, 0);
                }
            }
        }
    }
}
