using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steadsoft.Novus.Parser.Statements
{
    public class AssignmentStatement : ComputeStatement
    {
        public object Target { get; set; }
        public object Expression { get; set; }
        public AssignmentStatement(int Line, int Col) : base(Line, Col)
        {
        }
    }
}
