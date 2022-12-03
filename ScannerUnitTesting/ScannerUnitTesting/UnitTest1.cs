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

            var token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == false);
            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericC()
        {
            var tokens = CreateEnumerator("01234_546789;");

            var token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == false);
            token = tokens.GetNextToken(    true);
            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericD()
        {
            var tokens = CreateEnumerator("01234_ 546789;");

            var token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == true);
            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericE()
        {
            var tokens = CreateEnumerator("1:D;");

            var token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == false);
            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericF()
        {
            var tokens = CreateEnumerator("1 22 333:D;");

            var token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == false);
            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericG()
        {
            var tokens = CreateEnumerator("1 22 333:d;");

            var token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == false);
            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericH()
        {
            var tokens = CreateEnumerator("1 22 333:b;");

            var token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == true);
            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericI()
        {
            var tokens = CreateEnumerator("1 22.333:d;");

            var token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == false);
            Assert.IsTrue(token.Lexeme == "122.333:d");

            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.SemiColon);


        }

        [TestMethod]
        public void Test_NumericJ()
        {
            var tokens = CreateEnumerator("1 2F.333:h;");

            var token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == false);
            Assert.IsTrue(token.Lexeme == "12F.333:h");

            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericK()
        {
            var tokens = CreateEnumerator("FACE;");

            var token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.Identifier && token.IsInvalid == false);
            Assert.IsTrue(token.Lexeme == "FACE");

            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericL()
        {
            var tokens = CreateEnumerator("F1CE;");

            var token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.Identifier && token.IsInvalid == false);
            Assert.IsTrue(token.Lexeme == "F1CE");

            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericM()
        {
            var tokens = CreateEnumerator("XYZW:H;");

            var token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.Identifier && token.IsInvalid == false);
            Assert.IsTrue(token.Lexeme == "XYZW");

            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.Punctuator);

        }

        [TestMethod]
        public void Test_NumericN()
        {
            var tokens = CreateEnumerator("F1CE.;");

            var token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.Identifier && token.IsInvalid == false);
            Assert.IsTrue(token.Lexeme == "F1CE");

            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.Punctuator);

        }

        [TestMethod]
        public void Test_NumericO()
        {
            var tokens = CreateEnumerator("1FCE;");

            var token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == true);
            Assert.IsTrue(token.Lexeme == "1FCE");

            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericP()
        {
            var tokens = CreateEnumerator("1FCE;");

            var token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == true);
            Assert.IsTrue(token.Lexeme == "1FCE");

            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericQ()
        {
            var tokens = CreateEnumerator("1FCE.5FFB;");

            var token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == true);
            Assert.IsTrue(token.Lexeme == "1FCE.5FFB");

            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericR()
        {
            var tokens = CreateEnumerator("1FCE.5FFB:H;");

            var token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid ==  false);
            Assert.IsTrue(token.Lexeme == "1FCE.5FFB:H");

            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericS()
        {
            var tokens = CreateEnumerator("1FCE.5F.FB:H;");

            var token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == true);
            Assert.IsTrue(token.Lexeme == "1FCE.5F");

            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.Punctuator);

        }

        [TestMethod]
        public void Test_NumericT()
        {
            var tokens = CreateEnumerator("0110 0011 0101 1100:B;");

            var token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == false);
            Assert.IsTrue(token.Lexeme == "0110001101011100:B");

            token = tokens.GetNextToken(true);
            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericU()
        {
            var tokens = CreateEnumerator("dcb   bcd    bde;");

            var token = tokens.GetNextToken(true); // would ordinarily be an illegally formed identifier with spaces inside, but the augmentor fixes that.

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

            var token = tokens.GetNextToken(true); // would ordinarily be an illegally formed identifier with spaces inside, but the augmentor fixes that.

            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == false && token.Lexeme == "dcbbcdbde:h");

            token = tokens.GetNextToken(true);

            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericW()
        {
            var tokens = CreateEnumerator("dcl a bin = dcb bcd bde:h;");

            var token = tokens.GetNextToken(true); // would ordinarily be an illegally formed identifier with spaces inside, but the augmentor fixes that.

            Assert.IsTrue(token.TokenType == TokenType.Identifier && token.IsInvalid == false && token.Lexeme == "dcl");

            token = tokens.GetNextToken(true);

            Assert.IsTrue(token.TokenType == TokenType.Identifier && token.IsInvalid == false && token.Lexeme == "a");

            token = tokens.GetNextToken(true);

            Assert.IsTrue(token.TokenType == TokenType.Identifier && token.IsInvalid == false && token.Lexeme == "bin");

            token = tokens.GetNextToken(true);

            Assert.IsTrue(token.TokenType == TokenType.Equals && token.IsInvalid == false && token.Lexeme == "=");

            token = tokens.GetNextToken(true);

            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == false && token.Lexeme == "dcbbcdbde:h");

            token = tokens.GetNextToken(true);

            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }




        private TokenEnumerator CreateEnumerator(string Text)
        {
            var source = SourceFile.CreateFromText(Text);
            var tokenizer = new Tokenizer<Keywords>(@"..\..\..\CSV\hardcode.csv", TokenDefinition.Pathname, Assembly.GetExecutingAssembly()); // be able to supply a delegate that can sanity check tokens.
            return new TokenEnumerator(tokenizer, source, ValidateToken, TokenType.BlockComment, TokenType.LineComment);
        }

        private static void ValidateToken(TokenEnumerator tokens, Token token)
        {
            if (token.TokenType == TokenType.CR)
            {
                var next = tokens.PeekNextToken();

                if (next.TokenType == TokenType.LF)
                {
                    var discard = tokens.GetNextToken(); // just duiscard this because we've simply hit a Windows CR/LF pair.

                    token.Lexeme = "\r\n";
                    token.TokenType = TokenType.NewLine;
                }
            }

            if (token.TokenType == TokenType.Identifier && token.Lexeme.Contains(' '))
            {
                // The token structure allows spaces inside literals, therefore identifier recognition can 
                // only terminate when some other terminator character is encountered. If the terminator
                // does not indicate a numeric (i.e. hex) literal with trailing number base then the entire
                // concatenation of identifiers is returned. We therefore split that into several new tokens and  
                // push them onto the enumerator stack. The calling code has no idea and will simply see
                // several distinct and valid identifiers tokens.

                var idents = token.Lexeme.Split(' ', StringSplitOptions.RemoveEmptyEntries).Reverse(); // we're going to stack these so order is very important

                foreach (var ident in idents)
                {
                    tokens.PushToken(new Token(TokenType.Identifier, ident, token.LineNumber, 0));
                }

                var temp = tokens.GetNextToken();

                token.Lexeme = temp.Lexeme;
                token.TokenType = temp.TokenType;
            }

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
                    if (StripBaseIndicator(token.Lexeme, 'H').All(".0123456789ABCDEF".Contains) == false)
                    {
                        token.ErrorText = "This hexadecimal literal contains one or more invalid characters.";
                        token.IsInvalid = true;
                    }
                }

                if (token.Lexeme.ToUpper().EndsWith(":D"))
                {
                    if (StripBaseIndicator(token.Lexeme, 'D').All(".0123456789".Contains) == false)
                    {
                        token.ErrorText = "This decimal literal contains one or more invalid characters.";
                        token.IsInvalid = true;
                    }
                }

                if (token.Lexeme.ToUpper().EndsWith(":O"))
                {
                    if (StripBaseIndicator(token.Lexeme, 'O').All("_ 01234567".Contains) == false)
                    {
                        token.ErrorText = "This octal literal contains one or more invalid characters.";
                        token.IsInvalid = true;
                    }
                }

                if (token.Lexeme.ToUpper().EndsWith(":B"))
                {
                    if (StripBaseIndicator(token.Lexeme, 'B').All("_ 01".Contains) == false)
                    {
                        token.ErrorText = "This binary literal contains one or more invalid characters.";
                        token.IsInvalid = true;
                    }
                }

                if (token.Lexeme.ToUpper().EndsWithAny(":B", ":O", ":D", ":H") == false)
                {
                    if (token.Lexeme.All(".0123456789".Contains) == false)
                    {
                        token.ErrorText = "This non-decimal literal does not end with a valid base indicator";
                        token.IsInvalid = true;
                    }
                }


                return;

                string StripBaseIndicator(string str, char basechar)
                {
                    return str.ToUpper().TrimEnd(basechar).TrimEnd(':');
                }
            }
        }


    }
}