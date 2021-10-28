using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using static Sandbox.LexicalClass;

namespace Sandbox
{
    /// <summary>
    /// Represents a mechanism which can consume C# source and emit language tokens.
    /// </summary>
    public class Tokenizer
    {
        private readonly SparseTable<State, Char, (Step, State, TokenType)> table;
        private SourceFile source;

        public Tokenizer(string CSV)
        {
            table = new();

            using (FileStream fs = File.OpenRead(CSV))
            {
                bool flag;

                using StreamReader sr = new(fs, Encoding.UTF8);
                while (!sr.EndOfStream)
                {
                    var text = sr.ReadLine();

                    if (text == null)
                        break;

                    if (String.IsNullOrWhiteSpace(text))
                        continue;

                    if (text.Contains("//"))
                        continue;

                    var parts = text.Split(',').Select(a => a.Trim()).ToArray();

                    var curstate = (State)Enum.Parse(typeof(State), parts[0]);
                    var step = (Step)Enum.Parse(typeof(Step), parts[2]);
                    var newstate = (State)Enum.Parse(typeof(State), parts[3]);
                    var token = (TokenType)Enum.Parse(typeof(TokenType), parts[4]);

                    if (parts[1].StartsWith('\'') && parts[1].EndsWith('\''))
                    {
                        parts[1] = parts[1].Trim('\'');

                        flag = Char.TryParse(parts[1], out char code);

                        if (flag)
                            table.Add(curstate, code, (step, newstate, token));
                        else
                        {
                            parts[1] = Regex.Unescape(parts[1]);
                            flag = Char.TryParse(parts[1], out code);

                            if (flag)
                                table.Add(curstate, code, (step, newstate, token));
                        }
                    }
                    else
                    {
                        var cls = (LexicalClass)Enum.Parse(typeof(LexicalClass), parts[1]);
                        table.Add(curstate, cls, (step, newstate, token));
                    }
                }
            }
        }
        public Tokenizer(SourceFile File)
        {
            this.source = File;
        }

        public IEnumerable<Token> Tokenize(SourceFile Source)
        {
            if (Source == null) throw new ArgumentNullException(nameof(Source));

            source = Source;

            StringBuilder lexeme = new();
            State state = State.INITIAL;

            var tuple = (Step: Step.AppendReturn, State: State.INITIAL, TokenType: TokenType.Undecided);

            for (int I = 0; I < source.Chars.Count; I++)
            {
                var character = source.Chars[I];
                var charclass = character.Char.GetLexicalClass();


                bool found = table.TryGet(state, character.Char, out tuple) ? true : table.TryGet(state, charclass, out tuple) ? true : table.TryGet(state, Any, out tuple);

                if (found == false)
                    throw new InvalidOperationException($"No handler found in state '{state}' for char '{character.Char}' having lexical class '{charclass}' at L={character.Line} C={character.Col}.");

                switch (tuple.Step)
                {
                    case Step.AppendContinue:
                        {
                            lexeme.Append(character.Char);
                            break;
                        }
                    case Step.AppendReturn:
                        {
                            lexeme.Append(character.Char);
                            yield return new Token(tuple.TokenType, lexeme.ToString(), character.Line, character.Col + 1 - lexeme.Length);
                            lexeme.Clear();
                            break;
                        }
                    case Step.RestoreReturn:
                        {
                            I--;
                            yield return new Token(tuple.TokenType, lexeme.ToString(), character.Line, character.Col - lexeme.Length);
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

                state = tuple.State; // set our state to whatever the handlder told us.
            }
        }
    }
}