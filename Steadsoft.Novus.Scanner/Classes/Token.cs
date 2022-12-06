using System.Diagnostics;
using static Steadsoft.Novus.Scanner.Enums.TokenType;
using Steadsoft.Novus.Scanner.Enums;
using static System.Enum;
namespace Steadsoft.Novus.Scanner.Classes
{
    [DebuggerDisplay("{TokenType} {Lexeme} {KeywordText}")]
    public class Token
    {
        private static List<string> keywords;
        public TokenType TokenType { get;  set; }
        public string Lexeme { get; set; }
        public int LineNumber { get;  set; }
        public int ColNumber { get;  set; }
        public (Keywords,int)[] KeywordList { get; set; }
        public Operators Operator { get; private set; }
        public bool IsInvalid { get; set; }
        public string ErrorText { get;  set; }
        public bool ExactKeywordMatch { get; set; }
        public string KeywordText
        {
            get 
            {
                string text = "";

                foreach (var keyword in KeywordList)
                {
                    if (keyword.Item1 != Keywords.IsNotAKeyword)
                    text = text + keyword.ToString() + "-";
                }

                return text.TrimEnd('-');
            }
        }
        static Token()
        {
            keywords = System.Enum.GetNames(typeof(Keywords)).ToList().Select(k => k.ToLower()).ToList();
        }
        public Token(TokenType TokenType, string Lexeme, int LineNumber, int ColNumber)
        {
            this.TokenType = TokenType;
            this.Lexeme = Lexeme;
            this.LineNumber = LineNumber;
            this.ColNumber = ColNumber;
            this.IsInvalid = false;
            this.ErrorText= string.Empty;
            this.ExactKeywordMatch = false;
            this.KeywordList = new (Keywords,int)[6];

            Keywords keyword;
            Operators operater;

            ////if (TokenType == Identifier && TryParse(Lexeme, true, out keyword))
            //if (TokenType == Identifier && TryMatchFullName<Keywords>(Lexeme, (int)Keywords.AbbreviationShift, out keyword))
            //{
            //    Keyword = keyword;
            //}
            //else
            //    Keyword = Parse<Keywords>("0");

            if (TokenType != Identifier && TryParse(TokenType.ToString(), true, out operater))
                Operator = operater;
            else
                Operator = Parse<Operators>("0");
        }

        //public bool Matches(Keywords Keyword)
        //{
        //    if (ExactKeywordMatch)
        //        if (KeywordList.First() == Keyword)
        //            return true;


        //}



        private static bool TryMatchFullName<T>(string spelling, int scaler, out T FullValue) where T : struct, Enum
        {
            if (System.Enum.TryParse<T>(spelling, true, out FullValue))
            {
                int numeric = Convert.ToInt16(FullValue);

                if (Convert.ToInt16(FullValue) > scaler)
                {
                    Enum.TryParse((numeric - scaler).ToString(), out FullValue);
                }

                return true;
            }
            return false;
        }

    }
}