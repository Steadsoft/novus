namespace Sandbox
{
    
    public enum CharClass : uint
    {
        APL_IBEAM = 0x2336,
        None = 0,
        Any = APL_IBEAM,
        Alpha = APL_IBEAM + 1,
        Digit = APL_IBEAM + 2,
        Group = (char)(APL_IBEAM + 3),
        White = (char)(APL_IBEAM + 4),
        Separ = (char)(APL_IBEAM + 5),
        Punct = (char)(APL_IBEAM + 6),
        Symbl = (char)(APL_IBEAM + 7)
    }
    public static class CharSupport
    {
        // We use these unicode characters to represent character 
        // classes that comprise tokens. This means that we can't ever
        // use these chatacters in source code, and that isn't too
        // hard to address if it were needed.

        //public const int APL_IBEAM = 0x2336;
        //public const char None =  (char)(0);
        //public const char Any = (char)(APL_IBEAM + 0);
        //public const char Alpha = (char)(APL_IBEAM + 1);
        //public const char Digit = (char)(APL_IBEAM + 2);
        //public const char Group = (char)(APL_IBEAM + 3);
        //public const char White = (char)(APL_IBEAM + 4);
        //public const char Separ = (char)(APL_IBEAM + 5);
        //public const char Punct = (char)(APL_IBEAM + 6);
        //public const char Symbl = (char)(APL_IBEAM + 7);

        public static CharClass GetClass(char C)
        {
            // For now we use the Unicode defintions for
            // the various charactrer classes but ideally
            // these should be defined as needed by the language.

            if (char.IsLetter(C))
                return CharClass.Alpha;

            if (char.IsDigit(C))
                return CharClass.Digit;

            if (char.IsWhiteSpace(C))
                return CharClass.White;

            if (char.IsSeparator(C))
                return CharClass.Separ;

            if (char.IsPunctuation(C))
                return CharClass.Punct;

            if (char.IsSymbol(C))
                return CharClass.Symbl;

            return CharClass.None;
        }
    }
}