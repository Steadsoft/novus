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

        public BlockStatement(int Line, int Col) : base(Line, Col)
        {
            Children = new List<Statement>();
        }

        public void AddChild(Statement Stmt)
        {
            Children.Add(Stmt);
        }
    }
}
