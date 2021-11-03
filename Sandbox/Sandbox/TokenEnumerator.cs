using System.Collections.Generic;
using System.Linq;

namespace Sandbox
{
    public class TokenEnumerator
    {
        private IEnumerator<Token> enumerator;
        private Token pushed_token = null;
        private TokenType[] Skips;
        public TokenEnumerator(IEnumerable<Token> Source, params TokenType[] Skips)
        {
            enumerator = Source.GetEnumerator();
            this.Skips = Skips;
        }

        public Token GetNextToken()
        {
            if (pushed_token != null)
            {
                var token = pushed_token;
                pushed_token = null;

                if (Skips.Contains(token.TokenCode) == false)
                   return token;
            }

            while (enumerator.MoveNext())
            {
                if (Skips.Contains(enumerator.Current.TokenCode) == false)
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

