using System;

namespace SRI.Editor.Core
{
    public interface ITabPageButton
    {
        ITabPage ControlledPage { get; set; }
        public ITabPageContainer ParentContainer { get; set; }
        void SetTitle(string title);
        void Show();
        void Hide();
        bool Close(Action NonCancelCallback,Action CancelCallback);
        void ForceClose()
        {
            ParentContainer.RemovePage(ControlledPage, this);
        }
    }
}
