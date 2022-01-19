using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SRI.Editor.Core;
using SRI.Editor.Core.Projects;
using SRI.Editor.Extension;
using SRI.Editor.Main.Controls;
using System.IO;

namespace SRI.Editor.Main.Editors
{
    public partial class ProjectEditor : Grid, IEditor
    {
        public ProjectEditor()
        {
            InitializeComponent();
            AddConfiguration.Click += (_, _) =>
            {
                BuildConfigurationEditor buildConfigurationEditor = new BuildConfigurationEditor();
                Configurations.Children.Add(buildConfigurationEditor);
            };
        }
        Button AddConfiguration;
        StackPanel Configurations;
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            AddConfiguration = this.FindControl<Button>("AddConfiguration");
            Configurations = this.FindControl<StackPanel>("Configurations");
        }

        FileInfo OpendFile=null;
        LoadedProject __proj;
        public void OpenFile(FileInfo file)
        {
            OpendFile = file;
            __proj = ProjectEngine.Load(file);
            if (__proj.CoreProject != null)
                foreach (var item in __proj.CoreProject.BuildConfigurations)
                {
                    Configurations.Children.Add(new BuildConfigurationEditor(item));
                }
        }

        public void SetContent(string content)
        {
        }

        public string GetTitle()
        {
            if (OpendFile == null)
                return "Project Editor";
            else return OpendFile.Name;
        }

        public void Save()
        {
            __proj.CoreProject.BuildConfigurations.Clear();
            foreach (var item in Configurations.Children)
            {
                if (item is BuildConfigurationEditor bce)
                {
                    __proj.CoreProject.BuildConfigurations.Add(bce.ObtainConfiguration());
                }
            }
            ProjectEngine.Save(__proj);
            (Globals.CurrentMainWindow as MainWindow).OpenProject(OpendFile.FullName);
        }

        public void Preview()
        {
        }

        public void Save(FileInfo Path)
        {
            OpendFile=Path;
            __proj.ProjectFile = Path;
            __proj.WorkingDirectory = Path.Directory;
            Save();
        }

        public void Insert(string Content)
        {
        }
        ITabPageButton ParentButton;
        public void SetButton(ITabPageButton button)
        {
            ParentButton = button;
        }

        public void Dispose()
        {
        }
    }
}
