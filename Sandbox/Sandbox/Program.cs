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

            TokenEnumerator te = new TokenEnumerator(tokenizer.Tokenize(source), TokenType.BlockComment, TokenType.LineComment);

            ParseFile(te);

        }

        public static string ParsedGood(Statement Stmt)
        {
            return $"Parsed a {Stmt.GetType().Name} on line {Stmt.Line} at column {Stmt.Col} : '{Stmt.ToString()}'";
        }

        public static string ParsedBad(Statement Stmt, string Msg)
        {
            return $" Failed to parse a {Stmt.GetType().Name} on line {Stmt.Line} at column {Stmt.Col} ({Msg})";
        }

        public static void ParseFile(TokenEnumerator source)
        {
            string message;

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
                        if (TryParseUsing(source, token, out var usingStatement, out message))
                        {
                            Console.WriteLine(ParsedGood(usingStatement));
                        }
                        else
                        {
                            Console.WriteLine(ParsedBad(usingStatement, message));
                            token = source.SkipToNext(";");
                        }
                        token = source.GetNextToken();
                        continue;

                    case Keyword.Namespace:
                        if (TryParseNamespace(source, token, out var namespaceStatement, out message))
                        {
                            Console.WriteLine(ParsedGood(namespaceStatement));
                        }
                        else
                        {
                            Console.WriteLine(ParsedBad(namespaceStatement, message));
                            token = source.SkipToNext(";");
                        }
                        token = source.GetNextToken();
                        continue;

                    case Keyword.Type:
                        if (TryParseType(source, token, out var typeStatement, out message))
                        {
                            Console.WriteLine(ParsedGood(typeStatement));
                        }
                        else
                        {
                            Console.WriteLine(ParsedBad(typeStatement, message));
                            token = source.SkipToNext("}");
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

        public static bool TryParseUsing(TokenEnumerator source, Token Prior, out UsingStatement Stmt, out string DiagMsg)
        {
            Stmt = null;
            DiagMsg = null;

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
                    DiagMsg = $"Unexpected token {token.Lexeme}";
                    source.PushToken(token);
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

        public static bool TryParseNamespace(TokenEnumerator source, Token Prior, out NamespaceStatement Stmt, out string DiagMsg)
        {
            Stmt = null;
            DiagMsg = String.Empty;

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
                    DiagMsg = $"Unexpected token {token.Lexeme}";
                    source.PushToken(token);
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
                    if (TryParseNamespaceBlock(source, out var block, out DiagMsg))
                       {
                       Stmt = new NamespaceStatement (block, Prior.LineNumber, Prior.ColNumber, builder.ToString());
                       }
                    
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

        public static bool TryParseNamespaceBlock(TokenEnumerator source, out BlockStatement Block, out string DiagMsg)
        {
            Block = null;
            DiagMsg = string.Empty;

            var token = source.GetNextToken();

            while (token.TokenCode != TokenType.NoMoreTokens && token.TokenCode != TokenType.RBrace)
            {
                switch (token.Keyword)
                {
                    case Keyword.Namespace:
                        if (TryParseNamespace(source, token, out var namespaceStatement, out DiagMsg))
                        {
                            ; // Console.WriteLine(ParsedGood(namespaceStatement));
                        }
                        else
                        {
                            Console.WriteLine(ParsedBad(namespaceStatement, DiagMsg));
                            token = source.SkipToNext(";");
                        }
                        token = source.GetNextToken();
                        continue;

                    case Keyword.Type:
                        if (TryParseType(source, token, out var typeStatement, out DiagMsg))
                        {
                            ; // Console.WriteLine(ParsedGood(typeStatement));
                        }
                        else
                        {
                            Console.WriteLine(ParsedBad(typeStatement, DiagMsg));
                            token = source.SkipToNext("}");
                        }
                        token = source.GetNextToken();
                        continue;

                    default:
                        Console.WriteLine("Unexpected token {} found.");
                        break;
                }
            }

            return true;

        }

        public static bool TryParseRecord(TokenEnumerator source, ref TypeStatement Stmt, out string DiagMsg)
        {
            DiagMsg = String.Empty;

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
                    case Keyword.Readonly:
                        {
                            Stmt.IsReadOnly = true;
                            break;
                        }
                    case Keyword.Private when Stmt.AccessType == AccessType.Undefined: 
                        {
                            Stmt.AccessType = AccessType.Private;
                            break;
                        }
                    case Keyword.Internal when Stmt.AccessType == AccessType.Undefined:
                        {
                            Stmt.AccessType = AccessType.Internal;
                            break;
                        }
                    case Keyword.Public when Stmt.AccessType == AccessType.Undefined:
                        {
                            Stmt.AccessType = AccessType.Public;
                            break;
                        }
                    case Keyword.Private when Stmt.AccessType != AccessType.Undefined:
                    case Keyword.Internal when Stmt.AccessType != AccessType.Undefined:
                    case Keyword.Public when Stmt.AccessType != AccessType.Undefined:
                        {
                            DiagMsg = $"Cannot specify access type '{token.Keyword.ToString().ToLower()}' when access is already '{Stmt.AccessType.ToString().ToLower()}'";
                            return false;
                        }
                    default:
                        {
                            DiagMsg = $"Unexpected token '{token.Lexeme}'";
                            return false;
                        }
                }

                token = source.GetNextToken();
            }

            source.PushToken(token);

            if (Stmt.IsRecordClass && Stmt.IsRecordStruct)
                return false;

            if (Stmt.AccessType == AccessType.Undefined)
                Stmt.AccessType = AccessType.Internal;

            return true;
        }

        public static bool TryParseType(TokenEnumerator source, Token Prior, out TypeStatement Stmt, out string DiagMsg)
        {
            Stmt = null;
            DiagMsg = string.Empty;

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
                    DiagMsg = $"Unexpected token {token.Lexeme}";
                    source.PushToken(token);
                    return false;
                }

                name = token.Lexeme;

                token = source.GetNextToken();

                if (token.Keyword != Keyword.Class && token.Keyword != Keyword.Struct && token.Keyword != Keyword.Record && token.Keyword != Keyword.Singlet)
                {
                    DiagMsg = $"Unexpected token {token.Lexeme}";
                    source.PushToken(token);
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
                            if (TryParseRecord(source, ref Stmt, out DiagMsg) == false)
                            {
                                //source.SkipToNext("}");
                                return false;
                            }
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

