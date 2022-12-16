using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using static System.Net.Mime.MediaTypeNames;

namespace AntlrCSharp
{
    internal class Program
    {
        private static string text =
            $@"call = 1234{Environment.NewLine}ball = 1234{Environment.NewLine}";

        static void Main(string[] args)
        {
            //
            //TextReader source = File.OpenText(@"..\..\..\..\..\Antlr\test1.nr");

            TextReader source = File.OpenText(@"..\..\..\..\..\Antlr\test_3.nr");

            AntlrInputStream inputStream = new AntlrInputStream(source);
            noresLexer noresLexer = new noresLexer(inputStream);

            noresLexer.langcode = "en"; // set as needed.

            CommonTokenStream commonTokenStream = new CommonTokenStream(noresLexer);
            noresParser noresParser = new noresParser(commonTokenStream);

            noresParser.BuildParseTree = true;

            var tree = noresParser.prog();

            var listener = new TestListener();

            ParseTreeWalker walker = new ParseTreeWalker();

            walker.Walk(listener, tree);

            Console.WriteLine(tree.ToStringTree());
        }
    }
}