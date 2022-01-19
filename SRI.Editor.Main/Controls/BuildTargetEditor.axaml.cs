using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ScalableRelativeImage;
using SRI.Editor.Core.Projects;

namespace SRI.Editor.Main.Controls
{
    public partial class BuildTargetEditor : Grid
    {
        public BuildTargetEditor()
        {
            InitializeComponent();
            InitEvents();
        }
        public BuildTargetEditor(BuildTarget target)
        {

            InitializeComponent();
            InitEvents();
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
        StackPanel Symbols;

        TextBox FileBox;
        TextBox NameBox;
        TextBox OutputBox;
        TextBox ForegroundBox;
        TextBox BackgroundBox;
        TextBox WidthBox;
        TextBox HeightBox;
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            AddSymbols = this.FindControl<Button>("AddSymbols");
            Symbols = this.FindControl<StackPanel>("Symbols");
            FileBox = this.FindControl<TextBox>("FileBox");
            NameBox = this.FindControl<TextBox>("NameBox");
            OutputBox = this.FindControl<TextBox>("OutputBox");
            ForegroundBox = this.FindControl<TextBox>("ForegroundBox");
            BackgroundBox = this.FindControl<TextBox>("BackgroundBox");
            WidthBox = this.FindControl<TextBox>("WidthBox");
            HeightBox = this.FindControl<TextBox>("HeightBox");
        }
    }
}
