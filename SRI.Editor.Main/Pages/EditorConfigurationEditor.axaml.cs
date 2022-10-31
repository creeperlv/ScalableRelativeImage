using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CLUNL.Imaging.GPUAcceleration;
using CLUNL.Localization;
using OpenCL.NetCore;
using SRI.Editor.Core;
using SRI.Editor.Extension;
using SRI.Editor.Main.Data;
using System.Collections.Generic;
using System.IO;

namespace SRI.Editor.Main.Pages
{
    public partial class EditorConfigurationEditor : Grid, IEditor
    {
        static List<string> GPU = new List<string>();
        public EditorConfigurationEditor()
        {
            InitializeComponent();
            LoadFromSettings();
            InitEvents();
        }
        void InitEvents()
        {
            SaveButton.Click += (_, _) => { Save(); };
            LocalizationButton.Click += (_, _) => { HideAllPages(); LocalizationPanel.IsVisible = true; };
            VisualButton.Click += (_, _) => { HideAllPages(); VisualPanel.IsVisible = true; };
            AccelerationButton.Click += (_, _) => { HideAllPages(); AccelerationPanel.IsVisible = true; };
        }
        void HideAllPages()
        {
            LocalizationPanel.IsVisible = false;
            VisualPanel.IsVisible = false;
            AccelerationPanel.IsVisible = false;
        }
        public void LoadFromSettings()
        {
            UseBlurSwitch.IsChecked = EditorConfiguration.CurrentConfiguration.isBlurEnabled;
            UseTransparentSwitch.IsChecked = EditorConfiguration.CurrentConfiguration.TransparentInsteadOfBlur;
            {
                var CODES = Language.EnumerateLanguageCodes();
                List<ComboBoxItem> Items = new List<ComboBoxItem>();
                var USING = Language.ObtainLanguageInUse();

                int i = 0;
                int TARGET = 0;
                foreach (var item in CODES)
                {
                    if (item == USING)
                    {
                        TARGET = i;
                    };
                    Items.Add(new ComboBoxItem { Content = item });
                    i++;
                }
                LanguageBox.Items = Items;
                LanguageBox.SelectedIndex = TARGET;
            }
            if (GPU.Count == 0)
            {
                try
                {
                    var gpus = CommonGPUAcceleration.EnumerateGPUs();
                    GPU.Add("Software");
                    foreach (var item in gpus)
                    {
                        ErrorCode ec;
                        string Name = Cl.GetDeviceInfo(item, DeviceInfo.Name, out ec)
                            + "," +
                            Cl.GetPlatformInfo(Cl.GetDeviceInfo(item, DeviceInfo.Platform, out ec).CastTo<Platform>(), PlatformInfo.Name, out ec) +
                             "," +
                            Cl.GetPlatformInfo(Cl.GetDeviceInfo(item, DeviceInfo.Platform, out ec).CastTo<Platform>(), PlatformInfo.Version, out ec);
                        GPU.Add(Name);
                    }
                }
                catch (System.Exception)
                {

                }
            }
            CLUNLAP.Items = GPU;
            CLUNLAP.SelectedIndex = EditorConfiguration.CurrentConfiguration.ComputeMode;
            foreach (var item in GPU)
            {
                
            }
        }
        public void Dispose()
        {

        }

        public string GetTitle()
        {
            return "Editor Configuration";
        }

        public void Insert(string Content)
        {
        }

        public List<FileDialogFilter> ObtainExtensionFilters()
        {
            return new List<FileDialogFilter>();
        }

        public void OpenFile(FileInfo file)
        {
        }

        public void Preview()
        {
        }

        public void Save()
        {
            EditorConfiguration.CurrentConfiguration.isBlurEnabled = UseBlurSwitch.IsChecked.Value;
            EditorConfiguration.CurrentConfiguration.ComputeMode = CLUNLAP.SelectedIndex;
            EditorConfiguration.CurrentConfiguration.TransparentInsteadOfBlur = UseTransparentSwitch.IsChecked.Value;
            var CODE = (LanguageBox.SelectedItem as ComboBoxItem).Content as string;
            Language.SetLanguageCode(CODE);
            EditorConfiguration.Save();
            (Globals.CurrentMainWindow as MainWindow).ApplyConfiguration();
        }

        public void Save(FileInfo Path)
        {
        }

        public void SetButton(ITabPageButton button)
        {
        }

        public void SetContent(string content)
        {
        }
        ToggleSwitch UseBlurSwitch;
        ToggleSwitch UseTransparentSwitch;
        ComboBox LanguageBox;
        ComboBox CLUNLAP;
        Button VisualButton;
        Button AccelerationButton;
        Button LocalizationButton;
        Button SaveButton;
        StackPanel VisualPanel;
        StackPanel LocalizationPanel;
        StackPanel AccelerationPanel;
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            LanguageBox = this.FindControl<ComboBox>("LanguageBox");
            CLUNLAP = this.FindControl<ComboBox>("CLUNLAP");
            UseBlurSwitch = this.FindControl<ToggleSwitch>("UseBlurSwitch");
            UseTransparentSwitch = this.FindControl<ToggleSwitch>("UseTransparentSwitch");
            VisualButton = this.FindControl<Button>("VisualButton");
            AccelerationButton = this.FindControl<Button>("AccelerationButton");
            SaveButton = this.FindControl<Button>("SaveButton");
            LocalizationButton = this.FindControl<Button>("LocalizationButton");
            VisualPanel = this.FindControl<StackPanel>("VisualPanel");
            LocalizationPanel = this.FindControl<StackPanel>("LocalizationPanel");
            AccelerationPanel = this.FindControl<StackPanel>("AccelerationPanel");
        }
    }
}
