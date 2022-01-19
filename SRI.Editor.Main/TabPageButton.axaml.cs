using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using SRI.Editor.Core;
using System;

namespace SRI.Editor.Main
{
    public partial class TabPageButton : Grid, ITabPageButton
    {
        public TabPageButton()
        {
            InitializeComponent();
            CloseButton.Click += (_, _) =>
            {
                Close(null,null);
            };
            NameAreaButton.Click += (_, _) => { ParentContainer.ShowPage(this); };
        }
        Button NameAreaButton;
        Button CloseButton;
        public void SetTitle(string title)
        {
            NameAreaButton.Content = title;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            NameAreaButton = this.FindControl<Button>("NameAreaButton");
            CloseButton = this.FindControl<Button>("CloseButton");
        }

        public void Show()
        {
            this.Background = new SolidColorBrush(Color.FromArgb(128,0x22,0x88,0xEE));
            (child as IControl).IsVisible=true;
        }

        public void Hide()
        {
            this.Background = new SolidColorBrush(Color.FromArgb(01,0,0,0));
            (child as IControl).IsVisible=false;
        }

        public bool Close(Action act,Action act1)
        {
            if (child != null)
            {
                if (child.TryClose(act,act1))
                {
                    ParentContainer.RemovePage(child, this);
                    child.Dispose();
                    return true;
                }
                return false;
            }
            else
            {
                return true;
            }
        }

        ITabPage child;
        public ITabPage ControlledPage
        {
            get => child; set
            {
                child = value;
                SetTitle(child.GetTitle());
            }
        }

        public ITabPageContainer ParentContainer { get; set; }
    }
}
