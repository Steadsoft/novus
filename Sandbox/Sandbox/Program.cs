using System;
using System.Collections;
using System.Linq;
using static Sandbox.CharSupport;

namespace Sandbox
{
    // Just a sandbox for a lexer
    internal partial class Program
    {
        static void Main(string[] args)
        {
            var source = SourceFile.Create(@"..\..\..\TestFiles\test1.nov");

            /* this is just a test */
            int unused;

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

            //FunctionTable<State, Char, Func<Character, Action>> map = new();

            //map.Add(State.INITIAL, Alpha, (a) => { return new Action(Step.AppendResume, State.IDENTIFIER); });
            //map.Add(State.INITIAL, Digit, (a) => { return new Action(Step.AppendResume, State.INTEGER); });

            //var f = map[State.INITIAL, 'd']; // (new Character('a',0,0));

            // use unicode APL characters to represent source character classes.

            // first is 0x2336 and the last is 237A
        }
    }
}