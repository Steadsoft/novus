using Steadsoft.Novus.Parser.Enums;

namespace Steadsoft.Novus.Parser.Statements
{
    public class WhileStatement : KeywordStatement
    {
        public WhileStatement(int Line, int Col) : base(Line, Col, NovusKeywords.While)
        {

        }
    }
}