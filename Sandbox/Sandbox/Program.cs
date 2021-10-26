﻿using System;
using System.Collections;
using System.Linq;

namespace Sandbox
{
    internal partial class Program
    {
        static void Main(string[] args)
        {
            var source = SourceFile.Create(@"..\..\..\Program.cs");

            var tokenizer = new Tokenizer(source);

            foreach (var token in tokenizer.Tokens)
            {
                Console.WriteLine(token.Lexeme);
            }
        }
    }
}