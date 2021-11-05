using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox
{
    public class BlockStatement : Statement
    {
        public Statement Parent;
        public List<Statement> Children;

        BlockStatement(int Line, int Col) : base(Line, Col)
        {

        }

        public void AddChild(Statement Stmt)
        {
            Children.Add(Stmt);
        }
    }
}
