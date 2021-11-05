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

            parser.OnDiagnostic += MsgHandler;

            parser.TryParseFile(te, out var root);

        }

        private static void MsgHandler(object Sender, DiagnosticEventArgs Args)
        {
            ;
        }




    }
}

