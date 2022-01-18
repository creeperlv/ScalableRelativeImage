using Avalonia.Controls;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRI.Editor.Main
{
    public partial class MainWindow
    {

        StackPanel TabPageButtons;
        StackPanel ShapesList;
        StackPanel FileList;
        Grid TabPageContent;
        Grid Main_Root;
        Grid ProgressRoot;
        ProgressBar Progress_Bar;
        TextBlock Progress_Description;
        MenuItem Help_About;
        MenuItem File_New_PF;
        MenuItem File_New_Proj;
        MenuItem File_Open_Project;
        Button BuildButton_Toolbar;
        Button ShapesListRefreshButton;

        void InitializeWindow()
        {
            ExtendClientAreaChromeHints = Avalonia.Platform.ExtendClientAreaChromeHints.PreferSystemChrome;
            ExtendClientAreaToDecorationsHint = true;
            TransparencyLevelHint = WindowTransparencyLevel.Blur;
            Background = new SolidColorBrush(Colors.Transparent);
        }
        void __find_all_controls()
        {

            TabPageButtons = this.FindControl<StackPanel>("TabPageButtons");
            ShapesList = this.FindControl<StackPanel>("ShapesList");
            FileList = this.FindControl<StackPanel>("FileList");
            Main_Root = this.FindControl<Grid>("Main_Root");
            TabPageContent = this.FindControl<Grid>("TabPageContent");
            ProgressRoot = this.FindControl<Grid>("ProgressRoot");
            Progress_Bar = this.FindControl<ProgressBar>("Progress_Bar");
            Progress_Description = this.FindControl<TextBlock>("Progress_Description");
            Help_About = this.FindControl<MenuItem>("Help_About");
            File_New_PF = this.FindControl<MenuItem>("File_New_PF");
            File_New_Proj = this.FindControl<MenuItem>("File_New_Proj");
            File_Open_Project = this.FindControl<MenuItem>("File_Open_Project");
            ShapesListRefreshButton = this.FindControl<Button>("ShapesListRefreshButton");
            BuildButton_Toolbar = this.FindControl<Button>("BuildButton_Toolbar");
        }
        public void ShowProgressMask(string description, bool IsIndeterminate)
        {
            Main_Root.IsEnabled = false;
            ProgressRoot.IsVisible = true;
            Progress_Bar.IsIndeterminate = IsIndeterminate;
            Progress_Description.Text = description;
        }
        public void SetProgress(int Min, int Max, int Val)
        {
            Progress_Bar.Maximum = Max;
            Progress_Bar.Minimum = Min;
            Progress_Bar.Value = Val;
        }
        public void SetProgressDescription(string description)
        {
            Progress_Description.Text = description;
        }
        public void EndProgressMask()
        {
            Main_Root.IsEnabled = true;
            ProgressRoot.IsVisible = false;
        }
    }
}
