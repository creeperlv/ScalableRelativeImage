using CLUNL.Localization;

namespace SRI.Localization
{
    public class LocalizedString {
        public string ID;
        public string Fallback;
        public LocalizedString(string ID,string Fallback)
        {
            this.ID = ID;
            this.Fallback = Fallback;
        }
        public override string ToString()
        {
            return Language.Find(ID, Fallback);
        }
    }
    
}
