using Steadsoft.Novus.Scanner.Enums;
using static Steadsoft.Novus.Scanner.Enums.TokenType;

namespace Steadsoft.Novus.Scanner.Classes
{
    public class TokenEnumerator 
    {
        private readonly IEnumerator<Token> enumerator;
        private readonly TokenType[] Skips;
        private readonly Stack<Token> pushed = new();
        public TokenEnumerator(IEnumerable<Token> Source, params TokenType[] Skips)
        {
            enumerator = Source.GetEnumerator();
            this.Skips = Skips;
        }

        public void VerifyExpectedToken(TokenType Type, out Token Token)
        {
            var token = GetNextToken();

            if (token.TokenType != Type)
                throw new InternalErrorException($"Expected token (token type) '{Type}' has not been pushed by caller!");

            Token = token;

            return;
        }

        public void VerifyExpectedToken(Keywords Keyword, out Token Token)
        {
            var token = GetNextToken();

            if (token.Keyword != Keyword)
                throw new InternalErrorException($"Expected token (keyword) '{Keyword}' has not been pushed by caller!");

            Token = token;

            return;
        }


        public bool NextTokensAre(params TokenType[] Tokens)
        {
            var list = PeekNextTokens(Tokens.Length).ToArray();

            for (int I = 0; I < Tokens.Length; I++)
            {
                if (Tokens[I] != list[I].TokenType)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Returns the next 'n' tokens in order, without consuming them from the input.
        /// </summary>
        /// <param name="N"></param>
        /// <returns></returns>
        public List<Token> PeekNextTokens(int N)
        {
            var list = new List<Token>();

            for (int I = 0; I < N; I++)
            {
                list.Add(GetNextToken());
            }

            PushTokens(list);

            return list;
        }

        /// <summary>
        /// Consume and returns the next token.
        /// </summary>
        /// <returns></returns>
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
                if (Skips.Contains(enumerator.Current.TokenType) == false)
                    return enumerator.Current;
            }

            return new Token(NoMoreTokens, "", 0, 0);
        }

        /// <summary>
        /// Returns the supplied token to the input, no check is made so be careful!
        /// </summary>
        /// <param name="Token"></param>
        public void PushToken(Token Token)
        {
            pushed.Push(Token);
        }

        public void PushTokens(List<Token> Tokens)
        {
            for (int I = Tokens.Count - 1; I >= 0; I--)
            {
                pushed.Push(Tokens[I]);
            }
        }

        public void SkipToNext(string Lexeme)
        {
            var token = GetNextToken();

            while (token.Lexeme != Lexeme && token.TokenType != NoMoreTokens)
            {
                token = GetNextToken();
            }
        }
    }
}