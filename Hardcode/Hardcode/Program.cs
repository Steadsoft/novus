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

            var source = SourceFile.CreateFromFile(@"..\..\..\tokens_test_01.hcl");

            var tokenizer = new Tokenizer<Keywords>(@"..\..\..\hardcode.csv", TokenDefinition.Pathname, Assembly.GetExecutingAssembly()); // be able to supply a delegate that can sanity check tokens.
            var tokens = new TokenEnumerator(tokenizer, source, ValidateToken, TokenType.BlockComment, TokenType.LineComment);

            var t = tokens.GetNextToken();

            ValidateToken(t);

            while (t.TokenType != TokenType.NoMoreTokens) 
            { 
                tokes.Add(t);

                t = tokens.GetNextToken();

                ValidateToken(t);

            }
        }

        private static void ValidateToken(Token token)
        {

            if (token.TokenType == TokenType.NumericLiteral)
            {
                token.Lexeme = token.Lexeme.Trim();

                if (token.Lexeme.Contains('_') && token.Lexeme.Contains(' '))
                {
                    token.ErrorText = "This literal must not contain more than one kind of separator character.";
                    token.IsInvalid = true;
                }

                if (token.Lexeme.Contains("  ") || token.Lexeme.Contains("__"))
                {
                    token.ErrorText = "This literal must not contain repetitions of a separator character.";
                    token.IsInvalid = true;
                }

                token.Lexeme = token.Lexeme.Replace(" ", "").Replace("_", "");

                if (token.Lexeme.ToUpper().EndsWith(":H"))
                {
                    if (token.Lexeme.ToUpper().TrimEnd('H').TrimEnd(':').All(".0123456789ABCDEF".Contains) == false)
                    {
                        token.ErrorText = "This hexadecimal literal contains one or more invalid characters.";
                        token.IsInvalid = true;
                    }
                }

                if (token.Lexeme.ToUpper().EndsWith(":D"))
                {
                    if (token.Lexeme.Replace(" ", "").Replace("_", "").ToUpper().TrimEnd('D', ':').All(".0123456789".Contains) == false)
                    {
                        token.ErrorText = "This decimal literal contains one or more invalid characters.";
                        token.IsInvalid = true;
                    }
                }

                if (token.Lexeme.ToUpper().EndsWith(":O"))
                {
                    if (token.Lexeme.ToUpper().All("_ 01234567".Contains) == false)
                    {
                        token.ErrorText = "This octal literal contains one or more invalid characters.";
                        token.IsInvalid = true;
                    }
                }

                if (token.Lexeme.ToUpper().EndsWith(":B"))
                {
                    if (token.Lexeme.ToUpper().All("_ 01".Contains) == false)
                    {
                        token.ErrorText = "This binary literal contains one or more invalid characters.";
                        token.IsInvalid = true;
                    }
                }


                return;
            }
        }
    }
}