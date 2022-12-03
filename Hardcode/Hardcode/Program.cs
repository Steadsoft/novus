using Steadsoft.Novus.Scanner.Classes;
using Steadsoft.Novus.Scanner.Enums;
using System.Diagnostics;
using System.Reflection;
using System.Linq;
using System.Net.Http.Headers;
using Steadsoft.Novus.Scanner.Statics;

namespace Hardcode
{
    internal class Program
    {
        static void Main(string[] args)
        {
            double a = 12_345_678.11;
            double b = 12_3_45__6_78.11;

            List<Token> tokes = new List<Token>();

            var source = SourceFile.CreateFromFile(@"..\..\..\tokens_test_02.hcl");

            var tokenizer = new Tokenizer<Keywords>(@"..\..\..\hardcode.csv", TokenDefinition.Pathname, Assembly.GetExecutingAssembly()); // be able to supply a delegate that can sanity check tokens.
            var tokens = new TokenEnumerator(tokenizer, source, ValidateToken, TokenType.BlockComment, TokenType.LineComment);

            var t = tokens.GetNextToken(true);

            while (t.TokenType != TokenType.NoMoreTokens) 
            { 
                tokes.Add(t);

                t = tokens.GetNextToken(true);

            }

            var goods = tokes.Where(t => t.IsInvalid == false).ToList();
            var fails = tokes.Where(t => t.IsInvalid ).ToList();
        }

        private static void ValidateToken(TokenEnumerator tokens, Token token)
        {
            if (token.TokenType == TokenType.CR)
            {
                var next = tokens.PeekNextToken();

                if (next.TokenType == TokenType.LF)
                {
                    var discard = tokens.GetNextToken(); // just duiscard this because we've simply hit a Windows CR/LF pair.

                    token.Lexeme = "\r\n";
                    token.TokenType = TokenType.NewLine;
                }
            }

            if (token.TokenType == TokenType.Identifier && token.Lexeme.Contains(' '))
            {
                // The token structure allows spaces inside literals, therefore identifier recognition can 
                // only terminate when some other terminator character is encountered. If the terminator
                // does not indicate a numeric (i.e. hex) literal with trailing number base then the entire
                // concatenation of identifiers is returned. We therefore split that into several new tokens and  
                // push them onto the enumerator stack. The calling code has no idea and will simply see
                // several distinct and valid identifiers tokens.

                var idents = token.Lexeme.Split(' ', StringSplitOptions.RemoveEmptyEntries).Reverse(); // we're going to stack these so order is very important

                foreach (var ident in idents)
                {
                    tokens.PushToken(new Token(TokenType.Identifier, ident, token.LineNumber, 0));
                }

                var temp = tokens.GetNextToken();

                token.Lexeme = temp.Lexeme;
                token.TokenType = temp.TokenType;
            }

            if (token.TokenType == TokenType.NumericLiteral)
            {
                token.Lexeme = token.Lexeme.Trim();

                if (token.Lexeme.Contains('_') && token.Lexeme.Contains(' '))
                {
                    token.ErrorText = "A numeric literal must not contain more than one kind of separator character.";
                    token.IsInvalid = true;
                    return;
                }

                if (token.Lexeme.Contains("  ") || token.Lexeme.Contains("__"))
                {
                    token.ErrorText = "A numeric literal must not contain repetitions of a separator character.";
                    token.IsInvalid = true;
                    return;
                }

                if (token.Lexeme.Count(f => (f == '.')) > 1) // the FSM should prevent this, but we'll check this anyway.
                {
                    token.ErrorText = "A numeric literal must not contain more than one decimal point.";
                    token.IsInvalid = true;
                    return;
                }

                token.Lexeme = token.Lexeme.Replace(" ", "").Replace("_", "");

                if (token.Lexeme.ToUpper().EndsWith(":H"))
                {
                    if (StripBaseIndicator(token.Lexeme, 'H').All(".0123456789ABCDEF".Contains) == false)
                    {
                        token.ErrorText = "This hexadecimal literal contains one or more invalid characters.";
                        token.IsInvalid = true;
                        return;
                    }

                    if (token.Lexeme.Any("abcdef".Contains) && token.Lexeme.Any("ABCDEF".Contains))
                    {
                        token.ErrorText = "A hexadecimal literal must not contain both uppercase and lowercase letters.";
                        token.IsInvalid = true;
                        return;
                    }
                }

                if (token.Lexeme.ToUpper().EndsWith(":D"))
                {
                    if (StripBaseIndicator(token.Lexeme, 'D').All(".0123456789".Contains) == false)
                    {
                        token.ErrorText = "This decimal literal contains one or more invalid characters.";
                        token.IsInvalid = true;
                        return;
                    }
                }

                if (token.Lexeme.ToUpper().EndsWith(":O"))
                {
                    if (StripBaseIndicator(token.Lexeme, 'O').All(".01234567".Contains) == false)
                    {
                        token.ErrorText = "This octal literal contains one or more invalid characters.";
                        token.IsInvalid = true;
                        return;
                    }
                }

                if (token.Lexeme.ToUpper().EndsWith(":B"))
                {
                    if (StripBaseIndicator(token.Lexeme, 'B').All(".01".Contains) == false)
                    {
                        token.ErrorText = "This binary literal contains one or more invalid characters.";
                        token.IsInvalid = true;
                        return;
                    }
                }

                if (token.Lexeme.ToUpper().EndsWithAny(":B", ":O", ":D", ":H") == false)
                {
                    if (token.Lexeme.All(".0123456789".Contains) == false)
                    {
                        token.ErrorText = "This non-decimal literal does not end with a valid base indicator";
                        token.IsInvalid = true;
                        return;
                    }
                }


                return;

                string StripBaseIndicator(string str, char basechar)
                {
                    return str.ToUpper().TrimEnd(basechar).TrimEnd(':');
                }
            }
        }
    }
}