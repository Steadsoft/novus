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
            var tokens = new TokenEnumerator(tokenizer, source, TokenType.BlockComment, TokenType.LineComment);

            var t = tokens.GetNextToken();

            ValidateToken(t);

            while (t.TokenType != TokenType.NoMoreTokens) 
            { 
                tokes.Add(t);

                t = tokens.GetNextToken();

                ValidateToken(t);

            }
        }

        public static void ValidateToken(Token token) 
        { 
            if (token.TokenType == TokenType.Hexadecimal)
            {
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

                return;
            }

            if (token.TokenType == TokenType.Numeric)
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

                if (token.Lexeme.ContainsAny('a','b','c','d','e','f') || token.Lexeme.ContainsAny('A', 'B', 'C', 'D', 'E', 'F'))
                {
                    if (token.Lexeme.EndsWith(":h") || token.Lexeme.EndsWith(":H"))
                        ;
                    else
                    {
                        token.ErrorText = "A hexadecimal literal must end with the :h or :H type specifier.";
                        token.IsInvalid = true;
                    }
                }

                return;
            }
        }
    }
}