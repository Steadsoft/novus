﻿using System;

namespace Sandbox
{
    // Just a sandbox for a lexer
    internal partial class Program
    {
        static void Main(string[] args)
        {
            var source = SourceFile.Create(@"..\..\..\TestFiles\test1.nov");

            /* this is just a test */

            var tokenizer = new Tokenizer(source);

            foreach (var token in tokenizer.Tokens)
            {
                Console.Write($"[{token.LineNumber,-3} {token.ColNumber,-2}] {token.TokenCode,-12} '");
                var c = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"{token.Lexeme}");
                Console.ForegroundColor = c;
                Console.WriteLine("'");
            }

        }
    }
}