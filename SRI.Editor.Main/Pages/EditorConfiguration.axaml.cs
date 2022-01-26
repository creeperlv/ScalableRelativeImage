using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SRI.Editor.Core;
using SRI.Editor.Extension;
using System.Collections.Generic;
using System.IO;

namespace SRI.Editor.Main.Pages
{
    public partial class EditorConfiguration : UserControl,IEditor
    {
        public EditorConfiguration()
        {
            InitializeComponent();
        }

        public void Dispose()
        {
            
        }

        public string GetTitle()
        {
            return "Editor Configuration";
        }

        public void Insert(string Content)
        {
        }

        public List<FileDialogFilter> ObtainExtensionFilters()
        {
            return new List<FileDialogFilter>();
        }

        public void OpenFile(FileInfo file)
        {
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

        public void SetButton(ITabPageButton button)
        {
        }

        public void SetContent(string content)
        {
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
