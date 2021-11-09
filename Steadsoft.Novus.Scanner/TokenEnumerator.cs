﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Steadsoft.Novus.Scanner
{
    public class TokenEnumerator<T> where T : struct, System.Enum
    {
        private readonly IEnumerator<Token<T>> enumerator;
        private readonly TokenType[] Skips;
        private readonly Stack<Token<T>> pushed = new();
        public TokenEnumerator(IEnumerable<Token<T>> Source, params TokenType[] Skips)
        {
            enumerator = Source.GetEnumerator();
            this.Skips = Skips;
        }

        public void CheckExpectedToken(T Type) 
        {
            var token = GetNextToken();

            if (Convert.ToInt32(token.Keyword) != Convert.ToInt32(Type))
                throw new InternalErrorException($"Expected keyword token '{Type}' has not been pushed.");

            return;
        }

        public bool NextTokensAre(params TokenType[] Tokens)
        {
            var list = PeekNextTokens(Tokens.Length).ToArray();

            for (int I=0; I < Tokens.Length; I++)
            {
                if (Tokens[I] != list[I].TokenCode)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Returns the next 'n' tokens in order, without consuming them from the input.
        /// </summary>
        /// <param name="N"></param>
        /// <returns></returns>
        public List<Token<T>> PeekNextTokens(int N)
        {
            var list = new List<Token<T>>();

            for (int I=0; I < N; I++)
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
        public Token<T> GetNextToken()
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
                if (Skips.Contains(enumerator.Current.TokenCode) == false)
                    return enumerator.Current;
            }

            return new Token<T>(TokenType.NoMoreTokens, "", 0, 0);
        }

        /// <summary>
        /// Returns the supplied token to the input, no check is made so be careful!
        /// </summary>
        /// <param name="Token"></param>
        public void PushToken(Token<T> Token)
        {
            pushed.Push(Token);
        }

        public void PushTokens(List<Token<T>> Tokens)
        {
            for (int I = Tokens.Count - 1; I >= 0; I--)
            {
                pushed.Push(Tokens[I]);
            }
        }

        public void SkipToNext(string Lexeme)
        {
            var token = GetNextToken();

            while (token.Lexeme != Lexeme && token.TokenCode != TokenType.NoMoreTokens)
            {
                token = GetNextToken();
            }
        }
    }
}