using Steadsoft.Novus.Scanner.Classes;

namespace Steadsoft.Novus.EnhancedParser.Classes
{
    public sealed partial class Parser
    {
        #region Parsing methods

        /* All parsing methods accept a token which is used when we eventually create a concrete class 
         * instance for some parsed entity. The token tells us the line number etc where the parsed 
         * code element began.
         */

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Token">The token that represents the start of this construct.</param>
        /// <param name="using_directive">A populated using directive or null.</param>
        /// <returns></returns>
        private bool failed_to_parse_using_directive(Token Token, out using_directive? using_directive)
        {
            using_directive = null;
            return worked;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Token">The token that represents the start of this construct.</param>
        /// <param name="container">The namespace or type that contains the parsed construct.</param>
        /// <param name="namespace_declaration">A populated namespace declaration or null.</param>
        /// <returns></returns>
        private bool failed_to_parse_namespace_declaration(Token Token, namespace_member_declaration container, out namespace_declaration? namespace_declaration)
        {
            namespace_declaration = new namespace_declaration(Token);
            return worked;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Token">The token that represents the start of this construct.</param>
        /// <param name="container">The namespace or type that contains the parsed construct.</param>
        /// <param name="namespace_declaration">A populated type declaration or null.</param>
        /// <returns></returns>

        private bool failed_to_parse_type_declaration(Token Token, namespace_member_declaration container, out type_declaration? type_declaration)
        {
            type_declaration = null;
            return worked;
        }
        #endregion
    }
}
