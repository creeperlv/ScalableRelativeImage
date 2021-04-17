using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using AvaloniaEdit;
using ScalableRelativeImage.Nodes;
using System;
using System.Xml;

namespace ScalableRelativeImage.AvaloniaGUI
{
    public class ShapeButton : UserControl
    {
        TextEditor? MainEditor;
        public ShapeButton()
        {
            InitializeComponent();
        }
        public ShapeButton(Type Shape, TextEditor editor)
        {
            MainEditor = editor;
            InitializeComponent();
            try
            {
                INode? n = Activator.CreateInstance(Shape) as INode;
                if (n is not null && MainButton is not null)
                {
                    this.FindControl<TextBlock>("MainText").Text = Shape.Name;
                    this.FindControl<TextBlock>("SubText").Text = Shape.Assembly.GetName().Name;
                    MainButton.Click += (_, _) => {
                        var v = n.GetValueSet();
                        if (v is not null)
                        {
                            string xml = "";
                            var _node = MainWindow.GlobalXmlDocument.CreateNode(XmlNodeType.Element, Shape.Name, null);
                            var node = n;
                            if (_node.Attributes is not null)
                                foreach (var item in node.GetValueSet())
                                {
                                    _node.Attributes.Append(CreateAttribute(ref MainWindow.GlobalXmlDocument, item.Key, item.Value));
                                }
                            xml = _node.OuterXml;
                            editor.Text = editor.Text.Insert(editor.SelectionStart, xml);
                        }
                    };
                }
            }
            catch (Exception)
            {
                this.FindControl<TextBlock>("MainText").Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                this.FindControl<TextBlock>("MainText").Text = Shape.Name+" (?)";
            }
        }
        static XmlAttribute CreateAttribute(ref XmlDocument xmlDocument, string Name, string Value)
        {
            var attr = xmlDocument.CreateAttribute(Name);
            attr.Value = Value;
            return attr;
        }
        Button? MainButton;
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            MainButton = this.FindControl<Button>("MainButton");
        }
    }
}
