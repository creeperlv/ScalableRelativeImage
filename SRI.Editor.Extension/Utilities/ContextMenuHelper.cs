using Avalonia.Controls;
using Avalonia.Input;
using AvaloniaEdit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRI.Editor.Extension.Utilities
{
    public class EditorHelper
    {
        public static void KeyBind(TextEditor editor,IEditor Host)
        {
            editor.TextArea.PointerWheelChanged += (sender, e) => {
                if (e.KeyModifiers == KeyModifiers.Control)
                {
                    //Scale;
                    editor.FontSize += 1 * e.Delta.Y;
                    if (editor.FontSize < 1) editor.FontSize = 1;
                    e.Handled = true;
                }
            };
            //editor.PointerWheelChanged += (sender, e) => {
            //    if (e.KeyModifiers == KeyModifiers.Control)
            //    {
            //        //Scale;
            //        editor.FontSize += 1 * e.Delta.Y;
            //        if (editor.FontSize < 1) editor.FontSize = 1;
            //        e.Handled = true;
            //    }
            //};
            editor.KeyDown += (sender, e) => {

                if (e.KeyModifiers == KeyModifiers.Control)
                {
                    if (e.Key == Key.S)
                    {
                        Host.Save();
                    }
                }
            };
        }
    }
    public class ContextMenuHelper
    {
        public static void ApplyEditorContextMenu(TextEditor editor)
        {
            var list = new List<MenuItem>();
            {
                var item = new MenuItem();
                item.Header = "Copy";
                item.Click += (_, _) => { editor.Copy(); };
                list.Add(item);
            }
            {
                var item = new MenuItem();
                item.Header = "Cut";
                item.Click += (_, _) => { editor.Cut(); };
                list.Add(item);
            }
            {
                var item = new MenuItem();
                item.Header = "Paste";
                item.Click += (_, _) => { editor.Paste(); };
                list.Add(item);
            }
            {
                var item = new MenuItem();
                item.Header = "Undo"; 
                editor.TextChanged += (_, _) => { item.IsEnabled = editor.CanUndo; };
                item.Click += (_, _) => { if (editor.CanUndo) editor.Undo(); };
                list.Add(item);
            }
            {
                var item = new MenuItem();
                item.Header = "Redo";
                editor.TextChanged += (_, _) => { item.IsEnabled = editor.CanRedo; };
                item.Click += (_, _) => { if (editor.CanRedo) editor.Redo(); };
                list.Add(item);
            }
            editor.ContextMenu.Items = list;
        }
    }
}
