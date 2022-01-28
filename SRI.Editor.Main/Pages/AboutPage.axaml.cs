using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CLUNL.Localization;
using ScalableRelativeImage;
using SRI.Editor.Core;
using SRI.Localization;
using System.IO;

namespace SRI.Editor.Main.Pages
{
    public partial class AboutPage : UserControl, ITabPage, ILocalizable
    {
        static LocalizedString LVersion0 = new LocalizedString("About.Version", "Version:{0}");
        static LocalizedString LVersion1 = new LocalizedString("About.CoreVersion", "Core Version:{0}");
        public AboutPage()
        {
            InitializeComponent();
            VersionBlock.Text = string.Format(LVersion0.ToString(), typeof(MainWindow).Assembly.GetName().Version);// $"Version:{}";
            CoreVersionBlock.Text = string.Format(LVersion1.ToString(), typeof(SRIEngine).Assembly.GetName().Version);// $"Version:{}";
            //CoreVersionBlock.Text = $"Core Version:{typeof(SRIEngine).Assembly.GetName().Version}";

            ApplyLocalization();
        }

        public void Dispose()
        {
        }
        static LocalizedString LAboutTitle = new LocalizedString("About", "About");
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
        LocalizedString LTitle = new LocalizedString("SRIEditor.Title", "SRI Editor");
        public void ApplyLocalization()
        {
            this.FindControl<TextBlock>("Title").Text = LTitle.ToString();
        }
    }
}
