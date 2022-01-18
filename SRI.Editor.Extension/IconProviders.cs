using Avalonia.Controls;
using Avalonia.Media;
using SRI.Editor.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRI.Editor.Extension
{
    public static class IconProviders
    {
        static List<IIconProvider> Providers = new List<IIconProvider>();
        public static void RegisterProvider(IIconProvider p) { Providers.Add(p); }
        public static IControl ObtainIcon(string ID, Color foreground)
        {
            Viewbox viewbox = new Viewbox();
            foreach (var item in Providers)
            {
                var v = item.ObtainIcon(ID, foreground);
                if (v is not null) return v;
            }
            return viewbox;
        }
    }
}
