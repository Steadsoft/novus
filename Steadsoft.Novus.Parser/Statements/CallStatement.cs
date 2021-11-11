using Steadsoft.Novus.Parser.Enums;

namespace Steadsoft.Novus.Parser.Statements
{
    public class CallStatement : KeywordStatement
    {
        public CallStatement(int Line, int Col) : base(Line, Col, NovusKeywords.Call)
        {

        }
    }
}