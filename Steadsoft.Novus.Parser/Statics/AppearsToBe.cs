using System.Collections.Generic;
using static Steadsoft.Novus.Scanner.Enums.TokenType;
using Steadsoft.Novus.Scanner.Classes;

namespace Steadsoft.Novus.Parser.Statics
{
    /// <summary>
    /// Set of functions that try to determine if the next few tokens are compatible with some grammatical non-terminal.
    /// </summary>
    public static class AppearsToBeA
    {
        public static bool MethodDeclaration(TokenEnumerator TokenSource)
        {
            return
                TokenSource.NextTokensAre(Identifier, SemiColon) ||
                TokenSource.NextTokensAre(Identifier, Lesser) ||
                TokenSource.NextTokensAre(Identifier, ParenOpen, Identifier, Identifier) || 
                TokenSource.NextTokensAre(Identifier, ParenOpen, Identifier, ParenClose);
        }

        //public static bool FunctionReturnType(TokenEnumerator TokenSource)
        //{
        //    return TokenSource.NextTokensAre(ParenOpen, Identifier, ParenClose);
        //}

        public static bool FieldDeclaration(TokenEnumerator TokenSource)
        {
            var tokens = new List<Token>();

            var token = TokenSource.GetNextToken();

            if (token.TokenType != Identifier)
                return false;

            token = TokenSource.GetNextToken();

            while (token.TokenType != BraceOpen && token.TokenType != SemiColon)
            {
                tokens.Add(token);
                token = TokenSource.GetNextToken();
            }

            if (token.TokenType == SemiColon)
            {
                TokenSource.PushToken(token);
                TokenSource.PushTokens(tokens);
                return true;
            }

            return false;  
        }
    }
}