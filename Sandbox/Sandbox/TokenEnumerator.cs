using System.Collections.Generic;
using System.Linq;

namespace Sandbox
{
    public class TokenEnumerator
    {
        private IEnumerator<Token> enumerator;
        private Token pushed_token = null;
        private TokenType[] Skips;
        private Stack<Token> pushed = new Stack<Token>();
        public TokenEnumerator(IEnumerable<Token> Source, params TokenType[] Skips)
        {
            enumerator = Source.GetEnumerator();
            this.Skips = Skips;
        }

        public Token GetNextToken()
        {
            // Once a token has been consumed it is in the past, can't be re-read.
            // But we can 'push' a token at any point and the next time we get a
            // token it will be that pushed token.
            // this is how the parser can backtrack.

            if (pushed.Any())
            {
                return pushed.Pop();
            }

            while (enumerator.MoveNext())
            {
                if (Skips.Contains(enumerator.Current.TokenCode) == false)
                    return enumerator.Current;
            }

            return new Token(TokenType.NoMoreTokens, "", 0, 0);
        }

        public void PushToken(Token Token)
        {
            pushed.Push(Token);
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

