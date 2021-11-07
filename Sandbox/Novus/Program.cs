using Steadsoft.Novus.Parser;
using Steadsoft.Novus.Scanner;
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
        private static string parse_types_tests = @"..\..\..\TestFiles\parse_types_tests.nov";

        static void Main(string[] args)
        {
            var parser = Parser.CreateParser(SourceText.Pathname, parse_types_tests, TokenDefinition.Pathname, "novus.csv");

            parser.OnDiagnostic += MsgHandler;

            parser.TryParse(out var root);

            DumpParseTree(root);
        }

        private static void MsgHandler(object Sender, DiagnosticEventArgs Args)
        {
            Console.WriteLine(Args.Message);
        }

        private static void DumpParseTree(BlockStatement Root)
        {
            foreach (var stmt in Root.Children)
            {
                string text = stmt.Dump(0);
                Console.Write(text);
            }
        }
    }
}