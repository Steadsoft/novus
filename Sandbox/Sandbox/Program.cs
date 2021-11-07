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
            string TEXT =
                @"
                namespace a.x.rere
                {
                   namespace b
                   {
                      namespace namespace
                      {
                         type Category class sealed sealed abstract static public private
                         {
                            type Window class public abstract sealed
                            {

                            }
                         }
                      }
                   } 
                }

                namespace d
                {
                   type Umbrealla class
                   {

                   }
                }

                namespace abc;

                // using Steadsoft.Novus.Support;

                namespace Steadsoft.Novus.Support
                {
                   namespace Steadsoft.Novus.Compiler
                   {

                   }
                }

                type Umbrealla class
                {

                }

                type Window class public abstract sealed
                {

                }

                type Category1 class sealed sealed abstract static
                {
                type Category2 class sealed sealed abstract static
                {
                type Category3 class sealed sealed abstract static
                {
                type Category4 class sealed sealed abstract static
                {

                }
                }
                }
                }
                ";

            var parser = new Parser(TEXT, false);
            //var parser = new Parser(parse_types_tests, true);

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

