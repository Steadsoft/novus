using Steadsoft.Novus.Scanner.Enums;
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
        public static bool MethodDeclaration(TokenEnumerator<Keywords> TokenSource)
        {
            return TokenSource.NextTokensAre(LPar, Identifier, Identifier);
        }

        public static bool FunctionReturnType(TokenEnumerator<Keywords> TokenSource)
        {
            return TokenSource.NextTokensAre(LPar, Identifier, RPar);
        }

        public static bool FieldDeclaration(TokenEnumerator<Keywords> TokenSource)
        {
            var tokens = new List<Token>();

            var token = TokenSource.GetNextToken();

            while (token.TokenType != LBrace && token.TokenType != SemiColon)
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