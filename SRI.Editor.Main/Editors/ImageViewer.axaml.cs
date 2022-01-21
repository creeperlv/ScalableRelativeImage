using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SRI.Editor.Core;
using SRI.Editor.Extension;
using System.IO;

namespace SRI.Editor.Main.Editors
{
    public partial class ImageViewer : UserControl,IEditor
    {
        public ImageViewer()
        {
            InitializeComponent();
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public string GetTitle()
        {
            throw new System.NotImplementedException();
        }

        public void Insert(string Content)
        {
            throw new System.NotImplementedException();
        }

        public void OpenFile(FileInfo file)
        {
            throw new System.NotImplementedException();
        }

        public void Preview()
        {
            throw new System.NotImplementedException();
        }

        public void Save()
        {
            throw new System.NotImplementedException();
        }

        public void Save(FileInfo Path)
        {
            throw new System.NotImplementedException();
        }

        public void SetButton(ITabPageButton button)
        {
            throw new System.NotImplementedException();
        }

        public void SetContent(string content)
        {
            throw new System.NotImplementedException();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
