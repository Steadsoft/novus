using Steadsoft.Novus.Scanner.Classes;
using Steadsoft.Novus.Scanner.Enums;
using System.Diagnostics;
using System.Reflection;
using System.Linq;
using System.Net.Http.Headers;
using Steadsoft.Novus.Scanner.Statics;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Text.Json;

namespace Hardcode
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string TokenDefinitionsFile = @"..\..\..\hardcode.csv";
            string SourceFile = @"..\..\..\langtest_fr_1.hcl";
            string LanguageDictionary = @"..\..\..\lingua.keywords";

            Dictionary<string, string> E = new Dictionary<string, string> { { "ABC", "123" }, { "DEF", "456" } };
            Dictionary<string, string> F = new Dictionary<string, string> { { "UVW", "321" }, { "XYZ", "654" } };

            Dictionary<string, Dictionary<string, string>> all = new Dictionary<string, Dictionary<string, string>>();

            all.Add("E", E);
            all.Add("F", F);

            var xxx = JsonSerializer.Serialize(all);




            double a = 12_345_678.11;
            double b = 12_3_45__6_78.11;
            bool f = false;

            f = Double.TryParse("1.234e4", out var _);
            f = Double.TryParse("+0x7ff.3edp+1", out var _);

            var r = new Regex(@"([0-9a-fA-F]*\.[0-9a-fA-F]+|[0-9a-fA-F]+\.?)[Pp][-+]?[0-9]+[flFL]?");

            f = r.IsMatch("1.234p4");
            f = r.IsMatch("+7ff.3edp+1");

            List<Token> tokes = new List<Token>();

            // Create an input source from a source file.

            var input_source = SourceCode.CreateFromFile(SourceFile);

            // Create a token enumator from the souce using defintions from a file, skip comments as well.

            var token_enumerator = new TokenEnumerator<Keywords>(input_source, "fr", TokenDefinitionsFile, LanguageDictionary, TokenOrigin.File, ValidateToken, TokenType.BlockComment, TokenType.LineComment);

            var t = token_enumerator.GetNextToken(true);

            while (t.TokenType != TokenType.NoMoreTokens) 
            { 
                tokes.Add(t);

                t = token_enumerator.GetNextToken(true);

            }

            var goods = tokes.Where(t => t.IsInvalid == false).ToList();
            var fails = tokes.Where(t => t.IsInvalid ).ToList();

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
                        token.KeywordList[0] = Enum.Parse<Keywords>(Dictionary[token.Lexeme]);
                        token.ExactKeywordMatch= true;
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
                                        token.KeywordList[I] = Enum.Parse<Keywords>(Dictionary[kvp.Key]);
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
}