using Steadsoft.Novus.Scanner.Classes;
using static Steadsoft.Novus.Scanner.Enums.TokenType;
using static Steadsoft.Novus.Scanner.Enums.Keywords;
using Steadsoft.Novus.Scanner.Enums;

namespace Steadsoft.Novus.EnhancedParser.Classes
{
    public sealed partial class Parser
    {
        #region Parsing methods

        /* All parsing methods accept and propagate a token which is used when we eventually create a 
         * concrete class instance for some parsed entity. The token tells us the line number etc where 
         * the parsed construct began.
         */

        /// <summary>
        /// https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/namespaces#using-directives
        /// </summary>
        /// <param name="Token">The token that represents the start of this construct.</param>
        /// <param name="using_directive">A populated using directive or null.</param>
        /// <returns></returns>
        private bool failed_to_parse_using_directive(Token Token, out using_directive using_directive)
        {
            using_directive = default;

            if (source.PeekNextToken().Keyword != Using)
            {
                throw new InternalErrorException($"The expected token {Using} was not present.");
            }

            source.DiscardNextToken(); // discard the 'using'

            // alias: using identifier = namespace_or_type_name  ;

            if (source.NextTokensAre(Identifier, Equal, Identifier))
            {
                string ident = source.GetNextToken().Lexeme;

                source.DiscardNextToken(); // discard the '=' token

                // we next expect a potentially qialified and parameterized name.
                // using S = System is valid as is: using S = System.Stuff.Classname<int,string> ;

            }

            // using namespsace: using name ;

            // using static: using static type name ;

            return worked;
        }

        /// <summary>
        /// https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/namespaces#namespace-declarations
        /// </summary>
        /// <param name="Token">The token that represents the start of this construct.</param>
        /// <param name="container">The namespace or type that contains the parsed construct.</param>
        /// <param name="namespace_declaration">A populated namespace declaration or null.</param>
        /// <returns></returns>
        private bool failed_to_parse_namespace_declaration(Token Token, namespace_member_declaration container, out namespace_declaration namespace_declaration)
        {
            namespace_declaration = new namespace_declaration(Token);
            return worked;
        }

        /// <summary>
        /// https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/namespaces#type-declarations
        /// </summary>
        /// <param name="Token">The token that represents the start of this construct.</param>
        /// <param name="container">The namespace or type that contains the parsed construct.</param>
        /// <param name="namespace_declaration">A populated type declaration or null.</param>
        /// <returns></returns>
        private bool failed_to_parse_type_declaration(Token Token, namespace_member_declaration container, out type_declaration type_declaration)
        {
            type_declaration = default;
            return worked;
        }

        private bool failed_to_parse_qualified_typename(Token Token, out qualified_typename qualified_typename)
        {
            qualified_typename = default;

            while (true)
            {
                if (failed_to_parse_identifier_type_argument_list(Token, out var identifier_type_argument_list))
                {
                    return failed;
                }

                if (qualified_typename == default)
                    qualified_typename = new qualified_typename();

                qualified_typename.fullname.Add(identifier_type_argument_list);
            }

            return worked;
        }

        private bool failed_to_parse_identifier_type_argument_list(Token Token, out identifier_type_argument_list identifier_type_argument_list)
        {
            identifier_type_argument_list = default;

            if (source.PeekNextToken().TokenType != Identifier)
            {
                throw new InternalErrorException($"The expected token {Identifier} was not present.");
            }

            var ident = source.GetNextToken().Lexeme;

            identifier_type_argument_list = new identifier_type_argument_list() { identifier = ident };

            if (source.PeekNextToken().TokenType == Period)
            {
                return worked;
            }

            if (source.PeekNextToken().TokenType == Lesser)
            {

            }


            return worked;
        }

        private bool TryParseGenericArgList(Token Prior, GenericArgList Args, out string DiagMsg)
        {
            // TODO Parsing comma lists can be generified by having a generic loop that handles and consumes the comma and calls a supplied Func to do the parsing specific to the kind of items in the commalist.

            DiagMsg = null;

            source.VerifyExpectedToken(Lesser, out var token);

            try
            {
                // The pair of chars >> represent a right shift but also appear at the end
                // of a generic argument list sometimes, in the latter case we want these
                // to be seen as distinct > chars. The tokenizer allows this by letting us
                // temporarily set a hint during parsing, this is present only briefly as
                // we part certain constructs. This is signficant benefit because it removes the
                // need for the parser to understand token structure and removes the fiddly 
                // logic from the parsing code.

                source.PushHint(ParsingHint.SplitRightShift);

                while (token.TokenType != Greater)
                {
                    token = source.GetNextToken();

                    if (token.TokenType != Identifier)
                        return false;

                    var genericArg = new GenericArg(token.Lexeme);

                    Args.Add(genericArg);

                    token = source.GetNextToken();

                    switch (token.TokenType)
                    {
                        case Comma:
                            {
                                continue;
                            }

                        case Lesser:
                            {
                                source.PushToken(token);
                                if (TryParseGenericArgList(Prior, genericArg.GenericArgs, out DiagMsg) == false)
                                    return false;
                                token = source.GetNextToken();
                                continue;
                            }
                        case Greater:
                            {
                                return true;
                            }
                    }

                }
            }

            finally
            {
                source.PopHint();
            }

            return true;
        }

        #endregion
    }
}
