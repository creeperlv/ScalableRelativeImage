using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScalableRelativeImage
{
    /// <summary>
    /// Symbol used to render different image from a single SRI.
    /// </summary>
    [Serializable]
    public class Symbol
    {
        public string Name;
        public string Value;
    }
    /// <summary>
    /// Like Environment contains current Symbols.
    /// </summary>
    public class SymbolHelper
    {
        public Dictionary<string, string> Symbols = new Dictionary<string, string>();
        public string Lookup(string Symbol, string Fallback = "")
        {
            if (Symbols.ContainsKey(Symbol))
            {
                return Symbols[Symbol];
            }
            else
                return Fallback;
        }
        public bool TryLookup(string Symbol, string Fallback = "", out string Result)
        {
            if (Symbols.ContainsKey(Symbol))
            {
                Result = Symbols[Symbol];
                return true;
            }
            Result = Fallback;
            return false;
        }
        public void Set(Symbol s)
        {
            if (Symbols.ContainsKey(s.Name))
            {
                Symbols[s.Name] = s.Value;
            }
            else
            {
                Symbols.Add(s.Name, s.Value);
            }
        }
        public void Set(string Name, string Value)
        {
            if (Symbols.ContainsKey(Name))
            {
                Symbols[Name] = Value;
            }
            else
            {
                Symbols.Add(Name, Value);
            }
        }
    }
}
