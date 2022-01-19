using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SRI.Editor.Main.Controls
{
    public partial class KVBox : Grid
    {
        public KVBox()
        {
            InitializeComponent();
        }
        public KVBox(string k,string v)
        {
            InitializeComponent();
            KBox.Text = k;
            VBox.Text = v;
        }
        TextBox KBox;
        TextBox VBox;
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            KBox = this.FindControl<TextBox>("KeyBox");
            VBox = this.FindControl<TextBox>("ValueBox");
        }
        public (string,string) GetData()
        {
            return (KBox.Text, VBox.Text);
        }
    }
}
