namespace Steadsoft.Novus.Scanner.Classes
{
    public record Character
    {
        public char Char { get; private init; }
        public int Line { get; private init; }
        public int Col { get; private init; }
        public Character(char Char, int Line, int Col)
        {
            this.Char = Char;
            this.Line = Line;
            this.Col = Col;
        }
    }
}
