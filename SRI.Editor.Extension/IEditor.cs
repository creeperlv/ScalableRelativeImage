using SRI.Editor.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRI.Editor.Extension
{
    public interface IEditor:ITabPage
    {
        void OpenFile(FileInfo file);
        void SetContent(string content);
        
    }
}
