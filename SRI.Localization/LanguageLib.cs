using CLUNL.Localization;
using System;

namespace SRI.Localization
{
    public class LanguageLib
    {
        public static void InitLocal()
        {
            if (!Language.IsInited())
            {
                Language.Init("SRILanguage", "SRI");
            }
        }
    }
    public interface ILocalizable
    {
        void ApplyLocal();
    }    
}
