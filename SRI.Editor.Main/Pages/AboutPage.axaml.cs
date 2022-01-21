using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ScalableRelativeImage;
using SRI.Editor.Core;
using SRI.Localization;
using System.IO;

namespace SRI.Editor.Main.Pages
{
    public partial class AboutPage : UserControl,ITabPage,ILocalizable
    {
        public AboutPage()
        {
            InitializeComponent();
            VersionBlock.Text = $"Version:{typeof(MainWindow).Assembly.GetName().Version}";
            CoreVersionBlock.Text = $"Core Version:{typeof(SRIEngine).Assembly.GetName().Version}";

            ApplyLocal();
        }

        public void Dispose()
        {
        }
        LocalizedString LAboutTitle=new LocalizedString("About", "About");
        public string GetTitle() => LAboutTitle.ToString();

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
        LocalizedString LTitle=new LocalizedString("SRIEditor.Title","SRI Editor");
        public void ApplyLocal()
        {
            this.FindControl<TextBlock>("Title").Text = LTitle.ToString() ;
        }
    }
}
