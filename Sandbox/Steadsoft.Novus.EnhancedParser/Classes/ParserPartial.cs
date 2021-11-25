using Steadsoft.Novus.Scanner.Classes;
using Steadsoft.Novus.Scanner.Enums;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Steadsoft.Novus.Scanner.Enums.TokenType;
using static Steadsoft.Novus.Scanner.Enums.Keywords;

namespace Steadsoft.Novus.EnhancedParser.Classes
{
    public sealed partial class Parser
    {
        #region Parsing methods

        /* All parsing methods accept a token which is used when we eventually create a concrete class 
         * instance for some parsed entity. The token tells us the line number etc where the parsed 
         * code element began.
         */

        private bool failed_to_parse_using_directive(Token Token, out using_directive? using_directive)
        {
            using_directive = null;
            return worked;
        }

        private bool failed_to_parse_namespace_declaration(Token Token, out namespace_declaration? namespace_declaration)
        {
            namespace_declaration = new namespace_declaration(Token);
            return worked;
        }

        private bool failed_to_parse_type_declaration(Token Token, out type_declaration? type_declaration)
        {
            type_declaration = null;
            return worked;
        }
        #endregion
    }
}
