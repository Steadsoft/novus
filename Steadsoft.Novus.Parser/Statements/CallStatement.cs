using Steadsoft.Novus.Parser.Enums;
using static Steadsoft.Novus.Parser.Enums.NovusKeywords;

namespace Steadsoft.Novus.Parser.Statements
{
    public class CallStatement : KeywordStatement
    {
        public CallStatement(int Line, int Col) : base(Line, Col, Call)
        {

        }
    }
}