using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRI.Editor.Extension
{
    /// <summary>
    /// 
    /// </summary>
    public class EditorProvider
    {

        static Dictionary<string, string> RegisteredBindings = new();
        static Dictionary<string, EditorBinding> EditorBinds = new();
        public static Dictionary<string, string> EnumerateEditors()
        {
            Dictionary<string, string> ID_Name = new Dictionary<string, string>();
            foreach (var item in EditorBinds)
            {
                ID_Name.Add(item.Key, item.Value.FallbackName);
            }
            return ID_Name;
        }
        public static void RegisterEditor(string NameID, string FallbackName, Type EditorT, params string[] extensionNames)
        {
            EditorBinding bind = null;
            if (RegisteredBindings.ContainsKey(NameID))
            {
                bind = EditorBinds[NameID];
            }
            if (bind is null)
            {
                EditorBinding editorBinding = new EditorBinding();
                editorBinding.NameID = NameID;
                editorBinding.FallbackName = FallbackName;
                editorBinding.EditorT = EditorT;
                EditorBinds.Add(NameID, editorBinding);
            }
            foreach (var item in extensionNames)
            {
                var ext = item.ToUpper();
                if (!ext.StartsWith(".")) ext = "." + ext;
                if (!RegisteredBindings.ContainsKey(ext))
                    RegisteredBindings.Add(ext, NameID);
                else RegisteredBindings[ext] = NameID;
            }
        }
        public static IEditor RequestEditor(string EditorID)
        {
            return (IEditor)Activator.CreateInstance(EditorBinds[EditorID].EditorT);
        }
        public static IEditor FindAndInitEditor(FileInfo fi)
        {
            var ext = fi.Extension.ToUpper();
            if (RegisteredBindings.ContainsKey(ext))
            {
                var b = RegisteredBindings[ext];
                var e = EditorBinds[b];
                var editor = (IEditor)Activator.CreateInstance(e.EditorT);
                if (editor is not null)
                {
                    //editor.OpenFile(fi);
                    return editor;
                }
            }
            if (RegisteredBindings.ContainsKey("*"))
            {
                var b = RegisteredBindings["*"];
                var e = EditorBinds[b];
                var editor = (IEditor)Activator.CreateInstance(e.EditorT);
                if (editor is not null)
                {
                    //editor.OpenFile(fi);
                    return editor;
                }

            }
            {
                var editor = new EditorControl();
                //editor.OpenFile(fi);
                return editor;
            }
        }
    }
    internal class EditorBinding
    {
        internal string NameID;
        internal string FallbackName;
        internal Type EditorT;
    }
}
