using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Sandbox
{
    // Just a sandbox for a lexer
    internal partial class Program
    {
        static void Main(string[] args)
        {

            var source = SourceFile.Create(@"..\..\..\TestFiles\parse_test_1.nov");

            var tokenizer = new Tokenizer(@"..\..\..\TestFiles\csharp.csv");

            //Console.ForegroundColor = ConsoleColor.White;

            //foreach (var token in tokenizer.Tokenize(source))
            //{
            //    Console.Write($"[{token.LineNumber,-3} {token.ColNumber,-2}] {token.TokenCode,-12} '");
            //    var c = Console.ForegroundColor;
            //    Console.ForegroundColor = ConsoleColor.Green;
            //    Console.Write($"{token.Lexeme}");
            //    Console.ForegroundColor = c;
            //    Console.WriteLine("'");
            //}

            List<int> ints = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            TokenEnumerator te = new TokenEnumerator(tokenizer.Tokenize(source));

            Parse(te);

        }


        public static void Parse (TokenEnumerator source)
        {
            var token = source.GetNextToken();

            // We expect any of the following

            // using <qualified-name> ;
            // namespace <qualified-name> { }
            // one or more of: type <identifier> [<type-options>] { <type-body> }

            while (token != null)
            {
                if (token.TokenCode == TokenType.Identifier && token.Lexeme == "using")
                {
                    TryParseUsing(source, token, out var usingStatement);
                }

                if (token.TokenCode == TokenType.Identifier && token.Lexeme == "namespace")
                {
                    ParseNamespace(source);
                }

                if (token.TokenCode == TokenType.Identifier && token.Lexeme == "type")
                {
                    ParseType(source);
                }

                Console.WriteLine("Expected one of 'using', 'namespace' or 'type");

                token = source.GetNextToken();
            }
        }

        public static bool TryParseUsing(TokenEnumerator source, Token Prior, out UsingStatement Stmt)
        {
            Stmt = null;

            StringBuilder builder = new StringBuilder();

            // <ident>;
            // <ident>.
            // <ident>.<ident>;
            // <ident>.<ident>.
            // <ident>.<ident>.<ident>;

            var token = source.GetNextToken();

            while (token.TokenCode != TokenType.NoMoreTokens)
            {
                if (token.TokenCode != TokenType.Identifier)
                    return false;

                builder.Append(token.Lexeme);

                token = source.GetNextToken();

                if (token.Lexeme == ";")
                {
                    Stmt = new UsingStatement(Prior.LineNumber, Prior.ColNumber, builder.ToString());
                    return true;
                }

                if (token.Lexeme == ".")
                {
                    builder.Append(".");
                }

                token = source.GetNextToken();

            }

            return false;
        }

        public static void ParseNamespace(TokenEnumerator source)
        {
        }

        public static void ParseType(TokenEnumerator source)
        {
        }


    }
}

