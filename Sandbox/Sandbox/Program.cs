using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sandbox
{
    internal partial class Program
    {


        static void Main(string[] args)
        {
            var source = SourceFile.Create(@"..\..\..\Program.cs");

            var tokenizer = new Tokenizer(source);

            foreach (var token in tokenizer.Tokens)
            {
                Console.WriteLine(token.Lexeme);
            }
        }

        public class Tokenizer
        {
            private static Func<Character, Action>[,] table;
            private SourceFile source;

            static Tokenizer()
            {
                table = new Func<Character, Action>[50, 50];

                Add(0, Kind.CR, (a) => { return new Action(Step.DiscardResume, 0); });
                Add(0, Kind.LF, (a) => { return new Action(Step.DiscardResume, 0); });
                Add(0, Kind.Whitespace, (a) => { return new Action(Step.DiscardResume, 0); });
                Add(0, Kind.Letter, (a) => { return new Action(Step.AppendResume, 1); });
                Add(0, Kind.Digit, (a) => { return new Action(Step.AppendResume, 2); });
                Add(0, Kind.Punctuation, (a) => { return new Action(Step.AppendHalt, 0); });
                Add(0, Kind.Brace, (a) => { return new Action(Step.AppendHalt, 0); });
                // Assume its an identifier

                Add(1, Kind.Letter, (a) => { return new Action(Step.AppendResume, 1); });
                Add(1, Kind.Digit, (a) => { return new Action(Step.AppendResume, 1); });
                Add(1, Kind.AnythingElse, (a) => { return new Action(Step.RestoreHalt, 0); });

                // Assume its an integer

                Add(2, Kind.Digit, (a) => { return new Action(Step.AppendResume, 2); });
                Add(2, Kind.AnythingElse, (a) => { return new Action(Step.RestoreHalt, 0); });
            }
            public Tokenizer(SourceFile File)
            {
                this.source = File;
            }

            public IEnumerable<Token> Tokens
            {
                get
                {
                    StringBuilder lexeme = new StringBuilder();
                    int state = 0;

                    for (int I = 0; I < source.Chars.Count; I++)
                    {
                        var character = source.Chars[I];

                        var act = this[state, character.Kind](character);

                        switch (act.Step)
                        {
                            case Step.AppendResume:
                                {
                                    lexeme.Append(character.Char);
                                    break;
                                }
                            case Step.AppendHalt:
                                {
                                    lexeme.Append(character.Char);
                                    yield return new Token(TokenType.Identifier, lexeme.ToString(), character.Line, character.Col);
                                    lexeme.Clear();
                                    break;
                                }
                            case Step.RestoreHalt:
                                {
                                    I--;
                                    yield return new Token(TokenType.Identifier, lexeme.ToString(), character.Line, character.Col);
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

                        state = act.State;
                    }
                }
            }
            private static void Add(int State, Kind Kind, Func<Character, Action> Function)
            {
                table[State, (int)Kind] = Function;
            }

            private Func<Character, Action> this[int State, Kind Kind]
            {
                get
                {
                    return table[State, (int)Kind] == null ? table[State, (int)Kind.AnythingElse] : table[State, (int)Kind];
                }
            }
        }

        //public class HandlerTable
        //{
        //    private Func<Character, Action>[,] table;

        //    public HandlerTable(int NumStates, int NumEvents)
        //    {
        //        table = new Func<Character, Action>[NumStates, NumEvents];
        //    }

        //    public void Add(int State, Kind Kind, Func<Character, Action> Function)
        //    {
        //        table[State, (int)Kind] = Function;
        //    }

        //    public Func<Character, Action> this[int State, Kind Kind]
        //    {
        //        get
        //        {
        //            return table[State, (int)Kind] == null ? table[State, (int)Kind.AnythingElse] : table[State, (int)Kind];
        //        }
        //    }
        //}

        public static class CharHandlers
        {

        }


    }

}