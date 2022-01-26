using Avalonia.Controls;
using Avalonia.Media;
using SRI.Editor.Core;
using SRI.Editor.Main.Data;
using SRI.Editor.Main.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SRI.Editor.Main
{
    public partial class MainWindow
    {

        StackPanel TabPageButtons;
        StackPanel ShapesList;
        StackPanel FileList;
        ComboBox ConfigurationBox;
        Grid TabPageContent;
        Grid Main_Root;
        Grid DialogRoot;
        Grid ProgressRoot;
        ProgressBar Progress_Bar;
        TextBlock Progress_Description;
        TextBlock TitleBlock;
        MenuItem Help_About;
        MenuItem File_New_PF;
        MenuItem File_New_Proj;
        MenuItem File_Open_Project;
        MenuItem File_Open_File;
        MenuItem File_Open;
        MenuItem File_Save;
        MenuItem File_;
        MenuItem File_SaveAs;
        MenuItem File_New_SRI;
        MenuItem File_New;
        MenuItem File_Exit;
        MenuItem Tools_Options;
        MenuItem Menu_Build;
        MenuItem Help_GH;
        MenuItem Build_BuildProject;
        MenuItem Menu_Help;
        Button BuildButton_Toolbar;
        Button PreviewButton_Toolbar;
        Button SaveButton_Toolbar;
        Button SaveAsButton_Toolbar;
        Button ShapesListRefreshButton;

        TextBlock Project_Block;

        void ApplyVisualConfiguration()
        {
            ExtendClientAreaChromeHints = Avalonia.Platform.ExtendClientAreaChromeHints.PreferSystemChrome;
            ExtendClientAreaToDecorationsHint = true;
            if (EditorConfiguration.CurrentConfiguration.isBlurEnabled)
            {
                if (EditorConfiguration.CurrentConfiguration.TransparentInsteadOfBlur)
                {
                    TransparencyLevelHint = WindowTransparencyLevel.Transparent;
                }
                else
                {
                    TransparencyLevelHint = WindowTransparencyLevel.Mica;
                    if (ActualTransparencyLevel != WindowTransparencyLevel.Mica)
                    {
                        TransparencyLevelHint = WindowTransparencyLevel.Blur;
                    }

                }
            }
            else
            {
                TransparencyLevelHint = WindowTransparencyLevel.None;
            }
            Background = new SolidColorBrush(Colors.Transparent);
        }
        void __find_all_controls()
        {

            TabPageButtons = this.FindControl<StackPanel>("TabPageButtons");
            ShapesList = this.FindControl<StackPanel>("ShapesList");
            FileList = this.FindControl<StackPanel>("FileList");
            Main_Root = this.FindControl<Grid>("Main_Root");
            TabPageContent = this.FindControl<Grid>("TabPageContent");
            DialogRoot = this.FindControl<Grid>("DialogRoot");
            ProgressRoot = this.FindControl<Grid>("ProgressRoot");
            Progress_Bar = this.FindControl<ProgressBar>("Progress_Bar");
            Progress_Description = this.FindControl<TextBlock>("Progress_Description");
            ConfigurationBox = this.FindControl<ComboBox>("ConfigurationBox");
            Help_About = this.FindControl<MenuItem>("Help_About");
            File_Open = this.FindControl<MenuItem>("File_Open");
            File_New_PF = this.FindControl<MenuItem>("File_New_PF");
            File_Open_File = this.FindControl<MenuItem>("File_Open_File");
            File_ = this.FindControl<MenuItem>("File_");
            File_New_Proj = this.FindControl<MenuItem>("File_New_Proj");
            File_New_SRI = this.FindControl<MenuItem>("File_New_SRI");
            File_Open_Project = this.FindControl<MenuItem>("File_Open_Project");
            Build_BuildProject = this.FindControl<MenuItem>("Build_BuildProject");
            Menu_Help = this.FindControl<MenuItem>("Menu_Help");
            File_Exit = this.FindControl<MenuItem>("File_Exit");
            File_Save = this.FindControl<MenuItem>("File_Save");
            Help_GH = this.FindControl<MenuItem>("Help_GH");
            File_New = this.FindControl<MenuItem>("File_New");
            Menu_Build = this.FindControl<MenuItem>("Menu_Build");
            File_SaveAs = this.FindControl<MenuItem>("File_SaveAs");
            Tools_Options = this.FindControl<MenuItem>("Tools_Options");
            ShapesListRefreshButton = this.FindControl<Button>("ShapesListRefreshButton");
            BuildButton_Toolbar = this.FindControl<Button>("BuildButton_Toolbar");
            PreviewButton_Toolbar = this.FindControl<Button>("PreviewButton_Toolbar");
            SaveButton_Toolbar = this.FindControl<Button>("SaveButton_Toolbar");
            SaveAsButton_Toolbar = this.FindControl<Button>("SaveAsButton_Toolbar");
            Project_Block = this.FindControl<TextBlock>("Project_Block");
            TitleBlock = this.FindControl<TextBlock>("TitleBlock");
        }
        public void ApplyConfiguration()
        {

            ApplyVisualConfiguration();
            ApplyLocal();
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
        public void SetProgress(int Val)
        {
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
        List<IDialog> OpenedDialogs = new List<IDialog>();
        public void CloseDialog(IDialog dialog)
        {
            OpenedDialogs.Remove(dialog);
            DialogRoot.Children.Remove(dialog as SRIDialog);
            if (OpenedDialogs.Count > 0)
            {

            }
            else
            {

                DialogRoot.IsVisible = false;
                Main_Root.IsEnabled = true;
            }
        }
        public void ShowInputDialog(string title, string hint, Action<string> OnClosePrimaryDialog)
        {
            SRIDialog dialog = new SRIDialog();
            OpenedDialogs.Add(dialog);
            DialogRoot.Children.Add(dialog);
            dialog.SetContent(title, hint);
            dialog.SetInputDialog(OnClosePrimaryDialog);
            Main_Root.IsEnabled = false;
            DialogRoot.IsVisible = true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void CheckAssociatedOpen()
        {
            var CLA = Environment.GetCommandLineArgs();
            try
            {
                for (int i = 1; i < CLA.Length; i++)
                {
                    var FILE = CLA[i];
                    if (File.Exists(FILE))
                    {
                        if (FILE.ToUpper().EndsWith(".SRI-PROJ"))
                        {
                            OpenProject(FILE);
                        }
                        else
                        {

                            OpenFileEditor(new FileInfo(FILE));
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        public void ShowDialog(string title, string content, DialogButton button0 = null, DialogButton button1 = null, DialogButton button2 = null)
        {
            SRIDialog dialog = new SRIDialog();
            OpenedDialogs.Add(dialog);
            DialogRoot.Children.Add(dialog);
            dialog.SetContent(title, content);
            dialog.SetButtons(button0, button1, button2);
            DialogRoot.IsVisible = true;
            Main_Root.IsEnabled = false;
        }
    }
}
