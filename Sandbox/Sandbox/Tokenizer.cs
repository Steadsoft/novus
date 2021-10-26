using System;
using System.Collections.Generic;
using System.Text;

namespace Sandbox
{
    public class Tokenizer
    {
        private static readonly Func<Character, Action>[,] table;
        private readonly SourceFile source;

        static Tokenizer()
        {
            table = new Func<Character, Action>[50, 50];

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
                    return new Action(Step.AppendResume, States.SLASH_1);
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

            Add(States.SLASH_1, Kind.Symbol, (a) =>
            {
                if (a.Char == '/')
                    return new Action(Step.AppendResume, States.SLASH_2);
                return new Action(Step.AppendHalt, 0);

            });

            // a token with 2nd char a slash

            Add(States.SLASH_2, Kind.LF, (a) =>
            {
                return new Action(Step.AppendHalt, States.INITIAL, TokenType.Comment);
            });

            Add(States.SLASH_2, Kind.AnythingElse, (a) =>
            {
                return new Action(Step.AppendResume, States.SLASH_2);
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

                    state = action.State;
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