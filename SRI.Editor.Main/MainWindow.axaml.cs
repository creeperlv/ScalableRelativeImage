using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using ScalableRelativeImage;
using ScalableRelativeImage.Nodes;
using SRI.Editor.Core;
using SRI.Editor.Core.Projects;
using SRI.Editor.Extension;
using SRI.Editor.Extension.Defaults;
using SRI.Editor.Main.Controls;
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
    public partial class MainWindow : Window, ITabPageContainer
    {
        LoadedProject OpenedProject = null;
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            if (Program.isDesign) return;
            InitializeWindow();
            File_New_Proj.Click += async(_, _) => {


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
            File_Open_Project.Click += async (_, _) =>
            {
                await OpenProjectDialog();
            };
            BuildButton_Toolbar.Click += async (_, _) =>
              {
              };
            LoadShapeList();
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
        public void OpenProject(string file)
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
                        __root.SetFileSystemInfo(Proj.Directory);
                        FileList.Children.Add(__root);
                        BuildButton_Toolbar.IsEnabled = true;
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

        Dictionary<ITabPageButton, FileInfo> OpenFileBind = new Dictionary<ITabPageButton, FileInfo>();
        public void OpenDesignatedEditor(string ID, FileInfo fi)
        {
            bool isHit = false;
            foreach (var item in OpenFileBind)
            {
                if (item.Value.FullName == fi.FullName)
                {
                    isHit = true;
                    item.Key.Show();
                }
            }
            if (isHit is false)
            {
                var ed = EditorProvider.RequestEditor(ID);
                var t = AddPage(ed);
                OpenFileBind.Add(t, fi);
                ed.OpenFile(fi);
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
                        item.Key.Show();
                    }
                }
                if (isHit is false)
                {
                    var ed = EditorProvider.FindAndInitEditor(fi);
                    var t = AddPage(ed);
                    ed.OpenFile(fi);
                    OpenFileBind.Add(t, fi);
                }
            }
            catch (Exception e)
            {
            }
        }

    }
}
