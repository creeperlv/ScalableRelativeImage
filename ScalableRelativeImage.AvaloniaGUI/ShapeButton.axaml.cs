using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaEdit;
using ScalableRelativeImage.Nodes;
using System;

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
            INode? n = Activator.CreateInstance(Shape) as INode;
            if(n is not null && MainButton is not null)
            {
                MainButton.Content = Shape.Name;
                MainButton.Click += (_, _)=>{
                    var v=n.GetValueSet();
                    if(v is not null)
                    {
                        
                    }
                };
            }
        }
        Button? MainButton;
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            MainButton = this.FindControl<Button>("MainButton");
        }
    }
}
