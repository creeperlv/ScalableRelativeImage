using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SRI.Editor.Extension.Utilities
{
    public static class StringHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static bool IsEndsWith(string MainStr, params string[] strs)
        {
            foreach (var item in strs)
            {
                if (MainStr.EndsWith(item)) return true;
            }
            return false;
        }
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static bool IsEndsWithCaseInsensitive(string MainStr, params string[] strs)
        {
            var _MAINSTR = MainStr.ToUpper();
            foreach (var item in strs)
            {
                if (_MAINSTR.EndsWith(item.ToUpper())) return true;
            }
            return false;
        }
    }
}
