using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using SRI.Editor.Core;
using SRI.Editor.Extension;
using System.Collections.Generic;
using System.IO;

namespace SRI.Editor.Main.Controls
{
    public partial class FileTreeNode : UserControl
    {
        FileSystemInfo controlledItem;
        ITabPageContainer HostedContainer;
        public FileTreeNode(ITabPageContainer Container)
        {
            HostedContainer = Container;
            InitializeComponent();
            ApplyLanguage();
        }
        public FileTreeNode()
        {
            HostedContainer = null;
            InitializeComponent();
            ApplyLanguage();
        
        }
        
        public void ApplyLanguage()
        {
            //this.FindControl<MenuItem>("Menu_New_Item").Header = Language.Find("Menu.File.New.Item");
        }
        bool isDirectory = false;
        public bool IsDirectory { get => isDirectory; }
        public void SetFileSystemInfo(FileSystemInfo item)
        {
            NameBlock.Text = item.Name;
            controlledItem = item;
            this.MinWidth = NameBlock.DesiredSize.Width + 5;

            this.FindControl<MenuItem>("Menu_Open").Click += (a, b) => { Open(); };
            if (Directory.Exists(controlledItem.FullName)) isDirectory = true;
            CentralButton.DoubleTapped += CentralButton_DoubleTapped;
            if (isDirectory == true)
            {
                this.FindControl<MenuItem>("Menu_New_Item").Click += (a, b) =>
                {
                    //MainWindow.CurrentWindow.ShowNewItemDialog(item.FullName);
                };
                var icon = IconProviders.ObtainIcon("folder.generic", Color.FromArgb(0xFF, 0xDD, 0xBB, 0x88)) as Control;
                icon.Width = 15;
                icon.Height = 15;
                this.FindControl<MenuItem>("OpenWithMenu").IsVisible = false;
                this.FindControl<Grid>("IconContainer").Children.Add(icon);
                //if (!Repository.IsValid(item.FullName))
                {

                    this.FindControl<Grid>("GitIcon").IsVisible = false;
                    this.FindControl<MenuItem>("GitMenu").IsVisible = false;
                }
                this.FindControl<MenuItem>("Delete_Item").Click += (_, _) =>
                {
                    Globals.CurrentMainWindow.ShowDialog("Are you sure?", $"This operation will delete \"{controlledItem.Name}\".",
                        new DialogButton
                        {
                            LanguageID = "Dialog.OK",
                            Fallback = "OK",
                            OnClick = () =>
                            {

                                try
                                {
                                    Directory.Delete(controlledItem.FullName, true);
                                    (this.Parent as StackPanel).Children.Remove(this);
                                    SubNodes.Children.Clear();
                                    System.GC.Collect();
                                }
                                catch (System.Exception)
                                {
                                }
                            }
                        },
                        new DialogButton
                        {
                            LanguageID = "Dialog.Cancel",
                            Fallback = "Cancel",
                        });
                };
            }
            else
            {
                {
                    //Menu...
                    //OpenWithMenu
                    var ed = EditorProvider.EnumerateEditors();
                    var menu = this.FindControl<MenuItem>("OpenWithMenu");
                    List<MenuItem> submenu = new List<MenuItem>();
                    foreach (var editor in ed)
                    {
                        var id = editor.Key;
                        MenuItem menuItem = new MenuItem();
                        menuItem.Header = editor.Value;
                        menuItem.Click += (a, b) =>
                        {
                            HostedContainer.OpenDesignatedEditor(id, new FileInfo(controlledItem.FullName));
                        };
                        submenu.Add(menuItem);
                    }
                    menu.Items = submenu;
                }
                this.FindControl<Grid>("GitIcon").IsVisible = false;
                this.FindControl<MenuItem>("GitMenu").IsVisible = false;
                var icon = IconProviders.ObtainIcon("File" + item.Extension, Color.FromArgb(0xFF, 0x33, 0xAA, 0xEE)) as Control;
                icon.Width = 15;
                icon.Height = 15;
                this.FindControl<Grid>("IconContainer").Children.Add(icon);
                this.FindControl<MenuItem>("NewMenu").IsVisible = false;
                this.FindControl<MenuItem>("Delete_Item").Click += (_, _) =>
                {
                    Globals.CurrentMainWindow.ShowDialog("Are you sure?", $"This operation will delete \"{controlledItem.Name}\".", new DialogButton
                    {
                        LanguageID = "Dialog.OK",
                        Fallback = "OK",
                        OnClick = () =>
                        {


                            try
                            {
                                controlledItem.Delete();
                                (this.Parent as StackPanel).Children.Remove(this);
                            }
                            catch (System.Exception)
                            {
                            }
                        }
                    },
                        new DialogButton
                        {
                            LanguageID = "Dialog.Cancel",
                            Fallback = "Cancel",
                        });
                };

            }

        }
        public void Open()
        {
            if (isDirectory)
            {
                //Is directory.
                OpenDir();
            }
            else
            {
                //if (controlledItem.Name.ToUpper().EndsWith(".SRI-PROJ"))
                //{
                //    (HostedContainer as MainWindow).OpenProject(controlledItem.FullName);
                //}
                //else
                    HostedContainer.OpenFileEditor(new FileInfo(controlledItem.FullName));
            }
        }
        public void BuildChildren()
        {
            SubNodes.Children.Clear();
            DirectoryInfo directoryInfo = new DirectoryInfo(controlledItem.FullName);
            foreach (var item in directoryInfo.EnumerateDirectories())
            {
                //if (Program.configuration.HideBinFolders == true)
                {
                    if (item.Name.ToUpper() == "BIN") continue;
                }
                //if (Program.configuration.HideObjFolders == true)
                {
                    if (item.Name.ToUpper() == "OBJ") continue;
                }
                //if (Program.configuration.HideDotGitFolders == true)
                {
                    if (item.Name.ToUpper() == ".GIT") continue;
                }
                var node = new FileTreeNode(HostedContainer);
                node.SetFileSystemInfo(item);
                SubNodes.Children.Add(node);
                node.CheckBox.IsVisible = CheckBox.IsVisible;
            }
            foreach (var item in directoryInfo.EnumerateFiles())
            {

                var node = new FileTreeNode(HostedContainer);
                node.SetFileSystemInfo(item);
                SubNodes.Children.Add(node);
                node.CheckBox.IsVisible = CheckBox.IsVisible;
            }
        }
        void OpenDir()
        {

            if (SubNodes.Children.Count == 0)
            {
                BuildChildren();
                //foreach (var item in directoryInfo.EnumerateFileSystemInfos())
                //{
                //    var node = new FileTreeNode();
                //    node.SetFileSystemInfo(item);
                //    SubNodes.Children.Add(node);
                //    node.CheckBox.IsVisible = CheckBox.IsVisible;
                //}
            }
            else
            {
                SubNodes.Children.Clear();
            }
        }
        private void CentralButton_DoubleTapped(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Open();
        }

        Button CentralButton;
        TextBlock NameBlock;
        public StackPanel SubNodes;
        public ToggleButton CheckBox;
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            CentralButton = this.FindControl<Button>("CentralButton");
            NameBlock = this.FindControl<TextBlock>("NameBlock");
            SubNodes = this.FindControl<StackPanel>("SubNodes");
            CheckBox = this.FindControl<ToggleButton>("CheckBox");
        }
    }
}
