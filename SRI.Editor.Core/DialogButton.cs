using System;

namespace SRI.Editor.Core
{
    public class DialogButton
    {
        public string LanguageID;
        public string Fallback;
        public Action OnClick = null;
    }
}
