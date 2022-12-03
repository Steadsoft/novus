using Steadsoft.Novus.Scanner.Enums;
using System.Runtime.CompilerServices;

namespace Steadsoft.Novus.Scanner.Statics
{
    public static class CharExtensions
    {
        public const char LF = '\u000A';
        public const char CR = '\u000D';
        public const char NL = '\u0085';
        public const char LS = '\u2028';
        public const char PS = '\u2029';

        public static LexicalClass GetLexicalClass(this char C)
        {
            // For now we use the Unicode defintions for
            // the various charactrer classes but ideally
            // these should be defined as needed by the language.

            if (C == ' ')
                return LexicalClass.Space;

            if (IsHexDigit(C))
                return LexicalClass.Hex;

            if (char.IsLetter(C))
                return LexicalClass.Alpha;

            if (char.IsDigit(C))
                return LexicalClass.Digit;

            if (C == CR)
                return LexicalClass.CR;

            if (C == LF)
                return LexicalClass.LF;

            if ( C == NL | C == LS | C == PS)
                return LexicalClass.NewLine;

            if (char.IsWhiteSpace(C))
                return LexicalClass.White;

            if (char.IsSeparator(C))
                return LexicalClass.Separ;

            if (char.IsPunctuation(C))
                return LexicalClass.Punct;

            if (char.IsSymbol(C))
                return LexicalClass.Symbl;

            return LexicalClass.None;
        }

        private static bool IsHexDigit(Char C)
        {
            if (C == 'a' | C == 'b' | C == 'c' | C == 'd' | C == 'e' || C == 'f' | C == 'A' | C == 'B' | C == 'C' | C == 'D' | C == 'E' || C == 'F')
                return true;

            return false;
        }

    }
}