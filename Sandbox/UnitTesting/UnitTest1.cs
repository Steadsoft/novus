using Microsoft.VisualStudio.TestTools.UnitTesting;
using Steadsoft.Novus.Parser.Classes;
using Steadsoft.Novus.Scanner.Enums;
using Steadsoft.Novus.Parser.Statements;
using Steadsoft.Novus.Scanner.Classes;

namespace UnitTesting
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestTokenizer_1()
        {
            Token token;

            var parser = Parser.CreateParser(SourceOrigin.Text, Source.ValidText_1, TokenDefinition.Resource, "novus.csv");

            var tokens = parser.TokenSource.PeekNextTokens(9);

            token = parser.TokenSource.GetNextToken();
            token = parser.TokenSource.GetNextToken();
            token = parser.TokenSource.GetNextToken();
            token = parser.TokenSource.GetNextToken();
            token = parser.TokenSource.GetNextToken();
            token = parser.TokenSource.GetNextToken();
            token = parser.TokenSource.GetNextToken();
            token = parser.TokenSource.GetNextToken();
            token = parser.TokenSource.GetNextToken();

            token = parser.TokenSource.GetNextToken();

            Assert.IsTrue(token.Keyword == Keywords.Type);
        }

        [TestMethod]
        public void TestTokenizer_2()
        {
            Token token;

            var parser = Parser.CreateParser(SourceOrigin.Text, Source.DelimiterTest_1, TokenDefinition.Resource, "novus.csv");

            var tokens = parser.TokenSource.PeekNextTokens(2);


            Assert.IsTrue(tokens[0].TokenType == TokenType.QString && tokens[0].Lexeme == "lkfhjsdkfhsdkfghsdkgfhdfkg");
            Assert.IsTrue(tokens[1].TokenType == TokenType.QString && tokens[1].Lexeme == "jhdkjhdkajshdkajshdkkhsdhd");

        }

        [TestMethod]
        public void TestTokenizer_3()
        {
            Token token;

            var parser = Parser.CreateParser(SourceOrigin.Pathname, @"..\..\..\Sources\string_delimiter_test_1.nov", TokenDefinition.Resource, "novus.csv");

            var tokens = parser.TokenSource.PeekNextTokens(4);

            Assert.IsTrue(tokens[0].TokenType == TokenType.QString && tokens[0].Lexeme == @"lkfhj""dk""""""dkfghsdkgfhdfkg");
            Assert.IsTrue(tokens[1].TokenType == TokenType.QString && tokens[1].Lexeme == "jhdkjhd|||kajshdkajshdkkhsdhd");
            Assert.IsTrue(tokens[2].TokenType == TokenType.QString && tokens[2].Lexeme == @"lkfhj""dk^^^dkfghsdkgfhdfkg");
            Assert.IsTrue(tokens[3].TokenType == TokenType.QString && tokens[3].Lexeme == "jhdkjhd|||kajshdkajshdkkhsdhd");

        }

        [TestMethod]
        public void TestParser_1()
        {
            var parser = Parser.CreateParser(SourceOrigin.Text, Source.ValidText_1, TokenDefinition.Resource, "novus.csv");

            parser.TrySyntaxPhase(out var root);

            Assert.IsTrue(root.Children.Count == 7);

            Assert.IsTrue(root.Children[0] is NamespaceDeclaration);
                Assert.IsTrue(((NamespaceDeclaration)(root.Children[0])).Children.Count == 1);

            Assert.IsTrue(root.Children[1] is NamespaceDeclaration);
                Assert.IsTrue(((NamespaceDeclaration)(root.Children[1])).Children.Count == 1);

            Assert.IsTrue(root.Children[2] is NamespaceDeclaration);
            Assert.IsTrue(root.Children[3] is NamespaceDeclaration);
            Assert.IsTrue(root.Children[4] is TypeDeclaration);
            Assert.IsTrue(root.Children[5] is TypeDeclaration);

            Assert.IsTrue(root.Children[6] is TypeDeclaration);
                Assert.IsTrue(((TypeDeclaration)(root.Children[6])).Block.Children.Count == 1);
        }
    }
}