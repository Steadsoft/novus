using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steadsoft.Novus.Parser
{
    /// <summary>
    /// Represents any bracketed code block surrounded by { and }
    /// </summary>
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

        public override string Dump(int nesting)
        {
            StringBuilder builder = new StringBuilder();

            foreach (var child in Children)
            {
                builder.Append(child.Dump(nesting+1));
            }

            return builder.ToString();
        }
    }
}
