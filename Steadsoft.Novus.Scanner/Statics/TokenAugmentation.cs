﻿using Steadsoft.Novus.Scanner.Classes;
using Steadsoft.Novus.Scanner.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Steadsoft.Novus.Scanner.Statics
{
    public static class TokenAugmentation
    {
        public static void ValidateToken(TokenEnumerator<Keywords> tokens, string KeywordLanguage, Dictionary<string, string> Dictionary, Token token)
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

            // If we see a CR token followed by an LF token, then replace the two tokens 
            // with a single NewLine token.

            if (token.TokenType == TokenType.CR)
            {
                var next = tokens.PeekNextToken();

                if (next.TokenType == TokenType.LF)
                {
                    var discard = tokens.GetNextToken(); // just duiscard this because we've simply hit a Windows CR/LF pair.

                    token.Lexeme = "\r\n";
                    token.TokenType = TokenType.NewLine;
                }

                return;
            }

            // Strip any leading/trailing spaces from identifier tokens.
            // These can sometimes appear due to the way we tolerate spaces
            // inside numeric literals.

            if (token.TokenType == TokenType.Identifier)
            {
                token.Lexeme = token.Lexeme.TrimStart().TrimEnd();
            }

            // If we have an identifier with embedded spaces, we must split this up into
            // multiple distinct identifiers. This can happen if we see identifiers that 
            // start with hex chars but turn out to not be hex literals after all.

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
                    token.KeywordList[0] = (Enum.Parse<Keywords>(Dictionary[token.Lexeme]), 1);
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
                                    token.KeywordList[I] = (Enum.Parse<Keywords>(Dictionary[kvp.Key]), parts.Length);
                                }
                            }
                        }
                    }
                }
            }

            // Validate numeric literals

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
