namespace Steadsoft.Novus.Scanner
{
    public static class CharExtensions
    {
        public static LexicalClass GetLexicalClass(this char C)
        {
            // For now we use the Unicode defintions for
            // the various charactrer classes but ideally
            // these should be defined as needed by the language.

            if (char.IsLetter(C))
                return LexicalClass.Alpha;

            if (char.IsDigit(C))
                return LexicalClass.Digit;

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
    }
}