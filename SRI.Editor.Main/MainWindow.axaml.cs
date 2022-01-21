using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using ScalableRelativeImage;
using ScalableRelativeImage.Nodes;
using SRI.Editor.Core;
using SRI.Editor.Core.Projects;
using SRI.Editor.Extension;
using SRI.Editor.Extension.Defaults;
using SRI.Editor.Main.Controls;
using SRI.Editor.Main.Editors;
using SRI.Editor.Main.Pages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SRI.Editor.Main
{
    public partial class MainWindow : Window, ITabPageContainer, IWindow
    {
        LoadedProject OpenedProject = null;
        public MainWindow()
        {
            InitializeComponent();
            Globals.CurrentMainWindow = this;
#if DEBUG
            this.AttachDevTools();
#endif
            if (Program.isDesign) return;
            InitializeWindow();
            File_New_Proj.Click += async (_, _) =>
            {


                SaveFileDialog __dialog = new SaveFileDialog();
                __dialog.Filters.Add(new FileDialogFilter() { Extensions = new List<string> { "sri-proj" }, Name = "SRI Editor Project" });
                __dialog.Filters.Add(new FileDialogFilter() { Extensions = new List<string> { "*" }, Name = "All Formats" });
                var __target_location = await __dialog.ShowAsync(this);
                if (__target_location != null)
                {
                    if (__target_location != "")
                    {
                        File.WriteAllText(__target_location, ProjectEngine.NewEmptyProject());
                        OpenProject(__target_location);
                        OpenFileEditor(new FileInfo(__target_location));
                    }
                }
            };
            File_Exit.Click += async (a, b) => { await __Close(); };
            this.Closing += async (a, b) =>
             {
                 b.Cancel = true; 
                 await __Close();
             };
            Help_About.Click += (_, _) =>
              {
                  AddPage(new AboutPage());
              };
            File_New_SRI.Click += (_, _) =>
              {
                  var __editor = new SRIEditor();
                  __editor.SetContent(ProjectEngine.NewSRIDocument());
                  AddPage(__editor);
              };
            File_New_PF.Click += (_, _) =>
              {
                  AddPage(new BaseEditor());
              };
            File_Open_File.Click += async (_, _) =>
            {
                OpenFileDialog __dialog = new();
                var file = await __dialog.ShowAsync(this);
                if (file != null)
                    foreach (var item in file)
                    {
                        OpenFileEditor(new FileInfo(item));
                    }
            };
            PreviewButton_Toolbar.Click += (_, _) =>
              {
                  if (CurrentPage() != null)
                  {
                      CurrentPage().ControlledPage.Preview();
                  }
              };
            ShapesListRefreshButton.Click += (_, _) =>
              {
                  LoadShapeList();
              };
            File_Open_Project.Click += async (_, _) =>
              {
                  await OpenProjectDialog();
              };
            Help_GH.Click += (_, _) =>
              {
                  Process.Start(new ProcessStartInfo { UseShellExecute = true, FileName = "https://github.com/creeperlv/ScalableRelativeImage" });
              };
            BuildButton_Toolbar.Click += (_, _) =>
              {

                  this.SetProgress(0, 100, 0);
                  int Total = 0;
                  var P = ProjectEngine.BuildAsync(OpenedProject,
                        (ConfigurationBox.SelectedItem as ComboBoxItem).Content as string,

                      (_0_c, _0_t) =>
                      {
                          Dispatcher.UIThread.InvokeAsync(() =>
                          {
                              this.SetProgress(_0_c);
                              this.SetProgressDescription($"Building target \"{_0_t.Name}\"...({_0_c}/{Total})");
                          });
                      },

                      (_0_c, _0_t) =>
                      {
                          Dispatcher.UIThread.InvokeAsync(() =>
                          {
                              this.SetProgress(_0_c);
                              this.SetProgressDescription($"Building target \"{_0_t.Name}\"...({_0_c}/{Total})...Done!");
                          });
                      }, () =>
                      {
                          Dispatcher.UIThread.InvokeAsync(() =>
                          {
                              this.EndProgressMask();
                          });
                      },
                      (Exception e) =>
                      {
                          Dispatcher.UIThread.InvokeAsync(() =>
                          {
                              this.EndProgressMask();
                              this.ShowDialog("Error Happened", e.Message);
                          });
                      }, (_c, _w) =>
                      {
                      }
                      );
                  Total = P.Item1.TotalCount;
                  this.ShowProgressMask("Building...", false);
                  this.SetProgress(0, P.Item1.TotalCount, P.Item1.Current);
              };
            SaveAsButton_Toolbar.Click += (_, _) =>
              {

                  SaveAs();
              };
            File_Save.Click += (_, _) =>
            {
                Save();
            };
            File_SaveAs.Click += (_, _) =>
            {
                SaveAs();
            };
            SaveButton_Toolbar.Click += (_, _) =>
            {
                Save();
            };
            ApplyLocal();
            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(3000);
                    lock (OpenFileBind)
                    {
                        foreach (var item in OpenFileBind.Keys)
                        {
                            Dispatcher.UIThread.InvokeAsync(() =>
                            {
                                item.SetTitle(item.ControlledPage.GetTitle());
                            });
                        }
                    }
                }
            });
            LoadShapeList();
            CheckAssociatedOpen();
        }

        async Task __Close()
        {
            int cannot = 0;
            int waits = 0;
            for (int i = TabPageButtons.Children.Count - 1; i >= 0; i--)
            {
                var item = TabPageButtons.Children[i];
                waits++;
                if (item is ITabPageButton button)
                {
                    if (button.Close(() =>
                    {
                        cannot--;
                        waits--;
                    }, () =>
                    {
                        waits--;
                    }) == false)
                    {
                        cannot++;
                    }
                }
            }
            while (waits > 0)
            {
                await Task.Delay(100);
            }
            if(cannot > 0)
            {

            }
            else
            {
                System.Environment.Exit(0);
            }
        }
        void Save()
        {
            if (CurrentPage() != null)
                CurrentPage().ControlledPage.Save();
        }
        void SaveAs()
        {
            {
                SaveAs(CurrentPage().ControlledPage);
            }
        }
        public void SaveAs(ITabPage page)
        {
            if (page is IEditor editor)
            {
                Task.Run(async () =>
                {

                    SaveFileDialog __dialog = new SaveFileDialog();
                    __dialog.InitialFileName = editor.GetSuggestedFileName();
                    var t = await __dialog.ShowAsync(this);
                    if (t != null)
                    {
                        await Dispatcher.UIThread.InvokeAsync(() =>
                        {
                            editor.Save(new FileInfo(t));
                        });
                    }
                });
            }
        }
        async Task OpenProjectDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.AllowMultiple = false;
            var filePick = await openFileDialog.ShowAsync(this);
            if (filePick != null)
                if (filePick.Length > 0)
                {
                    OpenProject(filePick.First());
                }
        }
        FileTreeNode RootNode = null;
        public void OpenProject(string file)
        {
            try
            {
                FileInfo Proj = new FileInfo(file);
                if (Proj.Directory != null)
                {
                    //try
                    //{
                    Trace.WriteLine("Try to laod.");
                    var __proj = ProjectEngine.Load(Proj);
                    Trace.WriteLine("Load Completed.");
                    if (__proj.CoreProject != null)
                    {
                        OpenedProject = __proj;
                        FileList.Children.Clear();
                        var __root = new FileTreeNode(this);
                        RootNode = __root;
                        __root.SetFileSystemInfo(Proj.Directory);
                        FileList.Children.Add(__root);
                        BuildButton_Toolbar.IsEnabled = true;
                        ConfigurationBox.IsEnabled = true;
                        List<ComboBoxItem> ComboBoxItems = new List<ComboBoxItem>();
                        foreach (var item in __proj.CoreProject.BuildConfigurations)
                        {
                            ComboBoxItem comboBoxItem = new ComboBoxItem();
                            comboBoxItem.Content = item.Name;
                            ComboBoxItems.Add(comboBoxItem);
                        }

                        ConfigurationBox.Items = ComboBoxItems;
                        ConfigurationBox.SelectedIndex = 0;
                    }
                    else
                    {
                        Trace.WriteLine("Open Failed.");
                    }
                    //}
                    //catch (Exception)
                    //{
                    //}
                }

            }
            catch (Exception e)
            {
                ShowDialog("Cannot load project!", $"Error reason:{e}\r\nDo you wish to open base editor of the project file?",
                    new DialogButton
                    {
                        LanguageID = "DIalog.Yes",
                        Fallback = "Yes",
                        OnClick = () =>
                        {
                            OpenDesignatedEditor("SRI.Editor.BaseEditor", new FileInfo(file));
                        }
                    }, new DialogButton { Fallback = "No", LanguageID = "Dialog.No" });
            }
        }
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            __find_all_controls();
        }
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
            if (OpenFileBind.ContainsKey(button))
            {
                OpenFileBind.Remove(button);
            }
            if (CurrentButton == button)
                if (TabPageButtons.Children.Count != 0)
                {
                    ShowPage(TabPageButtons.Children.Last() as ITabPageButton);
                }
                else
                {
                    CurrentButton = null;
                }

            GC.Collect();
        }
        ITabPageButton CurrentButton = null;
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

        public ITabPageButton AddPage(ITabPage page)
        {
            TabPageButton tabPageButton = new TabPageButton();
            TabPageButtons.Children.Add(tabPageButton);
            TabPageContent.Children.Add(page as IControl);
            tabPageButton.ParentContainer = this;
            tabPageButton.ControlledPage = page;
            page.SetButton(tabPageButton);
            ShowPage(tabPageButton);
            return tabPageButton;
        }

        public ITabPageButton CurrentPage()
        {
            return CurrentButton;
        }
        public void SetOpenFileBind(ITabPageButton button, FileInfo info)
        {
            if (OpenFileBind.ContainsKey(button))
            {
                OpenFileBind[button] = info;
            }
            else
            {
                OpenFileBind.Add(button, info);
            }
        }
        Dictionary<ITabPageButton, FileInfo> OpenFileBind = new Dictionary<ITabPageButton, FileInfo>();
        public void OpenDesignatedEditor(string ID, FileInfo fi)
        {
            bool isHit = false;
            foreach (var item in OpenFileBind)
            {
                if (item.Value.FullName == fi.FullName)
                {
                    isHit = true;
                    ShowPage(item.Key);
                }
            }
            if (isHit is false)
            {
                var ed = EditorProvider.RequestEditor(ID);
                var t = AddPage(ed);
                OpenFileBind.Add(t, fi);
                ed.OpenFile(fi);
                ShowPage(t);
            }
        }

        public void OpenFileEditor(FileInfo fi)
        {
            try
            {
                bool isHit = false;
                foreach (var item in OpenFileBind)
                {
                    if (item.Value.FullName == fi.FullName)
                    {
                        isHit = true;
                        ShowPage(item.Key);
                    }
                }
                if (isHit is false)
                {
                    var ed = EditorProvider.FindAndInitEditor(fi);
                    var t = AddPage(ed);
                    ed.OpenFile(fi);
                    OpenFileBind.Add(t, fi);
                    ShowPage(t);
                }
            }
            catch (Exception e)
            {
            }
        }

    }
}
