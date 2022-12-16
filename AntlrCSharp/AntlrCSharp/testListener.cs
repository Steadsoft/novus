using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntlrCSharp
{
    public class TestListener : noresBaseListener
    {
        public override void EnterBasic_reference([NotNull] noresParser.Basic_referenceContext context)
        {
            base.EnterBasic_reference(context);
        }

        public override void EnterPreprocessor_stmt([NotNull] noresParser.Preprocessor_stmtContext context)
        {
            base.EnterPreprocessor_stmt(context);
        }
    }
}
