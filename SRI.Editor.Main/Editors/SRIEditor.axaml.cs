using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaEdit;
using AvaloniaEdit.Highlighting;
using AvaloniaEdit.Highlighting.Xshd;
using CLUNL.Localization;
using ScalableRelativeImage;
using SRI.Editor.Core;
using SRI.Editor.Core.Utilities;
using SRI.Editor.Extension;
using SRI.Editor.Extension.Utilities;
using SRI.Editor.Styles;
using SRI.Localization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace SRI.Editor.Main.Editors
{
    public partial class SRIEditor : Grid, ITabPage, IEditor,ILocalizable
    {
        public SRIEditor()
        {
            InitializeComponent();

            Init00();

            XshdSyntaxDefinition xshd;
            FileInfo fi = new(typeof(StyleLib).Assembly.Location);
            //new XmlReader()
            using (XmlReader reader = XmlReader.Create(System.IO.Path.Combine(fi.Directory.FullName, "Resources/Theme.xml")))
            {
                xshd = HighlightingLoader.LoadXshd(reader);
                CentralEditor.SyntaxHighlighting = HighlightingLoader.Load(xshd, HighlightingManager.Instance);
            }
            CentralEditor.TextChanged += CentralEditor_TextChanged;
            CentralEditor.ContextMenu = new ContextMenu();
            ContextMenuHelper.ApplyEditorContextMenu(CentralEditor);
            RowButtonClick();
            EditorHelper.KeyBind(CentralEditor, this);
            ApplyLocalization();
        }
        void Init00()
        {
            {
                ViewPortZoomBox.KeyDown += (a, b) =>
                {
                    if (b.Key == Avalonia.Input.Key.Enter) ApplyZoomBox();
                };
            }
            ExportButton.Click += async (_, _) =>
            {

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filters.Add(new FileDialogFilter() { Extensions = new List<string> { "png" }, Name = "Portable Network Graphics" });
                sfd.Filters.Add(new FileDialogFilter() { Extensions = new List<string> { "jpg", "jpeg" }, Name = "Joint Photographic Experts Group" });
                sfd.Filters.Add(new FileDialogFilter() { Extensions = new List<string> { "bmp" }, Name = "Bitmap Image" });
                sfd.Filters.Add(new FileDialogFilter() { Extensions = new List<string> { "*" }, Name = "All Formats" });
                var result = await sfd.ShowAsync(Globals.CurrentMainWindow as MainWindow);
                if (result is not null)
                {
                    var VI = Compile();
                    {

                        RenderProfile profile = new RenderProfile();
                        try
                        {
                            profile.TargetWidth = float.Parse(WidthBox.Text);

                        }
                        catch (System.Exception)
                        {
                            profile.TargetWidth = VI.RelativeWidth;
                        }
                        try
                        {
                            profile.TargetHeight = float.Parse(HeightBox.Text);
                        }
                        catch (System.Exception)
                        {
                            profile.TargetHeight = VI.RelativeHeight;
                        }
                        var img = VI.Render(profile);
                        img.Save(result);
                    }
                }
            };
            {
                ViewPortZoomIn.Click += (_, _) =>
                {
                    {
                        float v = float.Parse(ViewPortZoomBox.Text);
                        v += 10;
                        ViewPortZoomBox.Text = "" + v;
                        ApplyZoomBox();
                    }
                };
            }
            {
                ViewPortZoomOut.Click += (_, _) =>
                {
                    {
                        float v = float.Parse(ViewPortZoomBox.Text);
                        if (v - 10 > 0)
                            v -= 10;
                        ViewPortZoomBox.Text = "" + v;
                        ApplyZoomBox();
                    }
                };
            }
            {
                ViewPortZoomApply.Click += (_, _) =>
                {
                    ApplyZoomBox();
                };
            }
            {
                ColumnButton.Click += (_, _) =>
                {
                    ColumnButtonClick(); ReArrange();
                };
                RowButton.Click += (_, _) =>
                {
                    RowButtonClick(); ReArrange();
                };
            }

            RefreshButton.Click += RefreshButton_Click;
            {
                CodeView.Checked += (_, _) =>
                {
                    EditorGrid.IsVisible = true; ReArrange();
                };
                CodeView.Unchecked += (_, _) =>
                {
                    EditorGrid.IsVisible = false; ReArrange();
                };
                ImageView.Checked += (_, _) =>
                {
                    ViewerGrid.IsVisible = true; ReArrange();
                };
                ImageView.Unchecked += (_, _) =>
                {
                    ViewerGrid.IsVisible = false; ReArrange();
                };
            }
        }
        string OriginalHash = null;
        bool isChanged = false;
        private void CentralEditor_TextChanged(object sender, System.EventArgs e)
        {
            isChanged = true;
        }

        FileInfo OpenedFile = null;
        public void Dispose()
        {
        }
        string ObtainFileTitle()
        {
            if (OpenedFile != null)
            {
                return OpenedFile.Name;
            }
            return "New File";
        }
        public string GetTitle()
        {
            if (IsVaried())
                return "*" + ObtainFileTitle();
            return ObtainFileTitle();
        }
        bool IsVaried()
        {

            if (OriginalHash != null)
            {
                if (isChanged)
                {
                    var __hash = HashTool.HashString(CentralEditor.Text);
                    if (OriginalHash != __hash)
                    {
                        return true;
                    }
                    else
                    {
                    }
                }
            }
            return false;
        }

        public void Insert(string Content)
        {
            CentralEditor.Text = CentralEditor.Text.Insert(CentralEditor.SelectionStart, Content);

        }

        public void Preview()
        {
            RefreshButton_Click(null, null);
        }
        public void OpenFile(FileInfo Path)
        {
            OpenedFile = Path;
            if (button != null)
                button.SetTitle(Path.Name);
            CentralEditor.Text = File.ReadAllText(Path.FullName);
            CentralEditor.Document.UndoStack.ClearAll();
            OriginalHash = HashTool.HashString(CentralEditor.Text);
            isChanged = false;
        }
        public void Save()
        {
            if (OpenedFile != null)
            {
                File.WriteAllText(OpenedFile.FullName, CentralEditor.Text);
                isChanged = false;
                OriginalHash = HashTool.HashString(CentralEditor.Text);
            }
            else
            {
                button.ParentContainer.SaveAs(this);
            }
        }

        public void Save(FileInfo Path)
        {
            OpenedFile = Path;
            File.WriteAllText(OpenedFile.FullName, CentralEditor.Text);
            button.ParentContainer.SetOpenFileBind(button, Path);
            isChanged = false;
            OriginalHash = HashTool.HashString(CentralEditor.Text);
        }
        ITabPageButton button;
        public void SetButton(ITabPageButton button)
        {
            this.button = button;
        }

        public bool TryClose(Action NonCancelCallback, Action CancelCallback)
        {
            if (IsVaried())
            {
                Globals.CurrentMainWindow.ShowDialog("Unsaved content!", "Do you wish to save file first?",
                    new DialogButton()
                    {
                        LanguageID = "Dialog.OK",
                        Fallback = "OK",
                        OnClick = () =>
                        {
                            Save();
                            button.ForceClose();
                            if(NonCancelCallback != null)NonCancelCallback();
                        }
                    }, new DialogButton()
                    {
                        LanguageID = "Dialog.No",
                        Fallback = "No",
                        OnClick = () =>
                        {
                            button.ForceClose();
                            if (NonCancelCallback != null) NonCancelCallback();
                        }
                    }, new DialogButton()
                    {

                        LanguageID = "Dialog.Cancel",
                        Fallback = "Cancel",
                        OnClick = () => { if(CancelCallback!=null)CancelCallback(); }
                    });
                return false;
            }
            return true;
        }
        TextEditor CentralEditor;
        TextBlock ApplyDesignSymbolB;
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            CentralEditor = this.FindControl<TextEditor>("CentralEditor");
            ApplyDesignSymbolB = this.FindControl<TextBlock>("ApplyDesignSymbolB");
            InitControls();
        }

        public void SetContent(string content)
        {
            CentralEditor.Text = content;
        }
        static LocalizedString LSri = new LocalizedString("SRI.Title", "Scalable Relative Image");
        public List<FileDialogFilter> ObtainExtensionFilters()
        {
            return new List<FileDialogFilter> { new FileDialogFilter() { Extensions = new List<string> { "sri" }, Name = LSri } };
        }
        static LocalizedString LApplyDesignSymbol = new LocalizedString("ApplyDesignSymbol", "ApplyDesignSymbol");
        public void ApplyLocalization()
        {
            ApplyDesignSymbolB.Text = LApplyDesignSymbol;
        }
    }
}
