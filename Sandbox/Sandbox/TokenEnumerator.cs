using System.Collections.Generic;

namespace Sandbox
{
    public class TokenEnumerator
    {
        private IEnumerator<Token> enumerator;
        private Token pushed_token = null;
        public TokenEnumerator(IEnumerable<Token> Source)
        {
            enumerator = Source.GetEnumerator();
        }

        public Token GetNextToken()
        {
            if (pushed_token != null)
            {
                var token = pushed_token;
                pushed_token = null;
                return token;
            }

            while (enumerator.MoveNext())
            {
                return enumerator.Current;
            }

            return new Token(TokenType.NoMoreTokens, "", 0, 0);
        }

        public void StepBackwards(Token Token)
        {
            pushed_token = Token;
        }

        public Token SkipToNext(string Lexeme)
        {
            var token = GetNextToken();

            while (token.Lexeme != Lexeme && token.TokenCode != TokenType.NoMoreTokens)
            {
                token = GetNextToken();
            }

            return token;
        }


    }
}

