using CLUNL.Localization;
using System;
using System.Collections.Generic;

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
        public static List<string> EnumerateLocales()
        {
            List<string> locales = new List<string>();  
            return locales;
        }
    }
    public interface ILocalizable
    {
        void ApplyLocal();
    }    
}
