using SRI.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRI.Editor.Main
{
    public partial class MainWindow : ILocalizable
    {
        LocalizedString SFile_ = new LocalizedString("Menu.File", "_File");
        LocalizedString SFile_Open = new LocalizedString("Menu.File_Open", "_Open");
        LocalizedString SFile_Open_File = new LocalizedString("Menu.File_Open_File", "_File");
        LocalizedString LAbout = new LocalizedString("About","About");
        public void ApplyLocal()
        {
            File_.Header = SFile_.ToString();
            File_Open.Header = SFile_Open.ToString();
            File_Open_File.Header = SFile_Open_File.ToString();
            Help_About.Header = LAbout.ToString();
        }
    }
}
