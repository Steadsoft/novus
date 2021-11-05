using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Sandbox
{
    [DebuggerDisplay("{TokenCode} {Lexeme} {Keyword}")]
    public class Token
    {
        private static List<string> keywords;
        public TokenType TokenCode { get; private set; }
        public string Lexeme { get; private set; }
        public int LineNumber { get; private set; }
        public int ColNumber { get; private set; }
        public Keyword Keyword { get; private set; }

        static Token()
        {
            keywords = Enum.GetNames(typeof(Keyword)).ToList().Select(k => k.ToLower()).ToList();
        }
        public Token(TokenType TokenCode, string Lexeme, int LineNumber, int ColNumber)
        {
            this.TokenCode = TokenCode;
            this.Lexeme = Lexeme;
            this.LineNumber = LineNumber;
            this.ColNumber = ColNumber;

            Keyword keyword;

            if (TokenCode == TokenType.Identifier && Enum.TryParse<Keyword>(Lexeme, true, out keyword))
                Keyword = keyword;
            else
                Keyword = Keyword.IsNotKeyword;
        }
    }
}