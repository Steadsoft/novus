using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Steadsoft.Novus.Scanner
{
    [DebuggerDisplay("{TokenCode} {Lexeme} {Keyword}")]
    public class Token<T> where T : struct, System.Enum
    {
        private static List<string> keywords;
        public TokenType TokenCode { get; private set; }
        public string Lexeme { get; private set; }
        public int LineNumber { get; private set; }
        public int ColNumber { get; private set; }
        public T Keyword { get; private set; }

        static Token()
        {
            keywords = Enum.GetNames(typeof(T)).ToList().Select(k => k.ToLower()).ToList();
        }
        public Token(TokenType TokenCode, string Lexeme, int LineNumber, int ColNumber)
        {
            this.TokenCode = TokenCode;
            this.Lexeme = Lexeme;
            this.LineNumber = LineNumber;
            this.ColNumber = ColNumber;

            T keyword;

            if (TokenCode == TokenType.Identifier && Enum.TryParse<T>(Lexeme, true, out keyword))
                Keyword = keyword;
            else
                Keyword = Enum.Parse<T>("0");
        }
    }
}