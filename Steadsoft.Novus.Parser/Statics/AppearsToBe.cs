using Steadsoft.Novus.Scanner;
using static Steadsoft.Novus.Scanner.TokenType;

namespace Steadsoft.Novus.Parser
{
    /// <summary>
    /// Set of functions that try to determine if the next few tokens are compatible with some grammatical non-terminal.
    /// </summary>
    public static class AppearsToBeA
    {
        public static bool MethodDeclaration(TokenEnumerator<NovusKeywords> TokenSource)
        {
            return TokenSource.NextTokensAre(LPar, Identifier, Identifier);
        }

        public static bool FunctionReturnType(TokenEnumerator<NovusKeywords> TokenSource)
        {
            return TokenSource.NextTokensAre(LPar, Identifier, RPar);
        }

        public static bool FieldDeclaration(TokenEnumerator<NovusKeywords> TokenSource)
        {
            var tokens = new List<Token<NovusKeywords>>();

            var token = TokenSource.GetNextToken();

            while (token.TokenCode != LBrace && token.TokenCode != SemiColon)
            {
                tokens.Add(token);
                token = TokenSource.GetNextToken();
            }

            if (token.TokenCode == SemiColon)
            {
                TokenSource.PushToken(token);
                TokenSource.PushTokens(tokens);
                return true;
            }

            return false;
        }
    }
}