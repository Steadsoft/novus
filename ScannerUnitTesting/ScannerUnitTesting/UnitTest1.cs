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
        // SEE: https://efxa.org/2014/05/10/regular-expressions-for-matching-data-values-in-compiler-lexers/

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

        [TestMethod]
        public void Test_NumericX()
        {
            var tokens = CreateEnumerator("5.3876e4;");

            var token = tokens.GetNextToken(true); // would ordinarily be an illegally formed identifier with spaces inside, but the augmentor fixes that.

            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == false && token.Lexeme == "5.3876e4");

            token = tokens.GetNextToken(true);

            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericX2()
        {
            var tokens = CreateEnumerator("5.3876e-4;");

            var token = tokens.GetNextToken(true); // would ordinarily be an illegally formed identifier with spaces inside, but the augmentor fixes that.

            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == false && token.Lexeme == "5.3876e-4");

            token = tokens.GetNextToken(true);

            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericX3()
        {
            var tokens = CreateEnumerator("5.3876e+4;");

            var token = tokens.GetNextToken(true); // would ordinarily be an illegally formed identifier with spaces inside, but the augmentor fixes that.

            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == false && token.Lexeme == "5.3876e+4");

            token = tokens.GetNextToken(true);

            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }


        [TestMethod]
        public void Test_NumericY()
        {
            var tokens = CreateEnumerator("5.3876E4;");

            var token = tokens.GetNextToken(true); // would ordinarily be an illegally formed identifier with spaces inside, but the augmentor fixes that.

            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == false && token.Lexeme == "5.3876E4");

            token = tokens.GetNextToken(true);

            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericZ()
        {
            var tokens = CreateEnumerator("5.3876p4;");

            var token = tokens.GetNextToken(true); // would ordinarily be an illegally formed identifier with spaces inside, but the augmentor fixes that.

            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == false && token.Lexeme == "5.3876p4");

            token = tokens.GetNextToken(true);

            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericZ2()
        {
            var tokens = CreateEnumerator("5.3876p-4;");

            var token = tokens.GetNextToken(true); // would ordinarily be an illegally formed identifier with spaces inside, but the augmentor fixes that.

            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == false && token.Lexeme == "5.3876p-4");

            token = tokens.GetNextToken(true);

            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericZ3()
        {
            var tokens = CreateEnumerator("5.3876p+4;");

            var token = tokens.GetNextToken(true); // would ordinarily be an illegally formed identifier with spaces inside, but the augmentor fixes that.

            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == false && token.Lexeme == "5.3876p+4");

            token = tokens.GetNextToken(true);

            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }

        [TestMethod]
        public void Test_NumericZ4()
        {
            var tokens = CreateEnumerator("5.3A0D6p+4;");

            var token = tokens.GetNextToken(true); // would ordinarily be an illegally formed identifier with spaces inside, but the augmentor fixes that.

            Assert.IsTrue(token.TokenType == TokenType.NumericLiteral && token.IsInvalid == false && token.Lexeme == "5.3A0D6p+4");

            token = tokens.GetNextToken(true);

            Assert.IsTrue(token.TokenType == TokenType.SemiColon);

        }


        private TokenEnumerator<Keywords> CreateEnumerator(string Text)
        {
            var source = SourceCode.CreateFromString(Text);
            //var tokenizer = new Tokenizer<Keywords>(@"..\..\..\CSV\hardcode.csv", TokenOrigin.File, Assembly.GetExecutingAssembly()); // be able to supply a delegate that can sanity check tokens.
            return new TokenEnumerator<Keywords>(source, "en", @"..\..\..\CSV\hardcode.csv", @"..\..\..\Inputs\lingua.keywords", TokenOrigin.File, ValidateToken, TokenType.BlockComment, TokenType.LineComment);
        }

        void ValidateToken(TokenEnumerator<Keywords> tokens, string KeywordLanguage, Dictionary<string, string> Dictionary, Token token)
        {
            const char US = '_';
            const char SP = ' ';
            const char DOT = '.';
            const string BHEX = ":H";
            const string BOCT = ":O";
            const string BDEC = ":D";
            const string BBIN = ":B";
            const string HEX_CHARS = ".0123456789ABCDEF";
            const string DEC_CHARS = ".0123456789";
            const string OCT_CHARS = ".01234567";
            const string BIN_CHARS = ".01";
            const string HEX_LETTERS_L = "abcdef";
            const string HEX_LETTERS_U = "ABCDEF";
            const string FLOAT_HEX_REGEX = @"([0-9a-fA-F]*\.[0-9a-fA-F]+|[0-9a-fA-F]+\.?)[Pp][-+]?[0-9]+[flFL]?";

            Regex FloatHex = new Regex(FLOAT_HEX_REGEX);


            // Recognizing keyowrds is tricky because in some language a "keyword" is actually several terms.
            // If the lexeme exactkly match the spelling of a keyword, then it is that keyword.
            // But if it matches only one of several terms, we store the corresponding keyword into the 
            // array slot corresponding to which of the terms, it macthes.
            // During parsing, if a several tokens match terms 0, 1, 2 in order and they each refer to same keyword, then we can be confident 
            // those terms do correspond to that keyword.

            if (token.TokenType == TokenType.Identifier)
            {
                if (Dictionary.ContainsKey(token.Lexeme))
                {
                    token.KeywordList[0] = (Enum.Parse<Keywords>(Dictionary[token.Lexeme]),1);
                    token.ExactKeywordMatch = true;
                    return;
                }
                else
                {
                    foreach (var kvp in Dictionary)
                    {
                        if (kvp.Key.Contains(' ')) // multi word keywords must contain a space
                        {
                            var parts = kvp.Key.Split(' ');

                            for (int I = 0; I < parts.Length; I++)
                            {
                                if (token.Lexeme == parts[I])
                                {
                                    token.KeywordList[I] = (Enum.Parse<Keywords>(Dictionary[kvp.Key]),parts.Length);
                                }
                            }
                        }
                    }
                }
            }

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

                if (token.Lexeme.Contains(US) && token.Lexeme.Contains(SP))
                {
                    token.ErrorText = "A numeric literal must not contain more than one kind of separator character.";
                    token.IsInvalid = true;
                    return;
                }

                if (token.Lexeme.Contains("  ") || token.Lexeme.Contains("__"))
                {
                    token.ErrorText = "A numeric literal must not contain repetitions of a separator character.";
                    token.IsInvalid = true;
                    return;
                }

                if (token.Lexeme.Count(f => (f == DOT)) > 1) // the FSM should prevent this, but we'll check this anyway.
                {
                    token.ErrorText = "A numeric literal must not contain more than one decimal point.";
                    token.IsInvalid = true;
                    return;
                }

                token.Lexeme = token.Lexeme.Replace(" ", "").Replace("_", "");

                if (token.Lexeme.ToUpper().EndsWith(BHEX))
                {
                    if (StripBaseIndicator(token.Lexeme, 'H').All(HEX_CHARS.Contains) == false)
                    {
                        token.ErrorText = "This hexadecimal literal contains one or more invalid characters.";
                        token.IsInvalid = true;
                        return;
                    }

                    if (token.Lexeme.Any(HEX_LETTERS_L.Contains) && token.Lexeme.Any(HEX_LETTERS_U.Contains))
                    {
                        token.ErrorText = "A hexadecimal literal must not contain both uppercase and lowercase letters.";
                        token.IsInvalid = true;
                        return;
                    }
                }

                if (token.Lexeme.ToUpper().EndsWith(BDEC))
                {
                    if (StripBaseIndicator(token.Lexeme, 'D').All(DEC_CHARS.Contains) == false)
                    {
                        token.ErrorText = "This decimal literal contains one or more invalid characters.";
                        token.IsInvalid = true;
                        return;
                    }
                }

                if (token.Lexeme.ToUpper().EndsWith(BOCT))
                {
                    if (StripBaseIndicator(token.Lexeme, 'O').All(OCT_CHARS.Contains) == false)
                    {
                        token.ErrorText = "This octal literal contains one or more invalid characters.";
                        token.IsInvalid = true;
                        return;
                    }
                }

                if (token.Lexeme.ToUpper().EndsWith(BBIN))
                {
                    if (StripBaseIndicator(token.Lexeme, 'B').All(BIN_CHARS.Contains) == false)
                    {
                        token.ErrorText = "This binary literal contains one or more invalid characters.";
                        token.IsInvalid = true;
                        return;
                    }
                }

                if (token.Lexeme.ToUpper().EndsWithAny(BBIN, BOCT, BDEC, BHEX) == false)
                {
                    double value;

                    if (Double.TryParse(token.Lexeme, out value))
                        return;

                    if (FloatHex.IsMatch(token.Lexeme))
                        return;


                    if (token.Lexeme.All(DEC_CHARS.Contains) == false)
                    {
                        token.ErrorText = "This non-decimal literal does not end with a valid base indicator";
                        token.IsInvalid = true;
                        return;
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