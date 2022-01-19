using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaEdit;
using AvaloniaEdit.Highlighting;
using AvaloniaEdit.Highlighting.Xshd;
using SRI.Editor.Core;
using SRI.Editor.Core.Utilities;
using SRI.Editor.Extension.Utilities;
using SRI.Editor.Styles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace SRI.Editor.Extension.Defaults
{
    public partial class BaseEditor : Grid, ITabPage, IEditor
    {
        public BaseEditor()
        {
            InitializeComponent();
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
            EditorHelper.KeyBind(CentralEditor, this);
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
        }
        public void OpenFile(FileInfo Path)
        {
            OpenedFile = Path;
            if (button != null)
                button.SetTitle(Path.Name);
            var __Name = Path.Name.ToUpper();
            if (__Name.EndsWith(".MD"))
            {
                var DEF = HighlightingManager.Instance.GetDefinition("Markdown");
                if (DEF != null)
                    CentralEditor.SyntaxHighlighting = DEF;
            }
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
                            if (NonCancelCallback != null) NonCancelCallback();
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
                        OnClick = () => { if (CancelCallback != null) CancelCallback(); }
                    });
                return false;
            }
            return true;
        }
        TextEditor CentralEditor;
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            CentralEditor = this.FindControl<TextEditor>("CentralEditor");

        }

        public void SetContent(string content)
        {
            CentralEditor.Text = content;
        }
    }
}
