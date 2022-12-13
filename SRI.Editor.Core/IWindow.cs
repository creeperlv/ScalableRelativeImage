using System;

namespace SRI.Editor.Core
{
    public interface IWindow
    {
        void CloseDialog(IDialog dialog);
        void ShowInputDialog(string title, string hint, Action<string> OnClosePrimaryDialog);
        void ShowInputDialog(string title, string hint,string preinput, Action<string> OnClosePrimaryDialog);
        void ShowDialog(string title, string hint, DialogButton button0=null, DialogButton button1 = null, DialogButton button2 = null);
    }
}
