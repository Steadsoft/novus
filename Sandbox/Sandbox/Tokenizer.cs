using System;
using System.Collections.Generic;
using System.Text;
using static Sandbox.LexicalClass;

namespace Sandbox
{
    /// <summary>
    /// Represents a mechanism which can consume C# source and emit language tokens.
    /// </summary>
    public class Tokenizer
    {
        private static readonly SparseTable<State, Char, Func<Character, Action>> table;
        private readonly SourceFile source;

        static Tokenizer()
        {
            /* we use a 2D map of functions indexed by state and character just read */

            table = new();

            // these are the initializations of the table. 
            // Initialization proceeds by designating a state and then for that state we
            // add all desired handlers for specific characters, then we add handlers for 
            // classs of characters and finally if desired, we can add handlers for 'any' characters.

            table.Add(State.INITIAL, '/',   (a) => { return new Action(Step.AppendContinue, State.SLASH); });
            table.Add(State.INITIAL, '-', (a) => { return new Action(Step.AppendContinue, State.HYPHEN); });

            table.Add(State.INITIAL, White, (a) => { return new Action(Step.DiscardContinue, State.INITIAL); });
            table.Add(State.INITIAL, Alpha, (a) => { return new Action(Step.AppendContinue, State.IDENTIFIER); });
            table.Add(State.INITIAL, Digit, (a) => { return new Action(Step.AppendContinue, State.INTEGER); });
            table.Add(State.INITIAL, Punct, (a) => { return new Action(Step.AppendReturn, State.INITIAL, TokenType.Punctuator); });
            table.Add(State.INITIAL, Symbl, (a) => { return new Action(Step.AppendReturn, State.INITIAL, TokenType.Symbol); });

            table.Add(State.SLASH, '*', (a) => { return new Action(Step.AppendContinue, State.SLASH_STAR); });

            table.Add(State.SLASH_STAR, '*', (a) => { return new Action(Step.AppendContinue, State.SLASH_STAR_STAR); });
            table.Add(State.SLASH_STAR, Any, (a) => { return new Action(Step.AppendContinue, State.SLASH_STAR); });

            table.Add(State.SLASH_STAR_STAR, '/', (a) => { return new Action(Step.AppendReturn, State.INITIAL, TokenType.BlockComment); });

            table.Add(State.IDENTIFIER, '_',   (a) => { return new Action(Step.AppendContinue, State.IDENTIFIER); });
            table.Add(State.IDENTIFIER, Alpha, (a) => { return new Action(Step.AppendContinue, State.IDENTIFIER); });
            table.Add(State.IDENTIFIER, Digit, (a) => { return new Action(Step.AppendContinue, State.IDENTIFIER); });
            table.Add(State.IDENTIFIER, Any,   (a) => { return new Action(Step.RestoreReturn, State.INITIAL, TokenType.Identifier); });

            table.Add(State.INTEGER, Digit, (a) => { return new Action(Step.AppendContinue, State.INTEGER); });
            table.Add(State.INTEGER, Any,   (a) => { return new Action(Step.RestoreReturn, State.INITIAL, TokenType.Integer); });

            table.Add(State.HYPHEN, '>', (a) => { return new Action(Step.AppendReturn, State.INITIAL, TokenType.PointsTo); });
            table.Add(State.HYPHEN, Any, (a) => { return new Action(Step.RestoreReturn, State.INITIAL, TokenType.Symbol); });

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
                State state = State.INITIAL;

                for (int I = 0; I < source.Chars.Count; I++)
                {
                    var character = source.Chars[I];
                    var charclass = character.Char.GetLexicalClass();

                    // given the current state and the character just read 
                    // locate and call a handler function. That will return
                    // information that describes what to do, whether we've recognized
                    // a complete token and what state to enter next.

                    // If there's a handler for this specific char in this state then use it
                    // else see if there's a handler for the general class of the char, finally
                    // see if there's a handler for the 'any' case.

                    var action = table.GetHandler(state, character.Char) ?? table.GetHandler(state, charclass) ?? table.GetHandler(state, Any);

                    if (action == null)
                        throw new InvalidOperationException($"No handler found in state '{state}' for char '{character.Char}' having lexical class '{charclass}'.");

                    var result = action(character);

                    switch (result.Step)
                    {
                        case Step.AppendContinue:
                            {
                                lexeme.Append(character.Char);
                                break;
                            }
                        case Step.AppendReturn:
                            {
                                lexeme.Append(character.Char);
                                yield return new Token(result.TokenType, lexeme.ToString(), character.Line, character.Col);
                                lexeme.Clear();
                                break;
                            }
                        case Step.RestoreReturn:
                            {
                                I--;
                                yield return new Token(result.TokenType, lexeme.ToString(), character.Line, character.Col);
                                lexeme.Clear();
                                break;
                            }
                        case Step.DiscardContinue:
                            {
                                break;
                            }
                        default:
                            throw new InvalidOperationException("really?");
                    }

                    state = result.State; // set our state to whatever the handlder told us.
                }
            }
        }
    }
}