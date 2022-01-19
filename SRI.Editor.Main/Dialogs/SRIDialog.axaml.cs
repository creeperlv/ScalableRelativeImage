using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SRI.Editor.Core;
using System;

namespace SRI.Editor.Main.Dialogs
{
    public partial class SRIDialog : Grid, IDialog
    {
        public SRIDialog()
        {
            InitializeComponent();
        }
        TextBlock HeaderText;
        TextBlock ContentText;
        Button Button0;
        Button Button1;
        Button Button2;
        TextBox InputBox;
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            HeaderText = this.FindControl<TextBlock>("HeaderText");
            ContentText = this.FindControl<TextBlock>("ContentText");
            InputBox = this.FindControl<TextBox>("InputBox");
            Button0 = this.FindControl<Button>("Button0");
            Button1 = this.FindControl<Button>("Button1");
            Button2 = this.FindControl<Button>("Button2");
            InputBox.IsVisible = false;
        }
        public void SetContent(string Header, string Content)
        {
            HeaderText.Text = Header;
            ContentText.Text = Content;
        }
        public void SetInputDialog(Action<string> AfterInput)
        {
            InputBox.IsVisible = true;
            SetButtons(new DialogButton
            {
                LanguageID = "Dialog.OK",
                Fallback = "OK",
                OnClick = () => { AfterInput(InputBox.Text); }
            },
            new DialogButton
            {
                LanguageID = "Dialog.Cancel",
                Fallback = "Cancel"
            });
        }
        public void SetButtons(DialogButton button0 = null, DialogButton button1 = null, DialogButton button2 = null)
        {
            if (button0 == null)
            {
                if (button1 == button0 && button2 == button0)
                {
                    SetButtons(new DialogButton
                    {
                        LanguageID = "Dialog.OK",
                        Fallback = "OK"
                    });
                    return;
                }
            }
            Button0.IsVisible = false;
            Button1.IsVisible = false;
            Button2.IsVisible = false;
            if (button0 != null)
            {
                Button0.IsVisible = true;
                Grid.SetColumnSpan(Button0, 6);
                Button0.Content = button0.Fallback;
                Button0.Click += (_, _) =>
                {
                    if (button0.OnClick != null)
                        button0.OnClick(); CloseDialog();
                };
            }
            else
            {
                Button0.Click += (_, _) => { CloseDialog(); };
            }
            if (button1 != null)
            {
                Button1.IsVisible = true;
                Grid.SetColumnSpan(Button0, 3);
                Grid.SetColumnSpan(Button1, 3);
                Grid.SetColumn(Button1, 3);
                Button1.Content = button1.Fallback;
                Button1.Click += (_, _) =>
                {
                    if (button1.OnClick != null)
                        button1.OnClick(); CloseDialog();
                };
            }
            else
            {
                Button1.Click += (_, _) => { CloseDialog(); };
            }
            if (button2 != null)
            {
                Button2.IsVisible = true;
                Grid.SetColumnSpan(Button0, 2);
                Grid.SetColumnSpan(Button1, 2);
                Grid.SetColumnSpan(Button2, 2);
                Grid.SetColumn(Button1, 2);
                Grid.SetColumn(Button2, 4);
                Button2.Content = button2.Fallback;
                Button2.Click += (_, _) =>
                {
                    if (button2.OnClick != null)
                        button2.OnClick(); CloseDialog();
                };
            }
            else
            {
                Button2.Click += (_, _) => { CloseDialog(); };
            }
        }
        void CloseDialog()
        {
            Globals.CurrentMainWindow.CloseDialog(this);
        }
    }
}
