using Steadsoft.Novus.Parser.Enums;
using Steadsoft.Novus.Scanner.Enums;
using Steadsoft.Novus.Parser.Statements;
using Steadsoft.Novus.Parser.Statics;
using System.Reflection;
using System.Text;
using static Steadsoft.Novus.Scanner.Enums.TokenType;
using static Steadsoft.Novus.Scanner.Enums.Keywords;
using System.Linq;
using System.Collections.Generic;
using Steadsoft.Novus.Scanner.Classes;
using System.IO;

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
        private Tokenizer<Keywords> tokenizer;
        public string SourceName { get; private set; }
        private static List<Keywords> accessibilities = new List<Keywords>()
        {
            Public,
            Private,
            Protected,
            Internal
        };
        public delegate void DiagnosticEventHandler(object Sender, DiagnosticEventArgs Args);
        public TokenEnumerator TokenSource { get; private set; }
        public event DiagnosticEventHandler OnDiagnostic;
        private Parser(TokenEnumerator Source, Tokenizer<Keywords> Tokenizer)
        {
            TokenSource = Source;
            this.tokenizer = Tokenizer;
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
                sourceFile = SourceFile.CreateFromText(Input);
            }

            var tokenizer = new Tokenizer<Keywords>(Tokens, Definition, Assembly.GetExecutingAssembly());
            var source = new TokenEnumerator(tokenizer, sourceFile, BlockComment, LineComment);
            var p = new Parser(source, tokenizer);

            if (SourceText == SourceOrigin.Pathname)
                p.SourceName = Path.GetFullPath(Input);
            else
                p.SourceName = "Raw Text";

            return p;
        }
        public bool TrySyntaxPhase(out DclNamespaceStatement Tree)
        {
            int errors = 0;

            Tree = null;

            string message;

            // Create an imaginary global namespace:

            DclNamespaceStatement global = new DclNamespaceStatement(null, 0, 0, "@global");

            var token = TokenSource.PeekNextToken();

            // We expect any of the following

            // using <qualified-name> ;
            // namespace <qualified-name> { }
            // zero or more of: type <identifier>  [<type-options>] { <type-body> }

            while (token.TokenType != NoMoreTokens)
            {
                if (Tree == null)
                {
                    Tree = global; // new BlockStatement(token.LineNumber, token.ColNumber);
                }

                switch (token.Keyword)
                {
                    case Using:
                        if (TryParseUsing(token, global, out var usingStatement, out message))
                        {
                            Tree.AddChild(usingStatement);
                        }
                        else
                        {
                            errors++;
                            OnDiagnostic(this, ParsedBad(usingStatement, message));
                            TokenSource.SkipToNext(";");
                        }
                        break;

                    case Namespace:
                        if (TryParseNamespace(token, global, out var namespaceStatement, out message)) // no parent for the outermost namespace
                        {
                            Tree.AddChild(namespaceStatement);
                        }
                        else
                        {
                            errors++;
                            OnDiagnostic(this, ParsedBad(namespaceStatement, message));
                            TokenSource.SkipToNext(";");
                        }
                        break;

                    case Type:
                        if (TryParseTypeDeclaration(token, global, out var typeStatement, out message))
                        {
                            Tree.AddChild(typeStatement);
                        }
                        else
                        {
                            errors++;
                            OnDiagnostic(this, ParsedBad(typeStatement, message));
                            TokenSource.SkipToNext("}");
                        }
                        break;
                    default:
                        {
                            errors++;
                            OnDiagnostic(this, new DiagnosticEventArgs(Severity.Error, token.LineNumber, token.ColNumber, $"Unexpected token '{token.Lexeme}', expected one of 'using', 'namespace' or 'type'."));
                            break;
                        }
                }

                token = TokenSource.PeekNextToken();
            }

            return errors == 0;

        }
        public bool TrySemanticPhase(DclNamespaceStatement Tree)
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
        /// <summary>
        /// Analyzes and reports errors that are discovered within the optional keywords that can appear in various types of declarations.
        /// </summary>
        /// <param name="Stmt"></param>
        /// <param name="ReportErrors"></param>
        private void AnalyzeDclOptions(DclStatement Stmt, bool ReportErrors = true)
        {
            // This method is called by both the parser and the semantic checker.
            // It is called by the parser because we must know the specific kind
            // of type that's being declared in order to parse it, for example an enum
            // contains fundamentally different kinds of members.

            switch (Stmt)
            {
                case DclTypeStatement dclTypeStatement:
                    {
                        if (Stmt.Options.ContainsMoreThanOneOf(Class, Struct, Singlet, Enum, Interface))
                        {
                            if (ReportErrors)
                            {
                                OnDiagnostic(this, new DiagnosticEventArgs(Severity.Error, Stmt.Line, Stmt.Col, $"Unable to determine type-kind, a type must have only one of the kinds 'class', 'struct', 'singlet' or 'enum'."));
                            }
                        }
                        else
                        {
                            if (Stmt.Options.TryGetUnique(out var Item, Class, Struct, Singlet, Enum, Interface))
                            {
                                dclTypeStatement.TypeKind = Item;
                            }
                            else
                            {
                                if (ReportErrors)
                                    OnDiagnostic(this, new DiagnosticEventArgs(Severity.Warning, Stmt.Line, Stmt.Col, $"No type-kind specified for type '{Stmt.DeclaredName}', assuming 'class'."));
                                dclTypeStatement.TypeKind = Class;
                            }
                        }

                        var groups = Stmt.Options.GroupBy(a => a).Where(g => g.Count() > 1);

                        if (groups.Any())
                        {
                            foreach (var group in groups)
                            {
                                if (ReportErrors)
                                {
                                    OnDiagnostic(this, new DiagnosticEventArgs(Severity.Error, Stmt.Line, Stmt.Col, $"Duplicate options '{group.Key.ToString().ToLower()}' in {dclTypeStatement.TypeKind.ToString().ToLower()} '{Stmt.DeclaredName}'."));
                                }
                            }
                        }

                        // Now analyze each specific type according to their own specific rules.

                        if (ReportErrors)
                            switch (dclTypeStatement.TypeKind)
                            {
                                case Enum:
                                case Interface:
                                case Struct:
                                case Singlet:
                                    {
                                        if (dclTypeStatement.Options.ContainsOnly(dclTypeStatement.TypeKind, New, Public, Protected, Internal, Private) == false)
                                        {
                                            OnDiagnostic(this, new DiagnosticEventArgs(Severity.Error, Stmt.Line, Stmt.Col, $"Only the options 'new, public, protected, internal and private' may appear in the declaration of the {dclTypeStatement.TypeKind.ToString().ToLower()} '{Stmt.DeclaredName}'."));
                                        }
                                        break;
                                    }
                                case Class:
                                    {
                                        if (dclTypeStatement.Options.ContainsOnly(Class, New, Public, Protected, Internal, Private, Abstract, Sealed) == false)
                                        {
                                            OnDiagnostic(this, new DiagnosticEventArgs(Severity.Error, Stmt.Line, Stmt.Col, $"Only the options 'new, public, protected, internal, private and abstract' may appear in the declaration of the {dclTypeStatement.TypeKind.ToString().ToLower()} '{Stmt.DeclaredName}'."));
                                        }
                                        break;
                                    }
                                case IsNotAKeyword:
                                    {
                                        break;
                                    }
                                default:
                                    {
                                        break;
                                    }
                            }

                        break;
                    }
            }
        }
        private void AnalyzeType(DclTypeStatement Stmt)
        {
            ReportDuplicateDeclarations(Stmt);

            AnalyzeDclOptions(Stmt);

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
                                OnDiagnostic(this, new DiagnosticEventArgs(Severity.Error, declaration.Line, declaration.Col, $"The option '{group.Key.ToString().ToLower()}' appears more than once in the declaration of field '{declaration.DeclaredName}'."));
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
                                OnDiagnostic(this, new DiagnosticEventArgs(Severity.Error, declaration.Line, declaration.Col, $"The option '{group.Key.ToString().ToLower()}' appears more than once in the declaration of method '{declaration.DeclaredName}'."));
                            }
                        }

                        AnalyzeParameterList(declaration.Parameters);
                        break;
                    }
                case DclEnumMemberStatement declaration:
                    {
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
        private bool TryParseUsing(Token Prior, IContainer Parent, out UsingStatement Stmt, out string DiagMsg)
        {
            Stmt = null;
            DiagMsg = string.Empty;

            // TODO we will need to eventually load external .Net assemblies if 'using' is to be helpful. Until then we have no way to resolve decalred name to actual entities outside of the source.

            StringBuilder builder = new();

            TokenSource.VerifyExpectedToken(Using, out var token);

            token = TokenSource.GetNextToken();

            while (true)
            {
                if (token.TokenType != Identifier)
                {
                    DiagMsg = $"Unexpected token {token.Lexeme}";
                    TokenSource.PushToken(token);
                    return false;
                }

                builder.Append(token.Lexeme);

                token = TokenSource.GetNextToken();

                if (token.TokenType == SemiColon)
                {
                    Stmt = new UsingStatement(Parent, Prior.LineNumber, Prior.ColNumber, builder.ToString());
                    return true;
                }

                if (token.Lexeme == ".")
                {
                    builder.Append('.');
                }

                token = TokenSource.GetNextToken();

            }
        }
        private bool TryParseNamespace(Token Prior, IContainer Parent, out DclNamespaceStatement Stmt, out string DiagMsg)
        {
            Stmt = null;
            DiagMsg = string.Empty;

            StringBuilder builder = new();

            TokenSource.VerifyExpectedToken(Namespace, out var token);

            token = TokenSource.GetNextToken();

            while (token.TokenType != NoMoreTokens)
            {
                if (token.TokenType != Identifier)
                {
                    DiagMsg = $"Unexpected token {token.Lexeme}";
                    TokenSource.PushToken(token);
                    return false;
                }

                builder.Append(token.Lexeme);

                token = TokenSource.GetNextToken();

                if (token.TokenType == SemiColon)
                {
                    Stmt = new DclNamespaceStatement(Parent, Prior.LineNumber, Prior.ColNumber, builder.ToString());
                    return true;
                }

                if (token.TokenType == BraceOpen)
                {
                    TokenSource.PushToken(token);
                    Stmt = new DclNamespaceStatement(Parent, Prior.LineNumber, Prior.ColNumber, builder.ToString());
                    if (TryParseNamespaceBody(Stmt, out var block, out DiagMsg))
                    {
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
        private bool TryParseNamespaceBody(DclNamespaceStatement Parent, out BlockStatement Block, out string DiagMsg)
        {
            Block = null;
            DiagMsg = string.Empty;

            TokenSource.VerifyExpectedToken(BraceOpen, out var token);

            Block = new BlockStatement(token.LineNumber, token.ColNumber);

            token = TokenSource.GetNextToken();

            while (token.TokenType != NoMoreTokens && token.TokenType != BraceClose)
            {
                switch (token.Keyword)
                {
                    case Namespace:
                        TokenSource.PushToken(token);
                        if (TryParseNamespace(token, Parent, out var namespaceStatement, out DiagMsg))
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
                        if (TryParseTypeDeclaration(token, Parent, out var typeStatement, out DiagMsg))
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
        /// <summary>
        /// Tries tp arse a name that is an identifier possibly followed by a generic argument list.
        /// </summary>
        /// <param name="Prior"></param>
        /// <param name="GenericName"></param>
        /// <param name="DiagMsg"></param>
        /// <returns></returns>
        private bool TryParsePossiblyGenericName(Token Prior, out GenericName GenericName, out string DiagMsg)
        {
            GenericName = null;
            DiagMsg = null;

            var token = TokenSource.GetNextToken();

            if (token.TokenType != Identifier)
            {
                DiagMsg = $"Unexpected token {token.Lexeme}";
                return false;
            }

            GenericName = new GenericName(token.Lexeme);

            token = TokenSource.GetNextToken();

            if (token.TokenType != Lesser)
            {
                TokenSource.PushToken(token); // nothing more to do
                return true;
            }

            // at thus point we've seen : somename<

            TokenSource.PushToken(token);

            return TryParseGenericArgList(Prior, GenericName.GenericArgList, out DiagMsg);

        }
        private bool TryParseGenericArgList(Token Prior, GenericArgList Args, out string DiagMsg) 
        {
            // TODO Parsing comma lists can be generified by having a generic loop that handles and consumes the comma and calls a supplied Func to do the parsing specific to the kind of items in the commalist.

            DiagMsg = null;

            TokenSource.VerifyExpectedToken(TokenType.Lesser, out var token);

            try
            {
                // The pair of chars >> represent a right shift but also appear at the end
                // of a generic argument list sometimes, in the latter case we want these
                // to be seen as distinct > chars. The tokenizer allows this by letting us
                // temporarily set a hint during parsing, this is present only briefly as
                // we part certain constructs. This is signficant benefit because it removes the
                // need for the parser to understand token structure and removes the fiddly 
                // logic from the parsing code.

                TokenSource.PushHint(ParsingHint.SplitRightShift);

                while (token.TokenType != Greater)
                {
                    token = TokenSource.GetNextToken();

                    if (token.TokenType != Identifier)
                        return false;

                    var genericArg = new GenericArg(token.Lexeme);

                    Args.Add(genericArg);

                    token = TokenSource.GetNextToken();

                    switch (token.TokenType)
                    {
                        case TokenType.Comma:
                            {
                                continue;
                            }

                        case TokenType.Lesser:
                            {
                                TokenSource.PushToken(token);
                                if (TryParseGenericArgList(Prior, genericArg.GenericArgs, out DiagMsg) == false)
                                    return false;
                                token = TokenSource.GetNextToken();
                                continue;
                            }
                        case TokenType.Greater:
                            {
                                return true;
                            }
                    }

                }
            }

            finally
            {
                TokenSource.PopHint();
            }

            return true;
        }
        private bool TryParseDclOptions(Token Prior, DclStatement Stmt, out string DiagMsg)
        {
            bool parsed = true;

            DiagMsg = string.Empty;

            var token = TokenSource.GetNextToken();

            while (token.TokenType != BraceOpen && token.TokenType != SemiColon)
            {
                if (token.Keyword == IsNotAKeyword)
                {
                    DiagMsg = $"Unexpected token in type declaration '{token.Lexeme}'";
                    parsed = false;
                }
                else
                {
                    Stmt.AddOption(token.Keyword);
                }

                token = TokenSource.GetNextToken();
            }

            if (token.TokenType == BraceOpen)
            {
                TokenSource.PushToken(token); // put the brace { back.
                if (Stmt is DclMethodStatement)
                {
                    (Stmt as DclMethodStatement).HasBody = true;
                }
            }

            AnalyzeDclOptions(Stmt, false);

            return parsed;
        }
        private bool TryParseStructBody(Token Prior, IContainer Parent, ref DclTypeStatement Stmt, out string DiagMsg)
        {
            DiagMsg = string.Empty;

            TokenSource.VerifyExpectedToken(TokenType.BraceOpen, out var token);

            BlockStatement body = new(token.LineNumber, token.ColNumber);

            Stmt.AddBody(body);

            token = TokenSource.GetNextToken();

            while (token.TokenType != NoMoreTokens && token.TokenType != BraceClose)
            {
                switch (token.Keyword)
                {
                    case Type:
                        TokenSource.PushToken(token);
                        if (TryParseTypeDeclaration(token, Parent, out var typeStatement, out DiagMsg))
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

            return true;
        }
        private bool TryParseSingletBody(Token Prior, IContainer Parent, ref DclTypeStatement Stmt, out string DiagMsg)
        {
            DiagMsg = string.Empty;

            TokenSource.VerifyExpectedToken(TokenType.BraceOpen, out var token);

            BlockStatement body = new(token.LineNumber, token.ColNumber);

            Stmt.AddBody(body);

            token = TokenSource.GetNextToken();

            while (token.TokenType != NoMoreTokens && token.TokenType != BraceClose)
            {
                switch (token.Keyword)
                {
                    case Type:
                        TokenSource.PushToken(token);
                        if (TryParseTypeDeclaration(token, Parent, out var typeStatement, out DiagMsg))
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

            return true;
        }
        private bool TryParseInterfaceBody(Token Prior, IContainer Parent, ref DclTypeStatement Stmt, out string DiagMsg)
        {
            DiagMsg = string.Empty;

            TokenSource.VerifyExpectedToken(TokenType.BraceOpen, out var token);

            BlockStatement body = new(token.LineNumber, token.ColNumber);

            Stmt.AddBody(body);

            token = TokenSource.GetNextToken();

            while (token.TokenType != NoMoreTokens && token.TokenType != BraceClose)
            {
                switch (token.Keyword)
                {
                    case Type:
                        TokenSource.PushToken(token);
                        if (TryParseTypeDeclaration(token, Parent, out var typeStatement, out DiagMsg))
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

            return true;
        }
        private bool TryParseClassBody(Token Prior, IContainer Parent, ref DclTypeStatement Stmt, out string DiagMsg)
        {
            DiagMsg = string.Empty;

            TokenSource.VerifyExpectedToken(TokenType.BraceOpen, out var token);

            BlockStatement body = new(token.LineNumber, token.ColNumber);

            Stmt.AddBody(body);

            token = TokenSource.GetNextToken();

            while (token.TokenType != NoMoreTokens && token.TokenType != BraceClose)
            {
                switch (token.Keyword)
                {
                    case Type:
                        TokenSource.PushToken(token);
                        if (TryParseTypeDeclaration(token,  Parent, out var typeStatement, out DiagMsg))
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
                        OnDiagnostic(this, new DiagnosticEventArgs(Severity.Error, token.LineNumber, token.ColNumber, $"Unexpected token '{token.Lexeme}' found."));
                        break;
                }
            }

            return true;
        }
        private bool TryParseEnumBody(Token Prior, ref DclTypeStatement Stmt, out string DiagMsg)
        {
            DiagMsg = string.Empty;

            TokenSource.VerifyExpectedToken(TokenType.BraceOpen, out var token);

            BlockStatement body = new(token.LineNumber, token.ColNumber);

            Stmt.AddBody(body);

            token = TokenSource.GetNextToken();

            while (token.TokenType != NoMoreTokens && token.TokenType != BraceClose)
            {
                // The members of an enum are never other types or defintions, we must parse this differently

                if (token.TokenType != Identifier)
                {
                    OnDiagnostic(this, ParsedBad(Stmt, DiagMsg));
                    token = TokenSource.GetNextToken();
                    continue;
                }

                var name = token.Lexeme;

                token = TokenSource.GetNextToken();

                if (token.TokenType != Comma && token.TokenType != BraceClose)
                {
                    OnDiagnostic(this, ParsedBad(Stmt, DiagMsg));
                    token = TokenSource.GetNextToken();
                    continue;
                }

                // store the enum member as a child

                Stmt.Block.AddChild(new DclEnumMemberStatement(Prior.LineNumber, Prior.ColNumber, name, Stmt));

                if (token.TokenType == Comma)
                    token = TokenSource.GetNextToken();

                continue;
            }

            return true;

        }
        private bool TryParseTypeBody(Token Prior, IContainer Parent, ref DclTypeStatement Stmt, out string DiagMsg)
        {
            switch (Stmt.TypeKind)
            {
                case Enum:
                    {
                        return TryParseEnumBody(Prior, ref Stmt, out DiagMsg);
                    }
                case Class:
                    {
                        return TryParseClassBody(Prior, Parent, ref Stmt, out DiagMsg);
                    }
                case Struct:
                    {
                        return TryParseStructBody(Prior, Parent, ref Stmt, out DiagMsg);
                    }
                case Singlet:
                    {
                        return TryParseSingletBody(Prior, Parent, ref Stmt, out DiagMsg);
                    }
                case Interface:
                    {
                        return TryParseInterfaceBody(Prior, Parent, ref Stmt, out DiagMsg);
                    }
                default:
                    {
                        return TryParseClassBody(Prior, Parent, ref Stmt, out DiagMsg);
                    }
            }
        }
        private bool TryParseAccessorBlock(Token Prior, out BlockStatement Stmt, DclTypeStatement Parent, out string DiagMsg)
        {
            Stmt = new AccessibilityBlock(Prior.LineNumber, Prior.ColNumber, Accessibility.Unknown);

            DiagMsg = string.Empty;
            List<Keywords> terms = new List<Keywords>();

            var token = TokenSource.GetNextToken();

            if (token.Keyword == IsNotAKeyword)
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

            if (token.TokenType != BraceOpen)
            {
                if (token.Keyword == IsNotAKeyword)
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

            if (token.TokenType != BraceOpen)
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
        private bool TryParseDef(Token Prior, out DclStatement Stmt, DclTypeStatement Parent, out string DiagMsg)
        {
            Stmt = null;
            DiagMsg = string.Empty;
            bool flag;

            TokenSource.VerifyExpectedToken(Def, out var token);

            token = TokenSource.GetNextToken();

            if (token.TokenType != Identifier)
            {
                DiagMsg = $"Unexpected token {token.Lexeme}";
                return false;
            }
                
            TokenSource.PushToken(token);

            if (AppearsToBeA.MethodDeclaration(TokenSource))
            {
                if (TryParseMethodDeclaration(token, out var stmt, Parent, out DiagMsg))
                {
                    Stmt = stmt;
                    flag = TryParseMethodBody(token, stmt, out DiagMsg);
                    return flag;
                }
            }
            else
            {
                if (AppearsToBeA.FieldDeclaration(TokenSource))
                {
                    flag = TryParseFieldDeclaration(token, out var stmt, Parent, out DiagMsg);

                    if (flag)
                    {
                        Stmt = stmt;
                    }

                    return flag;
                }
            }

            return false;

        }
        private bool TryParseTypeDeclaration(Token Prior, IContainer Parent, out DclTypeStatement Stmt, out string DiagMsg)
        {
            Stmt = null;

            TokenSource.VerifyExpectedToken(Type, out var token);

            if (TryParsePossiblyGenericName(Prior, out var genericName, out DiagMsg) == false)
                return false;

            Stmt = new DclTypeStatement(Parent, Prior.LineNumber, Prior.ColNumber, genericName);

            if (TryParseDclOptions(token, Stmt, out DiagMsg))
                return TryParseTypeBody(token, Parent, ref Stmt, out DiagMsg);
            else
                TokenSource.SkipToNext("}");

            return false;
        }
        private bool TryParseMethodDeclaration(Token Prior, out DclMethodStatement Stmt, DclTypeStatement Parent, out string DiagMsg)
        {
            Token token;

            DclMethodStatement methodDef = null;

            Stmt = null;
            DiagMsg = string.Empty;

            if (!AppearsToBeA.MethodDeclaration(TokenSource))
            {
                DiagMsg = "Invalid parser method called.";
                return false;
            }

            if (TryParsePossiblyGenericName(Prior, out var genericName, out DiagMsg) == false)
                return false;

            methodDef = new DclMethodStatement(Prior.LineNumber, Prior.ColNumber, genericName, Parent);

            token = TokenSource.PeekNextToken();

            // TODO This code must expect parameter typenames to be generic names sometimes.

            if (token.TokenType == SemiColon || token.TokenType == BraceOpen)
            {
                if (token.TokenType == SemiColon)
                    methodDef.HasBody = false;
                else
                    methodDef.HasBody = true;

                return true;
            }

            if (TokenSource.NextTokensAre(ParenOpen, Identifier, Identifier)) 
            {
                TryParseParameterList(Prior, ref methodDef, out DiagMsg);
            }

            if (TokenSource.NextTokensAre(ParenOpen, Identifier, ParenClose) || TokenSource.NextTokensAre(ParenOpen, Identifier, Lesser))
            {
                TokenSource.GetNextToken();

                if (TryParsePossiblyGenericName(Prior, out var returnType, out DiagMsg))
                {
                    methodDef.Returns = returnType;
                    TokenSource.GetNextToken(); // Consume the closing rpar
                }
            }

            TryParseDclOptions(Prior, methodDef, out DiagMsg);

            Stmt = methodDef;

            return true;

        }
        private bool TryParseFieldDeclaration(Token Prior, out DclFieldStatement Stmt, DclTypeStatement Parent, out string DiagMsg)
        {
            Stmt = null;
            DiagMsg = string.Empty;

            var token = TokenSource.GetNextToken();

            var field = new DclFieldStatement(Prior.LineNumber, Prior.ColNumber, token.Lexeme, Parent.DeclaredName, Parent);

            token = TokenSource.GetNextToken();

            while (token.TokenType != SemiColon)
            {
                if (token.Keyword != IsNotAKeyword)
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
        private bool TryParseParameterList(Token Prior, ref DclMethodStatement Stmt, out string DiagMsg)
        {
            DiagMsg = string.Empty;

            Token token;

            token = TokenSource.GetNextToken();

            if (token.TokenType != ParenOpen)
            {
                DiagMsg = $"Unexpected token {token.Lexeme}";
                return false;
            }

            token = TokenSource.GetNextToken();

            while (token.TokenType != ParenClose)
            {
                if (token.TokenType != Identifier)
                {
                    DiagMsg = $"Unexpected token in parameter declaration '{token.Lexeme}'";
                    continue;
                }

                var pname = token.Lexeme;

                var ok = TryParsePossiblyGenericName(Prior, out var parameterType, out DiagMsg);

                if (ok == false)
                {
                    DiagMsg = $"Unexpected token in parameter declaration '{token.Lexeme}'";
                    continue;
                }

                token = TokenSource.GetNextToken();

                switch (token.Keyword)
                {
                    case Ref:
                        {
                            Stmt.AddParameter(new Parameter(pname, parameterType, PassBy.Ref));
                            break;
                        }
                    case Out:
                        {
                            Stmt.AddParameter(new Parameter(pname, parameterType, PassBy.Out));
                            break;
                        }

                    default:
                        {
                            TokenSource.PushToken(token);
                            Stmt.AddParameter(new Parameter(pname, parameterType, PassBy.Value));
                            break;
                        }
                }

                // if next token is a comma, go around again..

                token = TokenSource.GetNextToken();

                if (token.TokenType == Comma)
                {
                    token = TokenSource.GetNextToken();
                    continue;
                }
            }

            return true;

        }
        private bool TryParseMethodBody(Token Prior, DclMethodStatement Stmt, out string DiagMsg)
        {
            var method = (DclMethodStatement)(Stmt);

            DiagMsg = string.Empty;

            if (method.HasBody == false)
                return true;

            var token = TokenSource.GetNextToken();

            if (token.TokenType != BraceOpen)
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
            string containerName = "";

            if (Stmt is DclTypeStatement)
            {
                var stmt = (DclTypeStatement)(Stmt);

                if (stmt.TypeKind != IsNotAKeyword)
                    containerName = stmt.TypeKind.ToString();
                else
                    containerName = Stmt.ShortStatementTypeName;
            }
            else
            {
                containerName = Stmt.ShortStatementTypeName;
            }

            var dupeDeclarations = Stmt.Block.Children.
                Where(c => c is DclStatement).
                Cast<DclStatement>().
                GroupBy(dcl => dcl.DecoratedName).
                Where(dcl => dcl.Count() > 1);

            foreach (var declaration in dupeDeclarations)
            {
                var firstuse = declaration.OrderBy(ns => ns.Line).Take(1).First();  // the original, legally declared instance of the name.
                var duplicates = declaration.OrderBy(ns => ns.Line).Skip(1); // all the subsequent illegal declarations of the same name.

                foreach (var duplicate in duplicates)
                {
                    if (duplicate.ShortStatementTypeName == "method")
                        OnDiagnostic(this, new DiagnosticEventArgs(Severity.Error, duplicate.Line, duplicate.Col, $"Invalid {duplicate.ShortStatementTypeName} name '{duplicate.LiteralDecoratedName}' within {containerName} '{Stmt.DeclaredName}', there is already a defintion within this scope, of a {firstuse.ShortStatementTypeName} with this name and signature at line {firstuse.Line}."));
                    else
                        OnDiagnostic(this, new DiagnosticEventArgs(Severity.Error, duplicate.Line, duplicate.Col, $"Invalid {duplicate.ShortStatementTypeName} name '{duplicate.LiteralDecoratedName}' within {containerName} '{Stmt.DeclaredName}', there is already a defintion within this scope, of a {firstuse.ShortStatementTypeName} with this name at line {firstuse.Line}."));
                }
            }
        }
    }
}