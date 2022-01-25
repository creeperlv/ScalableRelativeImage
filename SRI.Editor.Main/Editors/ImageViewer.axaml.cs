using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using SRI.Editor.Core;
using SRI.Editor.Extension;
using System;
using System.Collections.Generic;
using System.IO;

namespace SRI.Editor.Main.Editors
{
    public partial class ImageViewer : UserControl,IEditor
    {
        public ImageViewer()
        {
            InitializeComponent();
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
        FileInfo __file=null;
        public void Dispose()
        {
        }

        public string GetTitle()
        {
            if (__file != null)
                return __file.Name;
            else return "Image Viewer";
        }

        public void Insert(string Content)
        {
        }
        Size S;
        public void OpenFile(FileInfo file)
        {
            __file = file;
            var B= new Bitmap(__file.FullName);
            ImagePreview.Source = B;
            S = B.Size;
            button0.SetTitle(GetTitle());
        }

        public void Preview()
        {
        }

        public void Save()
        {
        }

        public void Save(FileInfo Path)
        {
        }
        ITabPageButton button0;
        public void SetButton(ITabPageButton button)
        {
            button0= button;
        }

        public void SetContent(string content)
        {
        }
        Image ImagePreview;
        Button ViewPortZoomIn;
        Button ViewPortZoomOut;
        Button ViewPortZoomApply;
        TextBox ViewPortZoomBox;

        float PreviewScale = 1;

        public void ApplyPreviewZoom()
        {
            {
                ImagePreview.Width = S.Width * PreviewScale;
                ImagePreview.Height = S.Height * PreviewScale;
            }
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            ImagePreview = this.FindControl<Image>("ImagePreview");
            ViewPortZoomIn = this.FindControl<Button>("ViewPortZoomIn");
            ViewPortZoomOut = this.FindControl<Button>("ViewPortZoomOut");
            ViewPortZoomApply = this.FindControl<Button>("ViewPortZoomApply");
            ViewPortZoomBox = this.FindControl<TextBox>("ViewPortZoomBox");

        }

        public List<FileDialogFilter> ObtainExtensionFilters()
        {
            return new List<FileDialogFilter>();
        }
    }
}
