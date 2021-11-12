using Steadsoft.Novus.Scanner.Enums;

namespace Steadsoft.Novus.Parser.Statements
{
    public class KeywordStatement : ComputeStatement
    {
        public Keywords Keyword { get; private set; }
        public KeywordStatement(int Line, int Col, Keywords Keyword) : base(Line, Col)
        {
            this.Keyword = Keyword;
        }
    }
}