using CLUNL.Localization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRI.Editor.Main.Data
{
    [Serializable]
    public class EditorConfiguration
    {
        public static void Init()
        {
            CurrentConfiguration = JsonConvert.DeserializeObject<EditorConfiguration>(ObtainInstalled("SRI.Editor.Configuration.json", "SRI.Editor"), new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore, Formatting = Formatting.Indented });
        }
        public static void Save()
        {
            Save("SRI.Editor.Configuration.json", "SRI.Editor");
        }

        public static void Save(string SettingFileName, string ProductName)
        {

            var configuration = JsonConvert.SerializeObject(CurrentConfiguration);
            if (File.Exists("./" + SettingFileName))
            {
                File.Delete("./" + SettingFileName);
                File.WriteAllText("./" + SettingFileName, configuration);
            }
            else
            {
                string text = Path.Combine(new FileInfo(typeof(Language).Assembly.Location).Directory!.FullName, SettingFileName);
                if (File.Exists(text))
                {
                    File.Delete(text);
                    File.WriteAllText(text, configuration);
                }
                else
                {
                    string text2 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), ProductName);
                    if (!Directory.Exists(text2))
                    {
                        Directory.CreateDirectory(text2);
                    }

                    string text3 = Path.Combine(text2, SettingFileName);
                    if (File.Exists(text3))
                    {
                        File.Delete(text3);
                        File.WriteAllText(text3, configuration);
                    }
                    else
                    {
                        File.WriteAllText(text3, configuration);
                    }
                }
            }
        }
        public static string ObtainInstalled(string SettingFileName, string ProductName)
        {

            string configuration = "";
            if (File.Exists("./" + SettingFileName))
            {
                configuration = File.ReadAllText("./" + SettingFileName);
            }
            else
            {
                string text = Path.Combine(new FileInfo(typeof(Language).Assembly.Location).Directory!.FullName, SettingFileName);
                if (File.Exists(text))
                {
                    configuration = LoadFromFile(text);
                }
                else
                {
                    string text2 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), ProductName);
                    if (!Directory.Exists(text2))
                    {
                        Directory.CreateDirectory(text2);
                    }

                    string text3 = Path.Combine(text2, SettingFileName);
                    if (File.Exists(text3))
                    {
                        configuration = LoadFromFile(text3);
                    }
                    else
                    {
                        File.WriteAllText(text3, JsonConvert.SerializeObject(new EditorConfiguration()));
                        configuration = LoadFromFile(text3);
                    }
                }
            }

            return configuration;
            static string LoadFromFile(string p)
            {
                return File.ReadAllLines(p)[0];
            }
        }
        public static EditorConfiguration CurrentConfiguration = new EditorConfiguration();
        public bool isBlurEnabled = true;
        public int ComputeMode = 0;
        public bool TransparentInsteadOfBlur = false;
        public int Backend;
    }
}
