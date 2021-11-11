namespace Steadsoft.Novus.Parser.Statements
{
    public class ComputeStatement : Statement
    {
        public ComputeStatement(int Line, int Col) : base(Line, Col)
        {

        }

        public override string Dump(int nesting)
        {
            throw new NotImplementedException();
        }
    }
}
