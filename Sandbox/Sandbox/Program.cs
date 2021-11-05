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

            var parser = new Parser(te);

            parser.ParseFile(te);

        }

        public static string ParsedGood(Statement Stmt)
        {
            return $"Parsed a {Stmt.GetType().Name} on line {Stmt.Line} at column {Stmt.Col} : '{Stmt.ToString()}'";
        }

        public static string ParsedBad(Statement Stmt, string Msg)
        {
            return $" Failed to parse a {Stmt.GetType().Name} on line {Stmt.Line} at column {Stmt.Col} ({Msg})";
        }



    }
}

