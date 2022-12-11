using ScalableRelativeImage;
using ScalableRelativeImage.Nodes;
using SRI.Core.Backend.Magick;
using SRI.Core.Backend.SystemDrawing;
using SRI.Core.Core;
using SRI.Core.Utilities;
using SRI.Editor.Core.Projects;
using SRI.Editor.Main.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace SRI.Editor.Core
{
    public class BuildProcess
    {
        public int TotalCount;
        public int Current;
        LoadedProject project;
        string configuration;
        Action<int, BuildTarget> OnStartProcess = null;
        Action OnFinished = null;
        Action<int, BuildTarget> OnEndProcess = null;

        Action<Exception> OnError = null;
        Action<int, List<ExecutionWarning>> OnReceieveWarning = null;
        public BuildProcess(LoadedProject P, string configuration,
                            Action<int, BuildTarget> OnStartProcess = null,
                            Action<int, BuildTarget> OnEndProcess = null,
                            Action OnFinished = null,
                            Action<Exception> OnError = null,
                            Action<int, List<ExecutionWarning>> OnReceieveWarning = null)
        {
            this.OnStartProcess = OnStartProcess;
            this.OnEndProcess = OnEndProcess;
            this.OnFinished = OnFinished;
            this.OnError = OnError;
            this.OnReceieveWarning = OnReceieveWarning;
            this.configuration = configuration;
            Current = 0;
            project = P;
            foreach (var item in project.CoreProject.BuildConfigurations)
            {
                if (item.Name == configuration)
                {
                    TargetConfiguration = item;
                    break;
                }
            }
            TotalCount = TargetConfiguration.BuildTargets.Count;
        }
        BuildConfiguration TargetConfiguration = null;
        public void Execute()
        {
            try
            {
                foreach (var item in TargetConfiguration.BuildTargets)
                {
                    Current++;
                    if (OnStartProcess != null)
                    {
                        OnStartProcess(Current, item);
                    }
                    {
                        //Process
                        var source = new FileInfo(System.IO.Path.Combine(project.WorkingDirectory.FullName, item.File));
                        ImageNodeRoot imageNodeRoot = null;
                        if (OnReceieveWarning == null)
                        {
                            imageNodeRoot = SRIEngine.Deserialize(source, out _);

                        }
                        else
                        {
                            imageNodeRoot = SRIEngine.Deserialize(source, out var w);
                            OnReceieveWarning(Current, w);
                        }
                        foreach (var symbol in item.Symbols)
                        {
                            imageNodeRoot.Symbols.Set(symbol);
                        }
                        foreach (var symbol in TargetConfiguration.Symbols)
                        {
                            imageNodeRoot.Symbols.Set(symbol);
                        }

                        ColorConverter cc = new ColorConverter();
                        Color Foreground = Color.White;
                        Color Background = Color.Transparent;
                        if (item.Background != null)
                        {
                            Background = (Color)cc.ConvertFromString(item.Background);
                        }
                        else
                        {
                        }
                        if (item.Foreground != null)
                        {
                            Foreground = (Color)cc.ConvertFromString(item.Foreground);
                        }
                        else
                        {
                        }
                        int Width;
                        int Height;
                        {

                            if (item.Width == -1)
                            {
                                Width = (int)imageNodeRoot.RelativeWidth;
                            }
                            else
                            {
                                Width = item.Width;
                            }
                            if (item.Height == -1)
                            {
                                Height = (int)imageNodeRoot.RelativeHeight;
                            }
                            else
                            {
                                Height = item.Height;
                            }
                        }

                        RenderProfile renderProfile = new RenderProfile();
                        switch ((BackendDefinition)EditorConfiguration.CurrentConfiguration.Backend)
                        {
                            case BackendDefinition.SystemDrawing:
                                renderProfile.UseSystemDrawing();
                                break;
                            case BackendDefinition.Magick:
                                renderProfile.UseMagick();
                                break;
                            default:
                                break;
                        }
                        renderProfile.TargetWidth = Width;
                        renderProfile.TargetHeight = Height;
                        renderProfile.DefaultForeground = Foreground.ToColorF();
                        renderProfile.DefaultBackground = Background.ToColorF();
                        renderProfile.WorkingDirectory = source.Directory.FullName;
                        var bitmap = imageNodeRoot.Render(renderProfile);

                        string _Output = item.OutputName;
                        string output = System.IO.Path.Combine(project.WorkingDirectory.FullName, TargetConfiguration.OutputDirectory, _Output);
                        if (File.Exists(output)) File.Delete(output);
                        FileInfo fileInfo = new FileInfo(output);
                        if (!fileInfo.Directory.Exists)
                        {
                            fileInfo.Directory.Create();
                        }
                        bitmap.Save(output);
                    }
                    if (OnEndProcess != null)
                    {
                        OnEndProcess(Current, item);
                    }
                    GC.Collect();
                }
                OnFinished();
            }
            catch (Exception e)
            {
                if (OnError != null)
                {
                    OnError(e);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
