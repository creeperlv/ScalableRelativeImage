using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ScalableRelativeImage;
using SRI.Editor.Core;

namespace SRI.Editor.Main.Pages
{
    public partial class AboutPage : UserControl,ITabPage
    {
        public AboutPage()
        {
            InitializeComponent();
            VersionBlock.Text = $"Version:{typeof(MainWindow).Assembly.GetName().Version}";
            CoreVersionBlock.Text = $"Core Version:{typeof(SRIEngine).Assembly.GetName().Version}";
        }

        public void Dispose()
        {
        }
        string Title="About";
        public string GetTitle() => Title;

        public void Preview()
        {
        }

        public void Save()
        {
        }

        public void Save(string Path)
        {
        }

        public void SetButton(ITabPageButton button)
        {
        }

        public bool TryClose()
        {
            return true;
        }
        TextBlock VersionBlock;
        TextBlock CoreVersionBlock;
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            VersionBlock = this.FindControl<TextBlock>("VersionBlock");
            CoreVersionBlock = this.FindControl<TextBlock>("CoreVersionBlock");
        }

        public void Insert(string Content)
        {
        }
    }
}
