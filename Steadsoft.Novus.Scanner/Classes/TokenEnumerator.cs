using Steadsoft.Novus.Scanner.Enums;
using System.Text;
using static Steadsoft.Novus.Scanner.Enums.TokenType;
using static Steadsoft.Novus.Scanner.Enums.State;

namespace Steadsoft.Novus.Scanner.Classes
{
    public class TokenEnumerator 
    {
        private readonly IEnumerator<Token> enumerator;
        private readonly IEnumerable<Token> source;
        private readonly Tokenizer<Keywords> tokenizer;
        private readonly TokenType[] Skips;
        private readonly Stack<Token> pushed = new();
        public TokenEnumerator(Tokenizer<Keywords> Tokenizer, SourceFile SourceFile, params TokenType[] Skips)
        {
            tokenizer = Tokenizer;
            source = Tokenizer.Tokenize(SourceFile);
            enumerator = source.GetEnumerator();
            this.Skips = Skips;
        }

        /// <summary>
        /// Performs a parser correctness check.
        /// </summary>
        /// <remarks>
        /// Several parsing methods will, when they encounter certain tokens, push that token back into the
        /// token source and then call another parsing method to continue the parse. This method ensures that 
        /// this has been done correctly, a failure is not a source error but is a perser code/logic error.
        /// </remarks>
        /// <param name="Type">The token type of the expected next token.</param>
        /// <param name="Token">If successful, the next token.</param>
        /// <exception cref="InternalErrorException"></exception>
        public void VerifyExpectedToken(TokenType Type, out Token Token)
        {
            var token = GetNextToken();

            if (token.TokenType != Type)
                throw new InternalErrorException($"Expected token (token type) '{Type}' has not been pushed by caller!");

            Token = token;

            return;
        }

        /// <summary>
        /// Performs a parser correctness check.
        /// </summary>
        /// <remarks>
        /// Several parsing methods will, when they encounter certain tokens, push that token back into the
        /// token source and then call another parsing method to continue the parse. This method ensures that 
        /// this has been done correctly, a failure is not a source error but is a perser code/logic error.
        /// </remarks>
        /// <param name="Keyword">The token type of the expected next token.</param>
        /// <param name="Token">If successful, the next token.</param>
        /// <exception cref="InternalErrorException"></exception>
        public void VerifyExpectedToken(Keywords Keyword, out Token Token)
        {
            var token = GetNextToken();

            if (token.Keyword != Keyword)
                throw new InternalErrorException($"Expected token (keyword) '{Keyword}' has not been pushed by caller!");

            Token = token;

            return;
        }


        /// <summary>
        /// Indicates of the next tokens match the set of supplied token types.
        /// </summary>
        /// <param name="Tokens">One or more token types in lexical order.</param>
        /// <returns></returns>
        public bool NextTokensAre(params TokenType[] Tokens)
        {
            var list = PeekNextTokens(Tokens.Length).ToArray();

            for (int I = 0; I < Tokens.Length; I++)
            {
                if (Tokens[I] != list[I].TokenType)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Returns the next 'n' tokens in order, without consuming them from the input.
        /// </summary>
        /// <param name="N"></param>
        /// <returns></returns>
        public List<Token> PeekNextTokens(int N)
        {
            var list = new List<Token>();

            for (int I = 0; I < N; I++)
            {
                list.Add(GetNextToken());
            }

            PushTokens(list);

            return list;
        }

        /// <summary>
        /// Consume and returns the next token.
        /// </summary>
        /// <returns></returns>
        public Token GetNextToken()
        {
            // Once a token has been consumed it is in the past, can't be re-read.
            // But we can 'push' a token at any point and the next time we get a
            // token it will be that pushed token.
            // this is how the parser can backtrack.

            if (pushed.Any())
            {
                return pushed.Pop();
            }

            while (enumerator.MoveNext())
            {
                var token = enumerator.Current;

                if (token.TokenType == TokenType.Preprocessor)
                {
                    if (token.Lexeme == Preprocessor.Delimiter.Add)
                    {
                        ProcessDelimiterAdd(token);
                    }

                    if (token.Lexeme == Preprocessor.Delimiter.Drop)
                    {
                        ProcessDelimiterDrop();
                    }

                    // TODO we should report unrecognized preprocessor stuff here.
                    continue; // discard, do not return this to parser, 
                }

                if (Skips.Contains(token.TokenType) == false)
                    return token;
            }

            return new Token(NoMoreTokens, "", 0, 0);
        }

        private void ProcessDelimiterDrop()
        {
            tokenizer.Table.RemoveAllEntriesFor(INITIAL, DELIMITER0, DELIMITER1, DELIMITER2, DELIMITER3, DELIMITER4, DELIMITER5, DELIMITER6, DELIMITER7, DELIMITER8, DELIMITER9);
        }

        private void ProcessDelimiterAdd(Token token)
        {
            bool has_start_end = false;

            StringBuilder delimiter = new();

            Character c = tokenizer.GetNextRawChar();

            while (Char.IsWhiteSpace(c.Char))
            {
                c = tokenizer.GetNextRawChar();
            }

            while (c.Char != '\r')
            {
                delimiter.Append(c.Char);
                c = tokenizer.GetNextRawChar();
            }

            var chars = delimiter.ToString().Trim();

            if (chars.Contains(' '))
            {
                has_start_end = true;
            }
            else
            {
                has_start_end = false;
            }

            // Now we have the raw set of characters specfied for the string literal delimiter.
            // We must now use these to update the scanner table so that the delimiter becomes the
            // new one for literal string tokenization.

            int I,J;

            if (has_start_end)
            {

            }
            else
            {
                tokenizer.Table.Add(State.INITIAL, chars[0],  new Entry { Step = Step.DiscardContinue, State = State.DELIMITER0, TokenType = TokenType.Undecided });

                for (I = 1; I < chars.Length; I++)
                {
                    tokenizer.Table.Add((State)(I-1), chars[I], new Entry { Step = Step.DiscardContinue, State = (State)(I), TokenType = TokenType.Undecided });
                }

                tokenizer.Table.Add<LexicalClass>((State)(I - 1),LexicalClass.Any, new Entry { Step = Step.AppendContinue, State = (State)(I - 1), TokenType = TokenType.Undecided });

                for (J = 0; J < chars.Length-1; J++)
                {
                    tokenizer.Table.Add((State)(I - 1), chars[J], new Entry { Step = Step.DiscardContinue, State = (State)(I), TokenType = TokenType.Undecided });
                    I++;
                }

                tokenizer.Table.Add((State)(I - 1), chars[J], new Entry { Step = Step.DiscardReturn, State = State.INITIAL, TokenType = TokenType.QString });

            }
        }
        /// <summary> 
        /// Returns the supplied token to the input, no check is made so be careful!
        /// </summary>
        /// <param name="Token"></param>
        public void PushToken(Token Token)
        {
            pushed.Push(Token);
        }

        public void PushTokens(List<Token> Tokens)
        {
            for (int I = Tokens.Count - 1; I >= 0; I--)
            {
                pushed.Push(Tokens[I]);
            }
        }

        /// <summary>
        /// Consumes tokens until a token matching the specified lexeme is encountered.
        /// </summary>
        /// <remarks>
        /// This method repeatedly consumes tokens until a match is encountered. At that point 
        /// the method returns and the token that was sought will have been consumed, it will not be
        /// seen when the next token is retrieved.
        /// </remarks>
        /// <param name="Lexeme"></param>
        public void SkipToNext(string Lexeme)
        {
            var token = GetNextToken();

            while (token.Lexeme != Lexeme && token.TokenType != NoMoreTokens)
            {
                token = GetNextToken();
            }
        }
    }
}