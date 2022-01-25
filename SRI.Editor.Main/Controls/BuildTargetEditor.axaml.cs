using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ScalableRelativeImage;
using SRI.Editor.Core.Projects;
using SRI.Localization;

namespace SRI.Editor.Main.Controls
{
    public partial class BuildTargetEditor : Grid, ILocalizable
    {
        public BuildTargetEditor()
        {
            InitializeComponent();
            InitEvents();
            ApplyLocal();
        }
        public BuildTargetEditor(BuildTarget target)
        {

            InitializeComponent();
            InitEvents();
            ApplyLocal();
            {

                NameBox.Text = target.Name;
                OutputBox.Text = target.OutputName;

                FileBox.Text = target.File;
                ForegroundBox.Text = target.Foreground;
                BackgroundBox.Text = target.Background;
                WidthBox.Text = target.Width + "";
                HeightBox.Text = target.Height + "";
                foreach (var item in target.Symbols)
                {
                    Symbols.Children.Add(new KVBox(item.Name, item.Value));
                }
            }
        }
        void InitEvents()
        {
            AddSymbols.Click += (_, _) =>
            {
                Symbols.Children.Add(new KVBox());
            };
            DuplicateButton.Click += (_, _) =>
            {

                (Parent as StackPanel).Children.Add(new BuildTargetEditor(this.ObtainBuildTarget()));
            };
        }
        public BuildTarget ObtainBuildTarget()
        {
            var __target = new BuildTarget();
            __target.Name = NameBox.Text;
            __target.OutputName = OutputBox.Text;
            __target.File = FileBox.Text;
            __target.Foreground = ForegroundBox.Text;
            __target.Background = BackgroundBox.Text;
            __target.Width = int.Parse(WidthBox.Text);
            __target.Height = int.Parse(HeightBox.Text);

            foreach (var item in Symbols.Children)
            {
                if (item is KVBox kv)
                {
                    var d = kv.GetData();
                    __target.Symbols.Add(new Symbol { Name = d.Item1, Value = d.Item2 });
                }
            }
            return __target;
        }
        Button AddSymbols;
        Button DuplicateButton;
        StackPanel Symbols;

        TextBox FileBox;
        TextBox NameBox;
        TextBox OutputBox;
        TextBox ForegroundBox;
        TextBox BackgroundBox;
        TextBox WidthBox;
        TextBox HeightBox;

        TextBlock NameB;
        TextBlock SourceB;
        TextBlock OutputB;
        TextBlock Symbols_Text;
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            AddSymbols = this.FindControl<Button>("AddSymbols");
            DuplicateButton = this.FindControl<Button>("DuplicateButton");
            Symbols = this.FindControl<StackPanel>("Symbols");
            FileBox = this.FindControl<TextBox>("FileBox");
            NameBox = this.FindControl<TextBox>("NameBox");
            OutputBox = this.FindControl<TextBox>("OutputBox");
            ForegroundBox = this.FindControl<TextBox>("ForegroundBox");
            BackgroundBox = this.FindControl<TextBox>("BackgroundBox");
            WidthBox = this.FindControl<TextBox>("WidthBox");
            HeightBox = this.FindControl<TextBox>("HeightBox");
            Symbols_Text = this.FindControl<TextBlock>("Symbols_Text");
            NameB = this.FindControl<TextBlock>("NameB");
            SourceB = this.FindControl<TextBlock>("SourceB");
            OutputB = this.FindControl<TextBlock>("OutputB");
            this.FindControl<Button>("RemoveButton").Click += (_, _) =>
            {
                (Parent as StackPanel).Children.Remove(this);
            };
        }
        static LocalizedString LSymbols = new LocalizedString("Symbols", "Symbols");
        static LocalizedString LName = new LocalizedString("Project.Name", "Name");
        static LocalizedString LSource = new LocalizedString("Project.Source", "Source File");
        static LocalizedString LOutput = new LocalizedString("Project.Output", "Output File");
        public void ApplyLocal()
        {
            Symbols_Text.Text = LSymbols;
            NameB.Text = LName;
            SourceB.Text = LSource;
            OutputB.Text = LOutput;
        }
    }
}
