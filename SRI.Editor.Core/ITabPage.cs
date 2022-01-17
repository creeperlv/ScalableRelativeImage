using System;

namespace SRI.Editor.Core
{
    public interface ITabPage : IDisposable
    {
        string GetTitle();
        bool TryClose();
        void Save();
        void Preview();
        void Save(string Path);
        void Insert(string Content);
        void SetButton(ITabPageButton button);
    }
    public interface ITabPageButton
    {
        ITabPage ControlledPage { get; set; }
        public ITabPageContainer ParentContainer { get; set; }
        void SetTitle(string title);
        void Show();
        void Hide();
    }
    public interface ITabPageContainer
    {
        ITabPageButton CurrentPage();
        void ShowPage(ITabPageButton button);
        void RemovePage(ITabPage page, ITabPageButton button);
        void AddPage(ITabPage page);
    }
}
