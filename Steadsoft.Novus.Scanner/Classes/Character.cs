namespace Steadsoft.Novus.Scanner.Classes
{
    public record Character
    {
        public char Char { get; private set; }
        public int Line { get; private set; }
        public int Col { get; private set; }
        public Character(char Char, int Line, int Col)
        {
            this.Char = Char;
            this.Line = Line;
            this.Col = Col;
        }
    }
}
