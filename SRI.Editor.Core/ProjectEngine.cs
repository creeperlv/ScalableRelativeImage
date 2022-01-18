using ScalableRelativeImage;
using ScalableRelativeImage.Nodes;
using SRI.Editor.Core.Projects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SRI.Editor.Core
{
    public class ProjectEngine
    {
        public static LoadedProject Load(FileInfo file)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Project));

            var reader = file.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            XmlReader xmlReader = XmlReader.Create(reader);
            Project __project = (Project)xmlSerializer.Deserialize(xmlReader);
            xmlReader.Close();
            xmlReader.Dispose();
            reader.Close();
            reader.Dispose();
            return new LoadedProject { CoreProject = __project, WorkingDirectory = file.Directory, ProjectFile = file };
        }
        public static void BuildSync(LoadedProject project, string configuration, Action<int, BuildTarget> OnStartProcess = null,
                            Action<int, BuildTarget> OnEndProcess = null,
                            Action OnFinished = null,
                            Action<Exception> OnError = null,
                            Action<int, List<ExecutionWarning>> OnReceieveWarning = null)
        {
            BuildProcess process = new(project, configuration, OnStartProcess, OnEndProcess, OnFinished, OnError, OnReceieveWarning);
            process.Execute();
        }
        public static string NewEmptyProject()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Project));
            Project project = new Project();
            project.BuildConfigurations = new List<BuildConfiguration>();
            var conf = new BuildConfiguration();
            conf.Name = "Debug";
            conf.Symbols.Add(new Symbol { Name = "DEBUG", Value = "true" });
            conf.OutputDirectory = "bin/Debug";
            project.BuildConfigurations.Add(conf);
            var __target = new BuildTarget();
            __target.Name = "Target_Name(Any String)";
            __target.File = "<Path-To-Your-File>";
            __target.OutputName = "<Path-To-Your-File>.png";
            __target.Background = "Transparent";
            __target.Foreground = "White";
            __target.Width = 1024;
            __target.Height = 1024;
            __target.Symbols.Add(new Symbol { Name = "<Symbol>", Value = "<Any-Value>" });
            conf.BuildTargets.Add(__target);

            string content;
            using (MemoryStream ms = new MemoryStream())
            {
                using (var __sw = new StreamWriter(ms, Encoding.UTF8))
                {
                    xmlSerializer.Serialize(__sw, project);

                }
                content = Encoding.UTF8.GetString(ms.ToArray());
            }
            return content;
        }
        public static (BuildProcess, Task) BuildAsync(LoadedProject project, string configuration,
                            Action<int, BuildTarget> OnStartProcess = null,
                            Action<int, BuildTarget> OnEndProcess = null,
                            Action OnFinished = null,
                            Action<Exception> OnError = null,
                            Action<int, List<ExecutionWarning>> OnReceieveWarning = null)
        {
            BuildProcess process = new(project, configuration, OnStartProcess, OnEndProcess, OnFinished, OnError, OnReceieveWarning);
            return (process, Task.Run(() => process.Execute()));
        }
        public static void Save(LoadedProject project)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Project));
            xmlSerializer.Serialize(project.ProjectFile.OpenWrite(), project.CoreProject);
        }
    }
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
            TotalCount = P.CoreProject.BuildConfigurations.Count;
            Current = 0;
        }
        public void Execute()
        {
            try
            {
                BuildConfiguration TargetConfiguration = null;
                foreach (var item in project.CoreProject.BuildConfigurations)
                {
                    if (item.Name == configuration)
                    {
                        TargetConfiguration = item;
                        break;
                    }
                }
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
                        renderProfile.TargetWidth = Width;
                        renderProfile.TargetHeight = Height;
                        renderProfile.DefaultForeground = Foreground;
                        renderProfile.DefaultBackground = Background;
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
