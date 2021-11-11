using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steadsoft.Novus.Parser.Statements
{
    public class ComputeStatement : Statement
    {
        public ComputeStatement(int Line, int Col) : base(Line, Col)
        {

        }

        public override string Dump(int nesting)
        {
            throw new NotImplementedException();
        }
    }
}
