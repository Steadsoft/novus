using Steadsoft.Novus.Parser.Enums;
using static Steadsoft.Novus.Scanner.Enums.Keywords;

namespace Steadsoft.Novus.Parser.Statements
{
    public class CallStatement : KeywordStatement
    {
        public CallStatement(int Line, int Col) : base(Line, Col, Call)
        {

        }
    }
}