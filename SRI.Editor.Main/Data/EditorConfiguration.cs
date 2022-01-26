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

        }
        public static string ObtainInstalled(string SettingFileName, string ProductName)
        {

            string configuration = "";
            if (File.Exists("./" + SettingFileName))
            {
                configuration = File.ReadAllLines("./" + SettingFileName)[0];
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
        public bool TransparentInsteadOfBlur = false;
    }
}
