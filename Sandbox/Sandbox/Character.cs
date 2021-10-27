using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox
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
