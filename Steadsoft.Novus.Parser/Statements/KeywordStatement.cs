using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steadsoft.Novus.Parser.Statements
{
    public class KeywordStatement : ComputeStatement
    {
        public KeywordStatement(int Line, int Col) : base(Line, Col)
        {
        }
    }
}
