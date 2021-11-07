using System;
using System.Collections.Generic;
using System.Linq;

namespace Steadsoft.Novus.Scanner
{
    public class TokenEnumerator<T> where T : struct, System.Enum, IComparable<T>
    {
        private IEnumerator<Token<T>> enumerator;
        private TokenType[] Skips;
        private Stack<Token<T>> pushed = new Stack<Token<T>>();
        public TokenEnumerator(IEnumerable<Token<T>> Source, params TokenType[] Skips)
        {
            enumerator = Source.GetEnumerator();
            this.Skips = Skips;
        }

        public void CheckExpectedToken(T Type)
        {
            var token = GetNextToken();

            if (Convert.ToInt32(token.Keyword) != Convert.ToInt32(Type))
                throw new InternalErrorException($"Expected keyword token '{Type}' has not been pushed.");

            return;
        }

        public Token<T> GetNextToken()
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

            return new Token<T>(TokenType.NoMoreTokens, "", 0, 0);
        }

        public void PushToken(Token<T> Token)
        {
            pushed.Push(Token);
        }

        public Token<T> SkipToNext(string Lexeme)
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

