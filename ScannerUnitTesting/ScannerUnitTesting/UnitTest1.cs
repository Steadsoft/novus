using Steadsoft.Novus.Scanner.Classes;
using Steadsoft.Novus.Scanner.Enums;
using Steadsoft.Novus.Scanner.Statics;
using System.Reflection;

namespace ScannerUnitTesting
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Test_IdentifierA()
        {
            var tokens = CreateEnumerator("im_a_name;");
            var token = tokens.GetNextToken();
            Assert.IsTrue(token.TokenType == TokenType.Identifier);
            token = tokens.GetNextToken();
            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_IdentifierB()
        {
            var tokens = CreateEnumerator("im_a_name; ");

            var token = tokens.GetNextToken();
            Assert.IsTrue(token.TokenType == TokenType.Identifier);
            token = tokens.GetNextToken();
            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }
        [TestMethod]
        public void Test_IdentifierC()
        {
            var tokens = CreateEnumerator("im_a_name ;");

            var token = tokens.GetNextToken();
            Assert.IsTrue(token.TokenType == TokenType.Identifier);
            token = tokens.GetNextToken();
            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericA()
        {
            var tokens = CreateEnumerator("01234546789;");

            var token = tokens.GetNextToken();
            Assert.IsTrue(token.TokenType == TokenType.Numeric);
            token = tokens.GetNextToken();
            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericB()
        {
            var tokens = CreateEnumerator("01234 546789;");

            var token = tokens.GetNextToken();
            Assert.IsTrue(token.TokenType == TokenType.Numeric && token.IsInvalid == false);
            token = tokens.GetNextToken();
            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericC()
        {
            var tokens = CreateEnumerator("01234_546789;");

            var token = tokens.GetNextToken();
            Assert.IsTrue(token.TokenType == TokenType.Numeric && token.IsInvalid == false);
            token = tokens.GetNextToken();
            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericD()
        {
            var tokens = CreateEnumerator("01234_ 546789;");

            var token = tokens.GetNextToken();
            Assert.IsTrue(token.TokenType == TokenType.Numeric && token.IsInvalid == true);
            token = tokens.GetNextToken();
            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        private TokenEnumerator CreateEnumerator(string Text)
        {
            var source = SourceFile.CreateFromText(Text);
            var tokenizer = new Tokenizer<Keywords>(@"..\..\..\CSV\hardcode.csv", TokenDefinition.Pathname, Assembly.GetExecutingAssembly()); // be able to supply a delegate that can sanity check tokens.
            return new TokenEnumerator(tokenizer, source, ValidateToken, TokenType.BlockComment, TokenType.LineComment);
        }

        private static void ValidateToken(Token token)
        {
            if (token.TokenType == TokenType.Hexadecimal)
            {
                if (token.Lexeme.Contains('_') && token.Lexeme.Contains(' '))
                {
                    token.ErrorText = "This literal must not contain more than one kind of separator character.";
                    token.IsInvalid = true;
                }

                if (token.Lexeme.Contains("  ") || token.Lexeme.Contains("__"))
                {
                    token.ErrorText = "This literal must not contain repetitions of a separator character.";
                    token.IsInvalid = true;
                }

                return;
            }

            if (token.TokenType == TokenType.Numeric)
            {
                token.Lexeme = token.Lexeme.Trim();

                if (token.Lexeme.Contains('_') && token.Lexeme.Contains(' '))
                {
                    token.ErrorText = "This literal must not contain more than one kind of separator character.";
                    token.IsInvalid = true;
                }

                if (token.Lexeme.Contains("  ") || token.Lexeme.Contains("__"))
                {
                    token.ErrorText = "This literal must not contain repetitions of a separator character.";
                    token.IsInvalid = true;
                }

                if (token.Lexeme.ContainsAny('a', 'b', 'c', 'd', 'e', 'f') || token.Lexeme.ContainsAny('A', 'B', 'C', 'D', 'E', 'F'))
                {
                    if (token.Lexeme.EndsWith(":h") || token.Lexeme.EndsWith(":H"))
                        ;
                    else
                    {
                        token.ErrorText = "A hexadecimal literal must end with the :h or :H type specifier.";
                        token.IsInvalid = true;
                    }
                }

                return;
            }
        }


    }
}