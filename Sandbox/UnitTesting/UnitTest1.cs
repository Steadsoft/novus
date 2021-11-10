using Microsoft.VisualStudio.TestTools.UnitTesting;
using Steadsoft.Novus.Parser;
using Steadsoft.Novus.Scanner;

namespace UnitTesting
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestTokenizer_1()
        {
            Token<NovusKeywords> token;

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

            Assert.IsTrue(token.Keyword == NovusKeywords.Type);
        }
        [TestMethod]
        public void TestParser_1()
        {
            var parser = Parser.CreateParser(SourceOrigin.Text, Source.ValidText_1, TokenDefinition.Resource, "novus.csv");

            parser.TrySyntaxPhase(out var root);

            Assert.IsTrue(root.Children.Count == 7);

            Assert.IsTrue(root.Children[0] is DclNamespaceStatement);
                Assert.IsTrue(((DclNamespaceStatement)(root.Children[0])).Block.Children.Count == 1);

            Assert.IsTrue(root.Children[1] is DclNamespaceStatement);
                Assert.IsTrue(((DclNamespaceStatement)(root.Children[1])).Block.Children.Count == 1);

            Assert.IsTrue(root.Children[2] is DclNamespaceStatement);
            Assert.IsTrue(root.Children[3] is DclNamespaceStatement);
            Assert.IsTrue(root.Children[4] is DclTypeStatement);
            Assert.IsTrue(root.Children[5] is DclTypeStatement);

            Assert.IsTrue(root.Children[6] is DclTypeStatement);
                Assert.IsTrue(((DclTypeStatement)(root.Children[6])).Block.Children.Count == 1);
        }
    }
}