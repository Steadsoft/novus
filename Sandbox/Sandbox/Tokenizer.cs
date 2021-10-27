using System;
using System.Collections.Generic;
using System.Text;

namespace Sandbox
{
    /// <summary>
    /// Represents a mechanism which can consume C# source and emit language tokens.
    /// </summary>
    public class Tokenizer
    {
        private static readonly Func<Character, Action>[,] table;
        private readonly SourceFile source;

        static Tokenizer()
        {
            /* we use a 2D map of functions indexed by state and character just read */

            table = new Func<Character, Action>[50, 50];

            // these are the initializations of the table

            Add(States.INITIAL, Kind.CR, (a) => { return new Action(Step.DiscardResume, States.INITIAL); });
            Add(States.INITIAL, Kind.LF, (a) => { return new Action(Step.DiscardResume, States.INITIAL); });
            Add(States.INITIAL, Kind.Whitespace, (a) => { return new Action(Step.DiscardResume, States.INITIAL); });
            Add(States.INITIAL, Kind.Letter, (a) => { return new Action(Step.AppendResume, States.IDENTIFIER); });
            Add(States.INITIAL, Kind.Digit, (a) => { return new Action(Step.AppendResume, States.INTEGER); });
            Add(States.INITIAL, Kind.Punctuation, (a) => { return new Action(Step.AppendHalt, States.INITIAL); });
            Add(States.INITIAL, Kind.Brace, (a) => { return new Action(Step.AppendHalt, States.INITIAL); });
            Add(States.INITIAL, Kind.Parenthesis, (a) => { return new Action(Step.AppendHalt, States.INITIAL); });
            Add(States.INITIAL, Kind.Bracket, (a) => { return new Action(Step.AppendHalt, States.INITIAL); });
            Add(States.INITIAL, Kind.Symbol, (a) =>
            {
                if (a.Char == '/')
                    return new Action(Step.AppendResume, States.SLASH);
                return new Action(Step.AppendHalt, 0);

            });

            Add(States.INITIAL, Kind.Arrow, (a) => { return new Action(Step.AppendHalt, States.INITIAL); });

            // Assume its an identifier

            Add(States.IDENTIFIER, Kind.Letter, (a) => { return new Action(Step.AppendResume, States.IDENTIFIER); });
            Add(States.IDENTIFIER, Kind.Digit, (a) => { return new Action(Step.AppendResume, States.IDENTIFIER); });
            Add(States.IDENTIFIER, Kind.AnythingElse, (a) => { return new Action(Step.RestoreHalt, States.INITIAL, TokenType.Identifier); });

            // Assume its an integer

            Add(States.INTEGER, Kind.Digit, (a) => { return new Action(Step.AppendResume, States.INTEGER); });
            Add(States.INTEGER, Kind.AnythingElse, (a) => { return new Action(Step.RestoreHalt, States.INITIAL, TokenType.Integer); });

            // a token with 1st char a slash

            Add(States.SLASH, Kind.Symbol, (a) =>
            {
                if (a.Char == '/')
                    return new Action(Step.AppendResume, States.SLASH_SLASH);
                if (a.Char == '*')
                    return new Action(Step.AppendResume, States.SLASH_STAR);
                return new Action(Step.AppendHalt, 0);

            });

            Add(States.SLASH_SLASH, Kind.LF, (a) =>
            {
                return new Action(Step.AppendHalt, States.INITIAL, TokenType.LineComment);
            });

            Add(States.SLASH_SLASH, Kind.AnythingElse, (a) =>
            {
                return new Action(Step.AppendResume, States.SLASH_SLASH);
            });

            Add(States.SLASH_STAR, Kind.Symbol, (a) =>
            {
                if (a.Char == '*')
                    return new Action(Step.AppendResume, States.SLASH_STAR_STAR);

                return new Action(Step.AppendResume, States.SLASH_STAR);
            });

            Add(States.SLASH_STAR, Kind.AnythingElse, (a) =>
            {
                return new Action(Step.AppendResume, States.SLASH_STAR);
            });

            Add(States.SLASH_STAR_STAR, Kind.Symbol, (a) =>
            {
                if (a.Char == '/')
                    return new Action(Step.AppendHalt, States.INITIAL, TokenType.BlockComment);

                if (a.Char == '*')
                    return new Action(Step.AppendResume, States.SLASH_STAR_STAR);

                return new Action(Step.AppendResume, States.SLASH_STAR);
            });

            Add(States.SLASH_STAR_STAR, Kind.AnythingElse, (a) =>
            {
                return new Action(Step.AppendResume, States.SLASH_STAR);
            });

        }
        public Tokenizer(SourceFile File)
        {
            this.source = File;
        }

        public IEnumerable<Token> Tokens
        {
            get
            {
                StringBuilder lexeme = new();
                States state = States.INITIAL;

                for (int I = 0; I < source.Chars.Count; I++)
                {
                    var character = source.Chars[I];

                    // given the current state and the character just read 
                    // locate and call a handler function. That will return
                    // information that describes what to do, whether we've recognized
                    // a complete token and what state to enter next.

                    var action = this[state, character.Kind](character);

                    switch (action.Step)
                    {
                        case Step.AppendResume:
                            {
                                lexeme.Append(character.Char);
                                break;
                            }
                        case Step.AppendHalt:
                            {
                                lexeme.Append(character.Char);
                                yield return new Token(action.TokenType, lexeme.ToString(), character.Line, character.Col);
                                lexeme.Clear();
                                break;
                            }
                        case Step.RestoreHalt:
                            {
                                I--;
                                yield return new Token(action.TokenType, lexeme.ToString(), character.Line, character.Col);
                                lexeme.Clear();
                                break;
                            }
                        case Step.DiscardResume:
                            {
                                break;
                            }
                        default:
                            throw new InvalidOperationException("really?");
                    }

                    state = action.State; // set our state to whatever the handlder told us.
                }
            }
        }
        private static void Add(States State, Kind Kind, Func<Character, Action> Function)
        {
            table[(int)State, (int)Kind] = Function;
        }

        private Func<Character, Action> this[States State, Kind Kind]
        {
            get
            {
                return table[(int)State, (int)Kind] ?? table[(int)State, (int)Kind.AnythingElse];
            }
        }
    }
}