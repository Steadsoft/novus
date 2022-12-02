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
            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral);
            token = tokens.GetNextToken();
            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericB()
        {
            var tokens = CreateEnumerator("01234 546789;");

            var token = tokens.GetNextToken();
            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == false);
            token = tokens.GetNextToken();
            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericC()
        {
            var tokens = CreateEnumerator("01234_546789;");

            var token = tokens.GetNextToken();
            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == false);
            token = tokens.GetNextToken();
            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericD()
        {
            var tokens = CreateEnumerator("01234_ 546789;");

            var token = tokens.GetNextToken();
            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == true);
            token = tokens.GetNextToken();
            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericE()
        {
            var tokens = CreateEnumerator("1:D;");

            var token = tokens.GetNextToken();
            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == false);
            token = tokens.GetNextToken();
            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericF()
        {
            var tokens = CreateEnumerator("1 22 333:D;");

            var token = tokens.GetNextToken();
            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == false);
            token = tokens.GetNextToken();
            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericG()
        {
            var tokens = CreateEnumerator("1 22 333:d;");

            var token = tokens.GetNextToken();
            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == false);
            token = tokens.GetNextToken();
            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericH()
        {
            var tokens = CreateEnumerator("1 22 333:b;");

            var token = tokens.GetNextToken();
            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == true);
            token = tokens.GetNextToken();
            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericI()
        {
            var tokens = CreateEnumerator("1 22.333:d;");

            var token = tokens.GetNextToken();
            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == false);
            Assert.IsTrue(token.Lexeme == "122.333:d");

            token = tokens.GetNextToken();
            Assert.IsTrue(token.TokenType == TokenType.SemiColon);


        }

        [TestMethod]
        public void Test_NumericJ()
        {
            var tokens = CreateEnumerator("1 2F.333:h;");

            var token = tokens.GetNextToken();
            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == false);
            Assert.IsTrue(token.Lexeme == "12F.333:h");

            token = tokens.GetNextToken();
            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericK()
        {
            var tokens = CreateEnumerator("FACE;");

            var token = tokens.GetNextToken();
            Assert.IsTrue(token.TokenType == TokenType.Identifier && token.IsInvalid == false);
            Assert.IsTrue(token.Lexeme == "FACE");

            token = tokens.GetNextToken();
            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericL()
        {
            var tokens = CreateEnumerator("F1CE;");

            var token = tokens.GetNextToken();
            Assert.IsTrue(token.TokenType == TokenType.Identifier && token.IsInvalid == false);
            Assert.IsTrue(token.Lexeme == "F1CE");

            token = tokens.GetNextToken();
            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericM()
        {
            var tokens = CreateEnumerator("XYZW:H;");

            var token = tokens.GetNextToken();
            Assert.IsTrue(token.TokenType == TokenType.Identifier && token.IsInvalid == false);
            Assert.IsTrue(token.Lexeme == "XYZW");

            token = tokens.GetNextToken();
            Assert.IsTrue(token.TokenType == TokenType.Punctuator);

        }

        [TestMethod]
        public void Test_NumericN()
        {
            var tokens = CreateEnumerator("F1CE.;");

            var token = tokens.GetNextToken();
            Assert.IsTrue(token.TokenType == TokenType.Identifier && token.IsInvalid == false);
            Assert.IsTrue(token.Lexeme == "F1CE");

            token = tokens.GetNextToken();
            Assert.IsTrue(token.TokenType == TokenType.Punctuator);

        }

        [TestMethod]
        public void Test_NumericO()
        {
            var tokens = CreateEnumerator("1FCE;");

            var token = tokens.GetNextToken();
            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == true);
            Assert.IsTrue(token.Lexeme == "1FCE");

            token = tokens.GetNextToken();
            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericP()
        {
            var tokens = CreateEnumerator("1FCE;");

            var token = tokens.GetNextToken();
            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == true);
            Assert.IsTrue(token.Lexeme == "1FCE");

            token = tokens.GetNextToken();
            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericQ()
        {
            var tokens = CreateEnumerator("1FCE.5FFB;");

            var token = tokens.GetNextToken();
            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == true);
            Assert.IsTrue(token.Lexeme == "1FCE.5FFB");

            token = tokens.GetNextToken();
            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericR()
        {
            var tokens = CreateEnumerator("1FCE.5FFB:H;");

            var token = tokens.GetNextToken();
            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid ==  false);
            Assert.IsTrue(token.Lexeme == "1FCE.5FFB:H");

            token = tokens.GetNextToken();
            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericS()
        {
            var tokens = CreateEnumerator("1FCE.5F.FB:H;");

            var token = tokens.GetNextToken();
            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == true);
            Assert.IsTrue(token.Lexeme == "1FCE.5F");

            token = tokens.GetNextToken();
            Assert.IsTrue(token.TokenType == TokenType.Punctuator);

        }


        private TokenEnumerator CreateEnumerator(string Text)
        {
            var source = SourceFile.CreateFromText(Text);
            var tokenizer = new Tokenizer<Keywords>(@"..\..\..\CSV\hardcode.csv", TokenDefinition.Pathname, Assembly.GetExecutingAssembly()); // be able to supply a delegate that can sanity check tokens.
            return new TokenEnumerator(tokenizer, source, ValidateToken, TokenType.BlockComment, TokenType.LineComment);
        }

        private static void ValidateToken(Token token)
        {

            if (token.TokenType == TokenType.NumericLiteral)
            {
                token.Lexeme = token.Lexeme.Trim();

                if (token.Lexeme.Contains('_') && token.Lexeme.Contains(' '))
                {
                    token.ErrorText = "A numeric literal must not contain more than one kind of separator character.";
                    token.IsInvalid = true;
                }

                if (token.Lexeme.Contains("  ") || token.Lexeme.Contains("__"))
                {
                    token.ErrorText = "A numeric literal must not contain repetitions of a separator character.";
                    token.IsInvalid = true;
                }

                if (token.Lexeme.Count(f => (f == '.')) > 1) // the FSM should prevent this, but we'll check this anyway.
                {
                    token.ErrorText = "A numeric literal must not contain more than one decimal point.";
                    token.IsInvalid = true;
                }

                token.Lexeme = token.Lexeme.Replace(" ", "").Replace("_", "");

                if (token.Lexeme.ToUpper().EndsWith(":H"))
                {
                    if (token.Lexeme.ToUpper().TrimEnd('H').TrimEnd(':').All(".0123456789ABCDEF".Contains) == false)
                    {
                        token.ErrorText = "This hexadecimal literal contains one or more invalid characters.";
                        token.IsInvalid = true;
                    }
                }

                if (token.Lexeme.ToUpper().EndsWith(":D"))
                {
                    if (token.Lexeme.Replace(" ","").Replace("_","").ToUpper().TrimEnd('D', ':').All(".0123456789".Contains) == false)
                    {
                        token.ErrorText = "This decimal literal contains one or more invalid characters.";
                        token.IsInvalid = true;
                    }
                }

                 if (token.Lexeme.ToUpper().EndsWith(":O"))
                {
                    if (token.Lexeme.ToUpper().All("_ 01234567".Contains) == false)
                    {
                        token.ErrorText = "This octal literal contains one or more invalid characters.";
                        token.IsInvalid = true;
                    }
                }

                if (token.Lexeme.ToUpper().EndsWith(":B"))
                {
                    if (token.Lexeme.ToUpper().All("_ 01".Contains) == false)
                    {
                        token.ErrorText = "This binary literal contains one or more invalid characters.";
                        token.IsInvalid = true;
                    }
                }

                if (token.Lexeme.ToUpper().EndsWithAny(":B",":O",":D",":H") == false)
                {
                    if (token.Lexeme.All(".0123456789".Contains) == false)
                    {
                        token.ErrorText = "This non-decimal literal does not end with a valid base indicator";
                        token.IsInvalid = true;
                    }
                }


                return;
            }
        }


    }
}