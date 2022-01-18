using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaEdit;
using AvaloniaEdit.Highlighting;
using AvaloniaEdit.Highlighting.Xshd;
using SRI.Editor.Core;
using System.IO;
using System.Xml;

namespace SRI.Editor.Main.Pages
{
    public partial class BaseEditor : Grid,ITabPage
    {
        public BaseEditor()
        {
            InitializeComponent();

            XshdSyntaxDefinition xshd;
            FileInfo fi = new(typeof(Program).Assembly.Location);
            //new XmlReader()
            using (XmlReader reader = XmlReader.Create(System.IO.Path.Combine(fi.Directory.FullName, "Resources/Theme.xml")))
            {
                xshd = HighlightingLoader.LoadXshd(reader);
                CentralEditor.SyntaxHighlighting = HighlightingLoader.Load(xshd, HighlightingManager.Instance);
            }
        }
        string OpenedFile = null;
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
        public void Open(string Path)
        {
            CentralEditor.Text=File.ReadAllText(OpenedFile);
        }
        public void Save()
        {
            if (OpenedFile != null)
            {
                File.WriteAllText(OpenedFile, CentralEditor.Text);
            }
        }

        public void Save(string Path)
        {
            OpenedFile = Path;
        }

        public void SetButton(ITabPageButton button)
        {

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
    }
}
