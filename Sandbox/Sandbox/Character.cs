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
        public Kind Kind { get; private set; }
        public int Line { get; private set; }
        public int Col { get; private set; }
        public Character(Char Char, int Line, int Col)
        {
            this.Char = Char;
            this.Line = Line;
            this.Col = Col;
            this.Kind = Kind.Uncategorized;

            if (Char >= 0 && Char <= 0x1F)
                Kind = Kind.Uncategorized;

            if (Char == ' ')
                Kind = Kind.Whitespace;

            if (Char >= 'A' && Char <= 'Z')
                Kind = Kind.Letter;

            if (Char >= 'a' && Char <= 'z')
                Kind = Kind.Letter;

            if ((Char >= '!' && Char <= '/') || (Char >= ':' && Char <= '@') || (Char >= '[' && Char <= '`') || (Char >= '{' && Char <= '~'))
                Kind = Kind.Symbol;

            if (Char == '(' || Char == ')')
                Kind = Kind.Parenthesis;

            if (Char == '[' || Char == ']')
                Kind = Kind.Bracket;

            if (Char == '{' || Char == '}')
                Kind = Kind.Brace;

            if (Char == '<' || Char == '>')
                Kind = Kind.Arrow;

            if (Char >= '0' && Char <= '9')
                Kind = Kind.Digit;

            if (Char == '\r')
                Kind = Kind.CR;

            if (Char == '\n')
                Kind = Kind.LF;

            if (Char == ':' || Char == ';' || Char == '.' || Char == ',')
                Kind = Kind.Punctuation;
        }
    }
}
