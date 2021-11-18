using static Steadsoft.Novus.Scanner.Enums.Keywords;

namespace Steadsoft.Novus.Parser.Statements
{
    public class WhileStatement : KeywordStatement
    {
        public WhileStatement(int Line, int Col) : base(Line, Col, While)
        {

        }
    }
}