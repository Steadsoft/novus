﻿using Steadsoft.Novus.Parser.Enums;
using Steadsoft.Novus.Parser.Statements;
using Steadsoft.Novus.Parser.Statics;
using Steadsoft.Novus.Scanner;
using System.Reflection;
using System.Text;
using static Steadsoft.Novus.Scanner.TokenType;
using static Steadsoft.Novus.Parser.Enums.NovusKeywords;
using System.Linq;
using System.Collections.Generic;

namespace Steadsoft.Novus.Parser.Classes
{
    /// <summary>
    /// This class implements a recursive descent parser and semantic analyzer.
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
        private static List<NovusKeywords> accessibilities = new List<NovusKeywords>()
        { 
            Public, 
            Private, 
            Protected, 
            Internal
        };

        public delegate void DiagnosticEventHandler(object Sender, DiagnosticEventArgs Args);
        public TokenEnumerator<NovusKeywords> TokenSource { get; private set; }
        public event DiagnosticEventHandler OnDiagnostic;
        private Parser(TokenEnumerator<NovusKeywords> Source)
        {
            TokenSource = Source;
            OnDiagnostic = delegate { };
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
            var source = new TokenEnumerator<NovusKeywords>(tokenizer.Tokenize(sourceFile), BlockComment, LineComment);
            return new Parser(source);
        }
        public bool TrySyntaxPhase(out BlockStatement Tree)
        {
            int errors = 0;

            Tree = null;

            string message;

            var token = TokenSource.GetNextToken();

            // We expect any of the following

            // using <qualified-name> ;
            // namespace <qualified-name> { }
            // zero or more of: type <identifier>  [<type-options>] { <type-body> }

            while (token.TokenCode != NoMoreTokens)
            {
                if (Tree == null)
                {
                    Tree = new BlockStatement(token.LineNumber, token.ColNumber);
                }

                switch (token.Keyword)
                {
                    case Using:
                        TokenSource.PushToken(token);
                        if (TryParseUsing(token, out var usingStatement, out message))
                        {
                            Tree.AddChild(usingStatement);
                        }
                        else
                        {
                            errors++;
                            OnDiagnostic(this, ParsedBad(usingStatement, message));
                            TokenSource.SkipToNext(";");
                        }
                        token = TokenSource.GetNextToken();
                        continue;

                    case Namespace:
                        TokenSource.PushToken(token);
                        if (TryParseNamespace(token, out var namespaceStatement, out message))
                        {
                            Tree.AddChild(namespaceStatement);
                        }
                        else
                        {
                            errors++;
                            OnDiagnostic(this, ParsedBad(namespaceStatement, message));
                            TokenSource.SkipToNext(";");
                        }
                        token = TokenSource.GetNextToken();
                        continue;

                    case Type:
                        TokenSource.PushToken(token);
                        if (TryParseType(token, out var typeStatement, out message))
                        {
                            Tree.AddChild(typeStatement);
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
                            OnDiagnostic(this, new DiagnosticEventArgs(Severity.Error, token.LineNumber, token.ColNumber, "Expected one of 'using', 'namespace' or 'type"));
                            break;
                        }
                }
            }

            return errors == 0;

        }
        public bool TrySemanticPhase(ref BlockStatement Tree)
        {
            if (Tree == null) throw new System.ArgumentNullException(nameof(Tree));

            foreach (var node in Tree.Children)
            {
                switch (node)
                {
                    case BlockStatement stmt:
                        {
                            break;
                        }
                    case DclNamespaceStatement stmt:
                        {
                            AnalyzeNamespace(stmt);
                            break;
                        }
                }
            }

            return true;
        }
        private void AnalyzeNamespace(DclNamespaceStatement Stmt)
        {
            ReportDuplicateDeclarations(Stmt);

            if (Stmt.Block != null)
                foreach (var node in Stmt.Block.Children)
                {
                    switch (node)
                    {
                        case DclNamespaceStatement stmt:
                            {
                                AnalyzeNamespace(stmt);
                                break;
                            }
                        case DclTypeStatement stmt:
                            {
                                AnalyzeType(stmt);
                                break;
                            }
                    }
                }
        }
        private void AnalyzeType(DclTypeStatement Stmt)
        {
            ReportDuplicateDeclarations(Stmt);

            if (Stmt.Options.ContainsMoreThanOneOf(Class, Struct, Singlet))
            {
                OnDiagnostic(this, new DiagnosticEventArgs(Severity.Error, Stmt.Line, Stmt.Col, $"A type must have only one of the keywords 'class', 'struct' or 'singlet'."));
            }
            else
            {
                if (Stmt.Options.TryGetUnique(out var Item, Class, Struct, Singlet))
                {
                    Stmt.DeclaredKind = Item;
                }
            }

            var groups = Stmt.Options.GroupBy(a => a).Where(g => g.Count() > 1);

            if (groups.Any())
            {
                foreach (var group in groups)
                {
                    OnDiagnostic(this, new DiagnosticEventArgs(Severity.Error, Stmt.Line, Stmt.Col, $"Duplicate options '{group.Key.ToString().ToLower()}' in {Stmt.DeclaredKind.ToString().ToLower()} '{Stmt.Name}'."));
                }
            }

            if (Stmt.Block != null)
                foreach (var node in Stmt.Block.Children)
                {
                    switch (node)
                    {
                        case DclTypeStatement stmt:
                            {
                                AnalyzeType(stmt);
                                break;
                            }
                        case DclStatement stmt:
                            {
                                AnalyzeDef(stmt);
                                break;
                            }
                        case AccessibilityBlock stmt:
                            {
                                break;
                            }
                        default:
                            throw new System.NotImplementedException("Fix me.");

                    }
                }
        }
        private void AnalyzeDef(DclStatement Stmt)
        {
            var groups = Stmt.Options.GroupBy(a => a).Where(g => g.Count() > 1);

            switch (Stmt)
            {
                case DclFieldStatement declaration:
                    {
                        if (groups.Any())
                        {
                            foreach (var group in groups)
                            {
                                OnDiagnostic(this, new DiagnosticEventArgs(Severity.Error, declaration.Line, declaration.Col, $"The option '{group.Key.ToString().ToLower()}' appears more than once in the declaration of field '{declaration.Name}'."));
                            }
                        }

                        break;
                    }
                case DclMethodStatement declaration:
                    {
                        if (groups.Any())
                        {
                            foreach (var group in groups)
                            {
                                OnDiagnostic(this, new DiagnosticEventArgs(Severity.Error, declaration.Line, declaration.Col, $"The option '{group.Key.ToString().ToLower()}' appears more than once in the declaration of method '{declaration.Name}'."));
                            }
                        }

                        AnalyzeParameterList(declaration.Parameters);
                        break;
                    }
                default:
                    throw new System.NotImplementedException("Fix me.");
            }
        }
        private void AnalyzeParameterList(System.Collections.Generic.List<Parameter> Params)
        {
            ;
        }
        private bool TryParseUsing(Token<NovusKeywords> Prior, out UsingStatement Stmt, out string DiagMsg)
        {
            Stmt = null;
            DiagMsg = string.Empty;

            StringBuilder builder = new();

            // <ident>;
            // <ident>.
            // <ident>.<ident>;
            // <ident>.<ident>.
            // <ident>.<ident>.<ident>;

            TokenSource.CheckExpectedToken(Using);

            var token = TokenSource.GetNextToken();

            while (true)
            {
                if (token.TokenCode != Identifier)
                {
                    DiagMsg = $"Unexpected token {token.Lexeme}";
                    TokenSource.PushToken(token);
                    return false;
                }

                builder.Append(token.Lexeme);

                token = TokenSource.GetNextToken();

                if (token.TokenCode == SemiColon)
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
        private bool TryParseNamespace(Token<NovusKeywords> Prior, out DclNamespaceStatement Stmt, out string DiagMsg)
        {
            Stmt = null;
            DiagMsg = string.Empty;

            StringBuilder builder = new();

            TokenSource.CheckExpectedToken(Namespace);

            var token = TokenSource.GetNextToken();

            while (token.TokenCode != NoMoreTokens)
            {
                if (token.TokenCode != Identifier)
                {
                    DiagMsg = $"Unexpected token {token.Lexeme}";
                    TokenSource.PushToken(token);
                    return false;
                }

                builder.Append(token.Lexeme);

                token = TokenSource.GetNextToken();

                if (token.TokenCode == SemiColon)
                {
                    Stmt = new DclNamespaceStatement(Prior.LineNumber, Prior.ColNumber, builder.ToString());
                    return true;
                }

                if (token.TokenCode == LBrace)
                {
                    TokenSource.PushToken(token);
                    if (TryParseNamespaceBody(out var block, out DiagMsg))
                    {
                        Stmt = new DclNamespaceStatement(Prior.LineNumber, Prior.ColNumber, builder.ToString());
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

            if (token.TokenCode != LBrace)
                throw new System.InvalidOperationException("Expected token '{' has not been pushed.");

            Block = new BlockStatement(token.LineNumber, token.ColNumber);

            token = TokenSource.GetNextToken();

            while (token.TokenCode != NoMoreTokens && token.TokenCode != RBrace)
            {
                switch (token.Keyword)
                {
                    case Namespace:
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

                    case Type:
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
                        OnDiagnostic(this, new DiagnosticEventArgs(Severity.Error, token.LineNumber, token.ColNumber, $"Unexpected token '{token.Lexeme}' found."));
                        break;
                }
            }

            return true;

        }
        private bool TryParseType(Token<NovusKeywords> Prior, out DclTypeStatement Stmt, out string DiagMsg)
        {
            Stmt = null;

            TokenSource.CheckExpectedToken(Type);

            var token = TokenSource.GetNextToken();

            if (token.TokenCode != Identifier)
            {
                DiagMsg = $"Unexpected token {token.Lexeme}";
                return false;
            }

            var name = token.Lexeme;

            Stmt = new DclTypeStatement(Prior.LineNumber, Prior.ColNumber, name);

            if (TryParseTypeOptions(token, ref Stmt, out DiagMsg))
                return TryParseTypeBody(token, ref Stmt, out DiagMsg);

            return false;

        }
        private bool TryParseTypeOptions(Token<NovusKeywords> Prior, ref DclTypeStatement Stmt, out string DiagMsg)
        {
            DiagMsg = string.Empty;

            var token = TokenSource.GetNextToken();

            while (token.Lexeme != "{")
            {
                if (token.Keyword == IsNotKeyword)
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
        private bool TryParseTypeBody(Token<NovusKeywords> Prior, ref DclTypeStatement Stmt, out string DiagMsg)
        {
            DiagMsg = string.Empty;

            var token = TokenSource.GetNextToken();

            if (token.TokenCode != LBrace)
                throw new System.InvalidOperationException("Expected token '{' has not been pushed.");

            BlockStatement body = new(token.LineNumber, token.ColNumber);

            Stmt.AddBody(body);

            token = TokenSource.GetNextToken();

            while (token.TokenCode != NoMoreTokens && token.TokenCode != RBrace)
            {
                switch (token.Keyword)
                {
                    case Type:
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
                    case Def:
                        TokenSource.PushToken(token);
                        if (TryParseDef(token, out var defStatement, Stmt, out DiagMsg))
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
                    case Public:
                    case Internal:
                    case Protected:
                    case Private:
                        TokenSource.PushToken(token);
                        if (TryParseAccessorBlock(token, out var accessorStatement, Stmt, out DiagMsg))
                        {
                            body.AddChild(accessorStatement);
                        }
                        else
                        {
                            OnDiagnostic(this, ParsedBad(accessorStatement, DiagMsg));
                            TokenSource.SkipToNext("}");
                        }
                        token = TokenSource.GetNextToken();
                        continue;
                    default:
                        OnDiagnostic(this, new DiagnosticEventArgs(Severity.Error, token.LineNumber, token.ColNumber, "Unexpected token {} found."));
                        break;
                }
            }


            //source.SkipToNext("}");

            return true;

        }
        private bool TryParseAccessorBlock(Token<NovusKeywords> Prior, out BlockStatement Stmt, DclTypeStatement Parent, out string DiagMsg)
        {
            Stmt = new AccessibilityBlock(Prior.LineNumber, Prior.ColNumber, Accessibility.Unknown);

            DiagMsg = string.Empty;
            List<NovusKeywords> terms = new List<NovusKeywords>();

            var token = TokenSource.GetNextToken();

            if (token.Keyword == IsNotKeyword)
            {
                DiagMsg = $"Unexpected token {token.Lexeme}";
                TokenSource.SkipToNext("}");
                return false;
            }

            if (accessibilities.DoesntContain(token.Keyword))
            {
                DiagMsg = $"Unexpected token {token.Lexeme}";
                TokenSource.SkipToNext("}");
                return false;
            }

            terms.Add(token.Keyword);

            token = TokenSource.GetNextToken();

            if (token.TokenCode != LBrace)
            {
                if (token.Keyword == IsNotKeyword)
                {
                    DiagMsg = $"Unexpected token {token.Lexeme}";
                    TokenSource.SkipToNext("}");
                    return false;
                }

                if (accessibilities.DoesntContain(token.Keyword))
                {
                    DiagMsg = $"Unexpected token {token.Lexeme}";
                    TokenSource.SkipToNext("}");
                    return false;
                }

                terms.Add(token.Keyword);
                token = TokenSource.GetNextToken();
            }

            if (token.TokenCode != LBrace)
            {
                DiagMsg = $"Unexpected token {token.Lexeme}";
                TokenSource.SkipToNext("}");
                return false;
            }

            TokenSource.PushToken(token);

            Accessibility acc;

            if (terms.Count == 1)
            {
                if (terms[0] != Public && terms[0] != Private && terms[0] != Internal && terms[0] != Protected)
                {
                    DiagMsg = $"The accessibility level '{terms[0].ToString().ToLower()}' must not appear alone.";
                    return false;
                }

                acc = (Accessibility)System.Enum.Parse(typeof(Accessibility), terms[0].ToString());
            }
            else
            {
                var ordered = terms.OrderBy(t => t.ToString()).ToList();

                if ((ordered[0] != Internal || ordered[1] != Protected) && (ordered[0] != Private || ordered[1] != Protected))
                {
                    DiagMsg = $"The accessibilities '{ordered[0].ToString().ToLower()}' and '{ordered[1].ToString().ToLower()}' must not appear together.";
                    return false;
                }

                if (ordered[0] == Internal)
                    acc = Accessibility.Protected_Internal;
                else
                    acc = Accessibility.Private_Protected;
            }

            TokenSource.SkipToNext("}");

            Stmt = new AccessibilityBlock(Prior.LineNumber, Prior.ColNumber, acc);

            return true;
        }
        private bool TryParseDef(Token<NovusKeywords> Prior, out DclStatement Stmt, DclTypeStatement Parent, out string DiagMsg)
        {
            Stmt = null;
            DiagMsg = string.Empty;

            TokenSource.CheckExpectedToken(Def);

            var token = TokenSource.GetNextToken();

            if (token.TokenCode != Identifier)
            {
                DiagMsg = $"Unexpected token {token.Lexeme}";
                return false;
            }

            var name = token.Lexeme;

            Stmt = new DclStatement(Prior.LineNumber, Prior.ColNumber, name);

            if (AppearsToBeA.MethodDeclaration(TokenSource))
            {
                if (TryParseMethodDeclaration(token, ref Stmt, Parent, out DiagMsg))
                    return TryParseMethodBody(token, ref Stmt, out DiagMsg);
            }
            else
            {
                if (AppearsToBeA.FieldDeclaration(TokenSource))
                    return TryParseFieldDeclaration(token, ref Stmt, Parent, out DiagMsg);
            }

            return false;

        }
        private bool TryParseFieldDeclaration(Token<NovusKeywords> Prior, ref DclStatement Stmt, DclTypeStatement Parent, out string DiagMsg)
        {
            DiagMsg = string.Empty;

            var token = TokenSource.GetNextToken();

            DclFieldStatement field = new DclFieldStatement(Stmt.Line, Stmt.Col, Stmt.Name, token.Lexeme, Parent);

            token = TokenSource.GetNextToken();

            while (token.TokenCode != SemiColon)
            {
                if (token.Keyword != IsNotKeyword)
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
        private bool TryParseParameterList(Token<NovusKeywords> Prior, ref DclMethodStatement Stmt, out string DiagMsg)
        {
            DiagMsg = string.Empty;

            Token<NovusKeywords> token;

            token = TokenSource.GetNextToken();

            if (token.TokenCode != LPar)
            {
                DiagMsg = $"Unexpected token {token.Lexeme}";
                return false;
            }

            token = TokenSource.GetNextToken();

            while (token.TokenCode != RPar)
            {
                if (token.TokenCode != Identifier)
                {
                    DiagMsg = $"Unexpected token in parameter declaration '{token.Lexeme}'";
                    continue;
                }

                var pname = token.Lexeme;

                token = TokenSource.GetNextToken();

                if (token.TokenCode != Identifier)
                {
                    DiagMsg = $"Unexpected token in parameter declaration '{token.Lexeme}'";
                    continue;
                }

                var typename = token.Lexeme;

                token = TokenSource.GetNextToken();

                switch (token.Keyword)
                {
                    case Ref:
                        {
                            Stmt.AddParameter(new Parameter(pname, typename, PassBy.Ref));
                            break;
                        }
                    case Out:
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

                if (token.TokenCode == Comma)
                {
                    token = TokenSource.GetNextToken();
                    continue;
                }
            }

            return true;

        }
        private bool TryParseMethodDeclaration(Token<NovusKeywords> Prior, ref DclStatement Stmt, DclTypeStatement Parent, out string DiagMsg)
        {
            Token<NovusKeywords> token;

            DclMethodStatement methodDef;

            DiagMsg = string.Empty;

            if (!AppearsToBeA.MethodDeclaration(TokenSource))
            {
                DiagMsg = "Invalid parser method called.";
                return false;
            }

            methodDef = new DclMethodStatement(Stmt.Line, Stmt.Col, Stmt.Name, Parent);

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

            while (token.TokenCode != LBrace)
            {
                if (token.Keyword == IsNotKeyword)
                {
                    ; // error
                }

                methodDef.AddOption(token.Keyword);

                token = TokenSource.GetNextToken();
            }

            TokenSource.PushToken(token);

            return true;
        }
        private bool TryParseMethodBody(Token<NovusKeywords> Prior, ref DclStatement Stmt, out string DiagMsg)
        {
            DiagMsg = string.Empty;
            var token = TokenSource.GetNextToken();

            if (token.TokenCode != LBrace)
                throw new System.InvalidOperationException("Expected token '{' has not been pushed.");

            TokenSource.SkipToNext("}");

            ((DclMethodStatement)Stmt).AddBody(new BlockStatement(Prior.LineNumber, Prior.ColNumber));

            return true;
        }
        private static DiagnosticEventArgs ParsedBad(Statement Stmt, string Msg)
        {
            return new DiagnosticEventArgs(Severity.Error, Stmt.Line, Stmt.Col, $"Failed to parse {Stmt.GetType().Name} ({Msg})");
        }
        /// <summary>
        /// Seraches for duplicate named declarations within the scope defined by the supplied statement.
        /// </summary>
        /// <remarks>
        /// Statements that can contain declarations are namespaces and types.
        /// </remarks>
        /// <param name="Stmt">A statement that can contain declarations.</param>
        private void ReportDuplicateDeclarations(IBlockContainer Stmt)
        {
            var dupeDeclarations = Stmt.Block.Children.
                Where(c => c is DclStatement).
                Cast<DclStatement>().
                GroupBy(dcl => dcl.Name).
                Where(dcl => dcl.Count() > 1);

            foreach (var declaration in dupeDeclarations)
            {
                var firstuse = declaration.OrderBy(ns => ns.Line).Take(1).First();  // the original, legally declared instance of the name.
                var duplicates = declaration.OrderBy(ns => ns.Line).Skip(1); // all the subsequent illegal declarations of the same name.

                foreach (var duplicate in duplicates)
                {
                    OnDiagnostic(this, new DiagnosticEventArgs(Severity.Error, duplicate.Line, duplicate.Col, $"Invalid {duplicate.ShortStatementTypeName} name '{duplicate.Name}' within {Stmt.ShortStatementTypeName} '{Stmt.Name}', there is already a defintion of a {firstuse.ShortStatementTypeName} with this name at line {firstuse.Line}."));
                }
            }
        }
    }
}