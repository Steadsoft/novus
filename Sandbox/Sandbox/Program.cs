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

        public static string ParsedGood(Statement Stmt)
        {
            return $"Parsed a {Stmt.GetType().Name} on line {Stmt.Line} at column {Stmt.Col} : '{Stmt.ToString()}'";
        }

        public static string ParsedBad(Statement Stmt)
        {
            return $" Failed to parse a {Stmt.GetType().Name} on line {Stmt.Line} at column {Stmt.Col}";
        }

        public static void Parse(TokenEnumerator source)
        {
            var token = source.GetNextToken();

            // We expect any of the following

            // using <qualified-name> ;
            // namespace <qualified-name> { }
            // one or more of: type <identifier>  [<type-options>] { <type-body> }

            while (token.TokenCode != TokenType.NoMoreTokens)
            {
                switch (token.Keyword)
                {
                    case Keyword.Using:
                        if (TryParseUsing(source, token, out var usingStatement))
                        {
                            Console.WriteLine(ParsedGood(usingStatement));
                        }
                        else
                        {
                            Console.WriteLine(ParsedBad(usingStatement));
                            token = source.SkipToNext(";");
                        }
                        token = source.GetNextToken();
                        continue;

                    case Keyword.Namespace:
                        if (TryParseNamespace(source, token, out var namespaceStatement))
                        {
                            Console.WriteLine(ParsedGood(namespaceStatement));
                        }
                        else
                        {
                            Console.WriteLine(ParsedBad(namespaceStatement));
                            token = source.SkipToNext(";");
                        }
                        token = source.GetNextToken();
                        continue;

                    case Keyword.Type:
                        if (TryParseType(source, token, out var typeStatement))
                        {
                            Console.WriteLine(ParsedGood(typeStatement));
                        }
                        else
                        {
                            Console.WriteLine(ParsedBad(typeStatement));
                            token = source.SkipToNext(";");
                        }
                        token = source.GetNextToken();
                        continue;

                    default:
                        Console.WriteLine("Unexpected token {} found.");
                        break;
                }

                Console.WriteLine("Expected one of 'using', 'namespace' or 'type");

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
                {
                    Console.Write($"Unexpected token {token.Lexeme}");
                    source.StepBackwards(token);
                    return false;
                }

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

        public static bool TryParseNamespace(TokenEnumerator source, Token Prior, out NamespaceStatement Stmt)
        {
            Stmt = null;

            StringBuilder builder = new StringBuilder();

            // <ident>; or <ident>{
            // <ident>.
            // <ident>.<ident>; or <ident>.<ident>{
            // <ident>.<ident>.
            // <ident>.<ident>.<ident>; or <ident>.<ident>.<ident>{

            var token = source.GetNextToken();

            while (token.TokenCode != TokenType.NoMoreTokens)
            {
                if (token.TokenCode != TokenType.Identifier)
                {
                    Console.Write($"Unexpected token {token.Lexeme}");
                    source.StepBackwards(token);
                    return false;
                }

                builder.Append(token.Lexeme);

                token = source.GetNextToken();

                if (token.Lexeme == ";")
                {
                    Stmt = new NamespaceStatement(Prior.LineNumber, Prior.ColNumber, builder.ToString());
                    return true;
                }

                if (token.Lexeme == "{")
                {
                    Stmt = new NamespaceStatement (new BlockStatement(),Prior.LineNumber, Prior.ColNumber, builder.ToString());
                    // TryParseBlock
                    source.SkipToNext("}"); // very crude hack, just for now, to get progress through the file
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

        public static bool TryParseRecord(TokenEnumerator source, ref TypeStatement Stmt)
        {
            var token = source.GetNextToken();

            while (token.Lexeme != "{")
            {
                switch (token.Keyword)
                {
                    case Keyword.Class:
                        {
                            Stmt.IsRecordClass = true;
                            break;
                        }

                    case Keyword.Struct:
                        {
                            Stmt.IsRecordStruct = true;
                            break;
                        }
                }

                token = source.GetNextToken();
            }

            source.StepBackwards(token);

            if (Stmt.IsRecordClass && Stmt.IsRecordStruct)
                return false;

            return true;
        }

        public static bool TryParseType(TokenEnumerator source, Token Prior, out TypeStatement Stmt)
        {
            Stmt = null;
            string name;
            Keyword typeKind = Keyword.IsNotKeyword;

            // We expect any of the following

            // type <identifier> [<type-options>] { }


            // <ident>; or <ident>{
            // <ident>.
            // <ident>.<ident>; or <ident>.<ident>{
            // <ident>.<ident>.
            // <ident>.<ident>.<ident>; or <ident>.<ident>.<ident>{

            var token = source.GetNextToken();

            while (token.TokenCode != TokenType.NoMoreTokens)
            {
                if (token.TokenCode != TokenType.Identifier)
                {
                    Console.Write($"Unexpected token {token.Lexeme}");
                    source.StepBackwards(token);
                    return false;
                }

                name = token.Lexeme;

                token = source.GetNextToken();

                if (token.Keyword != Keyword.Class && token.Keyword != Keyword.Struct && token.Keyword != Keyword.Record && token.Keyword != Keyword.Singlet)
                {
                    Console.Write($"Unexpected token {token.Lexeme}");
                    source.StepBackwards(token);
                    return false;
                }

                typeKind = token.Keyword;

                Stmt = new TypeStatement(new TypeBody(), typeKind, Prior.LineNumber, Prior.ColNumber, name);

                switch (typeKind)
                {
                    case Keyword.Class:
                        {
                            //TryParseClass();
                            break; // parse class options
                        }
                    case Keyword.Struct:
                        {
                            break; // parse struct options
                        }
                    case Keyword.Record:
                        {
                            TryParseRecord(source, ref Stmt);
                            break; // parse record options
                        }
                    case Keyword.Singlet:
                        {
                            break; // parse singlet options
                        }

                }

                token = source.GetNextToken();

                if (token.Lexeme == "{")
                {
                    // TryParseBlock
                    source.SkipToNext("}"); // very crude hack, just for now, to get progress through the file
                    return true;
                }


                token = source.GetNextToken();

            }

            return false;

        }


    }
}

