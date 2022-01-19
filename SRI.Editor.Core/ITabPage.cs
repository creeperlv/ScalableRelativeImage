using System;
using System.IO;

namespace SRI.Editor.Core
{
    public interface ITabPage : IDisposable
    {
        string GetTitle();
        bool TryClose(Action NonCancelCallback, Action CancelCallback)
        {
            return true;
        }
        void Save();
        void Preview();


        void Save(FileInfo Path);
        void Insert(string Content);
        void SetButton(ITabPageButton button);
    }
}
