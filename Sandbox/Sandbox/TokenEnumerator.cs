using System.Collections.Generic;

namespace Sandbox
{
    public class TokenEnumerator
    {
        private IEnumerator<Token> enumerator;

        public TokenEnumerator(IEnumerable<Token> Source)
        {
            enumerator = Source.GetEnumerator();
        }

        public Token GetNextToken()
        {
            while (enumerator.MoveNext())
            {
                return enumerator.Current;
            }

            return new Token(TokenType.NoMoreTokens, "", 0, 0);
        }
    }
}

