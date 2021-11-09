using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steadsoft.Novus.Parser
{
    public class NamespaceStatement : Statement
    {
        public string Name { get; private set; }
        public BlockStatement Block {get; private set;}

        public NamespaceStatement(int Line, int Col, string Name):base(Line, Col)
        {
            this.Name = Name;
        }

        public void AddBlock (BlockStatement Stmt)
        {
            if (Stmt == null) throw new ArgumentNullException(nameof(Stmt));

            Block = Stmt;
            Block.Parent = this;
        }

        public override string ToString()
        {
            return Name;
        }

        public override string Dump(int nesting)
        {
            StringBuilder builder = new();

            builder.AppendLine($"{Prepad(nesting)}Namespace: [{Name}]");
            
            if (Block != null)
               builder.Append(Block.Dump(nesting));

            return builder.ToString();
        }
    }
}
