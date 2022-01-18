using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaEdit;
using AvaloniaEdit.Highlighting;
using AvaloniaEdit.Highlighting.Xshd;
using SRI.Editor.Core;
using SRI.Editor.Styles;
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
        }
        FileInfo OpenedFile = null;
        public void Dispose()
        {
        }

        public string GetTitle()
        {
            return "BaseEditor";
        }

        public void Insert(string Content)
        {
        }

        public void Preview()
        {
        }
        public void OpenFile(FileInfo Path)
        {
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
        }
        public void Save()
        {
            if (OpenedFile != null)
            {
                File.WriteAllText(OpenedFile.FullName, CentralEditor.Text);
            }
        }

        public void Save(FileInfo Path)
        {
            OpenedFile = Path;
        }
        ITabPageButton button;
        public void SetButton(ITabPageButton button)
        {
            this.button = button;
        }

        public bool TryClose()
        {
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
