using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using ScalableRelativeImage.Nodes;
using SRI.Editor.Core;
using SRI.Editor.Main.Controls;
using SRI.Editor.Main.Pages;
using System;
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
            if (Program.isDesign) return;
            InitializeWindow();
            Help_About.Click += (_, _) =>
            {
                AddPage(new AboutPage());
            };
            File_New_PF.Click += (_, _) =>
            {
                AddPage(new BaseEditor());
            };
            ShapesListRefreshButton.Click += (_, _) =>
            {
                LoadShapeList();
            };
            LoadShapeList();
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
            ShapesList = this.FindControl<StackPanel>("ShapesList");
            TabPageContent = this.FindControl<Grid>("TabPageContent");
            Help_About = this.FindControl<MenuItem>("Help_About");
            File_New_PF = this.FindControl<MenuItem>("File_New_PF");
            ShapesListRefreshButton = this.FindControl<Button>("ShapesListRefreshButton");

        }
        StackPanel TabPageButtons;
        StackPanel ShapesList;
        Grid TabPageContent;
        MenuItem Help_About;
        MenuItem File_New_PF;

        Button ShapesListRefreshButton;
        public void LoadShapeList()
        {
            ShapesList.Children.Clear();

            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                var TotalTypes = asm.GetTypes();
                foreach (var item in TotalTypes)
                {
                    if (item.IsAssignableTo(typeof(INode)))
                    {
                        if (item.Name != "ImageNodeRoot")
                            if (item.Name != "INode")
                                if (item.Name != "IContainer")
                                    if (item.Name != "GraphicNode")
                                    {
                                        try
                                        {
                                            ShapeButton shapeButton = new(item, this);
                                            ShapesList.Children.Add(shapeButton);
                                        }
                                        catch (Exception)
                                        {
                                        }
                                    }

                    }
                }
            }
        }
        public void RemovePage(ITabPage page, ITabPageButton button)
        {
            TabPageContent.Children.Remove((IControl)page);
            TabPageButtons.Children.Remove((IControl)button);
            if (CurrentButton == button)
                if (TabPageButtons.Children.Count != 0)
                {
                    ShowPage(TabPageButtons.Children.Last() as ITabPageButton);
                }
                else
                {
                    CurrentButton = null;
                }

            System.GC.Collect();
        }
        ITabPageButton CurrentButton=null;
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

        public ITabPageButton CurrentPage()
        {
            return CurrentButton;
        }
    }
}
