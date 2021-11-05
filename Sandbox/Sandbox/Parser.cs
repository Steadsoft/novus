using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox
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
        private TokenEnumerator source;
        public event DiagnosticEventHandler OnDiagnostic;

        public Parser (TokenEnumerator Source)
        {
            source = Source;
        }
        public bool TryParseFile(TokenEnumerator source, out BlockStatement Root)
        {
            int errors = 0;

            Root = null;

            string message;

            var token = source.GetNextToken();

            // We expect any of the following

            // using <qualified-name> ;
            // namespace <qualified-name> { }
            // one or more of: type <identifier>  [<type-options>] { <type-body> }

            while (token.TokenCode != TokenType.NoMoreTokens)
            {
                if (Root == null)
                {
                    Root = new BlockStatement(token.LineNumber, token.ColNumber);
                }

                switch (token.Keyword)
                {
                    case Keyword.Using:
                        source.PushToken(token);
;                       if (TryParseUsing(source, token, out var usingStatement, out message))
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

                    case Keyword.Namespace:
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

                    case Keyword.Type:
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
        public bool TryParseUsing(TokenEnumerator source, Token Prior, out UsingStatement Stmt, out string DiagMsg)
        {
            Stmt = null;
            DiagMsg = null;

            StringBuilder builder = new StringBuilder();

            // <ident>;
            // <ident>.
            // <ident>.<ident>;
            // <ident>.<ident>.
            // <ident>.<ident>.<ident>;

            var token = source.GetNextToken();

            if (token.Keyword != Keyword.Using)
                throw new InvalidOperationException($"Expected keyword token '{Keyword.Using}' has not been pushed.");

            token = source.GetNextToken();

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

                if (token.Lexeme == ";")
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

            return false;
        }
        public bool TryParseNamespace(TokenEnumerator source, Token Prior, out NamespaceStatement Stmt, out string DiagMsg)
        {
            Stmt = null;
            DiagMsg = String.Empty;

            StringBuilder builder = new StringBuilder();

            // <ident>; or <ident>{
            // <ident>.
            // <ident>.<ident>; or <ident>.<ident>{
            // <ident>.<ident>.
            // <ident>.<ident>.<ident>; or <ident>.<ident>.<ident>{

            var token = source.GetNextToken();

            if (token.Keyword != Keyword.Namespace)
                throw new InvalidOperationException($"Expected keyword token '{Keyword.Namespace}'has not been pushed.");

            token = source.GetNextToken();

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

                if (token.Lexeme == ";")
                {
                    Stmt = new NamespaceStatement(Prior.LineNumber, Prior.ColNumber, builder.ToString());
                    return true;
                }

                if (token.TokenCode == TokenType.LBrace)
                {
                    source.PushToken(token);
                    if (TryParseNamespaceBlock(source, out var block, out DiagMsg))
                    {
                        Stmt = new NamespaceStatement(Prior.LineNumber, Prior.ColNumber, builder.ToString());
                        Stmt.AddBlock(block);
                    }

                    source.SkipToNext("}"); // very crude hack, just for now, to get progress through the file
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
        public bool TryParseNamespaceBlock(TokenEnumerator source, out BlockStatement Block, out string DiagMsg)
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
                    case Keyword.Namespace:
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

                    case Keyword.Type:
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
                        OnDiagnostic(this, new DiagnosticEventArgs("Unexpected token {} found."));
                        break;
                }
            }

            return true;

        }
        public bool TryParseRecord(TokenEnumerator source, ref TypeStatement Stmt, out string DiagMsg)
        {
            DiagMsg = String.Empty;

            var token = source.GetNextToken();

            while (token.Lexeme != "{")
            {
                switch (token.Keyword)
                {
                    case Keyword.Class:
                        {
                            Stmt.IsRecordClass = true;
                            break;
                        }

                    case Keyword.Struct:
                        {
                            Stmt.IsRecordStruct = true;
                            break;
                        }
                    case Keyword.Readonly:
                        {
                            Stmt.IsReadOnly = true;
                            break;
                        }
                    case Keyword.Private when Stmt.AccessType == AccessType.Undefined:
                        {
                            Stmt.AccessType = AccessType.Private;
                            break;
                        }
                    case Keyword.Internal when Stmt.AccessType == AccessType.Undefined:
                        {
                            Stmt.AccessType = AccessType.Internal;
                            break;
                        }
                    case Keyword.Public when Stmt.AccessType == AccessType.Undefined:
                        {
                            Stmt.AccessType = AccessType.Public;
                            break;
                        }
                    case Keyword.Private when Stmt.AccessType != AccessType.Undefined:
                    case Keyword.Internal when Stmt.AccessType != AccessType.Undefined:
                    case Keyword.Public when Stmt.AccessType != AccessType.Undefined:
                        {
                            DiagMsg = $"Cannot specify access type '{token.Keyword.ToString().ToLower()}' when access is already '{Stmt.AccessType.ToString().ToLower()}'";
                            return false;
                        }
                    default:
                        {
                            DiagMsg = $"Unexpected token '{token.Lexeme}'";
                            return false;
                        }
                }

                token = source.GetNextToken();
            }

            source.PushToken(token);

            if (Stmt.IsRecordClass && Stmt.IsRecordStruct)
                return false;

            if (Stmt.AccessType == AccessType.Undefined)
                Stmt.AccessType = AccessType.Internal;

            return true;
        }
        public bool TryParseType(TokenEnumerator source, Token Prior, out TypeStatement Stmt, out string DiagMsg)
        {
            Stmt = null;
            DiagMsg = string.Empty;

            string name;
            Keyword typeKind = Keyword.IsNotKeyword;

            // We expect any of the following

            // type <identifier> [<type-options>] { }


            // <ident>; or <ident>{
            // <ident>.
            // <ident>.<ident>; or <ident>.<ident>{
            // <ident>.<ident>.
            // <ident>.<ident>.<ident>; or <ident>.<ident>.<ident>{

            var token = source.GetNextToken();

            if (token.Keyword != Keyword.Type)
                throw new InvalidOperationException($"Expected keyword token '{Keyword.Type}' has not been pushed.");

            token = source.GetNextToken();

            while (token.TokenCode != TokenType.NoMoreTokens)
            {
                if (token.TokenCode != TokenType.Identifier)
                {
                    DiagMsg = $"Unexpected token {token.Lexeme}";
                    source.PushToken(token);
                    return false;
                }

                name = token.Lexeme;

                token = source.GetNextToken();

                if (token.Keyword != Keyword.Class && token.Keyword != Keyword.Struct && token.Keyword != Keyword.Record && token.Keyword != Keyword.Singlet)
                {
                    DiagMsg = $"Unexpected token {token.Lexeme}";
                    source.PushToken(token);
                    return false;
                }

                typeKind = token.Keyword;

                Stmt = new TypeStatement(new TypeBody(), typeKind, Prior.LineNumber, Prior.ColNumber, name);

                switch (typeKind)
                {
                    case Keyword.Class:
                        {
                            //TryParseClass();
                            break; // parse class options
                        }
                    case Keyword.Struct:
                        {
                            break; // parse struct options
                        }
                    case Keyword.Record:
                        {
                            if (TryParseRecord(source, ref Stmt, out DiagMsg) == false)
                            {
                                //source.SkipToNext("}");
                                return false;
                            }
                            break; // parse record options
                        }
                    case Keyword.Singlet:
                        {
                            break; // parse singlet options
                        }
                }

                token = source.GetNextToken();

                if (token.Lexeme == "{")
                {
                    // TryParseBlock
                    source.SkipToNext("}"); // very crude hack, just for now, to get progress through the file
                    return true;
                }


                token = source.GetNextToken();

            }

            return false;

        }
        public static DiagnosticEventArgs ParsedGood(Statement Stmt)
        {
            return new DiagnosticEventArgs($"Parsed a {Stmt.GetType().Name} on line {Stmt.Line} at column {Stmt.Col} : '{Stmt.ToString()}'");
        }

        public static DiagnosticEventArgs ParsedBad(Statement Stmt, string Msg)
        {
            return new DiagnosticEventArgs($" Failed to parse a {Stmt.GetType().Name} on line {Stmt.Line} at column {Stmt.Col} ({Msg})");
        }
    }
}
