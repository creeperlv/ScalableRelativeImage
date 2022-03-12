using Avalonia.Controls;
using Avalonia.Media;
using SRI.Editor.Extension;

namespace SRI.Editor.Main
{
    /// <summary>
    /// Note: Non-Textblock-Based icons are from https://materialdesignicons.com.
    /// </summary>
    public class DefaultIconProvider : IIconProvider
    {
        private const string FILE_CPP = "FILE.CPP";
        private const string FILE_CXX = "FILE.CXX";
        private const string FILE_LW_PROJ = "FILE.LW-PROJ";
        private const string FILE_LW_PROJ_COLLECTION = "FILE.LW-PROJ-COLLECTION";
        private const string FILE_H = "FILE.H";
        private const string FILE_HPP = "FILE.HPP";
        private const string FILE_CS = "FILE.CS";
        private const string FILE_JAVA = "FILE.JAVA";
        private const string FILE_INI = "FILE.INI";
        private const string FILE_XML = "FILE.XML";
        private const string FILE_ZIP = "FILE.ZIP";
        private const string FILE_XAML = "FILE.XAML";
        private const string FILE_AXAML = "FILE.AXAML";
        private const string FILE_JSON = "FILE.JSON";
        private const string FILE_PS1 = "FILE.PS1";
        private const string FILE_C = "FILE.C";
        private const string FILE_7Z = "FILE.7Z";
        private const string FILE_SRI = "FILE.SRI";

        public IControl ObtainIcon(string ID, Color Foreground)
        {
            Viewbox viewbox = new Viewbox();
            Grid grid = new Grid();
            grid.Width = 24;
            grid.Height = 24;
            Canvas canvas = new Canvas();
            canvas.Width = 24;
            canvas.Height = 24;
            canvas.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch;
            canvas.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch;
            viewbox.Child = grid;
            grid.Children.Add(canvas);
            ID = ID.ToUpper();
            if (ID.StartsWith("FOLDER"))
            {
                var p = new Avalonia.Controls.Shapes.Path();
                p.Fill = new SolidColorBrush(Foreground);
                canvas.Children.Add(p);
                p.Data = StreamGeometry.Parse("M20,18H4V8H20M20,6H12L10,4H4C2.89,4 2,4.89 2,6V18A2,2 0 0,0 4,20H20A2,2 0 0,0 22,18V8C22,6.89 21.1,6 20,6Z");
                return viewbox;
            }
            else if (ID.StartsWith("FILE"))
            {
                switch (ID)
                {
                    case FILE_CPP:
                    case FILE_CXX:
                        {
                            TextBlock textBlock = new TextBlock();
                            textBlock.Text = "C ";
                            textBlock.Foreground = new SolidColorBrush(Color.FromRgb(0xEE, 0x33, 0xFF));
                            textBlock.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center;
                            textBlock.FontWeight = FontWeight.Black;
                            textBlock.FontSize = 22;
                            textBlock.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center;
                            grid.Children.Add(textBlock);
                            TextBlock textBlock2 = new TextBlock();
                            textBlock2.Text = " ++";
                            textBlock2.Foreground = new SolidColorBrush(Color.FromRgb(0xEE, 0x33, 0xFF));
                            textBlock2.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center;
                            textBlock2.FontWeight = FontWeight.Black;
                            textBlock2.FontSize = 18;
                            textBlock2.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center;
                            grid.Children.Add(textBlock2);
                        }
                        break;
                    case FILE_LW_PROJ:
                    case FILE_LW_PROJ_COLLECTION:
                    case FILE_INI:
                        {
                            var p = new Avalonia.Controls.Shapes.Path();
                            p.Fill = new SolidColorBrush(Foreground);
                            canvas.Children.Add(p);
                            p.Data = StreamGeometry.Parse("M12,15.5A3.5,3.5 0 0,1 8.5,12A3.5,3.5 0 0,1 12,8.5A3.5,3.5 0 0,1 15.5,12A3.5,3.5 0 0,1 12,15.5M19.43,12.97C19.47,12.65 19.5,12.33 19.5,12C19.5,11.67 19.47,11.34 19.43,11L21.54,9.37C21.73,9.22 21.78,8.95 21.66,8.73L19.66,5.27C19.54,5.05 19.27,4.96 19.05,5.05L16.56,6.05C16.04,5.66 15.5,5.32 14.87,5.07L14.5,2.42C14.46,2.18 14.25,2 14,2H10C9.75,2 9.54,2.18 9.5,2.42L9.13,5.07C8.5,5.32 7.96,5.66 7.44,6.05L4.95,5.05C4.73,4.96 4.46,5.05 4.34,5.27L2.34,8.73C2.21,8.95 2.27,9.22 2.46,9.37L4.57,11C4.53,11.34 4.5,11.67 4.5,12C4.5,12.33 4.53,12.65 4.57,12.97L2.46,14.63C2.27,14.78 2.21,15.05 2.34,15.27L4.34,18.73C4.46,18.95 4.73,19.03 4.95,18.95L7.44,17.94C7.96,18.34 8.5,18.68 9.13,18.93L9.5,21.58C9.54,21.82 9.75,22 10,22H14C14.25,22 14.46,21.82 14.5,21.58L14.87,18.93C15.5,18.67 16.04,18.34 16.56,17.94L19.05,18.95C19.27,19.03 19.54,18.95 19.66,18.73L21.66,15.27C21.78,15.05 21.73,14.78 21.54,14.63L19.43,12.97Z");

                        }
                        break;
                    case FILE_H:
                    case FILE_HPP:
                        {
                            TextBlock textBlock = new TextBlock();
                            textBlock.FontSize = 18;
                            textBlock.Text = "H";
                            textBlock.Foreground = new SolidColorBrush(Color.FromRgb(0x22, 0x88, 0xEE));
                            textBlock.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center;
                            textBlock.FontWeight = FontWeight.Black;
                            textBlock.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center;
                            grid.Children.Add(textBlock);
                        }
                        break;
                    case FILE_CS:
                        {
                            TextBlock textBlock = new TextBlock();
                            textBlock.FontSize = 18;
                            textBlock.Text = "C#";
                            textBlock.Foreground = new SolidColorBrush(Color.FromRgb(0x22, 0xEE, 0x88));
                            textBlock.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center;
                            textBlock.FontWeight = FontWeight.Black;
                            textBlock.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center;
                            grid.Children.Add(textBlock);
                        }
                        break;
                    case FILE_JAVA:
                        {
                            var p = new Avalonia.Controls.Shapes.Path();
                            p.Fill = new SolidColorBrush(Foreground);
                            canvas.Children.Add(p);
                            p.Data = StreamGeometry.Parse("M16.5,6.08C16.5,6.08 9.66,7.79 12.94,11.56C13.91,12.67 12.69,13.67 12.69,13.67C12.69,13.67 15.14,12.42 14,10.82C12.94,9.35 12.14,8.62 16.5,6.08M12.03,7.28C16.08,4.08 14,2 14,2C14.84,5.3 11.04,6.3 9.67,8.36C8.73,9.76 10.13,11.27 12,13C11.29,11.3 8.78,9.84 12.03,7.28M9.37,17.47C6.29,18.33 11.25,20.1 15.16,18.43C14.78,18.28 14.41,18.1 14.06,17.89C12.7,18.2 11.3,18.26 9.92,18.07C8.61,17.91 9.37,17.47 9.37,17.47M14.69,15.79C12.94,16.17 11.13,16.26 9.35,16.05C8.04,15.92 8.9,15.28 8.9,15.28C5.5,16.41 10.78,17.68 15.5,16.3C15.21,16.19 14.93,16 14.69,15.79M18.11,19.09C18.11,19.09 18.68,19.56 17.5,19.92C15.22,20.6 8.07,20.81 6.09,19.95C5.38,19.64 6.72,19.21 7.14,19.12C7.37,19.06 7.6,19.04 7.83,19.04C7.04,18.5 2.7,20.14 5.64,20.6C13.61,21.9 20.18,20 18.11,19.09M15.37,14.23C15.66,14.04 15.97,13.88 16.29,13.74C16.29,13.74 14.78,14 13.27,14.14C11.67,14.3 10.06,14.32 8.46,14.2C6.11,13.89 9.75,13 9.75,13C8.65,13 7.57,13.26 6.59,13.75C4.54,14.75 11.69,15.2 15.37,14.23M16.27,16.65C16.25,16.69 16.23,16.72 16.19,16.75C21.2,15.44 19.36,12.11 16.96,12.94C16.83,13 16.72,13.08 16.65,13.19C16.79,13.14 16.93,13.1 17.08,13.07C18.28,12.83 20,14.7 16.27,16.65M16.4,21.26C13.39,21.78 10.31,21.82 7.28,21.4C7.28,21.4 7.74,21.78 10.09,21.93C13.69,22.16 19.22,21.8 19.35,20.1C19.38,20.11 19.12,20.75 16.4,21.26Z");
                        }
                        break;
                    case FILE_XML:
                        {
                            var p = new Avalonia.Controls.Shapes.Path();
                            p.Fill = new SolidColorBrush(Foreground);
                            canvas.Children.Add(p);
                            p.Data = StreamGeometry.Parse("M12.89,3L14.85,3.4L11.11,21L9.15,20.6L12.89,3M19.59,12L16,8.41V5.58L22.42,12L16,18.41V15.58L19.59,12M1.58,12L8,5.58V8.41L4.41,12L8,15.58V18.41L1.58,12Z");
                        }
                        break;
                    case FILE_ZIP:
                        {

                            //var p = new Avalonia.Controls.Shapes.Path();
                            //p.Fill = new SolidColorBrush(Foreground);
                            //canvas.Children.Add(p);
                            //p.Data = StreamGeometry.Parse("M20 6H12L10 4H4C2.9 4 2 4.9 2 6V18C2 19.1 2.9 20 4 20H20C21.1 20 22 19.1 22 18V8C22 6.9 21.1 6 20 6M20 18H16V16H14V18H4V8H14V10H16V8H20V18M16 12V10H18V12H16M14 12H16V14H14V12M18 16H16V14H18V16Z");

                            Border b = new Border();
                            TextBlock textBlock = new TextBlock();
                            textBlock.FontSize = 12;
                            textBlock.Text = "ZIP";
                            textBlock.Foreground = new SolidColorBrush(Color.FromRgb(0xFF, 0xFF, 0xFF));
                            textBlock.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center;
                            textBlock.FontWeight = FontWeight.Black;
                            textBlock.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center;
                            b.Child = textBlock;
                            b.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center;
                            b.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center;
                            b.BorderThickness = new Avalonia.Thickness(1, 2);
                            b.BorderBrush = new SolidColorBrush(Color.FromRgb(0xFF, 0xFF, 0xFF));
                            b.Padding = new Avalonia.Thickness(2, 1);
                            grid.Children.Add(b);
                        }
                        break;
                    case FILE_XAML:
                    case FILE_AXAML:
                        {
                            var p = new Avalonia.Controls.Shapes.Path();
                            p.Fill = new SolidColorBrush(Foreground);
                            canvas.Children.Add(p);
                            p.Data = StreamGeometry.Parse("M16.93 7.9L19.31 12L16.95 16.09L16.33 15L17.8 12.5C17.97 12.19 17.97 11.83 17.8 11.54L16.32 9L16.93 7.9M16.92 6.57C16.89 6.57 16.85 6.59 16.83 6.62L15.56 8.87C15.5 8.94 15.5 9 15.56 9.09L17.18 11.9C17.22 11.97 17.22 12.05 17.18 12.12L15.57 14.91C15.53 14.97 15.53 15.06 15.57 15.13L16.85 17.36C16.87 17.4 16.91 17.42 16.94 17.42C17 17.42 17 17.4 17.04 17.36L20 12.23C20.08 12.09 20.08 11.92 20 11.79L17 6.62C17 6.59 16.96 6.57 16.92 6.57M21.91 11.67L17.23 3.58C17.11 3.38 16.89 3.25 16.66 3.25H7.28C7.05 3.25 6.83 3.38 6.71 3.58L2 11.67C1.91 11.87 1.91 12.13 2 12.33L6.71 20.42C6.83 20.62 7.05 20.75 7.28 20.75H16.66C16.89 20.75 17.11 20.62 17.23 20.42L21.91 12.33C22.03 12.13 22.03 11.88 21.91 11.67M7.3 3.95H16.12L11.76 11.65H6.93L10.87 4.82C10.91 4.75 10.86 4.66 10.78 4.66L8.21 4.65C8.13 4.65 8.06 4.7 8 4.76L4.04 11.65H2.84L7.3 3.95M6.17 12.46L9.74 18.63L8.5 18.63L4.87 12.35L4.67 12L4.87 11.65L8.5 5.37L9.73 5.37L6.17 11.53C6.15 11.57 6.13 11.61 6.11 11.65C6.03 11.88 6.03 12.13 6.12 12.35C6.13 12.39 6.15 12.43 6.17 12.46M7.3 20.05L2.85 12.35H4.05L8.03 19.23C8.07 19.3 8.14 19.34 8.22 19.34L10.79 19.34C10.87 19.34 10.92 19.25 10.88 19.18L6.94 12.35H11.77L16.17 20.05H7.3M16.8 19.75L12.37 12L16.78 4.21L21.29 12L16.8 19.75Z");
                        }
                        break;
                    case FILE_JSON:
                        {
                            var p = new Avalonia.Controls.Shapes.Path();
                            p.Fill = new SolidColorBrush(Foreground);
                            canvas.Children.Add(p);
                            p.Data = StreamGeometry.Parse("M5,3H7V5H5V10A2,2 0 0,1 3,12A2,2 0 0,1 5,14V19H7V21H5C3.93,20.73 3,20.1 3,19V15A2,2 0 0,0 1,13H0V11H1A2,2 0 0,0 3,9V5A2,2 0 0,1 5,3M19,3A2,2 0 0,1 21,5V9A2,2 0 0,0 23,11H24V13H23A2,2 0 0,0 21,15V19A2,2 0 0,1 19,21H17V19H19V14A2,2 0 0,1 21,12A2,2 0 0,1 19,10V5H17V3H19M12,15A1,1 0 0,1 13,16A1,1 0 0,1 12,17A1,1 0 0,1 11,16A1,1 0 0,1 12,15M8,15A1,1 0 0,1 9,16A1,1 0 0,1 8,17A1,1 0 0,1 7,16A1,1 0 0,1 8,15M16,15A1,1 0 0,1 17,16A1,1 0 0,1 16,17A1,1 0 0,1 15,16A1,1 0 0,1 16,15Z");
                        }
                        break;
                    case FILE_PS1:
                        {
                            var p = new Avalonia.Controls.Shapes.Path();
                            p.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0x22, 0x88, 0xEE));
                            canvas.Children.Add(p);
                            p.Data = StreamGeometry.Parse("M21.83,4C22.32,4 22.63,4.4 22.5,4.89L19.34,19.11C19.23,19.6 18.75,20 18.26,20H2.17C1.68,20 1.37,19.6 1.5,19.11L4.66,4.89C4.77,4.4 5.25,4 5.74,4H21.83M15.83,16H11.83C11.37,16 11,16.38 11,16.84C11,17.31 11.37,17.69 11.83,17.69H15.83C16.3,17.69 16.68,17.31 16.68,16.84C16.68,16.38 16.3,16 15.83,16M5.78,16.28C5.38,16.56 5.29,17.11 5.57,17.5C5.85,17.92 6.41,18 6.81,17.73C14.16,12.56 14.21,12.5 14.26,12.47C14.44,12.31 14.53,12.09 14.54,11.87C14.55,11.67 14.5,11.5 14.38,11.31L9.46,6.03C9.13,5.67 8.57,5.65 8.21,6C7.85,6.32 7.83,6.88 8.16,7.24L12.31,11.68L5.78,16.28Z");

                        }
                        break;
                    case FILE_SRI:
                        {
                            var p = new Avalonia.Controls.Shapes.Path();
                            p.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0x80, 0x40));
                            canvas.Children.Add(p);
                            p.Data = StreamGeometry.Parse("M11,13.5V21.5H3V13.5H11M9,15.5H5V19.5H9V15.5M12,2L17.5,11H6.5L12,2M12,5.86L10.08,9H13.92L12,5.86M17.5,13C20,13 22,15 22,17.5C22,20 20,22 17.5,22C15,22 13,20 13,17.5C13,15 15,13 17.5,13M17.5,15A2.5,2.5 0 0,0 15,17.5A2.5,2.5 0 0,0 17.5,20A2.5,2.5 0 0,0 20,17.5A2.5,2.5 0 0,0 17.5,15Z");

                        }
                        break;
                    case FILE_C:
                        {
                            TextBlock textBlock = new TextBlock();
                            textBlock.FontSize = 18;
                            textBlock.Text = "C";
                            textBlock.Foreground = new SolidColorBrush(Color.FromRgb(0xEE, 0x33, 0xFF));
                            textBlock.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center;
                            textBlock.FontWeight = FontWeight.Black;
                            textBlock.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center;
                            grid.Children.Add(textBlock);
                        }
                        break;
                    case FILE_7Z:
                        {
                            Border b = new Border();
                            TextBlock textBlock = new TextBlock();
                            textBlock.FontSize = 12;
                            textBlock.Text = "7Z";
                            textBlock.Foreground = new SolidColorBrush(Color.FromRgb(0xFF, 0xFF, 0xFF));
                            textBlock.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center;
                            textBlock.FontWeight = FontWeight.Black;
                            textBlock.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center;
                            b.Child = textBlock;
                            b.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center;
                            b.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center;
                            b.BorderThickness = new Avalonia.Thickness(1, 2);
                            b.BorderBrush = new SolidColorBrush(Color.FromRgb(0xFF, 0xFF, 0xFF));
                            b.Padding = new Avalonia.Thickness(4, 1);
                            grid.Children.Add(b);
                        }
                        break;
                    default:
                        {
                            var p = new Avalonia.Controls.Shapes.Path();
                            p.Fill = new SolidColorBrush(Foreground);
                            canvas.Children.Add(p);
                            p.Data = StreamGeometry.Parse("M14,2H6A2,2 0 0,0 4,4V20A2,2 0 0,0 6,22H18A2,2 0 0,0 20,20V8L14,2M18,20H6V4H13V9H18V20Z");
                        }
                        break;
                }
                return viewbox;
            }
            return null;
        }
    }
}
