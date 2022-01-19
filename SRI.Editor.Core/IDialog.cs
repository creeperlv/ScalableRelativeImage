using System;

namespace SRI.Editor.Core
{
    public interface IDialog
    {
        void SetContent(string Header, string Content);
        void SetInputDialog(Action<string> AfterInput);
        void SetButtons(DialogButton button0 = null, DialogButton button1 = null, DialogButton button2 = null);
    }
}
