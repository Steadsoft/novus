namespace Steadsoft.Novus.Parser.Statements
{
    public abstract class Statement
    {
        public int Line { get; private set; }
        public int Col { get; private set; }

        public Statement(int Line, int Col)
        {
            this.Line = Line;
            this.Col = Col;
        }

        public abstract string Dump(int nesting);

        public static string Prepad(int N)
        {
            return new string(' ', N);
        }
    }
}
