using CLUNL.Localization;
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
        static LocalizedString SFile_ = new LocalizedString("Menu.File", "_File");
        static LocalizedString SFile_Open = new LocalizedString("Menu.File_Open", "_Open");
        static LocalizedString SFile_Open_File = new LocalizedString("Menu.File_Open_File", "_File");
        static LocalizedString LAbout = new LocalizedString("About","About");
        static LocalizedString LMBProject = new LocalizedString("MenuBar.Project", "_Project");
        static LocalizedString LProject = new LocalizedString("Project", "Project");
        static LocalizedString LFile_Save = new LocalizedString("Menu.File_Save", "_Save");
        static LocalizedString LFile_SaveAs = new LocalizedString("Menu.File_SaveAs", "Save _As");
        static LocalizedString LExit = new LocalizedString("Menu.File_Exit", "_Exit");
        static LocalizedString LNew = new LocalizedString("Menu.File_New", "_New");
        static LocalizedString LBuild = new LocalizedString("Menu.Build", "_Build");
        static LocalizedString LHelp = new LocalizedString("Menu.Help", "_Help");
        static LocalizedString LNSRI = new LocalizedString("Menu.File_New_SRI", "_Scalable Relative Image");
        static LocalizedString LSRIEditor = new LocalizedString("SRIEditor.Title", "SRI Editor");
        public void ApplyLocalization()
        {
            File_.Header = SFile_;
            File_Open.Header = SFile_Open;
            File_Open_File.Header = SFile_Open_File;
            Help_About.Header = LAbout;
            File_Open_Project.Header = LMBProject;
            File_New_Proj.Header = LMBProject;
            Project_Block.Text = LProject;
            File_Save.Header = LFile_Save;
            File_SaveAs.Header = LFile_SaveAs;
            File_Exit.Header = LExit;
            Build_BuildProject.Header = LMBProject;
            Menu_Build.Header = LBuild;
            File_New.Header = LNew;
            File_New_SRI.Header = LNSRI;
            Menu_Help.Header = LHelp;
            TitleBlock.Text = LSRIEditor;
            foreach (var item in TabPageContent.Children)
            {
                if(item is ILocalizable l)
                {
                    l.ApplyLocalization();
                }
            }
        }
    }
}
