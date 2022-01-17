using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using SRI.Editor.Core;
using SRI.Editor.Main.Pages;
using System.Collections.Generic;
using System.Linq;

namespace SRI.Editor.Main
{
    public partial class MainWindow : Window, ITabPageContainer
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            InitializeWindow();
            Help_About.Click += (_, _) =>
            {
                AddPage(new AboutPage());
            };
        }
        void InitializeWindow()
        {
            ExtendClientAreaChromeHints = Avalonia.Platform.ExtendClientAreaChromeHints.PreferSystemChrome;
            ExtendClientAreaToDecorationsHint = true;
            TransparencyLevelHint = WindowTransparencyLevel.Blur;
            Background = new SolidColorBrush(Colors.Transparent);
        }
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            TabPageButtons = this.FindControl<StackPanel>("TabPageButtons");
            TabPageContent = this.FindControl<Grid>("TabPageContent");
            Help_About = this.FindControl<MenuItem>("Help_About");

        }
        StackPanel TabPageButtons;
        Grid TabPageContent;
        MenuItem Help_About;

        public void RemovePage(ITabPage page, ITabPageButton button)
        {
            TabPageContent.Children.Remove((IControl)page);
            TabPageButtons.Children.Remove((IControl)button);
            if (CurrentButton == button)
                if (TabPageButtons.Children.Count != 0)
                {
                    ShowPage(TabPageButtons.Children.Last() as ITabPageButton);
                }

            System.GC.Collect();
        }
        ITabPageButton CurrentButton;
        public void ShowPage(ITabPageButton button)
        {
            CurrentButton = button;
            foreach (var item in TabPageButtons.Children)
            {
                if (item is ITabPageButton btn)
                {

                    if (btn == button)
                    {
                        btn.Show();
                    }
                    else
                    {
                        btn.Hide();
                    }
                }
            }
        }

        public void AddPage(ITabPage page)
        {
            TabPageButton tabPageButton = new TabPageButton();
            TabPageButtons.Children.Add(tabPageButton);
            TabPageContent.Children.Add(page as IControl);
            tabPageButton.ParentContainer = this;
            tabPageButton.ControlledPage = page;
            ShowPage(tabPageButton);
        }
    }
}
