namespace Steadsoft.Novus.Parser.Statements
{

    public class UsingStatement : Statement
    {
        public string Name { get; private set; }

        public UsingStatement(int Line, int Col, string Name) : base(Line, Col)
        {
            this.Name = Name;
        }

        public override string ToString()
        {
            return Name;
        }

        public override string Dump(int nesting)
        {
            return "Using " + Name;
        }
    }
}
