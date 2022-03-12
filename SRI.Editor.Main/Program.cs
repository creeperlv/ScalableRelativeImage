using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using SRI.Editor.Extension;
using SRI.Editor.Extension.Defaults;
using SRI.Editor.Main.Data;
using SRI.Editor.Main.Editors;
using SRI.Editor.Styles;
using SRI.Localization;
using System;

namespace SRI.Editor.Main
{
    public class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args)
        {
            new StyleLib();
            EditorConfiguration.Init();
            LanguageLib.InitLocal();
            IconProviders.RegisterProvider(new DefaultIconProvider());
            {
                EditorProvider.RegisterEditor("SRI.Editor.SRIEditor", "SRI Editor", typeof(SRIEditor),"sri");
                EditorProvider.RegisterEditor("SRI.Editor.ProjectEditor", "Project Editor", typeof(ProjectEditor),"sri-proj", "*");
                EditorProvider.RegisterEditor("SRI.Editor.ImageViewer", "Image Viewer", typeof(ImageViewer), "png", "jpg", "bmp");
                EditorProvider.RegisterEditor("SRI.Editor.BaseEditor", "Basic Editor", typeof(BaseEditor), "*");
            }
            isDesign = false;
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }
        internal static bool isDesign=true;
        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace();
    }
}
