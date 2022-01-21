using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ScalableRelativeImage;
using SRI.Editor.Core.Projects;
using SRI.Localization;

namespace SRI.Editor.Main.Controls
{
    public partial class BuildConfigurationEditor : Grid,ILocalizable
    {
        public BuildConfigurationEditor()
        {
            InitializeComponent();
            InitEvents();
            ApplyLocal();
        }
        public BuildConfigurationEditor(BuildConfiguration config)
        {
            InitializeComponent();
            InitEvents();
            ApplyLocal();
            NameBox.Text = config.Name;
            OutputBox.Text = config.OutputDirectory;
            foreach (var item in config.Symbols)
            {
                Symbols.Children.Add(new KVBox(item.Name, item.Value));
            }
            foreach (var item in config.BuildTargets)
            {
                Configurations.Children.Add(new BuildTargetEditor(item));
            }
        }
        private void InitEvents()
        {
            AddConfiguration.Click += (_, _) =>
            {
                Configurations.Children.Add(new BuildTargetEditor());
            };
            AddSymbol.Click += (_, _) =>
            {
                Symbols.Children.Add(new KVBox());
            };
        }

        public BuildConfiguration ObtainConfiguration()
        {
            var __conf = new BuildConfiguration();
            __conf.Name = NameBox.Text;
            __conf.OutputDirectory = OutputBox.Text;
            foreach (var item in Symbols.Children)
            {
                if (item is KVBox kv)
                {
                    var d = kv.GetData();
                    __conf.Symbols.Add(new Symbol { Name = d.Item1, Value = d.Item2 });
                }
            }
            foreach (var item in Configurations.Children)
            {
                if (item is BuildTargetEditor bte)
                {
                    __conf.BuildTargets.Add(bte.ObtainBuildTarget());
                }
            }
            return __conf;
        }

        Button AddConfiguration;
        Button AddSymbol;

        TextBox NameBox;
        TextBox OutputBox;

        StackPanel Configurations;
        StackPanel Symbols;

        TextBlock Symbols_Text;
        TextBlock BuildTargets_Text;
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            AddConfiguration = this.FindControl<Button>("AddConfiguration");
            AddSymbol = this.FindControl<Button>("AddSymbol");
            Configurations = this.FindControl<StackPanel>("Configurations");
            Symbols = this.FindControl<StackPanel>("Symbols");
            NameBox = this.FindControl<TextBox>("NameBox");
            OutputBox = this.FindControl<TextBox>("OutputBox");
            Symbols_Text = this.FindControl<TextBlock>("Symbols_Text");
            BuildTargets_Text = this.FindControl<TextBlock>("BuildTargets_Text");
            this.FindControl<Button>("RemoveButton").Click += (_, _) => {
                (Parent as StackPanel).Children.Remove(this);
            };
        }

        static LocalizedString LSymbols = new LocalizedString("Symbols", "Symbols");
        static LocalizedString LBuildTargets_Text = new LocalizedString("BuildTargets", "BuildTargets");
        public void ApplyLocal()
        {

            Symbols_Text.Text = LSymbols;
            BuildTargets_Text.Text = LBuildTargets_Text;
            foreach (var item in Configurations.Children)
            {
                if (item is BuildTargetEditor bte)
                {
                    bte.ApplyLocal();
                }
            }
        }
    }
}
