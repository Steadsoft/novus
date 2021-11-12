using Steadsoft.Novus.Parser.Classes;
using Steadsoft.Novus.Parser.Statements;
using Steadsoft.Novus.Scanner.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sandbox
{
    // Just a sandbox for a lexer
    internal partial class Program
    {
        private static readonly string parse_types_tests = @"..\..\..\TestFiles\parse_types_tests.nov";
        private static List<DiagnosticEventArgs> messages = new List<DiagnosticEventArgs>();

        static void Main(string[] args)
        {
            // When we create a parser we can get supply the source text to be parsed as either a simple string 
            // or as a pathname to a source file. Likewise we can supply the token defintion CSV as either a pathname
            // to a file or the name of an embedded resource.

            var parser = Parser.CreateParser(SourceOrigin.Pathname, parse_types_tests, TokenDefinition.Resource, "novus.csv");

            parser.OnDiagnostic += MsgHandler;

            parser.TrySyntaxPhase(out var root);

            parser.TrySemanticPhase(ref root);

            foreach (var msg in messages.OrderBy(m => m.Line))
            {
                Console.WriteLine(msg.Message);
            }

            Console.WriteLine();

            DumpParseTree(root);
        }

        private static void MsgHandler(object Sender, DiagnosticEventArgs Args)
        {
            messages.Add(Args);
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