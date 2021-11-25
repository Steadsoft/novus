using Steadsoft.Novus.Scanner.Classes;
using Steadsoft.Novus.Scanner.Enums;
using System.IO;
using System.Reflection;
using static Steadsoft.Novus.Scanner.Enums.TokenType;
using static Steadsoft.Novus.Scanner.Enums.Keywords;

namespace Steadsoft.Novus.EnhancedParser.Classes
{
    public sealed partial class Parser
    {
        public static object Empty = new object();

        public static bool failed = true;
        public static bool worked = false;

        private Tokenizer<Keywords> tokenizer;
        private TokenEnumerator source;
        public string SourceName { get; private set; }

        private Parser(TokenEnumerator Source, string SourceName, Tokenizer<Keywords> Tokenizer)
        {
            source = Source;
            tokenizer = Tokenizer;
            this.SourceName = SourceName;
        }

        public static Parser CreateParser(SourceOrigin SourceText, string Input, TokenDefinition Definition, string Tokens)
        {
            SourceFile sourceFile;
            string sourceName;
            if (SourceText == SourceOrigin.Pathname)
            {
                sourceFile = SourceFile.CreateFromFile(Input);
            }
            else
            {
                sourceFile = SourceFile.CreateFromText(Input);
            }

            if (SourceText == SourceOrigin.Pathname)
                sourceName = Path.GetFullPath(Input);
            else
                sourceName = "Raw Text";

            var tokenizer = new Tokenizer<Keywords>(Tokens, Definition, Assembly.GetExecutingAssembly());
            var source = new TokenEnumerator(tokenizer, sourceFile, BlockComment, LineComment);
            var parser = new Parser(source, sourceName, tokenizer);

            return parser;
        }

        public bool TryParseCompilationUnit(out compilation_unit compilation_unit)
        {
            compilation_unit = new compilation_unit(); // this represents the global namespace
            bool parsed = true;
            Token t;

            // we are ignoring attributes at this time.

            t = source.PeekNextToken();

            // first parse any using statements

            while (t.Keyword == Using)
            {
                if (failed_to_parse_using_directive(t, out var using_directive))
                {
                    parsed = false;
                    continue;
                }

                compilation_unit.namespace_body.Add(using_directive);

                t = source.PeekNextToken();
            }

            // next parse any namespace or type declarations

            while (t.Keyword == Type || t.Keyword == Namespace)
            {
                switch (t.Keyword)
                {
                    case Type:
                        {
                            if (failed_to_parse_type_declaration(t, out var type_declaration))
                            {
                                parsed = false;
                                continue;
                            }
                            compilation_unit.namespace_body.Add(type_declaration);
                            break;
                        }
                    case Namespace:
                        {
                            if (failed_to_parse_namespace_declaration(t, out var namespace_declaration))
                            {
                                parsed = false;
                                continue;
                            }
                            compilation_unit.namespace_body.Add(namespace_declaration);
                            break;
                        }
                }

                t = source.PeekNextToken();
            }

            return parsed;
        }
    }
}