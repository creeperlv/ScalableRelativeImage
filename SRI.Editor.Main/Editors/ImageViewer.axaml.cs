using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SRI.Editor.Main.Editors
{
    public partial class ImageViewer : UserControl
    {
        public ImageViewer()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
