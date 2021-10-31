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
    /// Represents a mechanism which can consume source and emit language tokens.
    /// </summary>
    /// <remarks>
    /// A tokenizer instance is initialized with a handler map specific to some chosen
    /// language and then its 'Toeknize' method can be called with a source file to
    /// transform that source into a token stream.
    /// </remarks>
    public class Tokenizer
    {
        private readonly SparseTable<State, Char, (Step, State, TokenType)> table;
        private SourceFile source;

        /// <summary>
        /// Creates a new instance of a tokenizer and initialises its state machine
        /// from the CSV file that you supply.
        /// </summary>
        /// <param name="CSV"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public Tokenizer(string CSV)
        {
            if (string.IsNullOrWhiteSpace(CSV)) throw new ArgumentNullException(nameof(CSV));

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
                        var len = parts[1].Length;

                        parts[1] = parts[1].Substring(1).Substring(0, len - 2);

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

        /// <summary>
        /// Transforms the supplied source into a stream of tokens.
        /// </summary>
        /// <param name="Source"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public IEnumerable<Token> Tokenize(SourceFile Source)
        {
            if (Source == null) throw new ArgumentNullException(nameof(Source));

            int start_line = 0;
            int start_col = 0;

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

                if (state == State.INITIAL)
                {
                    start_line = character.Line;
                    start_col = character.Col;
                }

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
                            yield return new Token(tuple.TokenType, lexeme.ToString(), start_line, start_col);
                            lexeme.Clear();
                            break;
                        }
                    case Step.RewindReturn:
                        {
                            I--;
                            yield return new Token(tuple.TokenType, lexeme.ToString(), start_line, start_col);
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