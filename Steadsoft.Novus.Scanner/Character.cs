using System;

namespace Steadsoft.Novus.Scanner
{
    public record Character
    {
        public Char Char { get; private set; }
        public int Line { get; private set; }
        public int Col { get; private set; }
        public Character(Char Char, int Line, int Col)
        {
            this.Char = Char;
            this.Line = Line;
            this.Col = Col;
        }
    }
}
