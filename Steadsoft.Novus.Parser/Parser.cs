﻿using Steadsoft.Novus.Scanner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        public TokenEnumerator<NovusKeywords> TokenSource { get; private set; }
        public event DiagnosticEventHandler OnDiagnostic;
        private Parser(TokenEnumerator<NovusKeywords> Source)
        {
            TokenSource = Source;
        }
        /// <summary>
        /// Creates a parser instance that includes a lexical analyzer instance.
        /// </summary>
        /// <param name="SourceText">Indicates whether the source to be parsed is in a file or passed as a string.</param>
        /// <param name="Input">The pathname of a file or raw source code.</param>
        /// <param name="Definition">Indicates whether the token defintion file is passed as plain text, a file path or is an embedded resource file.</param>
        /// <param name="Tokens">The pathname of a token defintion file, tha plain text token definitions or the name of the embedded resource.</param>
        /// <returns></returns>
        public static Parser CreateParser(SourceOrigin SourceText, string Input, TokenDefinition Definition, string Tokens)
        {
            SourceFile sourceFile;

            if (SourceText == SourceOrigin.Pathname)
            {
                sourceFile = SourceFile.CreateFromFile(Input);
            }
            else
            {
                sourceFile = SourceFile.CreateFromString(Input);
            }

            var tokenizer = new Tokenizer<NovusKeywords>(Tokens, Definition, Assembly.GetExecutingAssembly());
            var source = new TokenEnumerator<NovusKeywords>(tokenizer.Tokenize(sourceFile), TokenType.BlockComment, TokenType.LineComment);
            return new Parser(source);
        }
        public bool TryParse(out BlockStatement Root)
        {
            int errors = 0;

            Root = null;

            string message;

            var token = TokenSource.GetNextToken();

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
                        TokenSource.PushToken(token);
                        if (TryParseUsing(token, out var usingStatement, out message))
                        {
                            Root.AddChild(usingStatement);
                        }
                        else
                        {
                            errors++;
                            OnDiagnostic(this, ParsedBad(usingStatement, message));
                            TokenSource.SkipToNext(";");
                        }
                        token = TokenSource.GetNextToken();
                        continue;

                    case NovusKeywords.Namespace:
                        TokenSource.PushToken(token);
                        if (TryParseNamespace(token, out var namespaceStatement, out message))
                        {
                            Root.AddChild(namespaceStatement);
                        }
                        else
                        {
                            errors++;
                            OnDiagnostic(this, ParsedBad(namespaceStatement, message));
                            TokenSource.SkipToNext(";");
                        }
                        token = TokenSource.GetNextToken();
                        continue;

                    case NovusKeywords.Type:
                        TokenSource.PushToken(token);
                        if (TryParseType(token, out var typeStatement, out message))
                        {
                            Root.AddChild(typeStatement);
                        }
                        else
                        {
                            errors++;
                            OnDiagnostic(this, ParsedBad(typeStatement, message));
                            TokenSource.SkipToNext("}");
                        }
                        token = TokenSource.GetNextToken();
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
        private bool TryParseUsing(Token<NovusKeywords> Prior, out UsingStatement Stmt, out string DiagMsg)
        {
            Stmt = null;
            DiagMsg = null;

            StringBuilder builder = new();

            // <ident>;
            // <ident>.
            // <ident>.<ident>;
            // <ident>.<ident>.
            // <ident>.<ident>.<ident>;

            TokenSource.CheckExpectedToken(NovusKeywords.Using);

            var token = TokenSource.GetNextToken();

            while (true)
            {
                if (token.TokenCode != TokenType.Identifier)
                {
                    DiagMsg = $"Unexpected token {token.Lexeme}";
                    TokenSource.PushToken(token);
                    return false;
                }

                builder.Append(token.Lexeme);

                token = TokenSource.GetNextToken();

                if (token.TokenCode == TokenType.SemiColon)
                {
                    Stmt = new UsingStatement(Prior.LineNumber, Prior.ColNumber, builder.ToString());
                    return true;
                }

                if (token.Lexeme == ".")
                {
                    builder.Append('.');
                }

                token = TokenSource.GetNextToken();

            }
        }
        private bool TryParseNamespace(Token<NovusKeywords> Prior, out NamespaceStatement Stmt, out string DiagMsg)
        {
            Stmt = null;
            DiagMsg = String.Empty;

            StringBuilder builder = new();

            TokenSource.CheckExpectedToken(NovusKeywords.Namespace);

            var token = TokenSource.GetNextToken();

            while (token.TokenCode != TokenType.NoMoreTokens)
            {
                if (token.TokenCode != TokenType.Identifier)
                {
                    DiagMsg = $"Unexpected token {token.Lexeme}";
                    TokenSource.PushToken(token);
                    return false;
                }

                builder.Append(token.Lexeme);

                token = TokenSource.GetNextToken();

                if (token.TokenCode == TokenType.SemiColon)
                {
                    Stmt = new NamespaceStatement(Prior.LineNumber, Prior.ColNumber, builder.ToString());
                    return true;
                }

                if (token.TokenCode == TokenType.LBrace)
                {
                    TokenSource.PushToken(token);
                    if (TryParseNamespaceBody(out var block, out DiagMsg))
                    {
                        Stmt = new NamespaceStatement(Prior.LineNumber, Prior.ColNumber, builder.ToString());
                        Stmt.AddBlock(block);
                    }

                    return true;
                }


                if (token.Lexeme == ".")
                {
                    builder.Append('.');
                }

                token = TokenSource.GetNextToken();

            }

            return false;
        }
        private bool TryParseNamespaceBody(out BlockStatement Block, out string DiagMsg)
        {
            Block = null;
            DiagMsg = string.Empty;

            var token = TokenSource.GetNextToken();

            if (token.TokenCode != TokenType.LBrace)
                throw new InvalidOperationException("Expected token '{' has not been pushed.");

            Block = new BlockStatement(token.LineNumber, token.ColNumber);

            token = TokenSource.GetNextToken();

            while (token.TokenCode != TokenType.NoMoreTokens && token.TokenCode != TokenType.RBrace)
            {
                switch (token.Keyword)
                {
                    case NovusKeywords.Namespace:
                        TokenSource.PushToken(token);
                        if (TryParseNamespace(token, out var namespaceStatement, out DiagMsg))
                        {
                            Block.AddChild(namespaceStatement);
                        }
                        else
                        {
                            OnDiagnostic(this, ParsedBad(namespaceStatement, DiagMsg));
                            TokenSource.SkipToNext(";");
                        }
                        token = TokenSource.GetNextToken();
                        continue;

                    case NovusKeywords.Type:
                        TokenSource.PushToken(token);
                        if (TryParseType(token, out var typeStatement, out DiagMsg))
                        {
                            Block.AddChild(typeStatement);
                        }
                        else
                        {
                            OnDiagnostic(this, ParsedBad(typeStatement, DiagMsg));
                            TokenSource.SkipToNext("}");
                        }
                        token = TokenSource.GetNextToken();
                        continue;

                    default:
                        OnDiagnostic(this, new DiagnosticEventArgs($"Unexpected token '{token.Lexeme}' found."));
                        break;
                }
            }

            return true;

        }
        private bool TryParseType(Token<NovusKeywords> Prior, out TypeStatement Stmt, out string DiagMsg)
        {
            Stmt = null;

            TokenSource.CheckExpectedToken(NovusKeywords.Type);

            var token = TokenSource.GetNextToken();

            if (token.TokenCode != TokenType.Identifier)
            {
                DiagMsg = $"Unexpected token {token.Lexeme}";
                return false;
            }

            var name = token.Lexeme;

            Stmt = new TypeStatement(Prior.LineNumber, Prior.ColNumber, name);

            if (TryParseTypeOptions(token, ref Stmt, out DiagMsg))
                return TryParseTypeBody(token, ref Stmt, out DiagMsg);

            return false;

        }
        private bool TryParseTypeOptions(Token<NovusKeywords> Prior, ref TypeStatement Stmt, out string DiagMsg)
        {
            DiagMsg = String.Empty;

            var token = TokenSource.GetNextToken();

            while (token.Lexeme != "{")
            {
                if (token.Keyword == NovusKeywords.IsNotKeyword)
                {
                    DiagMsg = $"Unexpected token in type declaration '{token.Lexeme}'";
                    continue;
                }

                Stmt.AddOption(token.Keyword);

                token = TokenSource.GetNextToken();
            }

            TokenSource.PushToken(token);

            return true;
        }
        private bool TryParseTypeBody(Token<NovusKeywords> Prior, ref TypeStatement Stmt, out string DiagMsg)
        {
            DiagMsg = string.Empty;

            var token = TokenSource.GetNextToken();

            if (token.TokenCode != TokenType.LBrace)
                throw new InvalidOperationException("Expected token '{' has not been pushed.");

            BlockStatement body = new(token.LineNumber, token.ColNumber);

            Stmt.AddBody(body);

            token = TokenSource.GetNextToken();

            while (token.TokenCode != TokenType.NoMoreTokens && token.TokenCode != TokenType.RBrace)
            {
                switch (token.Keyword)
                {
                    case NovusKeywords.Type:
                        TokenSource.PushToken(token);
                        if (TryParseType(token, out var typeStatement, out DiagMsg))
                        {
                            body.AddChild(typeStatement);
                        }
                        else
                        {
                            OnDiagnostic(this, ParsedBad(typeStatement, DiagMsg));
                            TokenSource.SkipToNext("}");
                        }
                        token = TokenSource.GetNextToken();
                        continue;
                    case NovusKeywords.Def:
                        TokenSource.PushToken(token);
                        if (TryParseDef(token, out var defStatement, out DiagMsg))
                        {
                            body.AddChild(defStatement);
                        }
                        else
                        {
                            OnDiagnostic(this, ParsedBad(defStatement, DiagMsg));
                            TokenSource.SkipToNext("}");
                        }
                        token = TokenSource.GetNextToken();
                        continue;
                    default:
                        OnDiagnostic(this, new DiagnosticEventArgs("Unexpected token {} found."));
                        break;
                }
            }


            //source.SkipToNext("}");

            return true;

        }
        private bool TryParseDef(Token<NovusKeywords> Prior, out DefStatement Stmt, out string DiagMsg)
        {
            Stmt = null;
            DiagMsg = String.Empty;

            TokenSource.CheckExpectedToken(NovusKeywords.Def);

            var token = TokenSource.GetNextToken();

            if (token.TokenCode != TokenType.Identifier)
            {
                DiagMsg = $"Unexpected token {token.Lexeme}";
                return false;
            }

            var name = token.Lexeme;

            Stmt = new DefStatement(Prior.LineNumber, Prior.ColNumber, name);

            if (AppearsToBeA.MethodDeclaration(TokenSource))
            {
                if (TryParseMethodDeclaration(token, ref Stmt, out DiagMsg))
                    return TryParseMethodBody(token, ref Stmt, out DiagMsg);
            }
            else
            {
                if (AppearsToBeA.FieldDeclaration(TokenSource))
                    return TryParseFieldDeclaration(token, ref Stmt, out DiagMsg);
            }

            return false;

        }
        private bool TryParseFieldDeclaration(Token<NovusKeywords> Prior, ref DefStatement Stmt, out string DiagMsg)
        {
            DiagMsg = String.Empty;

            var token = TokenSource.GetNextToken();

            DefFieldStatement field = new DefFieldStatement(Stmt.Line, Stmt.Col, Stmt.Name, token.Lexeme);

            token = TokenSource.GetNextToken();

            while (token.TokenCode != TokenType.SemiColon)
            {
                if (token.Keyword != NovusKeywords.IsNotKeyword)
                {
                    field.AddOption(token.Keyword);
                    token = TokenSource.GetNextToken();
                }
                else
                {
                    return false;
                }
            }

            Stmt = field;

            return true;
        }
        private bool TryParseParameterList(Token<NovusKeywords> Prior, ref DefMethodStatement Stmt, out string DiagMsg)
        {
            Parameter param;

            DiagMsg = String.Empty;

            Token<NovusKeywords> token;

            token = TokenSource.GetNextToken();

            if (token.TokenCode != TokenType.LPar)
            {
                DiagMsg = $"Unexpected token {token.Lexeme}";
                return false;
            }

            token = TokenSource.GetNextToken();

            while (token.TokenCode != TokenType.RPar)
            {
                if (token.TokenCode != TokenType.Identifier)
                {
                    DiagMsg = $"Unexpected token in parameter declaration '{token.Lexeme}'";
                    continue;
                }

                var pname = token.Lexeme;

                token = TokenSource.GetNextToken();

                if (token.TokenCode != TokenType.Identifier)
                {
                    DiagMsg = $"Unexpected token in parameter declaration '{token.Lexeme}'";
                    continue;
                }

                var typename = token.Lexeme;

                token = TokenSource.GetNextToken();

                switch (token.Keyword)
                {
                    case NovusKeywords.Ref:
                        {
                            Stmt.AddParameter(new Parameter(pname, typename, PassBy.Ref));
                            break;
                        }
                    case NovusKeywords.Out:
                        {
                            Stmt.AddParameter(new Parameter(pname, typename, PassBy.Out));
                            break;
                        }

                    default:
                        {
                            TokenSource.PushToken(token);
                            Stmt.AddParameter(new Parameter(pname, typename, PassBy.Value));
                            break;
                        }
                }

                // if next token is a comma, go around again..

                token = TokenSource.GetNextToken();

                if (token.TokenCode == TokenType.Comma)
                {
                    token = TokenSource.GetNextToken();
                    continue;
                }
            }

            return true;

        }
        private bool TryParseMethodDeclaration(Token<NovusKeywords> Prior, ref DefStatement Stmt, out string DiagMsg)
        {
            Token<NovusKeywords> token;

            DefMethodStatement methodDef = null; ;

            DiagMsg = String.Empty;

            if (!AppearsToBeA.MethodDeclaration(TokenSource))
            {
                DiagMsg = "Invalid parser method called.";
                return false;
            }

            methodDef = new DefMethodStatement(Stmt);

            TryParseParameterList(Prior, ref methodDef, out DiagMsg);

            Stmt = methodDef;

            var tokens = TokenSource.PeekNextTokens(3);

            // Is the next sequence a method return type?

            if (AppearsToBeA.FunctionReturnType(TokenSource))
            {
                TokenSource.GetNextToken();
                TokenSource.GetNextToken();
                methodDef.Returns = tokens[1].Lexeme;
                TokenSource.GetNextToken(); // Consume the closing rpar
            }

            // OK now look for any additional keywords (options like public, abstract etc)

            token = TokenSource.GetNextToken(); 

            while (token.TokenCode != TokenType.LBrace)
            {
                if (token.Keyword == NovusKeywords.IsNotKeyword)
                {
                    ; // error
                }

                methodDef.AddOption(token.Keyword);

                token = TokenSource.GetNextToken();
            }

            TokenSource.PushToken(token);

            return true;
        }
        private bool TryParseMethodBody(Token<NovusKeywords> Prior, ref DefStatement Stmt, out string DiagMsg)
        {
            DiagMsg = String.Empty;
            var token = TokenSource.GetNextToken();

            if (token.TokenCode != TokenType.LBrace)
                throw new InvalidOperationException("Expected token '{' has not been pushed.");

            TokenSource.SkipToNext("}");

            ((DefMethodStatement)(Stmt)).AddBody(new BlockStatement(Prior.LineNumber, Prior.ColNumber));

            return true;
        }
        private static DiagnosticEventArgs ParsedBad(Statement Stmt, string Msg)
        {
            return new DiagnosticEventArgs($" Failed to parse a {Stmt.GetType().Name} on line {Stmt.Line} at column {Stmt.Col} ({Msg})");
        }
    }
}