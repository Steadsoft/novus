using System.Diagnostics;
using static Steadsoft.Novus.Scanner.Enums.TokenType;
using static Steadsoft.Novus.Scanner.Enums.Keywords;
using Steadsoft.Novus.Scanner.Enums;

namespace Steadsoft.Novus.Scanner.Classes
{
    [DebuggerDisplay("{TokenType} {Lexeme} {Keyword}")]
    public class Token
    {
        private static List<string> keywords;
        public TokenType TokenType { get; private set; }
        public string Lexeme { get; private set; }
        public int LineNumber { get; private set; }
        public int ColNumber { get; private set; }
        public Keywords Keyword { get; private set; }
        public Operators Operator { get; private set; }
        static Token()
        {
            keywords = Enum.GetNames(typeof(Keywords)).ToList().Select(k => k.ToLower()).ToList();
        }
        public Token(TokenType TokenType, string Lexeme, int LineNumber, int ColNumber)
        {
            this.TokenType = TokenType;
            this.Lexeme = Lexeme;
            this.LineNumber = LineNumber;
            this.ColNumber = ColNumber;

            Keywords keyword;
            Operators operater;

            if (TokenType == Identifier && Enum.TryParse(Lexeme, true, out keyword))
                Keyword = keyword;
            else
                Keyword = Enum.Parse<Keywords>("0");

            if (TokenType != Identifier && Enum.TryParse(TokenType.ToString(), true, out operater))
                Operator = operater;
            else
                Operator = Enum.Parse<Operators>("0");
        }
    }
}