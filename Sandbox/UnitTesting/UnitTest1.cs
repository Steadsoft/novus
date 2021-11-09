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
            var parser = Parser.CreateParser(SourceOrigin.Text, Source.ValidText_1, TokenDefinition.Resource, "novus.csv");

            var tokens = parser.TokenSource.GetNextTokens(9);

            parser.TokenSource.PushTokens(tokens);
            parser.TokenSource.GetNextTokens(9);

            var token = parser.TokenSource.GetNextToken();

            Assert.IsTrue(token.Keyword == NovusKeywords.Type);
        }
        [TestMethod]
        public void TestParser_1()
        {
            var parser = Parser.CreateParser(SourceOrigin.Text, Source.ValidText_1, TokenDefinition.Resource, "novus.csv");

            parser.TryParse(out var root);

            Assert.IsTrue(root.Children.Count == 7);

            Assert.IsTrue(root.Children[0] is NamespaceStatement);
                Assert.IsTrue(((NamespaceStatement)(root.Children[0])).Block.Children.Count == 1);

            Assert.IsTrue(root.Children[1] is NamespaceStatement);
                Assert.IsTrue(((NamespaceStatement)(root.Children[1])).Block.Children.Count == 1);

            Assert.IsTrue(root.Children[2] is NamespaceStatement);
            Assert.IsTrue(root.Children[3] is NamespaceStatement);
            Assert.IsTrue(root.Children[4] is TypeStatement);
            Assert.IsTrue(root.Children[5] is TypeStatement);

            Assert.IsTrue(root.Children[6] is TypeStatement);
                Assert.IsTrue(((TypeStatement)(root.Children[6])).Body.Children.Count == 1);
        }
    }
}