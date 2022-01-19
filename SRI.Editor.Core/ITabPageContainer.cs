using System.IO;

namespace SRI.Editor.Core
{
    public interface ITabPageContainer
    {
        ITabPageButton CurrentPage();
        void ShowPage(ITabPageButton button);
        void RemovePage(ITabPage page, ITabPageButton button);
        ITabPageButton AddPage(ITabPage page);
        void OpenDesignatedEditor(string id, FileInfo fileInfo);
        void OpenFileEditor(FileInfo fileInfo);
        void SaveAs(ITabPage editor);
        void SetOpenFileBind(ITabPageButton button, FileInfo fileInfo);
    }
}
