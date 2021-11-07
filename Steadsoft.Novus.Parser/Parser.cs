﻿using Steadsoft.Novus.Scanner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steadsoft.Novus.Parser
{
    /// <summary>
    /// This class implements a recursive descent parser.
    /// </summary>
    /// <remarks>
    /// This class contains the methods needed to parse the language recursively. Each parse method conforms to a basic
    /// pattern that makes debugging a bit easier and helps detect errors that can arise as code is routinely refactored.
    /// The patter is that when a method has recognized a keyword, it pushes the token for that back into the source and
    /// then calls the parser for that keyword. Each keyword parser method then re-reads that token and verifies that it 
    /// is indeed the correct token before proceeding with its parsing.
    /// </remarks>
    public class Parser
    {
        public delegate void DiagnosticEventHandler(object Sender, DiagnosticEventArgs Args);
        private TokenEnumerator<NovusKeywords> source;
        public event DiagnosticEventHandler OnDiagnostic;

        public Parser(TokenEnumerator<NovusKeywords> Source)
        {
            source = Source;
        }

        public Parser(string Input, bool IsPath)
        {
            SourceFile input;

            if (IsPath)
            {
                input = SourceFile.CreateFromFile(Input);
            }
            else
            {
                input = SourceFile.CreateFromString(Input);
            }

            var tokenizer = new Tokenizer<NovusKeywords>(@"..\..\..\TestFiles\csharp.csv");
            source = new TokenEnumerator<NovusKeywords>(tokenizer.Tokenize(input), TokenType.BlockComment, TokenType.LineComment);

        }

        public bool TryParse(out BlockStatement Root)
        {
            int errors = 0;

            Root = null;

            string message;

            var token = source.GetNextToken();

            // We expect any of the following

            // using <qualified-name> ;
            // namespace <qualified-name> { }
            // zero or more of: type <identifier>  [<type-options>] { <type-body> }

            while (token.TokenCode != TokenType.NoMoreTokens)
            {
                if (Root == null)
                {
                    Root = new BlockStatement(token.LineNumber, token.ColNumber);
                }

                switch (token.Keyword)
                {
                    case NovusKeywords.Using:
                        source.PushToken(token);
                        if (TryParseUsing(source, token, out var usingStatement, out message))
                        {
                            Root.AddChild(usingStatement);
                        }
                        else
                        {
                            errors++;
                            OnDiagnostic(this, ParsedBad(usingStatement, message));
                            token = source.SkipToNext(";");
                        }
                        token = source.GetNextToken();
                        continue;

                    case NovusKeywords.Namespace:
                        source.PushToken(token);
                        if (TryParseNamespace(source, token, out var namespaceStatement, out message))
                        {
                            Root.AddChild(namespaceStatement);
                        }
                        else
                        {
                            errors++;
                            OnDiagnostic(this, ParsedBad(namespaceStatement, message));
                            token = source.SkipToNext(";");
                        }
                        token = source.GetNextToken();
                        continue;

                    case NovusKeywords.Type:
                        source.PushToken(token);
                        if (TryParseType(source, token, out var typeStatement, out message))
                        {
                            Root.AddChild(typeStatement);
                        }
                        else
                        {
                            errors++;
                            OnDiagnostic(this, ParsedBad(typeStatement, message));
                            token = source.SkipToNext("}");
                        }
                        token = source.GetNextToken();
                        continue;

                    default:
                        {
                            errors++;
                            OnDiagnostic(this, new DiagnosticEventArgs("Expected one of 'using', 'namespace' or 'type"));
                            break;
                        }
                }
            }

            return errors == 0;

        }
        public bool TryParseUsing(TokenEnumerator<NovusKeywords> source, Token<NovusKeywords> Prior, out UsingStatement Stmt, out string DiagMsg)
        {
            Stmt = null;
            DiagMsg = null;

            StringBuilder builder = new StringBuilder();

            // <ident>;
            // <ident>.
            // <ident>.<ident>;
            // <ident>.<ident>.
            // <ident>.<ident>.<ident>;

            source.CheckExpectedToken(NovusKeywords.Using);

            var token = source.GetNextToken();

            while (true)
            {
                if (token.TokenCode != TokenType.Identifier)
                {
                    DiagMsg = $"Unexpected token {token.Lexeme}";
                    source.PushToken(token);
                    return false;
                }

                builder.Append(token.Lexeme);

                token = source.GetNextToken();

                if (token.TokenCode == TokenType.SemiColon)
                {
                    Stmt = new UsingStatement(Prior.LineNumber, Prior.ColNumber, builder.ToString());
                    return true;
                }

                if (token.Lexeme == ".")
                {
                    builder.Append(".");
                }

                token = source.GetNextToken();

            }
        }
        public bool TryParseNamespace(TokenEnumerator<NovusKeywords> source, Token<NovusKeywords> Prior, out NamespaceStatement Stmt, out string DiagMsg)
        {
            Stmt = null;
            DiagMsg = String.Empty;

            StringBuilder builder = new StringBuilder();

            source.CheckExpectedToken(NovusKeywords.Namespace);

            var token = source.GetNextToken();

            while (token.TokenCode != TokenType.NoMoreTokens)
            {
                if (token.TokenCode != TokenType.Identifier)
                {
                    DiagMsg = $"Unexpected token {token.Lexeme}";
                    source.PushToken(token);
                    return false;
                }

                builder.Append(token.Lexeme);

                token = source.GetNextToken();

                if (token.TokenCode == TokenType.SemiColon)
                {
                    Stmt = new NamespaceStatement(Prior.LineNumber, Prior.ColNumber, builder.ToString());
                    return true;
                }

                if (token.TokenCode == TokenType.LBrace)
                {
                    source.PushToken(token);
                    if (TryParseNamespaceBody(source, out var block, out DiagMsg))
                    {
                        Stmt = new NamespaceStatement(Prior.LineNumber, Prior.ColNumber, builder.ToString());
                        Stmt.AddBlock(block);
                    }

                    return true;
                }


                if (token.Lexeme == ".")
                {
                    builder.Append(".");
                }

                token = source.GetNextToken();

            }

            return false;
        }
        public bool TryParseNamespaceBody(TokenEnumerator<NovusKeywords> source, out BlockStatement Block, out string DiagMsg)
        {
            Block = null;
            DiagMsg = string.Empty;

            var token = source.GetNextToken();

            if (token.TokenCode != TokenType.LBrace)
                throw new InvalidOperationException("Expected token '{' has not been pushed.");

            Block = new BlockStatement(token.LineNumber, token.ColNumber);

            token = source.GetNextToken();

            while (token.TokenCode != TokenType.NoMoreTokens && token.TokenCode != TokenType.RBrace)
            {
                switch (token.Keyword)
                {
                    case NovusKeywords.Namespace:
                        source.PushToken(token);
                        if (TryParseNamespace(source, token, out var namespaceStatement, out DiagMsg))
                        {
                            Block.AddChild(namespaceStatement);
                        }
                        else
                        {
                            OnDiagnostic(this, ParsedBad(namespaceStatement, DiagMsg));
                            token = source.SkipToNext(";");
                        }
                        token = source.GetNextToken();
                        continue;

                    case NovusKeywords.Type:
                        source.PushToken(token);
                        if (TryParseType(source, token, out var typeStatement, out DiagMsg))
                        {
                            Block.AddChild(typeStatement);
                        }
                        else
                        {
                            OnDiagnostic(this, ParsedBad(typeStatement, DiagMsg));
                            token = source.SkipToNext("}");
                        }
                        token = source.GetNextToken();
                        continue;

                    default:
                        OnDiagnostic(this, new DiagnosticEventArgs($"Unexpected token '{token.Lexeme}' found."));
                        break;
                }
            }

            return true;

        }
        public bool TryParseType(TokenEnumerator<NovusKeywords> source, Token<NovusKeywords> Prior, out TypeStatement Stmt, out string DiagMsg)
        {
            Stmt = null;
            DiagMsg = string.Empty;

            source.CheckExpectedToken(NovusKeywords.Type);

            var token = source.GetNextToken();

            if (token.TokenCode != TokenType.Identifier)
            {
                DiagMsg = $"Unexpected token {token.Lexeme}";
                return false;
            }

            var name = token.Lexeme;

            Stmt = new TypeStatement(Prior.LineNumber, Prior.ColNumber, name);

            if (TryParseTypeOptions(source, token, ref Stmt, out DiagMsg))
                return TryParseTypeBody(source, token, ref Stmt, out DiagMsg);

            return false;

        }
        public bool TryParseTypeOptions(TokenEnumerator<NovusKeywords> source, Token<NovusKeywords> Prior, ref TypeStatement Stmt, out string DiagMsg)
        {
            DiagMsg = String.Empty;

            var token = source.GetNextToken();

            while (token.Lexeme != "{")
            {
                if (token.Keyword == NovusKeywords.IsNotKeyword)
                {
                    DiagMsg = $"Unexpected token in type declaration '{token.Lexeme}'";
                    continue;
                }

                Stmt.AddOption(token.Keyword);

                token = source.GetNextToken();
            }

            source.PushToken(token);

            return true;
        }
        public bool TryParseTypeBody(TokenEnumerator<NovusKeywords> source, Token<NovusKeywords> Prior, ref TypeStatement Stmt, out string DiagMsg)
        {
            DiagMsg = string.Empty;

            var token = source.GetNextToken();

            if (token.TokenCode != TokenType.LBrace)
                throw new InvalidOperationException("Expected token '{' has not been pushed.");

            BlockStatement body = new BlockStatement(token.LineNumber, token.ColNumber);

            Stmt.AddBody(body);

            token = source.GetNextToken();

            while (token.TokenCode != TokenType.NoMoreTokens && token.TokenCode != TokenType.RBrace)
            {
                switch (token.Keyword)
                {
                    case NovusKeywords.Type:
                        source.PushToken(token);
                        if (TryParseType(source, token, out var typeStatement, out DiagMsg))
                        {
                            body.AddChild(typeStatement);
                        }
                        else
                        {
                            OnDiagnostic(this, ParsedBad(typeStatement, DiagMsg));
                            token = source.SkipToNext("}");
                        }
                        token = source.GetNextToken();
                        continue;

                    default:
                        OnDiagnostic(this, new DiagnosticEventArgs("Unexpected token {} found."));
                        break;
                }
            }


            //source.SkipToNext("}");

            return true;

        }

        private static DiagnosticEventArgs ParsedGood(Statement Stmt)
        {
            return new DiagnosticEventArgs($"Parsed a {Stmt.GetType().Name} on line {Stmt.Line} at column {Stmt.Col} : '{Stmt.ToString()}'");
        }
        private static DiagnosticEventArgs ParsedBad(Statement Stmt, string Msg)
        {
            return new DiagnosticEventArgs($" Failed to parse a {Stmt.GetType().Name} on line {Stmt.Line} at column {Stmt.Col} ({Msg})");
        }
    }

}