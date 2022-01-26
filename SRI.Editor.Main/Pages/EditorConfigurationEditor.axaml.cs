using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SRI.Editor.Core;
using SRI.Editor.Extension;
using SRI.Editor.Main.Data;
using System.Collections.Generic;
using System.IO;

namespace SRI.Editor.Main.Pages
{
    public partial class EditorConfigurationEditor : Grid, IEditor
    {
        public EditorConfigurationEditor()
        {
            InitializeComponent();
            LoadFromSettings();
        }
        public void LoadFromSettings()
        {
            UseBlurSwitch.IsChecked=EditorConfiguration.CurrentConfiguration.isBlurEnabled;
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
            EditorConfiguration.CurrentConfiguration.isBlurEnabled = UseBlurSwitch.IsChecked.Value;
            EditorConfiguration.Save();
            (Globals.CurrentMainWindow as MainWindow).ApplyConfiguration();
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
        ToggleSwitch UseBlurSwitch;
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            UseBlurSwitch = this.FindControl<ToggleSwitch>("UseBlurSwitch");
        }
    }
}
