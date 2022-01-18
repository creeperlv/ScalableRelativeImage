using System;
using Avalonia.Controls;
using Avalonia.Media;

namespace SRI.Editor.Extension
{
    public interface IIconProvider
    {
        IControl ObtainIcon(string ID, Color Foreground);
    }
}
