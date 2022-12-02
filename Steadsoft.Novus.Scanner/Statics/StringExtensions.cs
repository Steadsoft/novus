using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steadsoft.Novus.Scanner.Statics
{
    public static class StringExtensions
    {
        public static bool ContainsAny(this string Text, params char[] Characters)
        {
            foreach(char c in Characters) 
            { 
                if (Text.Contains(c))
                    return true;
            }

            return false;
        }

        public static bool EndsWithAny(this string Text, params string[] endings)
        {
            foreach(string ending in endings)
            {
                if (Text.EndsWith(ending))
                    return true;
            }

            return false;

        }
    }
}
