using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScalableRelativeImage
{
    [Serializable]
    public class Symbol
    {
        public string Name;
        public string Value;
    }
    public class SymbolHelper
    {
        public Dictionary<string, string> Symbols = new Dictionary<string, string>();
        public string Lookup(string Symbol,string Fallback = "")
        {
            if (Symbols.ContainsKey(Symbol))
            {
                return Symbols[Symbol];
            }else
            return Fallback;
        }
        public void Set(Symbol s)
        {
            if (Symbols.ContainsKey(s.Name))
            {
                Symbols[s.Name] = s.Value;
            }else
            {
                Symbols.Add(s.Name, s.Value);
            }
        }
    }
}
