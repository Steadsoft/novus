using System;
using System.Collections;
using System.Linq;

namespace Sandbox
{
    // Just a sandbox for a lexer
    internal partial class Program
    {
        static void Main(string[] args)
        {
            var source = SourceFile.Create(@"..\..\..\TestFiles\test1.nov");

            /* this is just a test */        int unused;      

            var tokenizer = new Tokenizer(source);

            foreach (var token in tokenizer.Tokens)
            {
                Console.Write($"Token ({token.TokenCode.ToString().PadRight(10)}) - Text '");
                var c = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"{token.Lexeme}");
                Console.ForegroundColor = c;
                Console.WriteLine("'");
            }
        }
    }
}