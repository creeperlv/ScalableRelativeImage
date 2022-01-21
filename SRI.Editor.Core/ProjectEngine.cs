using ScalableRelativeImage;
using SRI.Editor.Core.Projects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public static readonly string SRIContentTemplate = "<ScalableRelativeImage Flavor=\"CreeperLv.SRI\" FormatVersion=\"1.0.0.0\">" + "\n" +
"\t<ImageNodeRoot RelativeWidth=\"1920\" RelativeHeight=\"1080\">" + "\n" +
"\t\t<!--Shapes Here...-->" + "\n" +
"\t</ImageNodeRoot>" + "\n" +
"</ScalableRelativeImage>";
        public static string NewSRIDocument() {
            return SRIContentTemplate;
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
            project.ProjectFile.Delete();
            project.ProjectFile.Create().Close();
            var wr=project.ProjectFile.OpenWrite();
            xmlSerializer.Serialize(wr, project.CoreProject);
            wr.Close();
            wr.Dispose();
        }
    }
}
