using Steadsoft.Novus.Parser.Enums;

namespace Steadsoft.Novus.Parser.Statements
{
    public class KeywordStatement : ComputeStatement
    {
        public NovusKeywords Keyword { get; private set; }
        public KeywordStatement(int Line, int Col, NovusKeywords Keyword) : base(Line, Col)
        {
            this.Keyword = Keyword;
        }
    }
}