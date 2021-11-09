using Steadsoft.Novus.Scanner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steadsoft.Novus.Parser
{
    /// <summary>
    /// Set of functions that try to determine if the next few tokens are compatible with some grammatical non-terminal.
    /// </summary>
    public static class AppearsToBeA
    {
        public static bool MethodDeclaration(TokenEnumerator<NovusKeywords> TokenSource)
        {
            return TokenSource.NextTokensAre(TokenType.LPar, TokenType.Identifier, TokenType.Identifier);
        }

        public static bool FunctionReturnType(TokenEnumerator<NovusKeywords> TokenSource)
        {
            return TokenSource.NextTokensAre(TokenType.LPar, TokenType.Identifier, TokenType.RPar);
        }

        public static bool FieldDeclaration(TokenEnumerator<NovusKeywords> TokenSource)
        {
            var tokens = new List<Token<NovusKeywords>>();

            var token = TokenSource.GetNextToken();

            while (token.TokenCode != TokenType.LBrace && token.TokenCode != TokenType.SemiColon)
            {
                tokens.Add(token);
                token = TokenSource.GetNextToken();
            }

            if (token.TokenCode == TokenType.SemiColon)
            {
                TokenSource.PushToken(token);
                TokenSource.PushTokens(tokens);
                return true;
            }

            return false;
        }
    }
}