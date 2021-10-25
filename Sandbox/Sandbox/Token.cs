namespace Sandbox
{
    public class Token
    {
        public TokenType TokenCode { get; private set; }
        public string Lexeme { get; private set; }
        public int LineNumber { get; private set; }
        public int ColNumber { get; private set; }

        public Token(TokenType TokenCode, string Lexeme, int LineNumber, int ColNumber)
        {
            this.TokenCode = TokenCode;
            this.Lexeme = Lexeme;
            this.LineNumber = LineNumber;
            this.ColNumber = ColNumber;
        }
    }
}