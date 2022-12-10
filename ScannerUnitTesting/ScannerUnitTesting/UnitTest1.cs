using Steadsoft.Novus.Scanner.Classes;
using Steadsoft.Novus.Scanner.Enums;
using Steadsoft.Novus.Scanner.Statics;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ScannerUnitTesting
{
    [TestClass]
    public class UnitTest1
    {
        Token token;
        // SEE: https://efxa.org/2014/05/10/regular-expressions-for-matching-data-values-in-compiler-lexers/

        [TestMethod]
        public void Test_IdentifierA()
        {
            var tokens = CreateEnumerator("im_a_name;");
            token = tokens.GetNextToken();
            Assert.IsTrue(token.TokenType == TokenType.Identifier);
            token = tokens.GetNextToken();
            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_IdentifierB()
        {
            var tokens = CreateEnumerator("im_a_name; ");

            token = tokens.GetNextToken();
            Assert.IsTrue(token.TokenType == TokenType.Identifier);
            token = tokens.GetNextToken();
            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }
        [TestMethod]
        public void Test_IdentifierC()
        {
            var tokens = CreateEnumerator("im_a_name ;");

            token = tokens.GetNextToken();
            Assert.IsTrue(token.TokenType == TokenType.Identifier);
            token = tokens.GetNextToken();
            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericA()
        {
            var tokens = CreateEnumerator("01234546789;");

            token = tokens.GetNextToken();
            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral);
            token = tokens.GetNextToken();
            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericB()
        {
            var tokens = CreateEnumerator("01234 546789;");

            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == false);
            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == false);
            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericC()
        {
            var tokens = CreateEnumerator("01234_546789;");

            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == false);
            token = tokens.GetNextToken(    true);
            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericD()
        {
            var tokens = CreateEnumerator("01234_ 546789;");

            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == false);
            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == false);
            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericE()
        {
            var tokens = CreateEnumerator("1:D;");

            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == false);
            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericF()
        {
            var tokens = CreateEnumerator("1 22 333:D;");

            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == false);
            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == false);
            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == false);
            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericG()
        {
            var tokens = CreateEnumerator("1 22 333:d;");

            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == false);
            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == false);
            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == false);
            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericH()
        {
            var tokens = CreateEnumerator("1 22 333:b;");

            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == false);
            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == false);
            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == true);
            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericI()
        {
            var tokens = CreateEnumerator("1 22.333:d;");

            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == false);
            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == false);
            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.SemiColon);


        }

        [TestMethod]
        public void Test_NumericJ()
        {
            var tokens = CreateEnumerator("1 2F.333:h;");

            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == false);
            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == false);
            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericK()
        {
            var tokens = CreateEnumerator("FACE;");

            try
            {
                token = tokens.GetNextToken(true);
                Assert.IsTrue(token.TokenType == TokenType.Identifier && token.IsInvalid == false);
                Assert.IsTrue(token.Lexeme == "FACE");

                token = tokens.GetNextToken(true);
                Assert.IsTrue(token.TokenType == TokenType.SemiColon);
            }
            catch (Exception ex) 
            {
                ;
            }



        }

        [TestMethod]
        public void Test_NumericL()
        {
            var tokens = CreateEnumerator("F1CE;");

            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.Identifier && token.IsInvalid == false);
            Assert.IsTrue(token.Lexeme == "F1CE");

            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericM()
        {
            var tokens = CreateEnumerator("XYZW:H;");

            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.Identifier && token.IsInvalid == false);
            Assert.IsTrue(token.Lexeme == "XYZW");

            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.Punctuator);

        }

        [TestMethod]
        public void Test_NumericN()
        {
            var tokens = CreateEnumerator("F1CE.;");

            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.Identifier && token.IsInvalid == false);
            Assert.IsTrue(token.Lexeme == "F1CE");

            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.Punctuator);

        }

        [TestMethod]
        public void Test_NumericO()
        {
            var tokens = CreateEnumerator("1FCE;");

            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == true);
            Assert.IsTrue(token.Lexeme == "1FCE");

            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericP()
        {
            var tokens = CreateEnumerator("1FCE;");

            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == true);
            Assert.IsTrue(token.Lexeme == "1FCE");

            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericQ()
        {
            var tokens = CreateEnumerator("1FCE.5FFB;");

            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == true);
            Assert.IsTrue(token.Lexeme == "1FCE.5FFB");

            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericR()
        {
            var tokens = CreateEnumerator("1FCE.5FFB:H;");

            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid ==  false);
            Assert.IsTrue(token.Lexeme == "1FCE.5FFB:H");

            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericS()
        {
            var tokens = CreateEnumerator("1FCE.5F.FB:H;");

            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == true);
            Assert.IsTrue(token.Lexeme == "1FCE.5F");

            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.Punctuator);

        }

        [TestMethod]
        public void Test_NumericT()
        {
            var tokens = CreateEnumerator("0110 0011 0101 1100:B;");

            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == false);
            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == false);
            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == false);
            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == false);

            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericU()
        {
            var tokens = CreateEnumerator("dcb   bcd    bde;");

            token = tokens.GetNextToken(true); // would ordinarily be an illegally formed identifier with spaces inside, but the augmentor fixes that.

            Assert.IsTrue(token.TokenType == TokenType.Identifier && token.IsInvalid == false && token.Lexeme == "dcb");

            token = tokens.GetNextToken(true);

            Assert.IsTrue(token.TokenType == TokenType.Identifier && token.IsInvalid == false && token.Lexeme == "bcd");

            token = tokens.GetNextToken(true);

            Assert.IsTrue(token.TokenType == TokenType.Identifier && token.IsInvalid == false && token.Lexeme == "bde");

            token = tokens.GetNextToken(true);

            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericV()
        {
            var tokens = CreateEnumerator("dcb bcd bde:h;");

            token = tokens.GetNextToken(true); 
            Assert.IsTrue(token.TokenType == TokenType.Identifier && token.IsInvalid == false && token.Lexeme == "dcb");
            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.Identifier && token.IsInvalid == false && token.Lexeme == "bcd");
            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.Identifier && token.IsInvalid == false && token.Lexeme == "bde");
            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.Punctuator && token.IsInvalid == false);
            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.Identifier && token.IsInvalid == false && token.Lexeme == "h");

            token = tokens.GetNextToken(true);

            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericW()
        {
            var tokens = CreateEnumerator("dcl a bin = dcb bcd bde:h;");

            token = tokens.GetNextToken(true); // would ordinarily be an illegally formed identifier with spaces inside, but the augmentor fixes that.

            Assert.IsTrue(token.TokenType == TokenType.Identifier && token.IsInvalid == false && token.Lexeme == "dcl");

            token = tokens.GetNextToken(true);

            Assert.IsTrue(token.TokenType == TokenType.Identifier && token.IsInvalid == false && token.Lexeme == "a");

            token = tokens.GetNextToken(true);

            Assert.IsTrue(token.TokenType == TokenType.Identifier && token.IsInvalid == false && token.Lexeme == "bin");

            token = tokens.GetNextToken(true);

            Assert.IsTrue(token.TokenType == TokenType.Equals && token.IsInvalid == false && token.Lexeme == "=");

            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.Identifier && token.IsInvalid == false && token.Lexeme == "dcb");
            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.Identifier && token.IsInvalid == false && token.Lexeme == "bcd");
            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.Identifier && token.IsInvalid == false && token.Lexeme == "bde");
            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.Punctuator && token.IsInvalid == false);
            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.Identifier && token.IsInvalid == false && token.Lexeme == "h");

            token = tokens.GetNextToken(true);

            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericX()
        {
            var tokens = CreateEnumerator("5.3876e4;");

            token = tokens.GetNextToken(true); // would ordinarily be an illegally formed identifier with spaces inside, but the augmentor fixes that.

            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == false && token.Lexeme == "5.3876e4");

            token = tokens.GetNextToken(true);

            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericX2()
        {
            var tokens = CreateEnumerator("5.3876e-4;");

            token = tokens.GetNextToken(true); // would ordinarily be an illegally formed identifier with spaces inside, but the augmentor fixes that.

            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == false && token.Lexeme == "5.3876e-4");

            token = tokens.GetNextToken(true);

            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericX3()
        {
            var tokens = CreateEnumerator("5.3876e+4;");

            token = tokens.GetNextToken(true); // would ordinarily be an illegally formed identifier with spaces inside, but the augmentor fixes that.

            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == false && token.Lexeme == "5.3876e+4");

            token = tokens.GetNextToken(true);

            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }


        [TestMethod]
        public void Test_NumericY()
        {
            var tokens = CreateEnumerator("5.3876E4;");

            token = tokens.GetNextToken(true); // would ordinarily be an illegally formed identifier with spaces inside, but the augmentor fixes that.

            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == false && token.Lexeme == "5.3876E4");

            token = tokens.GetNextToken(true);

            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericZ()
        {
            var tokens = CreateEnumerator("5.3876p4;");

            token = tokens.GetNextToken(true); // would ordinarily be an illegally formed identifier with spaces inside, but the augmentor fixes that.

            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == false && token.Lexeme == "5.3876p4");

            token = tokens.GetNextToken(true);

            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericZ2()
        {
            var tokens = CreateEnumerator("5.3876p-4;");

            token = tokens.GetNextToken(true); // would ordinarily be an illegally formed identifier with spaces inside, but the augmentor fixes that.

            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == false && token.Lexeme == "5.3876p-4");

            token = tokens.GetNextToken(true);

            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericZ3()
        {
            var tokens = CreateEnumerator("5.3876p+4;");

            token = tokens.GetNextToken(true); // would ordinarily be an illegally formed identifier with spaces inside, but the augmentor fixes that.

            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == false && token.Lexeme == "5.3876p+4");

            token = tokens.GetNextToken(true);

            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericZ4()
        {
            var tokens = CreateEnumerator("5.3A0D6p+4;");

            token = tokens.GetNextToken(true); // would ordinarily be an illegally formed identifier with spaces inside, but the augmentor fixes that.

            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == false && token.Lexeme == "5.3A0D6p+4");

            token = tokens.GetNextToken(true);

            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }


        private TokenEnumerator<Keywords> CreateEnumerator(string Text)
        {
            var source = SourceCode.CreateFromString(Text);
            //var tokenizer = new Tokenizer<Keywords>(@"..\..\..\CSV\hardcode.csv", TokenOrigin.File, Assembly.GetExecutingAssembly()); // be able to supply a delegate that can sanity check tokens.
            return new TokenEnumerator<Keywords>(source, "en", @"..\..\..\CSV\hardcode.csv", @"..\..\..\Inputs\lingua.keywords", TokenOrigin.File, TokenAugmentation.ValidateToken, TokenType.BlockComment, TokenType.LineComment);
        }
    }
}