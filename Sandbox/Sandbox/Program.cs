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
            var source = SourceFile.Create(@"..\..\..\Tokenizer.cs");

            /* this is just a test */        int unused;      

            var tokenizer = new Tokenizer(source);

            foreach (var token in tokenizer.Tokens)
            {
                Console.WriteLine(token.Lexeme);
            }
        }
    }
}