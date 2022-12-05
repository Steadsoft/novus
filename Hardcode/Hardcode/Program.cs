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
            string SourceFile = @"..\..\..\langtest_en_1.hcl";
            string LanguageDictionary = @"..\..\..\fr.keywords";

            double a = 12_345_678.11;
            double b = 12_3_45__6_78.11;
            bool f = false;

            f = Double.TryParse("1.234e4", out var _);
            f = Double.TryParse("+0x7ff.3edp+1", out var _);

            var r = new Regex(@"([0-9a-fA-F]*\.[0-9a-fA-F]+|[0-9a-fA-F]+\.?)[Pp][-+]?[0-9]+[flFL]?");

            f = r.IsMatch("1.234p4");
            f = r.IsMatch("+7ff.3edp+1");

            List<Token> tokes = new List<Token>();

            Dictionary<string,string> token_dictionary = new Dictionary<string,string>();

            token_dictionary = JsonSerializer.Deserialize<Dictionary<string,string>>(File.ReadAllText(LanguageDictionary)).ToDictionary(x => x.Value, x=> x.Key);

            // Create an input source from a source file.

            var input_source = SourceCode.CreateFromFile(SourceFile);

            // Create a token enumator from the souce using defintions from a file, skip comments as well.

            var token_enumerator = new TokenEnumerator<Keywords>(input_source, TokenDefinitionsFile, TokenOrigin.File, ValidateToken, TokenType.BlockComment, TokenType.LineComment);

            var t = token_enumerator.GetNextToken(true);

            while (t.TokenType != TokenType.NoMoreTokens) 
            { 
                tokes.Add(t);

                t = token_enumerator.GetNextToken(true);

            }

            var goods = tokes.Where(t => t.IsInvalid == false).ToList();
            var fails = tokes.Where(t => t.IsInvalid ).ToList();

            void ValidateToken(TokenEnumerator<Keywords> tokens, Token token)
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


                if (token.TokenType == TokenType.Identifier)
                {
                    if (token_dictionary.ContainsKey(token.Lexeme))
                    {
                        token.Keyword = Enum.Parse<Keywords>(token_dictionary[token.Lexeme]);
                        return;
                    }
                    else
                    {
                        foreach (var kvp in token_dictionary)
                        {
                            if (kvp.Key.Contains(' ')) // multi word keywords must contain a space
                            {
                                if (kvp.Key.StartsWith(token.Lexeme))
                                {
                                    var follower = tokens.PeekNextToken();

                                    if (follower.TokenType == TokenType.Identifier && kvp.Key.EndsWith(follower.Lexeme))
                                    {
                                        // OK we have a keyword that has two terms and the two tokens match these.
                                        // We create a new token and discard the two original one.

                                        var discard = tokens.GetNextToken(); // discard the just peeked token.

                                        // modify the original token

                                        token.Keyword = Enum.Parse<Keywords>(kvp.Value);
                                        token.Lexeme = token.Lexeme + " " + follower.Lexeme;
                                        return;
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