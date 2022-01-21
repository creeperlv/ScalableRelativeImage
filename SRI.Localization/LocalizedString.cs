using CLUNL.Localization;
using System.Runtime.CompilerServices;

namespace SRI.Localization
{
    public class LocalizedString
    {
        public string ID;
        public string Fallback;
        public LocalizedString(string ID, string Fallback)
        {
            this.ID = ID;
            this.Fallback = Fallback;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator string(LocalizedString LS)
        {
            return LS.ToString();
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            return Language.Find(ID, Fallback);
        }
    }

}
